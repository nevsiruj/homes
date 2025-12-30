# 🚀 Guía Rápida: Instalación del GitHub Self-Hosted Runner

## 📋 Requisitos Previos

1. **PowerShell como Administrador**
2. **Token de GitHub** con permisos de `repo` y `workflow`

---

## 🔑 Paso 1: Crear Token de GitHub

1. Ve a: https://github.com/settings/tokens
2. Click en **"Generate new token"** → **"Generate new token (classic)"**
3. Nombre: `Self-Hosted Runner Token`
4. Selecciona los permisos:
   - ✅ `repo` (todos los sub-permisos)
   - ✅ `workflow`
   - ✅ `admin:org` → `read:org` (si es repo de organización)
5. Click en **"Generate token"**
6. **COPIA EL TOKEN** (solo se muestra una vez)

---

## 🖥️ Paso 2: Ejecutar el Script de Instalación

### Opción A: Instalación Interactiva (Recomendada)

```powershell
# 1. Abrir PowerShell como Administrador
# 2. Navegar al directorio del script
cd C:\repos\others\homes\.agent\workflows

# 3. Ejecutar el script (te pedirá el token)
.\install-github-runner.ps1 -GitHubToken "TU_TOKEN_AQUI" -RepoOwner "TU_USUARIO_GITHUB"
```

### Opción B: Con todos los parámetros

```powershell
.\install-github-runner.ps1 `
    -GitHubToken "ghp_xxxxxxxxxxxxxxxxxxxx" `
    -RepoOwner "tu-usuario" `
    -RepoName "homes" `
    -RunnerName "IIS-Server-Runner" `
    -RunnerPath "C:\actions-runner"
```

---

## ✅ Paso 3: Verificar la Instalación

### Verificar que el servicio está corriendo:

```powershell
cd C:\actions-runner
.\svc.sh status
```

**Salida esperada:** `active` o `running`

### Verificar en GitHub:

1. Ve a tu repositorio: `https://github.com/TU_USUARIO/homes`
2. Settings → Actions → Runners
3. Deberías ver tu runner con estado **"Idle"** (verde)

---

## 🔧 Comandos Útiles

### Ver estado del runner:
```powershell
cd C:\actions-runner
.\svc.sh status
```

### Detener el runner:
```powershell
cd C:\actions-runner
.\svc.sh stop
```

### Iniciar el runner:
```powershell
cd C:\actions-runner
.\svc.sh start
```

### Ver logs del runner:
```powershell
Get-Content "C:\actions-runner\_diag\Runner_*.log" -Tail 50
```

### Ver logs en tiempo real:
```powershell
Get-Content "C:\actions-runner\_diag\Runner_*.log" -Wait -Tail 20
```

---

## 🐛 Troubleshooting

### Problema: "El script no se puede ejecutar"

**Solución:**
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Problema: "Error al obtener token de GitHub"

**Causas comunes:**
- Token inválido o expirado
- Token sin permisos correctos
- Usuario/repositorio incorrecto

**Solución:**
1. Verifica que el token sea correcto
2. Verifica que tenga permisos `repo` y `workflow`
3. Verifica que el nombre de usuario y repositorio sean correctos

### Problema: "El servicio no inicia"

**Solución:**
```powershell
# Ver logs detallados
Get-Content "C:\actions-runner\_diag\Runner_*.log" -Tail 100

# Reintentar instalación del servicio
cd C:\actions-runner
.\svc.sh uninstall
.\svc.sh install
.\svc.sh start
```

### Problema: Runner aparece "Offline" en GitHub

**Solución:**
```powershell
# Verificar que el servicio esté corriendo
Get-Service -Name "actions.runner.*"

# Si no está corriendo, iniciarlo
cd C:\actions-runner
.\svc.sh start
```

---

## 🔄 Desinstalar el Runner

Si necesitas desinstalar completamente el runner:

```powershell
# 1. Detener el servicio
cd C:\actions-runner
.\svc.sh stop

# 2. Desinstalar el servicio
.\svc.sh uninstall

# 3. Remover el runner de GitHub
.\config.cmd remove --token "TU_TOKEN_AQUI"

# 4. Eliminar el directorio (opcional)
cd C:\
Remove-Item -Path "C:\actions-runner" -Recurse -Force
```

---

## 📊 Estructura de Directorios Creada

```
C:\
├── actions-runner\              # Runner de GitHub
│   ├── _work\                   # Directorio de trabajo (código clonado)
│   │   └── homes\
│   │       └── homes\           # Tu repositorio
│   │           ├── crm\
│   │           ├── api\
│   │           └── homes-web\
│   ├── _diag\                   # Logs del runner
│   └── config.cmd               # Configuración
│
└── DeployScripts\               # Scripts de deploy
    ├── logs\                    # Logs de deploys
    ├── deploy-crm.ps1          # (crear después)
    └── deploy-api.ps1          # (crear después)
```

---

## 🎯 Próximos Pasos

Después de instalar el runner:

1. ✅ **Verificar** que aparece en GitHub como "Idle"
2. 📝 **Crear** scripts de deploy (`deploy-crm.ps1`, `deploy-api.ps1`)
3. 🔄 **Crear** workflows de GitHub Actions (`.github/workflows/`)
4. 🧪 **Probar** con un push de prueba

---

## 📞 Soporte

Si tienes problemas:

1. Revisa los logs: `C:\actions-runner\_diag\Runner_*.log`
2. Verifica el estado del servicio: `.\svc.sh status`
3. Consulta la documentación oficial: https://docs.github.com/en/actions/hosting-your-own-runners

---

## ⚙️ Parámetros del Script

| Parámetro | Descripción | Requerido | Default |
|-----------|-------------|-----------|---------|
| `GitHubToken` | Token de GitHub con permisos | ✅ Sí | - |
| `RepoOwner` | Usuario/organización de GitHub | ❌ No | "TU_USUARIO" |
| `RepoName` | Nombre del repositorio | ❌ No | "homes" |
| `RunnerName` | Nombre del runner | ❌ No | "IIS-Server-Runner" |
| `RunnerPath` | Ruta de instalación | ❌ No | "C:\actions-runner" |
| `RunnerVersion` | Versión del runner | ❌ No | "2.321.0" |

---

## 🔐 Seguridad

- ✅ El token se usa solo durante la instalación
- ✅ El runner se ejecuta como servicio de Windows
- ✅ No expone puertos al internet
- ✅ Solo ejecuta workflows de tu repositorio
- ⚠️ **NUNCA** compartas tu token de GitHub

---

¡Listo! Ahora puedes instalar el runner con un solo comando. 🚀
