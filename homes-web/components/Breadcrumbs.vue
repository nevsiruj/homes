<template>
  <nav class="mb-6 text-sm" aria-label="Breadcrumb">
    <ol class="flex items-center space-x-2">
      <li v-for="(crumb, index) in breadcrumbs" :key="index" class="flex items-center">
        <NuxtLink 
          v-if="index < breadcrumbs.length - 1"
          :to="crumb.url"
          class="text-gray-600 hover:text-black transition-colors"
        >
          {{ crumb.name }}
        </NuxtLink>
        <span v-else class="text-black font-semibold">
          {{ crumb.name }}
        </span>
        <svg 
          v-if="index < breadcrumbs.length - 1"
          class="w-4 h-4 mx-2 text-gray-400"
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path fill-rule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clip-rule="evenodd" />
        </svg>
      </li>
    </ol>
  </nav>
</template>

<script setup>
defineProps({
  breadcrumbs: {
    type: Array,
    required: true,
    validator: (value) => {
      return value.every(crumb => crumb.name && crumb.url)
    }
  }
})
</script>

<style scoped>
/* Estilos adicionales si son necesarios */
</style>
