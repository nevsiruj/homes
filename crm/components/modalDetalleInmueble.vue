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
        <div class="relative bg-gradient-to-r from-gray-900 to-gray-700 text-white p-6 md:p-8">
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
              {{ inmueble.titulo }}
            </h3>
            <div class="flex flex-wrap items-center gap-4 text-sm text-gray-200">
              <span class="flex items-center gap-1">
                <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M5.05 4.05a7 7 0 119.9 9.9L10 18.9l-4.95-4.95a7 7 0 010-9.9zM10 11a2 2 0 100-4 2 2 0 000 4z" clip-rule="evenodd"/>
                </svg>
                {{ inmueble.ubicaciones || "Ubicación no especificada" }}
              </span>
              <span class="flex items-center gap-1 font-semibold text-yellow-300">
                <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M8.433 7.418c.155-.103.346-.196.567-.267v1.698a2.305 2.305 0 01-.567-.267C8.07 8.34 8 8.114 8 8c0-.114.07-.34.433-.582zM11 12.849v-1.698c.22.071.412.164.567.267.364.243.433.468.433.582 0 .114-.07.34-.433.582a2.305 2.305 0 01-.567.267z"/>
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-13a1 1 0 10-2 0v.092a4.535 4.535 0 00-1.676.662C6.602 6.234 6 7.009 6 8c0 .99.602 1.765 1.324 2.246.48.32 1.054.545 1.676.662v1.941c-.391-.127-.68-.317-.843-.504a1 1 0 10-1.51 1.31c.562.649 1.413 1.076 2.353 1.253V15a1 1 0 102 0v-.092a4.535 4.535 0 001.676-.662C13.398 13.766 14 12.991 14 12c0-.99-.602-1.765-1.324-2.246A4.535 4.535 0 0011 9.092V7.151c.391.127.68.317.843.504a1 1 0 101.511-1.31c-.563-.649-1.413-1.076-2.354-1.253V5z" clip-rule="evenodd"/>
                </svg>
                ${{ formatPrice(inmueble.precio) }}
              </span>
              <span class="px-3 py-1 bg-white/20 rounded-full text-xs font-medium">
                {{ inmueble.operaciones || "Operación" }}
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
                  :src="inmueble.imagenPrincipal"
                  alt="Imagen principal"
                  class="w-full h-96 object-cover"
                  v-if="inmueble.imagenPrincipal"
                />
                <div v-else class="w-full h-96 bg-gray-200 flex items-center justify-center">
                  <span class="text-gray-400">Sin imagen disponible</span>
                </div>
              </div>

              <!-- Imágenes de referencia (GALERÍA) -->
              <div v-if="inmueble.imagenesReferencia && inmueble.imagenesReferencia.length" class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M4 3a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V5a2 2 0 00-2-2H4zm12 12H4l4-8 3 6 2-4 3 6z" clip-rule="evenodd"/>
                  </svg>
                  Galería ({{ inmueble.imagenesReferencia.length }} imágenes)
                </h4>
                <div class="grid grid-cols-2 sm:grid-cols-3 gap-4">
                  <div
                    v-for="(imagen, index) in inmueble.imagenesReferencia"
                    :key="index"
                    @click="openLightbox(index)"
                    class="relative group cursor-pointer overflow-hidden rounded-lg shadow hover:shadow-xl transition-all duration-300 bg-gray-200"
                  >
                    <img
                      :src="getImageUrl(imagen)"
                      :alt="`Imagen ${index + 1} de ${inmueble.imagenesReferencia.length}`"
                      class="w-full h-56 object-cover transform group-hover:scale-110 transition-transform duration-300"
                      @error="handleImageError($event, imagen)"
                      @load="handleImageLoad($event, imagen)"
                      loading="eager"
                    />
                    <!-- Overlay con icono de zoom - solo visible en hover -->
                    <div class="absolute inset-0 bg-black/40 opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex items-center justify-center pointer-events-none">
                      <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0zM10 7v6m3-3H7"/>
                      </svg>
                    </div>
                    <!-- Número de imagen -->
                    <div class="absolute top-2 right-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-full">
                      {{ index + 1 }}/{{ inmueble.imagenesReferencia.length }}
                    </div>
                  </div>
                </div>
              </div>

              <!-- Características en cards -->
              <div class="grid grid-cols-2 sm:grid-cols-4 gap-4">
                <div class="bg-white rounded-xl shadow-sm p-4 text-center hover:shadow-md transition-shadow">
                  <svg class="w-8 h-8 mx-auto mb-2 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z"/>
                  </svg>
                  <p class="text-2xl font-bold text-gray-900">{{ inmueble.habitaciones || 0 }}</p>
                  <p class="text-xs text-gray-500 mt-1">Habitaciones</p>
                </div>
                <div class="bg-white rounded-xl shadow-sm p-4 text-center hover:shadow-md transition-shadow">
                  <svg class="w-8 h-8 mx-auto mb-2 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M3 5a2 2 0 012-2h10a2 2 0 012 2v8a2 2 0 01-2 2h-2.22l.123.489.804.804A1 1 0 0113 18H7a1 1 0 01-.707-1.707l.804-.804L7.22 15H5a2 2 0 01-2-2V5zm5.771 7H5V5h10v7H8.771z" clip-rule="evenodd"/>
                  </svg>
                  <p class="text-2xl font-bold text-gray-900">{{ inmueble.banos || 0 }}</p>
                  <p class="text-xs text-gray-500 mt-1">Baños</p>
                </div>
                <div class="bg-white rounded-xl shadow-sm p-4 text-center hover:shadow-md transition-shadow">
                  <svg class="w-8 h-8 mx-auto mb-2 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M8 16.5a1.5 1.5 0 11-3 0 1.5 1.5 0 013 0zM15 16.5a1.5 1.5 0 11-3 0 1.5 1.5 0 013 0z"/>
                    <path d="M3 4a1 1 0 00-1 1v10a1 1 0 001 1h1.05a2.5 2.5 0 014.9 0H10a1 1 0 001-1V5a1 1 0 00-1-1H3zM14 7a1 1 0 00-1 1v6.05A2.5 2.5 0 0115.95 16H17a1 1 0 001-1v-5a1 1 0 00-.293-.707l-2-2A1 1 0 0015 7h-1z"/>
                  </svg>
                  <p class="text-2xl font-bold text-gray-900">{{ inmueble.parqueos || 0 }}</p>
                  <p class="text-xs text-gray-500 mt-1">Parqueos</p>
                </div>
                <div class="bg-white rounded-xl shadow-sm p-4 text-center hover:shadow-md transition-shadow">
                  <svg class="w-8 h-8 mx-auto mb-2 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M3 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clip-rule="evenodd"/>
                  </svg>
                  <p class="text-2xl font-bold text-gray-900">{{ inmueble.metrosCuadrados || 0 }}</p>
                  <p class="text-xs text-gray-500 mt-1">m² Construc.</p>
                </div>
              </div>

              <!-- Descripción -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"/>
                  </svg>
                  Descripción
                </h4>
                <div class="prose prose-sm max-w-none text-gray-700" v-html="cleanContent(inmueble.contenido)"></div>
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
                  v-if="inmueble.imagenesReferencia.length > 1"
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
                    :src="getImageUrl(inmueble.imagenesReferencia[currentImageIndex])"
                    :alt="`Imagen ${currentImageIndex + 1}`"
                    class="max-w-full max-h-[90vh] object-contain rounded-lg"
                  />
                  <div class="absolute bottom-4 left-1/2 transform -translate-x-1/2 bg-black bg-opacity-70 text-white px-4 py-2 rounded-full text-sm">
                    {{ currentImageIndex + 1 }} / {{ inmueble.imagenesReferencia.length }}
                  </div>
                </div>
                
                <!-- Botón siguiente -->
                <button
                  v-if="inmueble.imagenesReferencia.length > 1"
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
                  <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"/>
                  </svg>
                  Información
                </h4>
                <div class="space-y-4">
                  <div class="flex justify-between items-center pb-3 border-b">
                    <span class="text-sm text-gray-500">Código</span>
                    <span class="font-semibold text-gray-900 bg-gray-100 px-3 py-1 rounded-lg">
                      {{ inmueble.codigoPropiedad || "N/A" }}
                    </span>
                  </div>
                  <div class="flex justify-between items-center pb-3 border-b">
                    <span class="text-sm text-gray-500">Tipo</span>
                    <span class="font-semibold text-gray-900">
                      {{ inmueble.tipos || "No especificado" }}
                    </span>
                  </div>
                  <div class="flex justify-between items-center">
                    <span class="text-sm text-gray-500">Lujo</span>
                    <span v-if="inmueble.luxury" class="px-3 py-1 bg-yellow-100 text-yellow-800 rounded-full text-xs font-medium">
                      ⭐ Premium
                    </span>
                    <span v-else class="px-3 py-1 bg-gray-100 text-gray-600 rounded-full text-xs font-medium">
                      Estándar
                    </span>
                  </div>
                </div>
              </div>

              <!-- Amenidades -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9z"/>
                    <path fill-rule="evenodd" d="M4 5a2 2 0 012-2 3 3 0 003 3h2a3 3 0 003-3 2 2 0 012 2v11a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm9.707 5.707a1 1 0 00-1.414-1.414L9 12.586l-1.293-1.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                  </svg>
                  Amenidades
                </h4>
                <div v-if="inmueble.amenidades.length > 0" class="flex flex-wrap gap-2">
                  <span
                    v-for="(amenidad, index) in inmueble.amenidades"
                    :key="amenidad.id || index"
                    class="inline-flex items-center px-3 py-1.5 bg-blue-50 text-blue-700 rounded-lg text-sm font-medium hover:bg-blue-100 transition-colors"
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
import { ref, watch } from "vue";
import LoaderModal from "./LoaderModal.vue"; 

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
    default: false,
  },
  inmueble: {
    type: Object,
    default: () => ({
      codigoPropiedad: "",
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
      imagenesReferencia: [],
    }),
  },
});

const emit = defineEmits(["close"]);

const isLoading = ref(true);
const lightboxOpen = ref(false);
const currentImageIndex = ref(0);

// Watcher para detectar cuando el modal se abre
watch(
  () => props.isOpen,
  (newVal) => {
    if (newVal) {
      console.log("🚪 [Modal] Modal abierto");
      // El loading se manejará con el watcher del inmueble
    }
  },
  { immediate: true }
);

// Simular carga de datos del inmueble  
watch(
  () => props.inmueble,
  async (newVal) => {
    if (!props.isOpen) return;
    
    console.log("🔍 [Modal] Inmueble recibido:", newVal?.codigoPropiedad || newVal?.id);
    console.log("🖼️ [Modal] Imágenes de referencia RAW (directo del prop):", newVal?.imagenesReferencia);
    console.log("📸 [Modal] Length del array:", newVal?.imagenesReferencia?.length);
    
    // 🔧 SI EL ARRAY ESTÁ VACÍO, LLAMAR AL SERVICIO DIRECTAMENTE
    if (newVal && (!newVal.imagenesReferencia || newVal.imagenesReferencia.length === 0)) {
      const inmuebleId = newVal.id || newVal.codigoPropiedad;
      
      if (inmuebleId) {
        console.log("� [Modal] Array vacío, llamando al servicio con ID:", inmuebleId);
        
        try {
          // Importar el servicio dinámicamente
          const { default: inmuebleService } = await import('~/services/inmuebleService');
          const inmuebleCompleto = await inmuebleService.getInmuebleById(inmuebleId);
          
          console.log("✅ [Modal] Datos obtenidos del servicio:", inmuebleCompleto);
          console.log("🖼️ [Modal] Imágenes del servicio:", inmuebleCompleto?.imagenesReferencia);
          
          if (inmuebleCompleto && inmuebleCompleto.imagenesReferencia) {
            // Copiar las imágenes al objeto actual
            newVal.imagenesReferencia = [...inmuebleCompleto.imagenesReferencia];
            console.log("✅ [Modal] Imágenes copiadas:", newVal.imagenesReferencia.length);
          }
        } catch (error) {
          console.error("❌ [Modal] Error al obtener inmueble:", error);
        }
      }
    }
    
    console.log("🖼️ [Modal] Imágenes FINAL:", newVal?.imagenesReferencia);
    
    // Validar si tenemos datos del inmueble
    const hasData = newVal && 
                    typeof newVal === 'object' && 
                    Object.keys(newVal).length > 3 && 
                    (newVal.id || newVal.codigoPropiedad || newVal.titulo);
    
    if (hasData) {
      isLoading.value = false;
      console.log("✅ [Modal] Datos válidos, mostrando contenido");
    } else {
      isLoading.value = true;
      console.log("⏳ [Modal] Esperando datos...");
    }
  },
  { immediate: true }
);

const formatPrice = (price) => {
  return Number(price).toLocaleString("es-GT");
};

const cleanContent = (content) => {
  if (!content) return "";
  return content.replace(/\\n/g, "\n").replace(/&nbsp;/g, "");
};

const getImageUrl = (imagen) => {
  console.log("🖼️ [Modal] Procesando imagen:", imagen);
  
  // Si es un string, asumir que es la URL directa
  if (typeof imagen === 'string') {
    console.log("🖼️ [Modal] Es string, retornando directamente:", imagen);
    return imagen;
  }
  
  // Si es un objeto, buscar la URL en diferentes propiedades posibles
  if (typeof imagen === 'object' && imagen) {
    const url = imagen.url || 
                imagen.imagen || 
                imagen.src || 
                imagen.path || 
                imagen.imagenUrl || 
                imagen.urlImagen ||
                imagen.rutaImagen ||
                imagen.archivo ||
                null;
    
    console.log("🖼️ [Modal] URL extraída:", url);
    return url;
  }
  
  console.log("⚠️ [Modal] No se pudo extraer URL de la imagen");
  return "";
};

const handleImageError = (event, imagen) => {
  console.error("❌ [Modal] Error cargando imagen:", imagen);
  console.error("❌ [Modal] URL que falló:", event.target.src);
  console.error("❌ [Modal] Error completo:", event);
  
  // Establecer un color de fondo para indicar que la imagen no cargó
  event.target.style.backgroundColor = '#ef4444';
  event.target.style.display = 'flex';
  event.target.style.alignItems = 'center';
  event.target.style.justifyContent = 'center';
  
  // Opcional: agregar texto de error
  const parent = event.target.parentElement;
  if (parent && !parent.querySelector('.error-message')) {
    const errorDiv = document.createElement('div');
    errorDiv.className = 'error-message absolute inset-0 flex items-center justify-center text-white text-sm bg-red-500 bg-opacity-75';
    errorDiv.textContent = 'Error al cargar imagen';
    parent.appendChild(errorDiv);
  }
};

const handleImageLoad = (event, imagen) => {
  console.log("✅ [Modal] Imagen cargada exitosamente:", imagen);
  console.log("✅ [Modal] Dimensiones naturales:", event.target.naturalWidth, 'x', event.target.naturalHeight);
  console.log("✅ [Modal] URL de la imagen:", event.target.src);
};

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
  if (currentImageIndex.value < props.inmueble.imagenesReferencia.length - 1) {
    currentImageIndex.value++;
  } else {
    currentImageIndex.value = 0; // Volver al inicio
  }
};

const previousImage = () => {
  if (currentImageIndex.value > 0) {
    currentImageIndex.value--;
  } else {
    currentImageIndex.value = props.inmueble.imagenesReferencia.length - 1; // Ir al final
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