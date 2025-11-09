//config.js

// Usar variable de entorno para producción, localhost para desarrollo
// La variable API_BASE_URL se establece durante el build en las tasks de deploy
export const API_BASE_URL = process.env.API_BASE_URL || import.meta.env?.VITE_API_BASE_URL || 'https://localhost:7234';

// Valores comentados para referencia:
// export const API_BASE_URL = 'https://localhost:44301'
// export const API_BASE_URL = 'https://app-pool.vylaris.online/homes/api'
// export const API_BASE_URL = 'https://medianaranja.vylaris.online/api' // Producción
