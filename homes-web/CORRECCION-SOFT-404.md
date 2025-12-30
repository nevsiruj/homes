# 🔧 CORRECCIÓN PENDIENTE: Soft 404 en Páginas Dinámicas

## 📋 Problema Identificado

**308 páginas con Soft 404** en Google Search Console:
- `/inmueble/*` (propiedades)
- `/proyecto/*` (proyectos)
- `/busqueda` (búsqueda)

### Causa Raíz:
Las páginas dinámicas que no existen están devolviendo **código 200** en lugar de **404**, lo que Google interpreta como "Soft 404".

---

## 🔧 Solución Requerida

### Archivo: `pages/inmueble/[slug].vue`

**Líneas 442-461** - Modificar la lógica de `useAsyncData`:

**ANTES (Incorrecto):**
```javascript
} = await useAsyncData(`inmueble-${slug}`, async () => {
  try {
    const data = await inmuebleService.getInmuebleBySlug(slug);
    if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
      return null;  // ❌ ESTO CAUSA SOFT 404
    }
    // ... resto del código
  } catch (err) {
    throw createError({
      statusCode: 404,
      statusMessage: "Inmueble no encontrado",
      fatal: true,
    });
  }
});
```

**DESPUÉS (Correcto):**
```javascript
} = await useAsyncData(`inmueble-${slug}`, async () => {
  try {
    const data = await inmuebleService.getInmuebleBySlug(slug);
    
    // ✅ LANZAR ERROR 404 CUANDO NO HAY DATOS
    if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
      throw createError({
        statusCode: 404,
        statusMessage: "Propiedad no encontrada",
        fatal: true,
      });
    }
    
    // Normalización de datos
    if (data.imagenesReferencia && data.imagenesReferencia.$values) {
      data.imagenesReferencia = data.imagenesReferencia.$values;
    }
    if (data.amenidades && data.amenidades.$values) {
      data.amenidades = data.amenidades.$values;
    }
    return data;
  } catch (err) {
    // Si ya es un error de Nuxt, re-lanzarlo
    if (err.statusCode) {
      throw err;
    }
    // Si es otro error, convertirlo a 404
    throw createError({
      statusCode: 404,
      statusMessage: "Inmueble no encontrado",
      fatal: true,
    });
  }
});
```

---

## 📁 Archivos Similares a Revisar

### 1. `pages/proyecto/[slug].vue`
Aplicar la misma corrección para proyectos.

### 2. `pages/busqueda/index.vue` (si existe)
Verificar que devuelva 200 solo cuando hay resultados.

---

## 🎯 Impacto Esperado

### Antes:
- 308 páginas con Soft 404
- Google las considera "vacías"
- No se indexan

### Después:
- Páginas que no existen: **404 real**
- Páginas que existen: **200 OK**
- Google las indexa correctamente
- Soft 404 bajan de 308 a ~0

---

## 📝 Pasos para Implementar

### Opción 1: Manual (Recomendado)
1. Abrir `pages/inmueble/[slug].vue` en VS Code
2. Ir a la línea 445
3. Cambiar `return null;` por el bloque de `throw createError`
4. Guardar
5. Repetir para `pages/proyecto/[slug].vue`

### Opción 2: Buscar y Reemplazar
1. Presionar `Ctrl + H` en VS Code
2. Buscar: `return null;` (dentro de useAsyncData)
3. Reemplazar por el código correcto
4. Verificar cada ocurrencia manualmente

---

## ✅ Verificación

Después de implementar:

1. **Probar localmente:**
   ```bash
   # Ir a una URL que no existe
   http://localhost:3001/inmueble/propiedad-que-no-existe
   
   # Debe mostrar la página de error.vue con código 404
   ```

2. **Verificar en producción:**
   - Hacer deployment
   - Esperar 1-2 semanas
   - Revisar Google Search Console
   - Los Soft 404 deberían bajar significativamente

---

## 🚨 IMPORTANTE

Esta corrección es **CRÍTICA** para SEO porque:
- Afecta 308 páginas (12.6% del total de problemas)
- Google está desperdiciando crawl budget
- Las propiedades reales no se están indexando

**Prioridad:** 🔴 ALTA  
**Tiempo estimado:** 15 minutos  
**Dificultad:** Baja

---

**Creado:** 23 de Diciembre 2025, 01:25 AM  
**Estado:** ⏳ PENDIENTE DE IMPLEMENTACIÓN
