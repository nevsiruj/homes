# deploy-production.ps1
# Script para deployar CRM y API a producción (app-pool)

param(
    [string]$RepoPath = "C:\repos\others\homes",
    [switch]$DeployCRM,
    [switch]$DeployAPI,
    [switch]$All
)

# Configurar log
$LogFile = "C:\DeployScripts\logs\deploy-production-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"
New-Item -ItemType Directory -Force -Path "C:\DeployScripts\logs" | Out-Null

function Write-Log {
    param([string]$Message)
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    "$timestamp - $Message" | Tee-Object -FilePath $LogFile -Append
}

# Si se especifica -All, activar todos los deploys
if ($All) {
    $DeployCRM = $true
    $DeployAPI = $true
}

# Verificar que al menos un componente esté seleccionado
if (-not ($DeployCRM -or $DeployAPI)) {
    Write-Log "ERROR: Debe especificar al menos un componente para deployar (-DeployCRM, -DeployAPI, o -All)"
    exit 1
}

try {
    Write-Log "=== Starting Production Deployment (app-pool) ==="
    
    # ========================================
    # 1. DEPLOY CRM
    # ========================================
    if ($DeployCRM) {
        Write-Log "========================================="
        Write-Log "Deploying CRM to \\vm06\shared\app-pool\homes\crm"
        Write-Log "========================================="
        
        $crmPath = "\\vm06\shared\app-pool\homes\crm"
        
        # Navegar al directorio del CRM
        Write-Log "Navigating to CRM directory"
        Set-Location "$RepoPath\crm"
        
        # Verificar que existen los archivos necesarios
        if (-not (Test-Path "package.json")) {
            throw "package.json not found in $PWD"
        }
        
        # Instalar dependencias
        Write-Log "Installing npm dependencies"
        npm ci --prefer-offline --no-audit
        
        # Build de produccion
        Write-Log "Building Nuxt SPA"
        $env:NODE_ENV = "production"
        
        $ErrorActionPreference = "Continue"
        npm run generate *>&1 | Out-Null
        $buildExitCode = $LASTEXITCODE
        $ErrorActionPreference = "Stop"
        
        if ($buildExitCode -ne 0) {
            throw "CRM Build failed with exit code $buildExitCode"
        }
        
        Write-Log "CRM Build completed successfully"
        
        # Verificar que el build fue exitoso
        if (-not (Test-Path ".output\public\index.html")) {
            throw "CRM Build failed - index.html not found in .output\public"
        }
        
        # Crear backup
        if (Test-Path $crmPath) {
            $BackupPath = "$crmPath-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
            Write-Log "Creating CRM backup: $BackupPath"
            Copy-Item -Path $crmPath -Destination $BackupPath -Recurse -Force
            
            # Limpiar backups antiguos (mantener solo los ultimos 3)
            Get-ChildItem -Path "\\vm06\shared\app-pool\homes" -Filter "crm-backup-*" | 
            Sort-Object CreationTime -Descending | 
            Select-Object -Skip 3 | 
            Remove-Item -Recurse -Force
        }
        
        # Crear directorio si no existe
        if (-not (Test-Path $crmPath)) {
            Write-Log "Creating CRM directory: $crmPath"
            New-Item -ItemType Directory -Path $crmPath -Force | Out-Null
        }
        
        # Limpiar directorio actual (excepto api y web.config de la raíz)
        Write-Log "Cleaning CRM directory (preserving api folder and root web.config)"
        Get-ChildItem -Path $crmPath -Recurse -Exclude "api", "web.config" | Where-Object { 
            # Excluir la carpeta api y todo su contenido
            $_.FullName -notmatch '\\api($|\\)' -and 
            # Excluir el web.config de la raíz (no los de subdirectorios)
            -not ($_.Name -eq "web.config" -and $_.DirectoryName -eq $crmPath)
        } | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
        
        # Copiar archivos del build
        Write-Log "Copying CRM build files"
        $BuildPath = "$RepoPath\crm\.output\public"
        Copy-Item -Path "$BuildPath\*" -Destination $crmPath -Recurse -Force
        
        # Crear archivo de version
        Write-Log "Creating CRM version file"
        $gitCommit = try { git -C $RepoPath rev-parse --short HEAD 2>$null } catch { "unknown" }
        $gitBranch = try { git -C $RepoPath rev-parse --abbrev-ref HEAD 2>$null } catch { "unknown" }
        
        $versionInfo = @{
            version    = Get-Date -Format "yyyy.MM.dd.HHmm"
            deployDate = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
            commit     = if ($gitCommit) { $gitCommit } else { "unknown" }
            branch     = if ($gitBranch) { $gitBranch } else { "unknown" }
            apiUrl     = "https://app-pool.vylaris.online/homes/api"
        }
        $versionInfo | ConvertTo-Json | Out-File -FilePath "$crmPath\version.json" -Encoding UTF8 -Force
        
        # Crear web.config para SPA
        Write-Log "Creating CRM web.config"
        $webConfig = @"
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
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
    <validation validateIntegratedModeConfiguration="false" />
    <urlCompression doStaticCompression="true" doDynamicCompression="false" />
  </system.webServer>
</configuration>
"@
        $webConfig | Out-File -FilePath "$crmPath\web.config" -Encoding UTF8 -Force
        
        Write-Log "CRM deployment completed successfully"
    }
    
    # ========================================
    # 2. DEPLOY API
    # ========================================
    if ($DeployAPI) {
        Write-Log "========================================="
        Write-Log "Deploying API to \\vm06\shared\app-pool\homes\api"
        Write-Log "========================================="
        
        $apiPath = "\\vm06\shared\app-pool\homes\api"
        
        # Navegar al directorio de la API
        Write-Log "Navigating to API directory"
        Set-Location "$RepoPath\api"
        
        # Verificar que existe el archivo .sln
        if (-not (Test-Path "CrmAgencySystem.sln")) {
            throw "CrmAgencySystem.sln not found in $PWD"
        }
        
        # Restaurar dependencias NuGet
        Write-Log "Restoring NuGet packages"
        dotnet restore CrmAgencySystem.sln
        
        # Build y Publish de la API
        Write-Log "Building and publishing .NET API"
        $publishPath = "$RepoPath\api\publish"
        
        dotnet publish "ApiService\ApiService.csproj" `
            --configuration Release `
            --output $publishPath `
            --no-restore `
            /p:EnvironmentName=Production
        
        # Verificar que el publish fue exitoso
        if (-not (Test-Path "$publishPath\ApiService.dll")) {
            throw "API Build failed - ApiService.dll not found in $publishPath"
        }
        
        # Crear app_offline.htm
        Write-Log "Creating app_offline.htm"
        $appOfflinePath = "$apiPath\app_offline.htm"
        
        if (Test-Path $apiPath) {
            $appOfflineContent = @"
<!DOCTYPE html>
<html>
<head>
    <title>Actualizando...</title>
    <meta http-equiv="refresh" content="5">
</head>
<body>
    <h1>Actualizando la aplicacion...</h1>
    <p>Por favor espera unos segundos. La pagina se recargara automaticamente.</p>
</body>
</html>
"@
            $appOfflineContent | Out-File -FilePath $appOfflinePath -Encoding UTF8 -Force
            Start-Sleep -Seconds 2
        }
        
        # Crear backup
        if (Test-Path $apiPath) {
            $BackupPath = "$apiPath-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
            Write-Log "Creating API backup: $BackupPath"
            Copy-Item -Path $apiPath -Destination $BackupPath -Recurse -Force -Exclude "app_offline.htm"
            
            # Limpiar backups antiguos
            Get-ChildItem -Path "\\vm06\shared\app-pool\homes" -Filter "api-backup-*" | 
            Sort-Object CreationTime -Descending | 
            Select-Object -Skip 3 | 
            Remove-Item -Recurse -Force
        }
        
        # Crear directorio si no existe
        if (-not (Test-Path $apiPath)) {
            Write-Log "Creating API directory: $apiPath"
            New-Item -ItemType Directory -Path $apiPath -Force | Out-Null
        }
        
        # Copiar archivos del publish
        Write-Log "Copying API published files"
        Get-ChildItem -Path $publishPath -Recurse | ForEach-Object {
            $targetPath = $_.FullName.Replace($publishPath, $apiPath)
            
            if ($_.PSIsContainer) {
                New-Item -ItemType Directory -Path $targetPath -Force | Out-Null
            }
            else {
                Copy-Item -Path $_.FullName -Destination $targetPath -Force
            }
        }
        
        # Copiar appsettings.json de produccion si existe
        $prodSettings = "$RepoPath\api\ApiService\appsettings.Production.json"
        if (Test-Path $prodSettings) {
            Write-Log "Copying production appsettings"
            Copy-Item -Path $prodSettings -Destination "$apiPath\appsettings.Production.json" -Force
        }
        
        # Eliminar app_offline.htm
        Write-Log "Removing app_offline.htm"
        if (Test-Path $appOfflinePath) {
            Remove-Item -Path $appOfflinePath -Force
        }
        
        Write-Log "API deployment completed successfully"
    }
    
    Write-Log "========================================="
    Write-Log "=== All deployments completed successfully ==="
    Write-Log "========================================="
    
    if ($DeployCRM) { Write-Log "CRM deployed to: \\vm06\shared\app-pool\homes\crm" }
    if ($DeployAPI) { Write-Log "API deployed to: \\vm06\shared\app-pool\homes\api" }
    
    exit 0
    
}
catch {
    Write-Log "ERROR: $($_.Exception.Message)"
    Write-Log "Stack Trace: $($_.ScriptStackTrace)"
    Write-Log "=== Deployment failed ==="
    exit 1
}
