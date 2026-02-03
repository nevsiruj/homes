<template>
  <div>
    <Index />
  </div>
</template>

<script setup>
import Index from "./home/index.vue";
import { useRequestURL } from "#imports";

const requestUrl = useRequestURL();
const origin = computed(() => {
  try {
    if (requestUrl.origin && !requestUrl.origin.includes('localhost')) {
      return requestUrl.origin;
    }
  } catch (e) { }
  return 'https://homesguatemala.com';
});

useSeoMeta({
  title: 'Casas y Apartamentos en Venta y Renta Guatemala | Homes Guatemala',
  description: 'Bienes raíces de lujo en Guatemala. Casas y apartamentos en venta y renta en Zona 10, 14, 15, 16, Cayalá, Carretera El Salvador. Proyectos inmobiliarios exclusivos. +18 años de experiencia.',
  ogTitle: 'Casas y Apartamentos en Venta y Renta Guatemala | Homes Guatemala',
  ogDescription: 'Encuentra propiedades de lujo en las mejores zonas de Guatemala. Venta, renta y proyectos inmobiliarios exclusivos.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
  ogImageSecureUrl: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
  ogImageType: 'image/webp',
  ogImageWidth: 1200,
  ogImageHeight: 630,
  ogUrl: () => origin.value,
  twitterCard: 'summary_large_image',
})

useHead({
  link: [
    { rel: 'canonical', href: () => origin.value }
  ]
})
</script>
