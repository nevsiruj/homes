<template>
  <Teleport to="body">
    <div
      v-if="isOpen"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
    <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
      <!-- Overlay -->
      <div
        class="fixed inset-0 bg-[#6a728273] bg-opacity-75 transition-opacity z-40"
        @click="$emit('close')"
      ></div>

      <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

      <!-- Modal -->
      <div
        class="relative inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-4xl sm:w-full z-50"
      >
        <div class="bg-white">
          <!-- Header del modal -->
          <div class="flex items-center justify-between px-6 py-4 border-b border-gray-200">
            <h3 class="text-lg font-medium text-gray-900">Vista Previa del Artículo</h3>
            <button
              @click="$emit('close')"
              class="text-gray-400 hover:text-gray-500 focus:outline-none"
            >
              <svg class="h-6 w-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>

          <!-- Contenido del artículo -->
          <div class="px-6 py-8 max-h-[70vh] overflow-y-auto">
            <article class="prose prose-lg max-w-none">
              <!-- Imagen destacada -->
              <img
                v-if="articulo.imageUrl"
                :src="articulo.imageUrl"
                :alt="articulo.title"
                class="w-full h-64 object-cover rounded-lg mb-6"
                @error="imagenError"
              />

              <!-- Categoría y fecha -->
              <div class="flex items-center gap-4 mb-4 text-sm text-gray-600">
                <span class="inline-flex items-center px-3 py-1 rounded-full bg-gray-100 text-gray-800">
                  {{ articulo.categorias }}
                </span>
                <span v-if="articulo.fechaCreacion">{{ formatearFecha(articulo.fechaCreacion) }}</span>
              </div>

              <!-- Título -->
              <h1 class="text-3xl md:text-4xl font-extrabold text-gray-900 mb-6">
                {{ articulo.title }}
              </h1>

              <!-- Contenido HTML -->
              <div class="article-content" v-html="articulo.content"></div>

              <!-- Permalink -->
              <div class="mt-8 p-4 bg-gray-50 rounded-lg border border-gray-200">
                <p class="text-sm text-gray-600 mb-1">URL del artículo:</p>
                <a
                  :href="articulo.permalink"
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-blue-600 hover:text-blue-800 text-sm break-all"
                >
                  {{ articulo.permalink }}
                </a>
              </div>
            </article>
          </div>

          <!-- Footer -->
          <div class="bg-gray-50 px-6 py-4 border-t border-gray-200">
            <button
              @click="$emit('close')"
              class="w-full sm:w-auto px-4 py-2 bg-gray-700 text-white rounded-md hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
            >
              Cerrar Vista Previa
            </button>
          </div>
        </div>
      </div>
    </div>
    </div>
  </Teleport>
</template>

<script setup>
const props = defineProps({
  isOpen: {
    type: Boolean,
    default: false
  },
  articulo: {
    type: Object,
    required: true
  }
});

defineEmits(['close']);

const formatearFecha = (fecha) => {
  if (!fecha) return '';
  const date = new Date(fecha);
  return date.toLocaleDateString('es-GT', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
};

const imagenError = (e) => {
  e.target.style.display = 'none';
};
</script>

<style scoped>
/* Contenedor principal del contenido */
.article-content {
  width: 100%;
  max-width: 100%;
  overflow-wrap: break-word;
  word-wrap: break-word;
  word-break: break-word;
  hyphens: auto;
}

/* Estilos para el contenido HTML del artículo */
.article-content :deep(h2) {
  font-size: 1.5rem;
  font-weight: 700;
  color: #111827;
  margin-top: 2rem;
  margin-bottom: 1rem;
  overflow-wrap: break-word;
  word-break: break-word;
}

.article-content :deep(h3) {
  font-size: 1.25rem;
  font-weight: 700;
  color: #111827;
  margin-top: 1.5rem;
  margin-bottom: 0.75rem;
  overflow-wrap: break-word;
  word-break: break-word;
}

.article-content :deep(p) {
  color: #374151;
  margin-bottom: 1rem;
  line-height: 1.75;
  overflow-wrap: break-word;
  word-break: break-word;
}

.article-content :deep(ul) {
  list-style-type: disc;
  list-style-position: inside;
  margin-bottom: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  padding-left: 0;
}

.article-content :deep(ol) {
  list-style-type: decimal;
  list-style-position: inside;
  margin-bottom: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  padding-left: 0;
}

.article-content :deep(li) {
  overflow-wrap: break-word;
  word-break: break-word;
}

.article-content :deep(blockquote) {
  border-left: 4px solid #d1d5db;
  padding-left: 1rem;
  font-style: italic;
  color: #4b5563;
  margin: 1rem 0;
  overflow-wrap: break-word;
  word-break: break-word;
}

.article-content :deep(a) {
  color: #2563eb;
  text-decoration: underline;
  overflow-wrap: break-word;
  word-break: break-all;
}

.article-content :deep(a:hover) {
  color: #1e40af;
}

.article-content :deep(img) {
  border-radius: 0.5rem;
  margin: 1rem 0;
  max-width: 100%;
  height: auto;
  display: block;
}

/* Estilos para alineación de imágenes */
.article-content :deep(img[data-alignment="left"]) {
  margin-left: 0;
  margin-right: auto;
}

.article-content :deep(img[data-alignment="center"]) {
  margin-left: auto;
  margin-right: auto;
}

.article-content :deep(img[data-alignment="right"]) {
  margin-left: auto;
  margin-right: 0;
}

.article-content :deep(code) {
  background-color: #f3f4f6;
  border-radius: 0.25rem;
  padding: 0.125rem 0.5rem;
  font-size: 0.875rem;
  font-family: monospace;
  overflow-wrap: break-word;
  word-break: break-all;
}

.article-content :deep(pre) {
  background-color: #f3f4f6;
  border-radius: 0.5rem;
  padding: 1rem;
  overflow-x: auto;
  margin-bottom: 1rem;
  max-width: 100%;
}

/* Estilos para videos de YouTube */
.article-content :deep(div[data-youtube-video]) {
  margin: 1rem 0;
  max-width: 100%;
  display: flex;
}

.article-content :deep(div[data-youtube-video] iframe) {
  border-radius: 0.5rem;
  max-width: 100%;
  height: auto;
  aspect-ratio: 16 / 9;
}

/* Estilos de alineación de videos de YouTube */
.article-content :deep(div[data-youtube-video][data-alignment="left"]) {
  justify-content: flex-start;
}

.article-content :deep(div[data-youtube-video][data-alignment="center"]) {
  justify-content: center;
}

.article-content :deep(div[data-youtube-video][data-alignment="right"]) {
  justify-content: flex-end;
}

/* Estilos para alineación de texto */
.article-content :deep([style*="text-align: left"]) {
  text-align: left;
}

.article-content :deep([style*="text-align: center"]) {
  text-align: center;
}

.article-content :deep([style*="text-align: right"]) {
  text-align: right;
}

.article-content :deep([style*="text-align: justify"]) {
  text-align: justify;
}
</style>
