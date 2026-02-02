/**
 * Limpia el cache de inmuebles (todas las entradas relacionadas)
 */
export function clearInmueblesCache() {
  for (const key of cache.keys()) {
    if (key.startsWith(`${API_BASE_URL}/Inmueble`) || key.startsWith('getInmuebles:')) {
      cache.delete(key);
    }
  }
}
import { API_BASE_URL } from '../config.js'

const cache = new Map();

const defaultTTL = 1000 * 60 * 5; // 5 minutos de caché

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


  /**
   * Obtiene inmuebles paginados y filtrados.
   * @param {number} pageNumber - El número de página solicitado.
   * @param {number} pageSize - El tamaño de la página solicitado.
   * @param {object} filters - Un objeto con los filtros a aplicar (ej. { Operaciones: 'Renta', Tipos: ['Casa'] }).
   * @returns {Promise<object>} Un objeto con la lista de items, totalCount, etc., ya procesado.
   */
  async getInmueblesPaginados(pageNumber = 1, pageSize = 9, filters = {}) {
    try {
      const params = new URLSearchParams();

      // Mapear los nombres de los parámetros del frontend a los del backend (DTO)
      // Se usa un objeto para mapear las claves del frontend a las del backend
      const backendParamNames = {
        operaciones: 'Operaciones',
        tipos: 'Tipos',
        ubicaciones: 'Ubicaciones',
        habitaciones: 'Habitaciones',
        caracteristicasPropiedad: 'Amenidades',
        precioMinimo: 'PrecioMinimo',
        precioMaximo: 'PrecioMaximo',
        titulo: 'Titulo',
        codigoPropiedad: 'CodigoPropiedad',
        orden: 'Orden',
        luxury: 'Luxury',
        searchterm: 'SearchTerm',
        contenido: 'Contenido', // Para búsquedas en el contenido
      };

      params.append('PageNumber', pageNumber.toString());
      params.append('PageSize', pageSize.toString());

      // Añadir los filtros dinámicamente. Ignorar `pageNumber`/`pageSize` si vienen en `filters`.
      const cleanedFilters = { ...filters };
      // Eliminar duplicados de la query que pueden venir en camelCase o PascalCase
      delete cleanedFilters.pageNumber;
      delete cleanedFilters.PageNumber;
      delete cleanedFilters.pageSize;
      delete cleanedFilters.PageSize;

      for (const key in cleanedFilters) {
        if (Object.prototype.hasOwnProperty.call(cleanedFilters, key)) {
          const value = cleanedFilters[key];
          const backendKey = backendParamNames[key.toLowerCase()] || key; // Usa el nombre mapeado

          if (Array.isArray(value)) {
            value.forEach(item => params.append(backendKey, item.toString()));
          } else if (value !== null && value !== undefined && value !== '') {
            params.append(backendKey, value.toString());
          }
        }
      }

      const url = `${API_BASE_URL}/Inmueble?${params.toString()}`;

      const cacheKey = `getInmuebles:${url}`;
      const cached = getFromCache(cacheKey);
      if (cached) {
        return cached;
      }
      
      const response = await fetch(url);
      
      if (!response.ok) {
        const errorBody = await response.text();
        throw new Error(`Error al obtener inmuebles paginados: ${response.statusText} (Status: ${response.status}). Detalles: ${errorBody}`);
      }

      const rawData = await response.json();
      
      // Manejar items que pueden venir como array directo o como {$values: [...]}
      let items = [];
      if (Array.isArray(rawData.items)) {
        items = rawData.items;
      } else if (rawData.items && rawData.items.$values && Array.isArray(rawData.items.$values)) {
        items = rawData.items.$values;
      }
      
      const pagedResult = {
        items: items,
        totalCount: rawData.totalCount,
        pageNumber: rawData.pageNumber,
        pageSize: rawData.pageSize,
        totalPages: rawData.totalPages,
        hasNextPage: rawData.hasNextPage,
        hasPreviousPage: rawData.hasPreviousPage,
      };

      setCache(cacheKey, pagedResult);
      return pagedResult;
    } catch (error) {
      // console.error("Error in inmuebleService.getInmueblesPaginados:", error); // Deshabilitado para producción
      throw error;
    }
  },


  async getInmueble() {
    try {
      const cacheKey = `${API_BASE_URL}/Inmueble:list`;
      const cached = getFromCache(cacheKey);
      if (cached) return cached;
      const response = await fetch(`${API_BASE_URL}/Inmueble`)
      if (!response.ok) throw new Error('Error fetching inmuebles')
      const data = await response.json()
      setCache(cacheKey, data);
      return data
    } catch (error) {
      //console.error('Error in inmuebleService.getInmueble:', error)
      throw error
    }
  },


  async getInmuebleById(id) {
    try {
      const cacheKey = `${API_BASE_URL}/Inmueble:id:${id}`;
      const cached = getFromCache(cacheKey);
      if (cached) return cached;
      const response = await fetch(`${API_BASE_URL}/Inmueble/${id}`)
      if (!response.ok) throw new Error('Error fetching inmueble')
      const data = await response.json()
      //console.log('Inmueble data:', data) 
      setCache(cacheKey, data);
      return data
    } catch (error) {
      //console.error('Error in inmuebleService.getInmuebleById:', error)
      throw error
    }
  },

  async getInmuebleBySlug(slug) {
    try {
      const safeSlug = encodeURIComponent(String(slug || '').trim());

      const cacheKey = `${API_BASE_URL}/Inmueble:slug:${safeSlug}`;
      const cached = getFromCache(cacheKey);
      if (cached) {
        console.log(`[CACHE HIT] Inmueble encontrado en caché: ${safeSlug}`);
        return cached;
      }
      
      console.log(`[API CALL] Solicitando inmueble: ${API_BASE_URL}/Inmueble/by-slug/${safeSlug}`);
      
      const response = await fetch(`${API_BASE_URL}/Inmueble/by-slug/${safeSlug}`, {
        headers: { 
          'Accept': 'application/json',
          'User-Agent': 'HomesGuatemala-Web/1.0'
        },
        // Agregar timeout para evitar que se cuelgue
        signal: AbortSignal.timeout(10000) // 10 segundos timeout
      });

      console.log(`[API RESPONSE] Status: ${response.status} para slug: ${safeSlug}`);

      if (!response.ok) {
        const body = await response.text().catch(() => '');
        console.error(`[API ERROR] ${response.status} ${response.statusText} - ${body}`);
        
        if (response.status === 404) {
          return null; // Retornar null para 404 en lugar de lanzar error
        }
        
        throw new Error(`Error al buscar el inmueble: ${response.status} ${response.statusText}. ${body}`);
      }

      const data = await response.json();
      
      // Validar que los datos sean correctos
      if (!data || typeof data !== 'object') {
        console.error('[API ERROR] Respuesta inválida del servidor');
        return null;
      }

      // (Opcional) asegurar el campo por si llega con otra capitalización
      if (!data.slugInmueble && data.SlugInmueble) {
        data.slugInmueble = data.SlugInmueble;
      }

      console.log(`[SUCCESS] Inmueble cargado: ${data.titulo || safeSlug}`);
      setCache(cacheKey, data);
      return data;
    } catch (error) {
      // Mejorar logging de errores
      if (error.name === 'AbortError' || error.name === 'TimeoutError') {
        console.error(`[TIMEOUT] La solicitud para ${slug} excedió el tiempo límite`);
      } else {
        console.error('[ERROR] Error en inmuebleService.getInmuebleBySlug:', error.message);
      }
      throw error;
    }
  }




}