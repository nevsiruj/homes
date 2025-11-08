<template>
  <section class="bg-white ">
    <div
      class="py-8 px-4 mx-auto max-w-screen-xl text-center lg:py-16 lg:px-12"
    >
      <h4
        class="mb-12 text-3xl text-center font-light uppercase leading-none tracking-tight text-gray-900 md:text-3xl lg:text-3xl "
      >
        Propiedades Favoritas
      </h4>

      <swiper
        :slidesPerView="1"
        :spaceBetween="5"
        :loop="true"
        :autoplay="{
          delay: 2500,
          disableOnInteraction: true,
        }"
        :pagination="{
          clickable: true,
        }"
        :navigation="true"
        :breakpoints="{
          '@0.00': {
            slidesPerView: 1,
            spaceBetween: 10,
          },
          '@0.75': {
            slidesPerView: 2,
            spaceBetween: 20,
          },
          '@1.00': {
            slidesPerView: 3,
            spaceBetween: 40,
          },
          '@1.50': {
            slidesPerView: 3,
            spaceBetween: 50,
          },
        }"
        :modules="modules"
        class="mySwiper"
      >
        <swiper-slide
          ><div class="bg-gray-100 p-3 rounded-lg relative overflow-hidden">
            <div class="relative">
              <div
                class="absolute top-2 left-2 bg-black opacity-75 text-white px-3 py-1 z-10"
              >
                <span class="text-xs font-regular">$1,700,000</span>
              </div>

              <img
                class="h-56 w-full object-cover rounded-lg object-center"
                src="https://homesguatemala.com/wp-content/uploads/2020/09/venta-homes.png"
                alt="content"
              />
              <div
                class="absolute inset-x-0 bottom-2 bg-black opacity-75 hover:opacity-90 py-2 text-center transform transition-transform duration-300 hover:scale-105"
              >
                <p class="font-semibold text-white">Casa | Zona 16</p>
              </div>
            </div>
          </div></swiper-slide
        >
        <swiper-slide
          ><div class="bg-gray-100 p-3 rounded-lg relative overflow-hidden">
            <div class="relative">
              <div
                class="absolute top-2 left-2 bg-black opacity-75 text-white px-3 py-1 z-10"
              >
                <span class="text-xs font-regular">$1,700,000</span>
              </div>

              <img
                class="h-56 w-full object-cover rounded-lg object-center"
                src="https://homesguatemala.com/wp-content/uploads/2020/09/venta-homes.png"
                alt="content"
              />
              <div
                class="absolute inset-x-0 bottom-2 bg-black opacity-75 hover:opacity-90 py-2 text-center transform transition-transform duration-300 hover:scale-105"
              >
                <p class="font-semibold text-white">Casa | Zona 16</p>
              </div>
            </div>
          </div></swiper-slide
        ><swiper-slide
          ><div class="bg-gray-100 p-3 rounded-lg relative overflow-hidden">
            <div class="relative">
              <div
                class="absolute top-2 left-2 bg-black opacity-75 text-white px-3 py-1 z-10"
              >
                <span class="text-xs font-regular">$1,700,000</span>
              </div>

              <img
                class="h-56 w-full object-cover rounded-lg object-center"
                src="https://homesguatemala.com/wp-content/uploads/2020/09/venta-homes.png"
                alt="content"
              />
              <div
                class="absolute inset-x-0 bottom-2 bg-black opacity-75 hover:opacity-90 py-2 text-center transform transition-transform duration-300 hover:scale-105"
              >
                <p class="font-semibold text-white">Casa | Zona 16</p>
              </div>
            </div>
          </div></swiper-slide
        >
        <swiper-slide
          ><div class="bg-gray-100 p-3 rounded-lg relative overflow-hidden">
            <div class="relative">
              <div
                class="absolute top-2 left-2 bg-black opacity-75 text-white px-3 py-1 z-10"
              >
                <span class="text-xs font-regular">$1,700,000</span>
              </div>

              <img
                class="h-56 w-full object-cover rounded-lg object-center"
                src="https://homesguatemala.com/wp-content/uploads/2020/09/venta-homes.png"
                alt="content"
              />
              <div
                class="absolute inset-x-0 bottom-2 bg-black opacity-75 hover:opacity-90 py-2 text-center transform transition-transform duration-300 hover:scale-105"
              >
                <p class="font-semibold text-white">Casa | Zona 16</p>
              </div>
            </div>
          </div></swiper-slide
        ><swiper-slide
          ><div class="bg-gray-100 p-3 rounded-lg relative overflow-hidden">
            <div class="relative">
              <div
                class="absolute top-2 left-2 bg-black opacity-75 text-white px-3 py-1 z-10"
              >
                <span class="text-xs font-regular">$1,700,000</span>
              </div>

              <img
                class="h-56 w-full object-cover rounded-lg object-center"
                src="https://homesguatemala.com/wp-content/uploads/2020/09/venta-homes.png"
                alt="content"
              />
              <div
                class="absolute inset-x-0 bottom-2 bg-black opacity-75 hover:opacity-90 py-2 text-center transform transition-transform duration-300 hover:scale-105"
              >
                <p class="font-semibold text-white">Casa | Zona 16</p>
              </div>
            </div>
          </div></swiper-slide
        >
        <swiper-slide
          ><div class="bg-gray-100 p-3 rounded-lg relative overflow-hidden">
            <div class="relative">
              <div
                class="absolute top-2 left-2 bg-black opacity-75 text-white px-3 py-1 z-10"
              >
                <span class="text-xs font-regular">$1,700,000</span>
              </div>

              <img
                class="h-56 w-full object-cover rounded-lg object-center"
                src="https://homesguatemala.com/wp-content/uploads/2020/09/venta-homes.png"
                alt="content"
              />
              <div
                class="absolute inset-x-0 bottom-2 bg-black opacity-75 hover:opacity-90 py-2 text-center transform transition-transform duration-300 hover:scale-105"
              >
                <p class="font-semibold text-white">Casa | Zona 16</p>
              </div>
            </div>
          </div></swiper-slide
        >
      </swiper>
    </div>
  </section>
</template>

<script>
// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from "swiper/vue";

// Import Swiper styles
import "swiper/css";

import "swiper/css/pagination";
import "swiper/css/navigation";

//import './style.css';

// import required modules
import { Autoplay, Pagination, Navigation } from "swiper/modules";

export default {
  components: {
    Swiper,
    SwiperSlide,
  },
  setup() {
    return {
      modules: [Autoplay, Pagination, Navigation],
    };
  },
};
</script>
