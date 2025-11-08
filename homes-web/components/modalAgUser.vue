<template>
  <div v-if="isOpen" class="modal-overlay">
    <div class="relative p-4 w-full max-w-md max-h-full">
      <!-- Modal content -->
      <div class="relative bg-white rounded-lg shadow-sm dark:bg-black">
        <!-- Modal header -->
        <div
          class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600 border-gray-200"
        >
          <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
            {{ isEditMode ? 'Editar Usuario' : 'Agregar Usuario' }}
          </h3>
          <button
            type="button"
            @click="closeModal"
            class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
          >
            <svg
              class="w-3 h-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 14"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
              />
            </svg>
            <span class="sr-only">Cerrar modal</span>
          </button>
        </div>
        <!-- Modal body -->
        <form @submit.prevent="handleSubmit" class="p-4 md:p-5">
          <div class="grid gap-4 mb-4 grid-cols-2">
            <div class="col-span-2">
              <label
                for="UserName"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Usuario</label
              >
              <input
                type="text"
                v-model="form.userName"
                id="UserName"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Usuario"
                required
              />
            </div>
            <div class="col-span-2 sm:col-span-1">
              <label
                for="UserEmail"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Email</label
              >
              <input
                type="email"
                v-model="form.userEmail"
                id="UserEmail"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Email"
                required
              />
            </div>
            <div class="col-span-2 sm:col-span-1">
              <label
                for="UserPss"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Contraseña</label
              >
              <input
                type="password"
                v-model="form.userPss"
                id="UserPss"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Contraseña"
                :required="!isEditMode"
              />
            </div>
            <div class="col-span-2 sm:col-span-1">
              <label
                for="UserRole"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Role</label
              >
              <select
                v-model="form.userRole"
                id="UserRole"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
              >
                <option value="Admin">Administrador</option>
                <option value="User">Usuario</option>
              </select>
            </div>
          </div>
          <button
            type="submit"
            class="text-white inline-flex items-center bg-black hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-gray-600 dark:hover:bg-black dark:focus:ring-gray-800"
          >
            <svg
              class="me-1 -ms-1 w-5 h-5"
              fill="currentColor"
              viewBox="0 0 20 20"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                fill-rule="evenodd"
                d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z"
                clip-rule="evenodd"
              ></path>
            </svg>
            {{ isEditMode ? 'Guardar cambios' : 'Agregar' }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
  },
  userData: {
    type: Object,
    default: () => ({}),
  },
})

const emit = defineEmits(['close', 'submit'])

// Estado del formulario
const form = ref({
  userName: '',
  userEmail: '',
  userPss: '',
  userRole: 'User',
})

// Determinar si está en modo edición
const isEditMode = ref(false)

// Observar cambios en userData para llenar el formulario en modo edición
watch(
  () => props.userData,
  (newData) => {
    if (newData && Object.keys(newData).length > 0) {
      isEditMode.value = true
      form.value = { ...newData }
    } else {
      isEditMode.value = false
      resetForm()
    }
  },
  { immediate: true }
)

// Resetear el formulario
function resetForm() {
  form.value = {
    userName: '',
    userEmail: '',
    userPss: '',
    userRole: 'User',
  }
}

// Cerrar el modal
function closeModal() {
  emit('close')
  resetForm()
}

// Manejar el envío del formulario
function handleSubmit() {
  emit('submit', form.value)
  closeModal()
}
</script>

<style>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}
</style>