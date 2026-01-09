<template>
    <div class="p-4">
        
        <div class="mt-5 mb-12">
            <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
                Gestión de Zonas
            </h2>
        </div>

        <!-- Barra de búsqueda y controles -->
        <div class="flex flex-col gap-4 mb-8 md:flex-row md:items-center md:justify-between">
            <div class="flex-1 max-w-2xl">
                <form @submit.prevent="handleSearch" class="flex items-center gap-2">
                    <label for="search-zona" class="sr-only">Buscar</label>
                    <div class="relative flex-1">
                        <input v-model="searchTerm" type="text" id="search-zona" placeholder="Buscar zona..."
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2.5" />
                    </div>
                    <button type="submit"
                        class="inline-flex items-center py-2.5 px-4 text-sm font-medium text-white bg-gray-700 rounded-lg border border-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 whitespace-nowrap">
                        <svg class="w-4 h-4 md:me-2" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none"
                            viewBox="0 0 20 20">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
                        </svg>
                        <span class="hidden md:inline">Buscar</span>
                    </button>
                </form>
            </div>

            <div class="flex flex-col sm:flex-row gap-2">
                <select v-model.number="itemsPerPage" @change="currentPage = 1"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2.5 w-full sm:w-auto">
                    <option :value="20">20 por página</option>
                    <option :value="50">50 por página</option>
                    <option :value="100">100 por página</option>
                </select>
                <button type="button" @click="agregarZona"
                    class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none whitespace-nowrap w-full sm:w-auto">
                    <span class="inline md:hidden">+ Zona</span>
                    <span class="hidden md:inline">Agregar Zona</span>
                </button>
            </div>
        </div>

        <!-- Tabla de zonas -->
        <div v-if="searchTerm" class="bg-yellow-50 border-l-4 border-yellow-400 p-4 mb-4">
            <div class="flex">
                <div class="flex-shrink-0">
                    <svg class="h-5 w-5 text-yellow-400" viewBox="0 0 20 20" fill="currentColor">
                        <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
                    </svg>
                </div>
                <div class="ml-3">
                    <p class="text-sm text-yellow-700">
                        El reordenamiento está deshabilitado mientras hay filtros activos. Limpia la búsqueda para poder reordenar.
                    </p>
                </div>
            </div>
        </div>
        <div class="relative overflow-x-auto shadow-md sm:rounded-lg bg-white">
            <table class="w-full text-sm text-left rtl:text-right text-gray-500">
                <thead class="text-xs text-gray-700 uppercase bg-gray-50">
                    <tr>
                        <th scope="col" class="px-6 py-3">Orden</th>
                        <th scope="col" class="px-6 py-3">Nombre de la Zona</th>
                        <th scope="col" class="px-6 py-3">Estado</th>
                        <th scope="col" class="px-6 py-3 text-center">Acciones</th>
                    </tr>
                </thead>
                <draggable 
                    v-model="zonasParaReordenar" 
                    tag="tbody"
                    item-key="id"
                    :disabled="searchTerm !== '' || loading"
                    handle=".drag-handle"
                    @end="onDragEnd"
                    ghost-class="ghost-row">
                    <template #item="{ element: zona, index }">
                        <tr :key="zona.id" class="odd:bg-white even:bg-gray-50 border-b-gray-500">
                        <td class="px-6 py-4">
                            <div class="flex items-center gap-3">
                                <div v-if="!searchTerm" class="drag-handle cursor-move" title="Arrastra para reordenar">
                                    <svg class="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/>
                                    </svg>
                                </div>
                                <span class="font-medium text-gray-900">{{ zona.orden }}</span>
                            </div>
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
                    </template>
                    <template #footer v-if="loading">
                        <tr v-for="i in 5" :key="`skeleton-${i}`">
                            <td colspan="4" class="px-6 py-4">
                                <div class="animate-pulse flex space-x-4">
                                    <div class="flex-1 space-y-3 py-1">
                                        <div class="h-4 bg-gray-300 rounded w-3/4"></div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </template>
                    <template #footer v-if="!loading && zonasParaReordenar.length === 0">
                        <tr>
                            <td colspan="4" class="px-6 py-8 text-center text-gray-500">
                                <div class="flex flex-col items-center justify-center">
                                    <svg class="w-12 h-12 mb-2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
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
                    </template>
                </draggable>
            </table>
        </div>

        <!-- Paginación (ocultar si no hay filtro porque draggable muestra todas) -->
        <div v-if="!loading && filteredZonas.length > 0 && searchTerm" class="flex justify-center items-center mt-6 space-x-2">
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
import draggable from 'vuedraggable';

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
        // console.error('Error al cargar zonas:', error);
        Swal.fire({
            icon: 'error',
            title: '¡Ups! Algo salió mal',
            text: 'No pudimos cargar las zonas en este momento. Por favor, intenta nuevamente o contacta al administrador si el problema persiste.',
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

const zonasParaReordenar = computed({
    get() {
        // Usar filteredZonas en lugar de paginatedZonas para tener todas las zonas
        return filteredZonas.value;
    },
    set(value) {
        // Actualizar el array completo de zonas
        zonas.value = value;
    }
});

const onDragEnd = async () => {
    // console.log('🔄 onDragEnd iniciado');
    // console.log('📝 searchTerm:', searchTerm.value);
    
    if (searchTerm.value) {
        // console.log('❌ Cancelado: hay filtro activo');
        return;
    }

    try {
        // console.log('📊 Total de zonas en el sistema:', zonas.value.length);
        // console.log('📊 zonasParaReordenar (todas):', zonasParaReordenar.value);
        
        // Crear array con el nuevo orden basado en TODAS las zonas
        const nuevoOrden = zonas.value.map(z => z.id);
        // console.log('🆕 Nuevo orden completo (IDs de todas las zonas):', nuevoOrden);
        // console.log('📊 Cantidad de IDs enviados:', nuevoOrden.length);
        
        // console.log('🚀 Enviando al backend...');
        const response = await zonaService.reorderZonas(nuevoOrden);
        // console.log('✅ Respuesta del backend:', response);
        
        if (response.success) {
            await cargarZonas();
            Swal.fire({
                icon: 'success',
                title: '¡Éxito!',
                text: 'Orden actualizado correctamente',
                timer: 1500,
                showConfirmButton: false
            });
        }
    } catch (error) {
        // console.error('❌ Error al reordenar zonas:', error);
        Swal.fire({
            icon: 'error',
            title: '¡Ups! No pudimos guardar el orden',
            text: 'Hubo un problema al actualizar el orden de las zonas. Por favor, intenta nuevamente.',
        });
        await cargarZonas(); // Recargar para restaurar el orden original
    }
};

const agregarZona = async () => {
    const { value: nombres } = await Swal.fire({
        title: 'Agregar Nueva(s) Zona(s)',
        html: `
            <div style="text-align: left;">
                <label for="swal-input-zonas" style="display: block; margin-bottom: 8px; font-weight: 500; color: #374151;">
                    Nombre de las zonas
                </label>
                <textarea 
                    id="swal-input-zonas" 
                    class="swal2-textarea" 
                    style="width: 100%; margin: 0; min-height: 100px; resize: vertical;" 
                    placeholder="Ej: Zona 17, Zona 18, Zona Mixco"></textarea>
                <p style="margin-top: 8px; font-size: 12px; color: #6b7280;">
                    💡 Puedes agregar múltiples zonas separándolas con comas
                </p>
            </div>
        `,
        focusConfirm: false,
        showCancelButton: true,
        confirmButtonText: 'Agregar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#374151',
        preConfirm: () => {
            const value = document.getElementById('swal-input-zonas').value;
            if (!value || !value.trim()) {
                Swal.showValidationMessage('Debes ingresar al menos un nombre de zona');
                return false;
            }
            return value;
        }
    });

    if (nombres) {
        try {
            // Dividir por comas y limpiar espacios
            const listaZonas = nombres
                .split(',')
                .map(z => z.trim())
                .filter(z => z.length > 0);

            if (listaZonas.length === 0) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Información incompleta',
                    text: 'Por favor, ingresa al menos un nombre de zona válido para continuar.',
                });
                return;
            }

            // Mostrar loader mientras se procesan
            Swal.fire({
                title: 'Procesando...',
                html: `Agregando ${listaZonas.length} zona(s)...`,
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            let exitosas = 0;
            let fallidas = 0;
            const errores = [];

            // Crear cada zona
            for (const nombre of listaZonas) {
                try {
                    const response = await zonaService.createZona({ nombre });
                    if (response.success) {
                        exitosas++;
                    } else {
                        fallidas++;
                        // Detectar si es error de duplicado
                        const mensaje = response.message?.toLowerCase() || '';
                        if (mensaje.includes('duplica') || mensaje.includes('existe') || mensaje.includes('already')) {
                            errores.push(`${nombre}: Ya existe una zona con este nombre`);
                        } else {
                            errores.push(`${nombre}: ${response.message || 'No se pudo crear'}`);
                        }
                    }
                } catch (error) {
                    fallidas++;
                    const errorMsg = error?.response?.data?.message?.toLowerCase() || error?.message?.toLowerCase() || '';
                    if (errorMsg.includes('duplica') || errorMsg.includes('existe') || errorMsg.includes('already')) {
                        errores.push(`${nombre}: Ya existe una zona con este nombre`);
                    } else {
                        errores.push(`${nombre}: No se pudo crear`);
                    }
                }
            }

            // Mostrar resultado
            await cargarZonas();

            if (fallidas === 0) {
                await Swal.fire({
                    icon: 'success',
                    title: '¡Éxito!',
                    text: `${exitosas} zona(s) agregada(s) correctamente`,
                    timer: 2000,
                    showConfirmButton: false
                });
            } else if (exitosas === 0) {
                await Swal.fire({
                    icon: 'error',
                    title: '¡Ups! No pudimos crear las zonas',
                    html: `Ocurrió un problema al agregar las zonas. Por favor, verifica la información e intenta nuevamente.<br><br><small style="color: #6b7280;">${errores.join('<br>')}</small>`,
                });
            } else {
                await Swal.fire({
                    icon: 'warning',
                    title: 'Completado con errores',
                    html: `
                        <p><strong>Exitosas:</strong> ${exitosas}</p>
                        <p><strong>Fallidas:</strong> ${fallidas}</p>
                        <br>
                        <p style="text-align: left; font-size: 12px;">
                            ${errores.join('<br>')}
                        </p>
                    `,
                });
            }
        } catch (error) {
            Swal.fire({
                icon: 'error',
                title: '¡Ups! Algo salió mal',
                text: 'No pudimos procesar las zonas en este momento. Por favor, intenta nuevamente.',
            });
        }
    }
};

const editarZona = async (zona) => {
    const { value: formValues } = await Swal.fire({
        title: 'Editar Zona',
        html: `
            <div style="text-align: left;">
                <label for="swal-input-nombre" style="display: block; margin-bottom: 8px; font-weight: 500; color: #374151;">
                    Nombre de la zona
                </label>
                <input id="swal-input-nombre" class="swal2-input" style="width: 100%; margin: 0;" placeholder="Nombre de la zona" value="${zona.nombre}">
                
                <div style="margin-top: 20px;">
                    <label style="display: flex; align-items: center; cursor: pointer;">
                        <input type="checkbox" id="swal-input-activo" ${zona.activo ? 'checked' : ''} style="width: 18px; height: 18px; margin-right: 8px; cursor: pointer;">
                        <span style="font-weight: 500; color: #374151;">Zona activa</span>
                    </label>
                </div>
            </div>
        `,
        focusConfirm: false,
        showCancelButton: true,
        confirmButtonText: 'Actualizar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#374151',
        preConfirm: () => {
            const nombre = document.getElementById('swal-input-nombre').value;
            const activo = document.getElementById('swal-input-activo').checked;
            
            if (!nombre) {
                Swal.showValidationMessage('Debes ingresar un nombre para la zona');
                return false;
            }
            
            return { nombre, activo };
        }
    });

    if (formValues) {
        const { nombre, activo } = formValues;
        
        if (nombre !== zona.nombre || activo !== zona.activo) {
            try {
                const response = await zonaService.updateZona(zona.id, { 
                    nombre, 
                    activo,
                    orden: zona.orden 
                });
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
                    title: '¡Ups! No pudimos actualizar',
                    text: 'Hubo un problema al guardar los cambios de la zona. Por favor, intenta nuevamente.',
                });
            }
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
                title: '¡Ups! No pudimos eliminar',
                text: 'Hubo un problema al eliminar la zona. Por favor, intenta nuevamente o verifica que no esté siendo utilizada.',
            });
        }
    }
};

onMounted(() => {
    cargarZonas();
});
</script>

<style scoped>
.drag-handle {
    transition: color 0.2s;
}

.drag-handle:hover {
    color: #374151;
}

.ghost-row {
    opacity: 0.5;
    background: #f3f4f6;
}

tbody tr {
    transition: all 0.3s ease;
}
</style>