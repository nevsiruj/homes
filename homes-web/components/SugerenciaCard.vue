<template>
    <div class="w-full">
        <NuxtLink :to="url"
            class="relative block w-full h-80 overflow-hidden rounded-lg shadow-md group fuente-raleway">
            <img v-if="item.imagenPrincipal" :src="item.imagenPrincipal.trim()"
                class="w-full h-full object-cover object-center transform transition-transform duration-500 group-hover:scale-105"
                :alt="item.titulo || 'Imagen de sugerencia'" />
            <div v-else class="w-full h-full bg-gray-200 flex items-center justify-center">
                <span class="text-gray-500">Sin imagen</span>
            </div>

            <div
                class="absolute inset-0 bg-gradient-to-t from-black via-transparent to-transparent opacity-70 group-hover:opacity-80 transition-opacity duration-300">
            </div>

            <div class="absolute bottom-0 left-0 p-4 text-white w-full">
                <h5 class="text-lg font-bold leading-tight drop-shadow-lg truncate">
                    {{ item.titulo || "Sugerencia sin título" }}
                </h5>
                <p class="text-sm mt-1 opacity-90 truncate">
                    {{ item.ubicaciones }}
                </p>
                <p class="text-lg font-semibold mt-2 opacity-95 precio-optima">
                    {{ formattedPrice }}
                </p>
            </div>

            <div
                class="absolute top-4 right-4 text-white opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24"
                    stroke="currentColor" stroke-width="2">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M14 5l7 7m0 0l-7 7m7-7H3" />
                </svg>
            </div>
        </NuxtLink>
    </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  item: { type: Object, required: true },
  type: {
    type: String,
    required: true,
    validator: (v) => ['propiedad', 'proyecto'].includes(v),
  },
});

// Normalizo posibles nombres del campo y lo preparo para URL
const safeSlug = computed(() => {
  const s =
    props.item?.slugInmueble ??
    props.item?.slug ??
    props.item?.SlugInmueble ??
    '';
  return encodeURIComponent(String(s).trim());
});

const url = computed(() => {
  if (props.type === 'propiedad') {
    // Si no hay slug, dejo un fallback neutral para evitar links rotos
    return safeSlug.value ? `/inmueble/${safeSlug.value}` : '#';
  }
  // Proyectos: sin cambios (ajusta si también usás slug en proyectos)
  return safeSlug.value ? `/proyecto/${safeSlug.value}` : '#';
});

const formattedPrice = computed(() => {
  // Si la propiedad precioActivo es false, oculta el precio y muestra el texto.
  if (props.item?.precioActivo === false) {
    return " ";
  }

  // Lógica existente para manejar precios no definidos o nulos.
  if (!props.item?.precio && props.item?.precio !== 0) {
    return 'Consultar precio';
  }
  
  // Lógica para formatear el precio si está activo.
  const priceNumber = Number(props.item.precio);
  return priceNumber.toLocaleString('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 0,
  });
});
</script>


<style scoped>
.fuente-raleway {
    font-family: 'Raleway', sans-serif;
}

.precio-optima {
    font-family: "Optima", serif;
}
</style>