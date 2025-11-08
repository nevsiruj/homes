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
            {{ isEditing ? "Editar cliente" : "Agregar cliente" }}
          </h3>
          <button
            type="button"
            @click="closeModal"
            class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
            data-modal-toggle="crud-modal"
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
            <span class="sr-only">Close modal</span>
          </button>
        </div>
        <!-- Modal body -->
        <form @submit.prevent="handleSubmit" class="p-4 md:p-5">
          <div class="grid gap-4 mb-4 grid-cols-2">
            <div class="col-span-2 sm:col-span-1">
              <label
                for="fecha"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Fecha de registro</label
              >
              <input
                type="date"
                v-model="form.clienteFecha"
                id="fecha"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                required
              />
            </div>
            <div class="col-span-2">
              <label
                for="name"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Nombre y apellido</label
              >
              <input
                type="text"
                v-model="form.clienteNombre"
                id="name"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Nombre y apellido"
                required
              />
            </div>
            <div class="col-span-2 sm:col-span-1">
              <label
                for="dni"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >DNI</label
              >
              <input
                type="text"
                v-model="form.clienteDni"
                id="dni"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="DNI"
                required
              />
            </div>
            <div class="col-span-2 sm:col-span-1">
              <label
                for="telefono"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Telefono</label
              >
              <input
                type="text"
                v-model="form.clienteTelefono"
                id="telefono"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Telefono"
                required
              />
            </div>
            <div class="col-span-2">
              <label
                for="direccion"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Direccion</label
              >
              <textarea
                v-model="form.clienteDireccion"
                id="direccion"
                rows="2"
                class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-gray-500 focus:border-gray-500 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                placeholder="Direccion"
              ></textarea>
            </div>
            <div class="col-span-2">
              <label
                for="observacion"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Observacion</label
              >
              <textarea
                v-model="form.clienteObservacion"
                id="observacion"
                rows="2"
                class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-gray-500 focus:border-gray-500 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-gray-500 dark:focus:border-gray-500"
                placeholder="Observacion"
                
              ></textarea>
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
            {{ isEditing ? "Guardar cambios" : "Agregar" }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue";
import clienteService from "../services/clienteServices.js";

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
  },
  clienteToEdit: {
    type: Object,
    default: null,
  },
});

const emit = defineEmits(["close", "cliente-added", "cliente-updated"]);

const form = ref({
  clienteFecha: "",
  clienteNombre: "",
  clienteDni: "",
  clienteTelefono: "",
  clienteDireccion: "",
  clienteObservacion: "",
});

const isEditing = ref(false);

watch(
  () => props.clienteToEdit,
  (newCliente) => {
    console.log('Cliente recibido en el modal:', newCliente); // Depuración
    if (!newCliente) {
      console.log('No se recibió un cliente válido'); // Depuración
      isEditing.value = false;
      resetForm();
      return;
    }

    isEditing.value = true;
    form.value = {
      ...newCliente,
      clienteFecha: newCliente.clienteFecha ? newCliente.clienteFecha.split('T')[0] : '', // Validación
    };
  },
  { immediate: true }
);

function closeModal() {
  emit("close");
}

// En el modal
async function handleSubmit() {
  try {
    if (isEditing.value) {
      // Modo edición
      await clienteService.updateCliente(props.clienteToEdit.id, form.value);
      emit("cliente-updated"); // Emitir evento para notificar que se ha actualizado un cliente
    } else {
      // Modo agregar
      await clienteService.createCliente(form.value);
      emit("cliente-added"); // Emitir evento para notificar que se ha agregado un cliente
    }
    closeModal(); // Cerrar el modal después de la operación
    resetForm(); // Limpiar el formulario
  } catch (error) {
    console.error("Error:", error);
  }
}

function resetForm() {
  form.value = {
    clienteFecha: "",
    clienteNombre: "",
    clienteDni: "",
    clienteTelefono: "",
    clienteDireccion: "",
    clienteObservacion: "",
  };
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
