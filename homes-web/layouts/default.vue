<template>
    <div>
      <slot />
    </div>
  </template>
  
  <script setup>
  // Define a base title and a template for all pages
  useHead({
    titleTemplate: (titleChunk) => {
      return titleChunk
        ? `${titleChunk} | Homes Guatemala`
        : 'Homes Guatemala - Venta y Alquiler de Propiedades';
    },
    meta: [
      {
        name: 'description',
        content: 'Encuentra las mejores propiedades inmobiliarias en Guatemala. Casas, apartamentos, terrenos, y más en venta y alquiler.',
      },
    ],
  });
  </script>