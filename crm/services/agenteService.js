import Swal from "sweetalert2";

// Función para obtener la URL base de la API
const getApiBaseUrl = () => {
  // Usar la configuración global inyectada por el plugin
  return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234';
};

let isAuthModalShown = false; // Global flag to track if the modal is already shown

// Nuevo: helper simple como en clienteService
const redirectToLogin = (message = "La sesión ha caducado. Por favor, inicie sesión de nuevo.") => {
  // Evita modales múltiples si llegan varias respuestas a la vez
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

const authService = (() => {
  /* ================= Helpers de Cookie & Token ================= */
  const setCookie = (name, value, days = 30) => {
    if (typeof document === "undefined") return;
    const secure = location.protocol === "https:" ? "; Secure" : "";
    const maxAge = `; Max-Age=${days * 24 * 60 * 60}`;
    document.cookie = `${name}=${encodeURIComponent(value)}; Path=/; SameSite=Lax${maxAge}${secure}`;
  };

  const getCookie = (name) => {
    if (typeof document === "undefined") return null;
    const cookies = document.cookie ? document.cookie.split("; ") : [];
    for (const c of cookies) {
      const [k, v] = c.split("=");
      if (k === name) return decodeURIComponent(v || "");
    }
    return null;
  };

  const deleteCookie = (name) => {
    if (typeof document === "undefined") return;
    document.cookie = `${name}=; Path=/; Max-Age=0; SameSite=Lax`;
  };

  // Nuevo: decodifica JWT y valida expiración
  const isJwtExpired = (token) => {
    try {
      const [, payloadB64] = token.split(".");
      if (!payloadB64) return true;
      const json = JSON.parse(atob(payloadB64.replace(/-/g, "+").replace(/_/g, "/")));
      const expMs = (json.exp || 0) * 1000;
      return Date.now() >= expMs;
    } catch {
      return true;
    }
  };

  const getToken = () => {
    // Preferimos localStorage; si no está, usamos cookie (SSR usará useCookie en middleware)
    const token =
      (typeof localStorage !== "undefined" && localStorage.getItem("jwt-token")) ||
      getCookie("jwt-token") ||
      null;

    // Si no hay token, retorna null
    if (!token) return null;

    // Si está expirado, limpiar y retornar null (disparará el modal)
    if (isJwtExpired(token)) {
      if (typeof localStorage !== "undefined") {
        localStorage.removeItem("jwt-token");
      }
      deleteCookie("jwt-token");
      return null;
    }

    return token;
  };

  // Actualizado: usa localStorage directamente y muestra modal si falta token (como clienteService)
  const getAuthHeaders = (hasBody = false) => {
    const token = (typeof localStorage !== "undefined") ? localStorage.getItem("jwt-token") : null;
    if (!token) {
      redirectToLogin("No se encontró una autenticación válida. Por favor, inicie sesión.");
      throw new Error("No se encontró una autenticación válida. Por favor, inicie sesión.");
    }
    const headers = { Accept: "application/json" };
    if (hasBody) headers["Content-Type"] = "application/json";
    headers["Authorization"] = `Bearer ${token}`;
    return headers;
  };

  const safeParse = async (response) => {
    if (response.status === 204) return null;
    const ct = response.headers.get("content-type") || "";
    const text = await response.text();
    if (!text) return null;
    if (ct.includes("application/json")) {
      try { return JSON.parse(text); } catch { return null; }
    }
    return text;
  };

  const fetchWithAuth = async (url, options = {}) => {
    const hasBody = !!options.body;
    const defaults = {
      method: "GET",
      credentials: "omit",
      ...options,
      headers: { ...getAuthHeaders(hasBody), ...(options.headers || {}) },
    };

    const res = await fetch(url, defaults);

    // Manejo explícito de 401/403 como en clienteService
    if (res.status === 401 || res.status === 403) {
      redirectToLogin();
      throw new Error(`Error ${res.status}: No autorizado`);
    }

    const body = await safeParse(res);
    if (!res.ok) {
      const msg = (body && (body.message || body.error || body.title)) || `HTTP ${res.status}`;
      throw new Error(msg);
    }
    return body;
  };

  /* ================= Métodos públicos ================= */
  async function login(email, password) {
    const res = await fetch(`${getApiBaseUrl()}/Agente/login`, {
      method: "POST",
      credentials: "omit",
      headers: { "Content-Type": "application/json", Accept: "application/json" },
      body: JSON.stringify({ email, password }),
    });

    const data = await safeParse(res);
    if (!res.ok) {
      throw {
        status: res.status,
        message: (data && (data.message || data.error || data.title)) || "Login failed",
      };
    }

    // Guarda en localStorage y cookie (para que SSR pueda leer con middleware)
    if (data?.token) {
      if (typeof localStorage !== "undefined") {
        localStorage.setItem("jwt-token", data.token);
      }
      setCookie("jwt-token", data.token, 30);
    }

    return data;
  }

  async function getCurrentUser() {
    const url = `${getApiBaseUrl()}/Agente/GetLoggedUser`;
    return fetchWithAuth(url, { method: "GET" });
  }

  async function register(registerData) {
    const url = `${getApiBaseUrl()}/Agente/register`;
    return fetchWithAuth(url, {
      method: "POST",
      body: JSON.stringify(registerData),
    });
  }

  async function logout() {
    const url = `${getApiBaseUrl()}/Agente/logout`;
    try {
      await fetchWithAuth(url, { method: "POST" });
    } catch (e) {
      // No bloqueamos el logout del cliente si el server falla
      //console.warn("Logout en servidor falló:", e?.message || e);
    } finally {
      if (typeof localStorage !== "undefined") {
        localStorage.removeItem("jwt-token");
      }
      deleteCookie("jwt-token");
    }
    return true;
  }

  async function getUserTenantId() {
    const url = `${getApiBaseUrl()}/Agente/tenant-id`;
    const data = await fetchWithAuth(url, { method: "GET" });
    return data?.TenantId ?? data?.tenantId ?? null;
  }

  async function getUsers() {
    const url = `${getApiBaseUrl()}/user`;
    return fetchWithAuth(url, { method: "GET" });
  }

  // Compat para código viejo que use este nombre
  async function fetchWithToken(url, options = {}) {
    return fetchWithAuth(url, options);
  }

  return {
    login,
    getCurrentUser,
    getLoggedUser: getCurrentUser, // alias solicitado
    register,
    logout,
    getUserTenantId,
    getUsers,
    // opcional: exportamos por compat
    fetchWithToken,
  };
})();

export default authService;
