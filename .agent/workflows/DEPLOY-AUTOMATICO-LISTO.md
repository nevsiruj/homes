# ✅ Deploy Automático Configurado

## 🎉 ¡Todo está listo!

Has configurado exitosamente el deploy automático para el CRM y la API usando GitHub Self-Hosted Runner.

---

## 📋 Últimos Pasos en el Servidor IIS

### 1. Copiar los scripts de deploy

En el **servidor IIS**, ejecuta:

```powershell
cd C:\repos\others\homes\.agent\workflows
.\copy-scripts-to-server.ps1
```

Esto copiará los scripts `deploy-crm.ps1` y `deploy-api.ps1` a `C:\DeployScripts\`

### 2. Verificar que el runner está activo

```powershell
cd C:\actions-runner
.\svc.sh status
```

Debería mostrar: `active` o `running`

### 3. Verificar en GitHub

Ve a: https://github.com/nevsiruj/homes/settings/actions/runners

Deberías ver tu runner con estado **"Idle"** (verde)

---

## 🚀 Cómo Funciona

### Deploy del CRM

Cuando hagas cambios en la carpeta `crm/`:

```bash
# En tu máquina local
cd C:\repos\others\homes\crm
# Hacer cambios...
git add .
git commit -m "feat: nueva funcionalidad en CRM"
git push origin main
```

**Automáticamente:**
1. GitHub detecta el push
2. Ejecuta el workflow `deploy-crm.yml`
3. El runner local ejecuta `deploy-crm.ps1`
4. Build de Nuxt SPA
5. Deploy a IIS en `C:\inetpub\wwwroot\homes-crm`
6. Reinicia el sitio

### Deploy de la API

Cuando hagas cambios en la carpeta `api/`:

```bash
# En tu máquina local
cd C:\repos\others\homes\api
# Hacer cambios...
git add .
git commit -m "feat: nuevo endpoint en API"
git push origin main
```

**Automáticamente:**
1. GitHub detecta el push
2. Ejecuta el workflow `deploy-api.yml`
3. El runner local ejecuta `deploy-api.ps1`
4. Build de .NET 6
5. Deploy a IIS en `C:\inetpub\wwwroot\homes-api`
6. Zero-downtime deployment

---

## 🧪 Probar el Deploy

### Opción 1: Hacer un cambio de prueba

```bash
# En tu máquina local
cd C:\repos\others\homes

# Hacer un cambio pequeño en el CRM
echo "# Test deploy" >> crm\README.md

# Commit y push
git add .
git commit -m "test: trigger CRM deploy"
git push origin main
```

### Opción 2: Ver el workflow en acción

1. Ve a: https://github.com/nevsiruj/homes/actions
2. Verás el workflow ejecutándose en tiempo real
3. Click en el workflow para ver los logs

---

## 📊 Monitoreo

### Ver logs de deploy

```powershell
# En el servidor IIS

# Ver último deploy del CRM
$log = Get-ChildItem "C:\DeployScripts\logs\deploy-crm-*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
Get-Content $log.FullName

# Ver último deploy de la API
$log = Get-ChildItem "C:\DeployScripts\logs\deploy-api-*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
Get-Content $log.FullName
```

### Ver todos los deploys

```powershell
Get-ChildItem "C:\DeployScripts\logs" | Sort-Object LastWriteTime -Descending
```

### Ver backups disponibles

```powershell
# Backups del CRM
Get-ChildItem "C:\inetpub\wwwroot" -Filter "homes-crm-backup-*"

# Backups de la API
Get-ChildItem "C:\inetpub\wwwroot" -Filter "homes-api-backup-*"
```

---

## 🔄 Rollback (Volver a versión anterior)

Si algo sale mal, puedes volver a una versión anterior:

### Rollback del CRM

```powershell
# Detener sitio
Stop-Website -Name "HomesCRM"

# Restaurar backup (cambiar fecha por la que necesites)
$backup = "C:\inetpub\wwwroot\homes-crm-backup-20241229-235000"
Remove-Item "C:\inetpub\wwwroot\homes-crm\*" -Recurse -Force
Copy-Item "$backup\*" -Destination "C:\inetpub\wwwroot\homes-crm" -Recurse -Force

# Reiniciar sitio
Start-Website -Name "HomesCRM"
```

### Rollback de la API

```powershell
# Crear app_offline.htm
"<h1>Mantenimiento</h1>" | Out-File "C:\inetpub\wwwroot\homes-api\app_offline.htm"

# Restaurar backup
$backup = "C:\inetpub\wwwroot\homes-api-backup-20241229-235000"
Remove-Item "C:\inetpub\wwwroot\homes-api\*" -Recurse -Force -Exclude "app_offline.htm"
Copy-Item "$backup\*" -Destination "C:\inetpub\wwwroot\homes-api" -Recurse -Force

# Eliminar app_offline.htm
Remove-Item "C:\inetpub\wwwroot\homes-api\app_offline.htm"

# Reciclar app pool
Restart-WebAppPool -Name "HomesAPI"
```

---

## 🛠️ Configuración de IIS

Si aún no has creado los sitios en IIS, ejecuta:

### Crear sitio para el CRM

```powershell
Import-Module WebAdministration

# Crear Application Pool
New-WebAppPool -Name "HomesCRM"
Set-ItemProperty IIS:\AppPools\HomesCRM -Name managedRuntimeVersion -Value ""

# Crear directorio
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\homes-crm" -Force

# Crear sitio
New-Website -Name "HomesCRM" `
    -PhysicalPath "C:\inetpub\wwwroot\homes-crm" `
    -ApplicationPool "HomesCRM" `
    -Port 8080

# Iniciar
Start-Website -Name "HomesCRM"

Write-Host "CRM disponible en: http://localhost:8080"
```

### Crear sitio para la API

```powershell
Import-Module WebAdministration

# Crear Application Pool
New-WebAppPool -Name "HomesAPI"
Set-ItemProperty IIS:\AppPools\HomesAPI -Name managedRuntimeVersion -Value "v4.0"

# Crear directorio
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot\homes-api" -Force

# Crear sitio
New-Website -Name "HomesAPI" `
    -PhysicalPath "C:\inetpub\wwwroot\homes-api" `
    -ApplicationPool "HomesAPI" `
    -Port 5000

# Iniciar
Start-Website -Name "HomesAPI"

Write-Host "API disponible en: http://localhost:5000"
```

---

## 🔧 Troubleshooting

### El workflow no se ejecuta

**Solución:**
1. Verifica que el runner esté activo: `cd C:\actions-runner; .\svc.sh status`
2. Verifica en GitHub: https://github.com/nevsiruj/homes/settings/actions/runners
3. Reinicia el runner: `.\svc.sh stop; .\svc.sh start`

### El deploy falla

**Solución:**
1. Ve a GitHub Actions y revisa los logs
2. Revisa los logs locales: `C:\DeployScripts\logs\`
3. Verifica que los sitios IIS existan
4. Verifica permisos en los directorios

### El sitio no carga después del deploy

**CRM:**
- Verifica que existe `C:\inetpub\wwwroot\homes-crm\index.html`
- Verifica que existe `C:\inetpub\wwwroot\homes-crm\web.config`
- Verifica que el sitio esté iniciado: `Get-Website -Name "HomesCRM"`

**API:**
- Verifica que existe `C:\inetpub\wwwroot\homes-api\ApiService.dll`
- Verifica que el Application Pool esté corriendo
- Revisa los logs de IIS: `C:\inetpub\logs\LogFiles\`

---

## 📁 Estructura de Archivos

```
C:\
├── actions-runner\              # GitHub Runner
│   ├── _work\homes\homes\       # Código clonado
│   └── _diag\                   # Logs del runner
│
├── DeployScripts\               # Scripts de deploy
│   ├── deploy-crm.ps1
│   ├── deploy-api.ps1
│   └── logs\                    # Logs de deploys
│
└── inetpub\wwwroot\
    ├── homes-crm\               # CRM deployado
    ├── homes-crm-backup-*\      # Backups del CRM
    ├── homes-api\               # API deployada
    └── homes-api-backup-*\      # Backups de la API
```

---

## ✅ Checklist Final

- [ ] Runner instalado y corriendo
- [ ] Scripts copiados a `C:\DeployScripts\`
- [ ] Workflows creados en `.github/workflows/`
- [ ] Sitios IIS creados (HomesCRM y HomesAPI)
- [ ] Permisos configurados
- [ ] Deploy de prueba exitoso
- [ ] Logs funcionando

---

## 🎯 Resumen

**Ahora tienes:**
- ✅ Deploy automático del CRM al hacer push
- ✅ Deploy automático de la API al hacer push
- ✅ Backups automáticos (últimos 3)
- ✅ Logs detallados de cada deploy
- ✅ Zero-downtime para la API
- ✅ Rollback fácil si algo falla
- ✅ Todo visible en GitHub Actions

**¡Listo para producción!** 🚀
