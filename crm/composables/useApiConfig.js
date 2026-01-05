// Composable para obtener la configuración de la API
export const useApiConfig = () => {
  const config = useRuntimeConfig();

  // Intentar leer configuración en tiempo de ejecución (inyectada por window.$config)
  // Esto permite cambiar la URL de la API en producción editando public/config.js sin recompilar
  const runtimeApiUrl = (typeof window !== 'undefined' && window.$config?.apiBaseUrl)
    ? window.$config.apiBaseUrl
    : null;

  return {
    apiBaseUrl: runtimeApiUrl || config.public.apiBaseUrl
  };
};
