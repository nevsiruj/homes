<template>
  <section class="bg-gray-50 py-16 px-5" itemscope itemtype="https://schema.org/FAQPage">
    <div class="container mx-auto max-w-4xl">
      <h2 class="title-alta-2 text-3xl md:text-4xl text-center font-bold text-gray-900 mb-10">
        Preguntas Frecuentes sobre Bienes Raíces en Guatemala
      </h2>
      <div class="space-y-6">
        <div v-for="(f, i) in faqs" :key="i" itemscope itemprop="mainEntity" itemtype="https://schema.org/Question" 
             class="bg-white rounded-lg shadow-sm p-6 hover:shadow-md transition-shadow">
          <h3 itemprop="name" class="text-lg font-semibold text-gray-900 mb-3">{{ f.q }}</h3>
          <div itemscope itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
            <p itemprop="text" class="text-gray-700 leading-relaxed" v-html="f.a"></p>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup>
const faqs = [
  { q: '¿Cuáles son las mejores zonas para comprar propiedades de lujo en Guatemala?', a: 'Las zonas más exclusivas incluyen <strong>Zona 10</strong>, <strong>Zona 14</strong>, <strong>Zona 15</strong>, <strong>Zona 16 Cayalá</strong> y <strong>Carretera a El Salvador</strong>.' },
  { q: '¿Cuánto cuesta comprar una casa de lujo en Guatemala?', a: 'Los precios pueden oscilar entre <strong>$350,000 USD hasta más de $2,000,000 USD</strong> dependiendo de la zona.' },
  { q: '¿Qué servicios ofrece Homes Guatemala como agencia inmobiliaria?', a: 'Ofrecemos <strong>asesoría personalizada</strong>, valoración profesional, marketing de lujo y gestión legal completa.' },
  { q: '¿Es buena inversión comprar bienes raíces en Guatemala?', a: 'Sí, las propiedades han mostrado una <strong>plusvalía promedio del 50% en los últimos 10 años</strong>.' }
];
</script>
