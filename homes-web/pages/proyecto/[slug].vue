<template>

    <Header />
    
    <!-- Breadcrumbs -->
    <div class="max-w-[1080px] mx-auto px-4 mt-24 lg:mt-28 mb-4" v-if="!isLoading && !notFound && proyectoDetalle">
      <Breadcrumbs 
        :breadcrumbs="[
          { name: 'Inicio', url: '/' },
          { name: 'Proyectos', url: '/proyectos-inmobiliarios' },
          { name: proyectoDetalle.titulo || 'Proyecto', url: route.fullPath }
        ]" 
      />
    </div>

    <!-- <Loader v-if="isLoading" /> -->
    <SlugSkeleton v-if="isLoading" />

    <!--  Mensaje cuando el proyecto no existe -->
  <div
    v-if="!isLoading && notFound"
    class="max-w-[1080px] mx-auto p-6 bg-white mt-24 lg:mt-12 text-center"
  >
    <h2 class="text-2xl font-semibold mb-2">El proyecto no existe</h2>
    <p class="text-gray-600 mb-6">
      Es posible que el enlace esté mal escrito o que el proyecto haya sido dado de baja.
    </p>
    <NuxtLink
      to="/proyectos-inmobiliarios"
      class="inline-flex items-center px-6 py-3 rounded-md bg-black text-white hover:bg-gray-700"
    >
      Ver proyectos disponibles
    </NuxtLink>
  </div>

    <div v-if="!isLoading && !notFound" class="max-w-[1080px] mx-auto p-4 bg-white mt-24 lg:mt-12">
      <div class="mb-5 mt-5">
        <h1
          ref="tituloRef"
          class="title-alta-2 text-xl font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl"
        >
          {{ proyectoDetalle.titulo || "Cargando..." }}
        </h1>
      </div>
  
      <div
        class="grid grid-cols-1 md:grid-cols-6 md:grid-rows-3 gap-8 h-auto md:h-[600px]"
      >
        <div class="md:col-span-5 md:row-span-3 relative">
          <button
            @click="prevMedia"
            class="absolute left-0 top-1/2 transform -translate-y-1/2 z-10 bg-white hover:bg-gray-200 rounded-full shadow-md p-2 md:p-3 focus:outline-none"
            aria-label="Contenido anterior"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              class="w-5 h-5 md:w-6 md:h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M15 19l-7-7 7-7"
              />
            </svg>
          </button>
  
          <div class="w-full h-auto md:h-[600px] relative">
            <img
              v-if="!showVideo"
              :src="getOptimizedImageUrl(mainImage)"
              :data-original-src="mainImage"
              class="w-full h-full object-cover"
              alt="Imagen principal del proyecto"
              @error="handleImageError($event)"
            />
  
            <div
              v-if="showVideo && proyectoDetalle.video && videoEmbedUrl"
              class="w-full h-full flex items-center justify-center bg-gray-900"
            >
              <iframe
                :src="videoEmbedUrl"
                frameborder="0"
                allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                allowfullscreen
                class="w-full h-full"
              ></iframe>
            </div>
          </div>
  
          <button
            @click="nextMedia"
            class="absolute right-0 top-1/2 transform -translate-y-1/2 z-10 bg-white hover:bg-gray-200 rounded-full shadow-md p-2 md:p-3 focus:outline-none"
            aria-label="Contenido siguiente"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              class="w-5 h-5 md:w-6 md:h-6"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 5l7 7-7 7"
              />
            </svg>
          </button>
        </div>
  
        <div class="md:col-start-6 p-2 md:p-0 relative">
          <swiper
            ref="mySwiper"
            :direction="isMobile ? 'horizontal' : 'vertical'"
            :slides-per-view="isMobile ? 3 : 4"
            :space-between="8"
            :mousewheel="!isMobile"
            :free-mode="true"
            :loop="false"
            :modules="[FreeMode, Mousewheel, Navigation]"
            class="lg:h-[600px] md:h-[600px] w-full"
          >
            <swiper-slide
              v-if="proyectoDetalle.imagenPrincipal"
              class="swiper-slide-custom"
              :class="{ 'border-selected': !showVideo && mainImage === proyectoDetalle.imagenPrincipal }"
              @click="setMainContent(proyectoDetalle.imagenPrincipal, 'image')"
            >
              <div class="slide-container">
                <img
                  :src="getOptimizedImageUrl(proyectoDetalle.imagenPrincipal)"
                  :data-original-src="proyectoDetalle.imagenPrincipal"
                  alt="Miniatura de la imagen principal"
                  class="slide-image"
                  loading="lazy"
                  @error="handleImageError($event)"
                />
              </div>
            </swiper-slide>
  
            <swiper-slide
              v-for="(image, index) in (proyectoDetalle?.imagenesReferenciaProyecto || [])"
              :key="`ref-img-${index}`"
              class="swiper-slide-custom"
              :class="{ 'border-selected': !showVideo && mainImage === image.url }"
              @click="setMainContent(image.url, 'image')"
            >
              <div class="slide-container">
                <img
                  :src="getOptimizedImageUrl(image.url)"
                  :data-original-src="image.url"
                  :alt="`Imagen de referencia ${index + 1} del proyecto`"
                  class="slide-image"
                  loading="lazy"
                  @error="handleImageError($event)"
                />
              </div>
            </swiper-slide>
  
            <swiper-slide
              v-if="proyectoDetalle.video && proyectoDetalle.video !== 'string'"
              class="swiper-slide-custom swiper-slide-video"
              :class="{ 'border-selected': showVideo }"
              @click="setMainContent(proyectoDetalle.video, 'video')"
            >
              <div class="slide-container video-container">
                <img
                  v-if="videoThumbnail"
                  :src="videoThumbnail"
                  alt="Miniatura del video del proyecto"
                  class="slide-image"
                  loading="lazy"
                  @error="handleImageError($event)"
                />
                <div v-else class="video-placeholder">
                  <svg
                    class="video-icon"
                    fill="currentColor"
                    viewBox="0 0 20 20"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      fill-rule="evenodd"
                      d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832L12.75 10.2A1 1 0 0012.75 9.8L9.555 7.168z"
                      clip-rule="evenodd"
                    ></path>
                  </svg>
                </div>
                <span class="video-label">Video</span>
              </div>
            </swiper-slide>
          </swiper>
        </div>
      </div>
  
      <div class="grid grid-cols-1 md:grid-cols-3 md:grid-rows-1 gap-4">
        <div class="order-first md:order-last md:col-span-1">
          <div class="mb-5 mt-8 md:mt-14 lg:mt-14 space-y-3">
            <h1
              class="precio-optima-d text-xl text-center font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl"
            >
              {{ formattedPrice }}
            </h1>
  
            <h1
              class="subtitle-optima text-lg text-center font-regular leading-none tracking-tight text-gray-900 md:text-2xl lg:text-3xl bg-gray-200 rounded-md p-3"
            >
              Código: {{ proyectoDetalle.codigoProyecto }}
            </h1>
            <div class="flex flex-wrap justify-center gap-2 mt-4 mb-4">
              <span
                v-for="(amenidad, index) in proyectoDetalle.amenidades"
                :key="index"
                class="bg-gray-100 text-gray-800 text-xs font-medium px-2.5 py-0.5 rounded-sm"
                >{{ amenidad.nombre }}</span
              >
            </div>
  
            <hr class="w-48 mx-auto text-gray-300" />
            <p class="mb-8 leading-relaxed long-text-roboto text-center">
              Contáctenos para más Información
            </p>
            <div class="flex justify-center">
              <a
                :href="whatsappLink"
                target="_blank"
                rel="noopener noreferrer"
                class="inline-flex boton-optima text-white bg-black border-0 py-2 px-6 focus:outline-none hover:bg-gray-600 rounded text-lg"
              >
              <svg
                class="w-5 h-5 me-2 text-white"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  fill-rule="evenodd"
                  d="M12 4a8 8 0 0 0-6.895 12.06l.569.718-.697 2.359 2.32-.648.379.243A8 8 0 1 0 12 4ZM2 12C2 6.477 6.477 2 12 2s10 4.477 10 10-4.477 10-10 10a9.96 9.96 0 0 1-5.016-1.347l-4.948 1.382 1.426-4.829-.006-.007-.033-.055A9.958 9.958 0 0 1 2 12Z"
                  clip-rule="evenodd"
                />
                <path
                  fill="currentColor"
                  d="M16.735 13.492c-.038-.018-1.497-.736-1.756-.83a1.008 1.008 0 0 0-.34-.075c-.196 0-.362.098-.49.291-.146.217-.587.732-.723.886-.018.02-.042.045-.057.045-.013 0-.239-.093-.307-.123-1.564-.68-2.751-2.313-2.914-2.589-.023-.04-.024-.057-.024-.057.005-.021.058-.074.085-.101.08-.079.166-.182.249-.283l.117-.14c.121-.14.175-.25.237-.375l.033-.066a.68.68 0 0 0-.02-.64c-.034-.069-.65-1.555-.715-1.711-.158-.377-.366-.552-.655-.552-.027 0 0 0-.112.005-.137.005-.883.104-1.213.311-.35.22-.94.924-.94 2.16 0 1.112.705 2.162 1.008 2.561l.041.06c1.161 1.695 2.608 2.951 4.074 3.537 1.412.564 2.081.63 2.461.63.16 0 .288-.013.4-.024l.072-.007c.488-.043 1.56-.599 1.804-1.276.192-.534.243-1.117.115-1.329-.088-.144-.239-.216-.43-.308Z"
                />
              </svg>

              Más Información
              </a>
            </div>
          </div>
        </div>
  
        <div class="md:col-span-2 md:order-first">
          <div class="mb-5 mt-1 md:mt-14 lg:mt-14 space-y-3">
            <h1
              class="subtitle-optima text-xl font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl"
            >
              {{ proyectoDetalle.titulo || "Cargando..." }}
            </h1>
  
            <div class="prose max-w-none description-content">
            <div v-html="formattedDescription"></div>
          </div>
          </div>
        </div>
      </div>
  
      <div v-if="suggestedProperties && suggestedProperties.length > 0" class="mt-12 mb-12">
    <h1
      class="title-alta-2 text-xl text-center font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl mt-8 mb-8"
    >
      Proyectos Sugeridos 
    </h1>
  
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
      <SugerenciaCard
        v-for="proyecto in suggestedProperties"
        :key="proyecto.id"
        :item="proyecto"
        type="proyecto" />
    </div>
  </div>
  
  <div v-else class="mt-12 mb-12 text-center">
    <p class="text-gray-600 text-lg">
        No hay proyectos sugeridos disponibles.
    </p>
  </div>
    </div>
    <RedesFlotantes />
    <Footer />
  </template>
  
  <script setup>
import { ref, computed, onMounted, nextTick, watch } from "vue";
import { useRoute, useAsyncData, createError, useHead, useSeoMeta } from "#imports";
import proyectoService from "../../services/proyectoService";
import Header from "../../components/header.vue";
import Footer from "../../components/footer.vue";
import RedesFlotantes from "../../components/redesFlotantes.vue";
import Loader from "../../components/Loader.vue";
import SugerenciaCard from "~/components/SugerenciaCard.vue";
import slugSkeleton from "~/components/slugSkeleton.vue";
import Breadcrumbs from "~/components/Breadcrumbs.vue";
import { useProjectSchema, useBreadcrumbSchema, useJsonldSchema } from "~/composables/useStructuredData";

// Swiper
import { Swiper, SwiperSlide } from "swiper/vue";
import "swiper/css";
import "swiper/css/free-mode";
import "swiper/css/mousewheel";
import "swiper/css/navigation";
import { FreeMode, Mousewheel, Navigation } from "swiper/modules";

// ===== Variables reactivas =====
const isMobile = ref(false);
const route = useRoute();
const proyectoDetalle = ref(null);
const mainImage = ref("");
const currentMediaIndex = ref(0);
const showVideo = ref(false);
const videoThumbnail = ref("");
const notFound = ref(false);
const suggestedProperties = ref([]);
const tituloRef = ref(null);

// ===== Funciones para manejo de media, navegación y helpers (declaradas al inicio) =====
const toSlug = (str = "") =>
  String(str)
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .trim()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "");

const normalizeProject = (p = {}) => {
  const incoming = p.slugProyecto ?? p.slug ?? p.SlugProyecto ?? p.Slug ?? "";
  const finalSlug =
    incoming && String(incoming).trim()
      ? String(incoming).trim()
      : toSlug(p.titulo || p.Titulo || "");
  return { ...p, slugProyecto: finalSlug, slug: finalSlug };
};
const hasSlug = (p) => !!String(p?.slugProyecto || p?.slug || "").trim();

const getVideoThumbnail = (url) => {
  const youtubeMatch = url?.match(
    /(?:https?:\/\/)?(?:www\.)?(?:m\.)?(?:youtube\.com|youtu\.be)\/(?:watch\?v=|embed\/|v\/|)([\w-]{11})(?:\S+)?/
  );
  if (youtubeMatch?.[1]) return `https://img.youtube.com/vi/${youtubeMatch[1]}/hqdefault.jpg`;
  const vimeoMatch = url?.match(
    /(?:https?:\/\/)?(?:www\.)?vimeo\.com\/(?:.*\/)?(\d+)(?:\S+)?/
  );
  if (vimeoMatch?.[1]) return `https://vumbnail.com/${vimeoMatch[1]}.jpg`;
  return null;
};

const getOptimizedImageUrl = (url) => {
  if (!url) return 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMzAwIiBoZWlnaHQ9IjMwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMzAwIiBoZWlnaHQ9IjMwMCIgZmlsbD0iI2Y5ZmFmYiIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LWZhbWlseT0iQXJpYWwiIGZvbnQtc2l6ZT0iMTgiIGZpbGw9IiM2Yjc0ODIiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGR5PSIuM2VtIj5JbcOhZ2VuPC90ZXh0Pjwvc3ZnPg==';
  
  // Si ya es una URL completa, la devolvemos tal como está
  if (url.startsWith('http://') || url.startsWith('https://')) {
    return url;
  }
  
  // Si es una URL relativa, construimos la URL completa
  const DOMINIO_IMAGENES = "https://app-pool.vylaris.online/dcmigserver/homes";
  return `${DOMINIO_IMAGENES}/${url}`;
};

const handleImageError = (event) => {
  // Fallback image cuando la imagen no carga (SVG placeholder)
  event.target.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMzAwIiBoZWlnaHQ9IjMwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iMzAwIiBoZWlnaHQ9IjMwMCIgZmlsbD0iI2Y5ZmFmYiIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LWZhbWlseT0iQXJpYWwiIGZvbnQtc2l6ZT0iMTgiIGZpbGw9IiM2Yjc0ODIiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGR5PSIuM2VtIj5Jw6FnZW48L3RleHQ+PC9zdmc+';
  console.warn('Error loading image:', event.target.getAttribute('data-original-src') || 'unknown');
};

const setMainContent = (content, type) => {
  if (type === "image") {
    mainImage.value = content;
    showVideo.value = false;
    videoThumbnail.value = "";
  } else {
    mainImage.value = "";
    showVideo.value = true;
    videoThumbnail.value = getVideoThumbnail(content);
  }
  currentMediaIndex.value = allMedia.value.findIndex((m) => m.url === content);
};

const prevMedia = () => {
  if (!allMedia.value.length) return;
  currentMediaIndex.value =
    (currentMediaIndex.value - 1 + allMedia.value.length) % allMedia.value.length;
  const m = allMedia.value[currentMediaIndex.value];
  setMainContent(m.url, m.type);
};

const nextMedia = () => {
  if (!allMedia.value.length) return;
  currentMediaIndex.value = (currentMediaIndex.value + 1) % allMedia.value.length;
  const m = allMedia.value[currentMediaIndex.value];
  setMainContent(m.url, m.type);
};

// ===== Carga de datos asíncrona con useAsyncData para SSR/SEO =====
const slug = route.params.slug;

const {
  data: fetchedProyecto,
  pending: isLoading,
  error,
} = await useAsyncData(`proyecto-${slug}`, async () => {
  try {
    if (!slug || typeof slug !== "string" || !slug.trim() || slug === "no-disponible") {
      throw createError({ statusCode: 404, statusMessage: "Proyecto no encontrado", fatal: true });
    }
    const data = await proyectoService.getProyectoBySlug(slug);
    if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
      throw createError({ statusCode: 404, statusMessage: "Proyecto no encontrado", fatal: true });
    }
    // Normaliza los datos de la API para amenidades e imágenes
    if (Array.isArray(data.amenidades)) {
      // ya es array
    } else {
      data.amenidades = [];
    }
    if (Array.isArray(data.imagenesReferenciaProyecto)) {
      // ya es array
    } else {
      data.imagenesReferenciaProyecto = [];
    }
    return data;
  } catch (err) {
    throw createError({
      statusCode: err.statusCode || 404,
      statusMessage: err.statusMessage || "Error al cargar el proyecto",
      fatal: true,
    });
  }
});

// Asigna los datos obtenidos a la variable reactiva
proyectoDetalle.value = fetchedProyecto.value;
notFound.value = !!error.value || !proyectoDetalle.value;

// ===== Propiedades computadas para SEO y la vista =====
const pageTitle = computed(() => {
  if (!proyectoDetalle.value?.titulo) return "Proyecto en Guatemala | Homes Guatemala";
  
  const titulo = proyectoDetalle.value.titulo;
  const precio = formattedPrice.value;
  const ubicacion = proyectoDetalle.value.zona || proyectoDetalle.value.ubicacion || '';
  
  let fullTitle = titulo;
  if (precio) {
    fullTitle += ` - ${precio}`;
  }
  if (ubicacion) {
    fullTitle += ` | ${ubicacion}`;
  }
  fullTitle += " | Homes Guatemala";
  
  return fullTitle;
});

const pageDescription = computed(() => {
  if (!proyectoDetalle.value) {
    return "Descubre increíbles proyectos inmobiliarios en Guatemala. Encuentra tu hogar ideal con Homes Guatemala.";
  }
  
  const titulo = proyectoDetalle.value.titulo || 'Proyecto';
  const precio = formattedPrice.value || '';
  const ubicacion = proyectoDetalle.value.zona || proyectoDetalle.value.ubicacion || 'Guatemala';
  const codigo = proyectoDetalle.value.codigoProyecto || '';
  
  let description = `${titulo} en ${ubicacion}`;
  
  // if (precio) {
  //   description += ` por ${precio}`;
  // }
  
  // if (codigo) {
  //   description += ` (Código: ${codigo})`;
  // }
  
  // Agregar contenido si existe
  if (proyectoDetalle.value.contenido) {
    const cleanText = proyectoDetalle.value.contenido
      .replace(/<[^>]*>?/gm, " ")
      .replace(/\s+/g, " ")
      .trim();
    
    const remainingLength = 155 - description.length - 3; // 3 for " - "
    if (remainingLength > 20 && cleanText.length > 0) {
      const excerpt = cleanText.length > remainingLength 
        ? cleanText.substring(0, remainingLength - 3) + "..." 
        : cleanText;
      description += ` - ${excerpt}`;
    }
  } else {
    description += " - Descubre más sobre este increíble proyecto inmobiliario.";
  }
  
  // Asegurar que no exceda 155 caracteres
  return description.length > 155 ? description.substring(0, 155) + "..." : description;
});

const pageImage = computed(() => {
  const DOMINIO_IMAGENES = "https://app-pool.vylaris.online/dcmigserver/homes";
  const img = proyectoDetalle.value?.imagenPrincipal;
  
  if (!img) {
    // Imagen por defecto de Homes Guatemala (mantener extensión original)
    return `${DOMINIO_IMAGENES}/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp`;
  }
  // Si ya es una URL completa, la devolvemos tal como está
  if (img.startsWith("http://") || img.startsWith("https://")) {
    return img;
  }
  // Si es una URL relativa, construimos la URL completa sin modificar la extensión
  let cleanImg = img.trim();
  cleanImg = cleanImg.startsWith('/') ? cleanImg.substring(1) : cleanImg;
  const encodedImg = encodeURI(cleanImg);
  return `${DOMINIO_IMAGENES}/${encodedImg}`;
});

const propertyUrl = computed(() => {
  const baseUrl = 'https://homesguatemala.com';
  const fullPath = route.fullPath || route.path || '';
  
  // Asegurar que la URL del proyecto sea específica
  if (!fullPath || fullPath === '/') {
    return `${baseUrl}/proyecto/${slug}`;
  }
  
  return `${baseUrl}${fullPath}`;
});

const formattedPrice = computed(() => {
  if (!proyectoDetalle.value?.precio) return "";
  const number = parseInt(proyectoDetalle.value.precio, 10);
  return new Intl.NumberFormat("es-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(number);
});

// ===== SEO y Meta Tags =====
// Debug: verificar valores en desarrollo
if (process.dev) {
  console.log('SEO Debug:', {
    title: pageTitle.value,
    description: pageDescription.value,
    image: pageImage.value,
    url: propertyUrl.value,
    slug: slug
  });
}

// Meta tags específicas para SEO y redes sociales
useSeoMeta({
  title: pageTitle,
  description: pageDescription,
  ogTitle: pageTitle,
  ogDescription: pageDescription,
  ogImage: pageImage,
  ogImageSecureUrl: pageImage,
  ogImageWidth: '1200',
  ogImageHeight: '630',
  // El tipo de imagen se puede dejar vacío o dinámico si se requiere, pero Open Graph lo detecta automáticamente
  ogImageAlt: pageTitle,
  ogUrl: propertyUrl,
  ogType: 'article',
  ogSiteName: 'Homes Guatemala',
  ogLocale: 'es_GT',
  twitterCard: 'summary_large_image',
  twitterTitle: pageTitle,
  twitterDescription: pageDescription,
  twitterImage: pageImage,
  twitterImageAlt: pageTitle,
  twitterUrl: propertyUrl,
  robots: 'index, follow',
  author: 'Homes Guatemala',
  articlePublisher: 'https://homesguatemala.com',
  articleAuthor: 'Homes Guatemala'
});

useHead({
  title: pageTitle,
  link: [
    {
      rel: 'canonical',
      href: propertyUrl
    }
  ],
  meta: [
    // Meta tags adicionales para WhatsApp
    { property: 'og:image:secure_url', content: pageImage },
    { name: 'thumbnail', content: pageImage }
  ]
});

const allMedia = computed(() => {
  const media = [];
  if (proyectoDetalle.value?.imagenPrincipal)
    media.push({ url: pageImage.value, type: "image" });
  if (proyectoDetalle.value?.imagenesReferenciaProyecto && proyectoDetalle.value.imagenesReferenciaProyecto.length > 0)
    media.push(...proyectoDetalle.value.imagenesReferenciaProyecto.map((img) => ({ url: img.url, type: "image" })));
  if (proyectoDetalle.value?.video && proyectoDetalle.value.video !== "string")
    media.push({ url: proyectoDetalle.value.video, type: "video", thumbnail: getVideoThumbnail(proyectoDetalle.value.video) });
  return media;
});

const videoEmbedUrl = computed(() => {
  const videoUrl = proyectoDetalle.value?.video;
  if (!videoUrl || videoUrl === "string") return "";
  const yt = videoUrl.match(/(?:youtu\.be\/|youtube\.com\/.*v=)([\w-]{11})/);
  if (yt?.[1]) return `https://www.youtube.com/embed/${yt[1]}`;
  const vm = videoUrl.match(/vimeo\.com\/(\d+)/);
  if (vm?.[1]) return `https://player.vimeo.com/video/${vm[1]}`;
  return "";
});

const formattedDescription = computed(() => {
  const c = proyectoDetalle.value?.contenido ?? "";
  const looksLikeHtml = /<\s*(p|br|ul|ol|li|h[1-6]|div|span)\b/i.test(c);
  if (looksLikeHtml) return c;
  return c
    .split(/\r?\n\r?\n/)
    .map((p) => `<p>${p.replace(/\r?\n/g, "<br>")}</p>`)
    .join("");
});

const whatsappLink = computed(() => {
  const phoneNumber = "50256330961";
  const titulo = proyectoDetalle.value?.titulo || 'Proyecto';
  const url = propertyUrl.value;
  
  const message = `Estoy interesado en esta propiedad: ${titulo}\n\n${url}`;
  
  return `https://api.whatsapp.com/send/?phone=${phoneNumber}&text=${encodeURIComponent(message)}`;
});

// Schema.org structured data for SEO
const projectSchema = computed(() => {
  if (!proyectoDetalle.value) return null;
  return useProjectSchema(proyectoDetalle.value);
});

const breadcrumbSchema = computed(() => {
  if (!proyectoDetalle.value) return null;
  return useBreadcrumbSchema([
    { name: 'Inicio', url: 'https://homesguatemala.com' },
    { name: 'Proyectos', url: 'https://homesguatemala.com/proyectos-inmobiliarios' },
    { name: proyectoDetalle.value.titulo || 'Proyecto', url: propertyUrl.value }
  ]);
});

watch([projectSchema, breadcrumbSchema], ([project, breadcrumb]) => {
  if (project) useJsonldSchema(project);
  if (breadcrumb) useJsonldSchema(breadcrumb);
}, { immediate: true });

watch(
  allMedia,
  (newMedia) => {
    if (newMedia && newMedia.length > 0) {
      const m = newMedia[currentMediaIndex.value] ?? newMedia[0];
      setMainContent(m.url, m.type);
    } else {
      mainImage.value = "";
      showVideo.value = false;
      videoThumbnail.value = "";
    }
  },
  { immediate: true }
);

// ===== Lógica de sugerencias y montaje de cliente =====
const loadSuggestedProperties = async () => {
  try {
    const data = await proyectoService.getProyecto();
    const items = Array.isArray(data) ? data : [];
    if (!items.length) {
      suggestedProperties.value = [];
      return;
    }

    const current = proyectoDetalle.value;
    const currentId = current?.id;
    const currentZone = current?.zona ?? current?.ubicaciones ?? current?.ubicacion ?? current?.zonaProyecto ?? null;
    const currentPrice = (() => {
      const p = current?.precio ?? current?.precioProyecto ?? current?.valor ?? current?.price ?? null;
      if (p == null) return null;
      const parsed = parseFloat(String(p).replace(/[^0-9.-]+/g, ""));
      return Number.isFinite(parsed) ? parsed : null;
    })();

    const available = items.filter((p) => p.id !== currentId);

    const getZone = (p) => p?.zona ?? p?.ubicaciones ?? p?.ubicacion ?? p?.zonaProyecto ?? null;
    const getPrice = (p) => {
      const v = p?.precio ?? p?.precioProyecto ?? p?.valor ?? p?.price ?? null;
      if (v == null) return null;
      const num = parseFloat(String(v).replace(/[^0-9.-]+/g, ""));
      return Number.isFinite(num) ? num : null;
    };

    const isPriceSimilar = (a, b) => {
      if (a == null || b == null) return false;
      const max = Math.max(a, b);
      const min = Math.min(a, b);
      if (min === 0) return false;
      return (max - min) / min <= 0.2; // 20% threshold
    };

    // 1) misma zona y precio similar
    const zoneAndPriceMatch = available.filter((p) => {
      const z = getZone(p);
      const pPrice = getPrice(p);
      return z && currentZone && z === currentZone && isPriceSimilar(pPrice, currentPrice);
    });

    // 2) misma zona, precio no similar
    const zoneMatch = available.filter((p) => {
      const z = getZone(p);
      const pPrice = getPrice(p);
      return z && currentZone && z === currentZone && !isPriceSimilar(pPrice, currentPrice);
    });

    // 3) precio similar, pero diferente zona
    const priceMatch = available.filter((p) => {
      const z = getZone(p);
      const pPrice = getPrice(p);
      const sameZone = z && currentZone && z === currentZone;
      return !sameZone && isPriceSimilar(pPrice, currentPrice);
    });

    // Combine in priority order and dedupe by id
    const combined = [];
    const seen = new Set();
    for (const list of [zoneAndPriceMatch, zoneMatch, priceMatch]) {
      for (const item of list) {
        if (!seen.has(item.id)) {
          combined.push(item);
          seen.add(item.id);
        }
      }
    }

    // If not enough, fill with random items from the remaining pool
    if (combined.length < 3) {
      const remaining = available.filter((p) => !seen.has(p.id));
      remaining.sort(() => Math.random() - 0.5);
      for (const r of remaining) {
        if (combined.length >= 3) break;
        combined.push(r);
        seen.add(r.id);
      }
    }

    // Normalize and keep only items with slugs
    suggestedProperties.value = combined
      .map((p) => normalizeProject(p))
      .filter(hasSlug)
      .slice(0, 3);
  } catch (e) {
    suggestedProperties.value = [];
  }
};

onMounted(async () => {
  const checkMobile = () => (isMobile.value = window.innerWidth < 768);
  checkMobile();
  window.addEventListener("resize", checkMobile);
  await loadSuggestedProperties();

  // Hacer scroll solo si la ruta corresponde a la página de proyecto
  nextTick(() => {
    if (route?.path && route.path.includes("/proyecto") && tituloRef.value) {
      tituloRef.value.scrollIntoView({ behavior: "smooth", block: "start" });
    }
  });
});
</script>

  
  <style scoped>
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

  /* Estilos para el editor de texto y listas */
.description-content :deep(p) {
  margin: 0 0 1rem !important;
  line-height: 1.65;
}

.description-content :deep(p + p) {
  margin-top: 1rem !important;
}

.description-content :deep(ul),
.description-content :deep(ol) {
  padding-left: 1.5rem !important;
  margin: 0 0 1rem !important;
}

.description-content :deep(ul) {
  list-style: disc outside !important;
}

.description-content :deep(ol) {
  list-style: decimal outside !important;
}

.description-content :deep(li) {
  display: list-item !important;
  margin: 0.25rem 0;
}

.description-content :deep(li p) {
  margin: 0 !important;
}


/* Estilos personalizados para Swiper slides */
.swiper-slide-custom {
  height: 96px !important;
  width: 96px !important;
  cursor: pointer;
  border-radius: 0.375rem;
  overflow: hidden;
  transition: all 0.2s ease;
}

@media (min-width: 1024px) {
  .swiper-slide-custom {
    height: 144px !important;
    width: 144px !important;
  }
}

.swiper-slide-custom:hover {
  transform: scale(1.05);
}

.border-selected {
  border: 2px solid #6b7280;
  box-shadow: 0 0 0 1px rgba(107, 114, 128, 0.2);
}

.slide-container {
  width: 100%;
  height: 100%;
  position: relative;
  overflow: hidden;
  border-radius: 0.375rem;
}

.slide-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.swiper-slide-video {
  background-color: #f3f4f6;
}

.video-container {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #e5e7eb;
}

.video-placeholder {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 100%;
  background-color: #e5e7eb;
}

.video-icon {
  width: 48px;
  height: 48px;
  color: #6b7280;
}

.video-label {
  position: absolute;
  bottom: 4px;
  left: 50%;
  transform: translateX(-50%);
  font-size: 0.75rem;
  color: #374151;
  background-color: rgba(255, 255, 255, 0.9);
  padding: 2px 6px;
  border-radius: 0.25rem;
  font-weight: 500;
}

/* Asegurar que el swiper tenga el alto correcto */
.swiper {
  width: 100%;
}

.swiper-wrapper {
  align-items: flex-start;
}
  
  @import "swiper/css";
  @import "swiper/css/free-mode";
  @import "swiper/css/mousewheel";
  @import "swiper/css/navigation";
  </style>