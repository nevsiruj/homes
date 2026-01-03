import { getAllZonas, getZonaById, createZona, updateZona, deleteZona } from '../api/zonasApi';

export default {
  async getAllZonas() {
    try {
      const response = await getAllZonas();
      return response;
    } catch (error) {
      console.error("Error al obtener zonas:", error);
      throw error;
    }
  },

  async getZonaById(id) {
    try {
      const response = await getZonaById(id);
      return response;
    } catch (error) {
      console.error("Error al obtener zona:", error);
      throw error;
    }
  },

  async createZona(zonaData) {
    try {
      const response = await createZona(zonaData);
      return response;
    } catch (error) {
      console.error("Error al crear zona:", error);
      throw error;
    }
  },

  async updateZona(id, zonaData) {
    try {
      const response = await updateZona(id, zonaData);
      return response;
    } catch (error) {
      console.error("Error al actualizar zona:", error);
      throw error;
    }
  },

  async deleteZona(id) {
    try {
      const response = await deleteZona(id);
      return response;
    } catch (error) {
      console.error("Error al eliminar zona:", error);
      throw error;
    }
  }
};
