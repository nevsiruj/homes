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
                    @input="limpiarError('title')"
                    :class="[
                      'mt-1 block w-full rounded-md shadow-sm focus:ring-gray-500 sm:text-sm p-2 border',
                      errores.title ? 'border-red-400 focus:border-red-500' : 'border-gray-300 focus:border-gray-500'
                    ]"
                    placeholder="Título del artículo"
                  />
                  <p v-if="errores.title" class="mt-1 text-sm text-red-500">
                    {{ errores.title }}
                  </p>
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
                    @input="limpiarError('slug')"
                    :class="[
                      'mt-1 block w-full rounded-md shadow-sm focus:ring-gray-500 sm:text-sm p-2 border',
                      errores.slug ? 'border-red-400 focus:border-red-500' : 'border-gray-300 focus:border-gray-500'
                    ]"
                    placeholder="ejemplo-articulo-url"
                  />
                  <p class="mt-1 text-xs text-gray-500">
                    El slug debe ser único. Se genera automáticamente desde el título.
                  </p>
                  <p v-if="errores.slug" class="mt-1 text-sm text-red-500">
                    {{ errores.slug }}
                  </p>
                </div>

                <!-- Imagen URL -->
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-2">
                    Imagen destacada
                  </label>
                  
                  <!-- Subir Archivo -->
                  <div>
                    <input
                      type="file"
                      accept="image/*"
                      @change="handleImageUpload"
                      ref="imageInputRef"
                      class="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 focus:outline-none p-2"
                    />
                  </div>

                  <!-- Preview de imagen -->
                  <div v-if="imagenPreview" class="mt-3">
                    <img
                      :src="imagenPreview"
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

                <!-- Estado del artículo -->
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-2">
                    Estado del artículo
                  </label>
                  <button
                    type="button"
                    @click="formData.activo = !formData.activo"
                    :class="[
                      'px-4 py-2 text-sm font-medium rounded-lg border transition-colors',
                      formData.activo
                        ? 'bg-green-100 text-green-800 border-green-300'
                        : 'bg-red-100 text-red-800 border-red-300'
                    ]"
                  >
                    {{ formData.activo ? 'Publicado' : 'Borrador' }}
                  </button>
                </div>

                <!-- Contenido con RichTextEditor -->
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-2">
                    Contenido *
                  </label>
                  <div :class="errores.content ? 'ring-1 ring-red-400 rounded-md' : ''">
                    <RichTextEditor v-model="formData.content" @update:modelValue="limpiarError('content')" />
                  </div>
                  <p v-if="errores.content" class="mt-1 text-sm text-red-500">
                    {{ errores.content }}
                  </p>
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
const imagenPreview = ref(null);
const imagenArchivo = ref(null);
const imageInputRef = ref(null);

// Estado de validaciones
const errores = ref({
  title: '',
  slug: '',
  content: ''
});
const intentoGuardar = ref(false);

const formData = ref({
  title: '',
  slug: '',
  content: '',
  imageUrl: '',
  categorias: 'Informativo',
  permalink: '',
  activo: true
});

const modoEdicion = computed(() => !!props.articulo);

const generarPermalink = computed(() => {
  if (formData.value.slug) {
    const categoriaNormalizada = formData.value.categorias.toLowerCase().replace(/\s+/g, '-');
    return `https://homesguatemala.com/${categoriaNormalizada}/${formData.value.slug}/`;
  }
  return '';
});

// Declarar funciones antes de usarlas
const validarFormulario = () => {
  let esValido = true;
  errores.value = { title: '', slug: '', content: '' };
  
  // Validar título
  if (!formData.value.title || formData.value.title.trim() === '') {
    errores.value.title = 'Por favor, ingresa un título para el artículo';
    esValido = false;
  } else if (formData.value.title.trim().length < 5) {
    errores.value.title = 'El título debe tener al menos 5 caracteres';
    esValido = false;
  }
  
  // Validar slug
  if (!formData.value.slug || formData.value.slug.trim() === '') {
    errores.value.slug = 'El slug es necesario para generar la URL del artículo';
    esValido = false;
  } else if (!/^[a-z0-9]+(-[a-z0-9]+)*$/.test(formData.value.slug)) {
    errores.value.slug = 'El slug solo puede contener letras minúsculas, números y guiones';
    esValido = false;
  }
  
  // Validar contenido
  if (!formData.value.content || formData.value.content.trim() === '' || formData.value.content === '<p></p>') {
    errores.value.content = 'El artículo necesita contenido. Escribe algo interesante para tus lectores';
    esValido = false;
  } else if (formData.value.content.replace(/<[^>]*>/g, '').trim().length < 50) {
    errores.value.content = 'El contenido es muy corto. Agrega más información (mínimo 50 caracteres)';
    esValido = false;
  }
  
  return esValido;
};

const limpiarError = (campo) => {
  if (errores.value[campo]) {
    errores.value[campo] = '';
  }
};

const resetearFormulario = () => {
  formData.value = {
    title: '',
    slug: '',
    content: '',
    imageUrl: '',
    categorias: 'Informativo',
    permalink: '',
    activo: true
  };
  categoriaSeleccionada.value = 'Informativo';
  mostrarInputCategoria.value = false;
  nuevaCategoria.value = '';
  imagenPreview.value = null;
  imagenArchivo.value = null;
  errores.value = { title: '', slug: '', content: '' };
  intentoGuardar.value = false;
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
  intentoGuardar.value = true;
  
  if (!validarFormulario()) {
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
      permalink: nuevoArticulo.permalink || '',
      activo: nuevoArticulo.activo !== undefined ? nuevoArticulo.activo : true
    };
    categoriaSeleccionada.value = nuevoArticulo.categorias || 'Informativo';
    mostrarInputCategoria.value = false;
    imagenPreview.value = nuevoArticulo.imageUrl || null;
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
  if (nuevoTitulo) {
    formData.value.slug = nuevoTitulo
      .toLowerCase()
      .normalize('NFD')
      .replace(/[\u0300-\u036f]/g, '')
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/^-+|-+$/g, '');
  }
});
</script>