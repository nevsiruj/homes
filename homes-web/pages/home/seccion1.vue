<template>
  <div class="homepage-section">
    <!-- 1. HERO SECTION -->
    <section class="bg-gradient-to-b from-gray-50 to-white py-12 md:py-16">
      <div class="container mx-auto px-5">
        <h1 class="title-alta text-center text-4xl md:text-5xl lg:text-6xl font-bold text-gray-900 mb-6 uppercase">
          Homes Guatemala - Bienes Raíces de Lujo
        </h1>
        <div class="max-w-4xl mx-auto text-center">
          <p class="long-text-roboto text-lg md:text-xl text-gray-700 mb-4 leading-relaxed">
            Somos líderes en <strong>bienes raíces de lujo en Guatemala (GT)</strong>, especializados en la 
            <strong>venta y renta de propiedades exclusivas</strong> en las zonas más prestigiosas de Ciudad de Guatemala. 
          </p>
          <div class="flex flex-wrap justify-center gap-4 mt-8">
            <NuxtLink to="/propiedades/venta" class="inline-flex boton-optima text-white bg-black border-0 py-4 px-8 hover:bg-gray-700 rounded-lg text-lg transition-colors">
              Casas y Apartamentos en Venta
            </NuxtLink>
            <NuxtLink to="/propiedades/renta" class="inline-flex boton-optima text-gray-900 bg-white border-2 border-gray-900 py-4 px-8 hover:bg-gray-100 rounded-lg text-lg transition-colors">
              Casas y Apartamentos en Renta
            </NuxtLink>
          </div>
        </div>
      </div>
    </section>

    <!-- 2. SECCIÓN PROYECTOS -->
    <section class="text-gray-600 body-font mt-12 pb-12">
      <div class="container mx-auto flex px-5 md:flex-row flex-col items-center">
        <div class="lg:max-w-lg lg:w-full md:w-1/2 w-5/6 mb-10 md:mb-0">
          <img 
            src="https://app-pool.vylaris.online/dcmigserver/homes/f64ca937-cba0-4276-b350-9aca8a1b51bc.webp"
            class="object-cover object-center rounded lg:h-[300px] md:h-[300px] h-[250px] w-full shadow-lg"
            alt="Proyectos inmobiliarios destacados en Guatemala"
            loading="lazy"
          />
        </div>
        <div class="lg:flex-grow md:w-1/2 lg:pl-24 md:pl-16 flex flex-col md:items-start md:text-left items-center text-center">
          <h2 class="title-font title-alta sm:text-4xl text-3xl mb-4 font-medium text-gray-900 uppercase">
            PROYECTOS INMOBILIARIOS EN ZONA 10, 14, 15 Y 16
          </h2>
          <p class="mb-6 leading-relaxed long-text-roboto text-lg">
            Descubre nuestros <strong>proyectos inmobiliarios más exclusivos</strong> en las zonas 10, 14, 15 y 16. Desarrollos diseñados para un estilo de vida sofisticado y con la mejor plusvalía del mercado guatemalteco.
          </p>
          <NuxtLink to="/proyectos-inmobiliarios" class="inline-flex boton-optima text-white bg-black py-4 px-8 hover:bg-gray-700 rounded-lg text-lg transition-all duration-300">
            Ver Proyectos
          </NuxtLink>
        </div>
      </div>
    </section>

    <!-- 3. SECCIÓN PROPIEDADES DESTACADAS (NUEVO/RESTAURADO) -->
    <section class="py-16 bg-white overflow-hidden">
      <div class="container mx-auto px-5">
        <h2 class="title-alta-2 text-3xl md:text-4xl font-bold text-gray-900 mb-10 text-center uppercase">
          Casas y Apartamentos Destacados
        </h2>
        <div v-if="destacadasProperties.length > 0">
          <Swiper
            :modules="[Pagination, Autoplay]"
            :slides-per-view="1"
            :space-between="20"
            :pagination="{ clickable: true }"
            :autoplay="{ delay: 5000 }"
            :breakpoints="{
              '768': { slidesPerView: 2 },
              '1024': { slidesPerView: 3 }
            }"
            class="pb-12"
          >
            <SwiperSlide v-for="p in destacadasProperties" :key="p.id">
              <div class="bg-gray-50 rounded-xl shadow-md overflow-hidden h-full group border border-gray-100">
                <NuxtLink :to="getPropertyUrl(p)">
                   <div class="relative h-64 overflow-hidden">
                     <img :src="p.imagenPrincipal" :alt="p.titulo" class="w-full h-full object-cover transform transition-transform duration-500 group-hover:scale-110" />
                     <div v-if="p.operacion" class="absolute top-4 left-4 bg-gray-900 text-white px-3 py-1 text-xs font-bold uppercase rounded">
                        {{ p.operacion }}
                     </div>
                   </div>
                   <div class="p-6">
                     <p class="text-xl font-bold mb-2 text-gray-900">{{ p.titulo }}</p>
                     <p class="text-gray-500 text-sm mb-4">{{ p.ubicaciones }}</p>
                     <p v-if="p.precio" class="text-2xl font-bold text-gray-900">${{ p.precio.toLocaleString() }}</p>
                   </div>
                </NuxtLink>
              </div>
            </SwiperSlide>
          </Swiper>
        </div>
      </div>
    </section>

    <!-- 4. SECCIÓN VENTA -->
    <section class="py-16 bg-gray-50 overflow-hidden">
      <div class="container mx-auto px-5">
        <h2 class="title-alta-2 text-3xl md:text-4xl font-bold text-gray-900 mb-10 text-center uppercase">
          Casas y Apartamentos en Venta
        </h2>
        <div v-if="ventaProperties.length > 0">
          <Swiper
            :modules="[Pagination, Autoplay]"
            :slides-per-view="1"
            :space-between="20"
            :pagination="{ clickable: true }"
            :breakpoints="{
              '768': { slidesPerView: 2 },
              '1024': { slidesPerView: 3 }
            }"
            class="pb-12"
          >
            <SwiperSlide v-for="p in ventaProperties" :key="p.id">
              <div class="bg-white rounded-xl shadow-md overflow-hidden h-full group">
                <NuxtLink :to="getPropertyUrl(p)">
                   <div class="relative h-64 overflow-hidden">
                     <img :src="p.imagenPrincipal" :alt="p.titulo" class="w-full h-full object-cover transform transition-transform duration-500 group-hover:scale-110" />
                     <div class="absolute top-4 left-4 bg-black text-white px-3 py-1 text-xs font-bold uppercase rounded">Venta</div>
                   </div>
                   <div class="p-6">
                     <p class="text-xl font-bold mb-2 text-gray-900">{{ p.titulo }}</p>
                     <p class="text-gray-500 text-sm mb-4">{{ p.ubicaciones }}</p>
                     <p v-if="p.precio" class="text-2xl font-bold text-gray-900">${{ p.precio.toLocaleString() }}</p>
                   </div>
                </NuxtLink>
              </div>
            </SwiperSlide>
          </Swiper>
        </div>
        <div class="text-center mt-8">
          <NuxtLink to="/propiedades/venta" class="text-gray-900 font-bold border-b-2 border-black pb-1 hover:text-gray-600 hover:border-gray-600 transition-all">Explorar todas las propiedades en venta</NuxtLink>
        </div>
      </div>
    </section>

    <!-- 5. SECCIÓN RENTA -->
    <section class="py-16 bg-white overflow-hidden">
      <div class="container mx-auto px-5">
        <h2 class="title-alta-2 text-3xl md:text-4xl font-bold text-gray-900 mb-10 text-center uppercase">
          Casas y Apartamentos en Renta
        </h2>
        <div v-if="rentaProperties.length > 0">
          <Swiper
            :modules="[Pagination, Autoplay]"
            :slides-per-view="1"
            :space-between="20"
            :pagination="{ clickable: true }"
            :breakpoints="{
              '768': { slidesPerView: 2 },
              '1024': { slidesPerView: 3 }
            }"
            class="pb-12"
          >
            <SwiperSlide v-for="p in rentaProperties" :key="p.id">
              <div class="bg-gray-50 rounded-xl shadow-md overflow-hidden h-full group border border-gray-100">
                <NuxtLink :to="getPropertyUrl(p)">
                   <div class="relative h-64 overflow-hidden">
                     <img :src="p.imagenPrincipal" :alt="p.titulo" class="w-full h-full object-cover transform transition-transform duration-500 group-hover:scale-110" />
                     <div class="absolute top-4 left-4 bg-gray-600 text-white px-3 py-1 text-xs font-bold uppercase rounded">Renta</div>
                   </div>
                   <div class="p-6">
                     <p class="text-xl font-bold mb-2 text-gray-900">{{ p.titulo }}</p>
                     <p class="text-gray-500 text-sm mb-4">{{ p.ubicaciones }}</p>
                     <p v-if="p.precio" class="text-2xl font-bold text-gray-900">${{ p.precio.toLocaleString() }}</p>
                   </div>
                </NuxtLink>
              </div>
            </SwiperSlide>
          </Swiper>
        </div>
        <div class="text-center mt-8">
          <NuxtLink to="/propiedades/renta" class="text-gray-900 font-bold border-b-2 border-black pb-1 hover:text-gray-600 hover:border-gray-600 transition-all">Explorar todas las propiedades en renta</NuxtLink>
        </div>
      </div>
    </section>

    <!-- 6. SECCIÓN BLOGS DESTACADOS (NUEVO) -->
    <section class="py-16 bg-white overflow-hidden">
      <div class="container mx-auto px-5">
        <h2 class="title-alta-2 text-3xl md:text-4xl font-bold text-gray-900 mb-4 text-center uppercase">
          Blog Inmobiliario
        </h2>
        <p class="text-center text-gray-600 mb-10 max-w-2xl mx-auto">
          Consejos, tendencias y guías para comprar, vender o invertir en bienes raíces en Guatemala
        </p>
        
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 mb-8">
          <NuxtLink 
            v-for="blog in featuredBlogs" 
            :key="blog.id" 
            :to="`/${blog.category.toLowerCase()}/${blog.slug}`"
            class="group bg-gray-50 rounded-xl overflow-hidden shadow-md hover:shadow-xl transition-all duration-300 border border-gray-100"
          >
            <div class="relative h-48 overflow-hidden">
              <img 
                :src="blog.previewImage" 
                :alt="blog.title"
                class="w-full h-full object-cover transform transition-transform duration-500 group-hover:scale-110"
                loading="lazy"
              />
              <div class="absolute top-4 right-4 bg-black text-white px-3 py-1 text-xs font-bold uppercase rounded">
                {{ blog.category }}
              </div>
            </div>
            <div class="p-6">
              <h3 class="text-xl font-bold mb-3 text-gray-900 group-hover:text-gray-600 transition-colors line-clamp-2">
                {{ blog.title }}
              </h3>
              <div class="text-sm text-gray-600 line-clamp-3 mb-4" v-html="blog.intro"></div>
              <span class="text-black font-semibold text-sm border-b-2 border-black group-hover:border-gray-600 transition-colors">
                Leer más →
              </span>
            </div>
          </NuxtLink>
        </div>

        <div class="text-center">
          <NuxtLink 
            to="/blog" 
            class="inline-flex boton-optima text-white bg-black py-4 px-8 hover:bg-gray-700 rounded-lg text-lg transition-all duration-300"
          >
            Ver Todos los Artículos
          </NuxtLink>
        </div>
      </div>
    </section>

    <!-- 7. FAQ SECTION -->
    <section class="bg-gray-50 py-16 px-5" itemscope itemtype="https://schema.org/FAQPage">
      <div class="container mx-auto max-w-4xl">
        <h2 class="title-alta-2 text-3xl md:text-4xl text-center font-bold text-gray-900 mb-10">
          Preguntas Frecuentes sobre Bienes Raíces en Guatemala
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div v-for="(faq, i) in faqs" :key="i" itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
               class="bg-white rounded-lg p-6 shadow-sm">
            <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">{{ faq.q }}</h3>
            <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
              <p itemprop="text" class="text-gray-700 leading-relaxed text-sm" v-html="faq.a"></p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { computed } from "vue";
import inmuebleService from "~/services/inmuebleService";
import { blogs as blogsData } from "~/data/blogs.js";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Pagination, Autoplay } from "swiper/modules";
import "swiper/css";
import "swiper/css/pagination";

// --- ⚡️ SSR DATA FETCHING ---
const [{ data: destacadasData }, { data: ventaData }, { data: rentaData }] = await Promise.all([
  useAsyncData('home-destacadas', () => inmuebleService.getInmueblesPaginados(1, 10, { Destacado: true })),
  useAsyncData('home-venta', () => inmuebleService.getInmueblesPaginados(1, 6, { Operaciones: 'Venta', Destacado: true })),
  useAsyncData('home-renta', () => inmuebleService.getInmueblesPaginados(1, 6, { Operaciones: 'Renta', Destacado: true }))
]);

const destacadasProperties = computed(() => destacadasData.value?.items || []);
const ventaProperties = computed(() => ventaData.value?.items || []);
const rentaProperties = computed(() => rentaData.value?.items || []);

// Blogs destacados - Mostrar los 3 más recientes
const featuredBlogs = computed(() => {
  // IDs de blogs destacados: 1 (Mejores Clubs), 3 (Mejores Proyectos), 4 (Mejores Zonas)
  const featuredIds = [3, 4, 1]; // Orden: Proyectos, Zonas, Clubs
  return blogsData.filter(blog => featuredIds.includes(blog.id));
});

const getPropertyUrl = (p) => {
  const slug = encodeURIComponent(String(p?.slugInmueble || p?.slug || '').trim());
  return slug ? `/inmueble/${slug}` : '#';
};

const faqs = [
  { q: '¿Cuáles son las mejores zonas en Guatemala?', a: 'Las zonas exclusivas incluyen <strong>Zona 10, 14, 15, 16 y Carretera a El Salvador</strong>.' },
  { q: '¿Cuánto cuesta una casa de lujo?', a: 'Los precios varían entre <strong>$350,000 USD y más de $2,000,000 USD</strong>.' },
  { q: '¿Servicios de Homes Guatemala?', a: 'Ofrecemos <strong>asesoría personalizada</strong>, marketing premium y gestión integral.' },
  { q: '¿Es buena inversión?', a: 'Sí, la <strong>plusvalía promedio ha sido del 50%</strong> en la última década en zonas premium.' }
];

useSeoMeta({
  title: 'Venta y Renta de Propiedades | Homes Guatemala',
  description: 'Casas y apartamentos de lujo en venta y renta en Guatemala. Propiedades exclusivas en Zona 10, 14, 15, 16. Asesoría personalizada premium.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp'
});
</script>

<style scoped>
.title-alta { font-family: "Raleway", sans-serif; font-weight: 300; }
.title-alta-2 { font-family: "Raleway", sans-serif; font-weight: 300; }
.boton-optima { font-family: "Optima", serif; font-weight: 400; }
.long-text-roboto { font-family: "Raleway", sans-serif; font-weight: 300; }

/* Utilidades para truncar texto */
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.line-clamp-3 {
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
