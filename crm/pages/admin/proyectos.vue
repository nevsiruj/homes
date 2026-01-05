<template>
  <div>
    <div class="mt-5 mb-12">
      <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
        Gestion de Proyectos
      </h2>
    </div>

    <div class="grid grid-cols-1 gap-4 mb-8 lg:grid-cols-3 xl:grid-cols-4">
      <div class="lg:col-span-2 xl:col-span-3">
        <form @submit.prevent class="flex flex-col items-start w-full">
          <label for="voice-search" class="sr-only">Buscar</label>
          <div class="relative w-full">
            <input
              type="text"
              v-model="searchTerm"
              @input="handleSearch"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2"
              placeholder="Buscar por título, código, ubicación o tipo"
              required
            />
          </div>
          <!-- Spinner y mensaje de búsqueda -->
          <div v-if="isSearching" class="mt-2 flex items-center space-x-2">
            <div class="spinner"></div>
            <span class="text-gray-500 text-sm fade-animation"
              >Buscando...</span
            >
          </div>
        </form>
      </div>
      <div class="flex justify-end items-end gap-2">
        <PaginationControls
          v-model:itemsPerPage="itemsPerPage"
          v-model:currentPage="currentPage"
          :perPageOptions="[20,50,100]"
        />
        <button
          v-if="isAdmin"
          type="button"
          @click="openModal"
          class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none"
        >
          Agregar registro
        </button>
      </div>
    </div>

    <Loader v-if="isLoading" />

    <!-- Tabla de proyectos -->
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table class="w-full text-sm text-left rtl:text-right text-gray-500">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50">
          <tr>
            <th 
              scope="col" 
              class="px-6 py-3 cursor-pointer hover:bg-gray-100 select-none"
              @click="handleSort('codigoProyecto')"
            >
              <div class="flex items-center gap-2">
                Código
                <span v-if="sortColumn === 'codigoProyecto'" class="text-gray-500">
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
            v-for="proyecto in paginatedProyectos"
            :key="proyecto.id"
            v-memo="[proyecto.id, proyecto.titulo, proyecto.precio]"
            class="odd:bg-white odd: even:bg-gray-50 even: border-b border-gray-200"
          >
            <td
              class="px-6 py-4 font-medium text-xs text-gray-900 whitespace-nowrap"
            >
              {{ proyecto.codigoProyecto }}
            </td>
            <td class="px-6 py-4 truncate max-w-xs">{{ proyecto.titulo }}</td>
            <!-- <td class="px-6 py-4 truncate max-w-xs">
              {{ proyecto.contenido }}
            </td> -->
            <td class="px-6 py-4">{{ proyecto.tipos }}</td>
            <!-- <td class="px-6 py-4">
              <img
                :src="proyecto.imagenPrincipal"
                alt="Imagen del proyecto"
                class="h-12 w-12 object-cover rounded"
              />
            </td> -->
            <!-- <td class="px-6 py-4">{{ proyecto.operaciones }}</td> -->
            <td class="px-6 py-4">{{ proyecto.ubicaciones }}</td>
            <td class="px-6 py-4">${{ proyecto.precio.toLocaleString() }}</td>
            <!-- <td class="px-6 py-4">{{ proyecto.habitaciones || "-" }}</td> -->
            <!-- <td class="px-6 py-4">
              {{ proyecto.amenidades?.map((a) => a.nombre).join(", ") || "-" }}
            </td> -->
            <!-- <td class="px-6 py-4">{{ proyecto.banos || "-" }}</td>
            <td class="px-6 py-4">{{ proyecto.parqueos || "-" }}</td> -->
            <td class="px-6 py-4">{{ proyecto.metrosCuadrados || "-" }}</td>
            <!-- <td class="px-6 py-4">
              <span v-if="proyecto.luxury" class="text-green-500">✔</span>
              <span v-else class="text-red-500">✖</span>
            </td> -->
            <td class="px-6 py-4 space-x-2 flex content-center justify-center">
              <button
                @click="openDetailModal(proyecto.id)"
                title="Ver detalles"
                class="flex items-center font-medium text-gray-600 hover:underline"
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
                @click="editProyecto(proyecto.id)"
                title="Editar registro"
                class="flex items-center font-medium text-gray-600 hover:underline"
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
                @click="confirmDelete(proyecto.id)"
                title="Eliminar registro"
                class="flex items-center font-medium text-gray-600 hover:underline"
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
                @click="copyProyectoUrl(proyecto.id)"
                :disabled="!proyecto.slugProyecto"
                class="flex items-center font-medium text-gray-600 hover:underline"
                title="Copiar URL"
              >
                <svg
                  v-if="copiedId !== proyecto.id"
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

    <!-- Paginación -->
    <div v-if="filteredProyectos.length > 0" class="flex items-center justify-between mt-4 px-4 py-3 bg-white border-t sm:px-6">
      <div class="flex justify-between flex-1 sm:hidden">
        <button
          @click="prevPage"
          :disabled="currentPage === 1"
          class="relative inline-flex items-center px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Anterior
        </button>
        <button
          @click="nextPage"
          :disabled="currentPage === totalPages"
          class="relative ml-3 inline-flex items-center px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Siguiente
        </button>
      </div>
      <div class="hidden sm:flex sm:flex-1 sm:items-center sm:justify-between">
        <div>
          <p class="text-sm text-gray-700">
            Mostrando
            <span class="font-medium">{{ (currentPage - 1) * itemsPerPage + 1 }}</span>
            a
            <span class="font-medium">{{ Math.min(currentPage * itemsPerPage, filteredProyectos.length) }}</span>
            de
            <span class="font-medium">{{ filteredProyectos.length }}</span>
            proyectos
          </p>
        </div>
        <div>
          <nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px" aria-label="Pagination">
            <button
              @click="prevPage"
              :disabled="currentPage === 1"
              class="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span class="sr-only">Anterior</span>
              <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z" clip-rule="evenodd" />
              </svg>
            </button>
            
            <!-- Números de página -->
            <button
              v-for="page in totalPages"
              :key="page"
              @click="changePage(page)"
              :class="[
                'relative inline-flex items-center px-4 py-2 border text-sm font-medium',
                page === currentPage
                  ? 'z-10 bg-gray-700 border-gray-700 text-white'
                  : 'bg-white border-gray-300 text-gray-500 hover:bg-gray-50'
              ]"
            >
              {{ page }}
            </button>
            
            <button
              @click="nextPage"
              :disabled="currentPage === totalPages"
              class="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span class="sr-only">Siguiente</span>
              <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path fill-rule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clip-rule="evenodd" />
              </svg>
            </button>
          </nav>
        </div>
      </div>
    </div>

    <!-- Mensaje cuando no hay proyectos -->
    <div
      v-if="filteredProyectos.length === 0"
      class="text-center py-8 text-gray-500"
    >
      {{
        searchTerm
          ? "No se encontraron proyectos que coincidan con la búsqueda."
          : "No hay proyectos registrados."
      }}
    </div>

    <modalAgProyecto
      v-show ="showModal"
      :isOpen="showModal"
      :proyecto-to-edit="proyectoToEdit"
      @close="closeModal"
      @proyecto-updated="loadProyectos"
    />

    <modal-detalle-proyecto
    v-show ="showModalDetalle"
      :isOpen="showModalDetalle"
      :proyecto="proyectoSeleccionado"
      @close="showModalDetalle = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import Swal from "sweetalert2";
import proyectoService from "../../services/proyectoService.js";
import modalAgProyecto from "../../components/modalAgProyecto.vue";
import modalDetalleProyecto from "../../components/modalDetalleProyecto.vue";
import { initFlowbite } from "flowbite";
import Loader from "~/components/Loader.vue";
import { useAuthStore } from "@/stores/auth";
import PaginationControls from "../../components/PaginationControls.vue";

const auth = useAuthStore();
const isAdmin = computed(() =>
  typeof auth.hasRole === "function"
    ? auth.hasRole("admin")
    : (auth.roles || []).includes("admin")
);

definePageMeta({
  layout: "admin",
  requiresAuth: true,
});

const proyectos = ref([]);
const searchTerm = ref("");
const showModal = ref(false);
const originalProyectos = ref([]);
const proyectoToEdit = ref(null);
const showModalDetalle = ref(false);
const proyectoSeleccionado = ref(null);
const isSearching = ref(false); // Estado para el spinner
const isLoading = ref(true); //Estado para el Loader

// Paginación
const currentPage = ref(1);
const itemsPerPage = ref(20);

// Estado para ordenamiento
const sortColumn = ref(null);
const sortDirection = ref('asc');

// Función para manejar el ordenamiento por columnas
const handleSort = (column) => {
  if (sortColumn.value === column) {
    sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
  } else {
    sortColumn.value = column;
    sortDirection.value = 'asc';
  }
  currentPage.value = 1;
};

// Función para obtener el valor de una columna para ordenar
const getColumnValue = (proyecto, column) => {
  switch (column) {
    case 'codigoProyecto':
      return (proyecto.codigoProyecto || '').toLowerCase();
    case 'titulo':
      return (proyecto.titulo || '').toLowerCase();
    case 'tipos':
      return (proyecto.tipos || '').toLowerCase();
    case 'ubicaciones':
      return (proyecto.ubicaciones || '').toLowerCase();
    case 'precio':
      return parseFloat(proyecto.precio) || 0;
    case 'metrosCuadrados':
      return parseFloat(proyecto.metrosCuadrados) || 0;
    default:
      return '';
  }
};

const openDetailModal = async (id) => {
  const detalleProyecto = await fetchProyectoById(id);
  if (detalleProyecto) {
    proyectoSeleccionado.value = detalleProyecto;
    showModalDetalle.value = true;
  }
};

// Computed property para filtrar proyectos
const filteredProyectos = computed(() => {
  let list = proyectos.value;

  // Filtrar por término de búsqueda
  if (searchTerm.value.trim()) {
    const term = searchTerm.value.toLowerCase().trim();
    list = list.filter((proyecto) => {
      return (
        (proyecto.titulo && proyecto.titulo.toLowerCase().includes(term)) ||
        (proyecto.codigoProyecto &&
          proyecto.codigoProyecto.toLowerCase().includes(term)) ||
        (proyecto.ubicaciones &&
          proyecto.ubicaciones.toLowerCase().includes(term)) ||
        (proyecto.tipos && proyecto.tipos.toLowerCase().includes(term)) ||
        (proyecto.operaciones &&
          proyecto.operaciones.toLowerCase().includes(term))
      );
    });
  }

  // Aplicar ordenamiento
  if (sortColumn.value) {
    list = [...list].sort((a, b) => {
      const aValue = getColumnValue(a, sortColumn.value);
      const bValue = getColumnValue(b, sortColumn.value);
      
      let comparison = 0;
      if (aValue < bValue) comparison = -1;
      if (aValue > bValue) comparison = 1;
      
      return sortDirection.value === 'asc' ? comparison : -comparison;
    });
  }

  return list;
});

// Computed para paginación
const paginatedProyectos = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  return filteredProyectos.value.slice(start, end);
});

const totalPages = computed(() => 
  Math.ceil(filteredProyectos.value.length / itemsPerPage.value)
);

const changePage = (page) => {
  if (page >= 1 && page <= totalPages.value) {
    currentPage.value = page;
  }
};

const nextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
  }
};

const prevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
  }
};

// Obtener proyectos
onMounted(async () => {
  console.log('🚀 onMounted ejecutándose en proyectos.vue');
  initFlowbite();
  try {
    console.log('⏳ Iniciando carga de proyectos...');
    await loadProyectos();
    console.log('✅ Carga de proyectos completada');
  } catch (error) {
    console.error("❌ Error al cargar proyectos:", error);
    Swal.fire({
      icon: "error",
      title: "No se pudieron cargar los proyectos",
      text: "Hubo un problema al obtener la lista de proyectos. Por favor recarga la página o inténtalo nuevamente en unos minutos.",
      confirmButtonText: "Entendido",
    });
  }
});

const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

// Función para cargar proyectos
const loadProyectos = async () => {
  try {
    isLoading.value = true;

    const response = await proyectoService.getProyecto();
    console.log('📦 Respuesta cruda de la API:', response);
    
    // La respuesta es directamente un array, no tiene $values
    const data = Array.isArray(response) ? response : (response.$values || []);
    console.log('📋 Data procesada:', data);
    console.log('🔢 Es array?', Array.isArray(data));
    console.log('📊 Cantidad de proyectos:', data.length);
    
    const processedData = data.map((proyecto) => ({
      id: proyecto.id,
      codigoProyecto: proyecto.codigoProyecto || "",
      titulo: proyecto.titulo || "",
      slugProyecto: proyecto.slugProyecto || "",
      precio: parseFloat(proyecto.precio) || 0,
      habitaciones: proyecto.habitaciones || 0,
      banos: proyecto.banos || 0,
      parqueos: proyecto.parqueos || 0,
      metrosCuadrados: parseFloat(proyecto.metrosCuadrados) || 0,
      imagenPrincipal:
        proyecto.imagenPrincipal || "https://via.placeholder.com/150",
      contenido: proyecto.contenido || "",
      tipos: proyecto.tipos || "",
      operaciones: proyecto.operaciones || "",
      ubicaciones: proyecto.ubicaciones || "",
      amenidades: proyecto.amenidades?.$values || [],
      luxury: Boolean(proyecto.luxury),
      video: proyecto.video || "",
    }));

    console.log('✅ Proyectos procesados:', processedData);
    console.log('📊 Total de proyectos:', processedData.length);

    proyectos.value = processedData;
    originalProyectos.value = [...processedData];
  } catch (error) {
    console.error('❌ Error al cargar proyectos:', error);
    Swal.fire(
      "No fue posible cargar los proyectos",
      "Ocurrió un error al obtener la lista de proyectos. Revisa tu conexión y vuelve a intentarlo.",
      "error"
    );
  } finally {
    isLoading.value = false;
  }
};

/* ========= Búsqueda: volver a página 1 ========= */
const handleSearch = async () => {
  isSearching.value = true; // Mostrar el spinner
  currentPage.value = 1; // Resetea a la primera página al buscar
  try {
    // Pequeño delay para mostrar el spinner
    await new Promise(resolve => setTimeout(resolve, 300));
  } finally {
    isSearching.value = false; // Ocultar el spinner
  }
};

// Función para abrir modal
const openModal = () => {
  showModal.value = true;
};

// Función para cerrar modal y recargar datos si es necesario
const closeModal = (shouldReload = false) => {
  showModal.value = false;
  proyectoToEdit.value = null;
  if (shouldReload) {
    loadProyectos();
  }
};

// Función para editar proyecto
async function editProyecto(id) {
  try {
    const detalleProyecto = await fetchProyectoById(id);

    const adaptedProyecto = {
      ...detalleProyecto,
      // amenidades: amenidadesIds,
    };

    proyectoToEdit.value = adaptedProyecto;
    showModal.value = true;
  } catch (error) {
    //console.error("Error al editar proyecto:", error);
    Swal.fire(
      "No se pudo abrir el editor",
      "No pudimos preparar el proyecto para editar. Inténtalo de nuevo.",
      "error"
    );
  }
}

// Función para confirmar y eliminar proyecto
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

      const success = await proyectoService.deleteProyecto(id);

      if (success) {
        await loadProyectos();
        Swal.fire("¡Eliminado!", "El proyecto ha sido eliminado.", "success");
      }
    }
  } catch (error) {
    //console.error("Error al eliminar proyecto:", error);
    Swal.fire(
      "No se pudo eliminar el proyecto",
      "Ocurrió un problema al intentar eliminar el proyecto. Por favor inténtalo de nuevo.",
      "error"
    );
  }
};

const fetchProyectoById = async (id) => {
  try {
    const response = await proyectoService.getProyectoById(id);
    if (!response) throw new Error("No se encontró el proyecto");

    return {
      id: response.id,
      codigoProyecto: response.codigoProyecto || "",
      titulo: response.titulo || "",
      slugProyecto: response.slugProyecto || "",
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
      amenidades: response.amenidades?.$values || [],
      luxury: Boolean(response.luxury),
      video: response.video || "",

      // 👇 Mapeo correcto de las imágenes de referencia:
      // Devolver siempre objetos con 'url' para mantener consistencia
      imagenesReferenciaProyecto: Array.isArray(
        response.imagenesReferenciaProyecto?.$values
      )
        ? response.imagenesReferenciaProyecto.$values.map((img) =>
            typeof img === "string" ? { url: img } : img || {}
          )
        : Array.isArray(response.imagenesReferenciaProyecto)
        ? response.imagenesReferenciaProyecto.map((img) =>
            typeof img === "string" ? { url: img } : img || {}
          )
        : [],
    };
  } catch (error) {
    //console.error("Error al obtener el proyecto por ID:", error);
    Swal.fire(
      "No se pudo obtener el proyecto",
      "No fue posible cargar los detalles del proyecto. Inténtalo nuevamente más tarde.",
      "error"
    );
    return null;
  }
};

// ====== Base del sitio ======
const siteOrigin = ref(
  import.meta.env.VITE_PUBLIC_SITE_ORIGIN ||
    (typeof window !== "undefined" ? window.location.origin : "") ||
    "https://homesguatemala.com"
);
const stripTrailingSlash = (u) => (u || "").replace(/\/+$/, "");

// Guarda la última URL copiada (por si querés mostrarla en algún lugar)
const lastCopiedUrl = ref("");
// Para animar el botón de la fila que fue copiada
const copiedId = ref(null);
let copiedTimer = null;

// Construye la URL completa del inmueble: https://dominio/inmueble/<slug>

const buildProyectoUrl = (inm) => {
  if (!inm?.slugProyecto) return "";
  const base = stripTrailingSlash(siteOrigin.value);
  return `${base}/proyecto/${inm.slugProyecto}`;
};

// Copiar al portapapeles con feedback y estado por fila
const copyProyectoUrl = async (id) => {
  try {
    const inm = proyectos.value.find((x) => x.id === id);
    if (!inm) {
      Swal.fire("Error", "No se encontró el proyecto.", "error");
      return;
    }
    const url = buildProyectoUrl(inm);
    if (!url) {
      Swal.fire(
        "Atención",
        "El proyecto no tiene slug para generar la URL.",
        "info"
      );
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

    Swal.fire({
      icon: "success",
      title: "URL copiada",
      text: url,
      timer: 1200,
      showConfirmButton: false,
    });
  } catch (e) {
    //console.error("No se pudo copiar la URL:", e);
    Swal.fire(
      "No se pudo copiar la URL",
      "No pudimos copiar la dirección automáticamente. Puedes copiarla manualmente desde el campo o intentarlo de nuevo.",
      "error"
    );
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
