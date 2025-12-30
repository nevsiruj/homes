# ============================================================================
# Script de Instalación de GitHub Self-Hosted Runner
# ============================================================================
# Este script instala y configura un GitHub Self-Hosted Runner en Windows
# para automatizar deploys a IIS
# ============================================================================

param(
    [Parameter(Mandatory=$true)]
    [string]$GitHubToken,
    
    [Parameter(Mandatory=$false)]
    [string]$RepoOwner = "TU_USUARIO",  # Cambiar por tu usuario de GitHub
    
    [Parameter(Mandatory=$false)]
    [string]$RepoName = "homes",
    
    [Parameter(Mandatory=$false)]
    [string]$RunnerName = "IIS-Server-Runner",
    
    [Parameter(Mandatory=$false)]
    [string]$RunnerPath = "C:\actions-runner",
    
    [Parameter(Mandatory=$false)]
    [string]$RunnerVersion = "2.321.0"
)

# Colores para output
function Write-Success { param([string]$Message) Write-Host "✅ $Message" -ForegroundColor Green }
function Write-Info { param([string]$Message) Write-Host "ℹ️  $Message" -ForegroundColor Cyan }
function Write-Warning { param([string]$Message) Write-Host "⚠️  $Message" -ForegroundColor Yellow }
function Write-Error { param([string]$Message) Write-Host "❌ $Message" -ForegroundColor Red }

# ============================================================================
# Verificar que se ejecuta como Administrador
# ============================================================================
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Error "Este script debe ejecutarse como Administrador"
    Write-Info "Click derecho en PowerShell → 'Ejecutar como administrador'"
    exit 1
}

Write-Success "Script ejecutándose como Administrador"

# ============================================================================
# Paso 1: Crear directorio para el runner
# ============================================================================
Write-Info "Paso 1/7: Creando directorio para el runner..."

if (Test-Path $RunnerPath) {
    Write-Warning "El directorio $RunnerPath ya existe"
    $response = Read-Host "¿Deseas eliminarlo y reinstalar? (S/N)"
    if ($response -eq "S" -or $response -eq "s") {
        # Detener el servicio si existe
        $serviceName = "actions.runner.$RepoOwner-$RepoName.$RunnerName"
        if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
            Write-Info "Deteniendo servicio existente..."
            Stop-Service -Name $serviceName -Force
            & "$RunnerPath\svc.sh" uninstall
            Start-Sleep -Seconds 3
        }
        Remove-Item -Path $RunnerPath -Recurse -Force
        Write-Success "Directorio eliminado"
    } else {
        Write-Error "Instalación cancelada"
        exit 1
    }
}

New-Item -ItemType Directory -Path $RunnerPath -Force | Out-Null
Write-Success "Directorio creado: $RunnerPath"

# ============================================================================
# Paso 2: Descargar el runner
# ============================================================================
Write-Info "Paso 2/7: Descargando GitHub Runner v$RunnerVersion..."

Set-Location $RunnerPath
$runnerUrl = "https://github.com/actions/runner/releases/download/v$RunnerVersion/actions-runner-win-x64-$RunnerVersion.zip"
$zipPath = "$RunnerPath\actions-runner.zip"

try {
    # Descargar con barra de progreso
    $ProgressPreference = 'SilentlyContinue'
    Invoke-WebRequest -Uri $runnerUrl -OutFile $zipPath -UseBasicParsing
    $ProgressPreference = 'Continue'
    Write-Success "Runner descargado exitosamente"
} catch {
    Write-Error "Error al descargar el runner: $($_.Exception.Message)"
    exit 1
}

# ============================================================================
# Paso 3: Extraer el archivo
# ============================================================================
Write-Info "Paso 3/7: Extrayendo archivos..."

try {
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipPath, $RunnerPath)
    Remove-Item $zipPath -Force
    Write-Success "Archivos extraídos correctamente"
} catch {
    Write-Error "Error al extraer archivos: $($_.Exception.Message)"
    exit 1
}

# ============================================================================
# Paso 4: Obtener token de registro desde GitHub
# ============================================================================
Write-Info "Paso 4/7: Obteniendo token de registro desde GitHub..."

$repoUrl = "https://github.com/$RepoOwner/$RepoName"
$apiUrl = "https://api.github.com/repos/$RepoOwner/$RepoName/actions/runners/registration-token"

try {
    $headers = @{
        "Authorization" = "token $GitHubToken"
        "Accept" = "application/vnd.github.v3+json"
    }
    
    $response = Invoke-RestMethod -Uri $apiUrl -Method Post -Headers $headers
    $registrationToken = $response.token
    Write-Success "Token de registro obtenido"
} catch {
    Write-Error "Error al obtener token de GitHub: $($_.Exception.Message)"
    Write-Info "Verifica que el token tenga permisos de 'repo' y 'workflow'"
    Write-Info "Crea un token en: https://github.com/settings/tokens"
    exit 1
}

# ============================================================================
# Paso 5: Configurar el runner
# ============================================================================
Write-Info "Paso 5/7: Configurando el runner..."

try {
    # Ejecutar configuración del runner
    $configArgs = @(
        "--url", $repoUrl,
        "--token", $registrationToken,
        "--name", $RunnerName,
        "--work", "_work",
        "--labels", "windows,iis,self-hosted",
        "--unattended",
        "--replace"
    )
    
    & "$RunnerPath\config.cmd" $configArgs
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Runner configurado exitosamente"
    } else {
        throw "Error en la configuración del runner (Exit code: $LASTEXITCODE)"
    }
} catch {
    Write-Error "Error al configurar el runner: $($_.Exception.Message)"
    exit 1
}

# ============================================================================
# Paso 6: Instalar como servicio de Windows
# ============================================================================
Write-Info "Paso 6/7: Instalando como servicio de Windows..."

try {
    # Instalar el servicio
    & "$RunnerPath\svc.sh" install
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Servicio instalado correctamente"
    } else {
        throw "Error al instalar el servicio (Exit code: $LASTEXITCODE)"
    }
} catch {
    Write-Error "Error al instalar el servicio: $($_.Exception.Message)"
    exit 1
}

# ============================================================================
# Paso 7: Iniciar el servicio
# ============================================================================
Write-Info "Paso 7/7: Iniciando el servicio..."

try {
    & "$RunnerPath\svc.sh" start
    Start-Sleep -Seconds 3
    
    # Verificar estado
    $status = & "$RunnerPath\svc.sh" status
    
    if ($status -match "active" -or $status -match "running") {
        Write-Success "Servicio iniciado correctamente"
    } else {
        Write-Warning "El servicio puede no estar corriendo. Estado: $status"
    }
} catch {
    Write-Error "Error al iniciar el servicio: $($_.Exception.Message)"
    exit 1
}

# ============================================================================
# Crear directorio para scripts de deploy
# ============================================================================
Write-Info "Creando directorio para scripts de deploy..."

$deployScriptsPath = "C:\DeployScripts"
$deployLogsPath = "C:\DeployScripts\logs"

New-Item -ItemType Directory -Path $deployScriptsPath -Force | Out-Null
New-Item -ItemType Directory -Path $deployLogsPath -Force | Out-Null

Write-Success "Directorio de scripts creado: $deployScriptsPath"

# ============================================================================
# Resumen de instalación
# ============================================================================
Write-Host "`n" -NoNewline
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  ✅ INSTALACIÓN COMPLETADA EXITOSAMENTE" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 Información del Runner:" -ForegroundColor Yellow
Write-Host "   • Nombre: $RunnerName" -ForegroundColor White
Write-Host "   • Ubicación: $RunnerPath" -ForegroundColor White
Write-Host "   • Repositorio: $repoUrl" -ForegroundColor White
Write-Host "   • Labels: windows, iis, self-hosted" -ForegroundColor White
Write-Host ""
Write-Host "🔧 Comandos útiles:" -ForegroundColor Yellow
Write-Host "   • Ver estado: " -NoNewline -ForegroundColor White
Write-Host "cd $RunnerPath; .\svc.sh status" -ForegroundColor Cyan
Write-Host "   • Detener: " -NoNewline -ForegroundColor White
Write-Host "cd $RunnerPath; .\svc.sh stop" -ForegroundColor Cyan
Write-Host "   • Iniciar: " -NoNewline -ForegroundColor White
Write-Host "cd $RunnerPath; .\svc.sh start" -ForegroundColor Cyan
Write-Host "   • Ver logs: " -NoNewline -ForegroundColor White
Write-Host "Get-Content '$RunnerPath\_diag\Runner_*.log' -Tail 50" -ForegroundColor Cyan
Write-Host ""
Write-Host "📁 Directorios creados:" -ForegroundColor Yellow
Write-Host "   • Runner: $RunnerPath" -ForegroundColor White
Write-Host "   • Scripts: $deployScriptsPath" -ForegroundColor White
Write-Host "   • Logs: $deployLogsPath" -ForegroundColor White
Write-Host ""
Write-Host "🚀 Próximos pasos:" -ForegroundColor Yellow
Write-Host "   1. Verifica que el runner aparezca en GitHub:" -ForegroundColor White
Write-Host "      $repoUrl/settings/actions/runners" -ForegroundColor Cyan
Write-Host "   2. Crea los scripts de deploy (deploy-crm.ps1, deploy-api.ps1)" -ForegroundColor White
Write-Host "   3. Crea los workflows de GitHub Actions" -ForegroundColor White
Write-Host "   4. Haz un push de prueba para verificar el deploy" -ForegroundColor White
Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# ============================================================================
# Verificar en GitHub
# ============================================================================
Write-Info "Verificando runner en GitHub..."
Start-Sleep -Seconds 5

try {
    $runnersUrl = "https://api.github.com/repos/$RepoOwner/$RepoName/actions/runners"
    $headers = @{
        "Authorization" = "token $GitHubToken"
        "Accept" = "application/vnd.github.v3+json"
    }
    
    $runners = Invoke-RestMethod -Uri $runnersUrl -Headers $headers
    $thisRunner = $runners.runners | Where-Object { $_.name -eq $RunnerName }
    
    if ($thisRunner) {
        Write-Success "Runner encontrado en GitHub!"
        Write-Host "   • ID: $($thisRunner.id)" -ForegroundColor White
        Write-Host "   • Estado: $($thisRunner.status)" -ForegroundColor White
        Write-Host "   • Ocupado: $($thisRunner.busy)" -ForegroundColor White
    } else {
        Write-Warning "Runner no encontrado en GitHub aún. Espera unos segundos y verifica manualmente."
    }
} catch {
    Write-Warning "No se pudo verificar en GitHub automáticamente. Verifica manualmente en:"
    Write-Host "   $repoUrl/settings/actions/runners" -ForegroundColor Cyan
}

Write-Host ""
Write-Success "¡Todo listo! El runner está esperando por trabajos de GitHub Actions."
Write-Host ""
