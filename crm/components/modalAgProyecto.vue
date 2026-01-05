<template>
  <div v-if="isOpen" class="modal-overlay">
    <div
      class="relative p-4 w-full max-w-6xl h-full max-h-full flex items-center justify-center"
    >
      <div
        class="relative bg-white rounded-lg shadow-sm w-full max-h-[90vh] flex flex-col"
      >
        <div
          class="flex items-center justify-between p-4 md:p-5 border-b rounded-t border-gray-200 flex-shrink-0"
        >
          <h3 class="text-lg font-semibold text-gray-900">
            {{ isEditing ? "Editar proyecto" : "Agregar Proyecto" }}
          </h3>
          <button
            type="button"
            @click="closeModal"
            class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center"
          >
            <svg
              class="w-3 h-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 14"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
              />
            </svg>
            <span class="sr-only">Close modal</span>
          </button>
        </div>

        <form
          @submit.prevent="handleSubmit"
          class="overflow-y-auto p-4 md:p-5 flex-grow"
        >
          <div class="grid gap-4 mb-4 md:grid-cols-2 lg:grid-cols-2">
            <div class="space-y-4 col-span-2 lg:col-span-1 md:col-span-1">
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Código de proyecto*</label
                >
                <input
                  v-model="form.codigoProyecto"
                  type="text"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                />
              </div>

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Título*</label
                >
                <input
                  v-model="form.titulo"
                  type="text"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                />
              </div>

              <div class="flex gap-2">
                <input
                  v-model="form.slugProyecto"
                  @input="onSlugInput"
                  type="text"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                />
                <button
                  type="button"
                  @click="regenerateSlug"
                  class="px-3 py-2 text-sm rounded-lg border border-gray-300 hover:bg-gray-100"
                  title="Regenerar desde Título"
                >
                  <svg
                    class="w-6 h-6 text-gray-800 dark:text-white"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M17.651 7.65a7.131 7.131 0 0 0-12.68 3.15M18.001 4v4h-4m-7.652 8.35a7.13 7.13 0 0 0 12.68-3.15M6 20v-4h4"
                    />
                  </svg>
                </button>
                <button
                  type="button"
                  @click="copySlugUrl"
                  class="px-3 py-2 text-sm rounded-lg border border-gray-300 hover:bg-gray-100"
                  title="Copiar URL completa"
                  :disabled="!form.slugProyecto"
                >
                  <svg
                    class="w-6 h-6 text-gray-800"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    width="24"
                    height="24"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M9 8v3a1 1 0 0 1-1 1H5m11 4h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-7a1 1 0 0 0-1 1v1m4 3v10a1 1 0 0 1-1 1H6a1 1 0 0 1-1-1v-7.13a1 1 0 0 1 .24-.65L7.7 8.35A1 1 0 0 1 8.46 8H13a1 1 0 0 1 1 1Z"
                    />
                  </svg>
                </button>
              </div>
              <p class="text-xs text-gray-500 mt-1">
                Se autogenera desde el título. Puedes editarlo para SEO.
              </p>
              <!-- <p class="text-xs text-gray-500 mt-1">
                URL: {{ fullSlugUrl || "—" }}
              </p> -->

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Tipo de proyecto*</label
                >
                <select
                  v-model="form.tipos"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                >
                  <option value="">Seleccione un tipo</option>
                  <option value="Apartamento">Apartamento</option>
                  <option value="Bodega">Bodega</option>
                  <option value="Casa">Casa</option>
                  <option value="Edificio">Edificio</option>
                  <option value="Local">Local</option>
                  <option value="Oficina">Oficina</option>
                  <option value="Terreno">Terreno</option>
                </select>
              </div>

              <!-- <div>
                <label
                  class="block mb-2 text-sm font-medium text-gray-900  "
                  >Operación*</label
                >
                <select
                  v-model="form.operaciones"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5          "
                  required
                >
                  <option value="">Seleccione una operación</option>
                  <option value="Venta">Venta</option>
                  <option value="Alquiler">Alquiler</option>
                </select>
              </div> -->

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Ubicación*</label
                >
                <select
                  v-model="form.ubicaciones"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                >
                  <option value="" disabled selected>
                    Selecciona una ubicación
                  </option>
                  <option v-for="zona in zonasActivas" :key="zona.id" :value="zona.nombre">
                    {{ zona.nombre }}
                  </option>
                </select>
              </div>
            </div>

            <div class="space-y-4">
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Precio ($)*</label
                >
                <input
                  v-model="form.precio"
                  type="number"
                  min="0"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                />
              </div>

              <!-- <div class="grid grid-cols-3 gap-2">
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Habitaciones</label
                  >
                  <input
                    v-model="form.habitaciones"
                    type="number"
                    min="0"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5          "
                  />
                </div>
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Baños</label
                  >
                  <input
                    v-model="form.banos"
                    type="number"
                    min="0"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5          "
                  />
                </div>
                <div>
                  <label
                    class="block mb-2 text-sm font-medium text-gray-900  "
                    >Parqueos</label
                  >
                  <input
                    v-model="form.parqueos"
                    type="number"
                    min="0"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5          "
                  />
                </div>
              </div> -->

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Metros cuadrados (minimo)</label
                >
                <input
                  v-model="form.metrosCuadrados"
                  type="number"
                  min="0"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                />
              </div>

              <!-- <div class="flex items-center">
                <input
                  v-model="form.luxury"
                  type="checkbox"
                  id="luxury-checkbox"
                  class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded focus:ring-gray-500   focus:ring-2    "
                />
                <label
                  for="luxury-checkbox"
                  class="ms-2 text-sm font-medium text-gray-900 "
                >
                  Propiedad de lujo
                </label>
              </div> -->
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Video URL</label
                >
                <input
                  v-model="form.video"
                  type="text"
                  min="0"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Amenidades</label
                >
                <div class="grid grid-cols-3 gap-2">
                  <div
                    v-for="amenidad in amenidadesDisponibles"
                    :key="amenidad.id"
                    class="flex items-center"
                  >
                    <input
                      :id="'amenidad-' + amenidad.id"
                      type="checkbox"
                      :value="amenidad.id"
                      v-model="form.amenidades"
                      class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded focus:ring-gray-500 focus:ring-2"
                    />
                    <label
                      :for="'amenidad-' + amenidad.id"
                      class="ms-2 text-sm font-medium text-gray-900"
                    >
                      {{ amenidad.nombre }}
                    </label>
                  </div>
                </div>
              </div>
            </div>

            <div class="col-span-2">
              <div class="space-y-4">
                <div>
                  <label class="block mb-2 text-sm font-medium text-gray-900"
                    >Descripción*</label
                  >
                  <RichTextEditor v-model="form.contenido" />
                </div>
              </div>
            </div>
          </div>

          <div class="space-y-4">
            <div class="grid lg:grid-cols-2 md:grid-cols-2 grid-cols-1 gap-5">
              <div class="mt-4">
                <label
                  for="imagenPrincipal"
                  class="block mb-2 text-sm font-medium text-gray-900"
                >
                  Selecciona la imagen principal
                </label>
                <input
                  id="imagenPrincipal"
                  type="file"
                  accept="image/*"
                  @change="handleMainImageUpload"
                  ref="mainImageInputRef"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                />
                <div v-if="mainImagePreview" class="mt-2">
                  <img
                    :src="mainImagePreview"
                    alt="Vista previa"
                    class="h-64 object-cover rounded shadow"
                  />
                </div>
              </div>

              <!-- <div class="mt-4">
                <label
                  for="imagenesReferenciaProyecto"
                  class="block mb-2 text-sm font-medium text-gray-900"
                >
                  Imágenes de Referencia
                </label>
                <input
                  id="imagenesReferenciaProyecto"
                  type="file"
                  multiple
                  accept="image/*"
                  @change="handleReferenceImagesUpload"
                  ref="referenceImagesInputRef"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                />

                <div class="flex flex-wrap gap-4 mt-4">
                  <div
                    v-for="(imagen, index) in referenceImagesPreviews"
                    :key="imagen.uniqueId || index"
                    class="relative w-22 h-22 group"
                  >
                    <img
                      :src="imagen.preview || imagen.url"
                      alt="Preview"
                      class="w-full h-full object-cover rounded-lg"
                    />

                    <button
                      @click.prevent="eliminarImagen(index)"
                      type="button"
                      class="absolute top-1 right-1 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity"
                      title="Eliminar imagen"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="h-5 w-5"
                        viewBox="0 0 20 20"
                        fill="currentColor"
                      >
                        <path
                          fill-rule="evenodd"
                          d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 14.707a1 1 0 001.414 0L11 13.414l1.293 1.293a1 1 0 001.414-1.414L12.414 12l1.293-1.293a1 1 0 00-1.414-1.414L11 10.586 9.707 9.293a1 1 0 00-1.414 1.414L9.586 12l-1.293 1.293a1 1 0 001.414 1.414z"
                          clip-rule="evenodd"
                        />
                      </svg>
                    </button>
                  </div>
                </div>
              </div> -->

              <div class="mt-4">
                <label
                  for="imagenesReferenciaProyecto"
                  class="block mb-2 text-sm font-medium text-gray-900"
                >
                  Imágenes de Referencia
                </label>
                <input
                  id="imagenesReferenciaProyecto"
                  type="file"
                  multiple
                  accept="image/*"
                  @change="handleReferenceImagesUpload"
                  ref="referenceImagesInputRef"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                />

                <draggable
                    v-model="referenceImagesPreviews"
                    tag="div"
                    :animation="200"
                    item-key="uniqueId"
                    class="flex flex-wrap gap-4 mt-4"
                    @end="onImageDragEnd"
                  >
                    <template #item="{ element: imagen, index }">
                      <div class="relative w-22 h-22 group">
                        <img
                          :src="imagen.preview || imagen.url"
                          alt="Preview"
                          class="w-full h-full object-cover rounded-lg"
                        />
                      <button
                        @click.prevent="eliminarImagen(index)"
                        type="button"
                        class="absolute top-1 right-1 bg-red-600 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity"
                        title="Eliminar imagen"
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          class="h-5 w-5"
                          viewBox="0 0 20 20"
                          fill="currentColor"
                        >
                          <path
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 14.707a1 1 0 001.414 0L11 13.414l1.293 1.293a1 1 0 001.414-1.414L12.414 12l1.293-1.293a1 1 0 00-1.414-1.414L11 10.586 9.707 9.293a1 1 0 00-1.414 1.414L9.586 12l-1.293 1.293a1 1 0 001.414 1.414z"
                            clip-rule="evenodd"
                          />
                        </svg>
                      </button>
                    </div>
                  </template>
                </draggable>
              </div>

              <!-- <div>
                <label
                  class="block mb-2 text-sm font-medium text-gray-900  "
                  >Descripción*</label
                >
                <textarea
                  v-model="form.contenido"
                  rows="4"
                  class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-gray-500 focus:border-gray-500          "
                  required
                ></textarea>
              </div> -->

              <!-- <div>
                <label
                  class="block mb-2 text-sm font-medium text-gray-900  "
                  >Amenidades</label
                >
                <div class="grid grid-cols-2 gap-2">
                  <div
                    v-for="amenidad in amenidadesDisponibles"
                    :key="amenidad.id"
                    class="flex items-center"
                  >
                    <input
                      :id="'amenidad-' + amenidad.id"
                      type="checkbox"
                      :value="amenidad.id"
                      v-model="form.amenidades"
                      class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded focus:ring-gray-500   focus:ring-2    "
                    />
                    <label
                      :for="'amenidad-' + amenidad.id"
                      class="ms-2 text-sm font-medium text-gray-900 "
                    >
                      {{ amenidad.nombre }}
                    </label>
                  </div>
                </div>
              </div> -->
            </div>
          </div>

          <button
            type="submit"
            class="text-white inline-flex items-center mt-8 bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
          >
            <svg
              class="me-1 -ms-1 w-5 h-5"
              fill="currentColor"
              viewBox="0 0 20 20"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                fill-rule="evenodd"
                d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z"
                clip-rule="evenodd"
              ></path>
            </svg>
            {{ isEditing ? "Guardar cambios" : "Agregar" }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, onMounted } from "vue";
import proyectoService from "../services/proyectoService.js";
import zonaService from "../services/zonaService.js";
import Swal from "sweetalert2";
import RichTextEditor from "./RichTextEditor.vue";
import draggable from "vuedraggable";

// === Props ===
const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
  },
  proyectoToEdit: {
    type: Object,
    default: null,
  },
});

// === Emits ===
const emit = defineEmits([
  "close",
  "proyecto-added",
  "proyecto-updated",
  "proyecto-deleted",
]);

// Amenidades disponibles
const amenidadesDisponibles = ref([
  { id: 1, nombre: "Airbnb" },
  { id: 2, nombre: "Gimnasio" },
  { id: 3, nombre: "Área verde" },
  { id: 4, nombre: "Balcón" },
  { id: 5, nombre: "Parqueo de Visitas" },
  { id: 6, nombre: "Bodega" },
  { id: 7, nombre: "Estudio" },
  { id: 8, nombre: "Jardin" },
  { id: 9, nombre: "Lavandería" },
  { id: 10, nombre: "Cuarto de Servicio" },
  { id: 11, nombre: "Área de Juegos" },
  { id: 12, nombre: "Business Center" },
  { id: 13, nombre: "Pet friendly" },
  { id: 14, nombre: "Salón Social" },
  { id: 15, nombre: "Piscina" },
  { id: 16, nombre: "Pet Garden" },
  { id: 17, nombre: "Amueblado" },
  { id: 18, nombre: "No mascotas" },
  { id: 19, nombre: "Sala familiar" },
  { id: 20, nombre: "Canchas deportivas" },
]);

const zonasActivas = ref([]);

// === Datos reactivos ===
const isEditing = ref(false);
const userEditedSlug = ref(false);

const form = ref({
  codigoProyecto: "",
  titulo: "",
  precio: 0,
  slugProyecto: "",
  metrosCuadrados: 0,
  contenido: "",
  tipos: "",
  ubicaciones: "",
  amenidades: [],
  video: "",
  imagenPrincipal: null,
  imagenesReferenciaProyecto: [],
});

const mainImagePreview = ref("");
const referenceImagesPreviews = ref([]);
const mainImageInputRef = ref(null);
const referenceImagesInputRef = ref(null);

// Contador para generar IDs únicos a cada imagen de referencia
let imageIdCounter = 0;

function extractPathFromUrl(url) {
  if (typeof url !== "string" || !url.includes("/")) return null;
  const parts = url.split("/");
  return parts[parts.length - 1];
}

/**
 * Limpia y normaliza una cadena para crear un slug.
 * @param {string} str - La cadena de texto.
 * @returns {string} El slug limpio.
 */
function slugify(str) {
  if (!str) return "";
  const cleanedStr = str
    .toString()
    .toLowerCase()
    .trim()
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/\s+/g, "-")
    .replace(/[^\w-]+/g, "")
    .replace(/--+/g, "-");
  return cleanedStr;
}

/**
 * Order imagen referenc
 */
function onImageDragEnd(event) {
  const { oldIndex, newIndex } = event;
  // Sincroniza el array de datos con el nuevo orden visual a partir de referenceImagesPreviews
  form.value.imagenesReferenciaProyecto = referenceImagesPreviews.value.map(
    (r) => form.value.imagenesReferenciaProyecto.find((f) => f.uniqueId === r.uniqueId) || r
  );
  console.log("IMÁGENES REORDENADAS:", form.value.imagenesReferenciaProyecto);
}

/**
 * Genera el slug a partir del título del formulario.
 */
function generateSlugFromTitle() {
  const newSlug = slugify(form.value.titulo);
  form.value.slugProyecto = newSlug;
}

/**
 * Resetea el formulario y las variables de estado.
 */
function resetForm() {
  form.value = {
    codigoProyecto: "",
    titulo: "",
    precio: 0,
    slugProyecto: "",
    metrosCuadrados: 0,
    contenido: "",
    tipos: "",
    ubicaciones: "",
    amenidades: [],
    video: "",
    imagenPrincipal: null,
    imagenesReferenciaProyecto: [],
  };
  userEditedSlug.value = false;
  isEditing.value = false;
  mainImagePreview.value = "";
  referenceImagesPreviews.value = [];
  if (mainImageInputRef.value) mainImageInputRef.value.value = "";
  if (referenceImagesInputRef.value) referenceImagesInputRef.value.value = "";
}

/**
 * Cierra el modal y resetea el formulario.
 */
function closeModal() {
  resetForm();
  emit("close");
}

function handleMainImageUpload(event) {
  const file = event.target.files[0];
  if (!file) return;
  const reader = new FileReader();
  reader.onload = () => {
    mainImagePreview.value = reader.result;
  };
  reader.readAsDataURL(file);
  form.value.imagenPrincipal = file;
}

function handleReferenceImagesUpload(event) {
  const files = Array.from(event.target.files);
  if (!files.length) return;
  files.forEach((file) => {
    const uniqueId = `new-${Date.now()}-${imageIdCounter++}`;
    const reader = new FileReader();
    reader.onload = () => {
      const obj = {
        uniqueId,
        preview: reader.result,
        file,
        isNew: true,
      };
      referenceImagesPreviews.value.push(obj);
      form.value.imagenesReferenciaProyecto.push(obj);
    };
    reader.readAsDataURL(file);
  });
  if (referenceImagesInputRef.value) referenceImagesInputRef.value.value = "";
}

function eliminarImagen(index) {
  Swal.fire({
    title: "¿Eliminar esta imagen del proyecto?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d",
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      const uniqueIdToRemove = referenceImagesPreviews.value[index]?.uniqueId;
      referenceImagesPreviews.value.splice(index, 1);
      form.value.imagenesReferenciaProyecto = form.value.imagenesReferenciaProyecto.filter(
        (img) => img.uniqueId !== uniqueIdToRemove
      );
      if (referenceImagesInputRef.value)
        referenceImagesInputRef.value.value = "";
    }
  });
}

// === Lógica para el slug ===
// === Lógica para el slug ===
function onSlugInput() {
  userEditedSlug.value = true;
  form.value.slugProyecto = slugify(form.value.slugProyecto);
}

function regenerateSlug() {
  userEditedSlug.value = false;
  generateSlugFromTitle();
}

// === URL completa ===
const siteOrigin = ref(
  import.meta.env.VITE_PUBLIC_SITE_ORIGIN ||
    (typeof window !== "undefined" ? window.location.origin : "") ||
    "https://tu-dominio.com"
);

const fullSlugUrl = ref("");

watch(
  () => form.value.slugProyecto,
  (slug) => {
    fullSlugUrl.value = slug
      ? `${stripTrailingSlash(siteOrigin.value)}/proyecto/${slug}`
      : "";
  },
  { immediate: true }
);

function stripTrailingSlash(str) {
  return str.endsWith("/") ? str.slice(0, -1) : str;
}

// === Copiar URL ===

async function copySlugUrl() {
  if (!fullSlugUrl.value) return;

  try {
    await navigator.clipboard.writeText(fullSlugUrl.value);
    Swal.fire({
      icon: "success",
      title: "Copiado",
      html: `<div style="font-size:14px; word-break:break-all;">
               ${fullSlugUrl.value}
             </div>`,
      timer: 2500,
      showConfirmButton: false,
    });
  } catch (err) {
    console.error("Error al copiar:", err);
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "No se pudo copiar la URL.",
    });
  }
}

// === Cargar zonas activas al montar el componente ===
onMounted(async () => {
  try {
    const response = await zonaService.getAllZonasActivas();
    if (response.success) {
      zonasActivas.value = response.data;
    }
  } catch (error) {
    console.error("Error loading zonas activas:", error);
  }
});

// === Watchers ===
// Watcher principal para el modal
watch(
  () => props.isOpen,
  (newStatus) => {
    if (!newStatus) {
      resetForm();
    }
  }
);

// Watcher para cargar datos en modo edición
watch(
  () => props.proyectoToEdit,
  (newVal) => {
    resetForm();
    if (!newVal) {
      isEditing.value = false;
      return;
    }
    isEditing.value = true;

    const amenidadesSeleccionadas = Array.isArray(newVal.amenidades?.$values)
      ? newVal.amenidades.$values.map((a) => a.nombre?.trim())
      : Array.isArray(newVal.amenidades)
      ? newVal.amenidades.map((a) => a.nombre?.trim())
      : [];

    const matchedIds = amenidadesSeleccionadas
      .map((nombre) => {
        const normalizedName = normalize(nombre);
        const match = amenidadesDisponibles.value.find(
          (a) => normalize(a.nombre) === normalizedName
        );
        return match ? match.id : null;
      })
      .filter((id) => id !== null);

    form.value = {
      ...newVal,
      amenidades: [...new Set(matchedIds)],
      slugProyecto: newVal.slugProyecto ?? "",
      precio: parseFloat(newVal.precio) || 0,
      metrosCuadrados: parseFloat(newVal.metrosCuadrados) || 0,
      contenido: newVal.contenido || "",
      tipos: newVal.tipos || "",
      ubicaciones: newVal.ubicaciones || "",
      imagenPrincipal: newVal.imagenPrincipal || null,
      // Normalizar imagenesReferenciaProyecto a un array de objetos
      imagenesReferenciaProyecto: [],
    };
    // Normalizar y construir objetos existentes con uniqueId
    const rawImgs = Array.isArray(newVal.imagenesReferenciaProyecto?.$values)
      ? newVal.imagenesReferenciaProyecto.$values
      : Array.isArray(newVal.imagenesReferenciaProyecto)
      ? newVal.imagenesReferenciaProyecto
      : [];

    console.log("[modalAgProyecto] proyectoToEdit.imagenesReferenciaProyecto (raw):", rawImgs);

    const imagenesReferenciaExistentes = rawImgs.map((it, idx) => {
      const url = typeof it === "string" ? it : it?.url || it;
      return {
        uniqueId: `existing-${idx}-${Date.now()}`,
        url,
        preview: url,
        path: extractPathFromUrl(url),
        isNew: false,
      };
    });

    form.value.imagenesReferenciaProyecto = imagenesReferenciaExistentes;
    console.log("[modalAgProyecto] Normalized imagenesReferenciaProyecto:", form.value.imagenesReferenciaProyecto);
    mainImagePreview.value = form.value.imagenPrincipal || "";
    referenceImagesPreviews.value = [...imagenesReferenciaExistentes];
    userEditedSlug.value = !!form.value.slugProyecto;
  },
  { deep: true, immediate: true }
);

// Watcher para el campo de Título, que genera el slug automáticamente.
watch(
  () => form.value.titulo,
  (newTitle) => {
    if (!userEditedSlug.value) {
      generateSlugFromTitle();
    }
  }
);

// === Lógica de envío del formulario ===
// === Lógica de envío del formulario ===
async function handleSubmit() {
  Swal.fire({
    title: isEditing.value ? "Actualizando Proyecto..." : "Creando Proyecto...",
    html: "Por favor, espere mientras se guardan los datos.",
    timerProgressBar: true,
    allowOutsideClick: false,
    didOpen: () => {
      Swal.showLoading();
    },
  });

  try {
    // 1. Manejar la imagen principal
    let mainImageUrlString = form.value.imagenPrincipal;
    if (mainImageUrlString instanceof File) {
      const uploadedMainImage = await uploadImage(mainImageUrlString);
      if (uploadedMainImage && uploadedMainImage.url) {
        mainImageUrlString = uploadedMainImage.url;
      } else {
        throw new Error("No se pudo subir la imagen principal.");
      }
    } // 2. Procesar imágenes de referencia //    a) Subir solo las nuevas imágenes (los 'File's)

    const filesToUpload = form.value.imagenesReferenciaProyecto.filter(
      (item) => item.isNew && item.file
    );
    const uploadedNewImages = await Promise.all(
      filesToUpload.map((fileObj) => uploadImage(fileObj.file, true))
    );
    const newImageUrls = uploadedNewImages
      .filter((result) => result && result.url)
      .map((result) => result.url); //    b) Reconstruir el array final de URLs respetando el orden de la UI

    let newUrlIndex = 0;
    const finalReferenceImages = referenceImagesPreviews.value.map((imgObj) => {
      if (imgObj.isNew && imgObj.file) {
        const newUrl = newImageUrls[newUrlIndex++];
        if (!newUrl) {
          throw new Error(
            "La URL de una imagen subida no se encontró. Hay una discrepancia en los datos."
          );
        }
        return { url: newUrl };
      } else {
        return { url: imgObj.url };
      }
    }); // 3. Mapear amenidades

    const amenidadesMapped = form.value.amenidades.map((id) => {
      const amenidad = amenidadesDisponibles.value.find((a) => a.id === id);
      return {
        amenidadProyectoId: amenidad?.id || id,
        nombre: amenidad?.nombre || "Amenidad desconocida",
      };
    }); // 4. Preparar los datos finales para el envío al backend

    const datosEnviar = {
      ...form.value,
      amenidades: amenidadesMapped,
      imagenPrincipal: mainImageUrlString,
      imagenesReferenciaProyecto: finalReferenceImages, // Usa el array final y ordenado
    };

    console.log("--- SUBMIT: Datos finales a enviar al backend ---");
    console.log(
      "  imagenPrincipal (string final):",
      datosEnviar.imagenPrincipal
    );
    console.log(
      "  imagenesReferenciaProyecto (array de objetos con url final):",
      datosEnviar.imagenesReferenciaProyecto
    );
    console.log("  Objetos completos (datosEnviar):", datosEnviar);
    console.log("-----------------------------------------------"); // 5. Realizar la llamada a la API (actualizar o crear)

    if (isEditing.value && props.proyectoToEdit && props.proyectoToEdit.id) {
      await proyectoService.updateProyecto(
        props.proyectoToEdit.id,
        datosEnviar
      );
      emit("proyecto-updated", datosEnviar);
    } else {
      await proyectoService.createProyecto(datosEnviar);
      emit("proyecto-added", datosEnviar);
    }

    Swal.close();
    Swal.fire(
      "✅ Guardado",
      isEditing.value
        ? "Proyecto actualizado exitosamente."
        : "Proyecto creado exitosamente.",
      "success"
    );
    closeModal();
  } catch (error) {
    console.error("❌ Error al guardar proyecto:", error);
    Swal.close();
    Swal.fire(
      "❌ Error",
      "Hubo un problema al guardar el proyecto. Por favor, revise la consola para más detalles.",
      "error"
    );
  }
}

// === Funciones de servicio ===
async function uploadImage(file, isReference = false) {
  const formData = new FormData();
  formData.append("Image", file);
  try {
    const response = await fetch(
      "https://app-pool.vylaris.online/dcmigserver/api/Upload/homes",
      {
        method: "POST",
        body: formData,
      }
    );
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`HTTP error! status: ${response.status} - ${errorText}`);
    }
    const data = await response.json();
    return {
      url: `https://app-pool.vylaris.online/dcmigserver/homes/${data.path}`,
      path: data.path,
    };
  } catch (error) {
    console.error(
      `Fallo al subir imagen ${isReference ? "de referencia" : "principal"}:`,
      error.message
    );
    return null;
  }
}

function normalize(str) {
  if (typeof str !== "string") return "";
  return str
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .trim();
}
</script>
