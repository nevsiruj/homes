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
            {{ isEditing ? "Editar inmueble" : "Agregar Inmueble" }}
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
                  >Código de propiedad*</label
                >
                <input
                  v-model="form.codigoPropiedad"
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

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Slug/URL</label
                >
                <div class="flex gap-2">
                  <input
                    v-model="form.slugInmueble"
                    @input="onSlugInput"
                    type="text"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                    required
                  />
                  <!-- Botón opcional para forzar regeneración -->
                  <button
                    type="button"
                    @click="regenerateSlug"
                    class="px-3 py-2 text-sm rounded-lg border border-gray-300 hover:bg-gray-100"
                    title="Regenerar desde Título + Código"
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
                  <!-- Botón opcional para copiar el enlace generado -->
                  <button
                    type="button"
                    @click="copySlugUrl"
                    :disabled="!form.slugInmueble"
                    class="px-3 py-2 text-sm rounded-lg border border-gray-300 hover:bg-gray-100"
                    title="Copiar URL Generada"
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
                  Se autogenera desde el título + código. Puedes editarlo para
                  SEO.
                </p>
                <!-- <p class="text-xs text-gray-500 mt-1 break-all">
  {{ fullSlugUrl || 'URL pendiente de slug...' }}
</p> -->
              </div>

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Tipo de propiedad*</label
                >
                <select
                  v-model="form.tipos"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  required
                >
                  <option value="">Seleccione un tipo</option>
                  <option value="Apartamento">Apartamento</option>
                  <option value="Casa">Casa</option>
                  <option value="Edificio">Edificio</option>
                  <option value="Local">Local</option>
                  <option value="Oficina">Oficina</option>
                  <option value="Terreno">Terreno</option>
                </select>
              </div>

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Operación*</label
                >
                <select
                  v-model="form.operaciones"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                >
                  <option value="">Seleccione una operación</option>
                  <option value="Venta">Venta</option>
                  <option value="Renta">Renta</option>
                </select>
              </div>

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
                  <option value="Amatitlán">Amatitlán</option>
                  <option value="Antigua">Antigua</option>
                  <option value="CAES Abajo KM 14">CAES Abajo KM 14</option>
                  <option value="CAES Arriba KM 14">CAES Arriba KM 14</option>
                  <option value="Carr. Salvador">Carr. Salvador</option>
                  <option value="Fraijanes">Fraijanes</option>
                  <option value="Mixco">Mixco</option>
                  <option value="Monterrico">Monterrico</option>
                  <option value="Muxbal">Muxbal</option>
                  <option value="Playa">Playa</option>
                  <option value="Puerto San José">Puerto San José</option>
                  <option value="San Cristóbal">San Cristóbal</option>
                  <option value="San José Pinula">San José Pinula</option>
                  <option value="Santa Catarina Pinula">
                    Santa Catarina Pinula
                  </option>
                  <option value="Zona 1">Zona 1</option>
                  <option value="Zona 2">Zona 2</option>
                  <option value="Zona 4">Zona 4</option>
                  <option value="Zona 7">Zona 7</option>
                  <option value="Zona 9">Zona 9</option>
                  <option value="Zona 10">Zona 10</option>
                  <option value="Zona 11">Zona 11</option>
                  <option value="Zona 12">Zona 12</option>
                  <option value="Zona 13">Zona 13</option>
                  <option value="Zona 14">Zona 14</option>
                  <option value="Zona 15">Zona 15</option>
                  <option value="Zona 16">Zona 16</option>
                </select>
              </div>
            </div>

            <div class="space-y-4 col-span-2 lg:col-span-1 md:col-span-1">
              <div class="flex items-center gap-4">
                <!-- Precio -->
                <div>
                  <label class="block mb-2 text-sm font-medium text-gray-900">
                    Precio ($)*
                  </label>
                  <input
                    v-model="form.precio"
                    type="number"
                    min="0"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 p-2.5"
                    required
                  />
                </div>

                <!-- Toggle Precio activo-->
                <div class="flex flex-col justify-center">
                  <label class="inline-flex items-center cursor-pointer mt-7">
                    <input
                      type="checkbox"
                      class="sr-only peer"
                      v-model="form.precioActivo"
                      :true-value="true"
                      :false-value="false"
                      @change="handleToggleChange"
                    />
                    <div
                      class="relative w-11 h-6 bg-gray-200 rounded-full peer peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-green-600"
                    ></div>
                    <span class="ms-3 text-sm font-medium text-gray-900">
                      Precio activo
                    </span>
                  </label>
                </div>
              </div>

              <div class="grid grid-cols-3 gap-2">
                <div>
                  <label class="block mb-2 text-sm font-medium text-gray-900"
                    >Habitaciones</label
                  >
                  <input
                    v-model="form.habitaciones"
                    type="number"
                    min="0"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  />
                </div>
                <div>
                  <label class="block mb-2 text-sm font-medium text-gray-900"
                    >Baños</label
                  >
                  <input
                    v-model="form.banos"
                    type="number"
                    min="0"
                    step="any"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  />
                </div>
                <div>
                  <label class="block mb-2 text-sm font-medium text-gray-900"
                    >Parqueos</label
                  >
                  <input
                    v-model="form.parqueos"
                    type="number"
                    min="0"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                  />
                </div>
              </div>

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Metros cuadrados (minimo)</label
                >
                <input
                  v-model="form.metrosCuadrados"
                  type="number"
                  min="0"
                  step="any"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5"
                />
              </div>

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
              <div class="flex items-center lg:mt-12 md:mt-12 mt-6">
                <input
                  v-model="form.luxury"
                  type="checkbox"
                  id="luxury-checkbox"
                  class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded focus:ring-gray-500 focus:ring-2"
                />
                <label
                  for="luxury-checkbox"
                  class="ms-2 text-sm font-medium text-gray-900"
                >
                  Propiedad de lujo
                </label>
              </div>
            </div>
            <div class="col-span-2 gap-2 mt-4 mb-4">
              <label class="block mb-2 text-sm font-medium text-gray-900"
                >Amenidades</label
              >

              <div class="grid lg:grid-cols-5 md:grid-cols-3 grid-cols-2">
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
                  for="imagenesReferencia"
                  class="block mb-2 text-sm font-medium text-gray-900"
                >
                  Imágenes de Referencia
                </label>
                <input
                  id="imagenesReferencia"
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
                    :key="index"
                    class="relative w-22 h-22 group"
                  >
                    <img
                      :src="imagen"
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
                  for="imagenesReferencia"
                  class="block mb-2 text-sm font-medium text-gray-900"
                >
                  Imágenes de Referencia
                </label>
                <button
                  v-if="referenceImagesPreviews.length"
                  type="button"
                  @click="eliminarTodasImagenes"
                  class="text-red-600 text-sm font-regular hover:underline"
                >
                  Eliminar todas
                </button>
                <input
                  id="imagenesReferencia"
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
import { ref, watch, computed, onBeforeUnmount } from "vue";
import inmuebleService from "../services/inmuebleService.js";
import Swal from "sweetalert2"; // Asegúrate de que SweetAlert2 está importado
import RichTextEditor from "./RichTextEditor.vue";
import draggable from "vuedraggable";

// === Props ===
const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
  },
  inmuebleToEdit: {
    type: Object,
    default: null,
  },
});

// === Emits ===
const emit = defineEmits([
  "close",
  "inmueble-added",
  "inmueble-updated",
  "inmueble-deleted",
]);

// Amenidades disponibles (mantener igual)
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

// === Datos reactivos ===
const isEditing = ref(false);
const userEditedSlug = ref(false);

const form = ref({
  codigoPropiedad: "",
  titulo: "",
  slugInmueble: "",
  precio: 0,
  precioActivo: false,
  habitaciones: 0,
  banos: 0,
  parqueos: 0,
  metrosCuadrados: 0,
  contenido: "",
  tipos: "",
  operaciones: "",
  ubicaciones: "",
  amenidades: [],
  luxury: false,
  video: "",
  imagenPrincipal: null, // Puede ser File o URL string
  imagenesReferencia: [], // Puede contener Files o objetos { url: string, path: string }
});

const mainImagePreview = ref("");
const referenceImagesPreviews = ref([]);

// Referencias para los inputs de tipo file (mantener igual)
const mainImageInputRef = ref(null);
const referenceImagesInputRef = ref(null);

// Contador para generar IDs únicos a cada imagen
let imageIdCounter = 0;

/**
 * Order imagen referenc - ARREGLADO: Ya no es necesario sincronizar manualmente
 * porque referenceImagesPreviews y form.value.imagenesReferencia están unidos
 */
function onImageDragEnd(event) {
  // El array ya se reordena automáticamente por v-model de draggable
  // Solo necesitamos asegurar que form.value.imagenesReferencia esté sincronizado
  form.value.imagenesReferencia = referenceImagesPreviews.value.map(preview => {
    // Buscar el objeto de imagen correspondiente por su uniqueId
    return form.value.imagenesReferencia.find(img => img.uniqueId === preview.uniqueId) || preview;
  });
  // console técnico comentado:
  // console.log("IMÁGENES REORDENADAS:", form.value.imagenesReferencia);
}

/**
 * Resetea el formulario y las variables de estado relacionadas.
 */
function resetForm() {
  form.value = {
    codigoPropiedad: "",
    titulo: "",
    precio: 0,
    slugInmueble: "",
    habitaciones: 0,
    banos: 0,
    parqueos: 0,
    metrosCuadrados: 0,
    contenido: "",
    tipos: "",
    operaciones: "",
    ubicaciones: "",
    amenidades: [],
    luxury: false,
    video: "",
    imagenPrincipal: null,
    imagenesReferencia: [],
    precioActivo: false,
  };

  userEditedSlug.value = false;

  mainImagePreview.value = "";
  referenceImagesPreviews.value = [];

  if (mainImageInputRef.value) {
    mainImageInputRef.value.value = "";
  }
  if (referenceImagesInputRef.value) {
    referenceImagesInputRef.value.value = "";
  }
}

/**
 * Cierra el modal y resetea el formulario.
 */
function closeModal() {
  resetForm();
  emit("close");
}

// === Manejo de imágenes (handleMainImageUpload, handleReferenceImagesUpload - mantener igual) ===

function handleMainImageUpload(event) {
  const file = event.target.files[0];
  if (!file) return;

  const reader = new FileReader();
  reader.onload = () => {
    mainImagePreview.value = reader.result;
  };
  reader.readAsDataURL(file);

  form.value.imagenPrincipal = file;
  // console técnico comentado:
  // console.log("IMAGEN PRINCIPAL: Nueva imagen seleccionada (File):", form.value.imagenPrincipal);
}

function handleReferenceImagesUpload(event) {
  const files = Array.from(event.target.files);
  if (!files.length) return;

  files.forEach((file) => {
    // Generar ID único para cada imagen nueva
    const uniqueId = `new-${Date.now()}-${imageIdCounter++}`;
    
    const reader = new FileReader();
    reader.onload = () => {
      // Crear objeto estructurado con ID único
      const imageObj = {
        uniqueId: uniqueId,
        preview: reader.result,
        file: file,
        isNew: true
      };
      
      referenceImagesPreviews.value.push(imageObj);
      form.value.imagenesReferencia.push(imageObj);
    };
    reader.readAsDataURL(file);
  });

  // console técnico comentado:
  // console.log("IMÁGENES REFERENCIA: Nuevas imágenes añadidas (Files):", form.value.imagenesReferencia);

  if (referenceImagesInputRef.value) {
    referenceImagesInputRef.value.value = "";
  }
}

/**
 * Elimina una imagen de referencia del formulario (solo localmente).
 * El backend se encargará de guardar solo las URLs que queden.
 * @param {number} index - El índice de la imagen a eliminar.
 */
function eliminarImagen(index) {
  Swal.fire({
    title: "¿Eliminar esta imagen del inmueble?",
    //text: "Esto solo eliminará la referencia de la imagen de este inmueble. La imagen no será borrada del servidor de almacenamiento.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d",
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      // Obtener el uniqueId de la imagen a eliminar
      const imageToRemove = referenceImagesPreviews.value[index];
      
      // Eliminar de ambos arrays usando el uniqueId
      referenceImagesPreviews.value.splice(index, 1);
      form.value.imagenesReferencia = form.value.imagenesReferencia.filter(
        img => img.uniqueId !== imageToRemove.uniqueId
      );

      // console técnico comentado:
      // console.log("ELIMINAR IMAGEN: Imagen eliminada del formulario localmente:", imageToRemove);
      // console.log("ELIMINAR IMAGEN: Estado actual de form.value.imagenesReferencia después de eliminar:", form.value.imagenesReferencia);
      // console.log("ELIMINAR IMAGEN: Estado actual de referenceImagesPreviews después de eliminar:", referenceImagesPreviews.value);

      if (referenceImagesInputRef.value) {
        referenceImagesInputRef.value.value = "";
      }

      //Swal.fire("✅ Eliminada", "La imagen fue eliminada del formulario.", "success");
    }
  });
}

// === Función para subir imágenes (uploadImage - mantener igual) ===
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
    // console técnico comentado:
    // console.log(`UPLOAD IMAGE: Respuesta del servidor al subir ${isReference ? "referencia" : "principal"}:`, data);

    return {
      url: `https://app-pool.vylaris.online/dcmigserver/homes/${data.path}`,
      path: data.path,
    };
  } catch (error) {
    // console técnico comentado:
    // console.error(`Fallo al subir imagen ${isReference ? "de referencia" : "principal"}:`, error.message);
    // No mostramos SweetAlert2 aquí directamente para evitar múltiples alertas
    // El catch de handleSubmit se encargará del error global.
    return null;
  }
}

// === Helper para extraer el path del nombre del archivo de la URL ===
function extractPathFromUrl(url) {
  // console técnico comentado:
  // console.log("extractPathFromUrl: Recibida URL:", url);
  if (typeof url !== "string" || !url.includes("/homes/")) {
    // console técnico comentado:
    // console.warn("extractPathFromUrl: URL inválida o no contiene '/homes/'. Devolviendo null. URL:", url);
    return null;
  }
  const parts = url.split("/");
  const path = parts[parts.length - 1];
  // console técnico comentado:
  // console.log("extractPathFromUrl: Path extraído:", path);
  return path;
}

// Función para crear slug limpio
function slugify(str) {
  if (!str) return "";
  return str
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "") // sin acentos
    .toLowerCase()
    .replace(/[^a-z0-9\s-]/g, "") // quita caracteres especiales
    .trim()
    .replace(/\s+/g, "-"); // espacios -> guiones
}

// Genera el slug a partir de título + código
function generateSlugFromTitleAndCode(titulo, codigo) {
  if (!titulo || !codigo) return "";
  const base = slugify(titulo);
  const code = slugify(String(codigo));
  return `${base}-${code}`;
}

// Debounce simple para no recalcular en cada tecla
let slugTimer = null;
watch(
  [() => form.value.titulo, () => form.value.codigoPropiedad],
  ([titulo, codigo]) => {
    // Evitar autogenerar cuando el usuario está editando o en modo edición
    if (isEditing.value || userEditedSlug.value) return;
    clearTimeout(slugTimer);
    slugTimer = setTimeout(() => {
      form.value.slugInmueble = generateSlugFromTitleAndCode(titulo, codigo);
    }, 250);
  }
);

// === Watcher para cargar datos en modo edición (mantener igual el mapeo) ===
function mapAmenidadesToIds(rawAmenidades) {
  const list = rawAmenidades?.$values ?? rawAmenidades ?? [];
  const byName = new Map(
    amenidadesDisponibles.value.map((a) => [normalize(a.nombre), a.id])
  );

  const ids = [];
  const notFound = [];

  for (const a of list) {
    let nombre = null;

    // Objetos { nombre, ... }
    if (a && typeof a === "object") {
      nombre = a.nombre ?? null;
    }
    // Strings (nombre directo)
    else if (typeof a === "string") {
      nombre = a;
    }

    // Ignorar números/IDs: la comparación será SOLO por nombre
    if (!nombre) continue;

    const idFromName = byName.get(normalize(nombre));
    if (idFromName) {
      ids.push(idFromName);
    } else {
      notFound.push(nombre);
    }
  }

  const unique = Array.from(new Set(ids));

  // Logs de depuración
  console.log("[Amenidades][name-only][raw]:", list);
  console.log("[Amenidades][name-only][mapped IDs]:", unique);
  if (notFound.length) {
    console.warn(
      "[Amenidades][name-only][no match]:",
      notFound,
      "Disponibles:",
      amenidadesDisponibles.value.map((x) => x.nombre)
    );
  }

  return unique;
}

watch(
  () => props.inmuebleToEdit,
  (newVal) => {
    resetForm();
    if (!newVal) {
      isEditing.value = false;
      return;
    }

    isEditing.value = true;
    // Marcar como editado para no regenerar el slug automáticamente
    userEditedSlug.value = true;

    // Unificar amenidades SOLO por nombre -> IDs locales
    const idsValidos = mapAmenidadesToIds(newVal.amenidades);

    console.log("[Amenidades][final IDs]:", idsValidos);

    // ARREGLADO: Convertir imágenes existentes a objetos con uniqueId
    const imagenesReferenciaExistentes = Array.isArray(newVal.imagenesReferencia)
      ? newVal.imagenesReferencia.map((url_string, idx) => ({
          uniqueId: `existing-${idx}-${Date.now()}`,
          url: url_string,
          path: extractPathFromUrl(url_string),
          preview: url_string,
          isNew: false
        }))
      : [];

    form.value = {
      ...newVal,
      slugInmueble: newVal.slugInmueble ?? newVal.slug ?? "",
      amenidades: idsValidos,
      precio: parseFloat(newVal.precio) || 0,
      habitaciones: newVal.habitaciones || 0,
      banos: newVal.banos || 0,
      parqueos: newVal.parqueos || 0,
      metrosCuadrados: parseFloat(newVal.metrosCuadrados) || 0,
      contenido: newVal.contenido || "",
      tipos: newVal.tipos || "",
      operaciones: newVal.operaciones || "",
      ubicaciones: newVal.ubicaciones || "",
      video: newVal.video || "",
      luxury: Boolean(newVal.luxury),
      imagenPrincipal: newVal.imagenPrincipal || null,
      precioActivo: Boolean(newVal.precioActivo ?? newVal.PrecioActivo),
      imagenesReferencia: imagenesReferenciaExistentes,
    };

    mainImagePreview.value = form.value.imagenPrincipal || "";
    // ARREGLADO: Usar el mismo array de objetos para mantener la estructura consistente
    referenceImagesPreviews.value = [...imagenesReferenciaExistentes];
  },
  { deep: true, immediate: true }
);

async function handleToggleChange() {
  // Si el inmueble no está en modo de edición o no tiene ID, no hacemos la llamada PATCH.
  if (!isEditing.value || !props.inmuebleToEdit || !props.inmuebleToEdit.id) {
    return;
  }

  try {
    await inmuebleService.togglePrecioActivo(
      props.inmuebleToEdit.id,
      form.value.precioActivo
    );

    Swal.fire({
      icon: "success",
      title: "¡Actualizado!",
      text: `El estado de "Precio activo" ha sido actualizado.`,
      timer: 1500,
      showConfirmButton: false,
    });
  } catch (error) {
    // console técnico comentado:
    // console.error("Error al cambiar el estado del toggle:", error);
    // Revierte el valor del toggle en la UI si la llamada falla.
    form.value.precioActivo = !form.value.precioActivo;
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Hubo un problema al actualizar el estado. Por favor, intente de nuevo.",
    });
  }
}

// Cuando el usuario escribe/pega en el input del slug, limpiamos y marcamos el flag
function onSlugInput(e) {
  userEditedSlug.value = true;
  // Asegurar que el input actualiza slugInmueble (no 'slug')
  form.value.slugInmueble = slugify(e.target.value);
}

// (Opcional) Botón para “Regenerar” el slug desde título + código aunque se haya editado
function regenerateSlug() {
  form.value.slugInmueble = generateSlugFromTitleAndCode(
    form.value.titulo,
    form.value.codigoPropiedad
  );
  userEditedSlug.value = true; // queda como valor “decidido”
}

function normalize(str) {
  if (typeof str !== "string") return "";
  return str
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .trim();
}

// === Función para limpiar el HTML ===

function cleanHtml(html) {
  if (!html) return "";

  // Usá el DOM para no romper la semántica
  const doc = new DOMParser().parseFromString(html, "text/html");

  // 1) Normalizar espacios en NODOS DE TEXTO (no dentro de etiquetas)
  doc.body.querySelectorAll("*").forEach((el) => {
    Array.from(el.childNodes).forEach((node) => {
      if (node.nodeType === Node.TEXT_NODE) {
        node.nodeValue = node.nodeValue
          .replace(/\u00A0/g, " ") // &nbsp; -> espacio
          .replace(/ {2,}/g, " "); // colapsar dobles espacios
      }
    });
  });

  // 2) <p> vacíos -> <br>, PERO NO si están dentro de <li>
  doc.body.querySelectorAll("p").forEach((p) => {
    if (p.closest("li")) return; // no tocar párrafos dentro de listas
    if (!p.textContent.trim()) {
      const br = doc.createElement("br");
      p.replaceWith(br);
    }
  });

  // 3) Colapsar <br> repetidos
  doc.body.innerHTML = doc.body.innerHTML
    .replace(/(?:\s*<br\s*\/?>\s*){2,}/gi, "<br>")
    .replace(/>\s+</g, "><"); // quitar gaps entre tags

  return doc.body.innerHTML;
}

function prepareForEditor(html) {
  if (!html) return "";

  // 1) Normalización básica de espacios (solo texto)
  html = html.replace(/\u00A0/g, " ");

  // 2) Quitá <br> que están ENTRE bloques (P, UL, OL, DIV, H1..H6, BLOCKQUOTE, TABLE)
  html = html
    .replace(/<\/p>\s*<br\s*\/?>\s*<p>/gi, "</p><p>")
    .replace(/<\/(ul|ol)>\s*<br\s*\/?>\s*<p/gi, "</$1><p")
    .replace(/<\/p>\s*<br\s*\/?>\s*<(ul|ol)\b/gi, "</p><$1")
    .replace(
      /<\/(ul|ol)>\s*<br\s*\/?>\s*<(h[1-6]|p|div|blockquote|table)\b/gi,
      "</$1><$2"
    )
    .replace(
      /<\/(div|h[1-6]|blockquote|table)>\s*<br\s*\/?>\s*<(div|h[1-6]|p|blockquote|table)\b/gi,
      "</$1><$2"
    );

  // 3) Dentro de P, colapsá <br> repetidos
  html = html.replace(
    /(<p[^>]*>[\s\S]*?)((?:<br\s*\/?>\s*){2,})/gi,
    (_m, head) => head + "<br>"
  );

  return html;
}

// === Lógica de envío del formulario ===
async function handleSubmit() {
  let timerInterval;
  Swal.fire({
    title: isEditing.value ? "Actualizando Inmueble..." : "Creando Inmueble...",
    html: "Por favor, espere mientras se guardan los datos.",
    timerProgressBar: true,
    allowOutsideClick: false,
    didOpen: () => {
      Swal.showLoading();
    },
    willClose: () => {
      clearInterval(timerInterval);
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
    }
    
    // 2. ARREGLADO: Procesar imágenes de referencia respetando el orden exacto
    // Usar referenceImagesPreviews como fuente de verdad para el orden
    const finalReferenceImages = [];
    
    for (const imageObj of referenceImagesPreviews.value) {
      if (imageObj.isNew && imageObj.file) {
        // Es una imagen nueva que necesita subirse
        const uploadResult = await uploadImage(imageObj.file, true);
        if (uploadResult && uploadResult.url) {
          finalReferenceImages.push({ url: uploadResult.url });
        } else {
          throw new Error(`No se pudo subir una de las imágenes de referencia.`);
        }
      } else if (imageObj.url) {
        // Es una imagen existente, mantener su URL
        finalReferenceImages.push({ url: imageObj.url });
      }
    }
    
    // 3. Mapear amenidades

    const amenidadesMapped = form.value.amenidades.map((id) => {
      const amenidad = amenidadesDisponibles.value.find((a) => a.id === id);
      return {
        amenidadInmuebleId: amenidad?.id || id,
        nombre: amenidad?.nombre || "Amenidad desconocida",
      };
    }); // 4. Preparar los datos finales para el envío al backend

    const datosEnviar = {
      ...form.value,
      slugInmueble: form.value.slugInmueble ?? form.value.slug ?? "",
      contenido: form.value.contenido,
      amenidades: amenidadesMapped,
      imagenPrincipal: mainImageUrlString,
      imagenesReferencia: finalReferenceImages, // Usa el array final y ordenado
      precioActivo: Boolean(form.value.precioActivo),
    }; // console técnico comentado: // console.log("--- SUBMIT: Datos finales a enviar al backend ---"); // console.log("  imagenPrincipal (string final):", datosEnviar.imagenPrincipal); // console.log("  imagenesReferencia (array de objetos con url final):", datosEnviar.imagenesReferencia); // console.log("  Objetos completos (datosEnviar):", datosEnviar); // console.log("-----------------------------------------------"); // 5. Realizar la llamada a la API (actualizar o crear)

    if (isEditing.value && props.inmuebleToEdit && props.inmuebleToEdit.id) {
      await inmuebleService.updateInmueble(
        props.inmuebleToEdit.id,
        datosEnviar
      );
      emit("inmueble-updated", datosEnviar);
    } else {
      await inmuebleService.createInmueble(datosEnviar);
      emit("inmueble-added", datosEnviar);
    }

    Swal.close();
    Swal.fire(
      "✅ Guardado",
      isEditing.value
        ? "Inmueble actualizado exitosamente."
        : "Inmueble creado exitosamente.",
      "success"
    );

    closeModal();
  } catch (error) {
    // console técnico comentado:
    // console.error("❌ Error al guardar inmueble:", error);
    Swal.close();
    Swal.fire(
      "❌ Error",
      "Hubo un problema al guardar el inmueble. Por favor, verifique e intente de nuevo.",
      "error"
    );
  }
}

//Elimina todas las imágenes de referencia
function eliminarTodasImagenes() {
  Swal.fire({
    title: "¿Eliminar todas las imágenes de referencia?",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d",
    confirmButtonText: "Sí, eliminar todas",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      form.value.imagenesReferencia = [];
      referenceImagesPreviews.value = [];
      if (referenceImagesInputRef.value) {
        referenceImagesInputRef.value.value = "";
      }
      // console técnico comentado:
      // console.log("✅ Todas las imágenes de referencia fueron eliminadas");
    }
  });
}

// Base del sitio (elige el orden de prioridad que prefieras)
const siteOrigin = ref(
  // 1) variable de entorno si la tenés definida
  import.meta.env.VITE_PUBLIC_SITE_ORIGIN ||
    // 2) si corrés en navegador, usa el origin actual
    (typeof window !== "undefined" ? window.location.origin : "") ||
    // 3) fallback por si acaso
    "https://tu-dominio.com"
);

// Util para evitar dobles slashes
function stripTrailingSlash(u) {
  return (u || "").replace(/\/+$/, "");
}

// URL completa calculada: https://dominio/inmueble/slug
const fullSlugUrl = ref(""); // <- variable que pediste “guardada”
const generatedUrl = computed(() => {
  const base = stripTrailingSlash(siteOrigin.value);
  const slug = form.value.slugInmueble || "";
  return slug ? `${base}/inmueble/${slug}` : "";
});

// Mantener `fullSlugUrl` siempre al día cuando cambia el slug
watch(
  () => form.value.slugInmueble,
  () => {
    fullSlugUrl.value = generatedUrl.value;
  },
  { immediate: true }
);

// Copiar al portapapeles con fallback
async function copySlugUrl() {
  try {
    const url = generatedUrl.value;
    if (!url) {
      Swal.fire("Atención", "Genera o ingresa un slug primero.", "info");
      return;
    }

    if (navigator.clipboard?.writeText) {
      await navigator.clipboard.writeText(url);
    } else {
      // Fallback para navegadores sin Clipboard API
      const ta = document.createElement("textarea");
      ta.value = url;
      document.body.appendChild(ta);
      ta.select();
      document.execCommand("copy");
      document.body.removeChild(ta);
    }

    Swal.fire({
      icon: "success",
      title: "URL copiada",
      text: url,
      timer: 1500,
      showConfirmButton: false,
    });
  } catch (e) {
    // console técnico comentado:
    // console.error("No se pudo copiar la URL:", e);
    Swal.fire("Error", "No se pudo copiar la URL al portapapeles.", "error");
  }
}
</script>
