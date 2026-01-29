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
    etiqueta: articulo.etiqueta || '',
    fechaCreacion: articulo.fechaCreacion,
    fechaActualizacion: articulo.fechaActualizacion,
    activo: articulo.activo,
    orden: articulo.orden
  };
};

export default {
    async deleteAllArticulos() {
      try {
        const response = await fetch(`${getApiBaseUrl()}/Articulo`, {
          method: "DELETE",
          headers: getAuthHeaders()
        });
        if (response.status === 401) {
          redirectToLogin();
          throw new Error("Error 401: No autorizado");
        }
        if (!response.ok) {
          // Intenta parsear el error como JSON, pero si falla, lanza un error genérico
          let errorData = {};
          try {
            errorData = await response.json();
          } catch (e) {}
          throw new Error(errorData.message || `HTTP status: ${response.status}`);
        }
        // Manejar respuesta vacía (sin body)
        let data = null;
        try {
          data = await response.json();
        } catch (e) {
          // Si no hay body, asumimos éxito
          data = { success: true, message: "Todos los artículos eliminados." };
        }
        return data;
      } catch (error) {
        console.error("Error al eliminar todos los artículos:", error);
        throw error;
      }
    },
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
        etiqueta: articuloData.etiqueta || '',
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
        etiqueta: articuloData.etiqueta || '',
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
  },

  // Método para importar artículos de manera masiva desde JSON
  async importarArticulosMasivo(articulos, onProgress = null) {
    const resultados = {
      exitosos: 0,
      fallidos: 0,
      errores: []
    };

    // Función para limpiar y procesar el contenido HTML
    const procesarContenido = (contenido) => {
      if (!contenido) return "";
      
      let contenidoLimpio = contenido;
      
      // Arreglar comillas dobles escapadas ("" -> ")
      contenidoLimpio = contenidoLimpio.replace(/""/g, '"');
      
      // Reemplazar enlaces de propiedades antiguos con los nuevos
      // Venta: cualquier enlace que contenga operacion=venta o /venta/
      contenidoLimpio = contenidoLimpio.replace(
        /https?:\/\/(?:old-web\.)?homesguatemala\.com\/propiedades\/[^"'\s<>]*(?:operacion=venta|fwp_operacion=venta)[^"'\s<>]*/gi,
        'https://homesguatemala.com/propiedades/venta'
      );
      
      // Renta: cualquier enlace que contenga operacion=renta o /renta/
      contenidoLimpio = contenidoLimpio.replace(
        /https?:\/\/(?:old-web\.)?homesguatemala\.com\/propiedades\/[^"'\s<>]*(?:operacion=renta|fwp_operacion=renta)[^"'\s<>]*/gi,
        'https://homesguatemala.com/propiedades/renta'
      );
      
      // Reemplazar enlaces genéricos de propiedades sin operación específica
      contenidoLimpio = contenidoLimpio.replace(
        /https?:\/\/old-web\.homesguatemala\.com\/propiedades\/(?!\?)[^"'\s<>]*/gi,
        'https://homesguatemala.com/propiedades/venta'
      );
      
      // Reemplazar enlaces del blog antiguo (nuestro blog, top proyectos, etc.)
      contenidoLimpio = contenidoLimpio.replace(
        /https?:\/\/(?:old-web\.)?homesguatemala\.com\/(?:los-top-proyectos-inmobiliarios-en-guatemala|blog|nuestro-blog)[^"'\s<>]*/gi,
        'https://homesguatemala.com/blog-inmobiliario'
      );
      
      // Extraer URLs de imágenes del contenido que no estén en tags <img>
      // Buscar URLs de imágenes sueltas (no dentro de src="...")
      const regexImagenSuelta = /(?<!src=["'])https?:\/\/[^\s"'<>]+\.(?:jpg|jpeg|png|gif|webp)(?![^<]*>)/gi;
      const imagenesSueltas = contenidoLimpio.match(regexImagenSuelta) || [];
      
      // Convertir URLs de imágenes sueltas a tags <img>
      imagenesSueltas.forEach(url => {
        // Solo reemplazar si no está ya dentro de un tag img
        const regexNoEnImg = new RegExp(`(?<!src=["'])${url.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')}(?![^<]*>)`, 'g');
        contenidoLimpio = contenidoLimpio.replace(regexNoEnImg, `<img src="${url}" alt="" style="max-width: 100%; height: auto;" />`);
      });
      
      return contenidoLimpio;
    };

    for (let i = 0; i < articulos.length; i++) {
      const articulo = articulos[i];
      
      try {
        // Obtener y procesar el contenido
        const contenidoOriginal = articulo.Content || articulo.content || articulo.contenido || "";
        const contenidoProcesado = procesarContenido(contenidoOriginal);
        
        // Mapear los campos del JSON al formato esperado
        // Soporta formato WordPress (Title, Content, etc) y formato estándar (title, content, etc)
        const articuloFormateado = {
          title: articulo.Title || articulo.title || articulo.titulo,
          content: contenidoProcesado,
          slug: articulo.Slug || articulo.slug,
          imageUrl: (articulo["Image URL"] || articulo.imageUrl || articulo.imagenUrl || "").split("|")[0], // Tomar solo la primera imagen
          categorias: articulo["Categorías"] || articulo.Categorias || articulo.categorias || articulo.categoria || "Informativo",
          activo: articulo.activo !== undefined ? articulo.activo : true,
          orden: articulo.orden || 0
        };

        await this.createArticulo(articuloFormateado);
        resultados.exitosos++;
      } catch (error) {
        resultados.fallidos++;
        resultados.errores.push({
          titulo: articulo.Title || articulo.title || articulo.titulo || `Artículo ${i + 1}`,
          error: error.message
        });
      }

      // Callback de progreso
      if (onProgress) {
        onProgress({
          actual: i + 1,
          total: articulos.length,
          porcentaje: Math.round(((i + 1) / articulos.length) * 100)
        });
      }

      // Pequeña pausa para no saturar el servidor
      await new Promise(resolve => setTimeout(resolve, 100));
    }

    return resultados;
  }
};
