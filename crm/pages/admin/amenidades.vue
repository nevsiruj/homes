<template>
    <div>
      <div class="mt-5 mb-12">
        <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold  ">
          Gestión de Amenidades
        </h2>
      </div>
  
      <div class="grid grid-cols-2 grid-rows-1 gap-4 mb-8">
        <div>
          <form @submit.prevent class="flex items-center max-w-lg">
            <label for="voice-search" class="sr-only">Buscar</label>
            <div class="relative w-full">
              <input
                type="text"
                v-model="searchTerm"
                @input="handleSearch"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2          "
                placeholder="Buscar amenidades por nombre"
                required
              />
            </div>
            <button
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
            </button>
          </form>
        </div>
        <div class="flex justify-end items-end">
          <button
            type="button"
            @click="openAddModal"
            class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2    focus:outline-none "
          >
            Agregar amenidad
          </button>
        </div>
      </div>
  
      <!-- Tabla de amenidades -->
      <div class="relative overflow-x-auto">
        <table class="w-1/2 text-sm text-left rtl:text-right text-gray-500  mx-auto">
          <thead class="text-xs text-gray-700 uppercase bg-gray-50   ">
            <tr>
              <th scope="col" class="px-6 py-3">ID</th>
              <th scope="col" class="px-6 py-3">Nombre</th>
              <th scope="col" class="px-6 py-3">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="amenidad in filteredAmenidades"
              :key="amenidad.id"
              class="odd:bg-white odd: even:bg-gray-50 even: border-b  border-gray-200"
            >
              <td class="px-6 py-4">{{ amenidad.id }}</td>
              <td class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap  ">
                {{ amenidad.nombre }}
              </td>
              <td class="px-6 py-4 space-x-1 flex content-center justify-center">
                <button
                  @click="editAmenidad(amenidad)"
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
                  Editar
                </button>
                <button
                  @click="confirmDelete(amenidad.id)"
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
                  Eliminar
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- Mensaje cuando no hay Amenidades -->
      <div
        v-if="filteredAmenidades.length === 0"
        class="text-center py-8 text-gray-500"
      >
        {{
          searchTerm
            ? "No se encontraron amenidades que coincidan con la búsqueda."
            : "No hay amenidades registradas."
        }}
      </div>
  
      <!-- Modal para agregar/editar amenidad -->
      <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
        <div class="bg-white rounded-lg shadow-lg p-6 w-full max-w-md ">
          <div class="flex justify-between items-center mb-4">
            <h3 class="text-xl font-bold text-gray-900  ">
              {{ isEditing ? 'Editar Amenidad' : 'Agregar Amenidad' }}
            </h3>
            <button @click="closeModal" class="text-gray-400 hover:text-gray-500">
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
              </svg>
            </button>
          </div>
          
          <form @submit.prevent="submitForm">
            <div class="mb-4">
              <label for="nombre" class="block mb-2 text-sm font-medium text-gray-900  ">Nombre</label>
              <input
                type="text"
                id="nombre"
                v-model="currentAmenidad.nombre"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5          "
                required
              />
            </div>
            
            <div class="flex justify-end space-x-3">
              <button
                type="button"
                @click="closeModal"
                class="text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-gray-300 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10          "
              >
                Cancelar
              </button>
              <button
                type="submit"
                class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center    "
              >
                {{ isEditing ? 'Actualizar' : 'Guardar' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted, computed } from "vue";
  import Swal from "sweetalert2";
  import { initFlowbite } from "flowbite";
  
  definePageMeta({
    layout: "admin",
  });
  
  // Datos de ejemplo para amenidades
  const amenidadesEjemplo = [
    { id: 1, nombre: "Piscina" },
    { id: 2, nombre: "Gimnasio" },
    { id: 3, nombre: "Seguridad 24/7" },
    { id: 4, nombre: "Estacionamiento" }
  ];
  
  const amenidades = ref([]);
  const searchTerm = ref("");
  const showModal = ref(false);
  const isEditing = ref(false);
  const currentAmenidad = ref({
    id: null,
    nombre: ""
  });
  
  // Computed property para filtrar amenidades
  const filteredAmenidades = computed(() => {
    if (!searchTerm.value.trim()) {
      return amenidades.value;
    }
  
    const term = searchTerm.value.toLowerCase().trim();
    return amenidades.value.filter((amenidad) => {
      return amenidad.nombre && amenidad.nombre.toLowerCase().includes(term);
    });
  });
  
  // Obtener amenidades
  onMounted(async () => {
    initFlowbite();
    try {
      await loadAmenidades();
    } catch (error) {
      console.error("Error al cargar amenidades:", error);
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "No se pudieron cargar las amenidades",
      });
  
      // Usar datos de ejemplo si hay error (solo para desarrollo)
      amenidades.value = amenidadesEjemplo;
    }
  });
  
  // Función para cargar amenidades (reemplazar con llamada real a tu API)
  const loadAmenidades = async () => {
    // Simulando llamada a API con timeout
    return new Promise((resolve) => {
      setTimeout(() => {
        amenidades.value = amenidadesEjemplo;
        resolve();
      }, 500);
    });
  };
  
  // Función para manejar la búsqueda
  const handleSearch = () => {
    // La propiedad computada filteredAmenidades ya maneja la búsqueda
  };
  
  // Función para abrir modal de agregar
  const openAddModal = () => {
    currentAmenidad.value = {
      id: null,
      nombre: ""
    };
    isEditing.value = false;
    showModal.value = true;
  };
  
  // Función para abrir modal de edición
  const editAmenidad = (amenidad) => {
    currentAmenidad.value = { ...amenidad };
    isEditing.value = true;
    showModal.value = true;
  };
  
  // Función para cerrar modal
  const closeModal = () => {
    showModal.value = false;
  };
  
  // Función para enviar el formulario (agregar/editar)
  const submitForm = async () => {
    try {
      Swal.fire({
        title: isEditing.value ? "Actualizando..." : "Guardando...",
        html: "Por favor espera",
        allowOutsideClick: false,
        didOpen: () => {
          Swal.showLoading();
        },
      });
  
      // Simulación para el ejemplo
      setTimeout(() => {
        if (isEditing.value) {
          // Editar amenidad existente
          const index = amenidades.value.findIndex(a => a.id === currentAmenidad.value.id);
          if (index !== -1) {
            amenidades.value[index] = { ...currentAmenidad.value };
          }
        } else {
          // Agregar nueva amenidad
          const newId = Math.max(...amenidades.value.map(a => a.id), 0) + 1;
          amenidades.value.push({
            ...currentAmenidad.value,
            id: newId
          });
        }
  
        Swal.fire({
          icon: "success",
          title: "¡Éxito!",
          text: isEditing.value 
            ? "Amenidad actualizada correctamente" 
            : "Amenidad agregada correctamente",
        });
        
        showModal.value = false;
      }, 1000);
    } catch (error) {
      console.error("Error al guardar amenidad:", error);
      Swal.fire("Error", "Ocurrió un error al guardar la amenidad", "error");
    }
  };
  
  // Función para confirmar y eliminar amenidad
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
  
        // Simulación para el ejemplo
        setTimeout(() => {
          amenidades.value = amenidades.value.filter((amenidad) => amenidad.id !== id);
          Swal.fire("¡Eliminado!", "La amenidad ha sido eliminada.", "success");
        }, 1000);
      }
    } catch (error) {
      console.error("Error al eliminar amenidad:", error);
      Swal.fire("Error", "Ocurrió un error al eliminar la amenidad", "error");
    }
  };
  </script>