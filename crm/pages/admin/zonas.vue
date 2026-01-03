<template>
    <div class="p-4">
        
        <div class="mt-5 mb-12">
            <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
                Gestión de Zonas
            </h2>
        </div>

        
        <div class="grid grid-cols-1 gap-4 mb-8 lg:grid-cols-3 xl:grid-cols-4">
            <div class="lg:col-span-2 xl:col-span-3">
                <form @submit.prevent="handleSearch" class="flex items-center">
                    <label for="search-zona" class="sr-only">Buscar</label>
                    <div class="relative w-full">
                        <input v-model="searchTerm" type="text" id="search-zona" placeholder="Buscar zona..."
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2" />
                    </div>
                    <button type="submit"
                        class="inline-flex items-center py-2 px-3 ms-2 text-sm font-medium text-white bg-gray-700 rounded-lg border border-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300">
                        <svg class="w-4 h-4 me-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none"
                            viewBox="0 0 20 20">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
                        </svg>
                        Buscar
                    </button>
                </form>
            </div>

            <div class="flex justify-end items-end gap-2">
                <select v-model.number="itemsPerPage" @change="currentPage = 1"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2">
                    <option :value="20">20 por página</option>
                    <option :value="50">50 por página</option>
                    <option :value="100">100 por página</option>
                </select>
                <button type="button" @click="agregarZona"
                    class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none">
                    Agregar Zona
                </button>
            </div>
        </div>

        
        <div class="relative overflow-x-auto shadow-md sm:rounded-lg bg-white">
            <table class="w-full text-sm text-left rtl:text-right text-gray-500">
                <thead class="text-xs text-gray-700 uppercase bg-gray-50">
                    <tr>
                        <th scope="col" class="px-6 py-3">ID</th>
                        <th scope="col" class="px-6 py-3">Nombre de la Zona</th>
                        <th scope="col" class="px-6 py-3">Estado</th>
                        <th scope="col" class="px-6 py-3 text-center">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    
                    <tr v-if="loading" v-for="i in 5" :key="`skeleton-${i}`">
                        <td colspan="4" class="px-6 py-4">
                            <div class="animate-pulse flex space-x-4">
                                <div class="flex-1 space-y-3 py-1">
                                    <div class="h-4 bg-gray-300 rounded w-3/4"></div>
                                </div>
                            </div>
                        </td>
                    </tr>

                    
                    <tr v-else-if="filteredZonas.length === 0">
                        <td colspan="4" class="px-6 py-8 text-center text-gray-500">
                            <div class="flex flex-col items-center justify-center">
                                <svg class="w-12 h-12 mb-2 text-gray-400" fill="none" stroke="currentColor"
                                    viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                                </svg>
                                <p class="text-lg font-medium">No se encontraron zonas</p>
                                <p class="text-sm">
                                    {{
                                        searchTerm
                                            ? "Intenta con otros términos de búsqueda"
                                            : "Agrega la primera zona para comenzar"
                                    }}
                                </p>
                            </div>
                        </td>
                    </tr>

                    
                    <tr v-else v-for="zona in paginatedZonas" :key="zona.id"
                        class="odd:bg-white even:bg-gray-50 border-b-gray-500">
                        <td class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap">
                            {{ zona.id }}
                        </td>
                        <td class="px-6 py-4">{{ zona.nombre }}</td>
                        <td class="px-6 py-4">
                            <span :class="[
                                'px-2 py-1 text-xs font-semibold rounded-full',
                                zona.activo
                                    ? 'bg-green-100 text-green-800'
                                    : 'bg-red-100 text-red-800',
                            ]">
                                {{ zona.activo ? "Activo" : "Inactivo" }}
                            </span>
                        </td>
                        <td class="px-6 py-4 text-center space-x-2 flex content-center justify-center">
                            <button @click="editarZona(zona)" type="button"
                                class="flex items-center font-medium text-gray-600 hover:text-gray-800 hover:underline">
                                <svg class="w-5 h-5 me-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                                </svg>
                                Editar
                            </button>
                            <button @click="eliminarZona(zona.id)" type="button"
                                class="flex items-center font-medium text-red-600 hover:text-red-800 hover:underline">
                                <svg class="w-5 h-5 me-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                        d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                </svg>
                                Eliminar
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        
        <div v-if="!loading && filteredZonas.length > 0" class="flex justify-center items-center mt-6 space-x-2">
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
    </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import zonaService from '~/services/zonaService';
import Swal from 'sweetalert2';

definePageMeta({
    layout: "admin",
    requiresAuth: true,
});

const zonas = ref([]);
const loading = ref(false);
const searchTerm = ref('');
const itemsPerPage = ref(20);
const currentPage = ref(1);

const cargarZonas = async () => {
    loading.value = true;
    try {
        const response = await zonaService.getAllZonas();
        if (response.success) {
            zonas.value = response.data;
        }
    } catch (error) {
        console.error('Error al cargar zonas:', error);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudieron cargar las zonas',
        });
    } finally {
        loading.value = false;
    }
};

const handleSearch = () => {
    currentPage.value = 1;
};

const filteredZonas = computed(() => {
    if (!searchTerm.value) {
        return zonas.value;
    }
    return zonas.value.filter((zona) =>
        zona.nombre.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
});

const totalPages = computed(() => {
    return Math.ceil(filteredZonas.value.length / itemsPerPage.value);
});

const paginatedZonas = computed(() => {
    const start = (currentPage.value - 1) * itemsPerPage.value;
    const end = start + itemsPerPage.value;
    return filteredZonas.value.slice(start, end);
});

const agregarZona = async () => {
    const { value: nombre } = await Swal.fire({
        title: 'Agregar Nueva Zona',
        input: 'text',
        inputLabel: 'Nombre de la zona',
        inputPlaceholder: 'Ej: Zona 17',
        showCancelButton: true,
        confirmButtonText: 'Agregar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#374151',
        inputValidator: (value) => {
            if (!value) {
                return 'Debes ingresar un nombre para la zona';
            }
        }
    });

    if (nombre) {
        try {
            const response = await zonaService.createZona({ nombre });
            if (response.success) {
                await Swal.fire({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
                await cargarZonas();
            }
        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo agregar la zona',
            });
        }
    }
};

const editarZona = async (zona) => {
    const { value: nombre } = await Swal.fire({
        title: 'Editar Zona',
        input: 'text',
        inputLabel: 'Nombre de la zona',
        inputValue: zona.nombre,
        showCancelButton: true,
        confirmButtonText: 'Actualizar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#374151',
        inputValidator: (value) => {
            if (!value) {
                return 'Debes ingresar un nombre para la zona';
            }
        }
    });

    if (nombre && nombre !== zona.nombre) {
        try {
            const response = await zonaService.updateZona(zona.id, { nombre });
            if (response.success) {
                await Swal.fire({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
                await cargarZonas();
            }
        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo actualizar la zona',
            });
        }
    }
};

const eliminarZona = async (id) => {
    const result = await Swal.fire({
        title: '¿Estás seguro?',
        text: 'Esta acción no se puede revertir',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#374151',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
        try {
            const response = await zonaService.deleteZona(id);
            if (response.success) {
                await Swal.fire({
                    icon: 'success',
                    title: '¡Eliminado!',
                    text: response.message,
                    timer: 2000,
                    showConfirmButton: false
                });
                await cargarZonas();
            }
        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo eliminar la zona',
            });
        }
    }
};

onMounted(() => {
    cargarZonas();
});
</script>

<style scoped>
/* Estilos adicionales si son necesarios */ 
</style>