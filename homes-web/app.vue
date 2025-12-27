<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<script setup>
import { useOrganizationSchema, useJsonldSchema } from './composables/useStructuredData'

useHead({
  htmlAttrs: {
    lang: 'es'
  },
  meta: [
    { name: 'viewport', content: 'width=device-width, initial-scale=1' },
    { charset: 'utf-8' }
  ]
})

// Inyectar Schema de Organización Global (Identidad)
useJsonldSchema(useOrganizationSchema())
</script>

<style>
/* Estilos globales mínimos */
body {
  margin: 0;
  padding: 0;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>
