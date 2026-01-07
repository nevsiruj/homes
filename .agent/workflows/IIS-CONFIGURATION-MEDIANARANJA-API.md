# Configuración de IIS para Media Naranja API

## ✅ Estado del Deployment
Los archivos de la API están correctamente desplegados en: `\\vm06\shared\medianaranja\api`

Archivos verificados:
- ✅ ApiService.dll
- ✅ web.config
- ✅ appsettings.json
- ✅ Todas las dependencias

## ❌ Problema Actual
La API devuelve **404 Not Found** al acceder a `https://medianaranja.vylaris.online/api/Articulo`

Esto indica que **IIS no está configurado correctamente** o el sitio no está apuntando a la ubicación correcta.

## 🔧 Pasos para Configurar IIS

### Opción 1: API como Aplicación dentro del sitio Media Naranja

1. **Abrir IIS Manager en VM06**

2. **Navegar al sitio "medianaranja"**
   - Si no existe, créalo primero

3. **Crear una Aplicación llamada "api"**
   - Click derecho en el sitio "medianaranja" → Add Application
   - Alias: `api`
   - Physical path: `\\vm06\shared\medianaranja\api`
   - Application pool: Seleccionar o crear uno para .NET 6

4. **Configurar el Application Pool**
   - Nombre: `MediaNaranjaAPI` (o el que prefieras)
   - .NET CLR version: **No Managed Code** (importante para .NET 6+)
   - Managed pipeline mode: Integrated
   - Identity: ApplicationPoolIdentity (o la cuenta que tenga permisos)

5. **Verificar permisos**
   - La cuenta del Application Pool debe tener permisos de lectura en `\\vm06\shared\medianaranja\api`
   - Click derecho en la carpeta → Properties → Security
   - Agregar: `IIS AppPool\MediaNaranjaAPI` con permisos de Read & Execute

### Opción 2: API como Sitio Independiente

1. **Crear un nuevo sitio en IIS**
   - Site name: `MediaNaranjaAPI`
   - Physical path: `\\vm06\shared\medianaranja\api`
   - Binding:
     - Type: https
     - Host name: `medianaranja.vylaris.online`
     - Path: `/api` (si es posible) o configurar URL Rewrite

2. **Configurar Application Pool** (igual que arriba)

3. **Configurar SSL**
   - Asignar el certificado SSL correspondiente

## 🧪 Verificación

Después de configurar IIS, verificar:

### 1. Verificar que el sitio está corriendo
```powershell
# En VM06
Import-Module WebAdministration
Get-Website | Where-Object { $_.Name -like "*media*" }
Get-WebApplication | Where-Object { $_.Path -like "*api*" }
```

### 2. Verificar el Application Pool
```powershell
Get-WebAppPoolState -Name "MediaNaranjaAPI"
```

### 3. Probar endpoints
- `https://medianaranja.vylaris.online/api/` (debería devolver algo o 404 específico de la API)
- `https://medianaranja.vylaris.online/api/Articulo` (debería devolver datos o error de la API, no 404 de IIS)

### 4. Revisar logs
- Event Viewer → Windows Logs → Application
- Buscar errores relacionados con ASP.NET Core
- Logs de la aplicación en: `\\vm06\shared\medianaranja\api\logs\` (si están configurados)

## 🔍 Diagnóstico de Problemas Comunes

### Error 404 - Not Found
**Causa**: IIS no encuentra la aplicación o la ruta no está configurada
**Solución**: Verificar que la aplicación/sitio existe en IIS y apunta a la ruta correcta

### Error 500.19 - Configuration Error
**Causa**: Falta el ASP.NET Core Hosting Bundle
**Solución**: Instalar desde https://dotnet.microsoft.com/download/dotnet/6.0
- Buscar "ASP.NET Core Runtime 6.0.x - Windows Hosting Bundle"
- Reiniciar IIS después de instalar: `iisreset`

### Error 500.30 - In-Process Start Failure
**Causa**: El Application Pool no está configurado correctamente
**Solución**: 
- Verificar que .NET CLR version = "No Managed Code"
- Verificar que el Hosting Bundle está instalado

### Error 500.0 - In-Process Handler Load Failure
**Causa**: Falta el módulo AspNetCoreModuleV2
**Solución**: Reinstalar el Hosting Bundle

## 📝 Comandos Útiles PowerShell (ejecutar en VM06)

### Verificar configuración actual
```powershell
# Listar todos los sitios
Get-Website

# Listar todas las aplicaciones
Get-WebApplication

# Ver configuración de un sitio específico
Get-Website -Name "medianaranja" | Select-Object *

# Ver bindings
Get-WebBinding -Name "medianaranja"
```

### Crear aplicación API (si no existe)
```powershell
Import-Module WebAdministration

# Crear Application Pool
New-WebAppPool -Name "MediaNaranjaAPI"
Set-ItemProperty IIS:\AppPools\MediaNaranjaAPI -Name managedRuntimeVersion -Value ""

# Crear aplicación
New-WebApplication -Name "api" -Site "medianaranja" -PhysicalPath "\\vm06\shared\medianaranja\api" -ApplicationPool "MediaNaranjaAPI"
```

### Reiniciar servicios
```powershell
# Reiniciar Application Pool
Restart-WebAppPool -Name "MediaNaranjaAPI"

# Reiniciar sitio
Restart-WebItem "IIS:\Sites\medianaranja"

# Reiniciar IIS completamente
iisreset
```

## 🎯 Configuración Recomendada

**Estructura sugerida en IIS:**

```
medianaranja (Sitio)
├── Physical Path: \\vm06\shared\medianaranja
├── Binding: https://medianaranja.vylaris.online
├── Application Pool: MediaNaranjaCRM (No Managed Code)
│
└── api (Aplicación)
    ├── Physical Path: \\vm06\shared\medianaranja\api
    ├── Application Pool: MediaNaranjaAPI (No Managed Code)
    └── URL: https://medianaranja.vylaris.online/api
```

## 📞 Siguiente Paso

**Acción requerida**: Configurar IIS en VM06 siguiendo los pasos anteriores.

Una vez configurado, la API debería responder correctamente en:
`https://medianaranja.vylaris.online/api/Articulo`
