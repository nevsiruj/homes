<template>
    <div class="w-full p-4 flex justify-center card-hover h-full">
        <NuxtLink :to="propertyPath"
            class="fuente-raleway property-card w-full max-w-sm bg-white border border-gray-200 rounded-lg shadow-sm overflow-hidden relative"
            @mouseenter="isHovering = true" @mouseleave="isHovering = false">

            <swiper :observer="true"
                :observeParents="true" :spaceBetween="30" :centeredSlides="true" :autoplay="{
                delay: 2500,
            }" :loop="true" :pagination="{ clickable: true }" :modules="modules" :speed="800" :grabCursor="true"
                :lazy="true" @swiper="onSwiperInit" class="mySwiper">

                <swiper-slide v-if="inmueble.imagenPrincipal">
                    <img :data-src="getOptimizedImageUrl(inmueble.imagenPrincipal, 400)"
                        :srcset="getSrcset(inmueble.imagenPrincipal)"
                        sizes="(max-width: 640px) 100vw, (max-width: 768px) 50vw, (max-width: 1024px) 33vw, 300px"
                        class="swiper-lazy w-sm h-64 object-cover" alt="Imagen principal del inmueble" />
                    <div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div>
                </swiper-slide>

                <swiper-slide
                    v-for="(imagen, index) in inmueble.imagenesReferencia.slice(0, inmueble.imagenPrincipal ? 3 : 4)"
                    :key="index">
                    <img :data-src="getOptimizedImageUrl(imagen.url, 400)" :srcset="getSrcset(imagen.url)"
                        sizes="(max-width: 640px) 100vw, (max-width: 768px) 50vw, (max-width: 1024px) 33vw, 300px"
                        class="swiper-lazy w-sm h-64 object-cover" :alt="`Imagen de referencia ${index + 1}`" />
                    <div class="swiper-lazy-preloader swiper-lazy-preloader-white"></div>
                </swiper-slide>

                <swiper-slide
                    v-if="!inmueble.imagenPrincipal && (!inmueble.imagenesReferencia || inmueble.imagenesReferencia.length === 0)">
                    <img src="https://via.placeholder.com/400x300?text=Sin+imagen" class="w-full h-64 object-cover"
                        alt="Sin imagen disponible" />
                </swiper-slide>
            </swiper>

            <div class="flex flex-col flex-grow p-5 pb-20">

                <div class="flex-grow">
                    <ul v-once class="flex flex-wrap items-center justify-center text-gray-900 mt-2 mb-4">
                    </ul>

                    <h5 class="text-xl max-w-xl subtitle-optima font-bold tracking-tight text-gray-900">
                        {{ inmueble.titulo || "Propiedad sin título" }}
                    </h5>
                    <p class="text-gray-600">
                        {{ inmueble.tipos }} en {{ inmueble.operaciones }}
                    </p>
                    <p class="text-gray-600 mt-1">
                        {{ inmueble.ubicaciones }}
                    </p>
                </div>

                <div class="flex items-center justify-start space-x-6 mt-2">
                    <div class="flex items-center text-sm">
                        <svg class="w-5 h-5 me-1.5" fill="#000000" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
                            <g>
                                <path d="M501.333,256h-344.32c2.027-6.933,3.093-14.08,2.987-21.333c0-25.6-32.107-42.667-53.333-42.667h-64 c-7.467,0-14.827,2.133-21.333,5.973v-80.64c0-5.867-4.8-10.667-10.667-10.667C4.8,106.667,0,111.467,0,117.333v277.333 c0,5.867,4.8,10.667,10.667,10.667c5.867,0,10.667-4.8,10.667-10.667v-53.333h469.333V384c0,5.867,4.8,10.667,10.667,10.667 c5.867,0,10.667-4.8,10.667-10.667V266.667C512,260.8,507.2,256,501.333,256z M42.667,213.333h64c12.907,0,32,11.2,32,21.333 C138.667,256,132.693,256,128,256H42.667c-11.733,0-21.333-9.6-21.333-21.333S30.933,213.333,42.667,213.333z M490.667,320H21.333 v-42.667h469.333V320z"></path>
                            </g>
                        </svg>
                        <span>{{ inmueble.habitaciones || 0 }}</span>
                    </div>
                    <div class="flex items-center text-sm">
                        <svg class="w-5 h-5 me-1.5" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path fill-rule="evenodd" clip-rule="evenodd" d="M5.38517 2.75C4.48209 2.75 3.75 3.48209 3.75 4.38516V11.25H4.03429C4.04516 11.25 4.05599 11.25 4.06675 11.25C4.07208 11.25 4.07739 11.25 4.08268 11.25L19.9332 11.25C19.944 11.25 19.9548 11.25 19.9657 11.25H22C22.4142 11.25 22.75 11.5858 22.75 12C22.75 12.4142 22.4142 12.75 22 12.75H21.7321C21.7386 12.7949 21.7433 12.8405 21.7463 12.8864C21.7501 12.9442 21.75 13.0066 21.75 13.0668L21.75 13.1047C21.75 13.4799 21.75 13.6998 21.7344 13.9452C21.5925 16.1815 20.384 18.2467 18.6326 19.597C18.6463 19.6186 18.6591 19.6412 18.6708 19.6646L19.6708 21.6646C19.8561 22.0351 19.7059 22.4856 19.3354 22.6708C18.9649 22.8561 18.5144 22.7059 18.3292 22.3354L17.3615 20.4C16.5597 20.8059 15.6878 21.073 14.7809 21.1648C14.5364 21.1896 14.3872 21.1952 14.133 21.2047L14.1263 21.205C13.3861 21.2328 12.6615 21.25 12 21.25C11.3385 21.25 10.6139 21.2328 9.87368 21.205L9.86699 21.2047C9.61278 21.1952 9.46358 21.1896 9.2191 21.1648C8.31222 21.073 7.44028 20.8059 6.63851 20.4L5.67082 22.3354C5.48558 22.7059 5.03507 22.8561 4.66459 22.6708C4.29411 22.4856 4.14394 22.0351 4.32918 21.6646L5.32918 19.6646C5.34089 19.6412 5.35366 19.6186 5.3674 19.597C3.61596 18.2467 2.4075 16.1815 2.26556 13.9452C2.24999 13.6998 2.24999 13.4798 2.25 13.1046L2.25 13.0827C2.25 13.0774 2.25 13.0721 2.24999 13.0668C2.24999 13.0483 2.24998 13.0296 2.25008 13.0108C2.25003 13.0072 2.25 13.0036 2.25 13V12.75H2C1.58579 12.75 1.25 12.4142 1.25 12C1.25 11.5858 1.58579 11.25 2 11.25H2.25V4.38516C2.25 2.65366 3.65366 1.25 5.38517 1.25C6.66715 1.25 7.81998 2.0305 8.29609 3.22079L8.40623 3.49613C9.19952 3.29489 10.0603 3.34152 10.8717 3.68813C11.887 4.12189 12.6258 4.94029 13.0041 5.90053C13.1526 6.27744 12.975 6.70417 12.6029 6.86436L6.64215 9.43044C6.45572 9.51069 6.24473 9.51197 6.05735 9.43396C5.86997 9.35596 5.72221 9.20535 5.6478 9.01651C5.26959 8.05665 5.24692 6.94515 5.66723 5.91014C5.96643 5.17335 6.45214 4.56929 7.04665 4.13607L6.90338 3.77788C6.65506 3.15708 6.05379 2.75 5.38517 2.75ZM4.08268 12.75C4.04261 12.75 4.01877 12.75 4.00076 12.7502C3.98765 12.7504 3.98298 12.7506 3.98281 12.7506C3.98215 12.7506 3.98276 12.7506 3.98281 12.7506C3.85775 12.7587 3.75904 12.8581 3.75057 12.9831C3.75052 12.9843 3.75035 12.9893 3.75022 13.0008C3.75001 13.0188 3.75 13.0426 3.75 13.0827C3.75 13.4853 3.75031 13.6573 3.76255 13.8501C3.94798 16.7718 6.45762 19.3775 9.37024 19.6725C9.5652 19.6922 9.67311 19.6964 9.92999 19.7061C10.658 19.7334 11.3629 19.75 12 19.75C12.6371 19.75 13.342 19.7334 14.07 19.7061C14.3269 19.6964 14.4348 19.6922 14.6298 19.6725C17.5424 19.3775 20.052 16.7718 20.2375 13.8501C20.2497 13.6573 20.25 13.4853 20.25 13.0827C20.25 13.0426 20.25 13.0188 20.2498 13.0008C20.2497 12.9906 20.2495 12.9855 20.2495 12.9837C20.2494 12.9825 20.2494 12.9824 20.2495 12.9837C20.2413 12.8584 20.1415 12.7587 20.0162 12.7505C20.0174 12.7506 20.0177 12.7506 20.0162 12.7505C20.0142 12.7505 20.009 12.7503 19.9992 12.7502C19.9812 12.75 19.9574 12.75 19.9173 12.75H4.08268ZM10.2824 5.06753C9.62506 4.78672 8.91452 4.82579 8.30713 5.12147C7.76827 5.3838 7.31118 5.8486 7.05701 6.47451C6.89349 6.87716 6.83436 7.29656 6.86648 7.70078L11.2476 5.81471C10.9982 5.49339 10.6713 5.2337 10.2824 5.06753Z" fill="#000000"></path>
                        </svg>
                        <span>{{ inmueble.banos || 0 }}</span>
                    </div>
                    <div class="flex items-center text-sm">
                        <svg class="w-5 h-5 me-1.5" fill="#000000" viewBox="0 0 50 50" xmlns="http://www.w3.org/2000/svg">
                            <path d="M7 6 A 1.0001 1.0001 0 0 0 6.2929688 6.2929688L3.2929688 9.2929688 A 1.0001 1.0001 0 0 0 3.0292969 10.242188L4.0292969 14.242188 A 1.0001 1.0001 0 0 0 5 15L10 15L10 17L8.5976562 17C6.7095541 17 4.9786821 18.068954 4.1308594 19.755859L4.1621094 19.697266L0.73828125 25.515625C0.25551192 26.302311 0 27.206624 0 28.128906L0 37.308594C0 38.783636 1.2163641 40 2.6914062 40L4.0917969 40C4.5721157 42.828207 7.039829 45 10 45C12.960171 45 15.427884 42.828207 15.908203 40L34.091797 40C34.572116 42.828207 37.039829 45 40 45C42.960171 45 45.427884 42.828207 45.908203 40L47 40C48.645455 40 50 38.645455 50 37L50 32.388672C50 29.951489 48.225687 27.858433 45.822266 27.457031L37.494141 26.068359L30.533203 18.777344C29.583575 17.650285 28.183972 17 26.710938 17L26 17L26 15L31 15 A 1.0001 1.0001 0 0 0 31.894531 14.447266L33.894531 10.447266 A 1.0001 1.0001 0 0 0 33.263672 9.0351562L22.263672 6.0351562 A 1.0001 1.0001 0 0 0 22 6L7 6 z M 7.4140625 8L21.865234 8L31.560547 10.644531L30.382812 13L5.78125 13L5.1074219 10.306641L7.4140625 8 z M 12 15L24 15L24 17L12 17L12 15 z M 8.5976562 19L17 19L17 26L2.7734375 26L5.9023438 20.683594L5.9179688 20.652344C6.4281453 19.637236 7.4617586 19 8.5976562 19 z M 19 19L26.710938 19C27.595903 19 28.433534 19.389465 29.003906 20.066406L29.023438 20.089844L34.662109 26L19 26L19 19 z M 2.0078125 28L36.916016 28L45.494141 29.429688C46.946721 29.672283 48 30.915854 48 32.388672L48 37C48 37.554545 47.554545 38 47 38L45.908203 38C45.427884 35.171793 42.960171 33 40 33C37.039829 33 34.572116 35.171793 34.091797 38L15.908203 38C15.427884 35.171793 12.960171 33 10 33C7.039829 33 4.5721157 35.171793 4.0917969 38L2.6914062 38C2.2984484 38 2 37.701552 2 37.308594L2 28.128906C2 28.085622 2.00596 28.043127 2.0078125 28 z M 10 35C12.220375 35 14 36.779625 14 39C14 41.220375 12.220375 43 10 43C7.7796254 43 6 41.220375 6 39C6 36.779625 7.7796254 35 10 35 z M 40 35C42.220375 35 44 36.779625 44 39C44 41.220375 42.220375 43 40 43C37.779625 43 36 41.220375 36 39C36 36.779625 37.779625 35 40 35 z"></path>
                        </svg>
                        <span>{{ inmueble.parqueos || 0 }}</span>
                    </div>
                </div>
            </div>


            <div class="absolute bottom-0 left-0 w-full bg-white p-4 flex items-center justify-between">
    <span
        v-if="inmueble.precioActivo"
        class="precio-optima text-gray-900"
    >
        ${{ inmueble.precio?.toLocaleString() || "0" }}
    </span>
    <span v-else class="subtitle-optima text-gray-900">
        
    </span>

    <div
        class="text-white bg-black hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5"
    >
        Ver detalles
    </div>
</div>

            
        </NuxtLink>
    </div>
</template>

<script setup>
import { ref, watch, computed } from 'vue';
import { Swiper, SwiperSlide } from "swiper/vue";
import { Autoplay, Pagination, Navigation } from "swiper/modules";
import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/navigation";
import { getOptimizedImageUrl, getSrcset } from "../helpers/useImageOptimization.js";
import { BedIcon, BathIcon, CarIcon } from "../components/icons";

const props = defineProps({
    inmueble: { type: Object, required: true },
});

const modules = [Autoplay, Pagination, Navigation];
const propertyPath = computed(() => `/inmueble/${props.inmueble?.slugInmueble || ''}`);


const isHovering = ref(false);
let swiperInstance = null;

const onSwiperInit = (swiper) => {
    swiperInstance = swiper;
    swiperInstance.autoplay.stop();
};

watch(isHovering, (newValue) => {

    if (!swiperInstance) {
        // console.warn('⚠️ Se intentó cambiar el autoplay, pero la instancia de Swiper no está lista.');
        return;
    }

    if (newValue) {
        swiperInstance.autoplay.start();
    } else {
        swiperInstance.autoplay.stop();
    }
});
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Raleway:wght@400;500;700&display=swap');

.fuente-raleway {
  font-family: 'Raleway', sans-serif;
}

.precio-optima {
    font-family: "Optima", serif;
    font-weight: normal;
    font-size: 24px;
}

.property-card {
    position: relative;
    will-change: transform;
    transition: transform 0.3s ease-in-out;
}

.property-card::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.12), 0 4px 8px rgba(0, 0, 0, 0.06);
    opacity: 0;

    transition: opacity 0.3s ease-in-out;

    z-index: -1;
    border-radius: inherit;
}


.property-card:hover {
    transform: scale(1.05);
    z-index: 10;
}

.property-card:hover::after {
    opacity: 1;
}
</style>