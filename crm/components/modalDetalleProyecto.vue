<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 modal-overlay bg-black bg-opacity-60 z-50 flex items-center justify-center p-4 backdrop-blur-sm"
  >
    <div
      class="relative bg-white rounded-2xl shadow-2xl w-full max-w-6xl max-h-[95vh] overflow-hidden transform transition-all"
      style="max-width: 1400px;"
    >
      <LoaderModal v-if="isLoading" />

      <div v-else class="flex flex-col h-full max-h-[95vh]">
        <!-- Header mejorado con gradiente -->
        <div class="relative bg-gradient-to-r from-purple-900 to-purple-700 text-white p-6 md:p-8">
          <button
            @click="closeModal"
            class="absolute top-4 right-4 text-white hover:bg-white/20 rounded-full p-2 transition-all duration-200 z-10"
          >
            <svg
              class="w-6 h-6"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </button>
          
          <div class="pr-12">
            <h3 class="text-2xl md:text-3xl font-bold mb-2">
              {{ proyecto.titulo }}
            </h3>
            <div class="flex flex-wrap items-center gap-4 text-sm text-purple-100">
              <span class="flex items-center gap-1">
                <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M5.05 4.05a7 7 0 119.9 9.9L10 18.9l-4.95-4.95a7 7 0 010-9.9zM10 11a2 2 0 100-4 2 2 0 000 4z" clip-rule="evenodd"/>
                </svg>
                {{ proyecto.ubicaciones || "Ubicación no especificada" }}
              </span>
              <span class="flex items-center gap-1 font-semibold text-yellow-300">
                <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M8.433 7.418c.155-.103.346-.196.567-.267v1.698a2.305 2.305 0 01-.567-.267C8.07 8.34 8 8.114 8 8c0-.114.07-.34.433-.582zM11 12.849v-1.698c.22.071.412.164.567.267.364.243.433.468.433.582 0 .114-.07.34-.433.582a2.305 2.305 0 01-.567.267z"/>
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-13a1 1 0 10-2 0v.092a4.535 4.535 0 00-1.676.662C6.602 6.234 6 7.009 6 8c0 .99.602 1.765 1.324 2.246.48.32 1.054.545 1.676.662v1.941c-.391-.127-.68-.317-.843-.504a1 1 0 10-1.51 1.31c.562.649 1.413 1.076 2.353 1.253V15a1 1 0 102 0v-.092a4.535 4.535 0 001.676-.662C13.398 13.766 14 12.991 14 12c0-.99-.602-1.765-1.324-2.246A4.535 4.535 0 0011 9.092V7.151c.391.127.68.317.843.504a1 1 0 101.511-1.31c-.563-.649-1.413-1.076-2.354-1.253V5z" clip-rule="evenodd"/>
                </svg>
                ${{ formatPrice(proyecto.precio) }}
              </span>
              <span class="px-3 py-1 bg-white/20 rounded-full text-xs font-medium">
                Proyecto
              </span>
            </div>
          </div>
        </div>

        <!-- Contenido con scroll mejorado -->
        <div class="flex-1 overflow-y-auto p-6 md:p-8 bg-gray-50">
          <div class="grid gap-6 lg:grid-cols-3">
            <!-- Columna izquierda: Imagen Principal (más grande) -->
            <div class="lg:col-span-2 space-y-6">
              <div class="bg-white rounded-xl shadow-md overflow-hidden">
                <img
                  :src="proyecto.imagenPrincipal"
                  alt="Imagen principal"
                  class="w-full h-96 object-cover"
                  v-if="proyecto.imagenPrincipal"
                />
                <div v-else class="w-full h-96 bg-gray-200 flex items-center justify-center">
                  <span class="text-gray-400">Sin imagen disponible</span>
                </div>
              </div>

              <!-- Características en card -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M3 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd"/>
                  </svg>
                  Características
                </h4>
                <div class="grid grid-cols-2 gap-4">
                  <div class="bg-gray-50 rounded-lg p-4 text-center">
                    <svg class="w-8 h-8 mx-auto mb-2 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M3 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd"/>
                    </svg>
                    <p class="text-2xl font-bold text-gray-900">{{ proyecto.metrosCuadrados || 0 }}</p>
                    <p class="text-xs text-gray-500 mt-1">m² Construcción</p>
                  </div>
                </div>
              </div>

              <!-- Descripción -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"/>
                  </svg>
                  Descripción
                </h4>
                <div class="prose prose-sm max-w-none text-gray-700" v-html="cleanContent(proyecto.contenido)"></div>
              </div>

              <!-- Imágenes de referencia -->
              <div v-if="proyecto.imagenesReferenciaProyecto && proyecto.imagenesReferenciaProyecto.length" class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M4 3a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V5a2 2 0 00-2-2H4zm12 12H4l4-8 3 6 2-4 3 6z" clip-rule="evenodd"/>
                  </svg>
                  Galería ({{ proyecto.imagenesReferenciaProyecto.length }} imágenes)
                </h4>
                <div class="grid grid-cols-2 sm:grid-cols-3 gap-4">
                  <div
                    v-for="(url, index) in proyecto.imagenesReferenciaProyecto"
                    :key="index"
                    @click="openLightbox(index)"
                    class="relative group cursor-pointer overflow-hidden rounded-lg shadow hover:shadow-xl transition-all duration-300"
                  >
                    <img
                      :src="url"
                      :alt="`Imagen ${index + 1} de ${proyecto.imagenesReferenciaProyecto.length}`"
                      class="w-full h-48 object-cover transform group-hover:scale-110 transition-transform duration-300"
                    />
                    <!-- Overlay con icono de zoom -->
                    <div class="absolute inset-0 bg-black bg-opacity-0 group-hover:bg-opacity-40 transition-all duration-300 flex items-center justify-center">
                      <svg class="w-8 h-8 text-white opacity-0 group-hover:opacity-100 transition-opacity duration-300" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0zM10 7v6m3-3H7"/>
                      </svg>
                    </div>
                    <!-- Número de imagen -->
                    <div class="absolute top-2 right-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-full">
                      {{ index + 1 }}/{{ proyecto.imagenesReferenciaProyecto.length }}
                    </div>
                  </div>
                </div>
              </div>
              
              <!-- Lightbox para ver imágenes en tamaño completo -->
              <div
                v-if="lightboxOpen"
                @click="closeLightbox"
                class="fixed inset-0 bg-black bg-opacity-95 z-[100] flex items-center justify-center p-4"
              >
                <button
                  @click.stop="closeLightbox"
                  class="absolute top-4 right-4 text-white hover:bg-white/20 rounded-full p-2 transition-all z-10"
                >
                  <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
                  </svg>
                </button>
                
                <!-- Botón anterior -->
                <button
                  v-if="proyecto.imagenesReferenciaProyecto.length > 1"
                  @click.stop="previousImage"
                  class="absolute left-4 text-white hover:bg-white/20 rounded-full p-3 transition-all"
                >
                  <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7"/>
                  </svg>
                </button>
                
                <!-- Imagen actual -->
                <div @click.stop class="max-w-6xl max-h-[90vh] relative">
                  <img
                    :src="proyecto.imagenesReferenciaProyecto[currentImageIndex]"
                    :alt="`Imagen ${currentImageIndex + 1}`"
                    class="max-w-full max-h-[90vh] object-contain rounded-lg"
                  />
                  <div class="absolute bottom-4 left-1/2 transform -translate-x-1/2 bg-black bg-opacity-70 text-white px-4 py-2 rounded-full text-sm">
                    {{ currentImageIndex + 1 }} / {{ proyecto.imagenesReferenciaProyecto.length }}
                  </div>
                </div>
                
                <!-- Botón siguiente -->
                <button
                  v-if="proyecto.imagenesReferenciaProyecto.length > 1"
                  @click.stop="nextImage"
                  class="absolute right-4 text-white hover:bg-white/20 rounded-full p-3 transition-all"
                >
                  <svg class="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"/>
                  </svg>
                </button>
              </div>
            </div>

            <!-- Columna derecha: Info adicional -->
            <div class="space-y-6">
              <!-- Información principal -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"/>
                  </svg>
                  Información
                </h4>
                <div class="space-y-4">
                  <div class="flex justify-between items-center pb-3 border-b">
                    <span class="text-sm text-gray-500">Código</span>
                    <span class="font-semibold text-gray-900 bg-gray-100 px-3 py-1 rounded-lg">
                      {{ proyecto.codigoProyecto || "N/A" }}
                    </span>
                  </div>
                  <div class="flex justify-between items-center pb-3 border-b">
                    <span class="text-sm text-gray-500">Ubicación</span>
                    <span class="font-semibold text-gray-900 text-right text-sm">
                      {{ proyecto.ubicaciones || "No especificado" }}
                    </span>
                  </div>
                </div>
              </div>

              <!-- Amenidades -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-purple-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9z"/>
                    <path fill-rule="evenodd" d="M4 5a2 2 0 012-2 3 3 0 003 3h2a3 3 0 003-3 2 2 0 012 2v11a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm9.707 5.707a1 1 0 00-1.414-1.414L9 12.586l-1.293-1.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                  </svg>
                  Amenidades
                </h4>
                <div v-if="proyecto.amenidades && proyecto.amenidades.length > 0" class="flex flex-wrap gap-2">
                  <span
                    v-for="(amenidad, index) in proyecto.amenidades"
                    :key="index"
                    class="inline-flex items-center px-3 py-1.5 bg-purple-50 text-purple-700 rounded-lg text-sm font-medium hover:bg-purple-100 transition-colors"
                  >
                    <svg class="w-4 h-4 mr-1.5" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"/>
                    </svg>
                    {{ amenidad.nombre || "Desconocida" }}
                  </span>
                </div>
                <p v-else class="text-gray-400 text-sm">No hay amenidades definidas</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Footer mejorado -->
        <div class="bg-white border-t p-6 flex justify-end gap-3">
          <button
            @click="closeModal"
            class="px-6 py-3 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 rounded-xl text-sm font-medium transition-all"
          >
            Cerrar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue"; // <-- **SE AÑADE 'ref' y 'watch'**
import LoaderModal from "./LoaderModal.vue";

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
    default: false,
  },
  proyecto: {
    type: Object,
    default: () => ({
      codigoProyecto: "",
      titulo: "",
      precio: 0,
      habitaciones: 0,
      banos: 0,
      parqueos: 0,
      metrosCuadrados: 0,
      imagenPrincipal: "",
      contenido: "",
      tipos: "",
      operaciones: "",
      ubicaciones: "",
      luxury: false,
      video: "",
      amenidades: [],
      imagenesReferenciaProyecto: [],
    }),
  },
});

const emit = defineEmits(["close"]);

// CORRECCIÓN: **Se declara la variable de estado `isLoading`**
const isLoading = ref(true); 
const lightboxOpen = ref(false);
const currentImageIndex = ref(0); 

const formatPrice = (price) => {
  return Number(price).toLocaleString("es-GT");
};

const cleanContent = (content) => {
  if (!content) return "";
  return content.replace(/\\n/g, "\n").replace(/&nbsp;/g, "");
};

// Se mantiene la lógica de simulación de carga
watch(
  () => props.proyecto,
  (newVal) => {
    if (props.isOpen) {
      if (!newVal || Object.keys(newVal).length === 0 || newVal.codigoProyecto === "") {
        isLoading.value = true;
      } else {
        setTimeout(() => {
          isLoading.value = false;
        }, 2000); // Delay de 2 segundos
      }
    }
  },
  { immediate: true }
);

const closeModal = () => {
  emit("close");
};

// Funciones del lightbox
const openLightbox = (index) => {
  currentImageIndex.value = index;
  lightboxOpen.value = true;
  // Prevenir scroll del body cuando el lightbox está abierto
  document.body.style.overflow = 'hidden';
};

const closeLightbox = () => {
  lightboxOpen.value = false;
  currentImageIndex.value = 0;
  // Restaurar scroll del body
  document.body.style.overflow = '';
};

const nextImage = () => {
  if (currentImageIndex.value < props.proyecto.imagenesReferenciaProyecto.length - 1) {
    currentImageIndex.value++;
  } else {
    currentImageIndex.value = 0; // Volver al inicio
  }
};

const previousImage = () => {
  if (currentImageIndex.value > 0) {
    currentImageIndex.value--;
  } else {
    currentImageIndex.value = props.proyecto.imagenesReferenciaProyecto.length - 1; // Ir al final
  }
};

// Soporte para teclas de navegación
const handleKeyPress = (event) => {
  if (!lightboxOpen.value) return;
  
  if (event.key === 'Escape') {
    closeLightbox();
  } else if (event.key === 'ArrowRight') {
    nextImage();
  } else if (event.key === 'ArrowLeft') {
    previousImage();
  }
};

// Agregar listener para teclas cuando se monta
watch(lightboxOpen, (isOpen) => {
  if (isOpen) {
    window.addEventListener('keydown', handleKeyPress);
  } else {
    window.removeEventListener('keydown', handleKeyPress);
  }
});
</script>

<style scoped>
.modal-overlay {
  backdrop-filter: blur(0.5px);
}
</style>