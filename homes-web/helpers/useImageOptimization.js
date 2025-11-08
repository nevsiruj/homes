export const getOptimizedImageUrl = (originalUrl, width) => {
  if (!originalUrl) {
    return `https://via.placeholder.com/${width}x${Math.round(
      width * 0.75
    )}?text=Sin+imagen`;
  }
  try {
    const url = new URL(originalUrl);
    url.searchParams.set("width", width);
    url.searchParams.set("format", "webp");
    return url.toString();
  } catch (e) {
    if (originalUrl.includes(".")) {
      return (
        originalUrl.split(".").slice(0, -1).join(".") + `.webp?width=${width}`
      );
    }
    return originalUrl + `?width=${width}&format=webp`;
  }
};

export const getSrcset = (originalUrl) => {
  const widths = [300, 400, 600, 800, 1200];
  return widths
    .map((width) => `${getOptimizedImageUrl(originalUrl, width)} ${width}w`)
    .join(", ");
};