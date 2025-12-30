<template>
  <div>
    <Headerb />
    <section class="text-gray-600 body-font">
      <div class="container px-5 py-12 mx-auto">
        <div class="flex flex-col text-center w-full">
          <h1 class="text-3xl md:text-4xl lg:text-5xl title-alta text-gray-900 mb-8 uppercase">
            Búsqueda Avanzada
          </h1>
          <FiltroBusquedaAvanzada @filter-applied="handleFilterAppliedFromAdvancedFilter"
            :initialFilters="currentAppliedFilters" />
          <hr class="w-48 h-1 mx-auto my-2 bg-gray-100 border-0 rounded-sm md:my-10" />
        </div>

        <!-- <div v-if="loading" class="flex justify-center items-center py-20">
          <div class="text-xl font-semibold text-grey-700">
            Cargando inmuebles...
          </div>
        </div> -->

        <div v-if="loading" class="flex flex-wrap -m-4 mt-10 justify-center">
          <Skeleton v-for="n in 9" :key="n" />
        </div>

        <div v-else-if="error" class="flex justify-center items-center py-20">
          <div class="text-red-600 text-xl font-semibold p-4 border border-red-300 rounded-md bg-red-50">
            Error al cargar los inmuebles: {{ error }}
            <p class="text-sm mt-2">
              Por favor, asegúrate de que tu backend esté funcionando
              correctamente.
            </p>
          </div>
        </div>

        <div v-else>
          <div v-if="inmuebles.length === 0" class="text-center text-gray-600 text-2xl py-10">
            No hay inmuebles disponibles para mostrar con los criterios
            seleccionados.
          </div>
          <div v-else>
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
              <InmuebleCard v-for="inmueble in inmuebles" :key="inmueble.id" :inmueble="inmueble" />
            </div>

            <div v-if="totalPages > 1" class="flex justify-center py-12">
              <nav aria-label="Page navigation" class="flex justify-center">
                <ul
                  class="inline-flex items-center -space-x-px text-base h-10 rounded-lg overflow-hidden border border-gray-300 shadow-sm">
                  <li>
                    <button @click="goToPrevPage" :disabled="currentPage === 1" :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white border-r border-gray-300 rounded-s-lg transition-colors duration-200',
                      'hover:bg-black hover:text-white ',
                      { 'opacity-50 cursor-not-allowed': currentPage === 1 },
                    ]">
                      Anterior
                    </button>
                  </li>
                  <li v-for="number in pageNumbers" :key="number">
                    <button @click="paginate(number)" :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight border-r border-gray-300 transition-colors duration-200',
                      {
                        'text-white bg-black font-bold border-gray-800 ':
                          currentPage === number,
                      },
                      {
                        'text-black bg-white hover:bg-black hover:text-white   ':
                          currentPage !== number,
                      },
                    ]" :aria-current="currentPage === number ? 'page' : undefined
                        ">
                      {{ number }}
                    </button>
                  </li>
                  <li>
                    <button @click="goToNextPage" :disabled="currentPage === totalPages" :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white rounded-e-lg transition-colors duration-200',
                      'hover:bg-black hover:text-white ',
                      {
                        'opacity-50 cursor-not-allowed':
                          currentPage === totalPages,
                      },
                    ]">
                      Siguiente
                    </button>
                  </li>
                </ul>
              </nav>
            </div>
          </div>
        </div>
      </div>
    </section>
    <RedesFlotantes />
    <Footer />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from "vue";
import { useRoute, useRouter } from "vue-router"; // Importa useRouter para manipular la URL
import Headerb from "../../components/headerb.vue";
import Footer from "../../components/footer.vue";
import { Swiper, SwiperSlide } from "swiper/vue";
import inmuebleService from "../../services/inmuebleService";
import { initFlowbite } from "flowbite";
import FiltroBusquedaAvanzada from "../../components/filtroBusquedaAvanzada.vue"; // Ajusta la ruta si es necesario

import InmuebleCard from '~/components/InmuebleCard.vue';
import skeleton from "~/components/skeleton.vue";

import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/navigation";

import { Autoplay, Pagination, Navigation } from "swiper/modules";

const route = useRoute();
const router = useRouter(); // Instancia del router para la navegación

const inmuebles = ref([]); // Ahora solo contendrá los inmuebles de la página actual filtrados por el backend
const loading = ref(true);
const error = ref(null);
const currentPage = ref(1);
const pageSize = ref(6); // Usamos 'pageSize' para coincidir con la convención del backend
const totalPages = ref(1);
const totalCount = ref(0); // Para el total de elementos, si tu API lo proporciona

// `currentAppliedFilters` será el objeto de filtros que se envía al backend.
// Se inicializará con los parámetros de la URL y luego se combinará con los filtros del formulario.
const currentAppliedFilters = ref({});

const modules = [Autoplay, Pagination, Navigation];

const pageNumbers = computed(() => {
  if (totalPages.value <= 1) return [];

  const pageRange = 2; // Muestra 2 números antes y 2 después de la página actual
  const numbers = [];
  
  for (let i = 1; i <= totalPages.value; i++) {
    // Siempre muestra la primera, la última y las páginas cercanas a la actual
    if (
      i === 1 ||
      i === totalPages.value ||
      (i >= currentPage.value - pageRange && i <= currentPage.value + pageRange)
    ) {
      numbers.push(i);
    }
  }

  // Añade "..." en los espacios vacíos
  const finalNumbers = [];
  let prevNum = 0;
  for (const num of numbers) {
    if (prevNum) {
      if (num - prevNum > 1) {
        finalNumbers.push('...');
      }
    }
    finalNumbers.push(num);
    prevNum = num;
  }
  return finalNumbers;
});

/**
 * Normaliza los datos de un inmueble.
 * Asume que `imagenesReferencia` y `amenidades` pueden venir como objetos con `$values`.
 * También normaliza los nombres de las propiedades para facilitar el acceso.
 * @param {object} inmueble - El objeto inmueble a normalizar.
 * @returns {object} El inmueble normalizado.
 */
const normalizeInmuebleData = (inmueble) => {
  const normalizedInmueble = { ...inmueble };

  if (normalizedInmueble.imagenesReferencia && normalizedInmueble.imagenesReferencia.$values) {
    normalizedInmueble.imagenesReferencia = normalizedInmueble.imagenesReferencia.$values;
  } else if (!normalizedInmueble.imagenesReferencia) {
    normalizedInmueble.imagenesReferencia = [];
  }

  if (normalizedInmueble.amenidades && normalizedInmueble.amenidades.$values) {
    normalizedInmueble.amenidades = normalizedInmueble.amenidades.$values;
  } else if (!normalizedInmueble.amenidades) {
    normalizedInmueble.amenidades = [];
  }

  // Normalizar los nombres de las propiedades (asegúrate de que coincidan con tus props en C#)
  // Por ejemplo, si tu backend envía 'Operaciones' con 'O' mayúscula, y tu frontend espera 'operaciones' minúscula
  normalizedInmueble.operaciones = inmueble.Operaciones || inmueble.operaciones;
  normalizedInmueble.tipos = inmueble.Tipos || inmueble.tipos;
  normalizedInmueble.ubicaciones = inmueble.Ubicaciones || inmueble.ubicaciones;
  normalizedInmueble.habitaciones = inmueble.Habitaciones || inmueble.habitaciones;
  normalizedInmueble.precio = inmueble.Precio || inmueble.precio;
  normalizedInmueble.titulo = inmueble.Titulo || inmueble.titulo;
  normalizedInmueble.contenido = inmueble.Contenido || inmueble.contenido;
  normalizedInmueble.codigoPropiedad = inmueble.CodigoPropiedad || inmueble.codigoPropiedad;
  normalizedInmueble.banos = inmueble.Banos || inmueble.banos;
  // Agrega más propiedades si es necesario

  return normalizedInmueble;
};


/**
 * Carga los inmuebles desde el servicio con los filtros y la paginación actuales.
 */
const fetchInmuebles = async () => {
  loading.value = true;
  error.value = null;
  try {
    // console.log("Enviando filtros al backend:", currentAppliedFilters.value); // Deshabilitado para producción

    const result = await inmuebleService.getInmueblesPaginados(
      currentPage.value,
      pageSize.value,
      currentAppliedFilters.value
    );

    if (result && Array.isArray(result.items)) {
      // console.log("Resultados recibidos del backend:", result.items); // Deshabilitado para producción
      inmuebles.value = result.items.map(normalizeInmuebleData);
      totalPages.value = result.totalPages || 1;
      totalCount.value = result.totalCount || 0;
      currentPage.value = result.pageNumber || 1;
    } else {
      inmuebles.value = [];
      totalPages.value = 0;
      totalCount.value = 0;
    }
  } catch (err) {
    // console.error("Error al cargar inmuebles en busqueda.vue:", err); // Deshabilitado para producción
    error.value = "Hubo un problema al cargar las propiedades.";
    inmuebles.value = [];
    totalPages.value = 0;
    totalCount.value = 0;
  } finally {
    loading.value = false;
  }
};

/**
 * Procesa los query parameters de la URL y los mapea a los filtros esperados.
 * Esto asegura que los filtros del menú o de una URL compartida se apliquen.
 * Los nombres de los parámetros en la URL (ej. 'Operaciones', 'Tipos') deben coincidir con tu DTO de backend.
 */
const parseUrlFilters = (query) => {
  const filters = {};

  // Mapea los query params del router (ej. route.query.Operaciones) 
  // a los nombres de los filtros que tu backend espera (ej. { Operaciones: 'Venta' })
  // y que tu componente FiltroBusquedaAvanzada pueda entender.

  if (query.Operaciones) { // Viene del menú (Venta/Renta)
    filters.Operaciones = query.Operaciones;
  }
  if (query.Tipos) { // Si envías Tipos desde la URL, conviértelo a array si no lo es
    filters.Tipos = Array.isArray(query.Tipos) ? query.Tipos : [query.Tipos];
  }
  if (query.Ubicaciones) {
    filters.Ubicaciones = Array.isArray(query.Ubicaciones) ? query.Ubicaciones : [query.Ubicaciones];
  }
  if (query.Habitaciones) {
    filters.Habitaciones = Array.isArray(query.Habitaciones) ? query.Habitaciones.map(Number) : [Number(query.Habitaciones)];
  }
  if (query.PrecioMinimo) {
    filters.PrecioMinimo = Number(query.PrecioMinimo);
  }
  if (query.PrecioMaximo) {
    filters.PrecioMaximo = Number(query.PrecioMaximo);
  }
  if (query.Titulo) { // Asumo que 'Nombre' en la URL corresponde a 'Titulo' o 'Contenido' en el backend
    filters.Titulo = query.Titulo; // Ajusta si tu DTO usa un nombre diferente
  }
  if (query.CodigoPropiedad) {
    filters.CodigoPropiedad = query.CodigoPropiedad;
  }
  if (query.Amenidades) {
        filters.Amenidades = Array.isArray(query.Amenidades) ? query.Amenidades : [query.Amenidades];
    }
  if (query.SearchTerm) {
    filters.SearchTerm = query.SearchTerm; // Asegúrate de incluir el filtro SearchTerm
  }

  // También maneja el `PageNumber` de la URL para la paginación
  if (query.PageNumber) {
    currentPage.value = parseInt(query.PageNumber);
  } else {
    currentPage.value = 1; // Por defecto a la página 1 si no está en la URL
  }

  return filters;
};


/**
 * Maneja los filtros aplicados desde el componente `FiltroBusquedaAvanzada`.
 * Estos filtros deben fusionarse o reemplazar los filtros de la URL.
 * @param {object} filtersFromAdvanced - Los filtros emitidos por el componente de filtro.
 */
const handleFilterAppliedFromAdvancedFilter = (filtersFromAdvanced) => {
  //console.log("Filtros recibidos del componente avanzado:", filtersFromAdvanced);
  // Reemplaza los filtros actuales con los que vienen del formulario avanzado.
  // Esto sobrescribe cualquier filtro previo de la URL si el usuario usa el formulario.
  currentAppliedFilters.value = { ...filtersFromAdvanced };
  currentPage.value = 1; // Siempre reiniciar a la primera página con nuevos filtros

  // Actualiza la URL para reflejar los nuevos filtros aplicados por el formulario.
  // Esto es crucial para la capacidad de compartir la URL.
  router.push({ query: { ...currentAppliedFilters.value, PageNumber: currentPage.value } });
};


// Observa los cambios en `route.query` (los parámetros de la URL)
watch(
  () => route.query,
  (newQuery) => {
    //console.log("Cambio detectado en los query parameters de la URL:", newQuery);
    // Parsear los filtros de la URL y actualizar `currentAppliedFilters`
    currentAppliedFilters.value = parseUrlFilters(newQuery);
    fetchInmuebles(); // Vuelve a cargar los inmuebles con los filtros actualizados
  },
  { deep: true, immediate: true } // `immediate: true` para que se ejecute al montar el componente
);

// Paginación
const paginate = (pageNumber) => {
  if (typeof pageNumber !== 'number') return;

  if (pageNumber > 0 && pageNumber <= totalPages.value) {
    currentPage.value = pageNumber;
    router.push({ query: { ...route.query, PageNumber: pageNumber } });
  }
};

const goToPrevPage = () => {
  if (currentPage.value > 1) {
    paginate(currentPage.value - 1);
  }
};

const goToNextPage = () => {
  if (currentPage.value < totalPages.value) {
    paginate(currentPage.value + 1);
  }
};

// onMounted solo para inicializar Flowbite y asegurar que los filtros se carguen.
onMounted(() => {
  initFlowbite();
  // `fetchInmuebles` y `parseUrlFilters` ya son llamados por el watcher con `immediate: true`
});
</script>

<style scoped>
.swiper-slide img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style>