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
            PROPIEDADES EN VENTA
          </h1>
          <hr class="w-48 h-1 mx-auto mb-2 bg-gray-100 border-0 rounded-sm" />
        </div>

        <div>
          <p
            class="mb-4 leading-relaxed long-text-roboto"
            id="venta-descripcion"
          >
            En esta sección encontrarás una cuidadosa selección de propiedades
            en venta en Guatemala, pensadas para quienes están listos para dar
            el siguiente paso: invertir en su patrimonio, iniciar una nueva
            etapa de vida o diversificar su portafolio inmobiliario ...
          </p>         

          <!-- Resto de párrafos: visibles en desktop o si verMas=true en mobile -->
          <div
            :id="'venta-extra'"
            v-show="!isMobile || verMas"
            class="animate-[fadeIn_200ms_ease-in-out]"
          >
            <p class="mb-4 leading-relaxed long-text-roboto">
              Ofrecemos casas, apartamentos, oficinas y terrenos en venta
              ubicados en zonas estratégicas como Zona 10, Zona 13, Zona 14,
              Zona 15, Zona 16 y Carretera a El Salvador, reconocidas por su
              seguridad, conectividad, cercanía a colegios, universidades,
              centros comerciales, áreas verdes y servicios esenciales.
            </p>

            <p class="mb-4 leading-relaxed long-text-roboto">
              Cada propiedad ha sido elegida por su diseño funcional, calidad
              constructiva y ubicación privilegiada, ofreciendo espacios
              pensados para el bienestar familiar y una vida cómoda y
              sofisticada. Además, muchas de estas opciones representan
              excelentes oportunidades de inversión inmobiliaria en Guatemala,
              ideales para quienes buscan generar ingresos a través de la renta
              a largo plazo o alquileres tipo Airbnb, gracias a su versatilidad,
              plusvalía y atractivo en el mercado actual.
            </p>

            <p class="mb-4 leading-relaxed long-text-roboto">
              Ya sea que busques tu próximo hogar o una inversión rentable, en
              Homes Guatemala tenemos la propiedad ideal para ti. Te acompañamos
              con asesoramiento experto y un enfoque personalizado para ayudarte
              a tomar la mejor decisión.
            </p>
          </div>
          <button
            v-if="isMobile"
            type="button"
            class="text-sm underline hover:no-underline"
            :aria-expanded="verMas ? 'true' : 'false'"
            aria-controls="venta-extra"
            @click="verMas = !verMas"
          >
            {{ verMas ? "Ocultar descripción completa de venta" : "Leer descripción completa de venta…" }}
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
              No se encontraron propiedades en venta.
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

// SEO Meta Tags optimizados para propiedades en venta
useSeoMeta({
  title: 'Propiedades en Venta Guatemala | Casas y Apartamentos de Lujo',
  description: 'Descubre las mejores propiedades en venta en Guatemala. Casas de lujo, apartamentos exclusivos y terrenos en Zona 10, 14, 15, 16 y Carretera a El Salvador.',
  ogTitle: 'Propiedades en Venta Guatemala | Homes Guatemala',
  ogDescription: 'Encuentra tu próximo hogar o inversión inmobiliaria en las zonas más exclusivas de Guatemala.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
  twitterCard: 'summary_large_image',
});

useHead({
  link: [
    { rel: 'canonical', href: 'https://homesguatemala.com/propiedades/venta' }
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

  // Siempre filtrar por Venta
  const filters = {
    Operaciones: "Venta",
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
