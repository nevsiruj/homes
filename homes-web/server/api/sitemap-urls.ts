import inmuebleService from '~/services/inmuebleService.js';
import proyectoService from '~/services/proyectoService.js';
import { blogs } from '~/data/blogs.js';

export default defineEventHandler(async (event) => {
    const baseUrl = 'https://homesguatemala.com';
    const urls = [];

    // 1. Propiedades
    let inmuebles = [];
    try {
        inmuebles = await inmuebleService.getInmueble();
        if (Array.isArray(inmuebles)) {
            inmuebles.forEach((item) => {
                // Usar slug si existe, si no, usar id
                const slug = item.slugInmueble || item.SlugInmueble || item.slug || item.Slug || item.id;
                if (slug) {
                    urls.push({ loc: `${baseUrl}/inmueble/${slug}`, priority: 0.8, changefreq: 'weekly' });
                }
            });
        }
    } catch (e) { /* ignorar error */ }

    // 2. Proyectos
    let proyectos = [];
    try {
        proyectos = await proyectoService.getProyecto();
        if (Array.isArray(proyectos)) {
            proyectos.forEach((item) => {
                const slug = item.slugProyecto || item.SlugProyecto || item.slug || item.Slug || item.id;
                if (slug) {
                    urls.push({ loc: `${baseUrl}/proyecto/${slug}`, priority: 0.7, changefreq: 'weekly' });
                }
            });
        }
    } catch (e) { /* ignorar error */ }

    // 3. Blog
    try {
        blogs.forEach((item) => {
            if (item.Slug) {
                urls.push({ loc: `${baseUrl}/blog-inmobiliario/${item.Slug}`, priority: 0.6, changefreq: 'weekly' });
            }
        });
    } catch (e) { /* ignorar error */ }

    // URLs estáticas
    urls.push({ loc: baseUrl + '/', priority: 1.0, changefreq: 'daily' });
    urls.push({ loc: baseUrl + '/propiedades', priority: 0.9, changefreq: 'daily' });
    urls.push({ loc: baseUrl + '/proyectos-inmobiliarios', priority: 0.8, changefreq: 'weekly' });
    urls.push({ loc: baseUrl + '/blog-inmobiliario', priority: 0.7, changefreq: 'weekly' });

    // Zonas para páginas dinámicas
    const zonas = [
        'zona-10', 'zona-13', 'zona-14', 'zona-15', 'zona-16',
        'carretera-a-el-salvador', 'antigua', 'san-jose-pinula',
        'santa-catarina-pinula', 'san-cristobal', 'muxbal', 'atitlan'
    ];
    zonas.forEach(zona => {
        urls.push({
            loc: `${baseUrl}/propiedades/zona/${zona}`,
            priority: 0.8,
            changefreq: 'weekly'
        });
    });

    return urls;
});
