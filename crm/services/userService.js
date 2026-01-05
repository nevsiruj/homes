import { useAuthStore } from '@/stores/auth'
import Swal from 'sweetalert2';

// Función para obtener la URL base de la API
const getApiBaseUrl = () => { if (typeof window !== 'undefined' && window.$config?.apiBaseUrl) return window.$config.apiBaseUrl; return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234'; };

// Evita modales duplicados
let isAuthModalShown = false;

// Helper simple como en clienteService
const redirectToLogin = (message = "La sesión ha caducado. Por favor, inicie sesión de nuevo.") => {
  if (isAuthModalShown) return;
  isAuthModalShown = true;
  Swal.fire({
    icon: "warning",
    title: "Sesión caducada",
    text: message,
    confirmButtonText: "Aceptar"
  }).then(() => {
    isAuthModalShown = false;
    window.location.href = "/";
  });
};

// Helper centralizado para token y 401
async function fetchWithTokenCheck(endpoint, options = {}) {
  const token = localStorage.getItem('jwt-token');

  // Si no hay token → modal y salir
  if (!token) {
    redirectToLogin("No se encontró una autenticación válida. Por favor, inicie sesión.");
    return null;
  }

  // Construir headers y request
  const hasBody = !!options.body;
  const headers = {
    Accept: 'application/json',
    ...(hasBody ? { 'Content-Type': 'application/json' } : {}),
    ...(options.headers || {}),
    Authorization: `Bearer ${token}`,
  };

  const url = `${getApiBaseUrl()}${endpoint}`;
  const response = await fetch(url, { credentials: 'omit', ...options, headers });

  // Manejo explícito de 401 → modal y salir
  if (response.status === 401) {
    redirectToLogin();
    return null;
  }

  return response;
}

const userService = (() => {
  // Ya no valida token ni redirige; sólo arma headers base
  const getHeaders = (hasBody = false) => {
    const headers = { Accept: 'application/json' };
    if (hasBody) headers['Content-Type'] = 'application/json';
    return headers;
  };

  const fetchWithAuth = async (endpoint, options = {}) => {
    const defaults = {
      ...options,
      headers: {
        ...getHeaders(!!options.body),
        ...(options.headers || {}),
      },
      credentials: 'omit',
    };

    try {
      const response = await fetchWithTokenCheck(endpoint, defaults);
      if (!response) return null; // token faltante o 401 ya manejado con modal

      if (!response.ok) {
        // Intenta JSON; si falla, usa texto
        let errorMsg = `Error en la solicitud: ${response.status}`;
        try {
          const err = await response.json();
          errorMsg = err?.message || err?.title || errorMsg;
        } catch {
          try {
            errorMsg = await response.text();
          } catch { }
        }
        throw new Error(errorMsg);
      }

      // Parseo seguro
      const ct = response.headers.get('content-type') || '';
      if (ct.includes('application/json')) return await response.json();
      return await response.text();
    } catch (error) {
      //console.error("Error en fetchWithAuth:", error);
      throw error;
    }
  };

  return {
    addUsuario: async (usuario) => {
      return fetchWithAuth('/user/register', {
        method: 'POST',
        body: JSON.stringify(usuario),
      });
    },
    getAllUsuarios: async () => {
      return fetchWithAuth('/user', { method: 'GET' });
    },
    getById: async (id) => {
      return fetchWithAuth(`/user/${id}`, { method: 'GET' });
    },
    editUsuario: async (id, usuario) => {
      return fetchWithAuth(`/user/${id}`, {
        method: 'PUT',
        body: JSON.stringify(usuario),
      });
    },
    removeUsuario: async (id) => {
      return fetchWithAuth(`/user/${id}`, { method: 'DELETE' });
    },
    toggleUserStatus: async (id, isActive) => {
      return fetchWithAuth(`/user/toggle-status/${id}`, {
        method: 'PUT',
        body: JSON.stringify({ isActive }),
        headers: { 'Content-Type': 'application/json' },
      });
    },
    getAllAgentes: async () => {
      return fetchWithAuth('/user', { method: 'GET' });
    },
  };
})();

export default userService;
