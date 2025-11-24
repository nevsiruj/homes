<template>
  <div class="h-screen w-screen overflow-hidden">
    <div class="flex h-full w-full">
      <!-- Columna izquierda con imagen -->
      <div class="hidden lg:w-1/2 h-full bg-gray-200 lg:flex items-center justify-center">
        <img 
          src="https://app-pool.vylaris.online/dcmigserver/homes/9e7e0abd-77e8-49b9-a903-5abaf498bd5a.webp" 
          alt="Imagen descriptiva" 
          class="object-cover w-full h-full"
        >
      </div>
      
      <!-- Columna derecha con formulario -->
      <div class="lg:w-1/2 w-full h-full bg-white p-8 flex items-center justify-center">
        <form @submit.prevent="handleLogin" class="w-full max-w-md">
          <h2 class="text-2xl font-bold mb-6 text-gray-800">Bienvenido</h2>
          
          <div class="mb-4">
            <label for="email" class="block text-gray-700 mb-2">Email</label>
            <input 
              type="email"
              id="email"
              name="email"
              v-model="email"
              required
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500"
            >
          </div>
          
          <div class="mb-4">
            <label for="password" class="block text-gray-700 mb-2">Contraseña</label>
            <input 
              type="password"
              id="password"
              name="password"
              v-model="password"
              required
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500"
            >
          </div>
          
          <!-- Error Message -->
          <div v-if="errorMessage" class="text-red-500 text-sm mt-2">
            {{ errorMessage }}
          </div>
          
          <!-- Loading State -->
          <div v-if="loading" class="text-center my-4">
            <div class="inline-block animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-gray-600"></div>
          </div>
          
          <button 
            type="submit" 
            :disabled="loading"
            class="w-full bg-gray-600 text-white py-2 px-4 rounded-md hover:bg-gray-700 transition duration-200 disabled:opacity-50"
          >
            Iniciar sesión
          </button>
        </form>
      </div>
    </div>
  </div>
</template>


<script>
import { ref, onMounted } from "vue";                
import { useRouter, useRoute } from "vue-router";     
import { initFlowbite } from "flowbite";
import { useAuthStore } from "@/stores/auth";         

export default {
  setup() {
    const email = ref("");
    const password = ref("");
    const errorMessage = ref("");
    const loading = ref(false);

    const router = useRouter();
    const route = useRoute();
    const auth = useAuthStore();                     

    const handleLogin = async () => {
      errorMessage.value = "";
      loading.value = true;
      try {
        // Autentica y puebla el store con usuario+roles
        await auth.login(email.value, password.value);

        // Respeta redirect=? si te mandó el middleware; fallback a /admin
        const redirect = route.query.redirect || "/admin";
        await router.push(redirect);
      } catch (error) {
        console.error("Error en login:", error);
        
        // Detectar errores de red
        if (error.message && (
          error.message.includes('fetch') || 
          error.message.includes('network') ||
          error.message.includes('conexión') ||
          error.message.includes('tardó demasiado')
        )) {
          errorMessage.value = "Error de conexión. Verifica tu internet e intenta nuevamente.";
        } else if (error.status === 401) {
          errorMessage.value = "Credenciales incorrectas. Por favor verifica tu email y contraseña.";
        } else if (error.status === 500) {
          errorMessage.value = "Error del servidor. Por favor intenta más tarde.";
        } else if (error.message) {
          errorMessage.value = error.message;
        } else {
          errorMessage.value = "Error desconocido al intentar iniciar sesión.";
        }
      } finally {
        loading.value = false;
      }
    };

    onMounted(() => {
      initFlowbite();
    });

    return { email, password, errorMessage, loading, handleLogin };
  },
};
</script>
