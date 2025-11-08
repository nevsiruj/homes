<template>
  <div v-if="isOpen" id="agregar-agente-modal" tabindex="-1" aria-hidden="true"
       class="overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 flex justify-center items-center w-full h-[calc(100%-1rem)] max-h-full bg-gray-500/50">
    <div class="relative p-4 w-full max-w-2xl max-h-full">
      <div class="relative bg-white rounded-lg shadow  ">
        <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t  ">
          <h3 class="text-xl font-semibold text-gray-900  ">
            Agregar Nuevo Agente
          </h3>
          <button @click="closeModal" type="button"
                  class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ml-auto inline-flex justify-center items-center    ">
            <svg class="w-3 h-3" fill="currentColor" viewBox="0 0 20 20"
                 xmlns="http://www.w3.org/2000/svg">
              <path fill-rule="evenodd"
                    d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 0 010-1.414z"
                    clip-rule="evenodd"></path>
            </svg>
          </button>
        </div>

        <form @submit.prevent="submitForm" class="p-4 md:p-5 space-y-4">
          <div class="grid gap-4 mb-4 sm:grid-cols-2">
            <div>
              <label for="nombre" class="block mb-2 text-sm font-medium text-gray-900  ">Nombre</label>
              <input v-model="formData.nombre" type="text" id="nombre" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
            </div>

            <div>
              <label for="apellido" class="block mb-2 text-sm font-medium text-gray-900  ">Apellido</label>
              <input v-model="formData.apellido" type="text" id="apellido" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
            </div>
          </div>

          <div class="grid gap-4 mb-4 sm:grid-cols-2">
            <div>
              <label for="dni" class="block mb-2 text-sm font-medium text-gray-900  ">DNI</label>
              <input v-model="formData.dni" @blur="checkDuplicate('dni')" type="text" id="dni" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
              <p v-if="fieldErrors.dni" class="mt-1 text-xs text-red-600">{{ fieldErrors.dni }}</p>
            </div>

            <div>
              <label for="email" class="block mb-2 text-sm font-medium text-gray-900  ">Email</label>
              <input v-model="formData.email" @blur="checkDuplicate('email')" type="email" id="email" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
              <p v-if="fieldErrors.email" class="mt-1 text-xs text-red-600">{{ fieldErrors.email }}</p>
            </div>
          </div>

          <div class="grid gap-4 mb-4 sm:grid-cols-2">
            <div>
              <label for="userName" class="block mb-2 text-sm font-medium text-gray-900  ">Nombre de usuario</label>
              <input v-model="formData.userName" @blur="checkDuplicate('userName')" type="text" id="userName" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
              <p v-if="fieldErrors.userName" class="mt-1 text-xs text-red-600">{{ fieldErrors.userName }}</p>
            </div>

            <div>
              <label for="especialidad" class="block mb-2 text-sm font-medium text-gray-900  ">Especialidad</label>
              <input v-model="formData.especialidad" type="text" id="especialidad" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
            </div>
          </div>

          <div class="grid gap-4 mb-4 sm:grid-cols-2">
            <div>
              <label for="rol" class="block mb-2 text-sm font-medium text-gray-900  ">Rol</label>
              <select v-model="formData.rol" id="rol"
                      class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5      ">
                <option value="" disabled selected>Selecciona un rol</option>
                <option value="Admin">Administrador</option>
                <option value="Agente">Agente</option>
              </select>
            </div>

            <div>
              <label for="fechaIngreso" class="block mb-2 text-sm font-medium text-gray-900  ">Fecha de Ingreso</label>
              <input v-model="formData.fechaIngreso" type="date" id="fechaIngreso"
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
            </div>
          </div>

          <div class="grid gap-4 mb-4 sm:grid-cols-2">
            <div>
              <label for="password" class="block mb-2 text-sm font-medium text-gray-900  ">Contraseña</label>
              <input v-model="formData.password" type="password" id="password" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />

              <ul v-if="passwordErrors.length > 0" class="mt-1 text-xs text-red-600 list-disc pl-5">
                <li v-for="(error, index) in passwordErrors" :key="index">{{ error }}</li>
              </ul>
            </div>

            <div>
              <label for="confirmPassword" class="block mb-2 text-sm font-medium text-gray-900  ">Confirmar Contraseña</label>
              <input v-model="formData.confirmPassword" type="password" id="confirmPassword" required
                     class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5        " />
            </div>
          </div>

          <div class="flex justify-end">
            <button @click="closeModal" type="button"
                    class="py-2 px-4 mr-2 bg-gray-300 hover:bg-gray-400 text-gray-800 font-semibold rounded-lg transition-colors">
              Cancelar
            </button>
            <button type="submit"
                    class="py-2 px-4 bg-gray-600 hover:bg-gray-700 text-white font-semibold rounded-lg transition-colors">
              Guardar
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import Swal from 'sweetalert2';
import agenteService from '@/services/agenteService';

// === Props ===
const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true
  }
});

// === Emits ===
const emit = defineEmits(['close', 'agente-agregado']);

// === Datos reactivos ===
const formData = ref({
  nombre: '',
  apellido: '',
  dni: '',
  email: '',
  userName: '',
  especialidad: '',
  fechaIngreso: new Date().toISOString().split('T')[0],
  rol: '',
  password: '',
  confirmPassword: ''
});

const passwordErrors = ref([]);
const fieldErrors = ref({
  dni: '',
  email: '',
  userName: ''
});
const existingUsers = ref([]);
const isLoading = ref(false);

// Cargar usuarios existentes cuando se abre el modal
watch(() => props.isOpen, async (isOpen) => {
  if (isOpen) {
    try {
      isLoading.value = true;
      const response = await agenteService.getUsers();
      existingUsers.value = response.$values || [];
      // //console.log("Usuarios existentes cargados:", existingUsers.value); // Para depuración
    } catch (error) {
      //console.error("Error al obtener usuarios:", error);
      existingUsers.value = [];
      Swal.fire("Error", "No se pudieron cargar los usuarios existentes", "error");
    } finally {
      isLoading.value = false;
    }
  }
});

// Validación de contraseña
function validatePassword(password) {
  const errors = [];
  if (password.length < 8) errors.push("Debe tener al menos 8 caracteres.");
  if (!/[A-Z]/.test(password)) errors.push("Debe contener al menos una mayúscula.");
  if (!/[0-9]/.test(password)) errors.push("Debe contener al menos un número.");
  if (!/[^a-zA-Z0-9]/.test(password)) errors.push("Debe contener al menos un carácter especial.");
  return errors;
}

// Verificar duplicados en un campo específico
function checkDuplicate(field) {
  if (!Array.isArray(existingUsers.value) || existingUsers.value.length === 0) {
      // Si no hay usuarios cargados, no podemos validar duplicados localmente.
      // Limpiamos el error si existía.
      fieldErrors.value[field] = '';
      return;
  }

  const value = formData.value[field]?.trim();
  if (!value) {
    fieldErrors.value[field] = '';
    return;
  }

  const fieldMap = {
    dni: 'dni',
    email: 'email',
    userName: 'userName'
  };

  if (!fieldMap[field]) {
    //console.warn(`Campo '${field}' no definido en fieldMap para la verificación de duplicados.`);
    fieldErrors.value[field] = ''; // Asegurar que no quede un error viejo
    return;
  }

  const userExists = existingUsers.value.some(user => {
    const userValue = user[fieldMap[field]];
    if (!userValue) return false;
    // Comparación case-insensitive para email y userName
    return (field === 'userName' || field === 'email')
      ? userValue.toLowerCase() === value.toLowerCase()
      : userValue === value;
  });

  // Establece el mensaje de error para el campo específico
  fieldErrors.value[field] = userExists
    ? `Este ${field === 'dni' ? 'DNI' : field === 'email' ? 'email' : 'nombre de usuario'} ya está registrado.`
    : '';
}

// Manejo de errores del backend
function handleBackendErrors(error) {
  // //console.log("Entrando a handleBackendErrors. Objeto error:", error);
  const backendErrors = error.response?.data?.errors?.$values;

  let hasSpecificErrors = false;
  let errorMessage = "<ul>";

  if (backendErrors && Array.isArray(backendErrors)) {
    // //console.log("Errores de backend encontrados:", backendErrors);
    backendErrors.forEach(err => {
      // //console.log("Procesando error individual:", err);

      if (err.code === 'DuplicateUserName') {
        fieldErrors.value.userName = err.description; // Establece el mensaje para el campo
        errorMessage += `<li>${err.description}</li>`;
        hasSpecificErrors = true;
      }
      // Asegúrate de incluir aquí otros errores de duplicado del backend para DNI o Email
      // Si tu backend los envía con un código diferente, como 'DuplicateEmail' o 'DuplicateDNI'
      if (err.code === 'DuplicateEmail') {
         fieldErrors.value.email = err.description;
         errorMessage += `<li>${err.description}</li>`;
         hasSpecificErrors = true;
      }
      if (err.code === 'DuplicateDNI') {
         fieldErrors.value.dni = err.description;
         errorMessage += `<li>${err.description}</li>`;
         hasSpecificErrors = true;
      }
      // Puedes añadir más lógica aquí para otros tipos de errores de backend si los hay
    });

    if (hasSpecificErrors) {
      errorMessage += "</ul>";
      Swal.fire({
        icon: "error",
        title: "Error de registro",
        html: errorMessage,
      });
      return true;
    }
  }
  return false;
}

// === Enviar formulario ===
async function submitForm() {
  // 1. Reiniciar TODOS los errores al inicio del submit para una validación limpia
  passwordErrors.value = [];
  Object.keys(fieldErrors.value).forEach(key => fieldErrors.value[key] = '');

  // 2. Validaciones frontales básicas (campos vacíos, email válido)
  if (!formData.value.nombre.trim()) {
    Swal.fire("Error", "El nombre es obligatorio.", "error");
    return;
  }
  if (!formData.value.apellido.trim()) {
    Swal.fire("Error", "El apellido es obligatorio.", "error");
    return;
  }
  if (!formData.value.email.includes('@')) {
    Swal.fire("Error", "Ingresa un correo válido.", "error");
    return;
  }
  if (!formData.value.password || !formData.value.confirmPassword) {
    Swal.fire("Error", "La contraseña y su confirmación son obligatorias.", "error");
    return;
  }
  if (formData.value.password !== formData.value.confirmPassword) {
    Swal.fire("Error", "Las contraseñas no coinciden.", "error");
    return;
  }

  // 3. Validación de formato de contraseña
  const pwdErrors = validatePassword(formData.value.password);
  if (pwdErrors.length > 0) {
    passwordErrors.value = pwdErrors; // Para mostrar debajo del campo
    Swal.fire({
      icon: "error",
      title: "Error de Contraseña",
      html: `<ul>${pwdErrors.map(err => `<li>${err}</li>`).join('')}</ul>`, // Para el SweetAlert
    });
    return;
  }

  // 4. Validación de duplicados en el frontend (DNI, Email, UserName)
  // Esta llamada poblará fieldErrors.value si encuentra duplicados localmente.
  const hasFrontendDuplicates = checkForDuplicates();
  if (hasFrontendDuplicates) {
    Swal.fire("Error", "Por favor corrige los campos marcados que ya están registrados.", "error");
    return;
  }

  try {
    const response = await agenteService.register({
      ...formData.value,
      // Si tu backend NO espera 'confirmPassword', puedes descomentar la siguiente línea
      // confirmPassword: undefined
    });

    Swal.fire("Éxito", "Agente registrado correctamente.", "success");
    emit("agente-agregado", response);
    closeModal();
  } catch (error) {
    //console.error("Error al registrar agente (catch):", error);

    // Intentamos manejar el error del backend.
    // handleBackendErrors actualizará fieldErrors.value si hay duplicados del backend.
    const handledByBackend = handleBackendErrors(error);

    if (!handledByBackend) {
      // Si handleBackendErrors no manejó el error (no era un duplicado o error específico)
      let errorMessage = "No se pudo registrar el agente. Inténtalo de nuevo más tarde.";

      if (error.response?.data) {
        if (typeof error.response.data === 'string') {
            errorMessage = error.response.data;
        } else if (error.response.data.message) {
            errorMessage = error.response.data.message;
        } else if (error.message) {
            errorMessage = error.message;
        }
      }
      Swal.fire("Error", errorMessage, "error");
    }
    // No necesitamos una llamada extra a checkForDuplicates() aquí,
    // porque handleBackendErrors ya actualiza fieldErrors.value.userName.
    // Y Vue reacciona a ese cambio para mostrar el p tag.
  }
}

// Verificar todos los duplicados (para validación frontend)
function checkForDuplicates() {
  checkDuplicate('dni');
  checkDuplicate('email');
  checkDuplicate('userName');
  return Object.values(fieldErrors.value).some(error => error !== '');
}

// === Cerrar modal ===
function closeModal() {
  resetForm();
  emit("close");
}

function resetForm() {
  formData.value = {
    nombre: "",
    apellido: "",
    dni: "",
    email: "",
    userName: "",
    especialidad: "",
    fechaIngreso: new Date().toISOString().split('T')[0],
    rol: "",
    password: "",
    confirmPassword: ""
  };
  passwordErrors.value = [];
  fieldErrors.value = {
    dni: "",
    email: "",
    userName: ""
  };
}
</script>

<style scoped>
/* Tus estilos existentes aquí */
.text-red-600 {
  color: #dc2626;
}
.list-disc {
  list-style-type: disc;
}
.pl-5 {
  padding-left: 1.25rem;
}
.mt-1 {
  margin-top: 0.25rem;
}
.text-xs {
  font-size: 0.75rem;
}
</style>