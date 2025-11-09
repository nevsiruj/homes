import Swal from "sweetalert2";

// Función para obtener la URL base de la API
const getApiBaseUrl = () => { return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234'; };

const redirectToLogin = (message = "La sesión ha caducado. Por favor, inicie sesión de nuevo.") => {
  Swal.fire({
    icon: "warning",
    title: "Sesión caducada",
    text: message,
    confirmButtonText: "Aceptar"
  }).then(() => {
    window.location.href = "/";
  });
};

// Cache simple para evitar llamadas repetidas
let clientesCache = null;
let cacheTimestamp = null;
const CACHE_DURATION = 30000; // 30 segundos

const getAuthHeaders = () => {
  const token = localStorage.getItem("jwt-token");
  if (!token) {
    redirectToLogin("No se encontró una autenticación valida. Por favor, inicie sesión.");
    throw new Error("No se encontró una autenticación valida. Por favor, inicie sesión.");
  }

  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${token}`
  };
};

export default {
  async getAllClientes(page = 1, pageSize = 50, agenteId = null, useCache = true) {
    try {
      // Verificar si hay cache válido
      const now = Date.now();
      if (useCache && clientesCache && cacheTimestamp && (now - cacheTimestamp) < CACHE_DURATION) {
        return clientesCache;
      }

      const params = new URLSearchParams({
        page: page.toString(),
        pageSize: pageSize.toString()
      });
      
      if (agenteId) {
        params.append('agenteId', agenteId.toString());
      }

      const response = await fetch(`${getApiBaseUrl()}/Cliente?${params}`, {
        method: "GET",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`HTTP status: ${response.status}`);
      }

      const data = await response.json();
      
      // Guardar en cache
      clientesCache = data;
      cacheTimestamp = now;
      
      return data;
    } catch (error) {
      throw error;
    }
  },
  
  // Método para limpiar el cache cuando se modifica un cliente
  clearCache() {
    clientesCache = null;
    cacheTimestamp = null;
  },

  async getClienteById(id) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Cliente/${id}`, {
        method: "GET",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al obtener los detalles del cliente (ID: ${id})`);
      }

      return await response.json();
    } catch (error) {
      throw error;
    }
  },

  async getClienteByTelefono(telefono) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Cliente/telefono/${telefono}`, {
        method: "GET",
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al buscar cliente: ${response.statusText}`);
      }

      return await response.json();
    } catch (error) {
      throw new Error(error.message || "Error de conexión");
    }
  },

  async deleteCliente(id) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Cliente/${id}`, {
        method: "DELETE",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al eliminar el cliente (ID: ${id})`);
      }
      
      // Limpiar cache después de eliminar
      this.clearCache();
    } catch (error) {
      throw error;
    }
  },

  async addCliente(cliente) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Cliente`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(cliente)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      // Solo aceptar 201 como éxito
      if (response.status !== 201) {
        const errorData = await response.json().catch(() => null);
        throw new Error(errorData?.message || `Error HTTP: ${response.status}`);
      }

      // Limpiar cache después de crear
      this.clearCache();
      
      return await response.json(); // Devuelve el cliente creado

    } catch (error) {
      //console.error("Error en addCliente:", error);
      // Transforma errores de red a un formato consistente
      throw new Error(error.message || "Error de conexión");
    }
  },

  async updateCliente(id, payload) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Cliente/${id}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify(payload.data || payload)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error HTTP: ${response.status} - ${response.statusText}`);
      }

      // Limpiar cache después de actualizar
      this.clearCache();

      const text = await response.text();
      if (!text) {
        return {}; // o true, dependiendo de lo que esperas
      }

      try {
        return JSON.parse(text);
      } catch (e) {
        //console.error("Respuesta inválida:", text);
        throw new Error("La respuesta del servidor no es un JSON válido");
      }
    } catch (error) {
      throw error;
    }
  },

  async patchCliente(id, payload) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Cliente/${id}/status`, {
        method: "PATCH",
        headers: getAuthHeaders(),
        body: JSON.stringify(payload)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData ? errorData.message || JSON.stringify(errorData) : response.statusText;
        throw new Error(`HTTP error! status: ${response.status}, message: ${errorMessage}`);
      }

      return response.status === 204 ? null : await response.json();
    } catch (error) {
      //console.error(`Error en patchCliente(${id}):`, error.message);
      throw error;
    }
  },

  async obtenerAmenidadesCliente(clienteId) {
    try {
      const cliente = await clienteService.getClienteById(clienteId);
  
      // Acceder a las amenidades dentro de preferencias
      const amenidades = cliente?.preferencias?.preferenciaAmenidades;
  
      if (Array.isArray(amenidades)) {
        //console.log("Amenidades del cliente:", amenidades);
        return amenidades;
      } else {
        //console.warn("No se encontraron amenidades para este cliente.");
        return [];
      }
    } catch (error) {
      //console.error("Error al obtener las amenidades:", error.message);
      return [];
    }
  } 
};
