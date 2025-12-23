# ⚡ CORRECCIÓN URGENTE - SOFT 404

## 🎯 ACCIÓN REQUERIDA AHORA

Google está validando las páginas. Necesitamos corregir el código ANTES de que termine la validación.

---

## 📝 INSTRUCCIONES PASO A PASO

### Archivo 1: `pages/inmueble/[slug].vue`

1. **Abrir el archivo:**
   - Presiona `Ctrl + P`
   - Escribe: `inmueble/[slug].vue`
   - Presiona Enter

2. **Ir a la línea 445:**
   - Presiona `Ctrl + G`
   - Escribe: `445`
   - Presiona Enter

3. **Buscar este código:**
   ```javascript
   if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
     return null;
   }
   ```

4. **Reemplazar por:**
   ```javascript
   if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
     throw createError({
       statusCode: 404,
       statusMessage: "Propiedad no encontrada",
       fatal: true,
     });
   }
   ```

5. **Guardar:**
   - Presiona `Ctrl + S`

---

### Archivo 2: `pages/proyecto/[slug].vue` (Si existe)

**Repetir los mismos pasos** para el archivo de proyectos.

1. Presiona `Ctrl + P`
2. Escribe: `proyecto/[slug].vue`
3. Buscar el mismo patrón `return null;`
4. Reemplazar con el bloque de `throw createError`

---

## ✅ Verificación

Después de hacer los cambios:

1. **Guardar todos los archivos** (`Ctrl + K` luego `S`)

2. **Verificar en el navegador:**
   ```
   http://localhost:3001/inmueble/propiedad-que-no-existe-123
   ```
   
   Debe mostrar:
   - La página de error que creamos (`error.vue`)
   - Código 404 en las DevTools (F12 → Network)

3. **Si funciona:**
   - Hacer commit
   - Push a producción
   - Google detectará el cambio en la próxima validación

---

## 🚨 POR QUÉ ES URGENTE

- Google está validando **ahora mismo** (iniciado 23/12/25)
- Si no corregimos antes de que termine, seguirá viendo Soft 404
- La validación puede tardar 1-7 días
- Mientras más rápido lo arreglemos, mejor

---

## 📊 IMPACTO

**Antes:**
- 308 páginas con Soft 404
- Google las marca como "vacías"
- No se indexan

**Después:**
- Propiedades que existen: 200 OK → Se indexan ✅
- Propiedades que no existen: 404 real → Google las ignora ✅
- Soft 404: 308 → ~0

---

## 🎬 ALTERNATIVA: Usar Buscar y Reemplazar

Si prefieres hacerlo más rápido:

1. **Abrir Buscar y Reemplazar:**
   - Presiona `Ctrl + Shift + H`

2. **En "Buscar":**
   ```
   return null;
   ```

3. **En "Reemplazar":**
   ```
   throw createError({
     statusCode: 404,
     statusMessage: "Propiedad no encontrada",
     fatal: true,
   });
   ```

4. **Filtrar archivos:**
   - Click en "..." → "Files to include"
   - Escribir: `pages/**/*.vue`

5. **Reemplazar:**
   - Click en "Replace All" (o revisar uno por uno)

⚠️ **CUIDADO:** Asegúrate de que solo reemplazas el `return null;` dentro de `useAsyncData` cuando no hay datos, no otros `return null;` del código.

---

**Creado:** 23 de Diciembre 2025, 01:30 AM  
**Prioridad:** 🔴 MÁXIMA  
**Tiempo estimado:** 5-10 minutos
