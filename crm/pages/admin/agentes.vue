<template>
  <div class="p-4">
    <!-- Encabezado -->
    <div class="mt-5 mb-12">
      <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold  ">Gestión de Agentes Inmobiliarios</h2>
    </div>

    <!-- Buscador y botón Agregar -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
      <div>
        <form @submit.prevent="handleSearch" class="flex items-center max-w-lg">
          <label for="voice-search" class="sr-only">Buscar</label>
          <div class="relative w-full">
            <input
              v-model="searchTerm"
              type="text"
              id="voice-search"
              placeholder="Buscar por nombre o ID..."
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2          "
            />
          </div>
          <button
            type="submit"
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

      <div class="flex justify-end">
        <button
          type="button"
          @click="showAgregarModal = true"
          class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2    "
        >
          Agregar Agente
        </button>
      </div>
    </div>

    <!-- Tabla de agentes -->
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg bg-white ">
      <table class="w-full text-sm text-left rtl:text-right text-gray-500 ">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50   ">
          <tr>
            <th scope="col" class="px-6 py-3">Rol</th>
            <th scope="col" class="px-6 py-3">Nombre Completo</th>
            <th scope="col" class="px-6 py-3">DNI</th>
            <!-- <th scope="col" class="px-6 py-3">Teléfono</th> -->
            <th scope="col" class="px-6 py-3">Especialidad</th>
            
            <th scope="col" class="px-6 py-3 text-center">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="agente in filteredAgentes"
            :key="agente.id"
            class="odd:bg-white odd: even:bg-gray-50 even:  border-b-gray-500 "
          >
            <td class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap  ">{{ agente.rol }}</td>
            <td class="px-6 py-4">{{ agente.nombre }} {{ agente.apellido }}</td>
            <td class="px-6 py-4">{{ agente.dni }}</td>
            <!-- <td class="px-6 py-4">{{ agente.telefono }} </td> -->
            <td class="px-6 py-4">{{ agente.especialidad }}</td>
            
            <td class="px-6 py-4 text-center space-x-2 flex content-center justify-center">
              <button
                @click="openDetailModal(agente)"
                type="button"
                class="flex items-center font-medium text-gray-600 hover:text-gray-800 hover:underline"
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
                Detalles
              </button>

              <!-- <button
                @click="editAgente(agente.id)"
                type="button"
                class="flex font-medium text-gray-600 hover:text-gray-800 hover:underline"
              >
                <svg
                  class="w-5 h-5 me-1"
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
              </button>-->

              <!-- <button
                @click="confirmDelete(agente.id)"
                type="button"
                class="flex font-medium text-gray-600 hover:text-gray-800 hover:underline"
              >
                <svg
                  class="w-5 h-5 me-1"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M20.5001 6H3.5"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M9.5 11L10 16"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M14.5 11L14 16"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65649 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                    stroke="#000000"
                    stroke-width="1.5"
                  />
                </svg>
                Eliminar
              </button>  -->
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal: Agregar Agente -->
    <AgregarAgente 
      :isOpen="showAgregarModal" 
      @close="showAgregarModal = false"
      @agente-agregado="loadAgentes"
    />

    <!-- Modal: Detalle del Agente -->
    <DetalleAgente :isOpen="isDetailOpen" :agente="selectedAgente" @close="closeDetailModal" />

    
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import Swal from 'sweetalert2';
import AgregarAgente from '../../components/agregarAgente.vue';
import DetalleAgente from '../../components/detalleAgente.vue';
import agenteService from '@/services/agenteService';
import userService from '@/services/userService';
import { useAuthStore } from "@/stores/auth";

const auth = useAuthStore();
const isAdmin = computed(() =>
  typeof auth.hasRole === "function"
    ? auth.hasRole("admin")
    : (auth.roles || []).includes("admin")
);

definePageMeta({
    layout: "admin",
    requiresAuth: true, roles: ['admin']
  });

// ----------------- Nuevo: helpers de alertas amigables -----------------
const showAlert = (opts = {}) => {
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

// const showErrorFriendly = (userMessage = "Ocurrió un problema. Por favor intenta de nuevo.", errorToLog = null) => {
//   // console técnico comentado para debugging (no mostrar al usuario)
//   if (errorToLog) {
//     /* console.error(errorToLog); */
//   }
//   return showAlert({ icon: "error", title: "Algo salió mal", text: userMessage });
// };

// const showInfoFriendly = (userMessage = "", title = "Información") =>
//   showAlert({ icon: "info", title, text: userMessage });

const showSuccessTransient = (userMessage = "", timer = 1200) =>
  showAlert({ icon: "success", title: "", text: userMessage, timer, showConfirmButton: false });

// === Datos reactivos ===
const agentes = ref([]);
const searchTerm = ref('');
const showAgregarModal = ref(false);
const isDetailOpen = ref(false);
const selectedAgente = ref(null);

// === Filtrar agentes ===
const filteredAgentes = computed(() => {
  if (!searchTerm.value.trim()) return agentes.value;

  const query = searchTerm.value.toLowerCase().trim();
  return agentes.value.filter(a => {
    // Verifica cada campo individualmente para evitar errores si algún campo es null/undefined
    return (
      (a.nombre?.toLowerCase().includes(query) || '') ||
      (a.apellido?.toLowerCase().includes(query) || '') ||
      (a.dni?.toString().includes(query) || '') ||
      (a.especialidad?.toLowerCase().includes(query) || '') ||
      (a.rol?.toLowerCase().includes(query) || '')
    );
  });
});

const handleSearch = () => {
  
  //console.log("Buscando:", searchTerm.value);
};

// === Cargar agentes desde API ===
async function loadAgentes() {
  try {
    const data = await agenteService.getUsers(); // Llamada real al backend
    //console.log("📥 Agentes cargados:", data);

    // Mapea los datos del backend (asumiendo formato PascalCase)
    agentes.value = data.$values || [];
  } catch (error) {
    // Log técnico en consola, mensaje amigable al usuario
    // await showErrorFriendly(
    //   "No fue posible cargar la lista de agentes. Revisa tu conexión e inténtalo nuevamente.",
    //   error
    // );
    agentes.value = []; // Deja la tabla vacía si falla
  }
}

// === Abrir modal detalle ===
function openDetailModal(agente) {
  selectedAgente.value = agente;
  isDetailOpen.value = true;
}

// Función para cerrar modal y recargar datos si es necesario
const closeDetailModal = (shouldReload = false) => {
  isDetailOpen.value = false;
  //inmuebleToEdit.value = null; 
  if (shouldReload) {
    loadAgentes();
  }
};

// === Editar agente ===
// function editAgente(id) {
//   Swal.fire("Info", "Función pendiente", "info");
//   // Aquí puedes implementar después el modal de edición
// }

// === Eliminar agente (usa userService.removeUsuario) ===
// async function confirmDelete(id) {
//   const result = await Swal.fire({
//     title: '¿Estás seguro?',
//    text: 'Este agente será eliminado permanentemente.',
//    icon: 'warning',
//     showCancelButton: true,
//     confirmButtonColor: '#d33',
//     cancelButtonColor: '#3085d6',
//     confirmButtonText: 'Sí, eliminar',
//     cancelButtonText: 'Cancelar'
//   });

//   if (!result.isConfirmed) return;

//   try {
//     Swal.fire({
//       title: 'Eliminando…',
//       html: 'Por favor espera',
//       allowOutsideClick: false,
//       didOpen: () => Swal.showLoading(),
//     });

//     await userService.removeUsuario(id);       // ⬅️ aquí va tu endpoint real
//     agentes.value = agentes.value.filter(a => a.id !== id);

//     Swal.fire('¡Eliminado!', 'Agente eliminado correctamente.', 'success');
//   } catch (error) {
//     //console.error("❌ Error al eliminar agente:", error?.message || error);
//     Swal.fire('Error', error?.message || 'No se pudo eliminar el agente.', 'error');
//   }
// }



// === Cargar agentes al montar ===
onMounted(async () => {
  await loadAgentes();
});
</script>