import Swal from "sweetalert2";

// Función para obtener la URL base de la API
const getApiBaseUrl = () => { return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234'; };

let isAuthModalShown = false; // Global flag to track if the modal is already shown

async function fetchWithTokenCheck(url, options = {}) {
  const token = localStorage.getItem('jwt-token');
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
        //await new Promise((resolve) => setTimeout(resolve, 1000));
        window.location.href = "/";
      });
    }
    throw new Error('No authentication token'); // Throw error instead of returning null
  }

  const headers = {
    ...options.headers,
    Authorization: `Bearer ${token}`,
  };

  return fetch(url, { ...options, headers });
}

export default {
  async getProyecto() {
    try {
      console.log('🔍 Llamando a API:', `${getApiBaseUrl()}/Proyecto`);
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Proyecto`);
      
      if (!response) {
        console.error('❌ No response from fetchWithTokenCheck');
        return null;
      }
      
      if (!response.ok) {
        console.error('❌ Response not OK:', response.status, response.statusText);
        throw new Error('Error fetching proyectos');
      }
      
      const data = await response.json();
      console.log('✅ Data recibida del API:', data);
      return data;
    } catch (error) {
      console.error('❌ Error in proyectoService.getProyecto:', error);
      throw error;
    }
  },

  async getProyectoById(id) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Proyecto/${id}`);
      if (!response.ok) throw new Error('Error fetching proyecto');
      const data = await response.json();
      //console.log('Proyecto data:', data);
      return data;
    } catch (error) {
      //console.error('Error in proyectoService.getProyectoById:', error);
      throw error;
    }
  },

  async createProyecto(payload) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Proyecto`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      });
      if (!response.ok) throw new Error('Error creating proyecto');
      return await response.json();
    } catch (error) {
      //console.error('Error in proyectoService.createProyecto:', error);
      throw error;
    }
  },

  async updateProyecto(id, data) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Proyecto/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        const errorText = await response.text(); // Lee como texto si falla
        throw new Error(`Error HTTP ${response.status}: ${errorText || "Sin mensaje"}`);
      }

      // Verificar si hay contenido antes de parsear
      const contentType = response.headers.get("content-type");
      if (contentType && contentType.includes("application/json")) {
        return await response.json();
      } else {
        // Si no hay JSON, devolver éxito pero sin datos
        return { success: true, message: "Datos actualizados, pero sin respuesta JSON" };
      }
    } catch (error) {
      //console.error("Error en updateProyecto:", error);
      throw error;
    }
  },

  async deleteProyecto(id) {
    try {
      const response = await fetchWithTokenCheck(`${getApiBaseUrl()}/Proyecto/${id}`, {
        method: 'DELETE',
      });
      if (!response.ok) throw new Error('Error deleting proyecto');
      return true;
    } catch (error) {
      //console.error('Error in proyectoService.deleteProyecto:', error);
      throw error;
    }
  },
};
