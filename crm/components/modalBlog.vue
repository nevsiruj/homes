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
          @click="cerrarModal"
        ></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <!-- Modal -->
        <div
          class="relative inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-4xl sm:w-full z-50"
        >
        <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div class="sm:flex sm:items-start">
            <div class="w-full">
              <h3 class="text-lg leading-6 font-medium text-gray-900 mb-4" id="modal-title">
                {{ modoEdicion ? 'Editar Artículo' : 'Agregar Nuevo Artículo' }}
              </h3>

              <form @submit.prevent="guardarArticulo" class="space-y-4">
                <!-- Título -->
                <div>
                  <label for="title" class="block text-sm font-medium text-gray-700">
                    Título *
                  </label>
                  <input
                    v-model="formData.title"
                    type="text"
                    id="title"
                    required
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-gray-500 focus:ring-gray-500 sm:text-sm p-2 border"
                    placeholder="Título del artículo"
                  />
                </div>

                <!-- Slug -->
                <div>
                  <label for="slug" class="block text-sm font-medium text-gray-700">
                    Slug (URL amigable) *
                  </label>
                  <input
                    v-model="formData.slug"
                    type="text"
                    id="slug"
                    required
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-gray-500 focus:ring-gray-500 sm:text-sm p-2 border"
                    placeholder="ejemplo-articulo-url"
                  />
                </div>

                <!-- Imagen URL -->
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-2">
                    Imagen destacada
                  </label>
                  
                  <!-- Tabs para elegir entre URL o Archivo -->
                  <div class="flex gap-2 mb-3">
                    <button
                      type="button"
                      @click="imagenTipo = 'url'"
                      :class="[
                        'px-4 py-2 text-sm rounded-lg border transition-colors',
                        imagenTipo === 'url'
                          ? 'bg-gray-700 text-white border-gray-700'
                          : 'bg-white text-gray-700 border-gray-300 hover:bg-gray-50'
                      ]"
                    >
                      URL de Imagen
                    </button>
                    <button
                      type="button"
                      @click="imagenTipo = 'archivo'"
                      :class="[
                        'px-4 py-2 text-sm rounded-lg border transition-colors',
                        imagenTipo === 'archivo'
                          ? 'bg-gray-700 text-white border-gray-700'
                          : 'bg-white text-gray-700 border-gray-300 hover:bg-gray-50'
                      ]"
                    >
                      Cargar Archivo
                    </button>
                  </div>

                  <!-- Input URL -->
                  <div v-if="imagenTipo === 'url'">
                    <input
                      v-model="formData.imageUrl"
                      type="url"
                      placeholder="https://ejemplo.com/imagen.jpg"
                      class="block w-full rounded-md border-gray-300 shadow-sm focus:border-gray-500 focus:ring-gray-500 sm:text-sm p-2 border"
                    />
                  </div>

                  <!-- Input File -->
                  <div v-else>
                    <input
                      type="file"
                      accept="image/*"
                      @change="handleImageUpload"
                      ref="imageInputRef"
                      class="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 focus:outline-none p-2"
                    />
                  </div>

                  <!-- Preview de imagen -->
                  <div v-if="imagenPreview || formData.imageUrl" class="mt-3">
                    <img
                      :src="imagenPreview || formData.imageUrl"
                      alt="Preview"
                      class="h-40 w-auto rounded-md object-cover shadow-md"
                      @error="imagenError"
                    />
                  </div>
                </div>

                <!-- Categorías -->
                <div>
                  <label for="categorias" class="block text-sm font-medium text-gray-700">
                    Categorías
                  </label>
                  <select
                    v-model="categoriaSeleccionada"
                    @change="handleCategoriaChange"
                    id="categorias"
                    class="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-gray-500 focus:ring-gray-500 sm:text-sm p-2 border"
                  >
                    <option value="Informativo">Informativo</option>
                    <option value="Guías">Guías</option>
                    <option value="Mercado">Mercado</option>
                    <option value="Inversión">Inversión</option>
                    <option value="Tendencias">Tendencias</option>
                    <option value="Noticias">Noticias</option>
                    <option value="__otra__">+ Agregar nueva categoría</option>
                  </select>
                  
                  <!-- Input para nueva categoría -->
                  <div v-if="mostrarInputCategoria" class="mt-2">
                    <input
                      v-model="nuevaCategoria"
                      type="text"
                      placeholder="Escribe la nueva categoría"
                      class="block w-full rounded-md border-gray-300 shadow-sm focus:border-gray-500 focus:ring-gray-500 sm:text-sm p-2 border"
                      @blur="agregarCategoria"
                      @keyup.enter="agregarCategoria"
                    />
                    <p class="mt-1 text-xs text-gray-500">Presiona Enter o haz clic fuera para agregar</p>
                  </div>
                </div>

                <!-- Contenido con RichTextEditor -->
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-2">
                    Contenido *
                  </label>
                  <RichTextEditor v-model="formData.content" />
                </div>

                <!-- Permalink (solo lectura) -->
                <div>
                  <label for="permalink" class="block text-sm font-medium text-gray-700">
                    Permalink (generado automáticamente)
                  </label>
                  <input
                    :value="generarPermalink"
                    type="text"
                    id="permalink"
                    readonly
                    class="mt-1 block w-full rounded-md border-gray-300 bg-gray-50 shadow-sm sm:text-sm p-2 border"
                  />
                </div>
              </form>
            </div>
          </div>
        </div>

        <!-- Botones -->
        <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse gap-2">
          <button
            type="button"
            @click="guardarArticulo"
            :disabled="guardando"
            class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-gray-700 text-base font-medium text-white hover:bg-gray-800 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 sm:ml-3 sm:w-auto sm:text-sm disabled:opacity-50"
          >
            {{ guardando ? 'Guardando...' : (modoEdicion ? 'Actualizar' : 'Guardar') }}
          </button>
          <button
            type="button"
            @click="cerrarModal"
            :disabled="guardando"
            class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 sm:mt-0 sm:w-auto sm:text-sm"
          >
            Cancelar
          </button>
        </div>
      </div>
    </div>
    </div>
  </Teleport>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import RichTextEditor from './RichTextEditor.vue';

const props = defineProps({
  isOpen: {
    type: Boolean,
    default: false
  },
  articulo: {
    type: Object,
    default: null
  }
});

const emit = defineEmits(['close', 'save']);

const guardando = ref(false);
const categoriaSeleccionada = ref('Informativo');
const mostrarInputCategoria = ref(false);
const nuevaCategoria = ref('');
const imagenTipo = ref('url');
const imagenPreview = ref(null);
const imagenArchivo = ref(null);
const imageInputRef = ref(null);

const formData = ref({
  title: '',
  slug: '',
  content: '',
  imageUrl: '',
  categorias: 'Informativo',
  permalink: ''
});

const modoEdicion = computed(() => !!props.articulo);

const generarPermalink = computed(() => {
  if (formData.value.slug) {
    return `https://homesguatemala.com/informativo/${formData.value.slug}/`;
  }
  return '';
});

// Declarar funciones antes de usarlas
const resetearFormulario = () => {
  formData.value = {
    title: '',
    slug: '',
    content: '',
    imageUrl: '',
    categorias: 'Informativo',
    permalink: ''
  };
  categoriaSeleccionada.value = 'Informativo';
  mostrarInputCategoria.value = false;
  nuevaCategoria.value = '';
  imagenTipo.value = 'url';
  imagenPreview.value = null;
  imagenArchivo.value = null;
  if (imageInputRef.value) {
    imageInputRef.value.value = '';
  }
};

const imagenError = (e) => {
  e.target.style.display = 'none';
};

const handleImageUpload = (event) => {
  const file = event.target.files[0];
  if (file) {
    imagenArchivo.value = file;
    const reader = new FileReader();
    reader.onload = (e) => {
      imagenPreview.value = e.target.result;
      formData.value.imageUrl = ''; // Limpiar URL si se carga archivo
    };
    reader.readAsDataURL(file);
  }
};

const uploadImage = async (file) => {
  const formDataUpload = new FormData();
  formDataUpload.append('Image', file);
  
  try {
    const response = await fetch(
      'https://app-pool.vylaris.online/dcmigserver/api/Upload/homes',
      {
        method: 'POST',
        body: formDataUpload,
      }
    );
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    
    const data = await response.json();
    return {
      url: `https://app-pool.vylaris.online/dcmigserver/homes/${data.path}`,
      path: data.path,
    };
  } catch (error) {
    console.error('Error al subir imagen:', error);
    return null;
  }
};

const handleCategoriaChange = () => {
  if (categoriaSeleccionada.value === '__otra__') {
    mostrarInputCategoria.value = true;
    nuevaCategoria.value = '';
  } else {
    mostrarInputCategoria.value = false;
    formData.value.categorias = categoriaSeleccionada.value;
  }
};

const agregarCategoria = () => {
  if (nuevaCategoria.value.trim()) {
    formData.value.categorias = nuevaCategoria.value.trim();
    categoriaSeleccionada.value = nuevaCategoria.value.trim();
    mostrarInputCategoria.value = false;
  } else {
    categoriaSeleccionada.value = 'Informativo';
    formData.value.categorias = 'Informativo';
    mostrarInputCategoria.value = false;
  }
};

const guardarArticulo = async () => {
  if (!formData.value.title || !formData.value.slug || !formData.value.content) {
    return;
  }

  guardando.value = true;
  
  try {
    let imageUrl = formData.value.imageUrl;
    
    // Si hay un archivo de imagen seleccionado, subirlo primero
    if (imagenArchivo.value) {
      const uploadResult = await uploadImage(imagenArchivo.value);
      if (uploadResult && uploadResult.url) {
        imageUrl = uploadResult.url;
      }
    }
    
    const datosArticulo = {
      ...formData.value,
      imageUrl: imageUrl,
      permalink: generarPermalink.value
    };

    emit('save', datosArticulo);
  } catch (error) {
    console.error('Error al guardar artículo:', error);
  } finally {
    setTimeout(() => {
      guardando.value = false;
    }, 500);
  }
};

const cerrarModal = () => {
  if (!guardando.value) {
    resetearFormulario();
    emit('close');
  }
};

// Watchers después de las declaraciones
watch(() => props.articulo, (nuevoArticulo) => {
  if (nuevoArticulo) {
    formData.value = {
      title: nuevoArticulo.title || '',
      slug: nuevoArticulo.slug || '',
      content: nuevoArticulo.content || '',
      imageUrl: nuevoArticulo.imageUrl || '',
      categorias: nuevoArticulo.categorias || 'Informativo',
      permalink: nuevoArticulo.permalink || ''
    };
    categoriaSeleccionada.value = nuevoArticulo.categorias || 'Informativo';
    mostrarInputCategoria.value = false;
    imagenTipo.value = 'url';
    imagenPreview.value = null;
    imagenArchivo.value = null;
    if (imageInputRef.value) {
      imageInputRef.value.value = '';
    }
  } else {
    resetearFormulario();
  }
}, { immediate: true });

// Auto-generar slug desde el título
watch(() => formData.value.title, (nuevoTitulo) => {
  if (nuevoTitulo && !modoEdicion.value) {
    formData.value.slug = nuevoTitulo
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/^-+|-+$/g, '');
  }
});
</script>