<template>
  <h1 class="sr-only">Homes Guatemala - Bienes Raíces, Venta y Renta de Propiedades</h1>
  <section class="text-gray-600 body-font mt-28">
    <div
      class="container mx-auto flex px-5 lg:py-12 md:flex-row flex-col items-center py-6"
    >
      <div class="lg:max-w-lg lg:w-full md:w-1/2 w-5/6 mb-10 md:mb-0">
        <img loading="lazy"
          decoding="async"
          class="object-cover object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
          alt="Proyectos inmobiliarios exclusivos en Guatemala - Homes Guatemala"
          src="https://app-pool.vylaris.online/dcmigserver/homes/f64ca937-cba0-4276-b350-9aca8a1b51bc.webp"
        />
      </div>
      <div
        class="lg:flex-grow md:w-1/2 lg:pl-24 md:pl-16 flex flex-col md:items-start md:text-left items-center text-center"
      >
        <h2
          class="title-font title-alta sm:text-4xl text-3xl mb-4 font-medium text-gray-900"
        >
          PROYECTOS
        </h2>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Descubre nuestros
          <strong>proyectos inmobiliarios más exclusivos</strong> en Ciudad de
          Guatemala, diseñados para quienes buscan un estilo de vida moderno y
          una excelente oportunidad de inversión. En Homes Guatemala
          desarrollamos y comercializamos casas y apartamentos nuevos o en
          construcción, estratégicamente ubicados en zonas con alta demanda y
          crecimiento como Zona 10, Zona 13, Zona 14, Zona 15, Zona 16 y
          Carretera a El Salvador ...
        </p>
        <div class="flex justify-center">
          <NuxtLink
            to="/proyectos-inmobiliarios"
            class="inline-flex boton-optima text-white bg-black border-0 py-4 px-6 focus:outline-none hover:bg-gray-600 rounded text-lg"
            >Ver proyectos</NuxtLink
          >
        </div>
      </div>
    </div>
  </section>
  <section class="text-gray-600 body-font">
    <div
      class="container mx-auto flex px-5 lg:py-12 md:flex-row flex-col items-center py-6"
    >
      <div class="lg:max-w-lg lg:w-full md:w-1/2 w-5/6 order-1 md:order-2">
        <img loading="lazy"
          decoding="async"
          class="object-cover object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
          alt="Propiedades en venta en Guatemala - Casas y apartamentos"
          src="https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp"
        />
      </div>
      <div
        class="lg:flex-grow md:w-1/2 lg:pr-24 md:pr-16 flex flex-col md:items-start md:text-left mb-16 md:mb-0 items-center text-center order-2 md:order-1"
      >
        <h2
          class="title-font title-alta sm:text-4xl text-3xl mb-4 mt-4 font-medium text-gray-900"
        >
          VENTA
        </h2>
        <p class="mb-4 leading-relaxed long-text-roboto">
          En esta sección encontrarás una cuidadosa selección de propiedades en
          venta en Guatemala, pensadas para quienes están listos para dar el
          siguiente paso: invertir en su patrimonio, iniciar una nueva etapa de
          vida o diversificar su portafolio inmobiliario ...
        </p>
        <div class="flex justify-center">
          <button
            @click="handleRedirectToPropiedades('venta')"
            class="inline-flex boton-optima text-white bg-black border-0 py-4 px-6 focus:outline-none hover:bg-gray-600 rounded text-lg"
          >
            Ver propiedades
          </button>
        </div>
      </div>
    </div>
  </section>
  <section class="text-gray-600 body-font">
    <div
      class="container mx-auto flex px-5 lg:py-12 md:flex-row flex-col items-center mb-6"
    >
      <div class="lg:max-w-lg lg:w-full md:w-1/2 w-5/6 mb-10 md:mb-0">
        <img loading="lazy"
          decoding="async"
          class="object-cover w-140 object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
          alt="Propiedades en renta en Guatemala - Apartamentos y casas amuebladas"
          src="https://app-pool.vylaris.online/dcmigserver/homes/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp"
        />
      </div>
      <div
        class="lg:flex-grow md:w-1/2 lg:pl-24 md:pl-16 flex flex-col md:items-start md:text-left items-center text-center"
      >
        <h2
          class="title-font title-alta sm:text-4xl text-3xl mb-4 font-medium text-gray-900"
        >
          RENTA
        </h2>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Nuestras propiedades en renta han sido seleccionadas para ofrecer una
          experiencia de vida sofisticada, funcional y adaptada a distintos
          estilos de vida. Desde apartamentos modernos hasta casas espaciosas en
          zonas residenciales, ofrecemos una variedad de opciones para elevar tu
          calidad de vida ...
        </p>
        <div class="flex justify-center">
          <button
          
            @click="handleRedirectToPropiedades('renta')"
            class="inline-flex boton-optima text-white bg-black border-0 py-4 px-6 focus:outline-none hover:bg-gray-600 rounded text-lg"
          >
            Ver propiedades
          </button>
        </div>
      </div>
    </div>

    <div v-if="suggestedPropertiesLoading" class="mt-12 mb-12 text-center">
      <p>Cargando propiedades destacadas...</p>
    </div>
    <div
      v-else-if="suggestedPropertiesError"
      class="mt-12 mb-12 text-center text-red-500"
    >
      <p>
        Error al cargar propiedades destacadas: {{ suggestedPropertiesError }}
      </p>
    </div>
    <div class="mt-12 mb-12 px-5">
      <h2
        class="title-alta-2 text-xl text-center font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl mt-8 mb-8"
      >
        Propiedades Destacadas
      </h2>
      
      <div v-if="featuredPropertiesLoading" class="text-center py-8">
        <p>Cargando propiedades destacadas...</p>
      </div>
      
      <div
        v-else-if="featuredPropertiesError"
        class="text-center py-8 text-red-500"
      >
        <p>{{ featuredPropertiesError }}</p>
      </div>
      
      <swiper
        v-else-if="featuredProperties.length > 0"
        :pagination="{ clickable: true }"
        :loop="true"
        :autoplay="{ delay: 3000, disableOnInteraction: false }"
        :modules="swiperModules"
        :slidesPerView="isMobile ? 1 : 3"
        :spaceBetween="isMobile ? 0 : 20"
        class="mySwiper suggested-images-slider"
      >
        <swiper-slide
          v-for="property in featuredProperties"
          :key="property.id"
        >
          <NuxtLink
            :to="`/inmueble/${property.slugInmueble}`"
            class="relative block w-full h-80 overflow-hidden rounded-lg shadow-md group"
          >
            <img
              :src="property.imagenPrincipal"
              class="w-full h-full object-cover object-center transform transition-transform duration-500 group-hover:scale-105"
              :alt="property.titulo"
            />
            <div
              class="absolute inset-0 bg-gradient-to-t from-black via-transparent to-transparent opacity-70 group-hover:opacity-80 transition-opacity duration-300"
            ></div>
            <div class="absolute bottom-0 left-0 p-4 text-white">
              <h5
                class="text-xl md:text-2xl font-bold leading-tight drop-shadow-lg"
              >
                {{ property.titulo }}
              </h5>
              <p class="text-sm mt-1 opacity-90">
                {{ property.ubicaciones }}
              </p>
              <p v-if="property.precio && property.precio > 0" class="text-lg mt-2 font-semibold drop-shadow-lg">
                ${{ property.precio?.toLocaleString() || "0" }}
              </p>
            </div>
            <div
              class="absolute top-4 right-4 text-white opacity-0 group-hover:opacity-100 transition-opacity duration-300"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-8 w-8"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                stroke-width="2"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  d="M14 5l7 7m0 0l-7 7m7-7H3"
                />
              </svg>
            </div>
          </NuxtLink>
        </swiper-slide>
      </swiper>
      
      <div v-else class="text-center py-8 text-gray-500">
        <p>No hay propiedades destacadas disponibles en este momento.</p>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, computed, watch, onUnmounted } from "vue";
import { useRoute, useRouter  } from "vue-router";
import inmuebleService from "../../services/inmuebleService";
// Componentes de Swiper
import { Swiper, SwiperSlide } from "swiper/vue";
// Estilos de Swiper
import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/autoplay";

// Módulos de Swiper
import { Pagination, Autoplay } from "swiper/modules";

// Define los módulos que se pasarán al componente Swiper
const swiperModules = [Pagination, Autoplay];

const isMobile = ref(false);
const route = useRoute();
const router = useRouter();
const inmuebleId = ref(null);
const inmuebleDetalle = ref({});
const mainMedia = ref({ url: "", type: "image" });
const currentMediaIndex = ref(0);
const suggestedProperties = ref([]);
const suggestedPropertiesLoading = ref(false);
const suggestedPropertiesError = ref(null);

// Propiedades destacadas dinámicas desde el backend
const featuredProperties = ref([]);
const featuredPropertiesLoading = ref(false);
const featuredPropertiesError = ref(null);

const checkMobile = () => {
  isMobile.value = window.innerWidth < 768; // Punto de corte para "móvil"
  //console.log("Es móvil:", isMobile.value);
};

const allMedia = computed(() => {
  const media = [];
  if (inmuebleDetalle.value.video) {
    media.push({ url: inmuebleDetalle.value.video, type: "video" });
  }
  if (
    inmuebleDetalle.value.imagenPrincipal &&
    inmuebleDetalle.value.imagenPrincipal !== inmuebleDetalle.value.video
  ) {
    media.push({ url: inmuebleDetalle.value.imagenPrincipal, type: "image" });
  }
  if (
    inmuebleDetalle.value.imagenesReferencia &&
    inmuebleDetalle.value.imagenesReferencia.length > 0
  ) {
    media.push(
      ...inmuebleDetalle.value.imagenesReferencia.map((img) => ({
        url: img.url,
        type: "image",
      }))
    );
  }
  return media;
});

watch(
  allMedia,
  (newMedia) => {
    if (newMedia.length > 0) {
      mainMedia.value = newMedia[0];
      currentMediaIndex.value = 0;
    } else {
      mainMedia.value = { url: "", type: "image" };
    }
  },
  { immediate: true }
);

const setMainMedia = (url, type) => {
  mainMedia.value = { url, type };
  currentMediaIndex.value = allMedia.value.findIndex(
    (media) => media.url === url
  );
};

const prevMedia = () => {
  if (allMedia.value.length > 0) {
    currentMediaIndex.value =
      (currentMediaIndex.value - 1 + allMedia.value.length) %
      allMedia.value.length;
    mainMedia.value = allMedia.value[currentMediaIndex.value];
  }
};

const nextMedia = () => {
  if (allMedia.value.length > 0) {
    currentMediaIndex.value =
      (currentMediaIndex.value + 1) % allMedia.value.length;
    mainMedia.value = allMedia.value[currentMediaIndex.value];
  }
};

const isYouTubeVideo = (url) => {
  return (
    url.includes("youtube.com/watch?v=") ||
    url.includes("youtu.be/") ||
    url.includes("googleusercontent.com/youtube.com/")
  );
};

const getYouTubeEmbedUrl = (url) => {
  let videoId = "";
  const watchMatch = url.match(
    /(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^"&?\/\s]{11})/
  );
  const googUserContentMatch = url.match(
    /googleusercontent\.com\/youtube\.com\/(\d+)$/
  );
  if (watchMatch && watchMatch[1]) {
    videoId = watchMatch[1];
  } else if (googUserContentMatch && googUserContentMatch[1]) {
    videoId = googUserContentMatch[1];
  }
  if (videoId) {
    return `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`;
  }
  return "";
};

const getYouTubeThumbnail = (url) => {
  let videoId = "";
  const watchMatch = url.match(
    /(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^"&?\/\s]{11})/
  );
  const googUserContentMatch = url.match(
    /googleusercontent\.com\/youtube\.com\/(\d+)$/
  );
  if (watchMatch && watchMatch[1]) {
    videoId = watchMatch[1];
  } else if (googUserContentMatch && googUserContentMatch[1]) {
    videoId = googUserContentMatch[1];
  }
  if (videoId) {
    return `https://img.youtube.com/vi/${videoId}/hqdefault.jpg`;
  }
  return null;
};

const formattedDescription = computed(() => {
  if (!inmuebleDetalle.value.contenido) return "";
  return formatContent(inmuebleDetalle.value.contenido);
});

const formatContent = (content) => {
  if (!content) return "";
  let formattedContent = content
    .replace(/&nbsp;/g, " ")
    .replace(/\r\n/g, "\n")
    .replace(/\t/g, "    ");
  const sections = formattedContent.split(/\n\s*\n/);
  let htmlOutput = "";
  sections.forEach((section) => {
    section = section.trim();
    if (section === "") return;
    if (
      section.includes(":") &&
      (section.includes("- ") ||
        section.includes("* ") ||
        section.includes(","))
    ) {
      const firstColonIndex = section.indexOf(":");
      if (firstColonIndex !== -1) {
        let title = section.substring(0, firstColonIndex).trim();
        let remainingContent = section.substring(firstColonIndex + 1).trim();
        htmlOutput += `<p class="font-semibold mt-4 mb-2"><strong>${title}:</strong></p>`;
        const items = remainingContent
          .split(/[\n,]\s*/)
          .filter((item) => item !== "")
          .map((item) => {
            return item.replace(/^(-|\*)\s*/, "").trim();
          });
        if (items.length > 0 && items.some((item) => item !== "")) {
          htmlOutput += `<ul class="list-disc list-inside space-y-1">`;
          items.forEach((item) => {
            if (item) {
              htmlOutput += `<li>${item}</li>`;
            }
          });
          htmlOutput += `</ul>`;
        } else {
          htmlOutput += `<p class="mb-2">${remainingContent.replace(
            /\n/g,
            "<br>"
          )}</p>`;
        }
      } else {
        htmlOutput += `<p class="mb-2">${section
          .replace(/([^:]+):/g, "<strong>$1:</strong>")
          .replace(/\n/g, "<br>")}</p>`;
      }
    } else {
      htmlOutput += `<p class="mb-2">${section
        .replace(/([^:]+):/g, "<strong>$1:</strong>")
        .replace(/\n/g, "<br>")}</p>`;
    }
  });
  return htmlOutput;
};

const whatsappLink = computed(() => {
  const phoneNumber = "+50256330961";
  const propertyTitle =
    inmuebleDetalle.value.titulo || "esta propiedad en tu sitio web";
  const message = `Hola, estoy interesado en ${propertyTitle}.`;
  const encodedMessage = encodeURIComponent(message);
  return `https://wa.me/${phoneNumber}?text=${encodedMessage}`;
});

const formattedPrice = computed(() => {
  if (!inmuebleDetalle.value.precio) return "";
  const number = parseInt(inmuebleDetalle.value.precio, 10);
  return new Intl.NumberFormat("es-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(number);
});

const loadSuggestedProperties = async () => {
  try {
    const responseData = await inmuebleService.getInmueblesPaginados(1, 15);

    //console.log("Respuesta completa de getInmueblesPaginados():", responseData);

    if (
  responseData &&
  Array.isArray(responseData.items)
) {
  const allProperties = responseData.items;

      const currentId = inmuebleDetalle.value?.id;

      //console.log("ID del inmueble actual para sugerencias:", currentId);
      //console.log("Todas las propiedades recibidas:", allProperties);

      // 1. Filtramos la propiedad actual (como ya lo hacías)
      const filteredProperties = allProperties.filter(
        (prop) => prop.id !== currentId
      );

      // 2. Mezclamos aleatoriamente las propiedades filtradas
      // Esta es una implementación simple del algoritmo de Fisher-Yates shuffle
      for (let i = filteredProperties.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [filteredProperties[i], filteredProperties[j]] = [
          filteredProperties[j],
          filteredProperties[i],
        ];
      }

      // 3. Tomamos las primeras 3 propiedades del array ya mezclado
      const randomSuggested = filteredProperties.slice(0, 3);

      // console.log("Propiedades sugeridas aleatorias:", randomSuggested);
      suggestedProperties.value = randomSuggested;
    } else {
      // console.log(
      //   "La estructura de datos de getInmueblesPaginados no es la esperada (falta .items.$values) o no es un array válido. Recibido:",
      //   responseData
      // );
      suggestedProperties.value = [];
    }
  } catch (err) {
    //console.error("Error al cargar propiedades sugeridas con paginación:", err);
    suggestedProperties.value = [];
  }
};

const loadFeaturedProperties = async () => {
  featuredPropertiesLoading.value = true;
  featuredPropertiesError.value = null;

  try {
    // Slugs específicos de las propiedades destacadas que deben mostrarse
    const featuredSlugs = [
      'casa-completamente-remodelada-en-venta-con-vistas-de-la-ciudad-zona-10-csv5506',
      'casa-de-un-nivel-en-venta-santa-catarina-pinula-cs5230',
      // 'casa-con-jardin-en-venta-en-siena-san-isidro-zona-16-csv5769',
      'casa-con-amplio-jardin-en-venta-hacienda-nueva-country-club-csv5342',
      'casa-en-venta-de-un-nivel-km-25-5-carretera-a-el-salvador-csv5591',
      'apartamento-en-venta-zona-10-asv5758',
      'apartamento-amueblado-en-venta-zona-10-aav5776'
    ];

    // Fetch all featured properties in parallel for faster loading
    const fetchPromises = featuredSlugs.map((slug) =>
      inmuebleService.getInmuebleBySlug(slug).catch((err) => {
        console.warn(`No se pudo cargar la propiedad con slug: ${slug}`, err);
        return null; // always resolve so Promise.all won't fail on a single request
      })
    );

    const resolved = await Promise.all(fetchPromises);
    const featuredPropertiesData = resolved
      .filter(Boolean)
      .map((property) => {
        const plainProperty = JSON.parse(JSON.stringify(property));
        return {
          ...plainProperty,
          imagenesReferencia:
            plainProperty.imagenesReferencia?.$values ||
            plainProperty.imagenesReferencia ||
            [],
          amenidades:
            plainProperty.amenidades?.$values || plainProperty.amenidades || [],
          precioActivo: plainProperty.precio != null && plainProperty.precio > 0,
        };
      });

    featuredProperties.value = featuredPropertiesData;

    if (featuredPropertiesData.length === 0) {
      featuredPropertiesError.value = "No se pudieron cargar las propiedades destacadas";
    }

  } catch (err) {
    console.error("Error al cargar propiedades destacadas:", err);
    featuredPropertiesError.value = "Error al cargar propiedades destacadas";
    featuredProperties.value = [];
  } finally {
    featuredPropertiesLoading.value = false;
  }
};

onMounted(async () => {
  // console.log("--- onMounted Iniciado ---");
  checkMobile();
  window.addEventListener("resize", checkMobile);

  // Cargar propiedades destacadas primero
  await loadFeaturedProperties();

  inmuebleId.value = route.params.id;
  //console.log("ID de la ruta (route.params.id):", inmuebleId.value);

  if (inmuebleId.value) {
    try {
      // console.log(
      //   `Intentando cargar detalles del inmueble con ID: ${inmuebleId.value}`
      // );
      const data = await inmuebleService.getInmuebleById(inmuebleId.value);
      //console.log("Detalles del inmueble principal recibidos:", data);

      if (data.imagenesReferencia && data.imagenesReferencia.$values) {
        data.imagenesReferencia = data.imagenesReferencia.$values;
      } else if (!data.imagenesReferencia) {
        data.imagenesReferencia = [];
      }
      if (data.amenidades && data.amenidades.$values) {
        data.amenidades = data.amenidades.$values;
      } else if (!data.amenidades) {
        data.amenidades = [];
      }
      inmuebleDetalle.value = data;
      //console.log("inmuebleDetalle.value actualizado.");

      // Inicializar mainMedia (para el carrusel principal de la propiedad en detalle)
      if (inmuebleDetalle.value.video) {
        mainMedia.value = { url: inmuebleDetalle.value.video, type: "video" };
      } else if (inmuebleDetalle.value.imagenPrincipal) {
        mainMedia.value = {
          url: inmuebleDetalle.value.imagenPrincipal,
          type: "image",
        };
      } else if (
        inmuebleDetalle.value.imagenesReferencia &&
        inmuebleDetalle.value.imagenesReferencia.length > 0
      ) {
        mainMedia.value = {
          url: inmuebleDetalle.value.imagenesReferencia[0].url,
          type: "image",
        };
      }
      //console.log("mainMedia inicializado:", mainMedia.value);

      const currentId = inmuebleDetalle.value.id;
      // console.log(
      //   "ID del inmueble cargado y listo para pasar a sugerencias:",
      //   currentId
      // );

      await loadSuggestedProperties(currentId);
    } catch (error) {
      // console.error(
      //   `ERROR: Error al cargar los detalles del inmueble ${inmuebleId.value}:`,
      //   error
      // );
    }
  } else {
    // console.warn(
    //   "No se encontró ID de inmueble en la ruta. No se cargarán detalles ni sugerencias por ID."
    // );
    await loadSuggestedProperties(null);
  }
  //console.log("--- onMounted Finalizado ---");
});

onUnmounted(() => {
  window.removeEventListener("resize", checkMobile);
});

const handleRedirectToPropiedades = (operacion) => {
  router.push({
    path: "/propiedades",
    query: {
      Operaciones: operacion.toLowerCase(), // "venta" o "renta"
    },
  });
};

// slug nuevo
const getSlug = (p) => {
  const s = p?.slugInmueble ?? p?.slug ?? p?.SlugInmueble ?? '';
  return encodeURIComponent(String(s).trim());
};

const getPropertyUrl = (p) => {
  const s = getSlug(p);
  return s ? `/inmueble/${s}` : '#';
};
</script>

<style scoped>
/* Estilos generales (mantienen su función) */
.description-content ul {
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
}
.description-content ul li {
  margin-left: 1.5rem;
}
.boton-optima {
  font-family: "Optima", serif;
  font-weight: 400;
  font-size: 16px;
  line-height: 1.3;
}
.precio-optima {
  font-family: "Optima", serif;
  font-weight: normal;
  font-size: 24px;
}
.precio-optima-d {
  font-family: "Optima", serif;
  font-weight: normal;
  font-size: 40px;
}

/* Swiper specific styles (mantienen su función) */
.swiper-pagination-bullet {
  background-color: #999;
  opacity: 0.7;
}
.swiper-pagination-bullet-active {
  background-color: #333;
  opacity: 1;
}

/* --- ESTILOS ESPECÍFICOS PARA EL CARRUSEL DE IMÁGENES --- */
.suggested-images-slider .swiper-slide {
  height: auto;
  display: flex;
  justify-content: center;
  align-items: center;
}

.suggested-images-slider .swiper-slide > a {
  position: relative;
  display: block;
  width: 100%;
  height: 100%;
}

.suggested-images-slider img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}

.suggested-images-slider .absolute.inset-0 {
  z-index: 10;
}

.suggested-images-slider .absolute.bottom-0 {
  z-index: 20;
}

.suggested-images-slider .absolute.top-4 {
  z-index: 20;
}

@media (max-width: 767px) {
  .suggested-images-slider .swiper-slide {
    width: 100% !important;
  }
  .suggested-images-slider {
    padding-bottom: 30px;
  }
}
</style>
