<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50">
    <div class="max-w-md w-full text-center px-4">
      <div class="mb-8">
        <h1 class="text-9xl font-bold text-gray-800">
          {{ error.statusCode }}
        </h1>
        <p class="text-2xl font-semibold text-gray-600 mt-4">
          {{ errorMessage }}
        </p>
        <p class="text-gray-500 mt-2">
          {{ errorDescription }}
        </p>
      </div>

      <div class="space-y-4">
        <NuxtLink
          to="/"
          class="inline-block bg-black text-white px-8 py-3 rounded-lg hover:bg-gray-800 transition-colors duration-200"
        >
          Volver al Inicio
        </NuxtLink>

        <div class="mt-6">
          <p class="text-sm text-gray-500 mb-3">O explora nuestras secciones:</p>
          <div class="flex flex-wrap justify-center gap-3">
            <NuxtLink
              to="/propiedades"
              class="text-sm text-gray-600 hover:text-black underline"
            >
              Propiedades
            </NuxtLink>
            <NuxtLink
              to="/proyectos-inmobiliarios"
              class="text-sm text-gray-600 hover:text-black underline"
            >
              Proyectos
            </NuxtLink>
            <NuxtLink
              to="/nosotros"
              class="text-sm text-gray-600 hover:text-black underline"
            >
              Nosotros
            </NuxtLink>
            <NuxtLink
              to="/blog"
              class="text-sm text-gray-600 hover:text-black underline"
            >
              Blog
            </NuxtLink>
          </div>
        </div>
      </div>

      <!-- SEO: Asegurar que Google entienda que es un error -->
      <div v-if="error.statusCode === 404" class="hidden">
        <h2>Página no encontrada - Error 404</h2>
        <p>La página que buscas no existe en Homes Guatemala</p>
      </div>
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  error: {
    type: Object,
    required: true
  }
});

// Mensajes personalizados según el código de error
const errorMessage = computed(() => {
  switch (props.error.statusCode) {
    case 404:
      return 'Página no encontrada';
    case 500:
      return 'Error del servidor';
    case 403:
      return 'Acceso denegado';
    default:
      return 'Algo salió mal';
  }
});

const errorDescription = computed(() => {
  switch (props.error.statusCode) {
    case 404:
      return 'Lo sentimos, la página que buscas no existe o ha sido movida.';
    case 500:
      return 'Estamos experimentando problemas técnicos. Por favor, intenta de nuevo más tarde.';
    case 403:
      return 'No tienes permiso para acceder a esta página.';
    default:
      return 'Ha ocurrido un error inesperado.';
  }
});

// SEO Meta para páginas de error
useSeoMeta({
  title: `Error ${props.error.statusCode} - Homes Guatemala`,
  description: errorDescription.value,
  robots: 'noindex, nofollow' // No indexar páginas de error
});

// Log del error (solo en desarrollo)
if (process.dev) {
  console.error('Error:', props.error);
}
</script>

<style scoped>
/* Estilos adicionales si son necesarios */
</style>
