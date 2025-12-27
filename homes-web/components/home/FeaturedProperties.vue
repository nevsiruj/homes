<template>
  <section class="py-16 bg-white overflow-hidden">
    <div class="container mx-auto px-5">
      <h2 class="title-alta-2 text-3xl md:text-4xl font-bold text-gray-900 mb-10 text-center">
        PROPIEDADES DESTACADAS
      </h2>
      
      <div v-if="loading" class="flex justify-center py-10">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
      </div>
      
      <div v-else-if="properties.length > 0" class="featured-properties-slider">
        <Swiper
          :modules="modules"
          :slides-per-view="1"
          :space-between="20"
          :pagination="{ clickable: true }"
          :autoplay="{ delay: 5000 }"
          :breakpoints="{
            '640': { slidesPerView: 2 },
            '1024': { slidesPerView: 3 }
          }"
        >
          <SwiperSlide v-for="p in properties" :key="p.id">
            <InmuebleCard :inmueble="p" />
          </SwiperSlide>
        </Swiper>
      </div>
    </div>
  </section>
</template>

<script setup>
import { Swiper, SwiperSlide } from "swiper/vue";
import { Pagination, Autoplay } from "swiper/modules";
import "swiper/css";
import "swiper/css/pagination";
import InmuebleCard from "~/components/InmuebleCard.vue";

defineProps({
  properties: Array,
  loading: Boolean
});

const modules = [Pagination, Autoplay];
</script>
