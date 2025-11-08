<template>
  <div>
    <div class="mt-5 mb-12">
      <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
        Gestion de Proyectos
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
              required
            />
          </div>
          <!-- <button
            type="button"
            @click="handleSearch"
            class="inline-flex items-center py-2 px-3 ms-2 text-sm font-medium text-white bg-gray-700 rounded-lg border border-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300    "
          >
            <svg
              class="w-4 h-4 me-2"
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
            Buscar
          </button> -->
          <!-- Spinner y mensaje de búsqueda -->
          <div v-if="isSearching" class="mt-2 flex items-center space-x-2">
            <div class="spinner"></div>
            <span class="text-gray-500 text-sm fade-animation"
              >Buscando...</span
            >
          </div>
        </form>
      </div>
      <div class="flex justify-end items-end">
        <button
          v-if="isAdmin"
          type="button"
          @click="openModal"
          class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 focus:outline-none"
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
            <th scope="col" class="px-6 py-3">Código</th>
            <th scope="col" class="px-6 py-3">Título</th>
            <!-- <th scope="col" class="px-6 py-3">Descripcion</th> -->
            <th scope="col" class="px-6 py-3">Tipo</th>
            <!-- <th scope="col" class="px-6 py-3">Imagen</th> -->
            <!-- <th scope="col" class="px-6 py-3">Operación</th> -->
            <th scope="col" class="px-6 py-3">Ubicación</th>
            <th scope="col" class="px-6 py-3">Precio</th>
            <!--<th scope="col" class="px-6 py-3">Habitaciones</th> -->
            <!-- <th scope="col" class="px-6 py-3">Amenidades</th> -->
            <!-- <th scope="col" class="px-6 py-3">Baños</th> -->
            <!-- <th scope="col" class="px-6 py-3">Parqueos</th> -->
            <th scope="col" class="px-6 py-3">m²</th>
            <!-- <th scope="col" class="px-6 py-3">Lujosa</th> -->
            <th scope="col" class="px-6 py-3">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="proyecto in filteredProyectos"
            :key="proyecto.id"
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


const openDetailModal = async (id) => {
  const detalleProyecto = await fetchProyectoById(id);
  if (detalleProyecto) {
    proyectoSeleccionado.value = detalleProyecto;
    showModalDetalle.value = true;
  }
};

// Computed property para filtrar proyectos
const filteredProyectos = computed(() => {
  if (!searchTerm.value.trim()) {
    return proyectos.value;
  }

  const term = searchTerm.value.toLowerCase().trim();
  return proyectos.value.filter((proyecto) => {
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
});

// Obtener proyectos
onMounted(async () => {
  initFlowbite();
  try {
    await loadProyectos();
  } catch (error) {
    //console.error("Error al cargar proyectos:", error);
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

    const response = await Promise.all([
      proyectoService.getProyecto(), // Llamada real
      delay(2000), // 👈 Delay de 2 segundos
    ]).then(([res]) => res);

    const data = response.$values || [];
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

    proyectos.value = processedData;
    originalProyectos.value = [...processedData];
  } catch (error) {
    //console.error("Error al procesar los proyectos:", error);
    // Swal.fire(
    //   "No fue posible cargar los proyectos",
    //   "Ocurrió un error al obtener la lista de proyectos. Revisa tu conexión y vuelve a intentarlo.",
    //   "error"
    // );
  } finally {
    isLoading.value = false; // 👈 garantizado mínimo 2s
  }
};

/* ========= Búsqueda: volver a página 1 ========= */
const handleSearch = async () => {
  isSearching.value = true; // Mostrar el spinner
  currentPage.value = 1; // Resetea a la primera página al buscar
  try {
    if (searchTerm.value.trim() && !allLoaded.value) {
      await loadAllInmuebles(); // Cargar todos los datos si no están cargados
    }
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
      imagenesReferenciaProyecto: Array.isArray(
        response.imagenesReferenciaProyecto?.$values
      )
        ? response.imagenesReferenciaProyecto.$values.map(
            (img) => img.url || img
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
    "https://tu-dominio.com"
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
