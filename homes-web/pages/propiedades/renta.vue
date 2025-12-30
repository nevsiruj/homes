<template>
  <div>
    <Header />
    <section class="text-gray-600 body-font mt-16 lg:mt-0">
      <div class="container px-5 py-24 mx-auto">
        <div class="flex flex-col text-center w-full mb-6">
          <h1
            id="inmuebles-section"
            class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900"
          >
            PROPIEDADES EN RENTA
          </h1>
          <hr class="w-48 h-1 mx-auto mb-2 bg-gray-100 border-0 rounded-sm" />
        </div>

        <div>
          <p
            class="mb-4 leading-relaxed long-text-roboto"
            id="renta-descripcion"
          >
            Nuestras propiedades en renta han sido seleccionadas para ofrecer
            una experiencia de vida sofisticada, funcional y adaptada a
            distintos estilos de vida. Desde apartamentos modernos hasta casas
            espaciosas en zonas residenciales, ofrecemos una variedad de
            opciones para elevar tu calidad de vida ...
          </p>        

          <div
            :id="'renta-extra'"
            v-show="!isMobile || verMas"
            class="animate-[fadeIn_200ms_ease-in-out]"
          >
            <p class="mb-4 leading-relaxed long-text-roboto">
              Ubicadas en zonas estratégicas de Ciudad de Guatemala, como Zona
              10, Zona 13, Zona 14, Zona 15, Zona 16 y Carretera a El Salvador,
              nuestras propiedades combinan accesibilidad con exclusividad.
              Están cerca de centros corporativos, comercios, colegios
              internacionales, áreas verdes y espacios de entretenimiento.
            </p>

            <p class="mb-4 leading-relaxed long-text-roboto">
              Ya sea que estés buscando un apartamento en alquiler por trabajo,
              una casa temporal para tu familia, o simplemente la libertad de
              vivir sin compromiso de compra, aquí encontrarás espacios cálidos,
              elegantes y listos para convertirse en tu hogar. Renta residencial
              en Guatemala con asesoramiento personalizado y propiedades
              diseñadas para disfrutar cada día con estilo y tranquilidad.
            </p>
          </div>
          <button
            v-if="isMobile"
            type="button"
            class="text-sm underline hover:no-underline "
            :aria-expanded="verMas ? 'true' : 'false'"
            aria-controls="renta-extra"
            @click="verMas = !verMas"
          >
            {{ verMas ? "Ocultar descripción completa de renta" : "Leer descripción completa de renta…" }}
          </button>
        </div>

        <div v-if="loading" class="flex flex-wrap -m-4 mt-10 justify-center">
          <Skeleton v-for="n in 9" :key="n" />
        </div>

        <div v-else-if="error" class="flex justify-center items-center py-20">
          <div
            class="text-red-600 text-xl font-semibold p-4 border border-red-300 rounded-md bg-red-50"
          >
            Error al cargar los inmuebles: {{ error }}
            <p class="text-sm mt-2">
              Por favor, refresque su navegador o presione F5.
            </p>
          </div>
        </div>

        <div v-else>
          <div
            v-if="inmuebles.length === 0"
            class="text-center text-gray-600 text-2xl py-10"
          >
            <p class="font-bold text-3xl mb-4">
              No se encontraron propiedades en renta.
            </p>
            <p class="text-xl">Te sugerimos ajustar los filtros para encontrar la propiedad ideal.</p>
          </div>
          <div v-else>
            <div id="propiedades-grid" class="flex flex-wrap -m-4 mt-10 justify-center">
              <div
                v-for="inmueble in inmuebles"
                :key="inmueble.id"
                class="w-full sm:w-1/2 md:w-1/2 lg:w-1/3 xl:w-1/3 p-4"
              >
                <InmuebleCard :inmueble="inmueble" />
              </div>
            </div>
            <div v-if="totalPages > 1" class="flex justify-center py-12">
              <nav aria-label="Page navigation" class="flex justify-center">
                <ul
                  class="inline-flex items-center -space-x-px text-base h-10 rounded-lg overflow-hidden border border-gray-300 shadow-sm"
                >
                  <li>
                    <button
                      @click="goToFirstPage"
                      :disabled="currentPage === 1"
                      :class="[
                        'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white border-r border-gray-300 rounded-s-lg transition-colors duration-200',
                        'hover:bg-black hover:text-white ',
                        { 'opacity-50 cursor-not-allowed': currentPage === 1 },
                      ]"
                    >
                      Primera
                    </button>
                  </li>
                  <li v-for="number in pageNumbers" :key="number">
                    <button
                      @click="goToPage(number)"
                      :class="[
                        'flex items-center justify-center px-4 h-10 leading-tight border-r border-gray-300 transition-colors duration-200',
                        {
                          'text-white bg-gray-800 font-bold border-gray-800 ':
                            currentPage === number,
                        },
                        {
                          'text-black bg-white hover:bg-black hover:text-white':
                            currentPage !== number,
                        },
                      ]"
                      :aria-current="
                        currentPage === number ? 'page' : undefined
                      "
                    >
                      {{ number }}
                    </button>
                  </li>
                  <li>
                    <button
                      @click="goToLastPage"
                      :disabled="currentPage === totalPages"
                      :class="[
                        'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white rounded-e-lg transition-colors duration-200',
                        'hover:bg-black hover:text-white ',
                        {
                          'opacity-50 cursor-not-allowed':
                            currentPage === totalPages,
                        },
                      ]"
                    >
                      Última
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
import { ref, computed, onMounted, watch, nextTick, onUnmounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import Header from "~/components/header.vue";
import Footer from "~/components/footer.vue";
import RedesFlotantes from "~/components/redesFlotantes.vue";
import InmuebleCard from "~/components/InmuebleCard.vue";
import inmuebleService from "~/services/inmuebleService";
import { initFlowbite } from "flowbite";
import Skeleton from "~/components/skeleton.vue";

// SEO Meta Tags optimizados para propiedades en renta
useSeoMeta({
  title: 'Propiedades en Renta Guatemala | Casas y Apartamentos en Alquiler',
  description: 'Encuentra las mejores propiedades en renta en Guatemala. Apartamentos amueblados, casas de lujo y oficinas en alquiler en Zona 10, 14, 15, 16 y más.',
  ogTitle: 'Propiedades en Renta Guatemala | Homes Guatemala',
  ogDescription: 'Renta tu próximo hogar en las zonas más exclusivas de Guatemala. Propiedades con seguridad, ubicación privilegiada y amenidades premium.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp',
  twitterCard: 'summary_large_image',
});

useHead({
  link: [
    { rel: 'canonical', href: 'https://homesguatemala.com/propiedades/renta' }
  ]
});

const route = useRoute();
const router = useRouter();

const inmuebles = ref([]);
const loading = ref(true);
const error = ref(null);
const currentPage = ref(1);
const itemsPerPage = ref(9);
const totalPages = ref(0);
const totalCount = ref(0);
const verMas = ref(false);
const isMobile = ref(false);

const checkIfMobile = () => {
  isMobile.value = window.innerWidth < 768;
};

const scrollToPropiedades = () => {
  if (process.client) {
    nextTick(() => {
      const grid = document.getElementById("propiedades-grid");
      const section = document.getElementById("inmuebles-section");
      const targetElement = grid || section;
      
      if (targetElement) {
        targetElement.scrollIntoView({ 
          behavior: "smooth", 
          block: "start",
          inline: "nearest"
        });
      }
    });
  }
};

const fetchInmuebles = async () => {
  loading.value = true;
  error.value = null;

  const page = parseInt(route.query.pageNumber || "1");
  const size = parseInt(route.query.pageSize || "9");

  currentPage.value = page;

  // Siempre filtrar por Renta
  const filters = {
    Operaciones: "Renta",
    Tipos: route.query.Tipos
      ? Array.isArray(route.query.Tipos)
        ? route.query.Tipos
        : [route.query.Tipos]
      : [],
    ubicaciones: route.query.Ubicaciones
      ? Array.isArray(route.query.Ubicaciones)
        ? route.query.Ubicaciones
        : [route.query.Ubicaciones]
      : [],
    caracteristicasPropiedad: route.query.Amenidades
      ? Array.isArray(route.query.Amenidades)
        ? route.query.Amenidades
        : [route.query.Amenidades]
      : [],
    precioMinimo: route.query.PrecioMinimo
      ? Number(route.query.PrecioMinimo)
      : null,
    precioMaximo: route.query.PrecioMaximo
      ? Number(route.query.PrecioMaximo)
      : null,
    HabitacionesMin: route.query.HabitacionesMin
      ? [route.query.HabitacionesMin]
      : [],
    Orden: route.query.Orden || "",
  };

  try {
    const cleanedFilters = Object.fromEntries(
      Object.entries(filters).filter(
        ([_, value]) =>
          value !== "" &&
          value !== null &&
          !(Array.isArray(value) && value.length === 0)
      )
    );

    const response = await inmuebleService.getInmueblesPaginados(
      page,
      size,
      cleanedFilters
    );

    if (response && Array.isArray(response.items)) {
      inmuebles.value = response.items.map((item) => {
        const plainItem = JSON.parse(JSON.stringify(item));
        return {
          ...plainItem,
          imagenesReferencia:
            plainItem.imagenesReferencia?.$values ||
            plainItem.imagenesReferencia ||
            [],
          amenidades:
            plainItem.amenidades?.$values || plainItem.amenidades || [],
        };
      });
      totalCount.value = response.totalCount || 0;
      totalPages.value = response.totalPages || 0;
    } else {
      inmuebles.value = [];
      totalCount.value = 0;
      totalPages.value = 0;
    }
  } catch (err) {
    error.value =
      "No se pudieron cargar los inmuebles. Intenta de nuevo más tarde.";
  } finally {
    loading.value = false;
    nextTick(() => {
      scrollToPropiedades();
    });
  }
};

const pageNumbers = computed(() => {
  const numbers = [];
  const maxPageButtons = isMobile.value ? 3 : 6;
  let startPage, endPage;

  if (totalPages.value <= maxPageButtons) {
    startPage = 1;
    endPage = totalPages.value;
  } else {
    const half = Math.floor(maxPageButtons / 2);
    startPage = currentPage.value - half;
    endPage = currentPage.value + (maxPageButtons - 1 - half);

    if (startPage < 1) {
      startPage = 1;
      endPage = maxPageButtons;
    }

    if (endPage > totalPages.value) {
      endPage = totalPages.value;
      startPage = totalPages.value - maxPageButtons + 1;
      if (startPage < 1) {
        startPage = 1;
      }
    }
  }

  for (let i = startPage; i <= endPage; i++) {
    numbers.push(i);
  }
  return numbers;
});

const goToPage = (pageNumber) => {
  if (
    pageNumber >= 1 &&
    pageNumber <= totalPages.value &&
    pageNumber !== currentPage.value
  ) {
    router.push({
      path: route.path,
      query: {
        ...route.query,
        pageNumber: pageNumber,
      },
    });
  }
};

const goToFirstPage = () => {
  if (currentPage.value !== 1) {
    router.push({
      path: route.path,
      query: {
        ...route.query,
        pageNumber: 1,
      },
    });
  }
};

const goToLastPage = () => {
  if (currentPage.value !== totalPages.value) {
    router.push({
      path: route.path,
      query: {
        ...route.query,
        pageNumber: totalPages.value,
      },
    });
  }
};

watch(
  [
    () => route.query.pageNumber,
    () => route.query.Tipos,
    () => route.query.Ubicaciones,
    () => route.query.Amenidades,
    () => route.query.PrecioMinimo,
    () => route.query.PrecioMaximo,
    () => route.query.HabitacionesMin,
    () => route.query.Orden,
  ],
  () => {
    fetchInmuebles();
  },
  { immediate: true }
);

onMounted(() => {
  initFlowbite();
  checkIfMobile();
  window.addEventListener("resize", checkIfMobile);
});

onUnmounted(() => {
  window.removeEventListener("resize", checkIfMobile);
});
</script>

<style scoped>
.precio-optima {
  font-family: "Optima", serif;
  font-weight: normal;
  font-size: 24px;
}
</style>
