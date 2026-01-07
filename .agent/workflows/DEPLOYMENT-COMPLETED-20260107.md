# ✅ Deployment Completado - Media Naranja

**Fecha**: 2026-01-07 19:17:45 - 19:25:58
**Duración**: ~8 minutos
**Estado**: ✅ EXITOSO

---

## 📦 Componentes Desplegados

### 1. ✅ CRM (Media Naranja)
- **Ubicación**: `\\vm06\shared\medianaranja`
- **Build**: Nuxt SPA (producción)
- **Estado**: Desplegado y verificado
- **Archivos clave**:
  - ✅ `index.html`
  - ✅ `200.html` (SPA fallback)
  - ✅ `_nuxt/` (assets)
  - ✅ `web.config`
  - ✅ `version.json`

### 2. ✅ API (Media Naranja)
- **Ubicación**: `\\vm06\shared\medianaranja\api`
- **Build**: .NET 6 API (Release)
- **Estado**: Desplegado y verificado
- **Archivos clave**:
  - ✅ `ApiService.dll`
  - ✅ `web.config`
  - ✅ `appsettings.json`
  - ✅ `appsettings.Production.json`
  - ✅ Todas las dependencias

### 3. ✅ Web (Homes Guatemala - QA)
- **Ubicación**: `\\vm06\shared\qa\homes\web`
- **Build**: Nuxt SSG (producción)
- **API URL**: `https://medianaranja.vylaris.online/api` ✅
- **Estado**: Desplegado y verificado
- **Archivos clave**:
  - ✅ `index.html`
  - ✅ `_nuxt/` (assets)
  - ✅ `web.config`
  - ✅ `version.json` (con apiUrl configurado)

---

## 📊 Información de Versión

### CRM
```json
{
  "version": "2026.01.07.1919",
  "deployDate": "2026-01-07 19:19:00"
}
```

### API
- **Build**: Release
- **Framework**: .NET 6.0
- **Hosting Model**: InProcess

### Web
```json
{
  "version": "2026.01.07.1925",
  "deployDate": "2026-01-07 19:25:58",
  "apiUrl": "https://medianaranja.vylaris.online/api"
}
```

---

## 🔄 Backups Creados

Los siguientes backups fueron creados automáticamente:

1. **CRM**: `\\vm06\shared\medianaranja-backup-20260107-191900`
2. **API**: `\\vm06\shared\medianaranja\api-backup-20260107-192049`
3. **Web**: `\\vm06\shared\qa\homes\web-backup-20260107-192356`

> **Nota**: El sistema mantiene automáticamente los últimos 3 backups de cada componente.

---

## ✅ Verificaciones Realizadas

### CRM
- ✅ `index.html` existe
- ✅ Directorio `_nuxt` con assets
- ✅ `web.config` configurado para SPA
- ✅ `version.json` creado

### API
- ✅ `ApiService.dll` existe
- ✅ `web.config` generado por dotnet publish
- ✅ `appsettings.json` presente
- ✅ Todas las dependencias copiadas

### Web
- ✅ `index.html` existe
- ✅ Directorio `_nuxt` con assets
- ✅ `web.config` configurado
- ✅ `version.json` con `apiUrl` correcto

---

## 🎯 Configuración de la Web

La aplicación web está configurada para consumir la API de Media Naranja:

**API URL**: `https://medianaranja.vylaris.online/api`

Esta configuración se aplicó automáticamente durante el deployment y está registrada en el archivo `version.json`.

---

## 📝 Logs de Deployment

**Archivo de log**: `C:\DeployScripts\logs\deploy-medianaranja-20260107-191745.log`

### Resumen de eventos:
- ✅ CRM Build completado exitosamente
- ✅ CRM desplegado en `\\vm06\shared\medianaranja`
- ✅ API Build completado exitosamente
- ✅ API desplegada en `\\vm06\shared\medianaranja\api`
- ✅ Web Build completado exitosamente
- ✅ Web desplegada en `\\vm06\shared\qa\homes\web`

---

## 🔧 Próximos Pasos - Configuración de IIS

### ⚠️ IMPORTANTE: Los archivos están desplegados pero IIS necesita configuración

Para que los sitios funcionen correctamente, necesitas configurar IIS en VM06:

### 1. CRM (Media Naranja)
```powershell
# Crear o verificar sitio en IIS
Import-Module WebAdministration

# Si el sitio no existe, créalo
New-Website -Name "medianaranja" `
    -PhysicalPath "\\vm06\shared\medianaranja" `
    -ApplicationPool "MediaNaranjaCRM" `
    -HostHeader "medianaranja.vylaris.online"

# Configurar Application Pool
New-WebAppPool -Name "MediaNaranjaCRM"
Set-ItemProperty IIS:\AppPools\MediaNaranjaCRM -Name managedRuntimeVersion -Value ""
```

### 2. API (Media Naranja)
```powershell
# Crear Application Pool para la API
New-WebAppPool -Name "MediaNaranjaAPI"
Set-ItemProperty IIS:\AppPools\MediaNaranjaAPI -Name managedRuntimeVersion -Value ""

# Crear aplicación API dentro del sitio medianaranja
New-WebApplication -Name "api" `
    -Site "medianaranja" `
    -PhysicalPath "\\vm06\shared\medianaranja\api" `
    -ApplicationPool "MediaNaranjaAPI"

# Reiniciar
Restart-WebAppPool -Name "MediaNaranjaAPI"
```

### 3. Web (QA Homes)
```powershell
# Crear sitio para QA
New-Website -Name "qa-homes-web" `
    -PhysicalPath "\\vm06\shared\qa\homes\web" `
    -ApplicationPool "QAHomesWeb" `
    -Port 80

# Configurar Application Pool
New-WebAppPool -Name "QAHomesWeb"
Set-ItemProperty IIS:\AppPools\QAHomesWeb -Name managedRuntimeVersion -Value ""
```

### 4. Reiniciar IIS
```powershell
iisreset
```

---

## 🧪 Verificación Post-Configuración

Una vez configurado IIS, verificar:

### CRM
- Acceder a: `https://medianaranja.vylaris.online`
- Verificar que carga la aplicación Nuxt
- Verificar que el routing funciona (SPA)

### API
- Acceder a: `https://medianaranja.vylaris.online/api/Articulo`
- Debería devolver datos JSON (no 404)
- Verificar otros endpoints

### Web
- Acceder al sitio QA configurado
- Verificar que puede comunicarse con la API
- Abrir DevTools → Network → verificar llamadas a `https://medianaranja.vylaris.online/api`

---

## 📞 Comandos Útiles

### Ver estado de los sitios
```powershell
Get-Website | Where-Object { $_.Name -like "*media*" -or $_.Name -like "*qa*" }
```

### Ver aplicaciones
```powershell
Get-WebApplication | Where-Object { $_.Path -like "*api*" }
```

### Reiniciar un sitio específico
```powershell
Restart-WebItem "IIS:\Sites\medianaranja"
```

### Ver logs de errores
```powershell
Get-EventLog -LogName Application -Source "ASP.NET*" -Newest 20
```

---

## 🔄 Re-deployment

Para volver a desplegar en el futuro:

### Todos los componentes
```powershell
cd C:\repos\others\homes
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -All
```

### Solo un componente
```powershell
# Solo CRM
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -DeployCRM

# Solo API
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -DeployAPI

# Solo Web
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -DeployWeb
```

---

## 📚 Documentación Adicional

- **Script de deployment**: `.agent\workflows\deploy-medianaranja.ps1`
- **Guía de configuración IIS**: `.agent\workflows\IIS-CONFIGURATION-MEDIANARANJA-API.md`
- **GitHub Actions workflow**: `.github\workflows\deploy-medianaranja.yml`

---

## ✅ Estado Final

| Componente | Build | Deploy | IIS Config | Estado |
|------------|-------|--------|------------|--------|
| CRM        | ✅    | ✅     | ⏳ Pendiente | Listo para configurar |
| API        | ✅    | ✅     | ⏳ Pendiente | Listo para configurar |
| Web        | ✅    | ✅     | ⏳ Pendiente | Listo para configurar |

**Próximo paso**: Configurar IIS en VM06 siguiendo los comandos anteriores.

---

**Deployment completado exitosamente** ✅
