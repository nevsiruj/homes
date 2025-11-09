// Composable para obtener la configuración de la API
export const useApiConfig = () => {
  const config = useRuntimeConfig();
  return {
    apiBaseUrl: config.public.apiBaseUrl
  };
};
