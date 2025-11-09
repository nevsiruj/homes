// Función para obtener la URL base de la API
const getApiBaseUrl = () => { return window.__NUXT__?.config?.public?.apiBaseUrl || 'https://localhost:7234'; };

// Obtiene los encabezados con token de autenticación
const getAuthHeaders = () => {
  const token = localStorage.getItem("jwt-token");

  if (!token) {
    throw new Error("No se encontró un token de autenticación");
  }

  return {
    "Content-Type": "application/json",
    "Accept": "application/json",
    "Authorization": `Bearer ${token}`
  };
};

const requerimientoService = {
  // Obtener todas las preferencias
  async getAllPreferencias() {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Preferencia`, {
        method: "GET",
        headers: getAuthHeaders()
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData?.title || `HTTP error! status: ${response.status}`;
        throw new Error(errorMessage);
      }

      return await response.json();
    } catch (error) {
      // console.error("Error fetching all preferencias:", error);
      throw error;
    }
  },

  // Obtener preferencia por ID
  async getPreferenciaById(preferenciaId) {
    const attempts = [];
    // Lista de intentos (ordenada por probabilidad / compatibilidad)
    const endpoints = [
      { method: "GET", url: `${getApiBaseUrl()}/Preferencia/${preferenciaId}` },
      { method: "GET", url: `${getApiBaseUrl()}/Preferencia/Get/${preferenciaId}` },
      { method: "GET", url: `${getApiBaseUrl()}/Preferencia/GetById/${preferenciaId}` },
      { method: "POST", url: `${getApiBaseUrl()}/Preferencia/Buscar`, body: JSON.stringify({ id: preferenciaId }) },
    ];

    for (const e of endpoints) {
      try {
        const opts = {
          method: e.method,
          headers: getAuthHeaders(),
        };
        if (e.method === "POST") {
          opts.headers = { ...opts.headers, "Content-Type": "application/json" };
          opts.body = e.body;
        }

        const resp = await fetch(e.url, opts);
        // parse seguro de respuesta
        const text = await resp.text().catch(() => "");
        const ct = resp.headers.get("content-type") || "";
        const body = text && ct.includes("application/json") ? JSON.parse(text) : text || null;

        if (resp.ok) {
          return body;
        }

        attempts.push({ url: e.url, status: resp.status, body });
      } catch (err) {
        attempts.push({ url: e.url, error: err.message || String(err) });
      }
    }

    const error = new Error(
      `No se pudo obtener la preferencia ${preferenciaId}. Reintentos: ${JSON.stringify(attempts)}`
    );
    // console.error(`Error fetching preferencia ${preferenciaId}:`, attempts);
    throw error;
  },

  // Crear una nueva preferencia para un cliente existente
  async addPreferencia(nuevaPreferencia) {
    // console.log(" Enviando nueva preferencia (raw):", nuevaPreferencia);

    // Normalización: asegurar que preferenciaAmenidades sea un array de objetos
    const amenidadesRaw = Array.isArray(nuevaPreferencia.preferenciaAmenidades)
      ? nuevaPreferencia.preferenciaAmenidades
      : [];

    const resolved = amenidadesRaw
      .map((v) => {
        if (typeof v === "number" && Number.isFinite(v)) return { AmenidadId: Number(v), Nombre: `Amenidad ${Number(v)}` };
        if (typeof v === "string" && /^\d+$/.test(v)) return { AmenidadId: Number(v), Nombre: `Amenidad ${Number(v)}` };
        if (v && typeof v === "object") {
          const relId = Number(v.Id ?? v.id ?? v.preferenciaAmenidadId ?? NaN);
          const id = Number(v.AmenidadId ?? v.amenidadId ?? v.id ?? NaN);
          const name = v.Nombre ?? v.nombre ?? null;
          if (Number.isFinite(id)) return { Id: Number.isFinite(relId) ? relId : 0, AmenidadId: id, Nombre: name ?? `Amenidad ${id}` };
        }
        return null;
      })
      .filter(Boolean)
      .filter((it) => Number.isFinite(it.AmenidadId));

    // Deduplicar por AmenidadId
    const map = new Map();
    for (const it of resolved) {
      if (!map.has(it.AmenidadId)) {
        map.set(it.AmenidadId, { Id: it.Id ?? 0, AmenidadId: it.AmenidadId, Nombre: it.Nombre ?? `Amenidad ${it.AmenidadId}` });
      }
    }
    const cleanedPreferenciaAmenidades = Array.from(map.values());

    const payload = {
      ...nuevaPreferencia,
      id: 0, // asegurar id = 0 para creación
      preferenciaAmenidades: cleanedPreferenciaAmenidades,
    };

    // console.log(" Enviando nueva preferencia (limpia):", payload);

    const response = await fetch(`${getApiBaseUrl()}/Preferencia`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      const errorText = await response.text();
      // console.error("🚫 Respuesta inválida del servidor:", errorText);
      throw new Error(`Error HTTP: ${response.status} - ${errorText}`);
    }

    const data = await response.json();
    // console.log("✅ Preferencia creada (respuesta):", data);

    return data;
  },

  // Actualizar preferencia por ID
  async updatePreferencia(preferenciaId, payload) {
    try {
      if (!payload.id) {
        payload.id = Number(preferenciaId);
      }

      if (Array.isArray(payload.preferenciaAmenidades) && !Array.isArray(payload.PreferenciaAmenidades)) {
        payload.PreferenciaAmenidades = payload.preferenciaAmenidades;
      }

      if (Array.isArray(payload.preferenciaAmenidades)) {
        const resolved = payload.preferenciaAmenidades
          .map((v) => {
            if (typeof v === "number" && Number.isFinite(v)) return { AmenidadId: Number(v), Nombre: `Amenidad ${Number(v)}` };
            if (typeof v === "string" && /^\d+$/.test(v)) return { AmenidadId: Number(v), Nombre: `Amenidad ${Number(v)}` };
            if (v && typeof v === "object") {
              const relId = Number(v.Id ?? v.id ?? v.preferenciaAmenidadId ?? NaN);
              const id = Number(v.AmenidadId ?? v.amenidadId ?? v.id ?? NaN);
              const name = v.Nombre ?? v.nombre ?? null;
              if (Number.isFinite(id)) return { Id: Number.isFinite(relId) ? relId : 0, AmenidadId: id, Nombre: name ?? `Amenidad ${id}` };
            }
            return null;
          })
          .filter(Boolean)
          .filter((it) => Number.isFinite(it.AmenidadId));

        const map = new Map();
        for (const it of resolved) {
          if (!map.has(it.AmenidadId)) {
            map.set(it.AmenidadId, { Id: it.Id ?? 0, AmenidadId: it.AmenidadId, Nombre: it.Nombre ?? `Amenidad ${it.AmenidadId}` });
          }
        }
        payload.preferenciaAmenidades = Array.from(map.values());
        payload.PreferenciaAmenidades = Array.from(map.values());
      } else {
        if (!payload.PreferenciaAmenidades && payload.preferenciaAmenidades === undefined) {
          payload.PreferenciaAmenidades = [];
        }
      }

      const idNum = Number(preferenciaId);
      payload.id = idNum;
      payload.Id = idNum;
      payload.PreferenciaId = idNum;

      const wrapped = { preferenciaDto: payload };
      wrapped.preferenciaId = idNum;
      wrapped.preferenciaDto = wrapped.preferenciaDto || {};
      wrapped.preferenciaDto.id = idNum;
      wrapped.preferenciaDto.Id = idNum;
      wrapped.preferenciaDto.PreferenciaId = idNum;

      const response = await fetch(`${getApiBaseUrl()}/Preferencia/${preferenciaId}`, {
        method: "PUT",
        headers: getAuthHeaders(),
        body: JSON.stringify(wrapped),
      });

      if (!response.ok) {
        const text = await response.text().catch(() => "");
        let parsed = null;
        try { parsed = text ? JSON.parse(text) : null; } catch (e) { parsed = null; }

        let msg = `HTTP ${response.status}`;
        if (parsed) {
          if (parsed.errors && typeof parsed.errors === "object") {
            const parts = [];
            for (const k of Object.keys(parsed.errors)) {
              const v = parsed.errors[k];
              if (Array.isArray(v)) parts.push(`${k}: ${v.join("; ")}`);
              else parts.push(`${k}: ${String(v)}`);
            }
            msg += ` - ${parts.join(" | ")}`;
          } else if (parsed.title || parsed.message) {
            msg += ` - ${parsed.title || parsed.message}`;
          } else {
            msg += ` - ${JSON.stringify(parsed)}`;
          }
        } else if (text) {
          msg += ` - ${text}`;
        } else {
          msg += ` - ${response.statusText || "Sin cuerpo en la respuesta"}`;
        }

        throw new Error(msg);
      }

      return response.status === 204 ? null : await response.json();
    } catch (error) {
      // console.error(`Error updating preferencia with ID ${preferenciaId}:`, error);
      throw error;
    }
  },

  // Actualizar parcialmente con PATCH
  async patchPreferencia(preferenciaId, partialPayload) {
    // console.log(`Enviando PATCH para la preferencia ${preferenciaId} con el payload:`, partialPayload);

    if (partialPayload.id && Number(partialPayload.id) !== Number(preferenciaId)) {
      // console.warn(`ID del payload (${partialPayload.id}) no coincide con el ID de la URL (${preferenciaId}). Se usará el de la URL.`);
      partialPayload.id = Number(preferenciaId);
    } else if (!partialPayload.id) {
      partialPayload.id = Number(preferenciaId);
    }

    try {
      const response = await fetch(`${getApiBaseUrl()}/Preferencia/${preferenciaId}`, {
        method: "PATCH",
        headers: getAuthHeaders(),
        body: JSON.stringify(partialPayload),
      });

      if (!response.ok) {
        const errorText = await response.text().catch(() => "Sin cuerpo de respuesta");
        const errorMessage = `HTTP error! status: ${response.status}, message: ${errorText}`;
        // console.error("🚫 Error en la petición PATCH:", errorMessage);
        throw new Error(errorMessage);
      }

      if (response.status === 204) {
        // console.log("✅ Preferencia actualizada parcialmente con éxito (código 204).");
        return null;
      }

      const data = await response.json().catch(() => null);
      // console.log("✅ Preferencia actualizada parcialmente (respuesta):", data);
      return data;

    } catch (error) {
      // console.error(`Error al actualizar parcialmente la preferencia con ID ${preferenciaId}:`, error);
      throw error;
    }
  },

  // Eliminar preferencia por ID
  async deletePreferencia(preferenciaId) {
    try {
      const response = await fetch(`${getApiBaseUrl()}/Preferencia/${preferenciaId}`, {
        method: "DELETE",
        headers: getAuthHeaders(),
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        const errorMessage = errorData ? errorData.message || JSON.stringify(errorData) : response.statusText;
        throw new Error(`HTTP error! status: ${response.status}, message: ${errorMessage}`);
      }

      return true;
    } catch (error) {
      // console.error(`Error deleting preferencia with ID ${preferenciaId}:`, error);
      throw error;
    }
  },
};

export default requerimientoService;
