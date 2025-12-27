<template>
  <div class="homepage-section">
    <!-- Hero Section -->
    <section class="bg-gradient-to-b from-gray-50 to-white py-12 md:py-16">
      <div class="container mx-auto px-5">
        <h1 class="title-alta text-center text-4xl md:text-5xl lg:text-6xl font-bold text-gray-900 mb-6">
          Homes Guatemala - Bienes Raíces de Lujo
        </h1>
        <div class="max-w-4xl mx-auto text-center">
          <p class="long-text-roboto text-lg md:text-xl text-gray-700 mb-4 leading-relaxed">
            Somos líderes en <strong>bienes raíces de lujo en Guatemala (GT)</strong>, especializados en la 
            <strong>venta y renta de propiedades exclusivas</strong> en las zonas más prestigiosas de Ciudad de Guatemala. 
          </p>
          <div class="flex flex-wrap justify-center gap-4 mt-8">
            <NuxtLink to="/propiedades/venta" class="inline-flex boton-optima text-white bg-black border-0 py-4 px-8 hover:bg-gray-700 rounded-lg text-lg transition-colors">
              Ver Propiedades en Venta
            </NuxtLink>
            <NuxtLink to="/propiedades/renta" class="inline-flex boton-optima text-gray-900 bg-white border-2 border-gray-900 py-4 px-8 hover:bg-gray-100 rounded-lg text-lg transition-colors">
              Ver Propiedades en Renta
            </NuxtLink>
          </div>
        </div>
      </div>
    </section>

    <!-- Proyectos destacados -->
    <section class="py-12 px-5 bg-white">
      <div class="container mx-auto flex flex-col md:flex-row items-center">
        <div class="lg:max-w-lg lg:w-full md:w-1/2 w-5/6 mb-10 md:mb-0">
          <nuxt-img 
            preload
            src="https://app-pool.vylaris.online/dcmigserver/homes/f64ca937-cba0-4276-b350-9aca8a1b51bc.webp"
            class="object-cover object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
            alt="Proyectos inmobiliarios de lujo en Guatemala"
            width="512"
            height="300"
          />
        </div>
        <div class="lg:flex-grow md:w-1/2 lg:pl-24 md:pl-16 flex flex-col md:items-start md:text-left items-center text-center">
          <h2 class="title-font title-alta sm:text-4xl text-3xl mb-4 font-medium text-gray-900 uppercase">
            Proyectos Inmobiliarios de Lujo
          </h2>
          <p class="mb-4 leading-relaxed long-text-roboto">
            Descubre nuestros <strong>proyectos inmobiliarios más exclusivos</strong> en Ciudad de Guatemala, diseñados para un estilo de vida moderno y una inversión segura.
          </p>
          <NuxtLink to="/proyectos-inmobiliarios" class="inline-flex boton-optima text-white bg-black py-4 px-6 hover:bg-gray-600 rounded text-lg">
            Ver proyectos
          </NuxtLink>
        </div>
      </div>
    </section>

    <!-- Carrusel de Propiedades Destacadas (SSR REAL) -->
    <section class="py-16 bg-gray-50 overflow-hidden">
      <div class="container mx-auto px-5 text-center">
        <h2 class="title-alta-2 text-3xl md:text-4xl font-bold text-gray-900 mb-10">
          PROPIEDADES DESTACADAS
        </h2>
        
        <div v-if="featuredProperties.length > 0" class="featured-slider">
          <Swiper
            v-if="!isMobile"
            :modules="[Pagination, Autoplay]"
            :slides-per-view="3"
            :space-between="30"
            :pagination="{ clickable: true }"
            class="pb-12"
          >
            <SwiperSlide v-for="p in featuredProperties" :key="p.id">
              <div class="bg-white rounded-lg shadow-lg overflow-hidden h-full">
                <NuxtLink :to="getPropertyUrl(p)">
                   <img :src="p.imagenPrincipal" :alt="p.titulo" class="w-full h-64 object-cover" />
                   <div class="p-6">
                     <h3 class="text-xl font-bold mb-2">{{ p.titulo }}</h3>
                     <p class="text-gray-600 text-sm mb-4">{{ p.ubicaciones }}</p>
                     <p v-if="p.precio" class="text-2xl font-bold text-gray-900">${{ p.precio.toLocaleString() }}</p>
                   </div>
                </NuxtLink>
              </div>
            </SwiperSlide>
          </Swiper>
          
          <!-- Versión móvil (lista para ahorrar JS inicial si fuera necesario, pero por ahora Swiper) -->
          <div v-else class="grid grid-cols-1 gap-6">
            <div v-for="p in featuredProperties.slice(0, 4)" :key="p.id" class="bg-white rounded-lg shadow-lg overflow-hidden">
                <NuxtLink :to="getPropertyUrl(p)">
                   <img :src="p.imagenPrincipal" :alt="p.titulo" class="w-full h-64 object-cover" />
                   <div class="p-6">
                     <h3 class="text-xl font-bold mb-2">{{ p.titulo }}</h3>
                     <p class="text-gray-600 text-sm">{{ p.ubicaciones }}</p>
                   </div>
                </NuxtLink>
            </div>
          </div>
        </div>
        <div v-else class="text-center py-8 text-gray-500">
          No hay propiedades disponibles ahora.
        </div>
      </div>
    </section>

    <!-- FAQ Section -->
    <section class="bg-white py-16 px-5" itemscope itemtype="https://schema.org/FAQPage">
      <div class="container mx-auto max-w-4xl">
        <h2 class="title-alta-2 text-3xl md:text-4xl text-center font-bold text-gray-900 mb-10">
          Preguntas Frecuentes sobre Bienes Raíces en Guatemala
        </h2>
        <div class="space-y-6">
          <div v-for="(faq, i) in faqs" :key="i" itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
               class="bg-gray-50 rounded-lg p-6">
            <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">{{ faq.q }}</h3>
            <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
              <p itemprop="text" class="text-gray-700 leading-relaxed" v-html="faq.a"></p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, onUnmounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import inmuebleService from "~/services/inmuebleService";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Pagination, Autoplay } from "swiper/modules";
import "swiper/css";
import "swiper/css/pagination";

// --- ⚡️ FIX DRÁSTICO PARA SEO (SSR REAL) ---
const { data: featuredData } = await useAsyncData('home-featured', () => 
  inmuebleService.getInmueblesPaginados(1, 10, { Destacado: true })
);

const featuredProperties = computed(() => featuredData.value?.items || []);

const isMobile = ref(false);
const router = useRouter();

const checkMobile = () => {
  if (process.client) {
    isMobile.value = window.innerWidth < 768;
  }
};

onMounted(() => {
  checkMobile();
  window.addEventListener("resize", checkMobile);
});

onUnmounted(() => {
  if (process.client) {
    window.removeEventListener("resize", checkMobile);
  }
});

const getSlug = (p) => {
  return encodeURIComponent(String(p?.slugInmueble || p?.slug || '').trim());
};

const getPropertyUrl = (p) => {
  const s = getSlug(p);
  return s ? `/inmueble/${s}` : '#';
};

const faqs = [
  { q: '¿Cuáles son las mejores zonas para comprar propiedades de lujo en Guatemala?', a: 'Las zonas exclusivas incluyen <strong>Zona 10, 14, 15, 16 y Carretera a El Salvador</strong>.' },
  { q: '¿Cuánto cuesta una casa de lujo en Guatemala?', a: 'Los precios varían entre <strong>$350,000 USD y más de $2,000,000 USD</strong>.' },
  { q: '¿Qué servicios ofrece Homes Guatemala?', a: 'Ofrecemos <strong>asesoría personalizada</strong>, marketing premium y gestión legal.' }
];

// Meta tags optimizados
useSeoMeta({
  title: 'Homes Guatemala | Bienes Raíces de Lujo - Casas y Apartamentos',
  description: 'Descubre las mejores propiedades de lujo en Guatemala con Homes Guatemala. Venta y renta en las zonas más exclusivas: 10, 14, 15 y 16.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp'
});
</script>

<style scoped>
.title-alta { font-family: "Raleway", sans-serif; font-weight: 300; }
.title-alta-2 { font-family: "Raleway", sans-serif; font-weight: 300; }
.boton-optima { font-family: "Optima", serif; font-weight: 400; }
</style>
