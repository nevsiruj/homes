// Cache para URLs optimizadas
const urlCache = new Map();
const MAX_CACHE_SIZE = 500;

export const getOptimizedImageUrl = (originalUrl, width) => {
  if (!originalUrl) {
    return `https://via.placeholder.com/${width}x${Math.round(
      width * 0.75
    )}?text=Sin+imagen`;
  }

  // Verificar cache
  const cacheKey = `${originalUrl}|${width}`;
  if (urlCache.has(cacheKey)) {
    return urlCache.get(cacheKey);
  }

  let optimizedUrl;
  try {
    const url = new URL(originalUrl);
    url.searchParams.set("width", width);
    url.searchParams.set("format", "webp");
    optimizedUrl = url.toString();
  } catch (e) {
    if (originalUrl.includes(".")) {
      optimizedUrl = originalUrl.split(".").slice(0, -1).join(".") + `.webp?width=${width}`;
    } else {
      optimizedUrl = originalUrl + `?width=${width}&format=webp`;
    }
  }

  // Guardar en cache con límite de tamaño
  if (urlCache.size >= MAX_CACHE_SIZE) {
    const firstKey = urlCache.keys().next().value;
    urlCache.delete(firstKey);
  }
  urlCache.set(cacheKey, optimizedUrl);

  return optimizedUrl;
};

export const getSrcset = (originalUrl) => {
  const widths = [300, 400, 600, 800, 1200];
  return widths
    .map((width) => `${getOptimizedImageUrl(originalUrl, width)} ${width}w`)
    .join(", ");
};