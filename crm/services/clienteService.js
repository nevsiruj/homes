import { API_BASE_URL } from "../config";
import Swal from "sweetalert2";

const redirectToLogin = (message = "La sesión ha caducado. Por favor, inicie sesión de nuevo.") => {
  Swal.fire({
    icon: "warning",
    title: "Sesión caducada",
    text: message,
    confirmButtonText: "Aceptar"
  }).then(() => {
    window.location.href = "/";
  });
};

const getAuthHeaders = () => {
  const token = localStorage.getItem("jwt-token");
  if (!token) {
    redirectToLogin("No se encontró una autenticación valida. Por favor, inicie sesión.");
    throw new Error("No se encontró una autenticación valida. Por favor, inicie sesión.");
  }

  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${token}`
  };
};

export default {
  async getAllClientes() {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente`, {
        method: "GET",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`HTTP status: ${response.status}`);
      }

      return await response.json();
    } catch (error) {
      throw error;
    }
  },

  async getClienteById(id) {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente/${id}`, {
        method: "GET",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al obtener los detalles del cliente (ID: ${id})`);
      }

      return await response.json();
    } catch (error) {
      throw error;
    }
  },

  async getClienteByTelefono(telefono) {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente/telefono/${telefono}`, {
        method: "GET",
        headers: getAuthHeaders(),
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al buscar cliente: ${response.statusText}`);
      }

      return await response.json();
    } catch (error) {
      throw new Error(error.message || "Error de conexión");
    }
  },

  async deleteCliente(id) {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente/${id}`, {
        method: "DELETE",
        headers: getAuthHeaders()
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error al eliminar el cliente (ID: ${id})`);
      }
    } catch (error) {
      throw error;
    }
  },

  async addCliente(cliente) {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(cliente)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      // Solo aceptar 201 como éxito
      if (response.status !== 201) {
        const errorData = await response.json().catch(() => null);
        throw new Error(errorData?.message || `Error HTTP: ${response.status}`);
      }

      return await response.json(); // Devuelve el cliente creado

    } catch (error) {
      //console.error("Error en addCliente:", error);
      // Transforma errores de red a un formato consistente
      throw new Error(error.message || "Error de conexión");
    }
  },

  async updateCliente(id, payload) {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente/${id}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify(payload.data || payload)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        throw new Error(`Error HTTP: ${response.status} - ${response.statusText}`);
      }

      const text = await response.text();
      if (!text) {
        return {}; // o true, dependiendo de lo que esperas
      }

      try {
        return JSON.parse(text);
      } catch (e) {
        //console.error("Respuesta inválida:", text);
        throw new Error("La respuesta del servidor no es un JSON válido");
      }
    } catch (error) {
      throw error;
    }
  },

  async patchCliente(id, payload) {
    try {
      const response = await fetch(`${API_BASE_URL}/Cliente/${id}/status`, {
        method: "PATCH",
        headers: getAuthHeaders(),
        body: JSON.stringify(payload)
      });

      if (response.status === 401) {
        redirectToLogin();
        throw new Error("Error 401: No autorizado");
      }

      if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData ? errorData.message || JSON.stringify(errorData) : response.statusText;
        throw new Error(`HTTP error! status: ${response.status}, message: ${errorMessage}`);
      }

      return response.status === 204 ? null : await response.json();
    } catch (error) {
      //console.error(`Error en patchCliente(${id}):`, error.message);
      throw error;
    }
  },

  async obtenerAmenidadesCliente(clienteId) {
    try {
      const cliente = await clienteService.getClienteById(clienteId);
  
      // Acceder a las amenidades dentro de preferencias
      const amenidades = cliente?.preferencias?.preferenciaAmenidades;
  
      if (Array.isArray(amenidades)) {
        //console.log("Amenidades del cliente:", amenidades);
        return amenidades;
      } else {
        //console.warn("No se encontraron amenidades para este cliente.");
        return [];
      }
    } catch (error) {
      //console.error("Error al obtener las amenidades:", error.message);
      return [];
    }
  } 
};