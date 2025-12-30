<template>
    <div>
      <Index />      
    </div>
  </template>
  <script setup>
  import Index from "./home/index.vue";
  
  useSeoMeta({
    title: 'Casas y Apartamentos en Venta y Renta Guatemala | Homes Guatemala',
    description: 'Bienes raíces de lujo en Guatemala. Casas y apartamentos en venta y renta en Zona 10, 14, 15, 16, Cayalá, Carretera El Salvador. Proyectos inmobiliarios exclusivos. +18 años de experiencia.',
    ogTitle: 'Casas y Apartamentos en Venta y Renta Guatemala | Homes Guatemala',
    ogDescription: 'Encuentra propiedades de lujo en las mejores zonas de Guatemala. Venta, renta y proyectos inmobiliarios exclusivos.',
    ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
    ogUrl: 'https://homesguatemala.com',
    twitterCard: 'summary_large_image',
  })

  useHead({
    link: [
      { rel: 'canonical', href: 'https://homesguatemala.com' }
    ]
  })
  </script>

