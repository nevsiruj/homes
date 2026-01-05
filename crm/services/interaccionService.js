import Swal from "sweetalert2";

// Función para obtener la URL base de la API
const getApiBaseUrl = () => { if (typeof window !== 'undefined' && window.$config?.apiBaseUrl) return window.$config.apiBaseUrl; return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234'; };

let isAuthModalShown = false; // Global flag to track if the modal is already shown

async function fetchWithTokenCheck(url, options = {}) {
  const token = localStorage.getItem("jwt-token");
  if (!token) {
    if (!isAuthModalShown) {
      isAuthModalShown = true; // Set the flag to true to prevent duplicate modals
      await Swal.fire({
        icon: 'warning',
        title: 'Autenticación requerida',
        text: 'No se encontró el token de autenticación. Por favor, inicia sesión.',
        confirmButtonText: 'Aceptar',
        allowOutsideClick: false, // Prevent closing the modal by clicking outside
      }).then(async () => {
        // Add a 1-second delay before redirecting
        await new Promise((resolve) => setTimeout(resolve, 1000));
        window.location.href = "/";
      });
    }
    return null; // Prevent further execution and stop error propagation
  }

  const headers = {
    ...options.headers,
    Authorization: `Bearer ${token}`,
  };

  return fetch(url, { ...options, headers });
}

const interaccionService = {

  // 📥 Obtener todas las interacciones
  async getAllInteracciones() {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Interaccion`, {
        method: "GET",
      });

      if (!response?.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData?.title || `HTTP error! status: ${response.status}`;
        throw new Error(errorMessage);
      }

      return await response.json();
    } catch (error) {
      //console.error("Error fetching all interactions:", error);
      throw error;
    }
  },

  // 📤 Crear una nueva interacción
  async addInteraccion(payload) {
    //console.log("📞 Enviando nueva interacción:", payload);

    const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Interaccion`, {
      method: 'POST',
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });

    if (!response?.ok) {
      const errorText = await response.text();
      //console.error("🚫 Respuesta inválida del servidor:", errorText);
      throw new Error(`Error HTTP: ${response.status}`);
    }

    const data = await response.json();
    //console.log("✅ Interacción creada:", data);
    return data;
  },

  // 🖋️ Actualizar una interacción por ID
  async updateInteraccion(id, payload) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Interaccion/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (!response?.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData ? errorData.message || JSON.stringify(errorData) : response.statusText;
        throw new Error(`HTTP error! status: ${response.status}, message: ${errorMessage}`);
      }

      return response.status === 204 ? null : await response.json();
    } catch (error) {
      //console.error(`Error updating interaccion with ID ${id}:`, error);
      throw error;
    }
  },

  // 🗑️ Eliminar una interacción por ID
  async deleteInteraccion(id) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Interaccion/${id}`, {
        method: "DELETE",
      });

      if (!response?.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData ? errorData.message || JSON.stringify(errorData) : response.statusText;
        throw new Error(`HTTP error! status: ${response.status}, message: ${errorMessage}`);
      }

      return true; // Indica eliminación exitosa
    } catch (error) {
      //console.error(`Error deleting interaccion with ID ${id}:`, error);
      throw error;
    }
  },

  async getInteraccionesByCliente(clienteId) {
    if (!clienteId) throw new Error("clienteId requerido");
    try {
      // Since the specific endpoint doesn't exist, get interactions from client data
      const clienteService = (await import('./clienteService')).default;
      const clienteData = await clienteService.getClienteById(clienteId);
      const cliente = clienteData?.$values ? clienteData.$values[0] : clienteData;

      // Extract interactions from client data
      let interaccionesData = null;
      if (cliente?.interacciones) {
        interaccionesData = cliente.interacciones;
      } else if (cliente?.historialInteracciones) {
        interaccionesData = cliente.historialInteracciones;
      } else if (cliente?.seguimientos) {
        interaccionesData = cliente.seguimientos;
      }

      if (interaccionesData?.$values) {
        return interaccionesData.$values;
      } else if (Array.isArray(interaccionesData)) {
        return interaccionesData;
      }

      return [];
    } catch (e) {
      console.warn('Error getting interactions from client data:', e);
      return [];
    }
  },

  // 🖋️ Actualizar solo el estado de una interacción por ID
  async patchStatus(id, nuevoStatus) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Interaccion/${id}/status`, {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(nuevoStatus),
      });

      if (!response?.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData ? errorData.message || JSON.stringify(errorData) : response.statusText;
        throw new Error(`HTTP error! status: ${response.status}, message: ${errorMessage}`);
      }

      return response.status === 204 ? null : await response.json();
    } catch (error) {
      //console.error(`Error updating status for interaccion with ID ${id}:`, error);
      throw error;
    }
  },
};

export default interaccionService;
