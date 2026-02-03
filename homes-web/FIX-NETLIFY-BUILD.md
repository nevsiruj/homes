# Fix Build Netlify - SSR Híbrido

## ❌ Problema Original

El build en Netlify fallaba con estos errores:
```
[500] Server Error - /propiedades/zona/zona-14
[500] Server Error - /propiedades/zona/zona-10  
[500] Server Error - /propiedades/zona/carretera-a-el-salvador
```

**Causa**: El comando `npm run generate` intentaba pre-renderizar TODAS las páginas en build time, pero las páginas dinámicas de zonas e inmuebles requieren datos de la API que no está disponible durante el build.

## ✅ Solución Implementada

### 1. **Cambio de Estrategia: Static → SSR Híbrido**

#### netlify.toml - Cambios:
```toml
[build]
  publish = ".output/public"  # Cambió de "dist"
  command = "npm install && npm run build"  # Cambió de "generate"
```

✅ **Por qué**: 
- `npm run build` → SSR híbrido (pre-render + SSR on-demand)
- `npm run generate` → Todo estático (falla si API no responde)

### 2. **Configuración Nitro para Netlify SSR**

#### nuxt.config.ts - Cambios:
```typescript
nitro: {
  preset: 'netlify',  // Soporte SSR en Netlify
  prerender: {
    failOnError: false,  // No fallar build por errores de pre-render
    routes: [
      '/',
      '/propiedades',
      '/nosotros',
      // ... páginas estáticas
    ],
    ignore: [
      '/inmueble/**',      // SSR on-demand
      '/propiedades/zona/**',  // SSR on-demand
      '/blog/**',
      '/proyecto/**'
    ]
  }
}
```

✅ **Resultado**:
- Páginas estáticas: Pre-renderizadas en build time (rápidas, SEO garantizado)
- Páginas dinámicas: Renderizadas on-demand en el servidor (SSR)

### 3. **Headers Corregidos**

Antes (netlify.toml):
```toml
[[headers]]
  for = "/_nuxt/*"
  [headers.values]
    Cache-Control = "..."  # ❌ Duplicado
    cache-control = "..."  # ❌ Duplicado
```

Después:
```toml
[[headers]]
  for = "/_nuxt/*"
  [headers.values]
    cache-control = "public, max-age=31536000, immutable"  # ✅ Solo uno

[[headers]]
  for = "/inmueble/*"
  [headers.values]
    cache-control = "public, max-age=3600, must-revalidate"
    X-Robots-Tag = "index, follow"
```

### 4. **Páginas de Zona - Client-side Fetch**

#### [zona].vue - Cambio:
```javascript
const { data: propiedades } = await useAsyncData(
  `zona-${zona}`,
  async () => { /* ... */ },
  {
    server: false,  // ✅ Solo ejecutar en cliente
    lazy: true,     // ✅ No bloquear render
    default: () => []
  }
);
```

✅ **Por qué**: Evita que el build falle si la API no responde durante la generación.

## 🚀 Despliegue a Netlify

### Opción A: Push a Git (Recomendado)
```bash
git add .
git commit -m "Fix: Cambiar a SSR híbrido para Netlify"
git push origin master
```

Netlify detectará automáticamente y ejecutará:
```bash
npm install && npm run build
```

### Opción B: Deploy Manual
```bash
# Localmente
npm run build

# Resultado en: .output/public/
```

## 📊 Comparación: Generate vs Build

| Aspecto | `generate` (antes) | `build` (ahora) |
|---------|-------------------|-----------------|
| **Tipo** | Static Site Generation (SSG) | SSR Híbrido |
| **Páginas dinámicas** | ❌ Deben existir en build | ✅ Renderizadas on-demand |
| **Requiere API** | ✅ Sí, en build time | ⚠️ Solo para páginas pre-renderizadas |
| **Metadatos OG** | ⚠️ Solo en páginas pre-renderizadas | ✅ En todas las páginas (SSR) |
| **Velocidad** | 🚀 Máxima (todo HTML estático) | ⚡ Rápida (SSR optimizado) |
| **SEO** | ✅ Excelente en páginas pre-renderizadas | ✅ Excelente en todas |
| **Costo Netlify** | 💰 Gratuito | 💰 Gratuito (con límites) |

## 🔍 Verificación Post-Deploy

### 1. Verificar páginas estáticas (pre-renderizadas):
```bash
curl -I https://homesguatemala.com/
# Debe responder 200 OK instantáneamente
```

### 2. Verificar páginas SSR (inmuebles):
```bash
curl -I https://homesguatemala.com/inmueble/casa-en-zona-10
# Debe responder 200 OK (puede tardar 100-300ms primera vez)
```

### 3. Facebook Debugger:
```
https://developers.facebook.com/tools/debug/
```
- Pegar URL de un inmueble
- Click "Depurar"
- Verificar metadatos OG presentes

### 4. Ver logs de Netlify:
```
https://app.netlify.com/sites/[TU-SITE]/deploys
```

## ⚠️ Notas Importantes

### Límites de Netlify (Plan Gratuito):
- **125,000** requests/mes
- **100 GB** bandwidth/mes
- **300 minutos** build/mes

### Optimizaciones Aplicadas:
1. ✅ Pre-render de páginas estáticas (/, /propiedades, etc.)
2. ✅ SSR on-demand para páginas dinámicas (/inmueble/*)
3. ✅ Cache agresivo para assets (_nuxt/*, images/*)
4. ✅ Middleware para detectar bots (SSR completo para Facebook/WhatsApp)
5. ✅ Headers de seguridad y SEO

### Si el build aún falla:

1. **Verificar API está accesible**:
```bash
curl https://app-pool.vylaris.online/api/Inmueble/by-slug/test
```

2. **Ver logs completos en Netlify**:
```
Settings → Build & deploy → Build settings → Build logs
```

3. **Probar build localmente**:
```bash
npm run build
# Ver si hay errores locales
```

## 📝 Comandos Útiles

```bash
# Desarrollo local
npm run dev

# Build para producción (SSR)
npm run build

# Preview del build
npm run preview

# Generar sitio estático (solo si necesitas 100% estático)
npm run generate

# Limpiar cache
rm -rf .nuxt .output node_modules/.cache
npm install
```

## 🎯 Próximos Pasos

1. ✅ Deploy a Netlify con `npm run build`
2. ✅ Verificar que las páginas de inmuebles funcionan
3. ✅ Probar metadatos en Facebook Debugger
4. ✅ Monitorear logs de Netlify primeros días
5. 📊 Revisar analytics (Netlify Analytics o Google Analytics)

## 🔗 Referencias

- [Nuxt Deployment: Netlify](https://nuxt.com/docs/getting-started/deployment#netlify)
- [Nitro Presets](https://nitro.unjs.io/deploy/providers/netlify)
- [Facebook Sharing Debugger](https://developers.facebook.com/tools/debug/)
