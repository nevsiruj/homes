<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 bg-gray-500/50 flex items-center justify-center z-50 p-4"
  >
    <div class="bg-white rounded-lg shadow-lg w-full max-w-2xl p-6 relative">
      <div class="flex justify-between items-center mb-4">
        <h3 class="text-xl font-bold text-gray-900">
          {{ isEditing ? "Editar Cliente" : "Agregar Cliente" }}
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
        <!-- Información Personal -->
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
              v-model="formData.nombre"
              type="text"
              id="nombre"
              required
              :disabled="isExistingClient && !cliente"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 disabled:opacity-60"
            />
          </div>
          <div>
            <label
              for="apellido"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Apellido</label
            >
            <input
              v-model="formData.apellido"
              type="text"
              id="apellido"
              required
              :disabled="isExistingClient && !cliente"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 disabled:opacity-60"
            />
          </div>
          <div>
            <label
              for="email"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Email</label
            >
            <input
              v-model="formData.email"
              type="text"
              id="email"
              :disabled="isExistingClient && !cliente"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5 disabled:opacity-60"
            />
          </div>
          <div>
            <label
              for="telefono"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Teléfono</label
            >
            <div class="flex">
              <select
                v-model="selectedCountryCode"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-l-lg focus:ring-gray-500 focus:border-gray-500 p-1 w-1/2"
              >
                <option
                  v-for="country in countries"
                  :key="country.code"
                  :value="country.callingCode"
                >
                  {{ country.emoji }} (+{{ country.callingCode }})
                </option>
              </select>
              <input
                v-model="formData.telefono"
                type="tel"
                id="telefono"
                required
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-r-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
              />
            </div>
          </div>
          <div>
            <label
              for="origen"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Viene desde:</label
            >
            <select
              v-model="formData.origen"
              id="origen"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            >
              <option value="">Selecciona tipo de contacto</option>
              <option value="WebHomes">Web Homes</option>
              <option value="WebLuxury">Web Luxury</option>
              <option value="IGHomes">IG Homes</option>
              <option value="IGLuxury">IG Luxury</option>
              <option value="FBHomes">FB Homes</option>
              <option value="FBLuxury">FB Luxury</option>
              <option value="WhatsApp">WhatsApp</option>
              <option value="PaginaExterna">Página Externa</option>
              <option value="Llamada">Llamada</option>
              <option value="ContactoPropio">Contacto Propio</option>
              <option value="Referido">Referido</option>
            </select>
          </div>
          <!-- NUEVO campo fechaRegistro -->
          <div>
            <label for="fechaRegistro" class="block mb-2 text-sm font-medium text-gray-900">
              Fecha de Registro
            </label>
            <input
              v-model="formData.fechaRegistro"
              type="date"
              id="fechaRegistro"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            />
          </div>
          <div v-if="isAdmin">
            <label
              for="agente"
              class="block mb-2 text-sm font-medium text-gray-900"
              >Asignar a:</label
            >
            <select
              v-model="selectedAgenteId"
              id="agente"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            >
              <option value="">Selecciona un agente</option>
              <option
                v-for="agente in agentes"
                :key="agente.id"
                :value="agente.id"
              >
                {{ agente.nombreCompleto }}
              </option>
            </select>
          </div>
        </div>

        <!-- Aviso solo cuando se detecta cliente existente durante creación -->
        <div
          v-if="isExistingClient && !cliente"
          class="mb-4 p-3 rounded bg-yellow-50 border border-yellow-200 text-sm text-yellow-800"
        >
          Cliente existente detectado. Solo estás agregando / modificando sus preferencias.
        </div>

        <!-- Preferencias -->
        <h4 class="text-lg font-medium text-gray-900 mb-4 mt-6 border-b pb-2">
          Requerimientos
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
  <option value="Amatitlán">Amatitlán</option>
  <option value="Antigua">Antigua</option>
  <option value="Atitlan">Atitlan</option>
  <option value="CAES Abajo KM 14">CAES Abajo KM 14</option>
  <option value="CAES Arriba KM 14">CAES Arriba KM 14</option>
  <option value="Carr. Salvador">Carr. Salvador</option>
  <option value="Fraijanes">Fraijanes</option>
  <option value="Mixco">Mixco</option>
  <option value="Monterrico">Monterrico</option>
  <option value="Muxbal">Muxbal</option>
  <option value="Playa">Playa</option>
  <option value="Puerto San José">Puerto San José</option>
  <option value="San Cristóbal">San Cristóbal</option>
  <option value="San José Pinula">San José Pinula</option>
  <option value="Santa Catarina Pinula">
    Santa Catarina Pinula
  </option>
  <option value="Zona 1">Zona 1</option>
  <option value="Zona 2">Zona 2</option>
  <option value="Zona 4">Zona 4</option>
  <option value="Zona 7">Zona 7</option>
  <option value="Zona 9">Zona 9</option>
  <option value="Zona 10">Zona 10</option>
  <option value="Zona 11">Zona 11</option>
  <option value="Zona 12">Zona 12</option>
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

          <!-- Mostrar Amenidades del cliente -->
          <!-- <div v-if="isEditing && clienteAmenidades.length > 0" class="mt-4">
  <h5 class="text-sm font-semibold text-gray-800   mb-2">Amenidades seleccionadas:</h5>
  <ul class="list-disc pl-5 space-y-1 text-sm text-gray-700 ">
    <li v-for="(amenidad, index) in clienteAmenidades" :key="index">
      {{ amenidad }}
    </li>
  </ul>
</div> -->
        </div>

        <!-- Notas Adicionales -->
        <h4 class="text-lg font-medium text-gray-900 mb-4 mt-6 border-b pb-2">
          Notas Adicionales
        </h4>
        <div class="mb-4">
          <label
            for="notas"
            class="block mb-2 text-sm font-medium text-gray-900"
            >Notas</label
          >
          <textarea
            v-model="formData.notas"
            id="notas"
            rows="4"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
            placeholder="Escribe cualquier nota relevante sobre el cliente..."
          ></textarea>
        </div>

        <!-- Botones -->
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
            {{ isEditing ? "Guardar cambios" : "Agregar cliente" }}
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
import agenteService from "@/services/agenteService";
import { codPais } from "../api/codigoPais";
import { useAuthStore } from "@/stores/auth";

// === Props ===
const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
    default: false,
  },
  isAdmin: {
    type: Boolean,
    default: false,
  },
  cliente: {
    type: Object,
    default: null,
  },
});
const emit = defineEmits(["close", "client-added", "client-updated"]);

// === Datos reactivos ===
const countries = ref(codPais);
const selectedCountryCode = ref(countries.value[0]?.callingCode || "502");
const isEditing = ref(false);
const selectedAgenteId = ref(null);

const allClientPreferences = ref([]); // NUEVO: Almacena todas las preferencias del cliente
const allClientInteractions = ref([]); // NUEVO: Almacena todas las interacciones del cliente
const formData = ref({
  id: null,
  nombre: "",
  apellido: "",
  email: "",
  telefono: "",
  dni: "",
  direccion: "",
  origen: "",
  notas: "",
  agenteId: "",
  fechaRegistro: "", // NUEVO
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
    amenidades: [], // IDs de las amenidades seleccionadas
  },
  interacciones: [],
});

const clienteAmenidades = ref([]); // Para mostrar en pantalla

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

const isLoadingCliente = ref(false);

// === Función para normalizar nombres ===
function normalize(str) {
  return str
    ?.toLowerCase()
    .normalize("NFKD")
    .replace(/[\u0300-\u036F]/g, "")
    .trim();
}

// === Agentes disponibles ===
const agentes = ref([]);

// NUEVO: bandera para cliente existente detectado por teléfono
const isExistingClient = ref(false);

// NUEVO: construir teléfono completo estandarizado
function buildFullPhone() {
  const raw = (formData.value.telefono || "").replace(/\D/g, "").replace(/^0+/, "");
  return `${selectedCountryCode.value}${raw}`;
}

// NUEVO: rellenar datos personales y bloquear edición
function fillClientData(cliente) {
  // Separar código país y número local
  let telefonoValue = cliente.telefono || "";
  let countryCode = selectedCountryCode.value;
  const foundCountry = countries.value.find(c => telefonoValue.startsWith(c.callingCode));
  if (foundCountry) {
    countryCode = foundCountry.callingCode;
    telefonoValue = telefonoValue.substring(countryCode.length);
  }
  selectedCountryCode.value = countryCode;

  formData.value.nombre = cliente.nombre || "";
  formData.value.apellido = cliente.apellido || "";
  formData.value.email = cliente.email || "";
  formData.value.id = cliente.id;
  formData.value.fechaRegistro = isoToDateInput(getFechaRegistro(cliente)); // ajustado
}

// NUEVO: verificar existencia por teléfono
const lastCheckedPhone = ref("");
let checkTimeout = null;

async function checkIfClientExists() {
  const fullPhone = buildFullPhone();
  if (!fullPhone || fullPhone.length < 8 || fullPhone === lastCheckedPhone.value) return;
  lastCheckedPhone.value = fullPhone;

  try {
    const cliente = await clienteService.getClienteByTelefono(fullPhone);
    if (cliente && cliente.id) {
      isExistingClient.value = true;
      isEditing.value = true; // reutiliza el flujo de actualización
      fillClientData(cliente);
      // Si viene con preferencias previas podrías mapearlas opcionalmente aquí
    } else {
      isExistingClient.value = false;
      if (!props.cliente) {
        isEditing.value = false;
        formData.value.id = null;
      }
    }
  } catch {
    // Si el servicio lanza error (404 u otro), se asume que no existe
    isExistingClient.value = false;
    if (!props.cliente) {
      isEditing.value = false;
      formData.value.id = null;
    }
  }
}

// NUEVO: watchers para teléfono y código de país
watch(
  () => formData.value.telefono,
  (nv) => {
    if (isEditing.value && isExistingClient.value) return; // evita spam mientras ya cargado
    clearTimeout(checkTimeout);
    if (!nv) {
      isExistingClient.value = false;
      return;
    }
    checkTimeout = setTimeout(() => checkIfClientExists(), 500); // debounce
  }
);

watch(
  () => selectedCountryCode.value,
  () => {
    if (formData.value.telefono) {
      clearTimeout(checkTimeout);
      checkTimeout = setTimeout(() => checkIfClientExists(), 300);
    }
  }
);

// === Cargar datos cuando cambia props.cliente (modo edición) ===
function mapClienteFromApi(clienteData) {
  // Teléfono / código país
  let telefonoValue = clienteData.telefono || "";
  let countryCode = "502";
  const foundCountry = countries.value.find((c) =>
    telefonoValue.startsWith(c.callingCode)
  );
  if (foundCountry) {
    countryCode = foundCountry.callingCode;
    telefonoValue = telefonoValue.substring(countryCode.length);
  }
  selectedCountryCode.value = countryCode;

  // Preferencias: tomar primera (o vacía)
  let preferenciasRaw = {};
  if (clienteData.preferencias?.$values?.length) {
    preferenciasRaw = clienteData.preferencias.$values[0];
  } else if (Array.isArray(clienteData.preferencias) && clienteData.preferencias.length > 0) {
    preferenciasRaw = clienteData.preferencias[0]; // <-- Cambio clave: Aceptar array directamente
  } else if (Array.isArray(clienteData.preferencias) && clienteData.preferencias.length) {
    preferenciasRaw = clienteData.preferencias[0];
  } else if (clienteData.preferencias && typeof clienteData.preferencias === "object") {
    preferenciasRaw = clienteData.preferencias;
  }

  // NUEVO: Guardar todas las preferencias existentes para no perderlas en la actualización
  if (Array.isArray(clienteData.preferencias?.$values)) {
    allClientPreferences.value = clienteData.preferencias.$values;
  } else if (Array.isArray(clienteData.preferencias)) {
    allClientPreferences.value = clienteData.preferencias;
  }

  // NUEVO: Guardar todas las interacciones existentes
  if (Array.isArray(clienteData.interacciones?.$values)) {
    allClientInteractions.value = clienteData.interacciones.$values;
  } else if (Array.isArray(clienteData.interacciones)) {
    allClientInteractions.value = clienteData.interacciones;
  }
  // Ubicaciones
  const ubicacionesArray = [];
  if (typeof preferenciasRaw.ubicacion === "string" && preferenciasRaw.ubicacion.trim()) {
    ubicacionesArray.push(
      ...preferenciasRaw.ubicacion.split(",").map(u => u.trim()).filter(Boolean)
    );
  }

  // Amenidades (robusto)
  const amenidadesFuente =
    preferenciasRaw.preferenciaAmenidades?.$values ||
    preferenciasRaw.preferenciaAmenidades ||
    [];
  const amenidadesSeleccionadas = [];
  clienteAmenidades.value = [];

  (amenidadesFuente || []).forEach(a => {
    const nombreAmen =
      a?.Nombre ||
      a?.nombre ||
      a?.amenidadInmueble?.nombre ||
      a?.amenidadInmueble?.Nombre ||
      "";
    if (nombreAmen) clienteAmenidades.value.push(nombreAmen);
    const norm = normalize(nombreAmen);
    const match = amenidadesDisponibles.value.find(am => normalize(am.nombre) === norm);
    if (match) amenidadesSeleccionadas.push(match.id);
  });

  formData.value = {
    id: clienteData.id ?? null,
    nombre: clienteData.nombre || "",
    apellido: clienteData.apellido || "",
    telefono: telefonoValue,
    email: clienteData.email || "",
    dni: clienteData.dni || "",
    direccion: clienteData.direccion || "",
    origen: clienteData.origen || "",
    notas: clienteData.notas || "",
    agenteId: clienteData.agenteId || "",
    fechaRegistro: isoToDateInput(getFechaRegistro(clienteData)), 
    preferencias: {
      id: preferenciasRaw.id || 0,
      tipo: preferenciasRaw.tipo || "",
      operacion: preferenciasRaw.operacion || "",
      ubicacion: ubicacionesArray,
      precioMin: preferenciasRaw.precioMin ?? null,
      precioMax: preferenciasRaw.precioMax ?? null,
      habitaciones: preferenciasRaw.habitaciones ?? null,
      banos: preferenciasRaw.banos ?? null,
      metrosCuadrados: preferenciasRaw.metrosCuadrados ?? null,
      amenidades: [...new Set(amenidadesSeleccionadas)],
    },
    interacciones: [],
  };
  selectedAgenteId.value = clienteData.agenteId || "";
}

// Reemplazado: watcher de props.cliente (siempre refetch)
watch(
  () => props.cliente,
  async (newVal) => {
    if (!newVal) {
      resetForm();
      allClientInteractions.value = []; // Limpiar interacciones al resetear
      allClientPreferences.value = []; // Limpiar preferencias al resetear
      return;
    }
    if (!newVal.id) return;
    isEditing.value = true;
    isLoadingCliente.value = true;
    try {
      const resp = await clienteService.getClienteById(newVal.id);
      const clienteData = resp?.$values ? resp.$values[0] : resp;
      if (clienteData) mapClienteFromApi(clienteData);
    } catch {
      // fallback: mantener lo que había
    } finally {
      isLoadingCliente.value = false;
    }
  }
  // Removido { immediate: true } - no necesitamos ejecutar en mount
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
        //console.error("Error al cargar agentes:", error);
      }
    }
  }
);

// Helper NUEVO: formatear ISO a valor para input date (sin hora)
function isoToDateInput(iso) {
  if (!iso) return "";
  const d = new Date(iso);
  if (isNaN(d.getTime())) return "";
  const pad = (n) => String(n).padStart(2, "0");
  const year = d.getFullYear();
  const month = pad(d.getMonth() + 1);
  const day = pad(d.getDate());
  return `${year}-${month}-${day}`;
}

// Helper NUEVO para leer fechaRegistro flexible
function getFechaRegistro(apiObj) {
  return apiObj.fechaRegistro || apiObj.FechaRegistro || "";
}

// Ajuste en resetForm para limpiar bandera
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
    notas: "",
    agenteId: "",
    fechaRegistro: "", // NUEVO
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
      amenidades: [],
    },
    interacciones: [],
  };
  isEditing.value = false;
  selectedAgenteId.value = null;
  selectedCountryCode.value = "502";
  clienteAmenidades.value = [];
  isExistingClient.value = false; // NUEVO
  allClientPreferences.value = []; // NUEVO: Limpiar al cerrar
  allClientInteractions.value = []; // NUEVO: Limpiar al cerrar
  lastCheckedPhone.value = "";
}

function closeModal() {
  resetForm();
  emit("close");
}

async function submitForm() {
    try {
        const loggedAgent = await agenteService.getCurrentUser();
        const agenteData = loggedAgent.$values
            ? loggedAgent.$values[0]
            : loggedAgent;
        const agenteId = agenteData.id || agenteData.userId || agenteData.usuarioId;
        if (!agenteId) throw new Error("No se encontró el ID del agente");

        const ubicacionStr = Array.isArray(formData.value.preferencias.ubicacion)
            ? formData.value.preferencias.ubicacion.filter(Boolean).join(",")
            : formData.value.preferencias.ubicacion || "";

        const preferenciaAmenidades = formData.value.preferencias.amenidades
            .map((id) => {
                const amenidad = amenidadesDisponibles.value.find((a) => a.id === id);
                return amenidad
                    ? { AmenidadId: amenidad.id, Nombre: amenidad.nombre }
                    : null;
            })
            .filter(Boolean);

        // Lógica para construir la lista de preferencias
        let finalPreferences = [];
        const editedPreferenceId = formData.value.preferencias.id || 0;

        // Añadir la preferencia que se está editando/creando
        const currentPreferencePayload = {
            Id: editedPreferenceId,
            Tipo: formData.value.preferencias.tipo || null,
            Operacion: formData.value.preferencias.operacion || null,
            Ubicacion: ubicacionStr,
            PrecioMin: formData.value.preferencias.precioMin || null,
            PrecioMax: formData.value.preferencias.precioMax || null,
            Habitaciones: formData.value.preferencias.habitaciones || null,
            Banos: formData.value.preferencias.banos || null,
            MetrosCuadrados: formData.value.preferencias.metrosCuadrados || null,
            PreferenciaAmenidades: preferenciaAmenidades,
        };

        if (isEditing.value) {
            // Reemplazar la preferencia editada en su posición original para mantener el orden
            const index = allClientPreferences.value.findIndex(p => p.id === editedPreferenceId);
            let tempPreferences = [...allClientPreferences.value];

            if (index > -1) {
                // Reemplaza el objeto en el índice encontrado con los datos del formulario
                // (currentPreferencePayload ya tiene la estructura del DTO con claves en mayúscula)
                tempPreferences[index] = currentPreferencePayload;
            } else {
                // Si por alguna razón no se encuentra (ej. es una nueva preferencia para un cliente existente), la añade al final.
                tempPreferences.push(currentPreferencePayload);
            }

            // Mapear todas las preferencias al formato DTO final para asegurar consistencia
            finalPreferences = tempPreferences.map(p => ({
                Id: p.Id ?? p.id,
                Tipo: p.Tipo ?? p.tipo,
                Operacion: p.Operacion ?? p.operacion,
                // ... y así sucesivamente para todos los campos, asegurando que el payload sea consistente
                Ubicacion: p.Ubicacion ?? p.ubicacion,
                PrecioMin: p.PrecioMin ?? p.precioMin,
                PrecioMax: p.PrecioMax ?? p.precioMax,
                Habitaciones: p.Habitaciones ?? p.habitaciones,
                Banos: p.Banos ?? p.banos,
                MetrosCuadrados: p.MetrosCuadrados ?? p.metrosCuadrados,
                PreferenciaAmenidades: (p.PreferenciaAmenidades || p.preferenciaAmenidades?.$values || p.preferenciaAmenidades || []).map(a => ({ AmenidadId: a.AmenidadId ?? a.amenidadId, Nombre: a.Nombre ?? a.nombre })),
            }));
        } else {
            // Si se está creando, solo se envía la nueva preferencia
            finalPreferences = [currentPreferencePayload];
        }

        // NUEVO: Asegurarse de que las interacciones existentes se incluyan en el payload de actualización
        const existingInteractions = isEditing.value ? allClientInteractions.value : [];


        const payload = {
            Id: isEditing.value ? parseInt(formData.value.id) : 0,
            AgenteId: selectedAgenteId.value || agenteId,
            Nombre: formData.value.nombre,
            Apellido: formData.value.apellido,
            Dni: formData.value.dni || null,
            Telefono: `${selectedCountryCode.value}${formData.value.telefono.replace(
                /^0+/,
                ""
            )}`,
            Origen: formData.value.origen || null,
            Direccion: formData.value.direccion || null,
            FechaRegistro: formData.value.fechaRegistro
                ? new Date(formData.value.fechaRegistro).toISOString().split("T")[0] // Solo fecha en formato ISO
                : new Date().toISOString().split("T")[0], // Solo fecha actual
            Notas: formData.value.notas || null,
            Email: formData.value.email || null,
            Status: 0,
            Preferencias: finalPreferences, // Usar la lista de preferencias construida
            Interacciones: existingInteractions, // <-- ¡CORRECCIÓN CLAVE!
        };

        let response;
        if (isEditing.value) {
            response = await clienteService.updateCliente(payload.Id, payload);
        } else {
            response = await clienteService.addCliente(payload);
        }

        Swal.fire(
            "Éxito",
            isEditing.value ? "Cliente actualizado" : "Cliente creado",
            "success"
        );

        // Agregar un delay de 2 segundos antes de cerrar el modal, emitir el evento y refrescar la página
        setTimeout(() => {
            if (isEditing.value) {
                emit("client-updated", response);
            } else {
                emit("client-added", response);
            }
            closeModal();
            window.location.reload(); // Refrescar la página
        },); // Delay de 2 segundos
    } catch (error) {
        Swal.fire(
            "Error",
            error.message || "Hubo un problema al guardar los datos.",
            "error"
        );
    }
}
</script>

<style scoped>
/* Estilos personalizados */
</style>
