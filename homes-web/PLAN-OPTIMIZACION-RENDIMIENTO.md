# 🚀 Plan de Optimización de Rendimiento - Homes Guatemala

## Análisis Realizado

El análisis de la página en `http://localhost:3001` reveló los siguientes problemas de rendimiento:

### 🔴 Problemas Críticos Detectados

| Problema | Impacto | Ubicación |
|----------|---------|-----------|
| **Over-fetching de API** | Alto | `[slug].vue:793`, `[zona].vue:105` |
| **Splash screen prolongado** | Alto | ~5-8 segundos |
| **Imágenes no optimizadas** | Medio | Componentes de tarjetas |
| **Múltiples llamadas redundantes** | Alto | Propiedades sugeridas |

---

## 📋 Optimizaciones Recomendadas

### 1. 🔥 CRÍTICO: Reducir Over-Fetching de API

**Problema:** Se están cargando 200 propiedades solo para mostrar 3 sugerencias.

**Archivos afectados:**
- `pages/inmueble/[slug].vue` (línea 793)
- `pages/propiedades/zona/[zona].vue` (línea 105)

**Solución:**
```javascript
// ANTES (malo):
const responseData = await inmuebleService.getInmueblesPaginados(1, 200);

// DESPUÉS (optimizado):
const responseData = await inmuebleService.getInmueblesPaginados(1, 20);
// O mejor aún, crear un endpoint específico para sugerencias
```

**Impacto:** Reducción de ~90% en datos transferidos

---

### 2. 🖼️ Optimizar Carga de Imágenes

**Estado actual:** El módulo `@nuxt/image-edge` está instalado pero no se usa consistentemente.

**Implementación:**
```vue
<!-- ANTES -->
<img :src="imagen.url" class="w-full h-64 object-cover" />

<!-- DESPUÉS -->
<NuxtImg 
  :src="imagen.url" 
  width="400" 
  height="256"
  format="webp"
  loading="lazy"
  quality="80"
  class="w-full h-64 object-cover" 
/>
```

**Configuración adicional en `nuxt.config.ts`:**
```typescript
image: {
  domains: ['app-pool.vylaris.online'],
  format: ['webp', 'avif'],
  quality: 80,
  presets: {
    card: {
      modifiers: { width: 400, height: 256, format: 'webp' }
    },
    thumbnail: {
      modifiers: { width: 150, height: 150, format: 'webp' }
    }
  }
}
```

---

### 3. ⚡ Implementar Caché de Estado Global (Pinia)

**Crear store para propiedades:**

```typescript
// stores/propiedadesStore.ts
import { defineStore } from 'pinia';

export const usePropiedadesStore = defineStore('propiedades', {
  state: () => ({
    propiedadesCache: new Map(),
    lastFetch: null,
    cacheDuration: 5 * 60 * 1000, // 5 minutos
  }),
  
  actions: {
    async getPropiedades(filters, forceRefresh = false) {
      const cacheKey = JSON.stringify(filters);
      const cached = this.propiedadesCache.get(cacheKey);
      
      if (!forceRefresh && cached && Date.now() - cached.timestamp < this.cacheDuration) {
        return cached.data;
      }
      
      const data = await inmuebleService.getInmueblesPaginados(...);
      this.propiedadesCache.set(cacheKey, { data, timestamp: Date.now() });
      return data;
    }
  }
});
```

---

### 4. 🎨 Optimizar el Splash Screen / Loading

**Problema:** El splash screen bloquea el contenido por 5-8 segundos.

**Solución - Skeleton Loading Progresivo:**
```vue
<!-- En lugar de un splash screen global, usar skeletons por componente -->
<template>
  <div v-if="pending">
    <PropertyCardSkeleton v-for="n in 9" :key="n" />
  </div>
  <div v-else>
    <!-- Contenido real -->
  </div>
</template>
```

---

### 5. 🔄 Optimizar Watchers Duplicados

**Problema en `propiedades/index.vue`:** Múltiples watchers que disparan la misma función.

**Solución:** Consolidar en un solo watcher:
```javascript
// ANTES: Múltiples watchers separados
watch(() => route.query.CodigoPropiedad, ...);
watch(() => route.query.propertyCode, ...);
watch([...muchos queries...], ...);

// DESPUÉS: Un solo watcher unificado
watch(
  () => route.query,
  () => fetchInmuebles(),
  { immediate: true, deep: true }
);
```

---

### 6. 📦 Lazy Loading de Componentes

**Implementación:**
```javascript
// En páginas que usan componentes pesados
const InmuebleCard = defineAsyncComponent(() => 
  import('~/components/InmuebleCard.vue')
);

const Swiper = defineAsyncComponent(() => 
  import('swiper/vue').then(m => m.Swiper)
);
```

---

### 7. ⚙️ Configuración de Vite para Producción

**Agregar en `nuxt.config.ts`:**
```typescript
vite: {
  build: {
    // Code splitting optimizado
    rollupOptions: {
      output: {
        manualChunks: {
          'swiper': ['swiper'],
          'vendor': ['vue', 'vue-router', 'pinia'],
        }
      }
    },
    // Minificación agresiva
    minify: 'terser',
    terserOptions: {
      compress: {
        drop_console: true,
        drop_debugger: true
      }
    }
  }
}
```

---

### 8. 🌐 Preconexiones y DNS Prefetch

**Ya implementado en `nuxt.config.ts` ✅:**
```typescript
link: [
  { rel: "preconnect", href: "https://app-pool.vylaris.online" },
  { rel: "dns-prefetch", href: "https://app-pool.vylaris.online" },
]
```

**Agregar también:**
```typescript
{ rel: "preconnect", href: "https://fonts.googleapis.com" },
{ rel: "preconnect", href: "https://fonts.gstatic.com", crossorigin: true },
```

---

### 9. 🗜️ Compresión de Assets

**Ya habilitado ✅:**
```typescript
nitro: {
  compressPublicAssets: true,
}
```

**Agregar para mejor compresión:**
```typescript
nitro: {
  compressPublicAssets: {
    gzip: true,
    brotli: true,
  },
  minify: true,
}
```

---

### 10. 📊 Eliminar Console.logs en Producción

**Archivos con console.log activos:**
- `services/inmuebleService.js` (línea 76)
- `pages/propiedades/index.vue` (líneas 369, 404)

**Solución:** Usar el terser config anterior o crear un plugin de Vite.

---

## 📈 Priorización de Implementación

| Prioridad | Optimización | Esfuerzo | Impacto |
|-----------|--------------|----------|---------|
| 🔴 1 | Reducir PageSize de 200 a 20 | Bajo | Alto |
| 🔴 2 | Usar `<NuxtImg>` con lazy loading | Medio | Alto |
| 🟡 3 | Implementar store Pinia para caché | Medio | Medio |
| 🟡 4 | Consolidar watchers | Bajo | Medio |
| 🟢 5 | Lazy loading de componentes | Bajo | Bajo |
| 🟢 6 | Eliminar console.logs | Bajo | Bajo |

---

## ⚡ Implementación Inmediata Sugerida

Para ver mejoras inmediatas, recomiendo empezar con:

1. **Cambiar PageSize=200 → PageSize=20** (5 minutos)
2. **Agregar `loading="lazy"` a imágenes** (10 minutos)
3. **Consolidar watchers duplicados** (15 minutos)

¿Deseas que implemente alguna de estas optimizaciones?
