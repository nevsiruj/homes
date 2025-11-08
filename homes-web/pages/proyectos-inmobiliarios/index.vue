<template>
  <Headerb />
  <section class="text-gray-600 body-font lg:mt-0">
    <div class="container px-5 py-12 mx-auto">
      <div class="flex flex-col text-center w-full mb-6">
        <h1
        id="proyectos-section"
          class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900"
        >
          PROYECTOS
        </h1>
        <hr class="w-48 h-1 mx-auto mb-2 bg-gray-100 border-0 rounded-sm" />
      </div>

      <!-- Primer párrafo truncado en móvil hasta la frase indicada -->
      <p class="mb-4 leading-relaxed long-text-roboto" id="intro-proyectos">
        Descubre nuestros
        <strong>proyectos inmobiliarios más exclusivos</strong> en Ciudad de
        Guatemala, diseñados para quienes buscan un estilo de vida moderno y una
        excelente oportunidad de inversión. En Homes Guatemala desarrollamos y
        comercializamos <strong>casas y apartamentos nuevos o en construcción,</strong>
        <!-- Resto del primer párrafo solo visible en desktop o cuando verMas=true en móvil -->
        <span v-show="!isMobile || verMas">
          estratégicamente ubicados en zonas con alta demanda y crecimiento como
          Zona 10, Zona 13, Zona 14, Zona 15, Zona 16 y Carretera a El Salvador.
        </span>
      </p>

      <!-- Botón SOLO en móvil para desplegar el resto -->
      <button
        v-if="isMobile"
        type="button"
        class="text-sm underline hover:no-underline mb-2"
        :aria-expanded="verMas ? 'true' : 'false'"
        aria-controls="intro-extra"
        @click="verMas = !verMas"
      >
        {{ verMas ? 'Ver menos' : 'Ver más…' }}
      </button>

      <!-- Resto de párrafos: visibles siempre en desktop; en móvil solo si verMas=true -->
      <div :id="'intro-extra'" v-show="!isMobile || verMas">
        <p class="mb-4 leading-relaxed long-text-roboto">
          Desde apartamentos rodeados de naturaleza hasta proyectos urbanos de
          alto nivel, cada desarrollo ha sido cuidadosamente seleccionado por su
          diseño arquitectónico, ubicación privilegiada y potencial de plusvalía a
          largo plazo.
        </p>
        <p class="mb-6 leading-relaxed long-text-roboto">
          Además de ofrecer propiedades ideales para vivir, muchos de nuestros
          proyectos representan excelentes oportunidades para generar ingresos a
          través de plataformas como Airbnb, gracias a su ubicación estratégica y
          alto atractivo para el mercado de renta a corto y mediano plazo.
        </p>
      </div>

      <form class="max-w-xl mx-auto px-4 py-8" id="proyecto-grid">
        <div class="flex justify-center flex-wrap gap-2 lg:gap-6 items-center">
          <div class="lg:flex-1 min-w-[200px]">
            <div class="relative">
              <button
                type="button"
                class="inline-flex items-center justify-between w-[250px] gap-2 rounded-md border px-3 py-2 text-sm bg-white"
                @click="openTipo = !openTipo"
              >
                <span>Tipo proyecto</span>
                <span
                  v-if="filtroTipo.length"
                  class="rounded bg-gray-200 px-2 py-0.5 text-xs"
                >
                  {{ filtroTipo.length }}
                </span>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.23 7.21a.75.75 0 011.06.02L10 10.94l3.71-3.71a.75.75 0 111.06 1.06l-4.24 4.25a.75.75 0 01-1.06 0L5.21 8.29a.75.75 0 01.02-1.08z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
              <div
                v-if="openTipo"
                class="absolute z-20 mt-2 w-64 rounded-md border bg-white p-2 shadow"
              >
                <div class="max-h-56 overflow-auto px-2">
                  <label
                    v-for="tipo in tiposUnicos"
                    :key="tipo"
                    class="flex items-center gap-2 py-1 text-sm cursor-pointer"
                  >
                    <input
                      type="checkbox"
                      class="h-4 w-4"
                      :value="tipo"
                      :checked="filtroTipo.includes(tipo)"
                      @change="toggleTipo(tipo, $event.target.checked)"
                    />
                    <span>{{ tipo }}</span>
                  </label>
                  <div
                    v-if="!tiposUnicos.length"
                    class="p-3 text-center text-xs text-gray-500"
                  >
                    Sin resultados
                  </div>
                </div>
                <div class="mt-2 flex items-center justify-between gap-2 px-2">
                  <button class="text-xs underline" @click="filtroTipo = []">
                    Limpiar
                  </button>
                  <button
                    class="rounded-md border px-2 py-1 text-xs"
                    @click="openTipo = false"
                  >
                    Cerrar
                  </button>
                </div>
              </div>
            </div>
          </div>

          <div class="lg:flex-1 min-w-[200px]">
            <div class="relative">
              <button
                type="button"
                class="inline-flex items-center justify-between w-[250px] gap-2 rounded-md border px-3 py-2 text-sm bg-white"
                @click="openUbi = !openUbi"
              >
                <span>Ubicación</span>
                <span
                  v-if="filtroUbicacion.length"
                  class="rounded bg-gray-200 px-2 py-0.5 text-xs"
                >
                  {{ filtroUbicacion.length }}
                </span>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.23 7.21a.75.75 0 011.06.02L10 10.94l3.71-3.71a.75.75 0 111.06 1.06l-4.24 4.25a.75.75 0 01-1.06 0L5.21 8.29a.75.75 0 01.02-1.08z"
                    clip-rule="evenodd"
                  />
                </svg>
              </button>
              <div
                v-if="openUbi"
                class="absolute z-20 mt-2 w-64 rounded-md border bg-white p-2 shadow"
              >
                <div class="max-h-56 overflow-auto px-2">
                  <label
                    v-for="ubic in ubicacionesUnicas"
                    :key="ubic"
                    class="flex items-center gap-2 py-1 text-sm cursor-pointer"
                  >
                    <input
                      type="checkbox"
                      class="h-4 w-4"
                      :value="ubic"
                      :checked="filtroUbicacion.includes(ubic)"
                      @change="toggleUbi(ubic, $event.target.checked)"
                    />
                    <span>{{ ubic }}</span>
                  </label>
                  <div
                    v-if="!ubicacionesUnicas.length"
                    class="p-3 text-center text-xs text-gray-500"
                  >
                    Sin resultados
                  </div>
                </div>
                <div class="mt-2 flex items-center justify-between gap-2 px-2">
                  <button
                    class="text-xs underline"
                    @click="filtroUbicacion = []"
                  >
                    Limpiar
                  </button>
                  <button
                    class="rounded-md border px-2 py-1 text-xs"
                    @click="openUbi = false"
                  >
                    Cerrar
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </form>

      <!-- <div v-if="loading" class="flex justify-center items-center py-20">
        <div class="text-xl font-semibold text-black">
          Cargando proyectos...
        </div>
      </div> -->
      <div v-if="loading" class="flex flex-wrap -m-4 mt-10 justify-center">
          <Skeleton v-for="n in 9" :key="n" />
        </div>

      <div v-else-if="error" class="flex justify-center items-center py-20">
        <div
          class="text-red-600 text-xl font-semibold p-4 border border-red-300 rounded-md bg-red-50"
        >
          Error al cargar los proyectos: {{ error }}
          <p class="text-sm mt-2">
            Verifica la consola para más detalles o si la API no está
            disponible.
          </p>
        </div>
      </div>

      <div v-else>
        <div
          v-if="filteredProyectos.length === 0"
          class="text-center text-gray-600 text-2xl py-10"
        >
          No hay proyectos disponibles para mostrar.
        </div>
        <div v-else>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            <ProyectoCard
              v-for="proyecto in currentProyectos"
              :key="proyecto.id"
              :proyecto="proyecto"
            />
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
                      'hover:bg-black hover:text-white',
                      { 'opacity-50 cursor-not-allowed': currentPage === 1 },
                    ]"
                  >
                    Primera
                  </button>
                </li>
                <li>
                  <!-- <button
                    @click="goToPrevPage"
                    :disabled="currentPage === 1"
                    :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white border-r border-gray-300 transition-colors duration-200',
                      'hover:bg-black hover:text-white',
                      { 'opacity-50 cursor-not-allowed': currentPage === 1 },
                    ]"
                  >
                    Anterior
                  </button> -->
                </li>
                <li v-for="number in pageNumbers" :key="number">
                  <button
                    @click="paginate(number)"
                    :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight border-r border-gray-300 transition-colors duration-200',
                      {
                        'text-white bg-gray-800 font-bold border-gray-800':
                          currentPage === number,
                      },
                      {
                        'text-black bg-white hover:bg-black hover:text-white':
                          currentPage !== number,
                      },
                    ]"
                    :aria-current="currentPage === number ? 'page' : undefined"
                  >
                    {{ number }}
                  </button>
                </li>
                <li>
                  <!-- <button
                    @click="goToNextPage"
                    :disabled="currentPage === totalPages"
                    :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white transition-colors duration-200',
                      'hover:bg-black hover:text-white',
                      {
                        'opacity-50 cursor-not-allowed':
                          currentPage === totalPages,
                      },
                    ]"
                  >
                    Siguiente
                  </button> -->
                </li>
                <li>
                  <button
                    @click="goToLastPage"
                    :disabled="currentPage === totalPages"
                    :class="[
                      'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white rounded-e-lg transition-colors duration-200',
                      'hover:bg-black hover:text-white',
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
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, watch, nextTick } from "vue";
import Headerb from "../../components/headerb.vue";
import Footer from "../../components/footer.vue";
import RedesFlotantes from "../../components/redesFlotantes.vue";
import ProyectoCard from "../../components/proyectoCard.vue";
import proyectoService from "../../services/proyectoService";
import { initFlowbite } from "flowbite";
import skeleton from "~/components/skeleton.vue";

// Estado
const allProyectos = ref([]);
const loading = ref(true);
const error = ref(null);
const currentPage = ref(1);
const itemsPerPage = ref(9);
const proyectosForHeaderCounts = ref([]);

// Filtros
const filtroTipo = ref([]);
const filtroUbicacion = ref([]);
// Estados de apertura para los dropdowns
const openTipo = ref(false);
const openUbi = ref(false);

// 👇 NUEVO: control de móvil y ver más
const isMobile = ref(false);
const verMas = ref(false);
const checkIfMobile = () => {
  isMobile.value = window.innerWidth < 768; // breakpoint md
};

// Helpers para checkboxes (evitan duplicados y actualizan paginación)
const toggleTipo = (value, checked) => {
  const set = new Set(filtroTipo.value);
  if (checked) set.add(value);
  else set.delete(value);
  filtroTipo.value = Array.from(set);
  currentPage.value = 1;
};
const toggleUbi = (value, checked) => {
  const set = new Set(filtroUbicacion.value);
  if (checked) set.add(value);
  else set.delete(value);
  filtroUbicacion.value = Array.from(set);
  currentPage.value = 1;
};

// Cargar proyectos
const loadAllProyectos = async () => {
  try {
    loading.value = true;
    error.value = null;
    const responseData = await proyectoService.getProyecto();
    if (responseData && Array.isArray(responseData.$values)) {
      const normalizedData = responseData.$values.map((proyecto) => ({
        id: proyecto.id,
        codigoProyecto: proyecto.codigoProyecto,
        title: proyecto.titulo,
        precio: proyecto.precio,
        metrosCuadrados: proyecto.metrosCuadrados,
        image: proyecto.imagenPrincipal?.trim(),
        imagenesReferencia:
          proyecto.imagenesReferenciaProyecto &&
          Array.isArray(proyecto.imagenesReferenciaProyecto.$values)
            ? proyecto.imagenesReferenciaProyecto.$values.map((img) => ({
                url: img.url?.trim(),
              }))
            : [],
        fechaCreacion: proyecto.fechaCreacion,
        descripcion: proyecto.contenido,
        tipos: proyecto.tipos?.trim(),
        ubicacion: proyecto.ubicaciones?.trim(),
        amenidades: Array.isArray(proyecto.amenidades?.$values)
          ? proyecto.amenidades.$values
          : [],
        video: proyecto.video?.trim(),
        slugProyecto: proyecto.slugProyecto,
      }));
      allProyectos.value = normalizedData;
      proyectosForHeaderCounts.value = normalizedData;
    } else {
      allProyectos.value = [];
      proyectosForHeaderCounts.value = [];
    }
  } catch (err) {
    error.value =
      "No se pudieron cargar los proyectos iniciales. Intenta de nuevo más tarde.";
    //console.error("Error al cargar los proyectos:", err);
  } finally {
    loading.value = false;
    // Ejecutar scroll después de que loading sea false y el DOM se actualice
    nextTick(() => {
      scrollToProyecto();
    });
  }
};

onMounted(() => {
  loadAllProyectos();
  initFlowbite();
  // 👇 NUEVO: inicialización de móvil + listener
  checkIfMobile();
  window.addEventListener("resize", checkIfMobile);
});

onUnmounted(() => {
  // 👇 NUEVO: limpiar listener
  window.removeEventListener("resize", checkIfMobile);
});

const tiposUnicos = computed(() => {
  const tipos = new Set();
  allProyectos.value.forEach((p) => p.tipos && tipos.add(p.tipos));
  return Array.from(tipos).sort();
});

const ubicacionesUnicas = computed(() => {
  const ubicaciones = new Set();
  allProyectos.value.forEach(
    (p) => p.ubicacion && ubicaciones.add(p.ubicacion)
  );
  return Array.from(ubicaciones).sort();
});

const filteredProyectos = computed(() => {
  let result = allProyectos.value;

  if (filtroTipo.value && filtroTipo.value.length) {
    result = result.filter((p) => {
      const v = p.tipos?.toLowerCase();
      return (
        v && filtroTipo.value.some((sel) => String(sel).toLowerCase() === v)
      );
    });
  }

  if (filtroUbicacion.value && filtroUbicacion.value.length) {
    result = result.filter((p) => {
      const v = p.ubicacion?.toLowerCase();
      return (
        v &&
        filtroUbicacion.value.some((sel) => String(sel).toLowerCase() === v)
      );
    });
  }

  return result;
});

watch([filtroTipo, filtroUbicacion], () => {
  currentPage.value = 1;
  // Scroll cuando cambien los filtros
  scrollToProyecto();
});

const currentProyectos = computed(() => {
  const end = currentPage.value * itemsPerPage.value;
  const start = end - itemsPerPage.value;
  return filteredProyectos.value.slice(start, end);
});

const totalPages = computed(() => {
  return Math.ceil(filteredProyectos.value.length / itemsPerPage.value) || 1;
});

const pageNumbers = computed(() => {
  const total = totalPages.value;
  const numbers = [];
  for (let i = 1; i <= total; i++) {
    numbers.push(i);
  }
  return numbers;
});

const paginate = (pageNumber) => {
  currentPage.value = pageNumber;
  // Scroll cuando se cambie de página
  scrollToProyecto();
};

const goToPrevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
    scrollToProyecto();
  }
};

const goToNextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
    scrollToProyecto();
  }
};

const goToFirstPage = () => {
  if (currentPage.value !== 1) {
    currentPage.value = 1;
    scrollToProyecto();
  }
};

const goToLastPage = () => {
  if (currentPage.value !== totalPages.value) {
    currentPage.value = totalPages.value;
    scrollToProyecto();
  }
};

const truncateText = (text, maxLength) => {
  if (!text) return "";
  return text.length > maxLength ? text.substring(0, maxLength) + "..." : text;
};

const scrollToProyecto = () => {
  if (process.client) {
    nextTick(() => {
      const grid = document.getElementById("proyecto-grid");
      const section = document.getElementById("proyectos-section");
      
      // Priorizar el grid si existe, sino el título de la sección
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
</script>
