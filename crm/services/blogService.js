import Swal from "sweetalert2";

// Función para obtener la URL base de la API
const getApiBaseUrl = () => {
  if (typeof window !== 'undefined' && window.$config?.apiBaseUrl) return window.$config.apiBaseUrl;
  return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234';
};

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

// Función para mapear del backend al frontend
const mapArticuloFromBackend = (articulo) => {
  return {
    id: articulo.id,
    title: articulo.titulo,
    content: articulo.contenido,
    slug: articulo.slug,
    permalink: articulo.permalink,
    imageUrl: articulo.imagenUrl,
    categorias: articulo.categoria,
    fechaCreacion: articulo.fechaCreacion,
    fechaActualizacion: articulo.fechaActualizacion,
    activo: articulo.activo,
    orden: articulo.orden
  };
};

export default {
  async getAllArticulos() {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Articulo`, {
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
      // Mapear cada artículo
      const mappedData = data.data.map(mapArticuloFromBackend);
      return {
        ...data,
        data: mappedData
      };
    } catch (error) {
      console.error("Error al obtener artículos:", error);
      throw error;
    }
  },

  async getArticuloById(id) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Articulo/${id}`, {
        method: "GET",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al obtener artículo (ID: ${id})`);
      }

      const data = await response.json();
      // Mapear el artículo
      const mappedData = mapArticuloFromBackend(data.data);
      return {
        ...data,
        data: mappedData
      };
    } catch (error) {
      console.error("Error al obtener artículo:", error);
      throw error;
    }
  },

  async createArticulo(articuloData) {
    try {
      // Mapear campos del frontend al backend
      const payload = {
        titulo: articuloData.title,
        contenido: articuloData.content,
        slug: articuloData.slug,
        imagenUrl: articuloData.imageUrl,
        categoria: articuloData.categorias,
        activo: articuloData.activo !== undefined ? articuloData.activo : true,
        orden: articuloData.orden || 0
      };

      const response = await fetch(`${getApiBaseUrl()}/Articulo`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(payload)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || `HTTP status: ${response.status}`);
      }

      const data = await response.json();
      // Mapear la respuesta si existe
      const mappedData = data.data ? mapArticuloFromBackend(data.data) : null;
      return {
        ...data,
        data: mappedData
      };
    } catch (error) {
      console.error("Error al crear artículo:", error);
      throw error;
    }
  },

  async updateArticulo(id, articuloData) {
    try {
      // Mapear campos del frontend al backend
      const payload = {
        id: id,
        titulo: articuloData.title,
        contenido: articuloData.content,
        slug: articuloData.slug,
        imagenUrl: articuloData.imageUrl,
        categoria: articuloData.categorias,
        permalink: articuloData.permalink,
        fechaCreacion: articuloData.fechaCreacion,
        fechaActualizacion: new Date().toISOString(),
        activo: articuloData.activo !== undefined ? articuloData.activo : true,
        orden: articuloData.orden || 0
      };

      const response = await fetch(`${getApiBaseUrl()}/Articulo/${id}`, {
        method: "PUT",
        headers: getAuthHeaders(),
        body: JSON.stringify(payload)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || `HTTP status: ${response.status}`);
      }

      const data = await response.json();
      // Mapear la respuesta si existe
      const mappedData = data.data ? mapArticuloFromBackend(data.data) : null;
      return {
        ...data,
        data: mappedData
      };
    } catch (error) {
      console.error("Error al actualizar artículo:", error);
      throw error;
    }
  },

  async deleteArticulo(id) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Articulo/${id}`, {
        method: "DELETE",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || `HTTP status: ${response.status}`);
      }

      const data = await response.json();
      return data;
    } catch (error) {
      console.error("Error al eliminar artículo:", error);
      throw error;
    }
  }
};
