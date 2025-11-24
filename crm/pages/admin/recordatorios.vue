<template>
  <div>
    <div class="mt-5 mb-12">
      <h2
        class="text-2xl md:text-4xl lg:text-4xl font-extrabold  "
      >
        Historial de Contactos con Clientes
      </h2>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-4 mb-8">
      <div class="w-full">
        <form @submit.prevent class="flex flex-col sm:flex-row items-stretch sm:items-center gap-2 w-full">
          <select
            v-if="isAdmin"
            v-model="selectedAgent"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2.5 w-full sm:w-auto sm:min-w-[200px]"
          >
            <option value="all">Todos los agentes</option>
            <option v-for="agente in agentesArray" 
                    :key="agente.id" 
                    :value="agente.id">
              {{ agente.nombre }} {{ agente.apellido }}
            </option>
          </select>
          <div class="relative flex-1 w-full">
            <input
              type="text"
              v-model.trim="searchTerm"
              @input="handleSearch"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2.5"
              placeholder="Buscar cliente..."
            />
          </div>
          <button
            type="button"
            @click="handleSearch"
            class="inline-flex items-center justify-center py-2.5 px-4 text-sm font-medium text-white bg-gray-700 rounded-lg border border-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 w-full sm:w-auto whitespace-nowrap"
          >
            <svg
              class="w-4 h-4 sm:me-2"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 20 20"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
              />
            </svg>
            <span class="hidden sm:inline">Buscar</span>
          </button>
        </form>
      </div>
      <div class="flex items-end justify-end">
        <PaginationControls
          v-model:itemsPerPage="itemsPerPage"
          v-model:currentPage="currentPage"
          :perPageOptions="[10,20,50]"
        />
      </div>
    </div>

    <!-- Skeleton de carga -->
    <div v-if="isLoading" class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table class="w-full text-sm text-left rtl:text-right text-gray-500">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50">
          <tr>
            <th scope="col" class="px-6 py-3">
              <div class="flex items-center justify-between">
                <span>Fecha Registro</span>
                <span class="ml-1">↕️</span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">
              <div class="flex items-center justify-between">
                <span>Cliente</span>
                <span class="ml-1">↕️</span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">
              <div class="flex items-center justify-between">
                <span>Agente</span>
                <span class="ml-1">↕️</span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">
              <div class="flex items-center justify-between">
                <span>Tipo de Contacto</span>
                <span class="ml-1">↕️</span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">
              <div class="flex items-center justify-between">
                <span>Estado</span>
                <span class="ml-1">↕️</span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">
              <div class="flex items-center justify-between">
                <span>Fecha Próximo Contacto</span>
                <span class="ml-1">↕️</span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">Notas</th>
            <th scope="col" class="px-6 py-3">Acciones</th>
          </tr>
        </thead>
        <tbody class="animate-pulse">
          <tr v-for="i in 5" :key="`skeleton-${i}`" class="border-b">
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-24"></div></td>
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-32"></div></td>
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-28"></div></td>
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-20"></div></td>
            <td class="px-6 py-4"><div class="h-6 bg-gray-300 rounded w-32"></div></td>
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-24"></div></td>
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-40"></div></td>
            <td class="px-6 py-4"><div class="h-4 bg-gray-300 rounded w-20"></div></td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <div
      v-else-if="apiError"
      class="text-center py-8 text-red-500 "
    >
      {{ apiError }}
    </div>
    <div
      v-else-if="finalRecordatorios.length === 0 && !isLoading && !apiError"
      class="text-center py-8 text-gray-500 "
    >
      {{
        searchTerm
          ? "No se encontraron recordatorios que coincidan con la búsqueda."
          : "No hay recordatorios registrados."
      }}
    </div>

    <div v-else class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table
        class="w-full text-sm text-left rtl:text-right text-gray-500 "
      >
        <thead
          class="text-xs text-gray-700 uppercase bg-gray-50   "
        >
          <tr>
            <th scope="col" class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none" @click="sortBy('fechaRegistro')">
              <div class="flex items-center gap-2">
                Fecha Registro
                <span v-if="sortColumn === 'fechaRegistro'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none" @click="sortBy('clienteNombre')">
              <div class="flex items-center gap-2">
                Cliente
                <span v-if="sortColumn === 'clienteNombre'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none" @click="sortBy('agenteNombre')">
              <div class="flex items-center gap-2">
                Agente
                <span v-if="sortColumn === 'agenteNombre'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none" @click="sortBy('tipoInteraccion')">
              <div class="flex items-center gap-2">
                Tipo de Contacto
                <span v-if="sortColumn === 'tipoInteraccion'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none" @click="sortBy('estadoInteraccion')">
              <div class="flex items-center gap-2">
                Estado
                <span v-if="sortColumn === 'estadoInteraccion'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none" @click="sortBy('fechaProximoContacto')">
              <div class="flex items-center gap-2">
                Fecha Próximo Contacto
                <span v-if="sortColumn === 'fechaProximoContacto'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">Notas</th>
            <th scope="col" class="px-6 py-3">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="recordatorio in paginatedRecordatorios"
            :key="recordatorio.interaccionId"
            class="odd:bg-white odd: even:bg-gray-50 even: border-b  border-gray-200"
          >
            <td class="px-6 py-4">
              {{ formatDate(recordatorio.fechaRegistro, true) }}
            </td>
            <td
              class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap  "
            >
              {{ recordatorio.clienteNombre }}
            </td>
            <td class="px-6 py-4">{{ recordatorio.agenteNombre || '-' }}</td>
            <td class="px-6 py-4">{{ recordatorio.tipoInteraccion }}</td>
            <td class="px-6 py-4">
              <span
                :class="getEstadoClass(recordatorio.estadoInteraccionDisplay)"
              >
                {{ recordatorio.estadoInteraccionDisplay }}
              </span>
            </td>
            <td class="px-6 py-4">
              {{ formatDate(recordatorio.fechaProximoContacto) }}
            </td>
            <td class="px-6 py-4 max-w-xs truncate">
              {{ recordatorio.notasInteraccion }}
            </td>
            <td class="px-6 py-4">
              <button
                @click="openUpdateModal(recordatorio)"
                class="font-medium text-gray-600 hover:underline"
              >
                Actualizar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Controles de paginación -->
    <div class="flex flex-col sm:flex-row justify-between items-center mt-4 gap-4">
      <div class="text-sm text-gray-700">
        Mostrando {{ ((currentPage - 1) * itemsPerPage) + 1 }} a
        {{ Math.min(currentPage * itemsPerPage, totalRecordatorios) }} de
        {{ totalRecordatorios }} recordatorios
      </div>

      <div class="flex gap-2">
        <button
          @click="currentPage = 1"
          :disabled="currentPage === 1"
          class="px-3 py-1 text-sm border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-100"
        >
          ‹‹ Primera
        </button>
        <button
          @click="currentPage--"
          :disabled="currentPage === 1"
          class="px-3 py-1 text-sm border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-100"
        >
          ‹ Anterior
        </button>
        <span class="px-3 py-1 text-sm">
          Página {{ currentPage }} de {{ totalPages }}
        </span>
        <button
          @click="currentPage++"
          :disabled="currentPage >= totalPages"
          class="px-3 py-1 text-sm border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-100"
        >
          Siguiente ›
        </button>
        <button
          @click="currentPage = totalPages"
          :disabled="currentPage >= totalPages"
          class="px-3 py-1 text-sm border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-100"
        >
          Última ››
        </button>
      </div>
    </div>

    <div
      v-if="showUpdateModal"
      id="updateRecordatorioModal"
      tabindex="-1"
      aria-hidden="true"
      class="fixed inset-0 z-50"
    >
      <!-- Overlay -->
      <div class="absolute inset-0 bg-gray-500/50" @click="closeUpdateModal"></div>

      <!-- Wrapper centrado -->
      <div class="relative flex min-h-full items-center justify-center p-4">
        <div class="w-full max-w-2xl">
          <div class="bg-white rounded-lg shadow max-h-[90vh] overflow-y-auto">
            <!-- header -->
            <div
              class="flex items-center justify-between p-4 md:p-5 border-b rounded-t  "
            >
              <h3 class="text-xl font-semibold text-gray-900  ">
                Actualizar Recordatorio
              </h3>
              <button
                type="button"
                @click="closeUpdateModal"
                class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center    "
              >
                <svg
                  class="w-3 h-3"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 14 14"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                  />
                </svg>
                <span class="sr-only">Cerrar modal</span>
              </button>
            </div>
            <!-- body -->
            <div class="p-4 md:p-5 space-y-4">
              <div class="grid grid-cols-1 gap-4 mb-4">
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Cliente</label
                  >
                  <input
                    v-model="currentRecordatorio.clienteNombre"
                    type="text"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        "
                    disabled
                  />
                </div>
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Tipo de Contacto</label
                  >
                  <select
                    v-model="currentRecordatorio.tipoInteraccion"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        "
                  >
                    <option value="" disabled>Seleccione un tipo</option>
                    <option value="Llamada">Llamada</option>
                    <option value="Email">Email</option>
                    <option value="Visita">Visita</option>
                  </select>
                </div>
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Estado</label
                  >
                  <select
                    v-model="currentRecordatorio.estadoInteraccionRaw"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        "
                  >
                    <option
                      v-for="(value, key) in statusMap"
                      :key="key"
                      :value="parseInt(key)"
                    >
                      {{ formatStatusForDisplay(value) }}
                    </option>
                  </select>
                </div>
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Fecha Próximo Contacto</label
                  >
                  <input
                    v-model="currentRecordatorio.fechaProximoContactoRaw"
                    type="date"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        "
                  />
                </div>
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Notas</label
                  >
                  <textarea
                    v-model="currentRecordatorio.notasInteraccion"
                    rows="4"
                    class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-gray-500 focus:border-gray-500        "
                    placeholder="Agregue notas adicionales..."
                  ></textarea>
                </div>
              </div>
            </div>
            <!-- footer -->
            <div
              class="flex items-center p-4 md:p-5 border-t border-gray-200 rounded-b  "
            >
              <button
                @click="updateRecordatorio"
                type="button"
                class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center    "
              >
                Guardar cambios
              </button>
              <button
                @click="closeUpdateModal"
                type="button"
                class="ms-3 text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-gray-300 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10          "
              >
                Cancelar
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import Swal from "sweetalert2";
import { initFlowbite } from "flowbite";
import PaginationControls from "../../components/PaginationControls.vue";

// Ajusta rutas si es necesario
import clienteService from "../../services/clienteService";
import interaccionService from "../../services/interaccionService";
import { statusMap } from "../../api/constants";
import { useAuthStore } from "@/stores/auth";

const auth = useAuthStore();
const isAdmin = computed(() =>
  typeof auth.hasRole === "function"
    ? auth.hasRole("admin")
    : (auth.roles || []).includes("admin")
);

// New: resolve the logged-in agent ID robustly
const currentAgentId = computed(() => {
  const u = auth?.user || auth?.currentUser || auth;
  return u?.id ?? u?.Id ?? null;
});

definePageMeta({
  layout: "admin", requiresAuth: true,
});

// Reactive data
const lastInteractions = ref([]);
const isLoading = ref(false);
const apiError = ref(null);
const searchTerm = ref("");
const selectedAgent = ref('all'); // Default: todos los agentes
const showUpdateModal = ref(false);

// Paginación local
const currentPage = ref(1);
const itemsPerPage = ref(10);

const paginatedRecordatorios = computed(() => {
  const list = finalRecordatorios.value || [];
  const start = (currentPage.value - 1) * itemsPerPage.value;
  return list.slice(start, start + itemsPerPage.value);
});

const totalRecordatorios = computed(() => (finalRecordatorios.value || []).length);
const totalPages = computed(() => Math.max(1, Math.ceil(totalRecordatorios.value / itemsPerPage.value)));

// Variables para ordenamiento
const sortColumn = ref('fechaRegistro'); // Columna por defecto
const sortDirection = ref('desc'); // Descendente por defecto (más recientes primero)

// Helper para búsqueda acento-insensible y lowercase (igual que en clientes.vue)
function normalizeSearch(str) {
  if (!str && str !== 0) return "";
  return String(str)
    .normalize("NFD")
    .replace(/[\u0000-\u001F]/g, "")
    .replace(/[\u0300-\u036f]/g, "")
     .toLowerCase()
     .trim();
}
const currentRecordatorio = ref({
  interaccionId: null,
  clienteId: null,
  clienteNombre: "",
  tipoInteraccion: "",
  estadoInteraccionRaw: null, // Este ya es el número del estado
  estadoInteraccionDisplay: "",
  fechaProximoContacto: "",
  fechaProximoContactoRaw: "",
  notasInteraccion: "",
  fechaRegistro: "",
  AgenteId: null,
});

// Agregamos una ref para almacenar los detalles completos del cliente
// Esto es necesario para acceder a todas sus interacciones existentes
const clienteDetalleCompleto = ref(null);

// Lista reactiva de agentes para el filtro
const agentesArray = ref([]);

// Formatea nombres de estado
const formatStatusForDisplay = (statusKey) => {
  if (statusKey === undefined || statusKey === null) return "N/A"; // Si es un número (valor raw del backend), usamos statusMap
  if (typeof statusKey === "number") {
    const statusText = statusMap[statusKey];
    if (!statusText) return "N/A"; // Formatear para mostrar en la UI
    return statusText
      .replace(/_/g, " ")
      .replace(/\b\w/g, (l) => l.toUpperCase())
      .replace("24h", "24 Horas")
      .replace("Largo Plazo", "Largo Plazo");
  } // Si ya es un string (como "contacto_inicial_pendiente")
  if (typeof statusKey === "string") {
    return statusKey
      .replace(/_/g, " ")
      .replace(/\b\w/g, (l) => l.toUpperCase())
      .replace("24h", "24 Horas")
      .replace("Largo Plazo", "Largo Plazo");
  }
  return "N/A";
};

// MODIFICADO: Función formatDate para incluir la hora
const formatDate = (dateString, includeTime = false) => {
  if (!dateString) return "";
  let date = new Date(dateString);
  if (isNaN(date.getTime())) {
    const isoDate = dateString.includes("T")
      ? dateString
      : `${dateString}T00:00:00Z`;
    const parsedDate = new Date(isoDate);
    if (!isNaN(parsedDate.getTime())) {
      date = parsedDate;
    } else {
      //console.warn("Fecha inválida o formato desconocido:", dateString);
      return "Fecha inválida";
    }
  }
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  let formattedDate = `${day}/${month}/${year}`;

  if (includeTime) {
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    formattedDate += ` ${hours}:${minutes}`;
  }
  return formattedDate;
};

// Clase CSS según el estado
const getEstadoClass = (estadoDisplay) => {
  switch (estadoDisplay) {
    case "Contacto Inicial Pendiente":
      return "bg-gray-100 text-gray-800 text-xs font-medium px-2.5 py-0.5 rounded   ";
    case "Seguimiento 24 Horas":
      return "bg-yellow-100 text-yellow-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Seguimiento 48 Horas":
      return "bg-orange-100 text-orange-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Agendó Cita":
      return "bg-blue-100 text-blue-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Negociacion":
      return "bg-purple-100 text-purple-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Cierre Proceso":
      return "bg-indigo-100 text-indigo-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Cierre Exitoso":
    case "Cerrado":
      return "bg-green-100 text-green-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Seguimiento Largo Plazo":
      return "bg-teal-100 text-teal-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    case "Descartado":
    case "No Interesado":
      return "bg-red-100 text-red-800 text-xs font-medium px-2.5 py-0.5 rounded ";
    default:
      return "bg-gray-100 text-gray-800 text-xs font-medium px-2.5 py-0.5 rounded   ";
  }
};

// Función para ordenar columnas
const sortBy = (column) => {
  if (sortColumn.value === column) {
    // Si ya está ordenando por esta columna, cambiar dirección
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
  } else {
    // Nueva columna, ordenar ascendente
    sortColumn.value = column;
    sortDirection.value = 'asc';
  }
};

// Propiedad computada para filtrar y ordenar los recordatorios finales
const finalRecordatorios = computed(() => {
  let recordsToShow = lastInteractions.value;

  // New: if not admin, restrict to records of the logged-in agent
  if (!isAdmin.value) {
    const myId = currentAgentId.value;
    recordsToShow = myId
      ? recordsToShow.filter((r) => String(r.AgenteId || r.agenteId) === String(myId))
      : [];
  }

  // ...existing code...
  const raw = searchTerm.value || "";
  const tokens = normalizeSearch(raw)
    .split(/\s+/)
    .filter(Boolean);

  if (tokens.length) {
    recordsToShow = recordsToShow.filter((recordatorio) => {
      const searchFields = [
        recordatorio.clienteNombre || "",
        recordatorio.agenteNombre || "",
        recordatorio.AgenteId || recordatorio.agenteId || "",
        recordatorio.estadoInteraccionDisplay || "",
        recordatorio.tipoInteraccion || "",
        recordatorio.notasInteraccion || "",
      ];
      const haystack = normalizeSearch(searchFields.join(" "));
      return tokens.every((t) => haystack.includes(t));
    });
  }

  // Mantener filtro por agente seleccionado solo para admin
  if (isAdmin.value && selectedAgent.value && selectedAgent.value !== 'all') {
    const sel = String(selectedAgent.value);
    recordsToShow = recordsToShow.filter((recordatorio) =>
      String(recordatorio.AgenteId || recordatorio.agenteId) === sel
    );
  }

  // Ordenamiento dinámico por columna seleccionada
  recordsToShow.sort((a, b) => {
    let valueA, valueB;

    switch (sortColumn.value) {
      case 'fechaRegistro':
        valueA = a.fechaRegistro ? new Date(a.fechaRegistro).getTime() : 0;
        valueB = b.fechaRegistro ? new Date(b.fechaRegistro).getTime() : 0;
        break;
      case 'clienteNombre':
        valueA = (a.clienteNombre || '').toLowerCase();
        valueB = (b.clienteNombre || '').toLowerCase();
        break;
      case 'agenteNombre':
        valueA = (a.agenteNombre || '').toLowerCase();
        valueB = (b.agenteNombre || '').toLowerCase();
        break;
      case 'tipoInteraccion':
        valueA = (a.tipoInteraccion || '').toLowerCase();
        valueB = (b.tipoInteraccion || '').toLowerCase();
        break;
      case 'estadoInteraccion':
        valueA = (a.estadoInteraccionDisplay || '').toLowerCase();
        valueB = (b.estadoInteraccionDisplay || '').toLowerCase();
        break;
      case 'fechaProximoContacto':
        valueA = a.fechaProximoContacto ? new Date(a.fechaProximoContacto).getTime() : Infinity;
        valueB = b.fechaProximoContacto ? new Date(b.fechaProximoContacto).getTime() : Infinity;
        break;
      default:
        return 0;
    }

    // Comparar valores
    let comparison = 0;
    if (valueA < valueB) {
      comparison = -1;
    } else if (valueA > valueB) {
      comparison = 1;
    }

    // Aplicar dirección de ordenamiento
    return sortDirection.value === 'asc' ? comparison : -comparison;
  });

  return recordsToShow;
});

// Carga recordatorios desde la API
const loadRecordatorios = async () => {
  isLoading.value = true;
  apiError.value = null;
  lastInteractions.value = [];

  try {
    console.log("🔄 Iniciando carga de recordatorios...");
    
    const clientesResponse = await clienteService.getAllClientes(1, 1000);
    console.log("📥 Respuesta de clientes:", clientesResponse);
    
    const clientesRaw = clientesResponse.data || clientesResponse.$values || [];
    console.log("📋 Clientes procesados:", clientesRaw.length);
    console.log("🔍 Primer cliente (ejemplo):", clientesRaw[0]);
    console.log("🔑 Claves del primer cliente:", clientesRaw[0] ? Object.keys(clientesRaw[0]) : 'No hay clientes');
    
    const clientesMap = new Map();
    clientesRaw.forEach((cliente) => {
      // Soportar tanto 'id' como 'Id' o 'ID'
      const clienteId = cliente.id || cliente.Id || cliente.ID;
      if (clienteId) {
        clientesMap.set(clienteId, cliente);
      }
    });
    
    console.log("🗺️ ClientesMap size:", clientesMap.size);
    console.log("🔑 Primeras 5 claves del map:", Array.from(clientesMap.keys()).slice(0, 5));

    // Cargar agentes para mostrar nombre en la vista y poder filtrar por agente
    let agentesMap = new Map();
    try {
      const agenteService = (await import("../../services/agenteService")).default;
      const agentesResp = await agenteService.getUsers();
      const agentesRaw = agentesResp.$values || agentesResp || [];
      console.log("👥 Agentes cargados:", agentesRaw.length);
      
      agentesRaw.forEach((ag) => {
        agentesMap.set(ag.id, ag);
      });
      // Guardar en la lista reactiva de agentes
      agentesArray.value = Array.from(agentesMap.values());
    } catch (e) {
      console.warn("⚠️ Error al cargar agentes:", e);
      // no bloquear si falla la carga de agentes
      agentesMap = new Map();
    }

    const interaccionesResponse =
      await interaccionService.getAllInteracciones();
    console.log("📞 Respuesta de interacciones:", interaccionesResponse);
    console.log("📞 Tipo de respuesta:", typeof interaccionesResponse);
    console.log("📞 Es array:", Array.isArray(interaccionesResponse));
    console.log("📞 Tiene $values:", interaccionesResponse?.$values);
    
    // Normalizar respuesta - puede venir directamente como array o dentro de $values
    const interaccionesRaw = Array.isArray(interaccionesResponse) 
      ? interaccionesResponse 
      : (interaccionesResponse.$values || interaccionesResponse.data || []);
    
    console.log("📋 Interacciones procesadas:", interaccionesRaw.length);
    console.log("🔍 Primera interacción (ejemplo):", interaccionesRaw[0]);
    
    const latestInteractionsMap = new Map();

    let processedCount = 0;
    let skippedCount = 0;
    
    interaccionesRaw.forEach((interaccion) => {
      // Intentar con diferentes variaciones del ID
      const clienteId = interaccion.clienteId || interaccion.ClienteId || interaccion.CLIENTEID;
      const cliente = clientesMap.get(clienteId);
      
      if (!cliente) {
        skippedCount++;
        if (skippedCount <= 3) {
          console.warn(`⚠️ Cliente no encontrado. ClienteId de interacción: ${clienteId} (tipo: ${typeof clienteId}), InteraccionId: ${interaccion.id}`);
          console.warn(`🔍 Claves disponibles en map:`, Array.from(clientesMap.keys()).slice(0, 10));
        }
        return;
      }
      
      processedCount++;
      
      if (cliente) {
        // Obtener el estado numérico y convertirlo a texto
        const estadoNumerico = cliente.status;
        const estadoTexto = statusMap[estadoNumerico] || "desconocido";
        const agenteObj = agentesMap.get(interaccion.agenteId);
        const agenteNombre = agenteObj
          ? `${agenteObj.nombre || ""} ${agenteObj.apellido || ""}`.trim()
          : "";
        const processedInteraccion = {
          interaccionId: interaccion.id,
          clienteId: interaccion.clienteId,
          clienteNombre:
            `${cliente.nombre || ""} ${cliente.apellido || ""}`.trim() ||
            "Cliente Desconocido",
          tipoInteraccion: interaccion.tipo || "N/A",
          estadoInteraccionRaw: estadoNumerico, // Guardar el valor numérico original
          estadoInteraccionDisplay: formatStatusForDisplay(estadoTexto),
          fechaProximoContacto: interaccion.fechaVencimiento,
          fechaProximoContactoRaw: interaccion.fechaVencimiento
            ? new Date(interaccion.fechaVencimiento).toISOString().split("T")[0]
            : "",
          notasInteraccion: interaccion.descripcion || "Sin notas",
          fechaRegistro: interaccion.fecha,
          AgenteId: interaccion.agenteId,
          agenteNombre: agenteNombre,
          fechaInteraccionReal: new Date(interaccion.fecha),
        }; // Mantener solo la interacción más reciente por cliente

        const existing = latestInteractionsMap.get(interaccion.clienteId);
        if (
          !existing ||
          processedInteraccion.fechaInteraccionReal >
            existing.fechaInteraccionReal
        ) {
          latestInteractionsMap.set(
            interaccion.clienteId,
            processedInteraccion
          );
        }
      }
    });

    console.log(`📊 Interacciones con cliente: ${processedCount}, sin cliente: ${skippedCount}`);
    
    lastInteractions.value = Array.from(latestInteractionsMap.values());
    console.log("✅ Recordatorios procesados:", lastInteractions.value.length);
  } catch (err) {
    console.error("❌ Error al cargar recordatorios:", err);
    apiError.value = "Error al cargar datos: " + (err.message || err);
    
    // Mostrar alerta amigable
    await Swal.fire({
      icon: 'error',
      title: 'Error al cargar recordatorios',
      text: 'No fue posible cargar la lista de recordatorios. Por favor, intenta nuevamente.',
      confirmButtonText: 'Entendido'
    });
  } finally {
    isLoading.value = false;
  }
};

// Maneja la búsqueda
const handleSearch = () => {
  if (
    searchTerm.value.trim() &&
    finalRecordatorios.value.length === 0 &&
    !isLoading.value
  ) {
    apiError.value =
      "No se encontraron recordatorios que coincidan con la búsqueda.";
  } else {
    apiError.value = null;
  }
};

// Abre modal de actualización
const openUpdateModal = async (recordatorio) => {
  // Hacemos esta función async
  isLoading.value = true;
  try {
    // Obtener el cliente completo con todas sus interacciones
    const fullCliente = await clienteService.getClienteById(
      recordatorio.clienteId
    );
    if (!fullCliente) throw new Error("Cliente completo no encontrado.");
    clienteDetalleCompleto.value = fullCliente; // Guardar el cliente completo

    currentRecordatorio.value = { ...recordatorio }; // Aseguramos que el estado raw sea el numérico del cliente, no el de la interacción
    currentRecordatorio.value.estadoInteraccionRaw =
      fullCliente.Status || fullCliente.status;
    //console.log(
    //   "openUpdateModal - currentRecordatorio:",
    //   currentRecordatorio.value
    // );
    currentRecordatorio.value.fechaProximoContactoRaw =
      recordatorio.fechaProximoContacto
        ? new Date(recordatorio.fechaProximoContacto)
            .toISOString()
            .split("T")[0]
        : "";
    showUpdateModal.value = true;
  } catch (error) {
    //console.error("Error al abrir modal de actualización:", error);
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "No se pudo cargar la información completa del cliente.",
      confirmButtonText: "Entendido",
    });
  } finally {
    isLoading.value = false;
  }
};

// Cierra modal de actualización
const closeUpdateModal = () => {
  showUpdateModal.value = false;
  clienteDetalleCompleto.value = null; // Limpiar al cerrar
};

// Actualiza recordatorio
const updateRecordatorio = async () => {
  try {
    isLoading.value = true;

    if (
      !currentRecordatorio.value?.clienteId ||
      !clienteDetalleCompleto.value
    ) {
      throw new Error("Datos incompletos para la actualización.");
    } // 1. Preparar la nueva interacción

    const nuevaInteraccion = {
      // No incluyas 'id' si el backend espera 0 para nuevas interacciones o lo genera automáticamente
      fecha: new Date().toISOString(),
      fechaVencimiento: currentRecordatorio.value.fechaProximoContactoRaw
        ? new Date(
            currentRecordatorio.value.fechaProximoContactoRaw + "T00:00:00"
          ).toISOString() // Asegurar formato ISO con hora para backend
        : null,
      tipo: currentRecordatorio.value.tipoInteraccion,
      descripcion: currentRecordatorio.value.notasInteraccion,
      clienteId: currentRecordatorio.value.clienteId,
      agenteId: currentRecordatorio.value.AgenteId,
      statusInteraccion:
        statusMap[currentRecordatorio.value.estadoInteraccionRaw], // Texto del estado
    };

    // console.log(
    //   "📞 Nueva interacción a añadir:",
    //   JSON.stringify(nuevaInteraccion, null, 2)
    // ); // 2. Clonar las interacciones existentes y añadir la nueva // Asumiendo que el backend espera una lista de DTOs de interacciones

    const interaccionesParaPayload =
      clienteDetalleCompleto.value.Interacciones?.$values || [];
    const mappedInteracciones = interaccionesParaPayload.map((interaccion) => ({
      Id: interaccion.Id,
      Tipo: interaccion.Tipo,
      Descripcion: interaccion.Descripcion,
      Fecha: interaccion.Fecha,
      FechaVencimiento: interaccion.FechaVencimiento,
      ClienteId: interaccion.ClienteId,
      AgenteId: interaccion.AgenteId,
      StatusInteraccion: interaccion.StatusInteraccion, // Asumiendo que esta propiedad existe en el DTO de interaccion
    })); // Añadir la nueva interacción. Asumiendo que el backend asigna el Id.

    mappedInteracciones.push({
      Tipo: nuevaInteraccion.tipo,
      Descripcion: nuevaInteraccion.descripcion,
      Fecha: nuevaInteraccion.fecha,
      FechaVencimiento: nuevaInteraccion.fechaVencimiento,
      ClienteId: nuevaInteraccion.clienteId,
      AgenteId: nuevaInteraccion.agenteId,
      StatusInteraccion: nuevaInteraccion.statusInteraccion,
    }); // 3. Preparar payload del cliente

    const clientePayload = {
      Id: clienteDetalleCompleto.value.Id || clienteDetalleCompleto.value.id,
      Nombre:
        clienteDetalleCompleto.value.Nombre ||
        clienteDetalleCompleto.value.nombre ||
        "",
      Apellido:
        clienteDetalleCompleto.value.Apellido ||
        clienteDetalleCompleto.value.apellido ||
        "",
      Telefono:
        clienteDetalleCompleto.value.Telefono ||
        clienteDetalleCompleto.value.telefono ||
        "",
      Dni:
        clienteDetalleCompleto.value.Dni ||
        clienteDetalleCompleto.value.dni ||
        "",
      Email:
        clienteDetalleCompleto.value.Email ||
        clienteDetalleCompleto.value.email ||
        "",
      Origen:
        clienteDetalleCompleto.value.Origen ||
        clienteDetalleCompleto.value.origen ||
        "",
      FechaRegistro:
        clienteDetalleCompleto.value.FechaRegistro ||
        clienteDetalleCompleto.value.fechaRegistro,
      Preferencias: clienteDetalleCompleto.value.Preferencias,
      AgenteId:
        clienteDetalleCompleto.value.AgenteId ||
        clienteDetalleCompleto.value.agenteId, // ¡CLAVE: El estado numérico directamente!

      Status: currentRecordatorio.value.estadoInteraccionRaw,
      Notas: currentRecordatorio.value.notasInteraccion, // Usar las notas actualizadas del modal
      Interacciones: mappedInteracciones, // <--- ¡Esto es CRÍTICO!
    };

  //  console.log(
  //     "📤 Payload cliente a enviar:",
  //     JSON.stringify(clientePayload, null, 2)
  //   ); // El backend debe manejar la actualización del cliente y sus interacciones en una sola llamada // Si tu backend espera la interacción por separado, entonces solo llama a interaccionService.addInteraccion // y luego updateCliente sin las interacciones en el payload. // Pero si quieres que el status del cliente se actualice a través del flujo de updateCliente // tal como lo hacía el otro componente, es mejor enviar las interacciones aquí. // Si `interaccionService.addInteraccion` es necesario antes, déjalo. // Si `updateCliente` maneja las interacciones por sí mismo, quita esta línea. // NO LLAMES interaccionService.addInteraccion AHI SI


    await loadRecordatorios();

    Swal.fire({
      icon: "success",
      title: "¡Actualizado!",
      text: "El estado y la interacción se guardaron correctamente",
      timer: 1500,
      showConfirmButton: false,
    });

    closeUpdateModal();
  } catch (error) {
    // console.error("❌ Error completo en updateRecordatorio:", {
    //   message: error.message,
    //   response: error.response?.data,
    //   stack: error.stack,
    // });

    Swal.fire({
      icon: "error",
      title: "Error",
      text:
        error.response?.data?.title || error.message || "Error al actualizar",
      confirmButtonText: "Entendido",
    });
  } finally {
    isLoading.value = false;
  }
};
// Inicialización al montar
onMounted(async () => {
  initFlowbite();
  await loadRecordatorios();
});
</script>

<style scoped>
/* Estilos para las columnas ordenables */
th.cursor-pointer {
  transition: background-color 0.2s ease;
  user-select: none;
}

th.cursor-pointer:hover {
  background-color: #f3f4f6 !important;
}

th.cursor-pointer:active {
  background-color: #e5e7eb !important;
}
</style>
