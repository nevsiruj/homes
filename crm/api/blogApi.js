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
    id: articulo.Id,
    title: articulo.Titulo,
    content: articulo.Contenido,
    slug: articulo.Slug,
    permalink: articulo.Permalink,
    imageUrl: articulo.ImagenUrl,
    categorias: articulo.Categoria,
    fechaCreacion: articulo.FechaCreacion,
    fechaActualizacion: articulo.FechaActualizacion,
    activo: articulo.Activo,
    orden: articulo.Orden
  };
};

export const getAllArticulos = async () => {
  try {
    const response = await fetch(`${getApiBaseUrl()}/api/Articulo`, {
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
};

export const getArticuloById = async (id) => {
  try {
    const response = await fetch(`${getApiBaseUrl()}/api/Articulo/${id}`, {
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
};

export const createArticulo = async (articuloData) => {
  try {
    // Mapear campos del frontend al backend
    const payload = {
      Titulo: articuloData.title,
      Contenido: articuloData.content,
      Slug: articuloData.slug,
      ImagenUrl: articuloData.imageUrl,
      Categoria: articuloData.categorias,
      Activo: articuloData.activo !== undefined ? articuloData.activo : true,
      Orden: articuloData.orden || 0
    };

    const response = await fetch(`${getApiBaseUrl()}/api/Articulo`, {
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
    // Mapear la respuesta
    const mappedData = mapArticuloFromBackend(data.data);
    return {
      ...data,
      data: mappedData
    };
  } catch (error) {
    console.error("Error al crear artículo:", error);
    throw error;
  }
};

export const updateArticulo = async (id, articuloData) => {
  try {
    // Mapear campos del frontend al backend
    const payload = {
      Id: id,
      Titulo: articuloData.title,
      Contenido: articuloData.content,
      Slug: articuloData.slug,
      ImagenUrl: articuloData.imageUrl,
      Categoria: articuloData.categorias,
      FechaCreacion: articuloData.fechaCreacion,
      FechaActualizacion: new Date().toISOString(),
      Activo: articuloData.activo !== undefined ? articuloData.activo : true,
      Orden: articuloData.orden || 0
    };

    const response = await fetch(`${getApiBaseUrl()}/api/Articulo/${id}`, {
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
    // Mapear la respuesta
    const mappedData = mapArticuloFromBackend(data.data);
    return {
      ...data,
      data: mappedData
    };
  } catch (error) {
    console.error("Error al actualizar artículo:", error);
    throw error;
  }
};

export const deleteArticulo = async (id) => {
  try {
    const response = await fetch(`${getApiBaseUrl()}/api/Articulo/${id}`, {
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
};
