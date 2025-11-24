import { API_BASE_URL } from '../config'

const cache = new Map();
const defaultTTL = 1000 * 60 * 3; // 3 minutes
const getFromCache = (key) => {
  const entry = cache.get(key);
  if (!entry) return null;
  if (Date.now() > entry.expires) {
    cache.delete(key);
    return null;
  }
  return entry.value;
};
const setCache = (key, value, ttl = defaultTTL) => {
  cache.set(key, { value, expires: Date.now() + ttl });
};

export default {

  
  async getProyecto() {
    try {
      const cacheKey = `${API_BASE_URL}/Proyecto:list`;
      const cached = getFromCache(cacheKey);
      if (cached) return cached;
      const response = await fetch(`${API_BASE_URL}/Proyecto`)
      if (!response.ok) throw new Error('Error fetching Proyectos')
      const data = await response.json();
      setCache(cacheKey, data);
      return data;
    } catch (error) {
      //console.error('Error in ProyectoService.getProyecto:', error)
      throw error
    }
  },


  async getProyectoById(id) {
    try {
      const cacheKey = `${API_BASE_URL}/Proyecto:id:${id}`;
      const cached = getFromCache(cacheKey);
      if (cached) return cached;
      const response = await fetch(`${API_BASE_URL}/Proyecto/${id}`)
      if (!response.ok) throw new Error('Error fetching Proyecto')
      const data = await response.json()
      setCache(cacheKey, data);
      return data
    } catch (error) {
      //console.error('Error in ProyectoService.getProyectoById:', error)
      throw error
    }
  },

  async getProyectoBySlug(slug) {
    try {
      const safeSlug = encodeURIComponent(String(slug || '').trim());
  
      const cacheKey = `${API_BASE_URL}/Proyecto:slug:${safeSlug}`;
      const cached = getFromCache(cacheKey);
      if (cached) return cached;
      const response = await fetch(`${API_BASE_URL}/Proyecto/by-slug/${safeSlug}`, {
        headers: { 'Accept': 'application/json' }
      });
  
      if (!response.ok) {
        const body = await response.text().catch(() => '');
        throw new Error(`Error al buscar el Proyecto: ${response.status} ${response.statusText}. ${body}`);
      }
  
      const data = await response.json();
      setCache(cacheKey, data);
  
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