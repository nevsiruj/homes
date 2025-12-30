# Script para copiar los scripts de deploy al servidor IIS
# Ejecutar este script en el servidor IIS

Write-Host "Copiando scripts de deploy al servidor IIS..." -ForegroundColor Cyan

# Crear directorio si no existe
$deployScriptsPath = "C:\DeployScripts"
if (-not (Test-Path $deployScriptsPath)) {
    New-Item -ItemType Directory -Path $deployScriptsPath -Force | Out-Null
    Write-Host "[OK] Directorio creado: $deployScriptsPath" -ForegroundColor Green
}

# Copiar deploy-crm.ps1
$crmScriptSource = "C:\repos\others\homes\.agent\workflows\deploy-crm.ps1"
$crmScriptDest = "C:\DeployScripts\deploy-crm.ps1"

if (Test-Path $crmScriptSource) {
    Copy-Item -Path $crmScriptSource -Destination $crmScriptDest -Force
    Write-Host "[OK] Copiado: deploy-crm.ps1" -ForegroundColor Green
}
else {
    Write-Host "[ERROR] No se encontro: $crmScriptSource" -ForegroundColor Red
}

# Copiar deploy-api.ps1
$apiScriptSource = "C:\repos\others\homes\.agent\workflows\deploy-api.ps1"
$apiScriptDest = "C:\DeployScripts\deploy-api.ps1"

if (Test-Path $apiScriptSource) {
    Copy-Item -Path $apiScriptSource -Destination $apiScriptDest -Force
    Write-Host "[OK] Copiado: deploy-api.ps1" -ForegroundColor Green
}
else {
    Write-Host "[ERROR] No se encontro: $apiScriptSource" -ForegroundColor Red
}

Write-Host ""
Write-Host "Scripts copiados exitosamente!" -ForegroundColor Green
Write-Host "Ubicacion: $deployScriptsPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "Archivos:" -ForegroundColor Yellow
Get-ChildItem $deployScriptsPath -Filter "deploy-*.ps1" | ForEach-Object {
    Write-Host "  - $($_.Name)" -ForegroundColor White
}
