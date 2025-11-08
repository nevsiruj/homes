import { API_BASE_URL } from '../config'

export default {

  
  async getProyecto() {
    try {
      const response = await fetch(`${API_BASE_URL}/Proyecto`)
      if (!response.ok) throw new Error('Error fetching Proyectos')
      return await response.json()
    } catch (error) {
      //console.error('Error in ProyectoService.getProyecto:', error)
      throw error
    }
  },


  async getProyectoById(id) {
    try {
      const response = await fetch(`${API_BASE_URL}/Proyecto/${id}`)
      if (!response.ok) throw new Error('Error fetching Proyecto')
      const data = await response.json()
      //console.log('Proyecto data:', data) 
      return data
    } catch (error) {
      //console.error('Error in ProyectoService.getProyectoById:', error)
      throw error
    }
  },

  async getProyectoBySlug(slug) {
    try {
      const safeSlug = encodeURIComponent(String(slug || '').trim());
  
      const response = await fetch(`${API_BASE_URL}/Proyecto/by-slug/${safeSlug}`, {
        headers: { 'Accept': 'application/json' }
      });
  
      if (!response.ok) {
        const body = await response.text().catch(() => '');
        throw new Error(`Error al buscar el Proyecto: ${response.status} ${response.statusText}. ${body}`);
      }
  
      const data = await response.json();
  
      // (Opcional) asegurar el campo por si llega con otra capitalización
      if (!data.slugProyecto && data.SlugProyecto) {
        data.slugProyecto = data.SlugProyecto;
      }
  
      return data;
    } catch (error) {
      //console.error('Error en proyectoService.getProyectoBySlug:', error);
      throw error;
    }
  }  
}