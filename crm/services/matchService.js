import { API_BASE_URL } from '../config';
import Swal from "sweetalert2";

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

const matchService = {
  // 📥 Obtener todos los matches pendientes de un cliente
  async getPendientesByCliente(clienteId) {
  if (!clienteId) throw new Error("clienteId requerido");

  try {
    const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match/clientes/${clienteId}/matches-sugeridos`, {
      method: "GET",
    });

    if (response?.status === 404) {
      console.log(`📭 No hay matches pendientes para cliente ${clienteId}`);
      return [];
    }
    if (!response?.ok) {
      const errorData = await response.json().catch(() => null);
      const errorMessage = errorData?.title || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    return await response.json();
  } catch (error) {
    console.error("Error al obtener matches pendientes:", error);
    throw error;
  }
},

async getSugeridos(clienteId) {
  if (!clienteId) throw new Error("clienteId requerido");

  try {
    const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match/clientes/${clienteId}/matches-sugeridos`, {
      method: "GET",
    });

    if (response?.status === 404) {
      console.log(`📭 No hay matches pendientes para cliente ${clienteId}`);
      return [];
    }
    if (!response?.ok) {
      const errorData = await response.json().catch(() => null);
      const errorMessage = errorData?.title || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    return await response.json();
  } catch (error) {
    console.error("Error al obtener matches pendientes:", error);
    throw error;
  }
},

// 📥 Obtener todos los matches enviados de un cliente
async getEnviadosByCliente(clienteId) {
  if (!clienteId) throw new Error("clienteId requerido");

  try {
    const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match/cliente/${clienteId}/enviados`, {
      method: "GET",
    });

    if (response?.status === 404) {
      console.log(`📭 No hay matches enviados para cliente ${clienteId}`);
      return [];
    }
    if (!response?.ok) {
      const errorData = await response.json().catch(() => null);
      const errorMessage = errorData?.title || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    return await response.json();
  } catch (error) {
    console.error("Error al obtener matches enviados:", error);
    throw error;
  }
},

// 🖋️ Marcar un match como enviado
async marcarComoEnviado(matchId) {
  if (!matchId) throw new Error("matchId requerido");
  try {
    const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match/${matchId}/enviar`, {
      method: "PUT",
    });

    if (!response?.ok) {
      let errJson = null;
      try {
        errJson = await response.json();
      } catch {}
      const errorMessage = errJson?.title || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    let updated = null;
    if (response.status !== 204) {
      try {
        updated = await response.json();
      } catch {
        updated = null;
      }
      if (updated?.$values && Array.isArray(updated.$values)) {
        updated = updated.$values[0] || null;
      }
    }
    console.log("Match marcado como enviado", updated || "");
    return { ok: true, match: updated || null };
  } catch (error) {
    console.error("Error al marcar el match como enviado:", error);
    return { ok: false, error };
  }
},

  // 📤 Crear un nuevo match
  async createMatch(payload) {
    try {
      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (!response?.ok) {
        const txt = await response.text().catch(() => "");
        console.error("Error al crear el match:", txt);
        throw new Error(`Error HTTP: ${response.status}`);
      }

      let data = null;
      try {
        data = await response.json();
      } catch {
        data = null;
      }

      // Normalizar posible envoltura
      if (data?.$values && Array.isArray(data.$values) && data.$values.length > 0) {
        data = data.$values[0];
      }
      console.log("Match creado:", data);
      return data;
    } catch (error) {
      console.error("Error al crear el match:", error);
      throw error;
    }
  },

  // 🖋️ Marcar un match como enviado (devuelve {ok, match?})
  // async marcarComoEnviado(matchId) {
  //   if (!matchId) throw new Error("matchId requerido");
  //   try {
  //     const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match/${matchId}/enviar`, {
  //       method: "PUT",
  //     });

  //     if (!response?.ok) {
  //       let errJson = null;
  //       try {
  //         errJson = await response.json();
  //       } catch {}
  //       const errorMessage = errJson?.title || `HTTP error! status: ${response.status}`;
  //       throw new Error(errorMessage);
  //     }

  //     let updated = null;
  //     // Algunos backends devuelven 204 No Content
  //     if (response.status !== 204) {
  //       try {
  //         updated = await response.json();
  //       } catch {
  //         updated = null;
  //       }
  //       if (updated?.$values && Array.isArray(updated.$values)) {
  //         updated = updated.$values[0] || null;
  //       }
  //     }
  //     console.log("Match marcado como enviado", updated || "");
  //     return { ok: true, match: updated || null };
  //   } catch (error) {
  //     console.error("Error al marcar el match como enviado:", error);
  //     return { ok: false, error };
  //   }
  // },

  // 🗑️ Eliminar un match por ID
  async deleteMatch(matchId) {
    if (!matchId) throw new Error("matchId requerido");

    try {
      const response = await fetchWithTokenCheck(`${API_BASE_URL}/Match/${matchId}`, {
        method: "DELETE",
      });

      if (!response?.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData?.title || `HTTP error! status: ${response.status}`;
        throw new Error(errorMessage);
      }

      console.log("Match eliminado");
      return true;
    } catch (error) {
      console.error("Error al eliminar el match:", error);
      throw error;
    }
  },
};

export default matchService;