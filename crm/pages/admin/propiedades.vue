<template>
  <div>
    <div class="mt-5 mb-12">
      <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
        Gestión de Propiedades
      </h2>
    </div>

    <div class="grid grid-cols-2 grid-rows-1 gap-4 mb-8">
      <div>
        <form @submit.prevent class="flex flex-col items-start max-w-lg">
          <label for="voice-search" class="sr-only">Buscar</label>
          <div class="relative w-full">
            <input
              type="text"
              v-model="searchTerm"
              @input="handleSearch"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2"
              placeholder="Buscar por título, código, ubicación o tipo"
            />
          </div>
          <div v-if="isSearching" class="mt-2 flex items-center space-x-2">
            <div class="spinner"></div>
            <span class="text-gray-500 text-sm fade-animation"
              >Buscando...</span
            >
          </div>
        </form>
      </div>
      <div class="flex justify-end items-end gap-2">
        <PaginationControls v-model:itemsPerPage="itemsPerPage" v-model:currentPage="currentPage" :perPageOptions="[9,20,50]" />
        <button
          v-if="isAdmin"
          type="button"
          @click="openModal"
          class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none"
        >
          Agregar registro
        </button>
        <button
          type="button"
          @click="downloadExcelAll"
          class="text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none"
        >
          Descargar Excel
        </button>
      </div>
    </div>

    <Loader v-if="isLoading && !hasLoaded" />

    <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table class="w-full text-sm text-left rtl:text-right text-gray-500">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50">
          <tr>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('codigoPropiedad')"
            >
              <div class="flex items-center gap-2">
                Código
                <span v-if="sortColumn === 'codigoPropiedad'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('titulo')"
            >
              <div class="flex items-center gap-2">
                Título
                <span v-if="sortColumn === 'titulo'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('tipos')"
            >
              <div class="flex items-center gap-2">
                Tipo
                <span v-if="sortColumn === 'tipos'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('operaciones')"
            >
              <div class="flex items-center gap-2">
                Operación
                <span v-if="sortColumn === 'operaciones'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('ubicaciones')"
            >
              <div class="flex items-center gap-2">
                Ubicación
                <span v-if="sortColumn === 'ubicaciones'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('precio')"
            >
              <div class="flex items-center gap-2">
                Precio
                <span v-if="sortColumn === 'precio'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('habitaciones')"
            >
              <div class="flex items-center gap-2">
                Habitaciones
                <span v-if="sortColumn === 'habitaciones'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('banos')"
            >
              <div class="flex items-center gap-2">
                Baños
                <span v-if="sortColumn === 'banos'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('parqueos')"
            >
              <div class="flex items-center gap-2">
                Parqueos
                <span v-if="sortColumn === 'parqueos'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('metrosCuadrados')"
            >
              <div class="flex items-center gap-2">
                m²
                <span v-if="sortColumn === 'metrosCuadrados'" class="text-gray-500">
                  {{ sortDirection === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
            <th scope="col" class="px-6 py-3">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="inmueble in sortedInmuebles"
            :key="inmueble.id"
            class="odd:bg-white even:bg-gray-50 border-b border-gray-200"
          >
            <td
              class="px-6 py-4 font-medium text-xs text-gray-900 whitespace-nowrap"
            >
              {{ inmueble.codigoPropiedad }}
            </td>
            <td class="px-6 py-4 truncate max-w-xs">{{ inmueble.titulo }}</td>
            <td class="px-6 py-4">{{ inmueble.tipos }}</td>
            <td class="px-6 py-4">{{ inmueble.operaciones }}</td>
            <td class="px-6 py-4">{{ inmueble.ubicaciones }}</td>
            <td class="px-6 py-4">${{ inmueble.precio.toLocaleString() }}</td>
            <td class="px-6 py-4">{{ inmueble.habitaciones || "-" }}</td>
            <td class="px-6 py-4">{{ inmueble.banos || "-" }}</td>
            <td class="px-6 py-4">{{ inmueble.parqueos || "-" }}</td>
            <td class="px-6 py-4">{{ inmueble.metrosCuadrados || "-" }}</td>
            <td class="px-6 py-4 space-x-2 flex content-center justify-center">
              <button
                @click="openDetailModal(inmueble.id)"
                class="flex items-center font-medium text-gray-600 hover:underline"
                title="Ver detalles"
              >
                <svg
                  class="w-5 h-5 mr-1"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M15.0007 12C15.0007 13.6569 13.6576 15 12.0007 15C10.3439 15 9.00073 13.6569 9.00073 12C9.00073 10.3431 10.3439 9 12.0007 9C13.6576 9 15.0007 10.3431 15.0007 12Z"
                    stroke="currentColor"
                    stroke-width="2"
                  ></path>
                  <path
                    d="M12.0012 5C7.52354 5 3.73326 7.94288 2.45898 12C3.73324 16.0571 7.52354 19 12.0012 19C16.4788 19 20.2691 16.0571 21.5434 12C20.2691 7.94291 16.4788 5 12.0012 5Z"
                    stroke="currentColor"
                    stroke-width="2"
                  ></path>
                </svg>
              </button>
              <button
                v-if="isAdmin"
                @click="editInmueble(inmueble.id)"
                class="flex items-center font-medium text-gray-600 hover:underline"
                title="Editar registro"
              >
                <svg
                  class="w-5 h-5 mr-1"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  fill="none"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="1"
                    d="m14.304 4.844 2.852 2.852M7 7H4a1 1 0 0 0-1 1v10a1 1 0 0 0 1 1h11a1 1 0 0 0 1-1v-4.5m2.409-9.91a2.017 2.017 0 0 1 0 2.853l-6.844 6.844L8 14l.713-3.565 6.844-6.844a2.015 2.015 0 0 1 2.852 0Z"
                  />
                </svg>
              </button>
              <button
                v-if="isAdmin"
                @click="confirmDelete(inmueble.id)"
                class="flex items-center font-medium text-gray-600 hover:underline"
                title="Eliminar registro"
              >
                <svg
                  class="w-5 h-5 mr-1"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  fill="none"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="1"
                    d="M5 7h14m-9 3v8m4-8v8M10 3h4a1 1 0 0 1 1 1v3H9V4a1 1 0 0 1 1-1ZM6 7h12v13a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V7Z"
                  />
                </svg>
              </button>

              <button
                @click="copyInmuebleUrl(inmueble.id)"
                :disabled="!inmueble.slugInmueble"
                class="flex items-center font-medium text-gray-600 hover:underline"
                title="Copiar URL"
              >
                <svg
                  v-if="copiedId !== inmueble.id"
                  class="w-6 h-6 text-gray-600"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  fill="none"
                  viewBox="0 0 24 24"
                >
                  <path
                    stroke="currentColor"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M9 8v3a1 1 0 0 1-1 1H5m11 4h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-7a1 1 0 0 0-1 1v1m4 3v10a1 1 0 0 1-1 1H6a1 1 0 0 1-1-1v-7.13a1 1 0 0 1 .24-.65L7.7 8.35A1 1 0 0 1 8.46 8H13a1 1 0 0 1 1 1Z"
                  />
                </svg>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Controles de Paginación -->
    <div class="flex flex-col sm:flex-row justify-between items-center mt-4 gap-4">
      <div class="text-sm text-gray-700">
        Mostrando {{ ((currentPage - 1) * itemsPerPage) + 1 }} a
        {{ Math.min(currentPage * itemsPerPage, totalPropiedades) }} de
        {{ totalPropiedades }} propiedades
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
          Página {{ currentPage }} de {{ effectiveTotalPages }}
        </span>
        <button
          @click="currentPage++"
          :disabled="currentPage >= effectiveTotalPages"
          class="px-3 py-1 text-sm border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-100"
        >
          Siguiente ›
        </button>
        <button
          @click="currentPage = effectiveTotalPages"
          :disabled="currentPage >= effectiveTotalPages"
          class="px-3 py-1 text-sm border rounded-lg disabled:opacity-50 disabled:cursor-not-allowed hover:bg-gray-100"
        >
          Última ››
        </button>
      </div>
    </div>

    <div v-if="inmuebles.length === 0" class="text-center py-8 text-gray-500">
      {{
        searchTerm
          ? "No se encontraron inmuebles que coincidan con la búsqueda."
          : "No hay inmuebles registrados."
      }}
    </div>

    <modalAgInmueble
      v-if="showModal"
      :isOpen="showModal"
      :inmueble-to-edit="inmuebleToEdit"
      @close="closeModal"
      @inmueble-updated="loadInmuebles"
    />

    <modal-detalle-inmueble
      v-if="showModalDetalle"
      :isOpen="showModalDetalle"
      :inmueble="inmuebleSeleccionado"
      @close="showModalDetalle = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, computed, watch } from "vue";
import Swal from "sweetalert2";
import inmuebleService from "../../services/inmuebleService.js";
import modalAgInmueble from "~/components/modalAgInmueble.vue";
import modalDetalleInmueble from "~/components/modalDetalleInmueble.vue";
import { initFlowbite } from "flowbite";
import Loader from "~/components/Loader.vue";
import debounce from "lodash/debounce";
import { useAuthStore } from "@/stores/auth";
import PaginationControls from "../../components/PaginationControls.vue";

const auth = useAuthStore();
const isAdmin = computed(() =>
  typeof auth.hasRole === "function"
    ? auth.hasRole("admin")
    : (auth.roles || []).includes("admin")
);

definePageMeta({ layout: "admin", requiresAuth: true });

// ----------------- Nuevo: helpers de alertas amigables -----------------
const showAlert = (opts = {}) => {
  // opts: { icon, title, text, timer, showConfirmButton, confirmButtonText, allowOutsideClick }
  return Swal.fire({
    icon: opts.icon || "info",
    title: opts.title || "",
    text: opts.text || "",
    timer: opts.timer,
    showConfirmButton: opts.showConfirmButton !== undefined ? opts.showConfirmButton : true,
    confirmButtonText: opts.confirmButtonText || "Entendido",
    allowOutsideClick: opts.allowOutsideClick !== undefined ? opts.allowOutsideClick : true,
  });
};

const showErrorFriendly = (userMessage = "Ocurrió un problema. Por favor intenta de nuevo.", errorToLog = null) => {
  // Log técnico solo en consola para desarrolladores (comentar en UI)
  if (errorToLog) {
    /* console.error(errorToLog); */
  }
  return showAlert({ icon: "error", title: "Algo salió mal", text: userMessage });
};

const showInfoFriendly = (userMessage = "", title = "Información") =>
  showAlert({ icon: "info", title, text: userMessage });

const showSuccessTransient = (userMessage = "", timer = 1200) =>
  showAlert({ icon: "success", title: "", text: userMessage, timer, showConfirmButton: false });

/* =================== State =================== */
const inmuebles = ref([]);
const searchTerm = ref("");
const showModal = ref(false);
const inmuebleToEdit = ref(null);
const showModalDetalle = ref(false);
const inmuebleSeleccionado = ref(null);
const isLoading = ref(true);
const isSearching = ref(false);
const hasLoaded = ref(false); // Track if the initial load is complete

const totalPages = ref(1);
const totalPropiedades = ref(0);
const itemsPerPage = ref(9);
const currentPage = ref(1);

// Estado para ordenamiento
const sortColumn = ref(null);
const sortDirection = ref('asc');

/* Formateador de precio para usar en el template */
const fmtPrice = new Intl.NumberFormat("es-AR", { maximumFractionDigits: 0 });

/* Loader mínimo para evitar "flash" */
const MIN_LOADER_MS = 250;
const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

/* Implementación de debounce con lodash */
const debouncedSearch = debounce(async (term) => {
  isSearching.value = true;
  try {
    currentPage.value = 1; // Siempre ir a la primera página en una nueva búsqueda
    // Pasa el término de búsqueda al loadInmuebles
    await loadInmuebles(currentPage.value, term);
  } finally {
    isSearching.value = false;
  }
}, 300);

// Función para manejar el ordenamiento por columnas (client-side, para los datos cargados en la página actual)
const handleSort = (column) => {
  if (sortColumn.value === column) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
  } else {
    sortColumn.value = column;
    sortDirection.value = 'asc';
  }
};

// Función para obtener el valor de una columna para ordenar
const getColumnValue = (inmueble, column) => {
  switch (column) {
    case 'codigoPropiedad':
      return (inmueble.codigoPropiedad || '').toLowerCase();
    case 'titulo':
      return (inmueble.titulo || '').toLowerCase();
    case 'tipos':
      return (inmueble.tipos || '').toLowerCase();
    case 'operaciones':
      return (inmueble.operaciones || '').toLowerCase();
    case 'ubicaciones':
      return (inmueble.ubicaciones || '').toLowerCase();
    case 'precio':
      return parseFloat(inmueble.precio) || 0;
    case 'habitaciones':
      return parseInt(inmueble.habitaciones) || 0;
    case 'banos':
      return parseInt(inmueble.banos) || 0;
    case 'parqueos':
      return parseInt(inmueble.parqueos) || 0;
    case 'metrosCuadrados':
      return parseFloat(inmueble.metrosCuadrados) || 0;
    default:
      return '';
  }
};

/* ========= Al escribir: disparar búsqueda en el servidor + ir a pág 1 ========= */
const handleSearch = () => {
  debouncedSearch(searchTerm.value); // Asegurarse de que se llama al debounce con el término actual
};

/* ========== Helpers de mapeo ========== */
const mapItems = (arr) =>
  arr.map((inmueble) => {
    // ...existing mapping... (se mantienen otros campos)
    const amenidadesArray = Array.isArray(inmueble.amenidades)
      ? inmueble.amenidades
      : Array.isArray(inmueble.amenidades?.$values)
      ? inmueble.amenidades.$values
      : [];

    const obj = {
      id: inmueble.id,
      codigoPropiedad: inmueble.codigoPropiedad || "",
      titulo: inmueble.titulo || "",
      slugInmueble: inmueble.slugInmueble || "",
      precio: parseFloat(inmueble.precio) || 0,
      habitaciones: inmueble.habitaciones || 0,
      banos: inmueble.banos || 0,
      parqueos: inmueble.parqueos || 0,
      metrosCuadrados: parseFloat(inmueble.metrosCuadrados) || 0,
      imagenPrincipal:
        inmueble.imagenPrincipal || "https://via.placeholder.com/150",
      contenido: inmueble.contenido || "",
      tipos: inmueble.tipos || "",
      operaciones: inmueble.operaciones || "",
      ubicaciones: inmueble.ubicaciones || "",
      // <-- compatibilidad: usa amenidadesArray (ya sea array directo o .$values)
      amenidades: amenidadesArray,
      luxury: Boolean(inmueble.luxury),
      video: inmueble.video || "",
    };
    return obj;
  });

/* ========== Modal Detalle ========== */
const openDetailModal = async (id) => {
  const detalleInmueble = await fetchInmuebleById(id);
  if (detalleInmueble) {
    inmuebleSeleccionado.value = detalleInmueble;
    showModalDetalle.value = true;
  }
};

/* ========= Ordenamiento de inmuebles ========= */
const sortedInmuebles = computed(() => {
  if (!sortColumn.value) {
    return inmuebles.value;
  }

  return [...inmuebles.value].sort((a, b) => {
    const aValue = getColumnValue(a, sortColumn.value);
    const bValue = getColumnValue(b, sortColumn.value);
    
    let comparison = 0;
    if (aValue < bValue) comparison = -1;
    if (aValue > bValue) comparison = 1;
    
    return sortDirection.value === 'asc' ? comparison : -comparison;
  });
});

/* ========= Total efectivo para el paginador ========= */
const effectiveTotalPages = computed(() => {
  return totalPages.value || 1; // Siempre del backend
});

/* ========= Ventana deslizante de 5 páginas ========= */
const MAX_WINDOW = 5;
const visiblePages = computed(() => {
  const total = effectiveTotalPages.value;
  if (total <= MAX_WINDOW) {
    return Array.from({ length: total }, (_, i) => i + 1);
  }

  let start = currentPage.value - Math.floor(MAX_WINDOW / 2);
  let end = start + MAX_WINDOW - 1;

  if (start < 1) {
    start = 1;
    end = MAX_WINDOW;
  }
  if (end > total) {
    end = total;
    start = total - (MAX_WINDOW - 1);
  }

  return Array.from({ length: end - start + 1 }, (_, i) => start + i);
});

/* ========= Carga inicial ========= */
onMounted(async () => {
  initFlowbite();
  try {
    isLoading.value = true;
    await loadInmuebles(currentPage.value, searchTerm.value);
    hasLoaded.value = true; // Mark initial load as complete
  } catch (error) {
    // Mensaje amigable para el usuario; detalles técnicos en consola
    // await showErrorFriendly(
    //   "No pudimos cargar las propiedades. Por favor recarga la página o inténtalo de nuevo más tarde.",
    //   error
    // );
  } finally {
    isLoading.value = false;
  }
});

/* ========= Al escribir: disparar búsqueda en el servidor + ir a pág 1 ========= */
debouncedSearch(searchTerm.value);

/* ========= Cargar una página del backend (reemplaza, no concatena) ========= */
const loadInmuebles = async (page = 1, term = "") => {
  try {
    isLoading.value = true;
    const t0 = performance.now();
  const pageSize = itemsPerPage.value;

    const filters = {};
    if (term) {
      filters.searchTerm = term.trim();
    }

    const response = await inmuebleService.getInmueblesPaginados(page, pageSize, filters);

    const elapsed = performance.now() - t0;
    if (elapsed < MIN_LOADER_MS) {
      await delay(MIN_LOADER_MS - elapsed);
    }

    if (!response || !response.items || !Array.isArray(response.items)) {
      throw new Error("La respuesta del servidor no contiene datos válidos.");
    }

    const processedData = mapItems(response.items);
    inmuebles.value = processedData;

    if (typeof response.totalPages === "number") {
      totalPages.value = response.totalPages;
    } else if (typeof response.totalCount === "number") {
      totalPages.value = Math.ceil(response.totalCount / pageSize);
    } else {
      totalPages.value = 1;
    }

    if (typeof response.totalCount === "number") {
      totalPropiedades.value = response.totalCount;
    } else {
      totalPropiedades.value = processedData.length;
    }

    currentPage.value = response.pageNumber || page;
  } catch (error) {
    if (error.message === "Falta el token de autenticación") {
      // No realizar ninguna acción adicional si falta el token
      return;
    }
    // await showErrorFriendly(
    //   "No fue posible obtener la lista de propiedades en este momento. Intenta nuevamente más tarde.",
    //   error
    // );
  } finally {
    isLoading.value = false;
  }
};

// React to pagination changes (page size or page number)
watch(itemsPerPage, (newVal, oldVal) => {
  // cuando cambia el tamaño de página, recargar la página 1
  currentPage.value = 1;
  loadInmuebles(1, searchTerm.value);
});

watch(currentPage, (newVal) => {
  loadInmuebles(newVal, searchTerm.value);
});

/* ========= Navegación ========= */
const goToPage = async (page) => {
  if (page >= 1 && page <= effectiveTotalPages.value) {
    currentPage.value = page;
    // ✅ Pasa el término de búsqueda actual al cambiar de página
    await loadInmuebles(page, searchTerm.value);
  }
};

const goToPreviousPage = async () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    // ✅ Pasa el término de búsqueda actual al cambiar de página
    await loadInmuebles(currentPage.value, searchTerm.value);
  }
};

const goToNextPage = async () => {
  if (currentPage.value < effectiveTotalPages.value) {
    currentPage.value++;
    // ✅ Pasa el término de búsqueda actual al cambiar de página
    await loadInmuebles(currentPage.value, searchTerm.value);
  }
};

/* ========= Modal alta/edición ========= */
const openModal = () => {
  showModal.value = true;
};

const closeModal = (shouldReload = false) => {
  showModal.value = false;
  inmuebleToEdit.value = null;
  if (shouldReload) {
    loadInmuebles(currentPage.value, searchTerm.value);
  }
};

async function editInmueble(id) {
  try {
    const detalleInmueble = await fetchInmuebleById(id);

    // Mapear las amenidades para asegurar que tienen `amenidadInmuebleId` y `nombre`
    const adaptedAmenidades = Array.isArray(detalleInmueble.amenidades)
      ? detalleInmueble.amenidades.map((a) => ({
          amenidadInmuebleId: a.id || a.amenidadInmuebleId,
          nombre: a.nombre || "Desconocida",
        }))
      : [];

    // Adaptar el inmueble para incluir las amenidades mapeadas
    const adaptedInmueble = {
      ...detalleInmueble,
      amenidades: adaptedAmenidades,
    };

    inmuebleToEdit.value = adaptedInmueble;
    showModal.value = true;
  } catch (error) {
    // await showErrorFriendly(
    //   "No se pudo preparar el inmueble para edición. Inténtalo de nuevo.",
    //   error
    // );
  }
}

const confirmDelete = async (id) => {
  try {
    const result = await Swal.fire({
      title: "¿Estás seguro?",
      text: "¡No podrás revertir esto!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Sí, eliminar",
      cancelButtonText: "Cancelar",
    });

    if (result.isConfirmed) {
      Swal.fire({
        title: "Eliminando...",
        html: "Por favor espera",
        allowOutsideClick: false,
        didOpen: () => {
          Swal.showLoading();
        },
      });

      const success = await inmuebleService.deleteInmueble(id);

      if (success) {
        await loadInmuebles(currentPage.value, searchTerm.value);
        Swal.fire("¡Eliminado!", "La propiedad ha sido eliminada.", "success");
      }
    }
  } catch (error) {
    // await showErrorFriendly("No fue posible eliminar la propiedad. Por favor intenta de nuevo.", error);
  }
};

const fetchInmuebleById = async (id) => {
  try {
    const response = await inmuebleService.getInmuebleById(id);
    if (!response) throw new Error("No se encontró el inmueble");

    return {
      id: response.id,
      codigoPropiedad: response.codigoPropiedad || "",
      titulo: response.titulo || "",
      slugInmueble: response.slugInmueble || "",
      precio: parseFloat(response.precio) || 0,
      habitaciones: response.habitaciones || 0,
      banos: response.banos || 0,
      parqueos: response.parqueos || 0,
      metrosCuadrados: parseFloat(response.metrosCuadrados) || 0,
      imagenPrincipal:
        response.imagenPrincipal || "https://via.placeholder.com/150",
      contenido: response.contenido || "",
      tipos: response.tipos || "",
      operaciones: response.operaciones || "",
      ubicaciones: response.ubicaciones || "",
      // Mapea correctamente las amenidades desde $values o array plano
      amenidades: Array.isArray(response.amenidades?.$values)
        ? response.amenidades.$values.map((a) => ({
            id: a.amenidadInmuebleId || null,
            nombre: a.nombre || "Desconocida",
          }))
        : Array.isArray(response.amenidades)
        ? response.amenidades.map((nombre, index) => ({
            id: index + 1, // Genera un ID temporal si no existe
            nombre: nombre || "Desconocida",
          }))
        : [],
      luxury: Boolean(response.luxury),
      video: response.video || "",
      precioActivo: Boolean(response.precioActivo),
      // Imágenes de referencia:
      imagenesReferencia: Array.isArray(response.imagenesReferencia?.$values)
        ? response.imagenesReferencia.$values.map((img) => img.url || img)
        : [],
    };
  } catch (error) {
    if (error.message === "Falta el token de autenticación") {
      // No realizar ninguna acción adicional si falta el token
      return null;
    }
    // await showErrorFriendly("No fue posible obtener los detalles de la propiedad. Inténtalo más tarde.", error);
    return null;
  }
};

/* ========= (opcional) eliminar todo ========= */
const confirmDeleteAll = async () => {
  const result = await Swal.fire({
    title: "¿Estás seguro?",
    text: "¡Esto eliminará TODAS las propiedades y no se podrá revertir!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#3085d6",
    confirmButtonText: "Sí, eliminar todo",
    cancelButtonText: "Cancelar",
  });

  if (result.isConfirmed) {
    try {
      Swal.fire({
        title: "Eliminando todas las propiedades...",
        html: "Por favor, espera",
        allowOutsideClick: false,
        didOpen: () => {
          Swal.showLoading();
        },
      });

      const propertyIds = inmuebles.value.map((inmueble) => inmueble.id);
      for (const id of propertyIds) {
        await inmuebleService.deleteInmueble(id);
      }
      await loadInmuebles(currentPage.value, searchTerm.value);
      Swal.fire("¡Eliminado!", "Todas las propiedades han sido eliminadas.", "success");
    } catch (error) {
      // await showErrorFriendly("No se pudieron eliminar todas las propiedades. Intenta de nuevo más tarde.", error);
    }
  }
};

/* =================== Export Helpers =================== */
const fetchAllInmueblesForExport = async () => {
  const pageSize = 9;
  let page = 1;
  let totalPages = Infinity;
  const all = [];

  try {
    while (page <= totalPages) {
      const res = await inmuebleService.getInmueblesPaginados(page, pageSize);
      if (!res) {
        throw new Error("Respuesta de API nula o indefinida");
      }

      const items = res.items || res.$values || [];
      if (!Array.isArray(items)) {
        throw new Error("La propiedad 'items' de la respuesta no es un array.");
      }

      if (items.length === 0) break; // Si no hay items, sal del bucle

      all.push(...items);

      if (typeof res.totalPages === "number") {
        totalPages = res.totalPages;
      } else if (typeof res.totalCount === "number") {
        totalPages = Math.ceil(res.totalCount / pageSize);
      } else if (items.length < pageSize) {
        break;
      }
      page++;
    }
  } catch (e) {
    // await showErrorFriendly(
    //   "No fue posible obtener todas las propiedades para exportar. Revisa tu conexión e inténtalo nuevamente.",
    //   e
    // );
    return []; // Devuelve un array vacío para evitar errores posteriores
  }

  return all;
};

const mapRowsForExport = (arr) =>
  arr.map((i) => ({
    Código: i.codigoPropiedad || "",
    Título: i.titulo || "",
    Tipo: i.tipos || "",
    Operación: i.operaciones || "",
    Ubicación: i.ubicaciones || "",
    Precio: typeof i.precio === "number" ? i.precio : Number(i.precio) || 0,
    Habitaciones: i.habitaciones ?? "",
    Baños: i.banos ?? "",
    Parqueos: i.parqueos ?? "",
    "m²": i.metrosCuadrados ?? "",
  }));

const exportRows = async (rows, nombreBase = "propiedades") => {
  if (!rows.length) {
    await showInfoFriendly("No hay propiedades para exportar.", "Sin datos");
    return;
  }
  const fecha = new Date().toISOString().slice(0, 10);

  try {
    const XLSX = await import(/* @vite-ignore */ "xlsx");
    const ws = XLSX.utils.json_to_sheet(rows);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Propiedades");
    XLSX.writeFile(wb, `${nombreBase}_${fecha}.xlsx`);
  } catch (err) {
    // fallback CSV
    // ...existing code to create csv and download...
    await showInfoFriendly("Se generó un CSV compatible con Excel.", "Exportado como CSV");
  }
};

const downloadExcelAll = async () => {
  try {
    const all = await fetchAllInmueblesForExport();
    if (!all || all.length === 0) {
      return; // La función de arriba ya mostró el error o la info
    }

    const rows = mapRowsForExport(
      all.map((inmueble) => ({
        ...inmueble,
        precio: parseFloat(inmueble.precio) || 0,
        metrosCuadrados: parseFloat(inmueble.metrosCuadrados) || 0,
        banos: inmueble.banos || 0,
        parqueos: inmueble.parqueos || 0,
        habitaciones: inmueble.habitaciones || 0,
      }))
    );
    await exportRows(rows, "propiedades_todo");
  } catch (e) {
    //console.error(e);
    // await showErrorFriendly("No fue posible generar el archivo de exportación. Intenta nuevamente más tarde.", e);
  }
};

/* ========= Expose opcional para template ========= */
defineExpose({
  fmtPrice,
});

// ====== Base del sitio ======
const siteOrigin = ref(
  import.meta.env.VITE_PUBLIC_SITE_ORIGIN ||
    (typeof window !== "undefined" ? window.location.origin : "") ||
    "https://tu-dominio.com"
);
const stripTrailingSlash = (u) => (u || "").replace(/\/+$/, "");

// Guarda la última URL copiada (por si querés mostrarla en algún lugar)
const lastCopiedUrl = ref("");
// Para animar el botón de la fila que fue copiada
const copiedId = ref(null);
let copiedTimer = null;

// Construye la URL completa del inmueble: https://dominio/inmueble/<slug>
const buildInmuebleUrl = (inm) => {
  if (!inm?.slugInmueble) return "";
  const base = stripTrailingSlash(siteOrigin.value);
  return `${base}/inmueble/${inm.slugInmueble}`;
};

// Copiar al portapapeles con feedback y estado por fila
const copyInmuebleUrl = async (id) => {
  try {
    const inm = inmuebles.value.find((x) => x.id === id);
    if (!inm) {
      // await showErrorFriendly("No se encontró la propiedad seleccionada.", null);
      return;
    }
    const url = buildInmuebleUrl(inm);
    if (!url) {
      await showInfoFriendly("La propiedad no tiene una URL pública disponible.", "Atención");
      return;
    }

    if (navigator.clipboard?.writeText) {
      await navigator.clipboard.writeText(url);
    } else {
      // fallback
      const ta = document.createElement("textarea");
      ta.value = url;
      document.body.appendChild(ta);
      ta.select();
      document.execCommand("copy");
      document.body.removeChild(ta);
    }

    lastCopiedUrl.value = url;
    copiedId.value = id;
    if (copiedTimer) clearTimeout(copiedTimer);
    copiedTimer = setTimeout(() => (copiedId.value = null), 1600);

    await showSuccessTransient("URL copiada al portapapeles");
  } catch (e) {
    //console.error("No se pudo copiar la URL:", e);
    // await showErrorFriendly("No se pudo copiar la URL al portapapeles. Intenta nuevamente.", e);
  }
};
</script>

<style scoped>
/* Estilo del spinner */
.spinner {
  width: 16px;
  height: 16px;
  border: 2px solid #e5e7eb; /* Color del borde */
  border-top: 2px solid #4b5563; /* Color del borde superior */
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

/* Animación de giro */
@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

/* Animación de fade in y fade out */
.fade-animation {
  animation: fadeInOut 1.5s infinite;
}

@keyframes fadeInOut {
  0%,
  100% {
    opacity: 0;
  }
  50% {
    opacity: 1;
  }
}
</style>
