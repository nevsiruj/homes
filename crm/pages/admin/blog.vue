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
                        <select v-model="filtroCategoria"
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2.5 flex-1 sm:flex-none sm:w-auto">
                            <option value="">Todas</option>
                            <option v-for="categoria in categoriasDisponibles" :key="categoria" :value="categoria">
                                {{ categoria }}
                            </option>
                        </select>
                        <button type="submit"
                            class="inline-flex items-center justify-center py-2.5 px-4 text-sm font-medium text-white bg-gray-700 rounded-lg border border-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 whitespace-nowrap">
                            <svg class="w-4 h-4 sm:me-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none"
                                viewBox="0 0 20 20">
                                <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
                            </svg>
                            <span class="hidden sm:inline">Buscar</span>
                        </button>
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
                                        d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
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
</script>