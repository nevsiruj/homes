# 🌐 Homes - Guía de Ambientes (Source of Truth)

Este documento es la referencia principal para que cualquier desarrollador o IA identifique los ambientes de QA y Producción del proyecto **Homes**.

## 🏗️ Servidor y Red
- **Host**: `VM06`
- **Shared Folder Base**: `\\vm06\shared`
- **Ubicación de Scripts**: `.agent/workflows/`

---

## 🧪 Ambiente: QA / Media Naranja
Utilizado para pruebas funcionales y validación del cliente.

### 🧩 API (Backend)
- **URL**: `https://medianaranja.vylaris.online/api`
- **Directorio de Deploy**: `\\vm06\shared\medianaranja\api`
- **Tecnología**: .NET 6 API
- **Script de Deploy**: `.agent/workflows/deploy-medianaranja.ps1 -DeployAPI`

### 📊 CRM (Admin)
- **URL**: `https://medianaranja.vylaris.online`
- **Directorio de Deploy**: `\\vm06\shared\medianaranja`
- **Tecnología**: Nuxt SPA
- **Script de Deploy**: `.agent/workflows/deploy-medianaranja.ps1 -DeployCRM`

### 🌐 Web (Frontend Público)
- **Directorio de Deploy**: `\\vm06\shared\qa\homes\web`
- **Tecnología**: Nuxt SSR/SSG
- **Conexión**: Consume la API de Media Naranja.
- **Script de Deploy**: `.agent/workflows/deploy-medianaranja.ps1 -DeployWeb`

---

## 🚀 Ambiente: Producción
Ambiente final para los usuarios.

### 🧩 API (Backend)
- **URL**: `https://app-pool.vylaris.online/homes/api`
- **Directorio de Deploy**: `\\vm06\shared\app-pool\homes\api`
- **Script de Deploy**: `.agent/workflows/deploy-production.ps1 -DeployAPI`

### 📊 CRM (Admin)
- **URL**: `https://app-pool.vylaris.online/homes/crm`
- **Directorio de Deploy**: `\\vm06\shared\app-pool\homes\crm`
- **Script de Deploy**: `.agent/workflows/deploy-production.ps1 -DeployCRM`

---

## 🛠️ Herramientas de Diagnóstico
- **Logs de Deploy**: `C:\DeployScripts\logs\`
- **Backups**: Cada deploy genera una carpeta `-backup-YYYYMMDD-HHMMSS` en el directorio compartido. Se mantienen los últimos 3.
- **Verificación**: Usar script `.agent/workflows/COMO-VERIFICAR-DEPLOY.md`.
