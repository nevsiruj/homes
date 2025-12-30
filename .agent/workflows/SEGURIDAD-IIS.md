# 🔒 Guía de Seguridad para Servidor IIS con IP Pública

## 📋 Índice
1. [Firewall de Windows](#firewall)
2. [Certificados SSL/HTTPS](#ssl)
3. [Hardening de IIS](#iis-hardening)
4. [Protección contra ataques](#proteccion-ataques)
5. [Monitoreo y Logs](#monitoreo)
6. [Backups](#backups)
7. [Actualizaciones](#actualizaciones)

---

## 🔥 1. Firewall de Windows

### Reglas básicas de firewall:

```powershell
# Abrir PowerShell como Administrador

# 1. PERMITIR solo puertos necesarios
# HTTP (80) - Solo si usas redirección a HTTPS
New-NetFirewallRule -DisplayName "HTTP" -Direction Inbound -LocalPort 80 -Protocol TCP -Action Allow

# HTTPS (443) - OBLIGATORIO
New-NetFirewallRule -DisplayName "HTTPS" -Direction Inbound -LocalPort 443 -Protocol TCP -Action Allow

# RDP (3389) - Solo desde IPs específicas
New-NetFirewallRule -DisplayName "RDP Seguro" -Direction Inbound -LocalPort 3389 -Protocol TCP -Action Allow -RemoteAddress "TU_IP_CASA","TU_IP_OFICINA"

# 2. BLOQUEAR todo lo demás (política por defecto)
Set-NetFirewallProfile -Profile Domain,Public,Private -DefaultInboundAction Block -DefaultOutboundAction Allow

# 3. BLOQUEAR puertos peligrosos explícitamente
New-NetFirewallRule -DisplayName "Bloquear SMB" -Direction Inbound -LocalPort 445 -Protocol TCP -Action Block
New-NetFirewallRule -DisplayName "Bloquear RPC" -Direction Inbound -LocalPort 135 -Protocol TCP -Action Block
New-NetFirewallRule -DisplayName "Bloquear NetBIOS" -Direction Inbound -LocalPort 137,138,139 -Protocol TCP -Action Block
```

### Verificar reglas activas:

```powershell
# Ver todas las reglas de firewall
Get-NetFirewallRule | Where-Object {$_.Enabled -eq $true} | Select-Object DisplayName, Direction, Action

# Ver puertos abiertos
Get-NetTCPConnection | Where-Object {$_.State -eq "Listen"} | Select-Object LocalAddress, LocalPort, OwningProcess
```

---

## 🔐 2. Certificados SSL/HTTPS (OBLIGATORIO)

### Opción A: Let's Encrypt (GRATIS) ⭐ Recomendado

```powershell
# 1. Instalar Win-ACME (cliente Let's Encrypt para Windows)
# Descargar desde: https://www.win-acme.com/

# 2. Ejecutar Win-ACME
cd C:\Tools\win-acme
.\wacs.exe

# 3. Seguir el asistente:
# - Opción: N (New certificate)
# - Opción: 1 (Single binding of an IIS site)
# - Seleccionar tu sitio
# - Validación: HTTP-01 (automática)
# - Instalación: Automática en IIS

# 4. Configurar renovación automática (ya viene configurada)
# El certificado se renueva automáticamente cada 60 días
```

### Opción B: Certificado comercial (Pagado)

```powershell
# 1. Generar CSR (Certificate Signing Request)
# En IIS Manager:
# - Server Certificates → Create Certificate Request
# - Completar información de la organización
# - Guardar el archivo .csr

# 2. Comprar certificado en:
# - DigiCert, Sectigo, GlobalSign, etc.
# - Enviar el archivo .csr
# - Recibir el certificado .cer/.crt

# 3. Instalar en IIS:
# - Server Certificates → Complete Certificate Request
# - Seleccionar el archivo .cer
# - Asignar al sitio en Bindings
```

### Configurar HTTPS en IIS:

```powershell
# Importar módulo de IIS
Import-Module WebAdministration

# Agregar binding HTTPS al sitio
New-WebBinding -Name "HomesCRM" -Protocol https -Port 443 -HostHeader "crm.tudominio.com" -SslFlags 1

# Forzar redirección HTTP → HTTPS
# Agregar a web.config:
```

```xml
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="HTTP to HTTPS redirect" stopProcessing="true">
          <match url="(.*)" />
          <conditions>
            <add input="{HTTPS}" pattern="off" ignoreCase="true" />
          </conditions>
          <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

---

## 🛡️ 3. Hardening de IIS

### Configuraciones de seguridad:

```powershell
# 1. Ocultar versión de IIS y ASP.NET
# Editar: C:\Windows\System32\inetsrv\config\applicationHost.config
# Cambiar:
# <httpProtocol>
#   <customHeaders>
#     <remove name="X-Powered-By" />
#   </customHeaders>
# </httpProtocol>

# O vía PowerShell:
Remove-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/httpProtocol/customHeaders" -Name "." -AtElement @{name='X-Powered-By'}

# 2. Deshabilitar métodos HTTP innecesarios
# Agregar a web.config:
```

```xml
<system.webServer>
  <security>
    <requestFiltering>
      <verbs>
        <add verb="TRACE" allowed="false" />
        <add verb="TRACK" allowed="false" />
        <add verb="OPTIONS" allowed="false" />
      </verbs>
    </requestFiltering>
  </security>
</system.webServer>
```

### Configurar límites de requests:

```powershell
# Limitar tamaño de requests (prevenir DoS)
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/security/requestFiltering/requestLimits" -Name "maxAllowedContentLength" -Value 30000000  # 30 MB

# Limitar longitud de URL
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/security/requestFiltering/requestLimits" -Name "maxUrl" -Value 4096

# Limitar query string
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/security/requestFiltering/requestLimits" -Name "maxQueryString" -Value 2048
```

### Deshabilitar Directory Browsing:

```powershell
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/directoryBrowse" -Name "enabled" -Value "False"
```

---

## 🚫 4. Protección contra Ataques

### A. Fail2Ban para Windows (Bloquear IPs maliciosas)

```powershell
# Instalar IPBan (alternativa a Fail2Ban para Windows)
# Descargar desde: https://github.com/DigitalRuby/IPBan

# 1. Descargar e instalar
Invoke-WebRequest -Uri "https://github.com/DigitalRuby/IPBan/releases/latest/download/IPBan-Windows.zip" -OutFile "C:\Temp\IPBan.zip"
Expand-Archive -Path "C:\Temp\IPBan.zip" -DestinationPath "C:\IPBan"

# 2. Configurar ipban.config
# Editar: C:\IPBan\ipban.config
# Configurar:
# - Intentos fallidos: 5
# - Tiempo de ban: 24 horas
# - Whitelist: Tu IP

# 3. Instalar como servicio
sc.exe create IPBan binPath= "C:\IPBan\DigitalRuby.IPBan.exe" start= auto
sc.exe start IPBan
```

### B. Rate Limiting (Limitar requests por IP)

Instalar módulo de IIS:

```powershell
# Instalar Dynamic IP Restrictions
Install-WindowsFeature Web-IP-Security

# Configurar límites
Add-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/security/dynamicIpSecurity/denyByConcurrentRequests" -Name "." -Value @{enabled='true';maxConcurrentRequests='20'}

# Bloquear IP después de muchos requests
Add-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/security/dynamicIpSecurity/denyByRequestRate" -Name "." -Value @{enabled='true';maxRequests='100';requestIntervalInMilliseconds='60000'}
```

### C. Protección contra SQL Injection y XSS

```xml
<!-- Agregar a web.config -->
<system.webServer>
  <security>
    <requestFiltering>
      <denyUrlSequences>
        <add sequence=".." />
        <add sequence=":" />
        <add sequence="\" />
        <add sequence="&lt;" />
        <add sequence="&gt;" />
      </denyUrlSequences>
      <fileExtensions allowUnlisted="true">
        <add fileExtension=".exe" allowed="false" />
        <add fileExtension=".bat" allowed="false" />
        <add fileExtension=".cmd" allowed="false" />
        <add fileExtension=".com" allowed="false" />
      </fileExtensions>
    </requestFiltering>
  </security>
</system.webServer>
```

### D. CORS (Control de acceso)

```xml
<!-- Solo permitir tu dominio -->
<system.webServer>
  <httpProtocol>
    <customHeaders>
      <add name="Access-Control-Allow-Origin" value="https://tudominio.com" />
      <add name="X-Frame-Options" value="SAMEORIGIN" />
      <add name="X-Content-Type-Options" value="nosniff" />
      <add name="X-XSS-Protection" value="1; mode=block" />
      <add name="Referrer-Policy" value="strict-origin-when-cross-origin" />
      <add name="Content-Security-Policy" value="default-src 'self'" />
    </customHeaders>
  </httpProtocol>
</system.webServer>
```

---

## 📊 5. Monitoreo y Logs

### Configurar logs de IIS:

```powershell
# Habilitar logs detallados
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.applicationHost/sites/siteDefaults/logFile" -Name "logFormat" -Value "W3C"

# Configurar campos a loguear
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.applicationHost/sites/siteDefaults/logFile" -Name "logExtFileFlags" -Value "Date,Time,ClientIP,UserName,Method,UriStem,UriQuery,HttpStatus,BytesSent,BytesRecv,TimeTaken,UserAgent,Referer"

# Rotar logs diariamente
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.applicationHost/sites/siteDefaults/logFile" -Name "period" -Value "Daily"
```

### Script de monitoreo de ataques:

```powershell
# Guardar como: C:\Scripts\monitor-attacks.ps1

$logPath = "C:\inetpub\logs\LogFiles\W3SVC1"
$alertEmail = "admin@tudominio.com"

# Buscar IPs con muchos errores 404
$suspiciousIPs = Get-Content "$logPath\*.log" | 
    Select-String "404" | 
    ForEach-Object { ($_ -split " ")[8] } | 
    Group-Object | 
    Where-Object { $_.Count -gt 50 } | 
    Select-Object Name, Count

if ($suspiciousIPs) {
    Write-Host "IPs sospechosas detectadas:"
    $suspiciousIPs | Format-Table
    
    # Bloquear IPs automáticamente
    foreach ($ip in $suspiciousIPs) {
        New-NetFirewallRule -DisplayName "Block $($ip.Name)" -Direction Inbound -RemoteAddress $ip.Name -Action Block
    }
}

# Programar ejecución cada hora
# schtasks /create /tn "Monitor Attacks" /tr "powershell.exe -File C:\Scripts\monitor-attacks.ps1" /sc hourly
```

---

## 💾 6. Backups

### Script de backup automático:

```powershell
# Guardar como: C:\Scripts\backup-iis.ps1

$backupPath = "D:\Backups\IIS"
$date = Get-Date -Format "yyyyMMdd-HHmmss"

# Crear directorio de backup
New-Item -ItemType Directory -Path "$backupPath\$date" -Force

# 1. Backup de configuración de IIS
& "$env:windir\system32\inetsrv\appcmd.exe" add backup "$date"

# 2. Backup de sitios web
$sites = @("C:\inetpub\wwwroot\homes-crm", "C:\inetpub\wwwroot\homes-api")
foreach ($site in $sites) {
    $siteName = Split-Path $site -Leaf
    Compress-Archive -Path $site -DestinationPath "$backupPath\$date\$siteName.zip"
}

# 3. Backup de certificados SSL
Export-PfxCertificate -Cert "Cert:\LocalMachine\My\*" -FilePath "$backupPath\$date\certificates.pfx" -Password (ConvertTo-SecureString -String "TuPasswordSegura" -Force -AsPlainText)

# 4. Limpiar backups antiguos (mantener últimos 7 días)
Get-ChildItem $backupPath | Where-Object { $_.CreationTime -lt (Get-Date).AddDays(-7) } | Remove-Item -Recurse -Force

Write-Host "Backup completado: $backupPath\$date"

# Programar backup diario a las 2 AM
# schtasks /create /tn "IIS Daily Backup" /tr "powershell.exe -File C:\Scripts\backup-iis.ps1" /sc daily /st 02:00
```

---

## 🔄 7. Actualizaciones y Parches

### Mantener Windows actualizado:

```powershell
# Instalar módulo de Windows Update
Install-Module PSWindowsUpdate -Force

# Ver actualizaciones disponibles
Get-WindowsUpdate

# Instalar todas las actualizaciones
Install-WindowsUpdate -AcceptAll -AutoReboot

# Programar actualizaciones automáticas
# Panel de Control → Windows Update → Configurar actualizaciones automáticas
```

### Actualizar .NET y componentes:

```powershell
# Verificar versión de .NET
dotnet --list-runtimes

# Descargar última versión desde:
# https://dotnet.microsoft.com/download
```

---

## 🎯 Checklist de Seguridad

### Configuración Inicial:
- [ ] Firewall configurado (solo puertos 80, 443, 3389)
- [ ] Certificado SSL instalado (Let's Encrypt o comercial)
- [ ] HTTPS forzado (redirección HTTP → HTTPS)
- [ ] Headers de seguridad configurados
- [ ] Directory browsing deshabilitado
- [ ] Métodos HTTP innecesarios bloqueados

### Protección Activa:
- [ ] IPBan instalado y configurado
- [ ] Rate limiting activado
- [ ] Request filtering configurado
- [ ] CORS configurado correctamente

### Monitoreo:
- [ ] Logs de IIS habilitados
- [ ] Script de monitoreo programado
- [ ] Alertas configuradas

### Mantenimiento:
- [ ] Backups automáticos diarios
- [ ] Windows Update automático
- [ ] Revisión mensual de logs
- [ ] Renovación automática de SSL

---

## 🚨 Acciones Inmediatas (Hacer HOY)

```powershell
# 1. Configurar firewall básico
Set-NetFirewallProfile -Profile Domain,Public,Private -DefaultInboundAction Block
New-NetFirewallRule -DisplayName "HTTPS" -Direction Inbound -LocalPort 443 -Protocol TCP -Action Allow

# 2. Instalar certificado SSL
# Usar Win-ACME (Let's Encrypt)

# 3. Ocultar headers
Remove-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.webServer/httpProtocol/customHeaders" -Name "." -AtElement @{name='X-Powered-By'}

# 4. Habilitar logs
Set-WebConfigurationProperty -PSPath 'MACHINE/WEBROOT/APPHOST' -Filter "system.applicationHost/sites/siteDefaults/logFile" -Name "logFormat" -Value "W3C"

# 5. Configurar backup
# Crear y programar script de backup
```

---

## 📚 Recursos Adicionales

- **OWASP Top 10:** https://owasp.org/www-project-top-ten/
- **IIS Security Best Practices:** https://docs.microsoft.com/en-us/iis/
- **Let's Encrypt:** https://letsencrypt.org/
- **Win-ACME:** https://www.win-acme.com/
- **IPBan:** https://github.com/DigitalRuby/IPBan

---

## ⚠️ IMPORTANTE

**Nunca expongas:**
- Puertos de base de datos (1433, 3306, 5432)
- Puertos de administración (8080, 8443)
- RDP sin restricción de IP
- Directorios de configuración

**Siempre usa:**
- HTTPS (nunca HTTP en producción)
- Contraseñas fuertes (mínimo 16 caracteres)
- Autenticación de dos factores (2FA)
- VPN para acceso administrativo

---

¿Necesitas ayuda implementando alguna de estas medidas? 🔒
