// Función para obtener la URL base de la API
const getApiBaseUrl = () => { return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234'; };

export default {
  async getAllZonas() {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Zona`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

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
        headers: {
          'Content-Type': 'application/json',
        },
      });

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
        headers: {
          'Content-Type': 'application/json',
        },
      });

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
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          nombre: zonaData.nombre,
          activo: zonaData.activo ?? true,
          orden: zonaData.orden ?? 0
        }),
      });

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
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          id: id,
          nombre: zonaData.nombre,
          activo: zonaData.activo ?? true,
          orden: zonaData.orden ?? 0
        }),
      });

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
        headers: {
          'Content-Type': 'application/json',
        },
      });

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
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(zonaIds),
      });

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
