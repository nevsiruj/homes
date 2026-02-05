// server/api/_sitemap-urls.ts

interface SitemapUrl {
  loc: string;
  lastmod?: Date | string;
  changefreq?: 'always' | 'hourly' | 'daily' | 'weekly' | 'monthly' | 'yearly' | 'never';
  priority?: number;
}

export default defineEventHandler(async () => {
  // La URL base correcta según tu config.js
  const API_BASE_URL = 'https://app-pool.vylaris.online/homes/api';

  try {
    // IMPORTANTE: Cambiamos las URLs a las que SI funcionan en tus services
    const [inmueblesData, proyectosData] = await Promise.allSettled([
      $fetch<any>(`${API_BASE_URL}/Inmueble?PageNumber=1&PageSize=500`),
      $fetch<any>(`${API_BASE_URL}/Proyecto`)
    ]);

    const routes: SitemapUrl[] = [];

    // --- Procesar Inmuebles ---
    if (inmueblesData.status === 'fulfilled') {
      const raw = inmueblesData.value;
      // Manejamos el formato $values que usa tu API de .NET
      let items: any[] = [];
      if (Array.isArray(raw.items)) items = raw.items;
      else if (raw.items?.$values) items = raw.items.$values;
      else if (Array.isArray(raw)) items = raw;

      items.forEach((item: any) => {
        // Usamos los campos exactos de tu service
        const slug = item.slugInmueble || item.SlugInmueble || item.slug || item.id;
        if (slug) {
          routes.push({
            loc: `/inmueble/${slug}`,
            lastmod: item.fechaModificacion || new Date(),
            changefreq: 'weekly',
            priority: 0.8
          });
        }
      });
    }

    // --- Procesar Proyectos ---
    if (proyectosData.status === 'fulfilled') {
      const raw = proyectosData.value;
      let items: any[] = [];
      if (Array.isArray(raw)) items = raw;
      else if (raw?.$values) items = raw.$values;

      items.forEach((item: any) => {
        const slug = item.slugProyecto || item.SlugProyecto || item.slug || item.id;
        if (slug) {
          routes.push({
            loc: `/proyecto/${slug}`,
            lastmod: new Date(),
            changefreq: 'monthly',
            priority: 0.7
          });
        }
      });
    }

    return routes;
  } catch (error) {
    console.error('Error crítico en sitemap-urls:', error);
    return [];
  }
});