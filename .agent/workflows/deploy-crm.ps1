# deploy-crm.ps1
# Script para deployar el CRM Nuxt SPA a IIS


param(
    [string]$RepoPath = "C:\actions-runner\_work\homes\homes",
    [string]$WebsiteName = "HomesCRM",
    [string]$WebsitePath = "C:\inetpub\wwwroot\homes-crm"
)

# Configurar log
$LogFile = "C:\DeployScripts\logs\deploy-crm-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"
New-Item -ItemType Directory -Force -Path "C:\DeployScripts\logs" | Out-Null

function Write-Log {
    param([string]$Message)
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    "$timestamp - $Message" | Tee-Object -FilePath $LogFile -Append
}

try {
    Write-Log "=== Starting CRM Deployment ==="
    
    # 1. Navegar al directorio del CRM
    Write-Log "Navigating to CRM directory"
    Set-Location "$RepoPath\crm"
    
    # 2. Verificar que existen los archivos necesarios
    if (-not (Test-Path "package.json")) {
        throw "package.json not found in $PWD"
    }
    
    # 3. Instalar dependencias
    Write-Log "Installing npm dependencies"
    npm ci --prefer-offline --no-audit
    
    # 4. Build de produccion (genera SPA en .output/public)
    Write-Log "Building Nuxt SPA"
    $env:NODE_ENV = "production"
    
    # Ejecutar build y capturar solo errores reales
    $ErrorActionPreference = "Continue"
    npm run generate *>&1 | Out-Null
    $buildExitCode = $LASTEXITCODE
    $ErrorActionPreference = "Stop"
    
    if ($buildExitCode -ne 0) {
        throw "Build failed with exit code $buildExitCode"
    }
    
    Write-Log "Build completed successfully"
    
    # Verificar que el build fue exitoso
    if (-not (Test-Path ".output\public\index.html")) {
        throw "Build failed - index.html not found in .output\public"
    }
    
    # 5. Detener el sitio en IIS
    Write-Log "Stopping IIS website: $WebsiteName"
    Import-Module WebAdministration
    
    # Verificar si el sitio existe
    if (Get-Website -Name $WebsiteName -ErrorAction SilentlyContinue) {
        Stop-Website -Name $WebsiteName
        Start-Sleep -Seconds 2
    }
    else {
        Write-Log "WARNING: Website $WebsiteName does not exist in IIS. Will create directory only."
    }
    
    # 6. Crear backup del deployment anterior
    if (Test-Path $WebsitePath) {
        $BackupPath = "$WebsitePath-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Write-Log "Creating backup: $BackupPath"
        Copy-Item -Path $WebsitePath -Destination $BackupPath -Recurse -Force
        
        # Limpiar backups antiguos (mantener solo los ultimos 3)
        Get-ChildItem -Path (Split-Path $WebsitePath) -Filter "$(Split-Path $WebsitePath -Leaf)-backup-*" | 
        Sort-Object CreationTime -Descending | 
        Select-Object -Skip 3 | 
        Remove-Item -Recurse -Force
    }
    
    # 7. Crear directorio si no existe
    if (-not (Test-Path $WebsitePath)) {
        Write-Log "Creating website directory: $WebsitePath"
        New-Item -ItemType Directory -Path $WebsitePath -Force | Out-Null
    }
    
    # 8. Limpiar directorio actual
    Write-Log "Cleaning website directory"
    Get-ChildItem -Path $WebsitePath -Recurse | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    
    # 9. Copiar archivos del build
    Write-Log "Copying build files to website directory"
    $BuildPath = "$RepoPath\crm\.output\public"
    Copy-Item -Path "$BuildPath\*" -Destination $WebsitePath -Recurse -Force
    
    # 10. Crear archivo de version
    Write-Log "Creating version file"
    
    $gitCommit = try { git -C $RepoPath rev-parse --short HEAD 2>$null } catch { "unknown" }
    $gitBranch = try { git -C $RepoPath rev-parse --abbrev-ref HEAD 2>$null } catch { "unknown" }
    
    $versionInfo = @{
        version    = Get-Date -Format "yyyy.MM.dd.HHmm"
        deployDate = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        commit     = if ($gitCommit) { $gitCommit } else { "unknown" }
        branch     = if ($gitBranch) { $gitBranch } else { "unknown" }
    }
    $versionInfo | ConvertTo-Json | Out-File -FilePath "$WebsitePath\version.json" -Encoding UTF8 -Force
    Write-Log "Version: $($versionInfo.version)"
    
    # 10. Crear web.config para SPA (importante para Nuxt SPA)
    Write-Log "Creating web.config for SPA routing"
    $webConfig = @"
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <!-- Rewrite rules para SPA -->
    <rewrite>
      <rules>
        <rule name="SPA Fallback" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
            <add input="{REQUEST_URI}" pattern="^/api/" negate="true" />
          </conditions>
          <action type="Rewrite" url="/200.html" />
        </rule>
      </rules>
    </rewrite>
    
    <!-- Deshabilitar validacion de configuracion que puede causar 500 -->
    <validation validateIntegratedModeConfiguration="false" />
    
    <!-- Compresion estatica -->
    <urlCompression doStaticCompression="true" doDynamicCompression="false" />
  </system.webServer>
</configuration>
"@
    $webConfig | Out-File -FilePath "$WebsitePath\web.config" -Encoding UTF8 -Force
    
    # 11. Reiniciar el sitio en IIS
    if (Get-Website -Name $WebsiteName -ErrorAction SilentlyContinue) {
        Write-Log "Starting IIS website: $WebsiteName"
        Start-Website -Name $WebsiteName
        
        # 12. Reciclar el Application Pool
        Write-Log "Recycling application pool"
        $appPool = (Get-Website -Name $WebsiteName).applicationPool
        Restart-WebAppPool -Name $appPool
    }
    
    # 13. Verificar deployment
    Write-Log "Verifying deployment"
    if (Test-Path "$WebsitePath\index.html") {
        Write-Log "OK - index.html found"
    }
    if (Test-Path "$WebsitePath\_nuxt") {
        Write-Log "OK - _nuxt assets folder found"
    }
    
    Write-Log "=== Deployment completed successfully ==="
    Write-Log "Deployed files from: $BuildPath"
    Write-Log "To: $WebsitePath"
    
    exit 0
    
}
catch {
    Write-Log "ERROR: $($_.Exception.Message)"
    Write-Log "Stack Trace: $($_.ScriptStackTrace)"
    
    # Intentar reiniciar el sitio en caso de error
    try {
        if (Get-Website -Name $WebsiteName -ErrorAction SilentlyContinue) {
            Start-Website -Name $WebsiteName
        }
    }
    catch {
        Write-Log "Failed to restart website after error"
    }
    
    Write-Log "=== Deployment failed ==="
    exit 1
}
