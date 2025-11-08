<template>
    <div v-if="isOpen" class="fixed inset-0 bg-black/30 flex items-center justify-center z-50">
      <div class="bg-white rounded-lg shadow-xl w-full max-w-md p-6 ">
        <div class="flex justify-between items-center mb-4">
          <h3 class="text-xl font-bold text-gray-900  ">
            Editar Cliente
          </h3>
          <button @click="$emit('close')" class="text-gray-400 hover:text-gray-500">
            ✕
          </button>
        </div>
  
        <form @submit.prevent="submitForm">
          <div class="grid grid-cols-1 gap-4 mb-4">
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900  ">DNI</label>
              <input
                v-model="formData.dni"
                type="text"
                class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5     "
                disabled
              />
            </div>
  
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900  ">Nombre *</label>
                <input
                  v-model="formData.nombre"
                  type="text"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5          "
                  required
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900  ">Apellido *</label>
                <input
                  v-model="formData.apellido"
                  type="text"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5          "
                  required
                />
              </div>
            </div>
  
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900  ">Email *</label>
              <input
                v-model="formData.email"
                type="email"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5          "
                required
              />
            </div>
  
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900  ">Teléfono *</label>
              <input
                v-model="formData.telefono"
                type="tel"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5          "
                required
              />
            </div>
  
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900  ">Dirección</label>
              <textarea
                v-model="formData.direccion"
                rows="2"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5          "
              ></textarea>
            </div>
          </div>
  
          <div class="flex justify-end space-x-3">
            <button
              type="button"
              @click="$emit('close')"
              class="text-white bg-gray-600 hover:bg-red-700 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center    "
            >
              Cancelar
            </button>
            <button
              type="submit"
              class="text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center "
            >
              Actualizar
            </button>
          </div>
        </form>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, watch } from 'vue';
  import Swal from 'sweetalert2';
  import clienteService from '../../services/clienteService';
  
  const props = defineProps({
    isOpen: Boolean,
    cliente: {
      type: Object,
      required: true
    }
  });
  
  const emit = defineEmits(['close', 'updated']);
  
  const formData = ref({ ...props.cliente });
  
  // Actualizar cuando cambia el prop cliente
  watch(() => props.cliente, (newVal) => {
    formData.value = { ...newVal };
  }, { immediate: true });
  
  const submitForm = async () => {
    try {
      await clienteService.updateCliente(formData.value.id, formData.value);
      
      Swal.fire({
        icon: 'success',
        title: '¡Actualizado!',
        text: 'El cliente se ha modificado correctamente',
        timer: 2000
      });
      
      emit('updated');
      emit('close');
    } catch (error) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: error.message || 'Ocurrió un error al actualizar el cliente'
      });
    }
  };
  </script>