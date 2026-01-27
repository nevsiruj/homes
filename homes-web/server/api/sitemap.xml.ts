export default defineEventHandler(async (event) => {
  // Obtener las URLs dinámicas desde el endpoint /api/sitemap-urls
  const runtimeConfig = useRuntimeConfig();
  const baseUrl = runtimeConfig.public?.siteUrl || 'https://homesguatemala.com';
  let urls = [];
  try {
    const res = await $fetch('/api/sitemap-urls');
    urls = Array.isArray(res) ? res : [];
  } catch (e) {
    urls = [];
  }

  // Generar el XML del sitemap
  const sitemap = `<?xml version="1.0" encoding="UTF-8"?>\n<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n${urls.map(urlObj => {
    const loc = urlObj.loc || urlObj;
    const priority = urlObj.priority ? `<priority>${urlObj.priority}</priority>` : '';
    const changefreq = urlObj.changefreq ? `<changefreq>${urlObj.changefreq}</changefreq>` : '';
    return `<url><loc>${loc}</loc>${priority}${changefreq}</url>`;
  }).join('\n')}\n</urlset>`;

  event.node.res.setHeader('Content-Type', 'application/xml');
  event.node.res.end(sitemap);
});
