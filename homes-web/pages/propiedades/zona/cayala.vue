<template>
  <div>
    <Header />
    <section class="text-gray-600 body-font mt-16 lg:mt-0">
      <div class="container px-5 py-24 mx-auto">
        <div class="flex flex-col text-center w-full mb-6">
          <h1 class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900">
            PROPIEDADES EN CAYALÁ, ZONA 16
          </h1>
          <hr class="w-48 h-1 mx-auto mb-2 bg-gray-100 border-0 rounded-sm" />
        </div>

        <div class="mb-8 max-w-4xl mx-auto">
          <p class="mb-4 leading-relaxed long-text-roboto">
            <strong>Cayalá</strong> es el desarrollo urbano más exclusivo de Guatemala, ubicado en <strong>Zona 16</strong>. 
            Ofrece un estilo de vida único con arquitectura europea, calles peatonales, comercios de lujo, 
            restaurantes gourmet y espacios verdes. Ideal para familias que buscan seguridad, comodidad y 
            una comunidad vibrante.
          </p>

          <p class="mb-4 leading-relaxed long-text-roboto">
            Encuentra <strong>casas y apartamentos en venta y renta en Cayalá</strong> con acabados de lujo, 
            amenidades premium y la mejor ubicación de Guatemala. Propiedades desde $250,000 hasta $2,000,000.
          </p>

          <h2 class="text-2xl font-bold text-gray-900 mb-4 mt-8">
            ¿Por qué invertir en Cayalá?
          </h2>
          <ul class="list-disc list-inside mb-6 long-text-roboto space-y-2">
            <li><strong>Plusvalía garantizada:</strong> Zona con mayor crecimiento en Guatemala</li>
            <li><strong>Seguridad 24/7:</strong> Comunidad cerrada con vigilancia privada</li>
            <li><strong>Amenidades de clase mundial:</strong> Club La Reserva, gimnasios, piscinas</li>
            <li><strong>Educación de prestigio:</strong> Colegios internacionales a 5 minutos</li>
            <li><strong>Comercio y entretenimiento:</strong> Todo a distancia caminable</li>
            <li><strong>Retorno de inversión:</strong> 5-7% anual en rentas</li>
          </ul>

          <h2 class="text-2xl font-bold text-gray-900 mb-4 mt-8">
            Tipos de propiedades en Cayalá
          </h2>
          <div class="grid md:grid-cols-2 gap-6 mb-8">
            <div class="bg-gray-50 p-6 rounded-lg">
              <h3 class="text-xl font-bold mb-2">Apartamentos</h3>
              <p class="text-sm">Desde 2 hasta 4 habitaciones. Áreas de 120m² a 350m². 
              Acabados de lujo, balcones, parqueos techados. Precios desde $250,000.</p>
            </div>
            <div class="bg-gray-50 p-6 rounded-lg">
              <h3 class="text-xl font-bold mb-2">Casas</h3>
              <p class="text-sm">Residencias de 3 a 5 habitaciones. Áreas de 250m² a 600m². 
              Jardines privados, terrazas, garaje. Precios desde $600,000.</p>
            </div>
          </div>

          <div class="bg-blue-50 border-l-4 border-blue-500 p-6 mb-8">
            <p class="font-bold text-lg mb-2">💰 Oportunidad de Inversión</p>
            <p class="text-sm">
              Cayalá ofrece el mejor retorno de inversión en Guatemala. Propiedades en renta 
              generan entre $1,500 y $5,000 mensuales. Plusvalía promedio: 8-12% anual.
            </p>
          </div>
        </div>

        <!-- Filtro de propiedades -->
        <div class="mb-8">
          <NuxtLink 
            to="/propiedades?Ubicaciones=Cayalá"
            class="inline-block bg-black text-white px-8 py-4 rounded-lg hover:bg-gray-800 transition-colors duration-200 text-lg font-semibold"
          >
            Ver Todas las Propiedades en Cayalá →
          </NuxtLink>
        </div>

        <!-- SEO Content -->
        <div class="mt-16 pt-8 border-t border-gray-100 max-w-4xl mx-auto">
          <h2 class="text-2xl font-bold text-gray-900 mb-4">
            Preguntas Frecuentes sobre Propiedades en Cayalá
          </h2>
          
          <div class="space-y-6">
            <div>
              <h3 class="text-lg font-bold mb-2">¿Cuánto cuesta un apartamento en Cayalá?</h3>
              <p class="text-sm long-text-roboto">
                Los apartamentos en Cayalá tienen precios desde $250,000 para unidades de 2 habitaciones, 
                hasta $800,000 para penthouses de lujo. El precio por m² promedio es de $2,000 a $2,500.
              </p>
            </div>

            <div>
              <h3 class="text-lg font-bold mb-2">¿Es buena inversión comprar en Cayalá?</h3>
              <p class="text-sm long-text-roboto">
                Sí, Cayalá es una excelente inversión. La plusvalía anual es de 8-12% y el retorno 
                por renta es de 5-7% anual. Es la zona con mayor demanda de alquiler en Guatemala.
              </p>
            </div>

            <div>
              <h3 class="text-lg font-bold mb-2">¿Qué amenidades tiene Cayalá?</h3>
              <p class="text-sm long-text-roboto">
                Cayalá cuenta con Club La Reserva (gimnasio, piscinas, canchas), restaurantes gourmet, 
                cafeterías, tiendas de lujo, cines, parques, áreas verdes, seguridad 24/7 y calles peatonales.
              </p>
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
import Header from '~/components/header.vue'
import Footer from '~/components/footer.vue'
import RedesFlotantes from '~/components/redesFlotantes.vue'

useSeoMeta({
  title: 'Propiedades en Cayalá Zona 16 - Casas y Apartamentos en Venta y Renta',
  description: 'Casas y apartamentos en venta y renta en Cayalá, Zona 16. Propiedades de lujo desde $250,000. Plusvalía garantizada 8-12% anual. Retorno inversión 5-7%. Ver catálogo.',
  ogTitle: 'Propiedades en Cayalá Zona 16 | Homes Guatemala',
  ogDescription: 'Encuentra tu hogar ideal en Cayalá. Casas y apartamentos de lujo con amenidades premium. Inversión segura.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
  twitterCard: 'summary_large_image',
})

useHead({
  link: [
    { rel: 'canonical', href: 'https://homesguatemala.com/propiedades/zona/cayala' }
  ]
})
</script>
