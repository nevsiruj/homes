import config from '../config';

const API_URL = `${config.apiBaseUrl}/Zona`;

export const getAllZonas = async () => {
  try {
    const response = await fetch(API_URL, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al obtener zonas');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en getAllZonas:', error);
    throw error;
  }
};

export const getAllZonasActivas = async () => {
  try {
    const response = await fetch(`${API_URL}/activas`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al obtener zonas activas');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en getAllZonasActivas:', error);
    throw error;
  }
};

export const getZonaById = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al obtener zona');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en getZonaById:', error);
    throw error;
  }
};

export const createZona = async (zonaData) => {
  try {
    const response = await fetch(API_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        nombre: zonaData.nombre,
        activo: zonaData.activo ?? true,
        orden: zonaData.orden ?? 0
      }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al crear zona');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en createZona:', error);
    throw error;
  }
};

export const updateZona = async (id, zonaData) => {
  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        id: id,
        nombre: zonaData.nombre,
        activo: zonaData.activo ?? true,
        orden: zonaData.orden ?? 0
      }),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al actualizar zona');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en updateZona:', error);
    throw error;
  }
};

export const deleteZona = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al eliminar zona');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en deleteZona:', error);
    throw error;
  }
};

export const reorderZonas = async (zonaIds) => {
  try {
    const response = await fetch(`${API_URL}/reorder`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(zonaIds),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Error al reordenar zonas');
    }

    return await response.json();
  } catch (error) {
    console.error('Error en reorderZonas:', error);
    throw error;
  }
};
