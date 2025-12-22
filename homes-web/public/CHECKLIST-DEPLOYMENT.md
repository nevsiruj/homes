# ✅ CHECKLIST DE DEPLOYMENT - SEO 100%

## Pre-Deployment

### Verificaciones Locales
- [x] Auditoría SEO interna: 100%
- [x] Servidor de desarrollo funcionando
- [x] Todos los tests pasando
- [x] Commits realizados en branch `dev`

### Archivos Críticos Modificados
- [x] `nuxt.config.ts` - Schema.org + hreflang
- [x] `pages/index.vue` - Título optimizado
- [x] `components/footer.vue` - LinkedIn agregado
- [x] `.gitignore` - debug_html.txt ignorado

---

## Deployment a Producción

### 1. Merge a Master
```bash
git checkout master
git pull origin master
git merge dev
git push origin master
```

### 2. Verificar Build
```bash
npm run build
```

**Verificar que no haya errores de:**
- TypeScript
- Linting
- Build de Nuxt

### 3. Deploy
Según tu plataforma:
- **Vercel:** Push automático
- **Netlify:** Push automático
- **Manual:** `npm run generate` y subir carpeta `.output`

---

## Post-Deployment (CRÍTICO)

### Verificaciones Inmediatas (Primeros 5 minutos)

#### 1. Verificar robots.txt
```
URL: https://homesguatemala.com/robots.txt
```
Debe mostrar:
```
User-agent: *
Disallow: /admin
Disallow: /_nuxt
Allow: /

Sitemap: https://homesguatemala.com/sitemap.xml
```

#### 2. Verificar Sitemap
```
URL: https://homesguatemala.com/sitemap.xml
```
Debe mostrar XML con todas las URLs

#### 3. Verificar Schema.org
```
URL: https://search.google.com/test/rich-results
```
Ingresar: `https://homesguatemala.com`
Debe detectar: **RealEstateAgent**

#### 4. Verificar Títulos
- Homepage: Debe tener 42 caracteres
- Propiedades: Debe tener 46 caracteres
- Nosotros: Debe tener 46 caracteres

#### 5. Verificar Redes Sociales
Inspeccionar footer, debe incluir:
- Facebook ✅
- Instagram ✅
- YouTube ✅
- LinkedIn ✅
- Twitter/X ✅

---

## Google Search Console (Primeros 30 minutos)

### 1. Solicitar Indexación
1. Ir a: https://search.google.com/search-console
2. Inspeccionar URL: `https://homesguatemala.com/`
3. Clic en "SOLICITAR INDEXACIÓN"
4. Esperar confirmación

### 2. Reenviar Sitemap
1. Ir a: Sitemaps
2. Eliminar sitemap antiguo (si existe)
3. Agregar: `sitemap.xml`
4. Verificar estado: "Correcto" en verde

### 3. Verificar Cobertura
1. Ir a: Cobertura
2. Verificar que NO haya:
   - "Bloqueado por robots.txt"
   - "Página con redirección"
   - "Error del servidor (5xx)"

---

## Monitoreo (Primeras 24 horas)

### Cada 2 Horas
- [ ] Verificar que el sitio esté online
- [ ] Verificar que no haya errores 500
- [ ] Verificar que robots.txt esté accesible

### Cada 6 Horas
- [ ] Revisar Google Search Console
- [ ] Verificar impresiones (deben empezar a subir)
- [ ] Revisar errores de rastreo

### Al Final del Día
- [ ] Captura de pantalla de métricas iniciales
- [ ] Documentar cualquier issue
- [ ] Reportar al cliente

---

## Métricas a Capturar (Baseline)

### Google Search Console
- Impresiones (últimas 24h): ______
- Clics (últimas 24h): ______
- Páginas indexadas: ______

### Google Analytics
- Sesiones (últimas 24h): ______
- Bounce Rate: ______
- Tiempo promedio: ______

### PageSpeed Insights
- Mobile Score: ______
- Desktop Score: ______
- LCP: ______

---

## Comunicación con Cliente

### Email Inmediato (Post-Deployment)
```
Asunto: ✅ Optimizaciones SEO Implementadas - Acción Requerida

Estimado Cliente,

Hemos completado las optimizaciones SEO críticas:
- SEO Técnico: 100%
- robots.txt: Corregido
- Schema.org: Implementado
- Títulos: Optimizados

ACCIÓN REQUERIDA (Próximas 24 horas):
1. Solicitar indexación en Google Search Console
2. Reenviar sitemap
3. Verificar cobertura

Adjunto encontrarás:
- Plan de Recuperación Urgente
- Resumen Ejecutivo
- Checklist de Verificación

Saludos,
Equipo de Desarrollo
```

### Seguimiento (48 horas)
```
Asunto: 📊 Reporte de Progreso SEO - 48 Horas

Métricas actuales:
- Páginas indexadas: [número]
- Impresiones: [número]
- Estado de robots.txt: ✅

Próximos pasos:
- [acción 1]
- [acción 2]
```

---

## Rollback Plan (Si algo sale mal)

### Si el sitio no carga:
```bash
git revert HEAD
git push origin master --force
```

### Si robots.txt está mal:
1. Acceder al servidor
2. Editar manualmente `/public/robots.txt`
3. Reiniciar servidor

### Si hay errores 500:
1. Revisar logs del servidor
2. Verificar variables de entorno
3. Contactar soporte de hosting

---

## Contactos de Emergencia

**Hosting/Servidor:**
- Proveedor: _____________
- Soporte: _____________
- Acceso: _____________

**DNS:**
- Proveedor: _____________
- Panel: _____________

**Equipo Técnico:**
- Desarrollador: _____________
- DevOps: _____________

---

## Notas Finales

- ✅ Todos los cambios están documentados
- ✅ Backup realizado antes del deployment
- ✅ Plan de rollback preparado
- ✅ Cliente informado

**Fecha de Deployment:** _______________
**Hora:** _______________
**Responsable:** _______________
**Estado:** _______________

---

**Última Actualización:** 22 de Diciembre 2025
**Versión:** 1.0
