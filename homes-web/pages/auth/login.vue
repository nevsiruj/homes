<template>
<div
    class="my-36 w-md mx-auto bg-white border border-gray-200 rounded-xl shadow-sm dark:bg-neutral-900 dark:border-neutral-700"
  >
    <div class="p-4 sm:p-7">
      <div class="text-center">           
        <h1 class="block text-2xl font-bold text-gray-800 dark:text-white">
          Login
        </h1>
      </div>

      <div class="mt-5">
        <!-- Form -->
        <form @submit="handleLogin">
          <div class="grid gap-y-4">
            <!-- Form Group -->
            <div>
              <label for="email" class="block text-sm mb-2 dark:text-white"
                >Email</label
              >
              <div class="relative">
                <input
                  type="email"
                  id="email"
                  name="email"
                  v-model="email"
                  class="py-3 px-4 block w-full border border-gray-300 rounded-lg text-sm"
                  required
                />
              </div>
            </div>
            <!-- End Form Group -->

            <!-- Form Group -->
            <div>
              <div class="flex justify-between items-center">
                <label for="password" class="block text-sm mb-2 dark:text-white"
                  >Password</label
                >
              </div>
              <div class="relative">
                <input
                  type="password"
                  id="password"
                  name="password"
                  v-model="password"
                  class="py-3 px-4 block w-full border border-gray-300 rounded-lg text-sm"
                  required
                />
              </div>
            </div>
            <!-- End Form Group -->

            <!-- Error Message -->
            <div v-if="errorMessage" class="text-red-500 text-sm mt-2">
              {{ errorMessage }}
            </div>

            <button
              type="submit"
              class="w-full py-3 px-4 inline-flex justify-center items-center gap-x-2 text-sm font-medium rounded-lg border border-transparent bg-gray-600 text-white hover:bg-black  disabled:pointer-events-none"
            >
              Ingresar
            </button>
          </div>
        </form>
        <!-- End Form -->
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { initFlowbite } from 'flowbite'

export default {
  setup() {
    const email = ref("");
    const password = ref("");
    const errorMessage = ref("");
    const router = useRouter();

    const handleLogin = (event) => {
      event.preventDefault(); 

      // Datos de ejemplo para la validación
      const validEmail = "prestame@gmail.com";
      const validPassword = "12345678";

      // Validar las credenciales
      if (email.value === validEmail && password.value === validPassword) {
        // Redirigir al usuario a la página de administración
        router.push("/admin"); // Asegúrate de que la ruta sea correcta
      } else {
        errorMessage.value = "Credenciales incorrectas. Inténtalo de nuevo.";
      }
    };

    onMounted(() => {
    initFlowbite();
})

    return {
      email,
      password,
      errorMessage,
      handleLogin,
    };
  },
};
</script>
