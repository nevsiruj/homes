# ✅ Guía de Verificación del Deploy Automático

## 🎉 ¡Push Completado!

Acabas de hacer push a `master`. Ahora vamos a verificar que el deploy automático funciona.

---

## 📋 Paso 1: Verificar en GitHub Actions

### Opción A: Navegador Web

1. **Abre tu navegador** y ve a:
   ```
   https://github.com/nevsiruj/homes/actions
   ```

2. **Deberías ver:**
   - Un workflow llamado **"Deploy CRM to IIS"** ejecutándose o completado
   - Estado: 🟡 Amarillo (en progreso) o 🟢 Verde (exitoso)

3. **Click en el workflow** para ver los detalles:
   - Verás cada paso ejecutándose
   - Logs en tiempo real
   - Tiempo de ejecución

### Opción B: Desde la Terminal

```bash
# Ver el último workflow ejecutado
gh run list --limit 5

# Ver detalles del último run
gh run view

# Ver logs en tiempo real
gh run watch
```

---

## 📋 Paso 2: Verificar en el Servidor IIS

### En el servidor donde instalaste el runner:

#### 1. Verificar que el runner recibió el trabajo

```powershell
# Ver logs del runner
cd C:\actions-runner
Get-Content "_diag\Runner_*.log" -Tail 50
```

**Deberías ver:**
- "Running job: deploy"
- "Checkout code"
- "Setup Node.js"
- "Deploy CRM to IIS"

#### 2. Verificar los logs de deploy

```powershell
# Ver el último log de deploy del CRM
$log = Get-ChildItem "C:\DeployScripts\logs\deploy-crm-*.log" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
Get-Content $log.FullName
```

**Deberías ver:**
```
2024-12-30 00:XX:XX - === Starting CRM Deployment ===
2024-12-30 00:XX:XX - Navigating to CRM directory
2024-12-30 00:XX:XX - Installing npm dependencies
2024-12-30 00:XX:XX - Building Nuxt SPA
2024-12-30 00:XX:XX - Stopping IIS website: HomesCRM
2024-12-30 00:XX:XX - Creating backup: C:\inetpub\wwwroot\homes-crm-backup-YYYYMMDD-HHMMSS
2024-12-30 00:XX:XX - Copying build files to website directory
2024-12-30 00:XX:XX - Creating web.config for SPA routing
2024-12-30 00:XX:XX - Starting IIS website: HomesCRM
2024-12-30 00:XX:XX - OK - index.html found
2024-12-30 00:XX:XX - OK - _nuxt assets folder found
2024-12-30 00:XX:XX - === Deployment completed successfully ===
```

#### 3. Verificar que los archivos se deployaron

```powershell
# Verificar archivos del CRM
Get-ChildItem "C:\inetpub\wwwroot\homes-crm" | Select-Object Name, LastWriteTime

# Debería mostrar:
# - index.html
# - 200.html
# - _nuxt (carpeta)
# - web.config
```

#### 4. Verificar que el sitio IIS está corriendo

```powershell
Import-Module WebAdministration

# Ver estado del sitio
Get-Website -Name "HomesCRM"

# Debería mostrar:
# Name       : HomesCRM
# State      : Started
```

#### 5. Verificar que el backup se creó

```powershell
# Ver backups del CRM
Get-ChildItem "C:\inetpub\wwwroot" -Filter "homes-crm-backup-*" | Sort-Object CreationTime -Descending

# Deberías ver al menos 1 backup con fecha/hora reciente
```

---

## 📋 Paso 3: Probar el Sitio en el Navegador

### En el servidor IIS:

```powershell
# Abrir el sitio en el navegador
Start-Process "http://localhost:8080"
```

**Deberías ver:**
- El CRM cargando correctamente
- Sin errores 404
- Navegación funcionando

---

## 🔍 Indicadores de Éxito

### ✅ Todo funcionó si ves:

1. **En GitHub Actions:**
   - ✅ Workflow completado con estado verde
   - ✅ Todos los pasos exitosos
   - ✅ Tiempo de ejecución: ~2-5 minutos

2. **En el Servidor:**
   - ✅ Log de deploy sin errores
   - ✅ Archivos actualizados en `C:\inetpub\wwwroot\homes-crm`
   - ✅ Backup creado
   - ✅ Sitio IIS corriendo

3. **En el Navegador:**
   - ✅ Sitio carga correctamente
   - ✅ Sin errores en consola
   - ✅ Navegación funciona

---

## ❌ Troubleshooting

### Si el workflow no aparece en GitHub Actions:

**Posibles causas:**
1. El runner no está activo
2. El workflow tiene errores de sintaxis
3. El push fue a una rama diferente

**Solución:**
```powershell
# En el servidor IIS, verificar runner
cd C:\actions-runner
.\svc.sh status

# Si no está corriendo, iniciarlo
.\svc.sh start

# Verificar en GitHub que el runner aparece como "Idle"
# https://github.com/nevsiruj/homes/settings/actions/runners
```

### Si el workflow falla:

**Solución:**
1. Ve a GitHub Actions y revisa el error específico
2. Revisa los logs en `C:\DeployScripts\logs\`
3. Verifica que los sitios IIS existan
4. Verifica permisos en los directorios

### Si el sitio no carga:

**Solución:**
```powershell
# Verificar que el sitio existe y está iniciado
Get-Website -Name "HomesCRM"

# Si está detenido, iniciarlo
Start-Website -Name "HomesCRM"

# Verificar que los archivos existen
Test-Path "C:\inetpub\wwwroot\homes-crm\index.html"

# Reciclar el Application Pool
Restart-WebAppPool -Name "HomesCRM"
```

---

## 🎯 Comandos Rápidos de Verificación

### Script de verificación completo:

```powershell
Write-Host "=== Verificacion del Deploy Automatico ===" -ForegroundColor Cyan
Write-Host ""

# 1. Estado del runner
Write-Host "1. Estado del Runner:" -ForegroundColor Yellow
cd C:\actions-runner
.\svc.sh status
Write-Host ""

# 2. Ultimo log de deploy
Write-Host "2. Ultimo Deploy del CRM:" -ForegroundColor Yellow
$log = Get-ChildItem "C:\DeployScripts\logs\deploy-crm-*.log" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if ($log) {
    Write-Host "Log: $($log.Name)" -ForegroundColor Green
    Write-Host "Fecha: $($log.LastWriteTime)" -ForegroundColor Green
    Write-Host ""
    Write-Host "Ultimas 10 lineas:" -ForegroundColor Cyan
    Get-Content $log.FullName -Tail 10
} else {
    Write-Host "No se encontraron logs de deploy" -ForegroundColor Red
}
Write-Host ""

# 3. Estado del sitio IIS
Write-Host "3. Estado del Sitio IIS:" -ForegroundColor Yellow
Import-Module WebAdministration
$site = Get-Website -Name "HomesCRM" -ErrorAction SilentlyContinue
if ($site) {
    Write-Host "Nombre: $($site.Name)" -ForegroundColor Green
    Write-Host "Estado: $($site.State)" -ForegroundColor Green
    Write-Host "Puerto: $($site.bindings.Collection[0].bindingInformation)" -ForegroundColor Green
} else {
    Write-Host "Sitio HomesCRM no encontrado en IIS" -ForegroundColor Red
}
Write-Host ""

# 4. Archivos deployados
Write-Host "4. Archivos Deployados:" -ForegroundColor Yellow
if (Test-Path "C:\inetpub\wwwroot\homes-crm") {
    $files = Get-ChildItem "C:\inetpub\wwwroot\homes-crm" | Select-Object -First 5
    $files | ForEach-Object {
        Write-Host "  - $($_.Name) ($($_.LastWriteTime))" -ForegroundColor White
    }
} else {
    Write-Host "Directorio no encontrado" -ForegroundColor Red
}
Write-Host ""

# 5. Backups
Write-Host "5. Backups Disponibles:" -ForegroundColor Yellow
$backups = Get-ChildItem "C:\inetpub\wwwroot" -Filter "homes-crm-backup-*" -ErrorAction SilentlyContinue | Sort-Object CreationTime -Descending
if ($backups) {
    $backups | Select-Object -First 3 | ForEach-Object {
        Write-Host "  - $($_.Name) ($($_.CreationTime))" -ForegroundColor White
    }
} else {
    Write-Host "No hay backups aun" -ForegroundColor Yellow
}
Write-Host ""

Write-Host "=== Verificacion Completada ===" -ForegroundColor Cyan
```

**Guarda este script como:** `C:\DeployScripts\verificar-deploy.ps1`

---

## 📊 Próximos Pasos

### Si todo funcionó correctamente:

1. ✅ **Ya tienes deploy automático funcionando!**
2. 🎯 **Cada vez que hagas push a `master` con cambios en `crm/`**, se deployará automáticamente
3. 📝 **Revisa los logs** en GitHub Actions para monitorear
4. 🔄 **Haz lo mismo para la API** cuando hagas cambios en `api/`

### Para deployar la API:

```bash
# Hacer un cambio en la API
echo "# Test deploy API" >> api/README.md

# Commit y push
git add .
git commit -m "test: probar deploy automatico de la API"
git push origin master
```

---

## 🎉 ¡Felicidades!

Has configurado exitosamente el deploy automático con GitHub Self-Hosted Runner.

**Ahora tienes:**
- ✅ Deploy automático del CRM
- ✅ Deploy automático de la API (cuando hagas cambios)
- ✅ Backups automáticos
- ✅ Logs detallados
- ✅ Zero-downtime para la API
- ✅ Todo visible en GitHub Actions

**¡Listo para producción!** 🚀
