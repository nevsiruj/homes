# restart-medianaranja-api.ps1
# Script para reiniciar el Application Pool de la API en VM06

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Reiniciando API de Media Naranja" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

try {
    # Importar módulo de IIS
    Import-Module WebAdministration -ErrorAction Stop
    
    # Nombre del Application Pool
    $apiPoolName = "MediaNaranjaAPI"
    
    # Verificar si el Application Pool existe
    if (Test-Path "IIS:\AppPools\$apiPoolName") {
        Write-Host "Reiniciando Application Pool: $apiPoolName" -ForegroundColor Yellow
        Restart-WebAppPool -Name $apiPoolName
        Start-Sleep -Seconds 3
        
        # Verificar estado
        $state = (Get-WebAppPoolState -Name $apiPoolName).Value
        Write-Host "✅ Application Pool reiniciado" -ForegroundColor Green
        Write-Host "   Estado actual: $state" -ForegroundColor Gray
    }
    else {
        Write-Host "❌ Application Pool '$apiPoolName' no encontrado" -ForegroundColor Red
        Write-Host ""
        Write-Host "Application Pools disponibles:" -ForegroundColor Yellow
        Get-WebAppPool | Select-Object Name, State | Format-Table -AutoSize
    }
    
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Proceso completado" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
}
catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "NOTA: Este script debe ejecutarse en VM06 como Administrador" -ForegroundColor Yellow
}
