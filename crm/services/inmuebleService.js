// inmuebleService.js
import { API_BASE_URL } from '../config'
import Swal from "sweetalert2";

function normalizeNumber(n, fallback = 1) {
  // Cubre casos: number, string numérica, { value: ... }, ref/computed-like, null/undefined
  const v = (n && typeof n === 'object' && 'value' in n) ? n.value : n
  const num = Number(v)
  return Number.isFinite(num) && num > 0 ? num : fallback
}

function normalizeValue(v) {
  // Asegura que filtros no manden [object Object]
  if (v == null) return ''
  if (typeof v === 'object' && 'value' in v) return String(v.value)
  return String(v)
}

let isAuthModalShown = false; // Global flag to track if the modal is already shown

async function fetchWithTokenCheck(url, options = {}) {
  const token = localStorage.getItem('jwt-token');
  if (!token) {
    if (!isAuthModalShown) {
      isAuthModalShown = true; // Set the flag to true to prevent duplicate modals
      await Swal.fire({
        icon: 'warning',
        title: 'Sesión caducada',
        text: 'Tu sesión ha caducado. Por favor, inicia sesión nuevamente.',
        confirmButtonText: 'Aceptar',
        allowOutsideClick: false, // Prevent closing the modal by clicking outside
      }).then(() => {
        // Reset the flag and redirect the user
        isAuthModalShown = false;
        window.location.href = "/";
      });
    }
    return null; // Prevent further execution and stop error propagation
  }

  const headers = {
    ...options.headers,
    Authorization: `Bearer ${token}`,
  };

  const response = await fetch(url, { ...options, headers });

  // Handle 401 Unauthorized explicitly
  if (response.status === 401) {
    if (!isAuthModalShown) {
      isAuthModalShown = true;
      await Swal.fire({
        icon: 'warning',
        title: 'Sesión caducada',
        text: 'Tu sesión ha caducado. Por favor, inicia sesión nuevamente.',
        confirmButtonText: 'Aceptar',
        allowOutsideClick: false,
      }).then(() => {
        isAuthModalShown = false;
        window.location.href = "/";
      });
    }
    return null;
  }

  return response;
}

export default {
  /**
   * Obtiene inmuebles paginados y filtrados.
   * @param {number|object} pageNumber
   * @param {number|object} pageSize
   * @param {object} filters Objeto de filtros (ej: { searchTerm: 'apartamento' })
   */
  async getInmueblesPaginados(pageNumber = 1, pageSize = 9, filters = {}) {
    try {
      const PageNumber = normalizeNumber(pageNumber, 1)
      const PageSize   = normalizeNumber(pageSize, 9)

      const params = new URLSearchParams()
      params.set('PageNumber', String(PageNumber))
      params.set('PageSize',   String(PageSize))

      // Serializa los filtros sin riesgo de "[object Object]"
      for (const key in filters) {
        if (!Object.prototype.hasOwnProperty.call(filters, key)) continue
        const val = filters[key]
        if (Array.isArray(val)) {
          val.forEach(item => {
            const s = normalizeValue(item)
            if (s !== '') params.append(key, s)
          })
        } else if (val !== null && val !== undefined && val !== '') {
          params.append(key, normalizeValue(val))
        }
      }

      const url = `${API_BASE_URL}/Inmueble?${params.toString()}`
      //console.log('Fetching URL:', url)

      const response = await fetchWithTokenCheck(url);
      if (!response.ok) {
        const errorBody = await response.text()
        throw new Error(
          `Error al obtener inmuebles paginados: ${response.statusText} (Status: ${response.status}). Detalles: ${errorBody}`
        )
      }

      const rawData = await response.json()

      // === Extracción robusta de items ===
      let rawItems = []
      if (Array.isArray(rawData)) {
        rawItems = rawData
      } else if (Array.isArray(rawData.$values)) {
        rawItems = rawData.$values
      } else if (rawData.items) {
        if (Array.isArray(rawData.items)) {
          rawItems = rawData.items
        } else if (Array.isArray(rawData.items.$values)) {
          rawItems = rawData.items.$values
        }
      }

      // Mapear y normalizar cada inmueble para usar estructuras consistentes en UI
      const mappedItems = (rawItems || []).map((it) => {
        const precioNum = (it && (typeof it.precio === 'number')) ? it.precio : (it?.precio ? Number(it.precio) : 0)
        const amen = Array.isArray(it?.amenidades)
          ? it.amenidades.map(a => (typeof a === 'string' ? a : (a?.nombre || ''))).filter(Boolean)
          : (Array.isArray(it?.amenidades?.$values) ? it.amenidades.$values.map(a => (a?.nombre || a)).filter(Boolean) : [])

        return {
          // conservar todo lo original por si se necesita
          __raw: it,
          id: it?.id,
          codigoPropiedad: it?.codigoPropiedad || it?.codigo || '',
          titulo: it?.titulo || it?.nombre || '',
          slugInmueble: it?.slugInmueble || it?.slug || '',
          precio: Number.isFinite(precioNum) ? precioNum : 0,
          precioActivo: Boolean(it?.precioActivo),
          precioMin: it?.precioMin ?? null,
          precioMax: it?.precioMax ?? null,
          habitaciones: it?.habitaciones ?? null,
          banos: it?.banos ?? null,
          parqueos: it?.parqueos ?? null,
          metrosCuadrados: it?.metrosCuadrados ?? null,
          imagenPrincipal: it?.imagenPrincipal || null,
          imagenesReferencia: Array.isArray(it?.imagenesReferencia?.$values)
            ? it.imagenesReferencia.$values.map(x => x.url || x)
            : (Array.isArray(it?.imagenesReferencia) ? it.imagenesReferencia.map(x => x.url || x) : []),
          tipos: it?.tipos || it?.tipo || '',
          operaciones: it?.operaciones || it?.operacion || '',
          ubicaciones: it?.ubicaciones || it?.ubicacion || '',
          amenidades: amen,
          luxury: Boolean(it?.luxury),
          video: it?.video || '',
        }
      })

      //console.debug('inmuebleService.getInmueblesPaginados -> items:', mappedItems.length)

      return {
        items: mappedItems,
        // metadata (si están presentes en la respuesta original)
        totalCount: rawData.totalCount ?? rawData.totalItems ?? null,
        pageNumber: rawData.pageNumber ?? PageNumber,
        pageSize: rawData.pageSize ?? PageSize,
        totalPages: rawData.totalPages ?? null,
        hasNextPage: rawData.hasNextPage ?? null,
        hasPreviousPage: rawData.hasPreviousPage ?? null,
        // opcional: exponer rawData para depuración si hace falta
        raw: rawData,
      }
    } catch (error) {
      //console.error('Error en inmuebleService.getInmueblesPaginados:', error)
      throw error
    }
  },

  async getInmueble() {
    try {
      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Inmueble`);
      if (!response.ok) throw new Error('Error fetching inmuebles')
      return await response.json()
    } catch (error) {
      //console.error('Error in inmuebleService.getInmueble:', error)
      throw error
    }
  },

  async getInmuebleById(id) {
    try {
      console.log(`🔍 [inmuebleService] Buscando inmueble con ID/código: ${id}`);
      
      // Verificar si es un número (ID) o string alfanumérico (código)
      const isNumericId = /^\d+$/.test(String(id).trim());
      
      if (isNumericId) {
        console.log(`📡 [inmuebleService] Buscando por ID numérico: ${id}`);
        // Búsqueda por ID numérico (método original)
        const response = await fetchWithTokenCheck(`${API_BASE_URL}/Inmueble/${id}`);
        
        console.log(`📡 [inmuebleService] Response status: ${response.status}`);
        
        if (!response.ok) {
          console.error(`❌ [inmuebleService] Error HTTP: ${response.status} ${response.statusText}`);
          throw new Error(`Error fetching inmueble: ${response.status} - ${response.statusText}`);
        }
        
        const data = await response.json();
        console.log(`📊 [inmuebleService] Raw response (by ID):`, data);
        
        return this.normalizeInmuebleData(data);
      } else {
        console.log(`🔤 [inmuebleService] Buscando por código alfanumérico: ${id}`);
        // Búsqueda por código usando el endpoint paginado
        return await this.getInmuebleByCodigoPropiedad(id);
      }
    } catch (error) {
      console.error(`❌ [inmuebleService] Error en getInmuebleById(${id}):`, error);
      throw error;
    }
  },

  async getInmuebleByCodigoPropiedad(codigo) {
    try {
      console.log(`🔤 [inmuebleService] Buscando por código de propiedad: ${codigo}`);
      
      // Usar el endpoint paginado con filtro de búsqueda
      const filters = {
        searchTerm: codigo,
        // Otros filtros si es necesario
      };
      
      const result = await this.getInmueblesPaginados(1, 50, filters);
      
      console.log(`📊 [inmuebleService] Resultados de búsqueda por código:`, result);
      
      if (!result.items || result.items.length === 0) {
        console.warn(`❌ [inmuebleService] No se encontró inmueble con código: ${codigo}`);
        return null;
      }
      
      // Buscar coincidencia exacta del código
      const codigoUpper = codigo.toUpperCase();
      let inmuebleEncontrado = result.items.find(item => 
        (item.codigoPropiedad || '').toUpperCase() === codigoUpper
      );
      
      // Si no hay coincidencia exacta, tomar el primero (búsqueda aproximada)
      if (!inmuebleEncontrado && result.items.length > 0) {
        console.warn(`⚠️ [inmuebleService] No hay coincidencia exacta, tomando el primero de ${result.items.length} resultados`);
        inmuebleEncontrado = result.items[0];
      }
      
      if (!inmuebleEncontrado) {
        console.warn(`❌ [inmuebleService] No se encontró inmueble con código: ${codigo}`);
        return null;
      }
      
      console.log(`✅ [inmuebleService] Inmueble encontrado por código:`, inmuebleEncontrado);
      
      // El resultado de getInmueblesPaginados ya está normalizado
      return inmuebleEncontrado;
      
    } catch (error) {
      console.error(`❌ [inmuebleService] Error buscando por código ${codigo}:`, error);
      throw error;
    }
  },

  normalizeInmuebleData(data) {
    console.log(`🔧 [inmuebleService] Normalizando datos del inmueble...`);
    
    // Normalizar la respuesta para uso consistente
    const normalizedData = {
      ...data,
      // Normalizar amenidades a array simple de strings
      amenidades: Array.isArray(data.amenidades?.$values) 
        ? data.amenidades.$values.map(a => a.nombre || a.Nombre || a)
        : (Array.isArray(data.amenidades) 
          ? data.amenidades.map(a => typeof a === 'string' ? a : (a?.nombre || a?.Nombre || ''))
          : [])
    };
    
    console.log(`✅ [inmuebleService] Datos normalizados:`, {
      id: normalizedData.id,
      codigoPropiedad: normalizedData.codigoPropiedad,
      titulo: normalizedData.titulo,
      tipos: normalizedData.tipos,
      operaciones: normalizedData.operaciones,
      ubicaciones: normalizedData.ubicaciones,
      precio: normalizedData.precio,
      habitaciones: normalizedData.habitaciones,
      banos: normalizedData.banos,
      amenidades: normalizedData.amenidades
    });
    
    return normalizedData;
  },

  async createInmueble(payload) {
    try {
      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Inmueble`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });
      if (!response.ok) throw new Error('Error creating inmueble')
      return await response.json()
    } catch (error) {
      //console.error('Error in inmuebleService.createInmueble:', error)
      throw error
    }
  },

  async updateInmueble(id, data) {
    try {
      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Inmueble/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        const errorText = await response.text()
        throw new Error(`Error HTTP ${response.status}: ${errorText || "Sin mensaje"}`)
      }

      const contentType = response.headers.get("content-type")
      if (contentType && contentType.includes("application/json")) {
        return await response.json()
      } else {
        return { success: true, message: "Datos actualizados, pero sin respuesta JSON" }
      }
    } catch (error) {
      //console.error("Error en updateInmueble:", error)
      throw error
    }
  },

  async deleteInmueble(id) {
    try {
      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Inmueble/${id}`, {
        method: 'DELETE',
      });
      if (!response.ok) throw new Error('Error deleting inmueble')
      return true
    } catch (error) {
      //console.error('Error in inmuebleService.deleteInmueble:', error)
      throw error
    }
  },

  async togglePrecioActivo(id, nuevoEstado) {
    try {
      const payload = { precioActivo: !!nuevoEstado }

      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Inmueble/${id}/precio-activo`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        const errorText = await response.text()
        throw new Error(`Error al actualizar el estado de precio activo: ${response.statusText} (Status: ${response.status}). Detalles: ${errorText}`)
      }

      const contentType = response.headers.get("content-type")
      if (contentType && contentType.includes("application/json")) {
        return await response.json()
      } else {
        return { success: true, message: "Estado de precio activo actualizado con éxito." }
      }
    } catch (error) {
      //console.error('Error en inmuebleService.togglePrecioActivo:', error)
      throw error
    }
  }
}