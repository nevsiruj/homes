<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50">
    <div class="text-center px-6">
      <h1 class="text-9xl font-bold text-gray-800">{{ error.statusCode }}</h1>
      <h2 class="text-3xl font-semibold text-gray-700 mt-4">
        {{ error.statusCode === 404 ? 'Página No Encontrada' : 'Error del Servidor' }}
      </h2>
      <p class="text-gray-600 mt-4 max-w-md mx-auto">
        {{ error.message || 'Lo sentimos, la página que buscas no existe o ha sido movida.' }}
      </p>
      <div class="mt-8 space-x-4">
        <NuxtLink 
          to="/" 
          class="inline-block bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition"
        >
          Volver al Inicio
        </NuxtLink>
        <NuxtLink 
          to="/propiedades" 
          class="inline-block bg-gray-200 text-gray-800 px-6 py-3 rounded-lg hover:bg-gray-300 transition"
        >
          Ver Propiedades
        </NuxtLink>
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
})

// Asegurar que el código de estado correcto se envíe al navegador
useHead({
  title: `Error ${props.error.statusCode}`,
  meta: [
    { name: 'robots', content: 'noindex, nofollow' }
  ]
})
</script>
