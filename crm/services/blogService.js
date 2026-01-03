import { getAllArticulos, getArticuloById, createArticulo, updateArticulo, deleteArticulo } from '../api/blogApi';

export default {
  async getAllArticulos() {
    try {
      const response = await getAllArticulos();
      return response;
    } catch (error) {
      console.error("Error al obtener artículos:", error);
      throw error;
    }
  },

  async getArticuloById(id) {
    try {
      const response = await getArticuloById(id);
      return response;
    } catch (error) {
      console.error("Error al obtener artículo:", error);
      throw error;
    }
  },

  async createArticulo(articuloData) {
    try {
      const response = await createArticulo(articuloData);
      return response;
    } catch (error) {
      console.error("Error al crear artículo:", error);
      throw error;
    }
  },

  async updateArticulo(id, articuloData) {
    try {
      const response = await updateArticulo(id, articuloData);
      return response;
    } catch (error) {
      console.error("Error al actualizar artículo:", error);
      throw error;
    }
  },

  async deleteArticulo(id) {
    try {
      const response = await deleteArticulo(id);
      return response;
    } catch (error) {
      console.error("Error al eliminar artículo:", error);
      throw error;
    }
  }
};
