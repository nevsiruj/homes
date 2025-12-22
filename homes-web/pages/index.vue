<template>
    <div>
      <Index />      
    </div>
  </template>
  <script setup>
  import Index from "./home/index.vue";
  
  useSeoMeta({
    title: 'Homes Guatemala - Bienes Raíces de Lujo',
    description: 'Encuentra las mejores casas y apartamentos en venta y alquiler en las zonas más exclusivas de Guatemala.',
    ogTitle: 'Homes Guatemala - Bienes Raíces de Lujo',
    ogDescription: 'Venta y alquiler de casas y apartamentos en las zonas más exclusivas de Guatemala.',
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

