<template>
    <div
      class="absolute inset-0 flex items-center justify-center bg-white bg-opacity-75 z-20 rounded-lg"
    >
      <img
        src="https://app-pool.vylaris.online/dcmigserver/homes/684980eb-ab75-41e5-8c2a-045abba3b954.webp"
        alt="Cargando..."
        class="w-32 h-auto animate-pulse-custom"
      />
    </div>
  </template>
  
  <script setup>
  // No necesita lógica específica aquí
  </script>
  
  <style scoped>
  /* Define la animación de pulso, si se mantiene en el modal */
  @keyframes pulse-custom {
    0%,
    100% {
      opacity: 1;
    }
    50% {
      opacity: 0.5;
    }
  }
  
  .animate-pulse-custom {
    animation: pulse-custom 1.5s cubic-bezier(0.4, 0, 0.6, 1) infinite;
  }
  </style>