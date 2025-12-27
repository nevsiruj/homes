<template>
  <!-- Hero Section con H1 visible y contenido SEO optimizado -->
  <section class="bg-gradient-to-b from-gray-50 to-white py-12 md:py-16">
    <div class="container mx-auto px-5">
      <h1 class="title-alta text-center text-4xl md:text-5xl lg:text-6xl font-bold text-gray-900 mb-6">
        Homes Guatemala - Bienes Raíces de Lujo
      </h1>
      <div class="max-w-4xl mx-auto text-center">
        <p class="long-text-roboto text-lg md:text-xl text-gray-700 mb-4 leading-relaxed">
          Somos líderes en <strong>bienes raíces de lujo en Guatemala (GT)</strong>, especializados en la 
          <strong>venta y renta de propiedades exclusivas</strong> en las zonas más prestigiosas de Ciudad de Guatemala. 
          Como la principal agencia de <strong>bienes raíces de lujo GT</strong>, nuestro portafolio incluye 
          <strong>casas de lujo, apartamentos premium y proyectos inmobiliarios</strong> en Zona 10, Zona 14, 
          Zona 15, Zona 16 y Carretera a El Salvador.
        </p>
        <p class="long-text-roboto text-lg text-gray-600 mb-6 leading-relaxed">
          Con más de una década de experiencia en el mercado inmobiliario guatemalteco, en <strong>Homes Guatemala</strong> 
          ofrecemos un servicio personalizado para ayudarte a encontrar la propiedad perfecta que se adapte a tu estilo 
          de vida y necesidades de inversión. Descubre propiedades exclusivas con las mejores amenidades, ubicaciones 
          estratégicas y oportunidades de inversión inmobiliaria en Guatemala.
        </p>
        <div class="flex flex-wrap justify-center gap-4 mt-8">
          <NuxtLink
            to="/propiedades/venta"
            class="inline-flex boton-optima text-white bg-black border-0 py-4 px-8 focus:outline-none hover:bg-gray-700 rounded-lg text-lg transition-colors duration-300"
          >
            Ver Propiedades en Venta
          </NuxtLink>
          <NuxtLink
            to="/propiedades/renta"
            class="inline-flex boton-optima text-gray-900 bg-white border-2 border-gray-900 py-4 px-8 focus:outline-none hover:bg-gray-100 rounded-lg text-lg transition-colors duration-300"
          >
            Ver Propiedades en Renta
          </NuxtLink>
        </div>
      </div>
    </div>
  </section>

  <section class="text-gray-600 body-font mt-12">
    <div
      class="container mx-auto flex px-5 lg:py-12 md:flex-row flex-col items-center py-6"
    >
      <div class="lg:max-w-lg lg:w-full md:w-1/2 w-5/6 mb-10 md:mb-0">
        <nuxt-img 
          preload
          format="webp"
          quality="80"
          width="512"
          height="300"
          class="object-cover object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
          alt="Proyecto inmobiliario destacado en Guatemala"
          src="https://app-pool.vylaris.online/dcmigserver/homes/f64ca937-cba0-4276-b350-9aca8a1b51bc.webp"
        />
      </div>
      <div
        class="lg:flex-grow md:w-1/2 lg:pl-24 md:pl-16 flex flex-col md:items-start md:text-left items-center text-center"
      >
        <h2
          class="title-font title-alta sm:text-4xl text-3xl mb-4 font-medium text-gray-900"
        >
          PROYECTOS INMOBILIARIOS DE LUJO
        </h2>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Descubre nuestros
          <strong>proyectos inmobiliarios más exclusivos</strong> en Ciudad de
          Guatemala, diseñados para quienes buscan un estilo de vida moderno y
          una excelente oportunidad de inversión. En Homes Guatemala
          desarrollamos y comercializamos <strong>casas y apartamentos nuevos</strong> o en
          construcción, estratégicamente ubicados en zonas con alta demanda y
          crecimiento como <strong>Zona 10, Zona 13, Zona 14, Zona 15, Zona 16</strong> y
          <strong>Carretera a El Salvador</strong>.
        </p>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Nuestros <strong>proyectos de bienes raíces</strong> destacan por su arquitectura contemporánea, 
          acabados de primera calidad y amenidades exclusivas que incluyen áreas verdes, seguridad 24/7, 
          gimnasios, salones de eventos y espacios de coworking. Cada desarrollo inmobiliario está pensado 
          para ofrecer el máximo confort, privacidad y plusvalía a largo plazo.
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
          class="object-cover object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
          alt="Propiedad de lujo en venta en Guatemala"
          src="https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp"
        />
      </div>
      <div
        class="lg:flex-grow md:w-1/2 lg:pr-24 md:pr-16 flex flex-col md:items-start md:text-left mb-16 md:mb-0 items-center text-center order-2 md:order-1"
      >
        <h2
          class="title-font title-alta sm:text-4xl text-3xl mb-4 mt-4 font-medium text-gray-900"
        >
          PROPIEDADES EN VENTA
        </h2>
        <p class="mb-4 leading-relaxed long-text-roboto">
          En esta sección encontrarás una cuidadosa selección de <strong>propiedades en
          venta en Guatemala</strong>, pensadas para quienes están listos para dar el
          siguiente paso: invertir en su patrimonio, iniciar una nueva etapa de
          vida o diversificar su portafolio inmobiliario. Ofrecemos <strong>casas de lujo, 
          apartamentos exclusivos, condominios premium</strong> y terrenos en las mejores ubicaciones 
          de Ciudad de Guatemala.
        </p>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Cada <strong>propiedad en venta</strong> ha sido cuidadosamente evaluada para garantizar 
          la mejor relación calidad-precio, documentación en regla y potencial de plusvalía. 
          Nuestro equipo de asesores inmobiliarios te acompaña en todo el proceso: desde la 
          búsqueda inicial, visitas guiadas, negociación, hasta el cierre legal de la transacción.
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
          class="object-cover w-140 object-center rounded lg:h-[300px] md:h-[300px] h-[250px]"
          alt="Residencia exclusiva en renta en Guatemala"
          src="https://app-pool.vylaris.online/dcmigserver/homes/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp"
        />
      </div>
      <div
        class="lg:flex-grow md:w-1/2 lg:pl-24 md:pl-16 flex flex-col md:items-start md:text-left items-center text-center"
      >
        <h2
          class="title-font title-alta sm:text-4xl text-3xl mb-4 font-medium text-gray-900"
        >
          PROPIEDADES EN RENTA
        </h2>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Nuestras <strong>propiedades en renta</strong> han sido seleccionadas para ofrecer una
          experiencia de vida sofisticada, funcional y adaptada a distintos
          estilos de vida. Desde <strong>apartamentos modernos amueblados</strong> hasta <strong>casas espaciosas en
          zonas residenciales</strong>, ofrecemos una variedad de opciones para elevar tu
          calidad de vida en las mejores zonas de Guatemala.
        </p>
        <p class="mb-4 leading-relaxed long-text-roboto">
          Todas nuestras <strong>propiedades en alquiler</strong> cuentan con excelente ubicación, 
          seguridad garantizada y fácil acceso a centros comerciales, escuelas internacionales, 
          hospitales y vías principales. Ofrecemos opciones amuebladas y sin amueblar, con contratos 
          flexibles y asesoría legal incluida. Ideal para ejecutivos, familias y profesionales que 
          buscan <strong>rentar una casa o apartamento de lujo en Guatemala</strong>.
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
  </section>

  <!-- Nueva sección: Por qué elegir Homes Guatemala -->
  <section class="bg-white py-16 px-5">
    <div class="container mx-auto max-w-6xl">
      <h2 class="title-alta-2 text-3xl md:text-4xl text-center font-bold text-gray-900 mb-8">
        ¿Por Qué Elegir Homes Guatemala?
      </h2>
      
      <div class="grid md:grid-cols-2 gap-8 mb-8">
        <div>
          <h3 class="title-font text-xl font-semibold text-gray-900 mb-3">
            Experiencia en Bienes Raíces de Lujo
          </h3>
          <p class="long-text-roboto text-gray-700 leading-relaxed">
            Con más de una década en el mercado inmobiliario guatemalteco, <strong>Homes Guatemala</strong> 
            se ha consolidado como líder en <strong>bienes raíces de lujo</strong>. Nuestro equipo de 
            asesores especializados conoce a profundidad cada zona de la ciudad, desde las exclusivas 
            áreas de <strong>Zona 10 y Zona 14</strong>, hasta las emergentes zonas residenciales en 
            <strong>Zona 15, Zona 16 y Carretera a El Salvador</strong>.
          </p>
        </div>
        
        <div>
          <h3 class="title-font text-xl font-semibold text-gray-900 mb-3">
            Servicio Integral y Personalizado
          </h3>
          <p class="long-text-roboto text-gray-700 leading-relaxed">
            Ofrecemos un servicio completo que abarca desde la búsqueda de la propiedad ideal hasta 
            el cierre legal de la transacción. Nuestros clientes reciben asesoría en valoración de 
            propiedades, análisis de inversión, gestión de documentación legal y acompañamiento en 
            todo el proceso de <strong>compra, venta o renta de propiedades de lujo en Guatemala</strong>.
          </p>
        </div>
      </div>

      <div class="bg-gray-50 rounded-lg p-8">
        <h3 class="title-font text-xl font-semibold text-gray-900 mb-4 text-center">
          Zonas Exclusivas que Cubrimos
        </h3>
        <p class="long-text-roboto text-gray-700 leading-relaxed mb-4">
          Especializados en las zonas más prestigiosas de Ciudad de Guatemala, donde se concentran 
          las mejores <strong>casas de lujo, apartamentos premium y proyectos inmobiliarios</strong> 
          del país. Nuestro portafolio incluye propiedades en ubicaciones estratégicas con acceso a 
          centros comerciales de primer nivel, colegios internacionales, hospitales privados, 
          restaurantes gourmet y áreas de entretenimiento.
        </p>
        <div class="grid grid-cols-2 md:grid-cols-3 gap-4 text-center">
          <div class="bg-white rounded p-3">
            <p class="font-semibold text-gray-900">Zona 10</p>
            <p class="text-sm text-gray-600">Zona Viva</p>
          </div>
          <div class="bg-white rounded p-3">
            <p class="font-semibold text-gray-900">Zona 14</p>
            <p class="text-sm text-gray-600">Residencial</p>
          </div>
          <div class="bg-white rounded p-3">
            <p class="font-semibold text-gray-900">Zona 15</p>
            <p class="text-sm text-gray-600">Vista Hermosa</p>
          </div>
          <div class="bg-white rounded p-3">
            <p class="font-semibold text-gray-900">Zona 16</p>
            <p class="text-sm text-gray-600">Cayalá</p>
          </div>
          <div class="bg-white rounded p-3">
            <p class="font-semibold text-gray-900">Carretera a El Salvador</p>
            <p class="text-sm text-gray-600">En crecimiento</p>
          </div>
          <div class="bg-white rounded p-3">
            <p class="font-semibold text-gray-900">Santa Catarina Pinula</p>
            <p class="text-sm text-gray-600">Exclusivo</p>
          </div>
        </div>
      </div>
    </div>
  </section>

  <section class="text-gray-600 body-font">
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

  <!-- Sección FAQ para SEO y Rich Snippets -->
  <section class="bg-gray-50 py-16 px-5" itemscope itemtype="https://schema.org/FAQPage">
    <div class="container mx-auto max-w-4xl">
      <h2 class="title-alta-2 text-3xl md:text-4xl text-center font-bold text-gray-900 mb-10">
        Preguntas Frecuentes sobre Bienes Raíces en Guatemala
      </h2>
      
      <div class="space-y-6">
        <!-- FAQ 1 -->
        <div itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
             class="bg-white rounded-lg shadow-sm p-6 hover:shadow-md transition-shadow">
          <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">
            ¿Cuáles son las mejores zonas para comprar propiedades de lujo en Guatemala?
          </h3>
          <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
            <p itemprop="text" class="text-gray-700 leading-relaxed">
              Las zonas más exclusivas para propiedades de lujo en Guatemala incluyen <strong>Zona 10</strong> 
              (conocida como Zona Viva), <strong>Zona 14</strong> (área residencial premium), 
              <strong>Zona 15 Vista Hermosa</strong>, <strong>Zona 16 Cayalá</strong> (la nueva ciudad moderna) 
              y <strong>Carretera a El Salvador</strong> con desarrollos como Santa Catarina Pinula y Condados 
              de San Cristóbal. Estas áreas ofrecen seguridad 24/7, acceso a servicios premium y alta plusvalía.
            </p>
          </div>
        </div>

        <!-- FAQ 2 -->
        <div itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
             class="bg-white rounded-lg shadow-sm p-6 hover:shadow-md transition-shadow">
          <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">
            ¿Cuánto cuesta comprar una casa de lujo en Guatemala?
          </h3>
          <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
            <p itemprop="text" class="text-gray-700 leading-relaxed">
              Los precios de casas de lujo en Guatemala varían según la ubicación y características. 
              En zonas exclusivas como Zona 14 y Zona 16, las casas de lujo pueden oscilar entre 
              <strong>$350,000 USD hasta más de $2,000,000 USD</strong>. Los apartamentos premium 
              en edificios de alta gama comienzan desde <strong>$180,000 USD</strong>. En Homes Guatemala 
              te ayudamos a encontrar opciones que se ajusten a tu presupuesto y necesidades.
            </p>
          </div>
        </div>

        <!-- FAQ 3 -->
        <div itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
             class="bg-white rounded-lg shadow-sm p-6 hover:shadow-md transition-shadow">
          <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">
            ¿Qué servicios ofrece Homes Guatemala como agencia inmobiliaria?
          </h3>
          <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
            <p itemprop="text" class="text-gray-700 leading-relaxed">
              En Homes Guatemala ofrecemos un servicio integral que incluye: <strong>asesoría personalizada</strong> 
              en la búsqueda de propiedades, <strong>valoración profesional de inmuebles</strong>, 
              <strong>fotografía y video profesional</strong> para vendedores, <strong>gestión legal</strong> 
              de compra-venta y alquileres, <strong>análisis de inversión inmobiliaria</strong> y 
              <strong>acompañamiento completo</strong> desde la primera visita hasta el cierre de la transacción.
            </p>
          </div>
        </div>

        <!-- FAQ 4 -->
        <div itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
             class="bg-white rounded-lg shadow-sm p-6 hover:shadow-md transition-shadow">
          <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">
            ¿Es buena inversión comprar bienes raíces en Guatemala?
          </h3>
          <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
            <p itemprop="text" class="text-gray-700 leading-relaxed">
              Sí, Guatemala ofrece excelentes oportunidades de inversión inmobiliaria. Las propiedades en 
              zonas estratégicas han mostrado una <strong>plusvalía promedio del 50% en los últimos 10 años</strong>. 
              Los apartamentos en renta pueden generar <strong>retornos anuales del 4-6%</strong>, superiores a 
              los certificados bancarios. Además, el crecimiento económico y la demanda de vivienda de calidad 
              continúan impulsando el mercado inmobiliario guatemalteco.
            </p>
          </div>
        </div>
      </div>

      <!-- CTA Final -->
      <div class="text-center mt-10">
        <p class="text-gray-600 mb-4">¿Tienes más preguntas? Estamos aquí para ayudarte.</p>
        <NuxtLink
          to="/nosotros"
          class="inline-flex boton-optima text-white bg-black border-0 py-3 px-8 focus:outline-none hover:bg-gray-700 rounded-lg text-lg transition-colors duration-300"
        >
          Contáctanos
        </NuxtLink>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";
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

// SEO Meta Tags optimizados para la página principal
useSeoMeta({
  title: 'Homes Guatemala | Bienes Raíces de Lujo - Casas y Apartamentos en Venta y Renta',
  description: 'Líder en bienes raíces de lujo en Guatemala. Encuentra casas, apartamentos y propiedades exclusivas en venta y renta en Zona 10, 14, 15, 16 y Carretera a El Salvador. Asesoría inmobiliaria profesional.',
  ogTitle: 'Homes Guatemala | Bienes Raíces de Lujo en Guatemala',
  ogDescription: 'Descubre propiedades exclusivas en las mejores zonas de Guatemala. Casas de lujo, apartamentos premium y proyectos inmobiliarios con asesoría personalizada.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
  twitterCard: 'summary_large_image',
  twitterTitle: 'Homes Guatemala - Bienes Raíces de Lujo',
  twitterDescription: 'Encuentra tu propiedad ideal en Guatemala. Casas, apartamentos y terrenos en las zonas más exclusivas.',
});

useHead({
  link: [
    { rel: 'canonical', href: 'https://homesguatemala.com/' }
  ],
  script: [
    {
      type: 'application/ld+json',
      innerHTML: JSON.stringify({
        "@context": "https://schema.org",
        "@type": "RealEstateAgent",
        "name": "Homes Guatemala",
        "description": "Agencia líder en bienes raíces de lujo en Guatemala, especializada en venta y renta de casas, apartamentos y propiedades exclusivas.",
        "url": "https://homesguatemala.com",
        "logo": "https://app-pool.vylaris.online/dcmigserver/homes/0ecfe259-77d7-450f-afb3-4ec21231dc6f.webp",
        "image": "https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp",
        "telephone": "+502-5633-0961",
        "email": "info@homesguatemala.com",
        "address": {
          "@type": "PostalAddress",
          "streetAddress": "Design Center Local 217, Zona 10",
          "addressLocality": "Ciudad de Guatemala",
          "postalCode": "01010",
          "addressCountry": "GT"
        },
        "geo": {
          "@type": "GeoCoordinates",
          "latitude": "14.6349",
          "longitude": "-90.5069"
        },
        "areaServed": {
          "@type": "City",
          "name": "Ciudad de Guatemala"
        },
        "priceRange": "$$$",
        "openingHoursSpecification": {
          "@type": "OpeningHoursSpecification",
          "dayOfWeek": ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"],
              "closes": "18:00"
        },
        "sameAs": [
          "https://www.facebook.com/homesguatemala",
          "https://www.instagram.com/homesguatemala",
          "https://www.youtube.com/@homesguatemala4975",
          "https://www.linkedin.com/company/homes-guatemala"
        ]
      })
    }
  ]
});

// Precargar fuentes críticas y la imagen LCP (Largest Contentful Paint)
useHead({
  link: [
    // Preload fuentes críticas
    {
      rel: 'preload',
      href: '/assets/font/ralewey/raleway-v36-latin-300.woff2',
      as: 'font',
      type: 'font/woff2',
      crossorigin: 'anonymous'
    },
    // Preload de la imagen principal del proyecto destacado (LCP)
    {
      rel: 'preload',
      href: 'https://app-pool.vylaris.online/dcmigserver/homes/f64ca937-cba0-4276-b350-9aca8a1b51bc.webp',
      as: 'image',
      type: 'image/webp'
    }
  ]
});

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
    // Lista de slugs de propiedades destacadas
    const featuredSlugs = [
      'casa-completamente-remodelada-en-venta-con-vistas-de-la-ciudad-zona-10-csv5506',
      'casa-de-un-nivel-en-venta-santa-catarina-pinula-cs5230',
      'casa-con-amplio-jardin-en-venta-hacienda-nueva-country-club-csv5342',
      'casa-en-venta-de-un-nivel-km-25-5-carretera-a-el-salvador-csv5591',
      'apartamento-en-venta-zona-10-asv5758',
      'apartamento-amueblado-en-venta-zona-10-aav5776'
    ];

    // Cargar propiedades destacadas individualmente con mejor manejo de errores
    const fetchPromises = featuredSlugs.map(async (slug) => {
      try {
        const property = await inmuebleService.getInmuebleBySlug(slug);
        return property;
      } catch (error) {
        console.warn(`No se pudo cargar la propiedad destacada: ${slug}`, error.message);
        return null;
      }
    });
    
    const resolved = await Promise.all(fetchPromises);
    
    const featuredPropertiesData = resolved
      .filter(property => property !== null)
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

    // Si no se cargaron suficientes propiedades destacadas, cargar propiedades aleatorias como fallback
    if (featuredPropertiesData.length < 3) {
      console.log('Cargando propiedades aleatorias como fallback...');
      try {
        const fallbackResponse = await inmuebleService.getInmueblesPaginados(1, 6);
        if (fallbackResponse && Array.isArray(fallbackResponse.items)) {
          const fallbackProperties = fallbackResponse.items
            .filter(prop => prop.imagenPrincipal) // Solo propiedades con imagen
            .slice(0, 6 - featuredPropertiesData.length)
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
          
          featuredPropertiesData.push(...fallbackProperties);
        }
      } catch (fallbackError) {
        console.error('Error al cargar propiedades de fallback:', fallbackError);
      }
    }

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
  // Usar URLs limpias sin query params para mejor SEO
  router.push(`/propiedades/${operacion.toLowerCase()}`);
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
