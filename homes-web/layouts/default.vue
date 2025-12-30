<template>
    <div>
      <slot />
    </div>
  </template>
  
  <script setup>
  // Define a base title and a template for all pages
  useHead({
    titleTemplate: (titleChunk) => {
      if (!titleChunk) return 'Homes Guatemala - Bienes Raíces de Lujo';
      return titleChunk.includes('Homes Guatemala') 
        ? titleChunk 
        : `${titleChunk} | Homes Guatemala`;
    },
    meta: [
      {
        name: 'description',
        content: 'Encuentra las mejores propiedades inmobiliarias en Guatemala. Casas, apartamentos, terrenos, y más en venta y alquiler.',
      },
    ],
  });
  </script>