<template>
  <div v-if="isOpen" class="modal-overlay fixed inset-0 bg-black bg-opacity-60 z-50 flex items-center justify-center p-4 backdrop-blur-sm animate-fade-in">
    <div class="flex items-center justify-center w-full h-full">
      <div class="fixed inset-0 bg-gray-900/50 transition-opacity" aria-hidden="true" @click="closeModal"></div>
      <div class="relative bg-white rounded-2xl shadow-2xl transform transition-all overflow-hidden" style="width: 90%; max-width: 400px; max-height: 90vh;">
        <div class="flex flex-col h-full">
          <div class="bg-gradient-to-r from-green-700 to-green-500 text-white p-6 flex-shrink-0 flex items-center gap-3">
            <svg class="w-8 h-8 text-yellow-300" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" d="M12 11c1.104 0 2-.896 2-2s-.896-2-2-2-2 .896-2 2 .896 2 2 2zm0 2c-2.21 0-4 1.79-4 4v1h8v-1c0-2.21-1.79-4-4-4z" />
            </svg>
            <h3 class="text-2xl font-bold mb-0">Cambiar contraseña</h3>
          </div>
          <div class="flex-1 overflow-y-auto p-6 bg-gray-50">
            <form @submit.prevent="handleChangePassword">
              <div class="mb-4">
                <label class="block text-sm font-semibold text-gray-700 mb-1">Nueva contraseña</label>
                <input v-model="newPassword" type="password" class="w-full px-3 py-2 border border-green-300 rounded-md focus:outline-none focus:ring-2 focus:ring-green-500 font-mono text-lg" required />
              </div>
              <div class="mb-4">
                <label class="block text-sm font-semibold text-gray-700 mb-1">Confirmar contraseña</label>
                <input v-model="confirmPassword" type="password" class="w-full px-3 py-2 border border-green-300 rounded-md focus:outline-none focus:ring-2 focus:ring-green-500 font-mono text-lg" required />
              </div>
              <button type="submit" class="w-full px-4 py-2 bg-green-700 text-white rounded-xl hover:bg-green-800 disabled:opacity-50 font-bold text-lg transition-all duration-150" :disabled="loadingPassword">
                {{ loadingPassword ? 'Cambiando...' : 'Cambiar contraseña' }}
              </button>
            </form>
          </div>
          <div class="bg-white border-t p-6 flex justify-end gap-3 flex-shrink-0">
            <button type="button" @click="closeModal" class="px-6 py-3 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-xl text-sm font-medium transition-all">Cerrar</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  backdrop-filter: blur(2px);
}
.animate-fade-in {
  animation: fadeIn 0.3s ease;
}
@keyframes fadeIn {
  from { opacity: 0; transform: scale(0.95); }
  to { opacity: 1; transform: scale(1); }
}
</style>

<script setup>
import { ref, watch } from 'vue';
import Swal from 'sweetalert2';
import userService from '@/services/userService';
const props = defineProps({
  isOpen: Boolean,
  userId: String
});
const emit = defineEmits(['close']);
const newPassword = ref('');
const confirmPassword = ref('');
const loadingPassword = ref(false);

function closeModal() {
  emit('close');
}

async function handleChangePassword() {
  if (!props.userId) {
    Swal.fire('Error', 'No se encontró el ID del usuario', 'error');
    return;
  }
  if (newPassword.value !== confirmPassword.value) {
    Swal.fire('Error', 'Las contraseñas no coinciden', 'error');
    return;
  }
  if (!newPassword.value) {
    Swal.fire('Error', 'La nueva contraseña no puede estar vacía', 'error');
    return;
  }
  loadingPassword.value = true;
  try {
    await userService.patchPassword(props.userId, newPassword.value, confirmPassword.value);
    Swal.fire('Éxito', 'Contraseña cambiada correctamente', 'success');
    newPassword.value = '';
    confirmPassword.value = '';
    closeModal();
  } catch (e) {
    Swal.fire('Error', e.message || 'No se pudo cambiar la contraseña', 'error');
  } finally {
    loadingPassword.value = false;
  }
}
</script>
