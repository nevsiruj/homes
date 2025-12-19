<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 bg-gray-500/50 flex items-center justify-center z-[1050] p-4"
  >
    <div class="bg-white rounded-lg shadow-lg w-full max-w-2xl p-6 relative">
      <div class="flex justify-between items-center mb-4">
        <h3 class="text-xl font-bold text-gray-900">
          {{ isEditing ? "Editar Requerimiento" : "Agregar Requerimiento" }}
        </h3>
        <button
          @click="closeModal"
          class="text-gray-400 hover:bg-gray-200 hover:text-gray-900 rounded-lg p-1.5 ml-auto inline-flex"
        >
          <svg
            class="w-5 h-5"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </button>
      </div>

      <form
        @submit.prevent="submitForm"
        class="max-h-[70vh] overflow-y-auto pr-2"
      >
        <h4 class="text-lg font-medium text-gray-900 mb-4 mt-6 border-b pb-2">
          Información Personal
        </h4>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <div>
            <label
              for="nombre"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Nombre</label
            >
            <input
              :value="formData.nombre"
              type="text"
              id="nombre"
              readonly
              class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5"
            />
          </div>
          <div>
            <label
              for="apellido"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Apellido</label
            >
            <input
              :value="formData.apellido"
              type="text"
              id="apellido"
              readonly
              class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5"
            />
          </div>
        </div>

        <h4 class="text-lg font-medium text-gray-900 mb-4 mt-6 border-b pb-2">
          Preferencias de Inmuebles
        </h4>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <div>
            <label
              for="prefTipo"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Tipo de Propiedad</label
            >
            <select
              v-model="formData.preferencias.tipo"
              id="prefTipo"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            >
              <option value="">Selecciona tipo</option>
              <option value="Apartamento">Apartamento</option>
              <option value="Casa">Casa</option>
              <option value="Edificio">Edificio</option>
              <option value="Local">Local</option>
              <option value="Oficina">Oficina</option>
              <option value="Terreno">Terreno</option>
            </select>
          </div>
          <div>
            <label
              for="prefOperacion"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Operación</label
            >
            <select
              v-model="formData.preferencias.operacion"
              id="prefOperacion"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            >
              <option value="">Selecciona operación</option>
              <option value="Venta">Venta</option>
              <option value="Alquiler">Alquiler</option>
            </select>
          </div>
          <div class="md:col-span-2">
            <label class="block mb-2 text-sm font-medium text-gray-900"
              >Ubicación Preferida*</label
            >
            <p class="mb-2 text-xs font-medium text-gray-900">
              Manten precionada la <strong>tecla Ctrl</strong> para seleccionar
              varias ubicaciones
            </p>
            <select
              v-model="formData.preferencias.ubicacion"
              multiple
              size="5"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
            >
              <option value="Antigua">Antigua</option>
              <option value="CAES Arriba KM 14">CAES Arriba KM 14</option>
              <option value="Muxbal">Muxbal</option>
              <option value="San José Pinula">San José Pinula</option>
              <option value="Santa Catarina Pinula">
                Santa Catarina Pinula
              </option>
              <option value="Zona 4">Zona 4</option>
              <option value="Zona 9">Zona 9</option>
              <option value="Zona 10">Zona 10</option>
              <option value="Zona 13">Zona 13</option>
              <option value="Zona 14">Zona 14</option>
              <option value="Zona 15">Zona 15</option>
              <option value="Zona 16">Zona 16</option>
            </select>
          </div>
          <div>
            <label
              for="prefPrecioMin"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Precio Mínimo</label
            >
            <input
              v-model.number="formData.preferencias.precioMin"
              type="number"
              id="prefPrecioMin"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            />
          </div>
          <div>
            <label
              for="prefPrecioMax"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Precio Máximo</label
            >
            <input
              v-model.number="formData.preferencias.precioMax"
              type="number"
              id="prefPrecioMax"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            />
          </div>
          <div>
            <label
              for="prefHabitaciones"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Habitaciones</label
            >
            <input
              v-model.number="formData.preferencias.habitaciones"
              type="number"
              id="prefHabitaciones"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            />
          </div>
          <div>
            <label
              for="prefBanos"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Baños</label
            >
            <input
              v-model.number="formData.preferencias.banos"
              type="number"
              id="prefBanos"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            />
          </div>
          <div class="md:col-span-2">
            <label
              for="prefMetrosCuadrados"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Metros Cuadrados mínimos (m²)</label
            >
            <input
              v-model.number="formData.preferencias.metrosCuadrados"
              type="number"
              id="prefMetrosCuadrados"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            />
          </div>
          <div>
            <label class="block mb-2 text-sm font-medium text-gray-900"
              >Amenidades</label
            >
            <div class="grid grid-cols-1 gap-2">
              <div
                v-for="amenidad in amenidadesDisponibles"
                :key="amenidad.id"
                class="flex items-center"
              >
                <input
                  :id="'amenidad-' + amenidad.id"
                  type="checkbox"
                  :value="amenidad.id"
                  v-model="formData.preferencias.amenidades"
                  class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded focus:ring-gray-500 focus:ring-2"
                />
                <label
                  :for="'amenidad-' + amenidad.id"
                  class="ms-2 text-sm font-medium text-gray-900"
                  >{{ amenidad.nombre }}</label
                >
              </div>
            </div>
          </div>
          </div>

        <h4 class="text-lg font-medium text-gray-900 mb-4 mt-6 border-b pb-2">
          Comentarios
        </h4>
        <div class="mb-4">
          <label
            for="comentarios"
            class="block mb-2 text-sm font-medium text-gray-900"
            >Comentarios</label
          >
          <textarea
            v-model="formData.preferencias.comentarios"
            id="comentarios"
            rows="4"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            placeholder="Escribe cualquier comentario relevante sobre el requerimiento..."
          ></textarea>
        </div>

        <div class="flex justify-end gap-3 mt-6">
          <button
            @click="closeModal"
            type="button"
            class="bg-gray-500 text-white px-4 py-2 rounded"
          >
            Cerrar
          </button>
          <button
            type="submit"
            class="bg-gray-600 text-white px-4 py-2 rounded"
          >
            {{ isEditing ? "Guardar cambios" : "Agregar Requerimiento" }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, watch, onMounted } from "vue";
import Swal from "sweetalert2";
import clienteService from "@/services/clienteService";
import requerimientoService from "@/services/requerimientoService";
import agenteService from "@/services/agenteService";
import { codPais } from "../api/codigoPais";
import { useAuthStore } from "@/stores/auth";

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
  },
  clienteId: {
    type: [Number, String],
    default: null,
  },
  preferenciaId: {
    type: [Number, String],
    default: null,
  },
});
const emit = defineEmits(["close", "pref-changed"]);

const countries = ref(codPais);
const selectedCountryCode = ref(countries.value[0]?.callingCode || "502");
const isEditing = ref(false);
const selectedAgenteId = ref(null);

const formData = ref({
  id: null,
  nombre: "",
  apellido: "",
  email: "",
  telefono: "",
  dni: "",
  direccion: "",
  origen: "",
  
  agenteId: "",
  preferencias: {
    id: 0,
    tipo: "",
    operacion: "",
    ubicacion: [],
    precioMin: null,
    precioMax: null,
    habitaciones: null,
    banos: null,
    metrosCuadrados: null,
    comentarios: "",
    amenidades: [],
    // almacenar la lista "raw" de preferenciaAmenidades recibida del servidor
    // (cada item puede venir con Id = PK de la relación, AmenidadId y Nombre)
    preferenciaAmenidadesRaw: [],
  },
  interacciones: [],
});

const clienteAmenidades = ref([]);

const amenidadesDisponibles = ref([
  { id: 1, nombre: "Airbnb" },
  { id: 2, nombre: "Gimnasio" },
  { id: 3, nombre: "Área verde" },
  { id: 4, nombre: "Balcón" },
  { id: 5, nombre: "Parqueo de Visitas" },
  { id: 6, nombre: "Bodega" },
  { id: 7, nombre: "Estudio" },
  { id: 8, nombre: "Jardín" },
  { id: 9, nombre: "Lavandería" },
  { id: 10, nombre: "Cuarto de Servicio" },
  { id: 11, nombre: "Área de Juegos" },
  { id: 12, nombre: "Business Center" },
  { id: 13, nombre: "Pet friendly" },
  { id: 14, nombre: "Salón Social" },
  { id: 15, nombre: "Piscina" },
  { id: 16, nombre: "Pet Garden" },
  { id: 17, nombre: "Amueblado" },
  { id: 18, nombre: "No mascotas" },
  { id: 19, nombre: "Sala familiar" },
  { id: 20, nombre: "Canchas deportivas" },
]);

function normalize(str) {
  return str
    ?.toLowerCase()
    .normalize("NFKD")
    .replace(/[\u0300-\u036F]/g, "")
    .trim();
}

// Reemplazar la función deduplicateAmenidades por versión que devuelve solo IDs únicos (números)
function deduplicateAmenidades(raw) {
  // console técnico comentado:
  // console.log("[dedup] entrada raw:", raw);
  const arr = Array.isArray(raw) ? raw : [];

  const ids = arr
    .map((v) => {
      if (typeof v === "number" && Number.isFinite(v)) return Number(v);
      if (typeof v === "string" && /^\d+$/.test(v)) return Number(v);
      if (v && typeof v === "object") {
        const n = Number(v.amenidadId ?? v.AmenidadId ?? v.id ?? v.amenidadId ?? NaN);
        if (Number.isFinite(n)) return n;
      }
      return NaN;
    })
    .filter((n) => Number.isFinite(n));

  const uniqueIds = [...new Set(ids)];
  const removed = ids.filter((id, idx) => ids.indexOf(id) !== idx);
  // console técnico comentado:
  // if (removed.length) console.warn("[dedup] duplicados detectados y eliminados (ids):", [...new Set(removed)]);
  // console técnico comentado:
  // console.log("[dedup] ids únicos resultantes:", uniqueIds);
  return uniqueIds;
}

const agentes = ref([]);

watch(
  () => props.clienteId,
  async (newVal) => {
    if (newVal) {
      try {
        const id = typeof newVal === "string" && /^\d+$/.test(newVal) ? Number(newVal) : newVal;
        const resp = await clienteService.getClienteById(id);
        const clienteData = resp?.$values ? resp.$values[0] : resp;
        const cliente = clienteData || {};

        // Asignamos únicamente id, nombre y apellido.
        formData.value.id = cliente.id ?? id;
        formData.value.nombre = cliente.nombre || "";
        formData.value.apellido = cliente.apellido || "";

        // Forzamos que el modal abra en modo "crear" (no prellenar preferencias ni demás campos)
        isEditing.value = false;

        // No tocamos el resto de formData (telefono, email, preferencias, etc.)
      } catch (err) {
        // console técnico comentado:
        // console.error("Error loading cliente in modalRequerimiento:", err);
        Swal.fire("Error", "No se pudo obtener la información del cliente. Intenta nuevamente más tarde.", "error");
        resetForm();
      }
    } else {
      resetForm();
    }
  },
  { immediate: true }
);

// Watcher: cuando se recibe preferenciaId, cargar preferencia para editar
watch(
  () => props.preferenciaId,
  async (newVal) => {
    if (!newVal) {
      isEditing.value = false;
      formData.value.preferencias.id = 0;
      return;
    }

    if (!props.clienteId) {
      console.error("Cliente ID no definido. No se puede cargar la preferencia.");
      Swal.fire("Error", "No se pudo cargar la preferencia: Cliente ID no definido.", "error");
      return;
    }

    try {
      const prefId = Number(newVal);
      const clienteResp = await clienteService.getClienteById(props.clienteId);
      const clienteData = clienteResp?.$values ? clienteResp.$values[0] : clienteResp;

      if (!clienteData || !Array.isArray(clienteData.preferencias?.$values)) {
        throw new Error("Preferencias no disponibles en los datos del cliente.");
      }

      const data = clienteData.preferencias.$values.find((p) => Number(p.id) === prefId);
      if (!data) {
        throw new Error(`No se encontró la preferencia con ID ${prefId}.`);
      }

      // Mapear campos a formData.preferencias
      formData.value.preferencias.id = data.id ?? prefId;
      formData.value.preferencias.tipo = data.tipo ?? "";
      formData.value.preferencias.operacion = data.operacion ?? "";
      formData.value.preferencias.ubicacion = data.ubicacion
        ? data.ubicacion.split(",").map((s) => s.trim())
        : [];
      formData.value.preferencias.precioMin = data.precioMin ?? null;
      formData.value.preferencias.precioMax = data.precioMax ?? null;
      formData.value.preferencias.habitaciones = data.habitaciones ?? null;
      formData.value.preferencias.banos = data.banos ?? null;
      formData.value.preferencias.metrosCuadrados = data.metrosCuadrados ?? null;
      formData.value.preferencias.comentarios = data.comentarios ?? "";

      const rawAms = data.preferenciaAmenidades?.$values || [];
      formData.value.preferencias.amenidades = rawAms.map((a) => a.amenidadId);
      formData.value.preferencias.preferenciaAmenidadesRaw = rawAms.map((a) => ({
        Id: a.id,
        AmenidadId: a.amenidadId,
        Nombre: a.nombre,
      }));

      isEditing.value = true;
      formData.value.id = data.clienteId ?? formData.value.id ?? props.clienteId ?? null;
    } catch (err) {
      console.error("Error al cargar la preferencia:", err.message || err);
      Swal.fire("Error", "No se pudo cargar la preferencia para edición.", "error");
      isEditing.value = false;
    }
  },
  { immediate: false }
);

// === Cargar agentes solo cuando se abre el modal ===
watch(
  () => props.isOpen,
  async (isOpen) => {
    if (isOpen && agentes.value.length === 0) {
      try {
        const data = await agenteService.getUsers();
        agentes.value =
          data.$values?.map((agente) => ({
            id: agente.id,
            nombreCompleto: `${agente.nombre} ${agente.apellido}`,
          })) || [];
      } catch (error) {
        // console técnico comentado:
        // console.error("Error al cargar agentes:", error);
      }
    }
  }
);

function resetForm() {
  formData.value = {
    id: null,
    nombre: "",
    apellido: "",
    email: "",
    telefono: "",
    dni: "",
    direccion: "",
    origen: "",
   
    agenteId: "",
    preferencias: {
      id: 0,
      tipo: "",
      operacion: "",
      ubicacion: [],
      precioMin: null,
      precioMax: null,
      habitaciones: null,
      banos: null,
      metrosCuadrados: null,
      comentarios: "",
      amenidades: [],
      preferenciaAmenidadesRaw: [],
    },
    interacciones: [],
  };
  isEditing.value = false;
  selectedAgenteId.value = null;
  selectedCountryCode.value = "502";
  clienteAmenidades.value = [];
}

function closeModal() {
  resetForm();
  emit("close");
}

async function submitForm() {
  try {
    const loggedAgent = await agenteService.getCurrentUser();
    const agenteData = loggedAgent.$values ? loggedAgent.$values[0] : loggedAgent;
    const agenteId = agenteData.id || agenteData.userId || agenteData.usuarioId;
    if (!agenteId) throw new Error("NO_AGENT_ID");

    const ubicacionStr = Array.isArray(formData.value.preferencias.ubicacion)
      ? formData.value.preferencias.ubicacion.filter(Boolean).join(", ")
      : formData.value.preferencias.ubicacion || "";

    const amenidadIdsUnicos = deduplicateAmenidades(formData.value.preferencias.amenidades);

    let preferenciaAmenidades;
    if (isEditing.value) {
      // Para edición, mantener la estructura completa con el ID de la relación
      preferenciaAmenidades = amenidadIdsUnicos.map((id) => {
        const amen = amenidadesDisponibles.value.find((a) => Number(a.id) === Number(id));
        const existingRel = formData.value.preferencias?.preferenciaAmenidadesRaw?.find((r) => Number(r.AmenidadId) === Number(id)) ?? null;
        const relIdNum = existingRel && Number.isFinite(Number(existingRel.Id)) ? Number(existingRel.Id) : 0;
        return { Id: relIdNum, AmenidadId: Number(id), Nombre: amen?.nombre ?? `Amenidad ${id}` };
      });
    } else {
      // Para creación, enviar solo AmenidadId y Nombre, sin el Id de la relación
      preferenciaAmenidades = amenidadIdsUnicos.map((id) => {
        const amen = amenidadesDisponibles.value.find((a) => Number(a.id) === Number(id));
        return { AmenidadId: Number(id), Nombre: amen?.nombre ?? `Amenidad ${id}` };
      });
    }

    const clienteId = props.clienteId || formData.value.id || 0;
    const prefId = formData.value.preferencias?.id || 0;

    if (prefId && prefId > 0) {
      const payload = {
        id: Number(prefId),
        tipo: formData.value.preferencias.tipo || null,
        operacion: formData.value.preferencias.operacion || null,
        ubicacion: ubicacionStr,
        precioMin: formData.value.preferencias.precioMin || null,
        precioMax: formData.value.preferencias.precioMax || null,
        habitaciones: formData.value.preferencias.habitaciones || null,
        banos: formData.value.preferencias.banos || null,
        metrosCuadrados: formData.value.preferencias.metrosCuadrados || null,
        comentarios: formData.value.preferencias.comentarios || "",
        preferenciaAmenidades: preferenciaAmenidades,
      };

      const resp = await requerimientoService.patchPreferencia(prefId, payload);
      const savedPref = resp?.$values ? resp.$values[0] : resp;

      const ts = Date.now();
      const detail = { action: "updated", preferencia: savedPref, clienteId: clienteId, timestamp: ts };

      try { window.dispatchEvent(new CustomEvent("preferencia:changed", { detail })); } catch (e) { /* no-op */ }
      try { emit("pref-changed", detail); } catch (e) { /* no-op */ }

      Swal.fire("Éxito", "Preferencia actualizada correctamente.", "success");
    } else {
      const nuevaPreferencia = {
        Tipo: formData.value.preferencias.tipo || null,
        Operacion: formData.value.preferencias.operacion || null,
        Ubicacion: ubicacionStr,
        PrecioMin: formData.value.preferencias.precioMin || null,
        PrecioMax: formData.value.preferencias.precioMax || null,
        Habitaciones: formData.value.preferencias.habitaciones || null,
        Banos: formData.value.preferencias.banos || null,
        MetrosCuadrados: formData.value.preferencias.metrosCuadrados || null,
        Comentarios: formData.value.preferencias.comentarios || "",
        ClienteId: clienteId,
        PreferenciaAmenidades: preferenciaAmenidades,
      };

      const resp = await requerimientoService.addPreferencia(nuevaPreferencia);
      const createdPref = resp?.$values ? resp.$values[0] : resp;

      const ts = Date.now();
      const detail = { action: "created", preferencia: createdPref, clienteId: clienteId, timestamp: ts };
      try { window.dispatchEvent(new CustomEvent("preferencia:changed", { detail })); } catch (e) { /* no-op */ }
      try { emit("pref-changed", detail); } catch (e) { /* no-op */ }

      Swal.fire("Éxito", "Requerimiento agregado correctamente.", "success");
    }

    // Agregar un delay de 2 segundos antes de cerrar el modal y refrescar la página
    setTimeout(() => {
      closeModal();
      window.location.reload(); // Refrescar la página
    },); // Delay de 2 segundos
  } catch (error) {
    Swal.fire("Error", "No fue posible guardar el requerimiento. Por favor intenta de nuevo más tarde.", "error");
  }
}

</script>

<style scoped>
/* Estilos personalizados */
.fixed {
  z-index: 1050; /* Mayor que el z-index de detalleCliente.vue */
}
</style>
``` 