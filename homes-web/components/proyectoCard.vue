<template>
    <div class="w-full p-4 flex justify-center card-hover h-full">
        <NuxtLink :to="proyectoPath"
            class="fuente-raleway property-card w-full max-w-sm bg-white border border-gray-200 rounded-lg shadow-sm overflow-hidden relative">

            <img class="w-full h-64 object-cover" :src="proyecto.image" :alt="proyecto.title" />

            <div class="p-5 pb-20">
                <h5 class="text-xl max-w-xl subtitle-optima font-bold tracking-tight text-gray-900">
                    {{ proyecto.title }}
                </h5>
                <p class="text-gray-600">
                    {{ proyecto.tipos }}
                </p>
                <p class="text-gray-600 mt-1">
                    {{ proyecto.ubicacion }}
                </p>
                
                <p class="text-sm text-gray-500 mt-2 leading-relaxed">
                    {{ truncateText(proyecto.descripcion, 80) }}
                </p>
            </div>

            <div
                class="absolute bottom-0 left-0 w-full bg-white p-4 flex items-center justify-end">
                <div
                    class="text-white bg-black hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5">
                    Ver detalles
                </div>
            </div>
        </NuxtLink>
    </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
    proyecto: {
        type: Object,
        required: true,
    },
});

const proyectoPath = computed(() => {
  const slug = props.proyecto?.slugProyecto || '';
  if (!slug) {
    return '/error';
  }
  return `/proyecto/${slug}`;
});

// Nueva función: elimina etiquetas HTML de forma segura (client + fallback SSR)
const stripHtml = (html) => {
  if (!html) return '';
  // Uso de DOM cuando está disponible (cliente)
  if (typeof document !== 'undefined') {
    const div = document.createElement('div');
    div.innerHTML = html;
    return div.textContent || div.innerText || '';
  }
  // Fallback simple para entornos sin DOM (SSR)
  return html.replace(/<[^>]*>/g, '');
};

// truncateText ahora limpia HTML antes de truncar
const truncateText = (text, maxLength) => {
    const plain = stripHtml(text || "");
    return plain.length > maxLength ? plain.substring(0, maxLength) + "..." : plain;
};
</script>

<style scoped>
.fuente-raleway {
    font-family: 'Raleway', sans-serif;
}

.precio-optima {
    font-family: "Optima", serif;
    font-weight: normal;
    font-size: 24px;
}

.property-card {
    position: relative;
    will-change: transform;
    transition: transform 0.3s ease-in-out;
}

.property-card::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.12), 0 4px 8px rgba(0, 0, 0, 0.06);
    opacity: 0;
    transition: opacity 0.3s ease-in-out;
    z-index: -1;
    border-radius: inherit;
}


.property-card:hover {
    transform: scale(1.05);
    z-index: 10;
}

.property-card:hover::after {
    opacity: 1;
}
</style>