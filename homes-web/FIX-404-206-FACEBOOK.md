# Fix para Errores 404/206 en Facebook Debugger

## 🔧 Problemas Solucionados

### 1. **Error 404**: Facebook no puede encontrar la página
- **Causa**: El SSR no estaba esperando correctamente a que se cargaran los datos
- **Solución**: Configuración explícita de `server: true` y `lazy: false` en `useAsyncData`

### 2. **Error 206**: Respuesta parcial del servidor  
- **Causa**: El contenido se renderizaba antes de que los metadatos estuvieran listos
- **Solución**: Establecer metadatos inmediatamente con valores directos (no funciones)

## ✅ Mejoras Implementadas

### En `[slug].vue`:

1. **useAsyncData mejorado**:
   ```javascript
   {
     server: true,      // Forzar ejecución en servidor
     lazy: false,       // Esperar datos antes de renderizar
     key: `inmueble-detail-${slug}`, // Clave única
   }
   ```

2. **Mejor manejo de errores**:
   - Logs detallados en SSR
   - Validación robusta de datos
   - Manejo de casos edge en imágenes

3. **Metadatos mejorados**:
   - Schema.org más completo con ImageObject
   - Meta tags adicionales para compatibilidad
   - Validación de URLs de imágenes

### En `inmuebleService.js`:

1. **Timeout en fetch**: 10 segundos máximo
2. **Logs detallados** para debugging
3. **Retorno de `null`** en lugar de error para 404
4. **User-Agent** personalizado

### Nuevo `server/middleware/bot-handler.ts`:

- Detecta bots de redes sociales
- Asegura SSR completo para crawlers
- Aumenta timeout para bots
- Cache-Control optimizado

## 🧪 Cómo Validar el Fix

### 1. Reiniciar el servidor
```bash
# Detener servidor actual (Ctrl+C)
npm run dev
```

### 2. Verificar en el navegador
```bash
# Abrir una propiedad
https://localhost:3000/inmueble/[slug-de-prueba]

# Click derecho → Ver código fuente (View Page Source)
# Buscar: <meta property="og:image"
# Verificar que tenga una URL completa y válida
```

### 3. Limpiar caché de Facebook
1. Ir a: https://developers.facebook.com/tools/debug/
2. Pegar URL de un inmueble: `https://homesguatemala.com/inmueble/[slug]`
3. Click en **"Depurar"** (Debug)
4. Verificar que no haya errores 404 o 206
5. Click en **"Volver a extraer"** (Scrape Again)
6. Verificar que se vean:
   - ✅ Título correcto
   - ✅ Descripción correcta
   - ✅ Imagen correcta

### 4. Probar en WhatsApp
1. Copiar la URL de un inmueble
2. Enviársela a ti mismo en WhatsApp
3. Verificar que aparezca el preview con:
   - Imagen
   - Título
   - Descripción

## 📊 Logs de Debugging

### En desarrollo (navegador console):
```
🔍 [INMUEBLE SEO] Datos completos: {...}
📝 [INMUEBLE SEO] pageTitle: ...
🖼️ [INMUEBLE SEO] pageImage: ...
🔗 [INMUEBLE SEO] propertyUrl: ...
```

### En servidor (terminal):
```
🌐 [SSR] Generando metadatos para: slug-ejemplo
📝 [SSR] pageTitle: Casa en Venta...
🖼️ [SSR] pageImage: https://...
🤖 [BOT DETECTED] facebookexternalhit/1.1
[API CALL] Solicitando inmueble: ...
[SUCCESS] Inmueble cargado: Casa en Venta...
```

## ⚠️ Problemas Comunes

### Si aún aparece error 404:
1. Verificar que la API esté respondiendo correctamente
2. Revisar logs del servidor para ver qué slug está fallando
3. Verificar que el slug en la URL sea correcto

### Si aparece error 206:
1. Verificar que no haya errores de JavaScript en el servidor
2. Revisar que todos los metadatos tengan valores válidos
3. Verificar que las imágenes existan y sean accesibles

### Si la imagen no aparece:
1. Verificar que `imagenPrincipal` tenga un valor válido
2. Comprobar que la URL de la imagen sea accesible públicamente
3. Verificar el formato de la imagen (JPEG, PNG, WEBP)

## 🔄 Notas sobre Caché

- **Facebook**: Guarda caché por ~7 días
- **WhatsApp**: Guarda caché por ~24 horas
- **Solución**: Usar "Scrape Again" en Facebook Debugger

## 📱 Testing Rápido

```bash
# URL de ejemplo para testing
https://homesguatemala.com/inmueble/casa-zona-10-en-venta

# Verificar que aparezca en:
1. Facebook Debugger ✅
2. WhatsApp Preview ✅
3. LinkedIn Preview ✅
4. Twitter Card Validator ✅
```

## 🎯 Próximos Pasos

Si aún hay problemas:
1. Verificar logs del servidor para errores específicos
2. Probar con diferentes slugs
3. Verificar la conectividad con la API
4. Revisar si hay problemas de red o firewall
