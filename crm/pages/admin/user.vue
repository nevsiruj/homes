<template>
    <div class="grid grid-cols-2 grid-rows-1 gap-4 mb-8">
      <div>
        <form class="flex items-center max-w-lg mx-auto">
          <label for="voice-search" class="sr-only">Buscar</label>
          <div class="relative w-full">
            <input
              type="text"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2          "
              placeholder="Buscar"
              required
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
      <div class="flex justify-end items-end">
        <button
          type="button"
          @click.prevent="openModal"
          class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2    focus:outline-none "
        >
          Agregar usuario
        </button>
      </div>
    </div>
  
    <!-- Tabla -->
    <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table class="w-full text-sm text-left rtl:text-right text-gray-500 ">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50   ">
          <tr>
            <th scope="col" class="px-6 py-3">Id</th>
            <th scope="col" class="px-6 py-3">Usuario</th>
            <th scope="col" class="px-6 py-3">Email</th>
            <th scope="col" class="px-6 py-3">Role</th>
            <th scope="col" class="px-6 py-3">Accion</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="user in users"
            :key="user.id"
            class="odd:bg-white odd: even:bg-gray-50 even: border-b  border-gray-200"
          >
            <th scope="row" class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap  ">
              {{ user.id }}
            </th>
            <td class="px-6 py-4">{{ user.userName }}</td>
            <td class="px-6 py-4">{{ user.userEmail }}</td>
            <td class="px-6 py-4">{{ user.userRole }}</td>
            <td class="flex px-6 py-4">
              <button
                @click="editUser(user)"
                class="flex font-medium text-gray-600 hover:underline"
              >
                <svg
                  class="w-5 h-5 text-gray-800  "
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
                @click="deleteUser(user.id)"
                class="flex font-medium text-gray-600 hover:underline ml-4"
              >
                <svg
                  class="w-5 h-5 text-gray-800  "
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
                Eliminar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  
    <!-- Modal -->
    <ModalAgUser
      :isOpen="isModalOpen"
      :userData="selectedUser"
      @close="closeModal"
      @submit="handleSubmit"
    />
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue'
  // import userService from '~/services/userService'
  // import ModalAgUser from '../../components/modalAgUser.vue'
  
  definePageMeta({
    layout: 'admin',
  })
  
  const isModalOpen = ref(false)
  const users = ref([])
  const selectedUser = ref({})
  
  // Obtener usuarios al cargar el componente
  onMounted(async () => {
    await fetchUsers()
  })
  
  // Función para obtener usuarios
  async function fetchUsers() {
    try {
      const data = await userService.getUsers()
      users.value = data 
      console.log('Usuarios actualizados:', data) 
    } catch (error) {
      console.error('Error fetching users:', error)
    }
  }
  
  // Abrir modal para agregar o editar
  function openModal() {
    selectedUser.value = {} // Limpiar datos de edición
    isModalOpen.value = true
  }
  
  // Abrir modal para editar un usuario
  function editUser(user) {
    selectedUser.value = { ...user } // Llenar con datos del usuario
    isModalOpen.value = true
  }
  
  // Cerrar modal
  function closeModal() {
    isModalOpen.value = false
  }
  
  // Manejar el envío del formulario (agregar o editar)
  async function handleSubmit(formData) {
  try {
    if (formData.id) {
      // Editar usuario
      const updatedUser = await userService.updateUser(formData.id, formData)
      console.log('Usuario actualizado:', updatedUser) // Depuración

      const index = users.value.findIndex(user => user.id === formData.id)
      if (index !== -1) {
        users.value[index] = updatedUser // Actualiza el usuario en el array
      }
      
    } else {
      // Agregar usuario
      const newUser = await userService.createUser(formData)
      console.log('Usuario creado:', newUser) // Depuración
    }
    // Actualizar la lista de usuarios
    await fetchUsers()
  } catch (error) {
    console.error('Error:', error)
  }
}

  
  // Eliminar un usuario
  async function deleteUser(userId) {
    try {
      await userService.deleteUser(userId)
      // Actualizar la lista de usuarios
      await fetchUsers()
    } catch (error) {
      console.error('Error deleting user:', error)
    }
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