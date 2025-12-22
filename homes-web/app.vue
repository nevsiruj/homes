<template>
  <NuxtLayout>
    <!-- Loader global en cada navegación -->
    <!-- <Loader v-if="showLoader" /> -->
    <NuxtPage />
  </NuxtLayout>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useNuxtApp, useRouter } from '#app'
import { useOrganizationSchema, useJsonldSchema } from './composables/useStructuredData'
//import Loader from '~/components/Loader.vue'

const showLoader = ref(false)
const startedAt = ref(0)
const MIN_DURATION_MS = 2000
const delay = (ms) => new Promise((r) => setTimeout(r, ms))

// Asegura el mínimo de 2s entre start y finish
const startLoader = () => {
  startedAt.value = Date.now()
  showLoader.value = true
}
const stopLoader = async () => {
  const elapsed = Date.now() - startedAt.value
  if (elapsed < MIN_DURATION_MS) {
    await delay(MIN_DURATION_MS - elapsed)
  }
  showLoader.value = false
}

const nuxtApp = useNuxtApp()
const router = useRouter()

useHead({
  htmlAttrs: {
    lang: 'es'
  },
  link: [
    { rel: 'alternate', hreflang: 'es-GT', href: 'https://homesguatemala.com' },
    { rel: 'alternate', hreflang: 'x-default', href: 'https://homesguatemala.com' }
  ],
  meta: [
    { name: 'description', content: 'Líderes en bienes raíces en Guatemala. Encuentra casas y apartamentos.' }
  ]
})

// Inyectar Schema de Organización Global (Identidad)
useJsonldSchema(useOrganizationSchema())

onMounted(() => {
  // Hooks nativos de Nuxt para navegación de páginas
  nuxtApp.hook('page:start', () => {
    startLoader()
  })
  nuxtApp.hook('page:finish', async () => {
    await stopLoader()
  })

  // Fallbacks por si una navegación falla o se cancela
  router.onError(async () => {
    await stopLoader()
  })
})
</script>

<!-- Opcional: si tu Loader NO es full-screen, podés envolverlo en un overlay -->
<style scoped>
/* Ejemplo de overlay (usa solo si tu Loader no ocupa toda la pantalla)
.loader-overlay {
  position: fixed;
  inset: 0;
  display: grid;
  place-items: center;
  background: rgba(255, 255, 255, 0.75);
  z-index: 9999;
}
*/
</style>
