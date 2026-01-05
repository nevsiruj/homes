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
  async getAllZonas() {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona`, {
        method: 'GET',
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al obtener zonas');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al obtener zonas:", error);
      throw error;
    }
  },

  async getAllZonasActivas() {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona/activas`, {
        method: 'GET',
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al obtener zonas activas');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al obtener zonas activas:", error);
      throw error;
    }
  },

  async getZonaById(id) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona/${id}`, {
        method: 'GET',
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al obtener zona');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al obtener zona:", error);
      throw error;
    }
  },

  async createZona(zonaData) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({
          nombre: zonaData.nombre,
          activo: zonaData.activo ?? true,
          orden: zonaData.orden ?? 0
        }),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al crear zona');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al crear zona:", error);
      throw error;
    }
  },

  async updateZona(id, zonaData) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona/${id}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify({
          id: id,
          nombre: zonaData.nombre,
          activo: zonaData.activo ?? true,
          orden: zonaData.orden ?? 0
        }),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al actualizar zona');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al actualizar zona:", error);
      throw error;
    }
  },

  async deleteZona(id) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona/${id}`, {
        method: 'DELETE',
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al eliminar zona');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al eliminar zona:", error);
      throw error;
    }
  },

  async reorderZonas(zonaIds) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona/reorder`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify(zonaIds),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error al reordenar zonas');
      }

      return await response.json();
    } catch (error) {
      console.error("Error al reordenar zonas:", error);
      throw error;
    }
  }
};
