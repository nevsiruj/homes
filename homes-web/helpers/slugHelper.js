/**
 * Generates a URL-friendly slug from a property object.
 * Used as a client-side fallback when the API doesn't provide a slug.
 * 
 * **NOTE**: This implementation should match the backend SlugGenerator.cs logic:
 * - Removes accents (á→a, é→e, etc.) using Unicode normalization
 * - Converts to lowercase
 * - Replaces non-alphanumeric characters with hyphens
 * - Removes leading/trailing hyphens
 * 
 * @param {Object} item - Property object with titulo and codigoPropiedad
 * @returns {string} URL-friendly slug
 * 
 * @example
 * generateSlug({ titulo: "Apartamento en Venta", codigoPropiedad: "ASV8508" })
 * // Returns: "apartamento-en-venta-asv8508"
 */
export function generateSlug(item) {
  if (!item) return '';
  
  const title = item.titulo || '';
  const code = item.codigoPropiedad || '';
  const text = `${title} ${code}`.trim();
  
  if (!text) return '';
  
  // NFD = Canonical Decomposition (splits accented chars into base + combining marks)
  // Then we remove the combining marks ([\u0300-\u036f])
  // This matches the backend's manual accent replacement
  return text
    .toLowerCase()
    .normalize('NFD')                  // Decompose accented characters
    .replace(/[\u0300-\u036f]/g, '')   // Remove accent marks (á→a, é→e, etc.)
    .replace(/[^a-z0-9]+/g, '-')       // Replace non-alphanumeric with hyphens
    .replace(/^-+|-+$/g, '');           // Remove leading/trailing hyphens
}

/**
 * Gets the slug from a property object, using the API-provided slug
 * or generating one as a fallback.
 * 
 * @param {Object} item - Property object
 * @returns {string} URL-encoded slug
 */
export function getPropertySlug(item) {
  if (!item) return '';
  
  // Try to get slug from various possible field names
  const slug = item?.slugInmueble ?? 
               item?.slug ?? 
               item?.SlugInmueble ?? 
               generateSlug(item);
  
  return encodeURIComponent(String(slug).trim());
}

/**
 * Generates the full URL path for a property.
 * 
 * @param {Object} item - Property object
 * @param {string} type - Type of property ('propiedad' or 'proyecto')
 * @returns {string} URL path (e.g., '/inmueble/apartamento-en-venta-asv8508')
 */
export function getPropertyUrl(item, type = 'propiedad') {
  const slug = getPropertySlug(item);
  
  if (!slug) return '#';
  
  if (type === 'proyecto') {
    return `/proyecto/${slug}`;
  }
  
  return `/inmueble/${slug}`;
}
