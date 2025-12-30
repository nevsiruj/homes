export default defineEventHandler(async (event) => {
    try {
        // URLs estáticas
        const staticUrls = [
            { loc: 'https://homesguatemala.com/', priority: 1.0, changefreq: 'daily' },
            { loc: 'https://homesguatemala.com/propiedades', priority: 0.9, changefreq: 'daily' },
            { loc: 'https://homesguatemala.com/propiedades/venta', priority: 0.9, changefreq: 'daily' },
            { loc: 'https://homesguatemala.com/propiedades/renta', priority: 0.9, changefreq: 'daily' },
            { loc: 'https://homesguatemala.com/proyectos-inmobiliarios', priority: 0.8, changefreq: 'weekly' },
            { loc: 'https://homesguatemala.com/nosotros', priority: 0.7, changefreq: 'monthly' },
            { loc: 'https://homesguatemala.com/blog', priority: 0.7, changefreq: 'weekly' },
        ];

        // Zonas para páginas dinámicas
        const zonas = [
            'zona-10', 'zona-13', 'zona-14', 'zona-15', 'zona-16',
            'carretera-a-el-salvador', 'antigua', 'san-jose-pinula',
            'santa-catarina-pinula', 'san-cristobal', 'muxbal', 'atitlan'
        ];

        const zonasUrls = zonas.map(zona => ({
            loc: `https://homesguatemala.com/propiedades/zona/${zona}`,
            priority: 0.8,
            changefreq: 'weekly'
        }));

        // Combinar todas las URLs
        const allUrls = [...staticUrls, ...zonasUrls];

        return allUrls;
    } catch (error) {
        console.error('Error generating sitemap URLs:', error);
        return [];
    }
});
