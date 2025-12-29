<template>
  <Header />
  
  <div class="max-w-[1200px] mx-auto p-4 mt-24 lg:mt-12">
    <!-- Breadcrumbs -->
    <Breadcrumbs :breadcrumbs="breadcrumbItems" />

    <!-- Título y descripción -->
    <div class="mb-8">
      <h1 class="text-3xl md:text-4xl lg:text-5xl font-bold mb-4 text-gray-900">
        Propiedades en {{ zonaNombre }}
      </h1>
      
      <p class="text-lg text-gray-700 mb-4">
        Descubre las mejores propiedades disponibles en {{ zonaNombre }}, Guatemala.
      </p>

      <div class="flex items-center gap-4 text-sm text-gray-600">
        <span class="font-semibold">{{ totalPropiedades }} propiedades encontradas</span>
        <span v-if="!isLoading">•</span>
        <span v-if="!isLoading">{{ tiposPropiedad }}</span>
      </div>
    </div>

    <!-- Loading skeleton -->
    <div v-if="isLoading" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div v-for="n in 6" :key="n" class="animate-pulse">
        <div class="bg-gray-300 h-64 rounded-lg mb-4"></div>
        <div class="h-4 bg-gray-300 rounded w-3/4 mb-2"></div>
        <div class="h-4 bg-gray-300 rounded w-1/2"></div>
      </div>
    </div>

    <!-- Grid de propiedades -->
    <div v-else-if="propiedades.length > 0" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <SugerenciaCard
        v-for="propiedad in propiedades"
        :key="propiedad.id"
        :item="propiedad"
        type="propiedad"
      />
    </div>

    <!-- Sin resultados -->
    <div v-else class="text-center py-16">
      <svg class="mx-auto h-16 w-16 text-gray-400 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
      </svg>
      <h2 class="text-2xl font-semibold text-gray-900 mb-2">
        No hay propiedades disponibles
      </h2>
      <p class="text-gray-600 mb-6">
        No encontramos propiedades en {{ zonaNombre }} en este momento.
      </p>
      <NuxtLink 
        to="/propiedades" 
        class="inline-block px-6 py-3 bg-black text-white rounded hover:bg-gray-800 transition-colors"
      >
        Explorar propiedades en otras zonas
      </NuxtLink>
    </div>

    <!-- Información adicional sobre la zona -->
    <div v-if="propiedades.length > 0" class="mt-12 bg-gray-50 rounded-lg p-6">
      <h2 class="text-2xl font-bold mb-4">Sobre {{ zonaNombre }}</h2>
      <p class="text-gray-700 leading-relaxed">
        {{ zonaNombre }} es una de las zonas más buscadas para comprar o alquilar propiedades en Guatemala. 
        Encuentra casas, apartamentos, terrenos y más en esta ubicación privilegiada.
      </p>
    </div>
  </div>

  <Footer />
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRoute, useAsyncData } from '#imports';
import inmuebleService from '../../../services/inmuebleService';

const route = useRoute();
const zona = route.params.zona;

// Normalizar nombre de zona para display
const zonaNombre = computed(() => {
  if (!zona) return '';
  return zona
    .split('-')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ');
});

// Breadcrumbs
const breadcrumbItems = computed(() => [
  { name: 'Inicio', url: '/' },
  { name: 'Propiedades', url: '/propiedades' },
  { name: zonaNombre.value, url: route.fullPath }
]);

// Cargar propiedades por zona
const { data: propiedades, pending: isLoading } = await useAsyncData(
  `zona-${zona}`,
  async () => {
    try {
      // Optimización: Reducido de 200 a 50 para mejor rendimiento inicial
      const response = await inmuebleService.getInmueblesPaginados(1, 50);
      const allProps = response?.items || [];
      
      // Filtrar por zona (buscar en zona, ubicaciones, o ubicacion)
      return allProps.filter(p => {
        const propZona = (p.zona || p.ubicaciones || p.ubicacion || '').toLowerCase();
        const searchZona = zona.toLowerCase().replace(/-/g, ' ');
        return propZona.includes(searchZona) || searchZona.includes(propZona);
      });
    } catch (error) {
      console.error('Error cargando propiedades por zona:', error);
      return [];
    }
  }
);

// Computed properties
const totalPropiedades = computed(() => propiedades.value?.length || 0);

const tiposPropiedad = computed(() => {
  if (!propiedades.value || propiedades.value.length === 0) return '';
  
  const tipos = new Set();
  propiedades.value.forEach(p => {
    if (p.tipoPropiedad) tipos.add(p.tipoPropiedad);
  });
  
  return Array.from(tipos).join(', ');
});

// SEO Meta Tags
const pageTitle = computed(() => `Propiedades en ${zonaNombre.value} | Homes Guatemala`);
const pageDescription = computed(() => 
  `Encuentra las mejores propiedades en venta y alquiler en ${zonaNombre.value}, Guatemala. ${totalPropiedades.value} propiedades disponibles. Casas, apartamentos, terrenos y más.`
);
const pageUrl = computed(() => `https://homesguatemala.com${route.fullPath}`);

useSeoMeta({
  title: pageTitle,
  description: pageDescription,
  ogTitle: pageTitle,
  ogDescription: pageDescription,
  ogUrl: pageUrl,
  ogType: 'website',
  twitterCard: 'summary_large_image',
  twitterTitle: pageTitle,
  twitterDescription: pageDescription,
});

// Schema.org - Breadcrumbs
const breadcrumbSchema = useBreadcrumbSchema(breadcrumbItems.value);
useSchemaOrg(breadcrumbSchema);
</script>

<style scoped>
/* Estilos específicos si son necesarios */
</style>
