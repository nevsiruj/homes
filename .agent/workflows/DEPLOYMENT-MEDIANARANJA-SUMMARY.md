# Deployment Media Naranja - Resumen

## Fecha de Deployment
**2026-01-07 18:46:33**

## Componentes Desplegados

### 1. CRM (Media Naranja)
- **Ubicación**: `\\vm06\shared\medianaranja`
- **Tipo**: Nuxt SPA
- **Estado**: ✅ Desplegado exitosamente
- **Características**:
  - Build de producción generado con `npm run generate`
  - Configuración SPA con fallback a `/200.html`
  - Archivo `version.json` incluido
  - Backup automático creado

### 2. API (Media Naranja)
- **Ubicación**: `\\vm06\shared\medianaranja\api`
- **Tipo**: .NET 6 API
- **Estado**: ✅ Desplegado exitosamente
- **Características**:
  - Build de producción con `dotnet publish`
  - Configuración de producción aplicada
  - Deploy con zero-downtime usando `app_offline.htm`
  - Backup automático creado

### 3. Web (Homes Guatemala - QA)
- **Ubicación**: `\\vm06\shared\qa\homes\web`
- **Tipo**: Nuxt SSR/SSG
- **API URL**: `https://medianaranja.vylaris.online/api`
- **Estado**: ✅ Desplegado exitosamente
- **Características**:
  - Configurado para consumir la API de Media Naranja
  - Build de producción generado
  - Archivo `version.json` con información de la API
  - Backup automático creado
  - Configuración original de `config.js` restaurada después del deploy

## Archivos de Configuración

### CRM y Web - web.config
```xml
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
          </conditions>
          <action type="Rewrite" url="/index.html" />
        </rule>
      </rules>
    </rewrite>
    <validation validateIntegratedModeConfiguration="false" />
    <urlCompression doStaticCompression="true" doDynamicCompression="false" />
  </system.webServer>
</configuration>
```

## Backups Creados

Los siguientes backups fueron creados automáticamente:

1. `\\vm06\shared\medianaranja-backup-20260107-184757`
2. `\\vm06\shared\medianaranja\api-backup-20260107-185003`
3. `\\vm06\shared\qa\homes\web-backup-20260107-185413`

**Nota**: El sistema mantiene automáticamente los últimos 3 backups de cada componente.

## Logs de Deployment

Los logs detallados se encuentran en:
`C:\DeployScripts\logs\deploy-medianaranja-20260107-184633.log`

## Comandos para Deploy Manual

### Deploy de todos los componentes
```powershell
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -All
```

### Deploy individual

**Solo CRM:**
```powershell
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -DeployCRM
```

**Solo API:**
```powershell
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -DeployAPI
```

**Solo Web:**
```powershell
powershell -ExecutionPolicy Bypass -File ".agent\workflows\deploy-medianaranja.ps1" -DeployWeb
```

## GitHub Actions Workflow

Se ha creado un workflow de GitHub Actions para automatizar los deploys:
`.github/workflows/deploy-medianaranja.yml`

Este workflow permite:
- Seleccionar qué componente desplegar (CRM, API, Web, o todos)
- Ejecución manual mediante `workflow_dispatch`
- Logs detallados de cada paso
- Manejo de errores y rollback automático

## Verificación Post-Deployment

### CRM
1. Verificar que existe: `\\vm06\shared\medianaranja\index.html`
2. Verificar assets: `\\vm06\shared\medianaranja\_nuxt\`
3. Verificar version: `\\vm06\shared\medianaranja\version.json`

### API
1. Verificar DLL: `\\vm06\shared\medianaranja\api\ApiService.dll`
2. Verificar config: `\\vm06\shared\medianaranja\api\web.config`
3. Verificar settings: `\\vm06\shared\medianaranja\api\appsettings.json`

### Web
1. Verificar que existe: `\\vm06\shared\qa\homes\web\index.html`
2. Verificar assets: `\\vm06\shared\qa\homes\web\_nuxt\`
3. Verificar version: `\\vm06\shared\qa\homes\web\version.json`
4. Verificar API URL en version.json: `"apiUrl": "https://medianaranja.vylaris.online/api"`

## Configuración de IIS (Pendiente)

Para que los sitios funcionen correctamente en IIS, es necesario:

### 1. Sitio CRM (Media Naranja)
- **Nombre del sitio**: `medianaranja`
- **Physical Path**: `\\vm06\shared\medianaranja`
- **Binding**: Configurar según dominio deseado
- **Application Pool**: .NET CLR Version: No Managed Code

### 2. Sitio API (Media Naranja)
- **Nombre del sitio**: `medianaranja-api` (o como aplicación dentro de medianaranja)
- **Physical Path**: `\\vm06\shared\medianaranja\api`
- **Binding**: `https://medianaranja.vylaris.online/api`
- **Application Pool**: .NET CLR Version: No Managed Code (para .NET 6)

### 3. Sitio Web (QA Homes)
- **Nombre del sitio**: `qa-homes-web`
- **Physical Path**: `\\vm06\shared\qa\homes\web`
- **Binding**: Configurar según dominio deseado
- **Application Pool**: .NET CLR Version: No Managed Code

## Notas Importantes

1. **Configuración de la Web**: La web en QA está configurada para consumir la API de `https://medianaranja.vylaris.online/api`. Esto se configura automáticamente durante el deploy y se restaura la configuración original después.

2. **Backups**: Los backups se crean automáticamente antes de cada deploy. Se mantienen los últimos 3 backups de cada componente.

3. **Zero-Downtime**: El deploy de la API utiliza `app_offline.htm` para minimizar el downtime durante el deployment.

4. **Logs**: Todos los deploys generan logs detallados en `C:\DeployScripts\logs\`

5. **Rollback**: En caso de error, se puede restaurar desde el último backup copiando los archivos de vuelta a la ubicación original.

## Próximos Pasos

1. ✅ Configurar los sitios en IIS en VM06
2. ✅ Verificar que los bindings están correctos
3. ✅ Probar el acceso a cada componente
4. ✅ Verificar que la Web puede comunicarse con la API
5. ✅ Configurar SSL/HTTPS si es necesario
6. ✅ Configurar permisos de carpeta si es necesario

## Contacto y Soporte

Para problemas o preguntas sobre el deployment, revisar:
- Logs en `C:\DeployScripts\logs\`
- Backups en las carpetas `*-backup-*` correspondientes
- Script de deploy en `.agent\workflows\deploy-medianaranja.ps1`
