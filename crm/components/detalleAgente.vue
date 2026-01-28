<template>
  <div
    v-if="isOpen"
    class="modal-overlay fixed inset-0 bg-black bg-opacity-60 z-50 flex items-center justify-center p-4 backdrop-blur-sm"
    aria-labelledby="modal-title"
    role="dialog"
    aria-modal="true"
  >
    <div class="flex items-center justify-center w-full h-full">
      <!-- Fondo oscuro -->
      <div
        class="fixed inset-0 bg-gray-900/50 transition-opacity"
        aria-hidden="true"
        @click="closeModal"
      ></div>

      <!-- Contenido del modal -->
      <div
        class="relative bg-white rounded-2xl shadow-2xl transform transition-all overflow-hidden"
        style="width: 90%; max-width: 1200px; max-height: 90vh;"
      >
        <div class="flex flex-col h-full" style="max-height: 90vh;">
          <!-- Header mejorado con gradiente -->
          <div class="relative bg-linear-to-r from-green-900 to-green-700 text-white p-6 md:p-8 shrink-0">
            <button
              @click="closeModal"
              type="button"
              class="absolute top-4 right-4 text-white hover:bg-white/20 rounded-full p-2 transition-all duration-200 z-10"
            >
              <svg
                class="w-6 h-6"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M6 18L18 6M6 6l12 12"
                ></path>
              </svg>
            </button>
            
            <div class="pr-12">
              <h3 class="text-2xl md:text-3xl font-bold mb-2">
                {{ agente.nombre }} {{ agente.apellido }}
              </h3>
              <div class="flex flex-wrap items-center gap-4 text-sm text-green-100">
                <span class="flex items-center gap-1">
                  <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z"/>
                    <path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z"/>
                  </svg>
                  {{ agente.email || 'Sin email' }}
                </span>
                <span class="flex items-center gap-1">
                  <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M2 3a1 1 0 011-1h2.153a1 1 0 01.986.836l.74 4.435a1 1 0 01-.54 1.06l-1.548.773a11.037 11.037 0 006.105 6.105l.774-1.548a1 1 0 011.059-.54l4.435.74a1 1 0 01.836.986V17a1 1 0 01-1 1h-2C7.82 18 2 12.18 2 5V3z"/>
                  </svg>
                  {{ agente.telefono || 'Sin teléfono' }}
                </span>
                <span class="px-3 py-1 bg-white/20 rounded-full text-xs font-medium capitalize">
                  {{ agente.rol || 'Agente' }}
                </span>
              </div>
            </div>
          </div>

          <!-- Contenido con scroll mejorado -->
          <div class="flex-1 overflow-y-auto p-6 md:p-8 bg-gray-50">
            <div class="space-y-6">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <!-- Información Personal -->
                <div class="bg-white rounded-xl shadow-md p-6">
                  <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                    <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clip-rule="evenodd"/>
                    </svg>
                    Información Personal
                  </h4>
                  <div class="space-y-4">
                    <div class="flex justify-between items-center pb-3 border-b">
                      <span class="text-sm text-gray-500">Nombre completo</span>
                      <span class="font-semibold text-gray-900">
                        {{ agente.nombre }} {{ agente.apellido }}
                      </span>
                    </div>
                    <div class="flex justify-between items-center pb-3 border-b">
                      <span class="text-sm text-gray-500">DNI</span>
                      <span class="font-semibold text-gray-900 bg-gray-100 px-3 py-1 rounded-lg">
                        {{ agente.dni || 'N/A' }}
                      </span>
                    </div>
                    <div class="flex justify-between items-center pb-3 border-b">
                      <span class="text-sm text-gray-500">Email</span>
                      <span class="font-semibold text-gray-900 text-sm">
                        {{ agente.email || 'N/A' }}
                      </span>
                    </div>
                    <div class="flex justify-between items-center pb-3 border-b">
                      <span class="text-sm text-gray-500">Teléfono</span>
                      <span class="font-semibold text-gray-900">
                        {{ agente.telefono || 'N/A' }}
                      </span>
                    </div>
                    <div class="flex justify-between items-center pb-3 border-b">
                      <span class="text-sm text-gray-500">Fecha de ingreso</span>
                      <span class="font-semibold text-gray-900">
                        {{ formatDate(agente.fechaIngreso) }}
                      </span>
                    </div>
                    <div class="flex justify-between items-center">
                      <span class="text-sm text-gray-500">Rol</span>
                      <span class="px-3 py-1 bg-green-100 text-green-800 rounded-full text-xs font-medium capitalize">
                        {{ agente.rol || 'Agente' }}
                      </span>
                    </div>
                  </div>
                </div>

                <!-- Estadísticas Profesionales -->
                <div class="bg-white rounded-xl shadow-md p-6">
                  <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                    <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M2 11a1 1 0 011-1h2a1 1 0 011 1v5a1 1 0 01-1 1H3a1 1 0 01-1-1v-5zM8 7a1 1 0 011-1h2a1 1 0 011 1v9a1 1 0 01-1 1H9a1 1 0 01-1-1V7zM14 4a1 1 0 011-1h2a1 1 0 011 1v12a1 1 0 01-1 1h-2a1 1 0 01-1-1V4z"/>
                    </svg>
                    Estadísticas Profesionales
                  </h4>
                  <div class="space-y-4">
                    <div class="bg-linear-to-r from-green-50 to-green-100 p-4 rounded-lg">
                      <span class="text-xs text-green-700 font-medium block mb-1">Especialidad</span>
                      <p class="text-lg font-bold text-green-900">{{ agente.especialidad || 'w' }}</p>
                    </div>
                    <div class="bg-linear-to-r from-blue-50 to-blue-100 p-4 rounded-lg">
                      <span class="text-xs text-blue-700 font-medium block mb-1">Experiencia</span>
                      <p class="text-lg font-bold text-blue-900">{{ agente.experiencia || 'N/A' }}</p>
                    </div>
                    <div class="grid grid-cols-2 gap-4">
                      <div class="bg-gray-50 p-4 rounded-lg text-center">
                        <svg class="w-8 h-8 mx-auto mb-2 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                          <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z"/>
                        </svg>
                        <p class="text-2xl font-bold text-gray-900">{{ agente.propiedadesVendidas || "0" }}</p>
                        <p class="text-xs text-gray-500 mt-1">Propiedades</p>
                      </div>
                      <div class="bg-gray-50 p-4 rounded-lg text-center">
                        <svg class="w-8 h-8 mx-auto mb-2 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                          <path d="M8.433 7.418c.155-.103.346-.196.567-.267v1.698a2.305 2.305 0 01-.567-.267C8.07 8.34 8 8.114 8 8c0-.114.07-.34.433-.582zM11 12.849v-1.698c.22.071.412.164.567.267.364.243.433.468.433.582 0 .114-.07.34-.433.582a2.305 2.305 0 01-.567.267z"/>
                          <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-13a1 1 0 10-2 0v.092a4.535 4.535 0 00-1.676.662C6.602 6.234 6 7.009 6 8c0 .99.602 1.765 1.324 2.246.48.32 1.054.545 1.676.662v1.941c-.391-.127-.68-.317-.843-.504a1 1 0 10-1.51 1.31c.562.649 1.413 1.076 2.353 1.253V15a1 1 0 102 0v-.092a4.535 4.535 0 001.676-.662C13.398 13.766 14 12.991 14 12c0-.99-.602-1.765-1.324-2.246A4.535 4.535 0 0011 9.092V7.151c.391.127.68.317.843.504a1 1 0 101.511-1.31c-.563-.649-1.413-1.076-2.354-1.253V5z" clip-rule="evenodd"/>
                        </svg>
                        <p class="text-lg font-bold text-gray-900">{{ agente.valorVendidoTotal || "-" }}</p>
                        <p class="text-xs text-gray-500 mt-1">Valor Total</p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Certificaciones y Clientes -->
              <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <!-- Certificaciones -->
                <div class="bg-white rounded-xl shadow-md p-6">
                  <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                    <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M6.267 3.455a3.066 3.066 0 001.745-.723 3.066 3.066 0 013.976 0 3.066 3.066 0 001.745.723 3.066 3.066 0 012.812 2.812c.051.643.304 1.254.723 1.745a3.066 3.066 0 010 3.976 3.066 3.066 0 00-.723 1.745 3.066 3.066 0 01-2.812 2.812 3.066 3.066 0 00-1.745.723 3.066 3.066 0 01-3.976 0 3.066 3.066 0 00-1.745-.723 3.066 3.066 0 01-2.812-2.812 3.066 3.066 0 00-.723-1.745 3.066 3.066 0 010-3.976 3.066 3.066 0 00.723-1.745 3.066 3.066 0 012.812-2.812zm7.44 5.252a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                    </svg>
                    Certificaciones
                  </h4>
                  <div
                    v-if="agente.certificaciones && agente.certificaciones.length > 0"
                    class="space-y-2"
                  >
                    <div
                      v-for="(cert, index) in agente.certificaciones"
                      :key="index"
                      class="flex items-start gap-2 p-3 bg-green-50 rounded-lg"
                    >
                      <svg class="w-5 h-5 text-green-600 mt-0.5 shrink-0" fill="currentColor" viewBox="0 0 20 20">
                        <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                      </svg>
                      <span class="text-sm text-gray-700">{{ cert }}</span>
                    </div>
                  </div>
                  <p v-else class="text-gray-400 text-center py-8">
                    No tiene certificaciones registradas
                  </p>
                </div>

                <!-- Clientes Atendidos -->
                <div class="bg-white rounded-xl shadow-md p-6">
                  <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                    <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M9 6a3 3 0 11-6 0 3 3 0 016 0zM17 6a3 3 0 11-6 0 3 3 0 016 0zM12.93 17c.046-.327.07-.66.07-1a6.97 6.97 0 00-1.5-4.33A5 5 0 0119 16v1h-6.07zM6 11a5 5 0 015 5v1H1v-1a5 5 0 015-5z"/>
                    </svg>
                    Clientes Atendidos
                  </h4>
                  <div
                    v-if="agente.clientesAtendidos && agente.clientesAtendidos.length > 0"
                    class="space-y-3 max-h-64 overflow-y-auto"
                  >
                    <div
                      v-for="cliente in agente.clientesAtendidos"
                      :key="cliente.id"
                      class="p-4 border-l-4 border-green-500 bg-gray-50 rounded-lg hover:shadow-md transition-shadow"
                    >
                      <p class="font-semibold text-gray-900 mb-1">
                        {{ cliente.nombre }}
                      </p>
                      <span class="inline-block px-2 py-1 bg-blue-100 text-blue-800 text-xs rounded-full mb-2">
                        {{ cliente.tipo }}
                      </span>
                      <p class="text-sm text-gray-600">
                        <span class="font-medium">Propiedades:</span>
                        {{ cliente.propiedades && cliente.propiedades.length > 0 ? cliente.propiedades.join(", ") : "Ninguna" }}
                      </p>
                    </div>
                  </div>
                  <p v-else class="text-gray-400 text-center py-8">
                    No hay clientes atendidos registrados
                  </p>
                </div>
              </div>

              <!-- Notas -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"/>
                  </svg>
                  Notas Adicionales
                </h4>
                <p class="text-gray-700 leading-relaxed">
                  {{ agente.notas || "Sin notas adicionales" }}
                </p>
              </div>

              <!-- Cambiar contraseña -->
              <div class="bg-white rounded-xl shadow-md p-6 mt-6">
                <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                  <svg class="w-6 h-6 text-yellow-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M10 2a4 4 0 00-4 4v2H5a2 2 0 00-2 2v6a2 2 0 002 2h10a2 2 0 002-2v-6a2 2 0 00-2-2h-1V6a4 4 0 00-4-4zm-2 4a2 2 0 114 0v2h-4V6zm8 4v6H4v-6h12z"/>
                  </svg>
                  Cambiar contraseña
                </h4>
                <form @submit.prevent="handleChangePassword">
                  <div class="mb-4">
                    <label class="block text-sm font-medium text-gray-700 mb-1">Nueva contraseña</label>
                    <input v-model="newPassword" type="password" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-green-500" required />
                  </div>
                  <div class="mb-4">
                    <label class="block text-sm font-medium text-gray-700 mb-1">Confirmar contraseña</label>
                    <input v-model="confirmPassword" type="password" class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-green-500" required />
                  </div>
                  <button type="submit" class="px-4 py-2 bg-green-700 text-white rounded hover:bg-green-800 disabled:opacity-50" :disabled="loadingPassword">
                    {{ loadingPassword ? 'Cambiando...' : 'Cambiar contraseña' }}
                  </button>
                </form>
              </div>
            </div>
          </div>

          <!-- Footer mejorado -->
          <div class="bg-white border-t p-6 flex justify-end gap-3 shrink-0">
            <button
              type="button"
              @click="closeModal"
              class="px-6 py-3 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 rounded-xl text-sm font-medium transition-all"
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import Swal from 'sweetalert2'
import userService from '@/services/userService'
const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true
  },
  agente: {
    type: Object,
    default: () => ({
      id: '',
      nombre: "",
      apellido: "",
      dni: "",
      telefono: "",
      email: "",
      especialidad: "",
      experiencia: "",
      propiedadesVendidas: 0,
      valorVendidoTotal: "",
      fechaIngreso: "",
      rol: "",
      certificaciones: [],
      clientesAtendidos: [],
      notas: ""
    })
  }
});

const emit = defineEmits(["close", "password-changed"]);

function closeModal() {
  emit('close');
}

// === Formatear fecha ===
function formatDate(dateString) {
  if (!dateString) return "Fecha inválida";

  const date = new Date(dateString);
  if (isNaN(date.getTime())) return "Fecha inválida";

  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Mes empieza en 0
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
}

// === Cambiar contraseña ===
const newPassword = ref('')
const confirmPassword = ref('')
const loadingPassword = ref(false)

async function handleChangePassword() {
  if (!props.agente || !props.agente.id) {
    Swal.fire('Error', 'No se encontró el ID del agente', 'error')
    return
  }
  if (newPassword.value !== confirmPassword.value) {
    Swal.fire('Error', 'Las contraseñas no coinciden', 'error')
    return
  }
  if (!newPassword.value) {
    Swal.fire('Error', 'La nueva contraseña no puede estar vacía', 'error')
    return
  }
  loadingPassword.value = true
  try {
    await userService.patchPassword(props.agente.id, newPassword.value, confirmPassword.value)
    Swal.fire('Éxito', 'Contraseña cambiada correctamente', 'success')
    // Emitir evento para que el padre pueda recargar datos si es necesario
    emit('password-changed', props.agente.id)
    newPassword.value = ''
    confirmPassword.value = ''
    // Cerrar el modal automáticamente
    emit('close')
  } catch (e) {
    Swal.fire('Error', e.message || 'No se pudo cambiar la contraseña', 'error')
  } finally {
    loadingPassword.value = false
  }
}

watch(
  () => props.agente,
  (nuevo) => {
    if (nuevo && nuevo.id) {
      console.log('DetalleAgente id:', nuevo.id)
    } else {
      console.warn('DetalleAgente: agente o id no definido', nuevo)
    }
  },
  { immediate: true }
)
</script>

<style scoped>
.modal-overlay {
  backdrop-filter: blur(2px);
}
</style>