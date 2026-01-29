<template>
    <div class="p-4">
        <!-- Encabezado -->
        <div class="mt-5 mb-12">
            <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
                Gestión de Artículos del Blog
            </h2>
        </div>

        <!-- Buscador y botón Agregar -->
        <div class="flex flex-col gap-4 mb-8 md:flex-row md:items-center md:justify-between">
            <!-- Formulario de búsqueda -->
            <div class="flex-1 w-full">
                <form @submit.prevent="handleSearch" class="flex flex-col gap-2 sm:flex-row sm:items-center">
                    <div class="relative flex-1 w-full">
                        <input v-model="searchTerm" type="text" id="search-blog" placeholder="Buscar artículos..."
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2.5" />
                    </div>
                    <div class="flex gap-2 w-full sm:w-auto">
                                    <!-- Selector de orden -->
                                    <select v-model="ordenSeleccionado"
                                        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2.5 flex-1 sm:flex-none sm:w-auto">
                                        <option value="fecha-desc">Más recientes</option>
                                        <option value="fecha-asc">Más antiguas</option>
                                        <option value="titulo-asc">Título A-Z</option>
                                        <option value="titulo-desc">Título Z-A</option>
                                    </select>
                        <select v-model="filtroCategoria"
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2.5 flex-1 sm:flex-none sm:w-auto">
                            <option value="">Todas</option>
                            <option v-for="categoria in categoriasDisponibles" :key="categoria" :value="categoria">
                                {{ categoria }}
                            </option>
                        </select>
                        <!-- <button type="submit"
                            class="inline-flex items-center justify-center py-2.5 px-4 text-sm font-medium text-white bg-gray-700 rounded-lg border border-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 whitespace-nowrap">
                            <svg class="w-4 h-4 sm:me-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none"
                                viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
                            </svg>
                            <span class="hidden sm:inline">Buscar</span>
                        </button> -->
                    </div>
                </form>
            </div>

            <!-- Controles de paginación y agregar -->
            <div class="flex flex-col gap-2 sm:flex-row sm:items-center w-full md:w-auto">
                <select v-model.number="itemsPerPage" @change="currentPage = 1"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2.5 w-full sm:w-auto">
                    <option :value="10">10 por página</option>
                    <option :value="20">20 por página</option>
                    <option :value="50">50 por página</option>
                </select>
                <button type="button" @click="abrirModalAgregar"
                    class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none whitespace-nowrap w-full sm:w-auto">
                    <span class="inline sm:hidden">+ Artículo</span>
                    <span class="hidden sm:inline">Agregar Artículo</span>
                </button>
                <!-- <button type="button" @click="eliminarTodosLosBlogs"
                    class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none whitespace-nowrap w-full sm:w-auto">
                    <span class="inline sm:hidden">🗑️ Todos</span>
                    <span class="hidden sm:inline">Eliminar Todos</span>
                </button> -->
                <!-- <button type="button" @click="mostrarModalImportar = true"
                    class="text-gray-700 bg-gray-200 hover:bg-gray-300 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none whitespace-nowrap w-full sm:w-auto">
                    <span class="inline sm:hidden">📥 Importar</span>
                    <span class="hidden sm:inline">📥 Importar JSON</span>
                </button> -->
            </div>
        </div>

        <!-- Tabla de artículos -->
        <div class="relative overflow-x-auto shadow-md sm:rounded-lg bg-white">
            <table class="w-full text-sm text-left rtl:text-right text-gray-500">
                <thead class="text-xs text-gray-700 uppercase bg-gray-50">
                    <tr>
                        <th scope="col" class="px-6 py-3">Imagen</th>
                        <th scope="col" class="px-6 py-3">Título</th>
                        <th scope="col" class="px-6 py-3">Categoría</th>
                        <th scope="col" class="px-6 py-3">Etiquetas</th>
                        <th scope="col" class="px-6 py-3">Fecha</th>
                        <th scope="col" class="px-6 py-3">Estado</th>
                        <th scope="col" class="px-6 py-3 text-center">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <!-- Skeleton de carga -->
                    <tr v-if="loading" v-for="i in 5" :key="`skeleton-${i}`">
                        <td colspan="6" class="px-6 py-4">
                            <div class="animate-pulse flex space-x-4">
                                <div class="flex-1 space-y-3 py-1">
                                    <div class="h-4 bg-gray-300 rounded w-3/4"></div>
                                </div>
                            </div>
                        </td>
                    </tr>

                    <!-- Mensaje cuando no hay artículos -->
                    <tr v-else-if="filteredArticulos.length === 0">
                        <td colspan="6" class="px-6 py-8 text-center text-gray-500">
                            <div class="flex flex-col items-center justify-center">
                                <svg class="w-12 h-12 mb-2 text-gray-400" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                                </svg>
                                <p class="text-lg font-medium">No se encontraron artículos</p>
                                <p class="text-sm">
                                    {{
                                        searchTerm || filtroCategoria
                                            ? "Intenta con otros términos de búsqueda"
                                            : "Agrega el primer artículo para comenzar"
                                    }}
                                </p>
                            </div>
                        </td>
                    </tr>

                    <!-- Lista de artículos -->
                    <tr v-else v-for="articulo in paginatedArticulos" :key="articulo.id"
                        class="odd:bg-white even:bg-gray-50 border-b-gray-500">
                        <td class="px-6 py-4">
                            <img v-if="articulo.imageUrl" :src="articulo.imageUrl" :alt="articulo.title"
                                class="w-20 h-12 object-cover rounded"
                                @error="(e) => (e.target.style.display = 'none')" />
                            <div v-else class="w-20 h-12 bg-gray-200 rounded flex items-center justify-center">
                                <svg class="w-6 h-6 text-gray-400" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2 2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                                </svg>
                            </div>
                        </td>
                        <td class="px-6 py-4 font-medium text-gray-900">
                            <div class="max-w-xs truncate">{{ articulo.title }}</div>
                            <div class="text-xs text-gray-500 mt-1">{{ articulo.slug }}</div>
                        </td>
                        <td class="px-6 py-4">
                            <span class="px-2 py-1 text-xs font-semibold rounded-full bg-gray-100 text-gray-800">
                                {{ articulo.categorias }}
                            </span>
                        </td>
                        <td class="px-6 py-4">
                            <div v-if="articulo.etiqueta" class="flex flex-wrap gap-1">
                                <span v-for="tag in articulo.etiqueta.split(',').map(e=>e.trim()).filter(Boolean)" :key="tag"
                                    class="inline-block bg-blue-100 text-blue-800 text-xs font-semibold px-2 py-0.5 rounded">
                                    {{ tag }}
                                </span>
                            </div>
                            <span v-else class="text-xs text-gray-400">—</span>
                        </td>
                        <td class="px-6 py-4">{{ formatearFecha(articulo.fechaCreacion) }}</td>
                        <td class="px-6 py-4">
                            <span :class="[
                                'px-2 py-1 text-xs font-semibold rounded-full',
                                articulo.activo
                                    ? 'bg-green-100 text-green-800'
                                    : 'bg-red-100 text-red-800',
                            ]">
                                {{ articulo.activo ? "Publicado" : "Borrador" }}
                            </span>
                        </td>
                        <td class="px-6 py-4">
                            <div class="flex items-center justify-center gap-2">
                                <button @click="verVistaPrevia(articulo)" type="button"
                                    class="flex items-center font-medium text-gray-600 hover:text-gray-800 hover:underline"
                                    title="Vista Previa">
                                    <svg class="w-5 h-5 me-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                            d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                            d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                                    </svg>
                                </button>
                                <button @click="editarArticulo(articulo)" type="button"
                                    class="flex items-center font-medium text-gray-600 hover:text-gray-800 hover:underline"
                                    title="Editar">
                                    <svg class="w-5 h-5 me-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                            d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                                    </svg>
                                </button>
                                <button @click="eliminarArticulo(articulo.id)" type="button"
                                    class="flex items-center font-medium text-gray-600 hover:text-gray-800 hover:underline"
                                    title="Eliminar">
                                    <svg class="w-5 h-5 me-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                            d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                    </svg>
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Paginación -->
        <div v-if="!loading && filteredArticulos.length > 0" class="flex justify-center items-center mt-6 space-x-2">
            <button @click="currentPage > 1 ? currentPage-- : null" :disabled="currentPage === 1" :class="[
                'px-3 py-2 text-sm font-medium rounded-lg',
                currentPage === 1
                    ? 'bg-gray-200 text-gray-400 cursor-not-allowed'
                    : 'bg-gray-700 text-white hover:bg-gray-800',
            ]">
                Anterior
            </button>
            <span class="text-sm text-gray-700">
                Página {{ currentPage }} de {{ totalPages }}
            </span>
            <button @click="currentPage < totalPages ? currentPage++ : null" :disabled="currentPage === totalPages"
                :class="[
                    'px-3 py-2 text-sm font-medium rounded-lg',
                    currentPage === totalPages
                        ? 'bg-gray-200 text-gray-400 cursor-not-allowed'
                        : 'bg-gray-700 text-white hover:bg-gray-800',
                ]">
                Siguiente
            </button>
        </div>

        <!-- Modales -->
        <modalBlog :isOpen="modalBlogOpen" :articulo="articuloSeleccionado" @close="cerrarModalBlog"
            @save="guardarArticulo" />

        <vistaBlog v-if="articuloPreview" :isOpen="vistaPreviewOpen" :articulo="articuloPreview" @close="cerrarVistaPrevia" />

        <!-- Modal de Importación Masiva JSON -->
        <Teleport to="body">
            <div v-if="mostrarModalImportar" class="fixed inset-0 z-50 overflow-y-auto">
                <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center">
                    <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" @click="cerrarModalImportar"></div>
                    
                    <div class="relative inline-block bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:max-w-2xl sm:w-full z-50">
                        <div class="bg-white px-4 pt-5 pb-4 sm:p-6">
                            <h3 class="text-lg font-medium text-gray-900 mb-4">📥 Importar Artículos desde JSON</h3>
                            
                            <div v-if="!importando" class="space-y-4">
                                <p class="text-sm text-gray-600">
                                    Pega el contenido JSON con los artículos a importar. El formato debe ser un array de objetos.
                                </p>
                                
                                <div class="bg-gray-50 border border-gray-200 rounded-lg p-3">
                                    <p class="text-xs text-gray-500 mb-2">Formato esperado:</p>
                                    <pre class="text-xs text-gray-600 overflow-x-auto">[
  {
    "Title": "Título del artículo",
    "Content": "&lt;p&gt;Contenido HTML&lt;/p&gt;",
    "Slug": "titulo-del-articulo",
    "Image URL": "https://...",
    "Categorías": "Informativo"
  }
]</pre>
                                </div>

                                <!-- Input para subir archivo -->
                                <div class="flex items-center gap-2">
                                    <label class="flex-1">
                                        <span class="text-sm font-medium text-gray-700">O sube un archivo .json:</span>
                                        <input type="file" accept=".json" @change="cargarArchivoJSON" 
                                            class="mt-1 block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:text-sm file:font-semibold file:bg-gray-100 file:text-gray-700 hover:file:bg-gray-200" />
                                    </label>
                                </div>

                                <!-- Textarea para pegar JSON -->
                                <div>
                                    <label class="text-sm font-medium text-gray-700">Pegar JSON:</label>
                                    <textarea v-model="jsonImportar" rows="10" 
                                        class="mt-1 block w-full rounded-lg border-gray-300 shadow-sm focus:border-gray-500 focus:ring-gray-500 text-sm font-mono"
                                        placeholder='[{"Title": "...", "Content": "...", "Slug": "..."}]'></textarea>
                                </div>

                                <!-- Validación y preview -->
                                <div v-if="jsonValidado" class="bg-green-50 border border-green-200 rounded-lg p-3">
                                    <p class="text-sm text-green-800">
                                        ✅ JSON válido. Se encontraron <strong>{{ articulosParaImportar.length }}</strong> artículos.
                                    </p>
                                </div>
                                <div v-else-if="errorJson" class="bg-red-50 border border-red-200 rounded-lg p-3">
                                    <p class="text-sm text-red-800">❌ {{ errorJson }}</p>
                                </div>
                            </div>

                            <div v-else class="space-y-4">
                                <p class="text-sm text-gray-600">Importando artículos... Por favor espera.</p>
                                
                                <div class="w-full bg-gray-200 rounded-full h-4">
                                    <div class="bg-blue-600 h-4 rounded-full transition-all duration-300" 
                                         :style="{ width: progresoImportacion.porcentaje + '%' }"></div>
                                </div>
                                
                                <p class="text-sm text-center text-gray-600">
                                    {{ progresoImportacion.actual }} / {{ progresoImportacion.total }} 
                                    ({{ progresoImportacion.porcentaje }}%)
                                </p>
                            </div>
                        </div>
                        
                        <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse gap-2">
                            <button v-if="!importando && jsonValidado" type="button" @click="iniciarImportacion"
                                class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm">
                                Iniciar Importación ({{ articulosParaImportar.length }})
                            </button>
                            <button v-if="!importando" type="button" @click="validarJSON"
                                class="w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:w-auto sm:text-sm">
                                Validar JSON
                            </button>
                            <button v-if="!importando" type="button" @click="cerrarModalImportar"
                                class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none sm:mt-0 sm:w-auto sm:text-sm">
                                Cancelar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </Teleport>
    </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import blogService from "~/services/blogService";
import modalBlog from "~/components/modalBlog.vue";
import vistaBlog from "~/components/vistaBlog.vue";
import Swal from "sweetalert2";

definePageMeta({
    layout: "admin",
    requiresAuth: true,
});

const articulos = ref([]);
const loading = ref(false);
const searchTerm = ref("");
const filtroCategoria = ref("");
const itemsPerPage = ref(10);
const currentPage = ref(1);
const modalBlogOpen = ref(false);
const vistaPreviewOpen = ref(false);
const articuloSeleccionado = ref(null);
const articuloPreview = ref(null);

// Variables para importación masiva
const mostrarModalImportar = ref(false);
const importando = ref(false);
const jsonImportar = ref("");
const jsonValidado = ref(false);
const errorJson = ref("");
const articulosParaImportar = ref([]);
const progresoImportacion = ref({ actual: 0, total: 0, porcentaje: 0 });

const cargarArticulos = async () => {
    loading.value = true;
    try {
        const response = await blogService.getAllArticulos();
        if (response.success) {
            articulos.value = response.data;
        }
    } catch (error) {
        console.error("Error al cargar artículos:", error);
        Swal.fire({
            icon: "error",
            title: "Error",
            text: "No se pudieron cargar los artículos",
        });
    } finally {
        loading.value = false;
    }
};

const handleSearch = () => {
    currentPage.value = 1;
};

// Extraer categorías únicas de los artículos cargados
const categoriasDisponibles = computed(() => {
    const categorias = articulos.value
        .map(articulo => articulo.categorias)
        .filter(categoria => categoria && categoria.trim() !== '');
    return [...new Set(categorias)].sort();
});

const ordenSeleccionado = ref('fecha-desc');
const filteredArticulos = computed(() => {
    let resultado = articulos.value;

    if (searchTerm.value) {
        resultado = resultado.filter(
            (articulo) =>
                articulo.title.toLowerCase().includes(searchTerm.value.toLowerCase()) ||
                articulo.slug.toLowerCase().includes(searchTerm.value.toLowerCase())
        );
    }

    if (filtroCategoria.value) {
        resultado = resultado.filter(
            (articulo) => articulo.categorias === filtroCategoria.value
        );
    }

    // Ordenamiento
    resultado = [...resultado];
    switch (ordenSeleccionado.value) {
        case 'fecha-asc':
            resultado.sort((a, b) => new Date(a.fechaCreacion) - new Date(b.fechaCreacion));
            break;
        case 'fecha-desc':
            resultado.sort((a, b) => new Date(b.fechaCreacion) - new Date(a.fechaCreacion));
            break;
        case 'titulo-asc':
            resultado.sort((a, b) => a.title.localeCompare(b.title));
            break;
        case 'titulo-desc':
            resultado.sort((a, b) => b.title.localeCompare(a.title));
            break;
    }
    return resultado;
});

const totalPages = computed(() => {
    return Math.ceil(filteredArticulos.value.length / itemsPerPage.value);
});

const paginatedArticulos = computed(() => {
    const start = (currentPage.value - 1) * itemsPerPage.value;
    const end = start + itemsPerPage.value;
    return filteredArticulos.value.slice(start, end);
});

const formatearFecha = (fecha) => {
    if (!fecha) return "";
    const date = new Date(fecha);
    return date.toLocaleDateString("es-GT", {
        year: "numeric",
        month: "short",
        day: "numeric",
    });
};

const abrirModalAgregar = () => {
    articuloSeleccionado.value = null;
    modalBlogOpen.value = true;
};

const editarArticulo = (articulo) => {
    articuloSeleccionado.value = { ...articulo };
    modalBlogOpen.value = true;
};

const cerrarModalBlog = () => {
    modalBlogOpen.value = false;
    articuloSeleccionado.value = null;
};

const guardarArticulo = async (datosArticulo) => {
    try {
        let response;
        if (articuloSeleccionado.value && articuloSeleccionado.value.id) {
            // Editar
            const datosCompletos = {
                ...datosArticulo,
                fechaCreacion: articuloSeleccionado.value.fechaCreacion,
                orden: articuloSeleccionado.value.orden || 0,
                permalink: articuloSeleccionado.value.permalink
            };
            response = await blogService.updateArticulo(
                articuloSeleccionado.value.id,
                datosCompletos
            );
        } else {
            // Crear nuevo
            response = await blogService.createArticulo(datosArticulo);
        }

        if (response.success) {
            await Swal.fire({
                icon: "success",
                title: "¡Éxito!",
                text: response.message,
                timer: 2000,
                showConfirmButton: false,
            });
            cerrarModalBlog();
            await cargarArticulos();
        }
    } catch (error) {
        // Mostrar el mensaje de error del backend si existe
        let mensajeError = "No se pudo guardar el artículo";
        if (error.message) {
            // Hacer el mensaje más amigable para el usuario
            if (error.message.includes("slug")) {
                mensajeError = "Ya existe un artículo con esta URL (slug). Por favor, utiliza un slug diferente.";
            } else {
                mensajeError = error.message;
            }
        }
        Swal.fire({
            icon: "error",
            title: "Error",
            text: mensajeError,
        });
    }
};

const verVistaPrevia = (articulo) => {
    articuloPreview.value = articulo;
    vistaPreviewOpen.value = true;
};

const cerrarVistaPrevia = () => {
    vistaPreviewOpen.value = false;
    articuloPreview.value = null;
};

// Funciones para importación masiva
const cerrarModalImportar = () => {
    if (!importando.value) {
        mostrarModalImportar.value = false;
        jsonImportar.value = "";
        jsonValidado.value = false;
        errorJson.value = "";
        articulosParaImportar.value = [];
    }
};

const cargarArchivoJSON = (event) => {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            jsonImportar.value = e.target.result;
            validarJSON();
        };
        reader.readAsText(file);
    }
};

const validarJSON = () => {
    errorJson.value = "";
    jsonValidado.value = false;
    articulosParaImportar.value = [];

    if (!jsonImportar.value.trim()) {
        errorJson.value = "Por favor, ingresa el contenido JSON";
        return;
    }

    try {
        const datos = JSON.parse(jsonImportar.value);
        
        if (!Array.isArray(datos)) {
            errorJson.value = "El JSON debe ser un array de artículos";
            return;
        }

        if (datos.length === 0) {
            errorJson.value = "El array está vacío";
            return;
        }

        // Validar que al menos tengan título y contenido
        const articulosValidos = datos.filter(art => {
            const titulo = art.Title || art.title || art.titulo;
            const contenido = art.Content || art.content || art.contenido;
            return titulo && contenido;
        });

        if (articulosValidos.length === 0) {
            errorJson.value = "No se encontraron artículos válidos. Cada artículo debe tener al menos 'Title' y 'Content'";
            return;
        }

        articulosParaImportar.value = articulosValidos;
        jsonValidado.value = true;
    } catch (e) {
        errorJson.value = `Error al parsear JSON: ${e.message}`;
    }
};

const iniciarImportacion = async () => {
    if (articulosParaImportar.value.length === 0) {
        Swal.fire({
            icon: "warning",
            title: "Sin datos",
            text: "No hay artículos para importar",
        });
        return;
    }

    const confirmacion = await Swal.fire({
        title: "¿Iniciar importación?",
        text: `Se importarán ${articulosParaImportar.value.length} artículos`,
        icon: "question",
        showCancelButton: true,
        confirmButtonColor: "#3b82f6",
        cancelButtonColor: "#6b7280",
        confirmButtonText: "Sí, importar",
        cancelButtonText: "Cancelar",
    });

    if (!confirmacion.isConfirmed) return;

    importando.value = true;
    progresoImportacion.value = { actual: 0, total: articulosParaImportar.value.length, porcentaje: 0 };

    try {
        const resultados = await blogService.importarArticulosMasivo(
            articulosParaImportar.value,
            (progreso) => {
                progresoImportacion.value = progreso;
            }
        );

        mostrarModalImportar.value = false;
        importando.value = false;
        jsonImportar.value = "";
        jsonValidado.value = false;
        articulosParaImportar.value = [];

        let mensajeResultado = `✅ Exitosos: ${resultados.exitosos}\n❌ Fallidos: ${resultados.fallidos}`;
        
        if (resultados.errores.length > 0) {
            mensajeResultado += "\n\nErrores:";
            resultados.errores.slice(0, 5).forEach(err => {
                mensajeResultado += `\n- ${err.titulo}: ${err.error}`;
            });
            if (resultados.errores.length > 5) {
                mensajeResultado += `\n... y ${resultados.errores.length - 5} más`;
            }
        }

        await Swal.fire({
            icon: resultados.fallidos === 0 ? "success" : "warning",
            title: "Importación completada",
            html: `<pre style="text-align: left; font-size: 12px; white-space: pre-wrap;">${mensajeResultado}</pre>`,
            width: 500,
        });

        await cargarArticulos();
    } catch (error) {
        importando.value = false;
        Swal.fire({
            icon: "error",
            title: "Error",
            text: "Ocurrió un error durante la importación: " + error.message,
        });
    }
};

const eliminarArticulo = async (id) => {
    const result = await Swal.fire({
        title: "¿Estás seguro?",
        text: "Esta acción no se puede revertir",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#374151",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar",
    });

    if (result.isConfirmed) {
        try {
            const response = await blogService.deleteArticulo(id);
            if (response.success) {
                await Swal.fire({
                    icon: "success",
                    title: "¡Eliminado!",
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false,
                });
                await cargarArticulos();
            }
        } catch (error) {
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "No se pudo eliminar el artículo",
            });
        }
    }
};

onMounted(() => {
    cargarArticulos();
});

// Eliminar todos los blogs
const eliminarTodosLosBlogs = async () => {
    const result = await Swal.fire({
        title: "¿Eliminar todos los artículos?",
        text: "Esta acción eliminará todos los artículos del blog y no se puede revertir.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#374151",
        confirmButtonText: "Sí, eliminar todos",
        cancelButtonText: "Cancelar",
    });
    if (result.isConfirmed) {
        // Eliminar uno por uno
        const total = articulos.value.length;
        let exitosos = 0;
        let fallidos = 0;
        let errores = [];
        for (let [i, articulo] of articulos.value.entries()) {
            try {
                await blogService.deleteArticulo(articulo.id);
                exitosos++;
            } catch (error) {
                fallidos++;
                errores.push({ titulo: articulo.title, error: error.message });
            }
            // Mostrar progreso
            await Swal.update({
                title: `Eliminando artículos... (${i + 1}/${total})`,
                text: `Eliminados: ${exitosos} | Fallidos: ${fallidos}`,
                icon: "info",
                showConfirmButton: false,
                allowOutsideClick: false,
                allowEscapeKey: false,
                allowEnterKey: false
            });
        }
        let mensajeResultado = `✅ Exitosos: ${exitosos}\n❌ Fallidos: ${fallidos}`;
        if (errores.length > 0) {
            mensajeResultado += "\n\nErrores:";
            errores.slice(0, 5).forEach(err => {
                mensajeResultado += `\n- ${err.titulo}: ${err.error}`;
            });
            if (errores.length > 5) {
                mensajeResultado += `\n... y ${errores.length - 5} más`;
            }
        }
        await Swal.fire({
            icon: fallidos === 0 ? "success" : "warning",
            title: "Eliminación completada",
            html: `<pre style="text-align: left; font-size: 12px; white-space: pre-wrap;">${mensajeResultado}</pre>`,
            width: 500,
        });
        await cargarArticulos();
    }
};
</script>