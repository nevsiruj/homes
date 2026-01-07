import { API_BASE_URL } from '../config'

const cache = new Map();
const defaultTTL = 1000 * 60 * 3; // 3 minutos

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

// Función para mapear del DTO del backend al formato del frontend
const mapBlogFromBackend = (backendBlog) => {
  return {
    id: backendBlog.id,
    Title: backendBlog.titulo,
    Content: backendBlog.contenido,
    Slug: backendBlog.slug,
    Permalink: backendBlog.permalink,
    'Image URL': backendBlog.imagenUrl, // Asumiendo que ya viene con | si hay múltiples
    'Categorías': backendBlog.categoria,
    FechaCreacion: backendBlog.fechaCreacion,
    FechaActualizacion: backendBlog.fechaActualizacion,
    Activo: backendBlog.activo,
    Orden: backendBlog.orden
  };
};

export default {
  /**
   * Obtiene todos los blogs activos.
   * @returns {Promise<Array>} Lista de blogs mapeados al formato frontend.
   */
  async getAllBlogs() {
    const cacheKey = 'blogs-all';
    const cached = getFromCache(cacheKey);
    if (cached) return cached;

    try {
      const response = await fetch(`${API_BASE_URL}/Articulo/activos`);
      if (!response.ok) {
        throw new Error(`Error HTTP: ${response.status}`);
      }
      const data = await response.json();
      if (!data.success) {
        throw new Error(data.message || 'Error al obtener blogs');
      }
      const blogs = data.data.map(mapBlogFromBackend);
      setCache(cacheKey, blogs);
      return blogs;
    } catch (error) {
      console.error('Error fetching blogs:', error);
      throw error;
    }
  },

  /**
   * Obtiene un blog por slug.
   * @param {string} slug - El slug del blog.
   * @returns {Promise<object>} El blog mapeado al formato frontend.
   */
  async getBlogBySlug(slug) {
    const cacheKey = `blog-${slug}`;
    const cached = getFromCache(cacheKey);
    if (cached) return cached;

    try {
      const response = await fetch(`${API_BASE_URL}/Articulo/slug/${slug}`);
      if (!response.ok) {
        if (response.status === 404) {
          throw new Error('Blog no encontrado');
        }
        throw new Error(`Error HTTP: ${response.status}`);
      }
      const data = await response.json();
      if (!data.success) {
        throw new Error(data.message || 'Error al obtener blog');
      }
      const blog = mapBlogFromBackend(data.data);
      setCache(cacheKey, blog);
      return blog;
    } catch (error) {
      console.error('Error fetching blog by slug:', error);
      throw error;
    }
  },

  /**
   * Obtiene blogs por categoría.
   * @param {string} categoria - La categoría normalizada.
   * @returns {Promise<Array>} Lista de blogs de la categoría.
   */
  async getBlogsByCategoria(categoria) {
    const cacheKey = `blogs-categoria-${categoria}`;
    const cached = getFromCache(cacheKey);
    if (cached) return cached;

    try {
      const response = await fetch(`${API_BASE_URL}/Articulo/categoria/${categoria}`);
      if (!response.ok) {
        throw new Error(`Error HTTP: ${response.status}`);
      }
      const data = await response.json();
      if (!data.success) {
        throw new Error(data.message || 'Error al obtener blogs por categoría');
      }
      const blogs = data.data.map(mapBlogFromBackend);
      setCache(cacheKey, blogs);
      return blogs;
    } catch (error) {
      console.error('Error fetching blogs by categoria:', error);
      throw error;
    }
  }
};