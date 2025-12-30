# deploy-api.ps1
# Script para deployar la API .NET 6 a IIS

param(
    [string]$RepoPath = "C:\actions-runner\_work\homes\homes",
    [string]$WebsiteName = "HomesAPI",
    [string]$WebsitePath = "C:\inetpub\wwwroot\homes-api"
)


# Configurar log
$LogFile = "C:\DeployScripts\logs\deploy-api-$(Get-Date -Format 'yyyyMMdd-HHmmss').log"
New-Item -ItemType Directory -Force -Path "C:\DeployScripts\logs" | Out-Null

function Write-Log {
    param([string]$Message)
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    "$timestamp - $Message" | Tee-Object -FilePath $LogFile -Append
}

try {
    Write-Log "=== Starting API Deployment ==="
    
    # 1. Navegar al directorio de la API
    Write-Log "Navigating to API directory"
    Set-Location "$RepoPath\api"
    
    # 2. Verificar que existe el archivo .sln
    if (-not (Test-Path "CrmAgencySystem.sln")) {
        throw "CrmAgencySystem.sln not found in $PWD"
    }
    
    # 3. Restaurar dependencias NuGet
    Write-Log "Restoring NuGet packages"
    dotnet restore CrmAgencySystem.sln
    
    # 4. Build y Publish de la API
    Write-Log "Building and publishing .NET API"
    $publishPath = "$RepoPath\api\publish"
    
    dotnet publish "ApiService\ApiService.csproj" `
        --configuration Release `
        --output $publishPath `
        --no-restore `
        /p:EnvironmentName=Production
    
    # Verificar que el publish fue exitoso
    if (-not (Test-Path "$publishPath\ApiService.dll")) {
        throw "Build failed - ApiService.dll not found in $publishPath"
    }
    
    # 5. Crear app_offline.htm para deploy sin downtime
    Write-Log "Creating app_offline.htm (zero-downtime deployment)"
    $appOfflinePath = "$WebsitePath\app_offline.htm"
    
    if (Test-Path $WebsitePath) {
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
    
    # 6. Crear backup del deployment anterior
    if (Test-Path $WebsitePath) {
        $BackupPath = "$WebsitePath-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Write-Log "Creating backup: $BackupPath"
        
        # Copiar solo archivos importantes (no app_offline.htm)
        Copy-Item -Path $WebsitePath -Destination $BackupPath -Recurse -Force -Exclude "app_offline.htm"
        
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
    
    # 8. Copiar archivos del publish
    Write-Log "Copying published files to website directory"
    
    # Copiar todos los archivos excepto app_offline.htm
    Get-ChildItem -Path $publishPath -Recurse | ForEach-Object {
        $targetPath = $_.FullName.Replace($publishPath, $WebsitePath)
        
        if ($_.PSIsContainer) {
            New-Item -ItemType Directory -Path $targetPath -Force | Out-Null
        }
        else {
            Copy-Item -Path $_.FullName -Destination $targetPath -Force
        }
    }
    
    # 9. Copiar appsettings.json de produccion si existe
    $prodSettings = "$RepoPath\api\ApiService\appsettings.Production.json"
    if (Test-Path $prodSettings) {
        Write-Log "Copying production appsettings"
        Copy-Item -Path $prodSettings -Destination "$WebsitePath\appsettings.Production.json" -Force
    }
    
    # 10. Eliminar app_offline.htm para reactivar el sitio
    Write-Log "Removing app_offline.htm (reactivating site)"
    if (Test-Path $appOfflinePath) {
        Remove-Item -Path $appOfflinePath -Force
    }
    
    # 11. Reciclar el Application Pool
    Write-Log "Recycling application pool"
    Import-Module WebAdministration
    
    if (Get-Website -Name $WebsiteName -ErrorAction SilentlyContinue) {
        $appPool = (Get-Website -Name $WebsiteName).applicationPool
        Restart-WebAppPool -Name $appPool
        Start-Sleep -Seconds 3
    }
    else {
        Write-Log "WARNING: Website $WebsiteName does not exist in IIS"
    }
    
    # 12. Verificar deployment
    Write-Log "Verifying deployment"
    if (Test-Path "$WebsitePath\ApiService.dll") {
        Write-Log "OK - ApiService.dll found"
    }
    if (Test-Path "$WebsitePath\web.config") {
        Write-Log "OK - web.config found"
    }
    if (Test-Path "$WebsitePath\appsettings.json") {
        Write-Log "OK - appsettings.json found"
    }
    
    Write-Log "=== Deployment completed successfully ==="
    Write-Log "Deployed files from: $publishPath"
    Write-Log "To: $WebsitePath"
    
    exit 0
    
}
catch {
    Write-Log "ERROR: $($_.Exception.Message)"
    Write-Log "Stack Trace: $($_.ScriptStackTrace)"
    
    # Eliminar app_offline.htm en caso de error
    try {
        if (Test-Path "$WebsitePath\app_offline.htm") {
            Remove-Item -Path "$WebsitePath\app_offline.htm" -Force
            Write-Log "Removed app_offline.htm after error"
        }
    }
    catch {
        Write-Log "Failed to remove app_offline.htm"
    }
    
    Write-Log "=== Deployment failed ==="
    exit 1
}
