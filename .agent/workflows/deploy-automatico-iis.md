---
description: Configurar deploy automático en IIS a través de push
---

# Deploy Automático del CRM a IIS mediante Push

Este workflow describe cómo configurar un deploy automático del **CRM (Nuxt SPA)** a un servidor IIS local cuando hagas push a GitHub.

## 🎯 Solución Recomendada: GitHub Self-Hosted Runner

Como tu servidor IIS es **local** (sin IP pública), usaremos un **Self-Hosted Runner** de GitHub que se ejecuta en tu servidor local.

### ✅ Ventajas:
- **GRATIS** - No consume minutos de GitHub Actions
- **Simple** - Configuración en 10 minutos
- **Seguro** - No expone puertos al internet
- **Integrado** - Todo visible en GitHub Actions
- **Ligero** - Solo ~50MB de RAM

---

## 📋 Paso 1: Instalar GitHub Self-Hosted Runner en el Servidor IIS

### 1.1 Descargar e Instalar el Runner

Ejecuta estos comandos en **PowerShell como Administrador** en tu servidor IIS:

```powershell
# Crear directorio para el runner
New-Item -ItemType Directory -Path "C:\actions-runner" -Force
Set-Location "C:\actions-runner"

# Descargar el runner (versión Windows x64)
Invoke-WebRequest -Uri "https://github.com/actions/runner/releases/download/v2.321.0/actions-runner-win-x64-2.321.0.zip" -OutFile "actions-runner.zip"

# Extraer el archivo
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory("$PWD\actions-runner.zip", "$PWD")

# Limpiar
Remove-Item "actions-runner.zip"
```

### 1.2 Obtener Token de Registro desde GitHub

1. Ve a tu repositorio en GitHub: `https://github.com/TU_USUARIO/homes`
2. Settings → Actions → Runners → New self-hosted runner
3. Selecciona **Windows** y copia el **token** que aparece

### 1.3 Configurar el Runner

```powershell
# Configurar el runner (reemplaza YOUR_TOKEN con el token de GitHub)
.\config.cmd --url https://github.com/TU_USUARIO/homes --token YOUR_TOKEN --name "IIS-Server-Runner" --work "_work"

# Cuando te pregunte:
# - Runner group: Presiona Enter (default)
# - Runner name: IIS-Server-Runner (o el que prefieras)
# - Work folder: Presiona Enter (default: _work)
# - Run as service: Y (Sí)
# - User account: Presiona Enter (NETWORK SERVICE)
```

### 1.4 Instalar como Servicio de Windows

```powershell
# Instalar el runner como servicio
.\svc.sh install

# Iniciar el servicio
.\svc.sh start

# Verificar que está corriendo
.\svc.sh status
```

✅ **El runner ahora está escuchando por trabajos de GitHub!**

---

## 📋 Paso 2: Crear Script de Deploy PowerShell

Crea el archivo `C:\DeployScripts\deploy-crm.ps1`:

```powershell
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
    
    # 4. Build de producción (genera SPA en .output/public)
    Write-Log "Building Nuxt SPA"
    $env:NODE_ENV = "production"
    npm run generate
    
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
    } else {
        Write-Log "WARNING: Website $WebsiteName does not exist in IIS. Will create directory only."
    }
    
    # 6. Crear backup del deployment anterior
    if (Test-Path $WebsitePath) {
        $BackupPath = "$WebsitePath-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
        Write-Log "Creating backup: $BackupPath"
        Copy-Item -Path $WebsitePath -Destination $BackupPath -Recurse -Force
        
        # Limpiar backups antiguos (mantener solo los últimos 3)
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
    
    # 10. Crear web.config para SPA (importante para Nuxt SPA)
    Write-Log "Creating web.config for SPA routing"
    $webConfig = @"
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <system.webServer>
        <rewrite>
            <rules>
                <rule name="SPA Routes" stopProcessing="true">
                    <match url=".*" />
                    <conditions logicalGrouping="MatchAll">
                        <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
                        <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
                    </conditions>
                    <action type="Rewrite" url="/200.html" />
                </rule>
            </rules>
        </rewrite>
        <staticContent>
            <mimeMap fileExtension=".json" mimeType="application/json" />
            <mimeMap fileExtension=".webp" mimeType="image/webp" />
        </staticContent>
        <httpProtocol>
            <customHeaders>
                <add name="Cache-Control" value="public, max-age=31536000" />
            </customHeaders>
        </httpProtocol>
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
        Write-Log "✅ index.html found"
    }
    if (Test-Path "$WebsitePath\_nuxt") {
        Write-Log "✅ _nuxt assets folder found"
    }
    
    Write-Log "=== Deployment completed successfully ==="
    Write-Log "Deployed files from: $BuildPath"
    Write-Log "To: $WebsitePath"
    
    exit 0
    
} catch {
    Write-Log "❌ ERROR: $($_.Exception.Message)"
    Write-Log "Stack Trace: $($_.ScriptStackTrace)"
    
    # Intentar reiniciar el sitio en caso de error
    try {
        if (Get-Website -Name $WebsiteName -ErrorAction SilentlyContinue) {
            Start-Website -Name $WebsiteName
        }
    } catch {
        Write-Log "Failed to restart website after error"
    }
    
    Write-Log "=== Deployment failed ==="
    exit 1
}
```

**Crear el directorio de scripts:**

```powershell
New-Item -ItemType Directory -Path "C:\DeployScripts" -Force
New-Item -ItemType Directory -Path "C:\DeployScripts\logs" -Force
```

---

## 📋 Paso 3: Crear GitHub Actions Workflow

Crea el archivo `.github/workflows/deploy-crm.yml` en la raíz del repositorio:

```yaml
name: Deploy CRM to IIS

on:
  push:
    branches:
      - main  # Cambiar a 'dev' si usas otra rama
    paths:
      - 'crm/**'  # Solo deployar cuando cambien archivos del CRM
      - '.github/workflows/deploy-crm.yml'  # O cuando cambie este workflow

jobs:
  deploy:
    runs-on: self-hosted  # Usar el runner local
    
    defaults:
      run:
        working-directory: crm
    
    steps:
    - name: 📥 Checkout code
      uses: actions/checkout@v4
      
    - name: 📦 Setup Node.js
      uses: actions/setup-node@v4
      with:
        node-version: '20'
        cache: 'npm'
        cache-dependency-path: crm/package-lock.json
        
    - name: 🔍 Verify environment
      run: |
        Write-Host "Node version: $(node --version)"
        Write-Host "NPM version: $(npm --version)"
        Write-Host "Current directory: $PWD"
        Write-Host "Files in directory:"
        Get-ChildItem
      shell: powershell
        
    - name: 🚀 Deploy to IIS
      run: |
        # Ejecutar el script de deploy
        C:\DeployScripts\deploy-crm.ps1 -RepoPath "$env:GITHUB_WORKSPACE" -WebsiteName "HomesCRM" -WebsitePath "C:\inetpub\wwwroot\homes-crm"
      shell: powershell
      
    - name: 📊 Show deployment logs
      if: always()
      run: |
        $latestLog = Get-ChildItem "C:\DeployScripts\logs\deploy-crm-*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
        if ($latestLog) {
          Write-Host "=== Deployment Log ==="
          Get-Content $latestLog.FullName
        }
      shell: powershell
      
    - name: ✅ Deployment Success
      if: success()
      run: Write-Host "✅ CRM deployed successfully to IIS!"
      shell: powershell
      
    - name: ❌ Deployment Failed
      if: failure()
      run: Write-Host "❌ CRM deployment failed! Check logs above."
      shell: powershell
```

---

## 📋 Paso 4: Configurar el Sitio en IIS (Si no existe)

Si aún no tienes el sitio configurado en IIS, ejecuta:

```powershell
# Importar módulo de IIS
Import-Module WebAdministration

# Crear Application Pool
New-WebAppPool -Name "HomesCRM"
Set-ItemProperty IIS:\AppPools\HomesCRM -Name managedRuntimeVersion -Value ""  # No Managed Code para SPA

# Crear directorio del sitio
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\homes-crm" -Force

# Crear sitio web
New-Website -Name "HomesCRM" `
    -PhysicalPath "C:\inetpub\wwwroot\homes-crm" `
    -ApplicationPool "HomesCRM" `
    -Port 8080  # Cambiar al puerto que prefieras

# Iniciar el sitio
Start-Website -Name "HomesCRM"

Write-Host "✅ Sitio IIS creado: http://localhost:8080"
```

---

## 🧪 Paso 5: Probar el Deploy

### 5.1 Hacer un cambio de prueba

```powershell
# En tu máquina local
cd C:\repos\others\homes\crm

# Hacer un cambio pequeño (ej: editar README.md)
echo "Test deploy" >> README.md

# Commit y push
git add .
git commit -m "test: trigger deploy"
git push origin main
```

### 5.2 Ver el progreso en GitHub

1. Ve a tu repositorio en GitHub
2. Click en la pestaña **Actions**
3. Verás el workflow "Deploy CRM to IIS" ejecutándose
4. Click en el workflow para ver los logs en tiempo real

### 5.3 Verificar en el servidor

```powershell
# Ver logs del deploy
Get-Content "C:\DeployScripts\logs\deploy-crm-*.log" | Select-Object -Last 50

# Verificar archivos deployados
Get-ChildItem "C:\inetpub\wwwroot\homes-crm"

# Verificar estado del sitio
Get-Website -Name "HomesCRM"

# Probar en navegador
Start-Process "http://localhost:8080"
```

---

## 🔧 Troubleshooting

### Problema: Runner no aparece en GitHub

**Solución:**
```powershell
cd C:\actions-runner
.\svc.sh status
.\svc.sh start
```

### Problema: Build falla por falta de memoria

**Solución:** Agregar al workflow antes del deploy:
```yaml
- name: Increase Node memory
  run: $env:NODE_OPTIONS="--max-old-space-size=4096"
  shell: powershell
```

### Problema: El sitio muestra 404 en rutas

**Solución:** Verificar que `web.config` tenga las reglas de rewrite correctas (el script ya lo crea automáticamente).

### Problema: Permisos denegados

**Solución:**
```powershell
# Dar permisos al usuario del runner
$path = "C:\inetpub\wwwroot\homes-crm"
$acl = Get-Acl $path
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("NETWORK SERVICE","FullControl","ContainerInherit,ObjectInherit","None","Allow")
$acl.SetAccessRule($rule)
Set-Acl $path $acl
```

### Ver logs del runner

```powershell
# Ver logs del servicio
Get-Content "C:\actions-runner\_diag\Runner_*.log" | Select-Object -Last 100
```

---

## 📊 Monitoreo

### Ver historial de deploys

```powershell
# Listar todos los logs
Get-ChildItem "C:\DeployScripts\logs" | Sort-Object LastWriteTime -Descending

# Ver último deploy
$latestLog = Get-ChildItem "C:\DeployScripts\logs\deploy-crm-*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
Get-Content $latestLog.FullName
```

### Ver backups disponibles

```powershell
Get-ChildItem "C:\inetpub\wwwroot" -Filter "homes-crm-backup-*" | Sort-Object CreationTime -Descending
```

### Rollback a backup anterior

```powershell
# Detener sitio
Stop-Website -Name "HomesCRM"

# Restaurar backup (reemplazar con la fecha del backup deseado)
$backupPath = "C:\inetpub\wwwroot\homes-crm-backup-20241229-120000"
Remove-Item "C:\inetpub\wwwroot\homes-crm\*" -Recurse -Force
Copy-Item "$backupPath\*" -Destination "C:\inetpub\wwwroot\homes-crm" -Recurse -Force

# Reiniciar sitio
Start-Website -Name "HomesCRM"
```

---

## 🎯 Resumen del Flujo

```
1. Haces cambios en /crm
2. git push origin main
3. GitHub detecta el push
4. GitHub notifica al runner local
5. Runner ejecuta el workflow
6. Workflow ejecuta deploy-crm.ps1
7. Script hace build y copia a IIS
8. IIS reinicia y sirve la nueva versión
9. ✅ Deploy completado
```

**Todo visible en GitHub Actions con logs completos!**

---

## ⚙️ Configuración Avanzada (Opcional)

### Deploy solo en horarios específicos

Agregar al workflow:
```yaml
on:
  push:
    branches:
      - main
  schedule:
    - cron: '0 2 * * *'  # Deploy automático a las 2 AM
```

### Notificaciones por email

Agregar al final del workflow:
```yaml
- name: Send notification
  if: always()
  run: |
    # Aquí puedes agregar lógica para enviar emails
    Write-Host "Deployment finished with status: ${{ job.status }}"
```

### Deploy a múltiples ambientes

Crear workflows separados:
- `.github/workflows/deploy-crm-dev.yml` (rama dev)
- `.github/workflows/deploy-crm-prod.yml` (rama main)

---

## ✅ Checklist de Instalación

- [ ] Runner instalado en servidor IIS
- [ ] Runner configurado como servicio
- [ ] Script `deploy-crm.ps1` creado
- [ ] Workflow `.github/workflows/deploy-crm.yml` creado
- [ ] Sitio IIS configurado
- [ ] Permisos configurados
- [ ] Deploy de prueba exitoso
- [ ] Logs funcionando correctamente

---

¿Listo para empezar? Comienza con el **Paso 1** y avísame si necesitas ayuda en algún paso! 🚀
