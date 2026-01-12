# 🏠 Homes - Sistema de Gestión Inmobiliaria

Sistema completo de gestión inmobiliaria que incluye API backend, CRM administrativo y frontend web público.

## 📋 Estructura del Proyecto

```
homes/
├── 🔧 api/              # API Backend (.NET Core)
├── 👨‍💼 crm/              # CRM Administrativo (Nuxt.js)
├── 🌐 homes-web/        # Frontend Web Público (Nuxt.js)
└── 📄 README.md         # Este archivo
```

## 🌐 Ambientes y Deployment

### 🧪 QA / Media Naranja
Ambiente de pruebas principal.
- **API (Media Naranja)**: `https://medianaranja.vylaris.online/api`
  - *Path*: `\\vm06\shared\medianaranja\api`
- **CRM (Media Naranja)**: `https://medianaranja.vylaris.online`
  - *Path*: `\\vm06\shared\medianaranja`
- **Web Frontend (QA)**: `\\vm06\shared\qa\homes\web`
  - *Script*: `.agent/workflows/deploy-medianaranja.ps1 -DeployWeb`

### 🚀 Producción (App-Pool)
- **API**: `https://app-pool.vylaris.online/homes/api`
  - *Path*: `\\vm06\shared\app-pool\homes\api`
- **CRM**: `https://app-pool.vylaris.online/homes/crm`
  - *Path*: `\\vm06\shared\app-pool\homes\crm`
- **Script**: `.agent/workflows/deploy-production.ps1`

### 🛠️ Documentación de Ambientes
Para más detalles sobre servidores, IPs y configuraciones de IIS, consultar:
- [Guía de Verificación de Deploy](.agent/workflows/COMO-VERIFICAR-DEPLOY.md)
- [Resumen Media Naranja](.agent/workflows/DEPLOYMENT-MEDIANARANJA-SUMMARY.md)

## 🛠️ Tecnologías Utilizadas

### Backend API
- **.NET Core** - Framework principal
- **Entity Framework** - ORM para base de datos
- **SQL Server** - Base de datos
- **JWT Authentication** - Autenticación y autorización

### CRM Administrativo
- **Nuxt.js 3** - Framework Vue.js full-stack
- **Vue.js 3** - Framework frontend reactivo
- **Tailwind CSS** - Framework de estilos
- **Chart.js** - Gráficos y estadísticas

### Frontend Web
- **Nuxt.js 3** - Framework Vue.js full-stack
- **Vue.js 3** - Framework frontend reactivo
- **Tailwind CSS** - Framework de estilos
- **Responsive Design** - Diseño adaptable

## 🚀 Características

### API Backend
- ✅ Gestión de propiedades e inmuebles
- ✅ Sistema de usuarios y agentes
- ✅ API RESTful
- ✅ Autenticación JWT
- ✅ Documentación Swagger

### CRM Administrativo
- ✅ Dashboard con estadísticas
- ✅ Gestión de agentes y clientes
- ✅ Administración de propiedades
- ✅ Sistema de matching
- ✅ Generación de reportes
- ✅ Gestión de tareas y recordatorios

### Frontend Web
- ✅ Catálogo de propiedades
- ✅ Búsqueda avanzada con filtros
- ✅ Páginas de detalle de propiedades
- ✅ Blog inmobiliario
- ✅ Formularios de contacto
- ✅ Diseño responsive

## 📦 Instalación y Configuración

### Prerrequisitos
- **Node.js** (v16 o superior)
- **.NET Core SDK** (v6.0 o superior)
- **SQL Server** o **SQL Server Express**

### Backend API
```bash
cd api
dotnet restore
dotnet run
```

### CRM Administrativo
```bash
cd crm
npm install
npm run dev
```

### Frontend Web
```bash
cd homes-web
npm install
npm run dev
```

## 🌐 URLs de Desarrollo

- **API Backend**: `http://localhost:5000`
- **CRM Admin**: `http://localhost:3000`
- **Web Frontend**: `http://localhost:3001`

## 🔧 Configuración

### Base de Datos
1. Configurar string de conexión en `api/appsettings.json`
2. Ejecutar migraciones: `dotnet ef database update`

### Variables de Entorno
Crear archivos `.env` en cada proyecto con las variables necesarias.

## 👥 Equipo de Desarrollo

- **Desarrollador Principal**: nevsiruj
- **Email**: ngonzalosuarez@gmail.com

## 📄 Licencia

Este proyecto es privado y confidencial.

---

⭐ **¿Te gusta el proyecto?** ¡Dale una estrella en GitHub!