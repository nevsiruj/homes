// Script para generar todas las rutas de propiedades y proyectos para prerender
import { API_BASE_URL } from '../config.js'

async function generateRoutes() {
  const routes = ['/'];
  
  try {
    console.log('🏠 Obteniendo propiedades...');
    const inmuebleResponse = await fetch(`${API_BASE_URL}/Inmueble?PageSize=10000`);
    
    if (inmuebleResponse.ok) {
      const inmuebleData = await inmuebleResponse.json();
      const inmuebles = inmuebleData.items?.$values || inmuebleData.items || [];
      
      inmuebles.forEach(item => {
        const slug = item.slugInmueble || item.SlugInmueble || item.slug || item.Slug;
        if (slug) {
          routes.push(`/inmueble/${slug}`);
        }
      });
      console.log(`✅ ${inmuebles.length} propiedades agregadas`);
    }
  } catch (error) {
    console.error('⚠️ Error obteniendo propiedades:', error);
  }
  
  try {
    console.log('🏗️ Obteniendo proyectos...');
    const proyectoResponse = await fetch(`${API_BASE_URL}/Proyecto`);
    
    if (proyectoResponse.ok) {
      const proyectoData = await proyectoResponse.json();
      const proyectos = proyectoData.$values || proyectoData || [];
      
      proyectos.forEach(item => {
        const slug = item.slugProyecto || item.SlugProyecto || item.slug || item.Slug;
        if (slug) {
          routes.push(`/proyecto/${slug}`);
        }
      });
      console.log(`✅ ${proyectos.length} proyectos agregados`);
    }
  } catch (error) {
    console.error('⚠️ Error obteniendo proyectos:', error);
  }
  
  console.log(`🎉 Total de rutas: ${routes.length}`);
  return routes;
}

export default generateRoutes;
