<template>
  <Header />

  <!-- <Loader v-if="isLoading" /> -->

  <SlugSkeleton v-if="isLoading" />

  <!-- Mensaje simple cuando la propiedad no existe -->
  <div
    v-if="!isLoading && notFound"
    class="max-w-[1080px] mx-auto p-6 bg-white mt-24 lg:mt-12 text-center"
  >
    <h2 class="text-2xl font-semibold mb-2">La propiedad no existe</h2>
    <p class="text-gray-600 mb-6">
      Es posible que el enlace esté mal escrito o que la propiedad haya sido
      dada de baja.
    </p>
    <NuxtLink
      to="/propiedades"
      class="inline-flex items-center px-6 py-3 rounded-md bg-black text-white hover:bg-gray-700"
    >
      Ver catálogo completo de propiedades
    </NuxtLink>
  </div>

  <div
    v-if="!isLoading && !notFound"
    class="max-w-[1080px] mx-auto p-4 bg-white mt-24 lg:mt-12"
  >
    <!-- Breadcrumbs -->
    <Breadcrumbs 
      v-if="inmuebleDetalle"
      :breadcrumbs="[
        { name: 'Inicio', url: '/' },
        { name: 'Propiedades', url: '/propiedades' },
        { name: inmuebleDetalle.titulo || 'Propiedad', url: $route.fullPath }
      ]" 
    />

    <div class="mb-5 mt-5">
      <h1
        ref="tituloRef"
        class="title-alta-2 text-xl font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl"
      >
        {{ inmuebleDetalle.titulo || "Cargando..." }}
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

        <div
          v-if="mainMedia.type === 'video'"
          class="w-full h-full md:h-[600px] bg-black flex items-center justify-center"
        >
          <template v-if="isYouTubeVideo(mainMedia.url)">
            <iframe
              :src="getYouTubeEmbedUrl(mainMedia.url)"
              frameborder="0"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowfullscreen
              class="w-full h-full object-contain"
            ></iframe>
          </template>
          <template v-else>
            <video
              :src="mainMedia.url"
              controls
              class="w-full h-full object-contain"
              alt="Video del inmueble"
            ></video>
          </template>
        </div>
        <img
          v-else
          :src="mainMedia.url"
          class="w-full h-auto md:h-[600px] object-cover"
          alt="Imagen principal del inmueble"
        />

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
          :loop="shouldEnableLoop"
          :modules="[FreeMode, Mousewheel, Navigation]"
          class="lg:h-[600px] md:h-[600px] w-full"
        >
          <swiper-slide
            v-for="(media, index) in allMedia"
            :key="index"
            class="!h-24 !w-24 lg:!h-36 lg:!w-36 cursor-pointer rounded-md relative overflow-hidden"
            :class="{ 'border-2 border-gray-500': media.url === mainMedia.url }"
            @click="setMainMedia(media.url, media.type)"
          >
            <img
              v-if="media.type === 'image'"
              :src="media.url"
              alt="Miniatura del inmueble"
              class="w-full h-full object-cover"
            />
            <div
              v-else
              class="w-full h-full bg-black flex items-center justify-center"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-12 w-12 text-white absolute"
                viewBox="0 0 20 20"
                fill="currentColor"
              >
                <path
                  fill-rule="evenodd"
                  d="M10 18a8 8 0 100-16 8 8 0 000 16zM9.555 7.168A1 1 0 008 8v4a1 1 0 001.555.832l3-2a1 1 0 000-1.664l-3-2z"
                  clip-rule="evenodd"
                />
              </svg>
              <img
                v-if="
                  isYouTubeVideo(media.url) && getYouTubeThumbnail(media.url)
                "
                :src="getYouTubeThumbnail(media.url)"
                class="w-full h-full object-cover opacity-60"
                alt="Video thumbnail"
              />
              <img
                v-else-if="inmuebleDetalle.imagenPrincipal"
                :src="inmuebleDetalle.imagenPrincipal"
                class="w-full h-full object-cover opacity-60"
                alt="Video thumbnail"
              />
              <div v-else class="w-full h-full bg-gray-700 opacity-60"></div>
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
            Código: {{ inmuebleDetalle.codigoPropiedad }}
          </h1>
          <hr class="w-48 mx-auto text-gray-300" />

          <div
            class="grid grid-cols-2 gap-2 justify-items-center text-sm md:text-base"
          >
            <div class="flex items-center justify-between">
              <svg
                class="w-5 h-5 me-2"
                viewBox="0 0 192 192"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
              >
                <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                <g
                  id="SVGRepo_tracerCarrier"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                ></g>
                <g id="SVGRepo_iconCarrier">
                  <path
                    stroke="#000000"
                    stroke-width="12"
                    d="M96 22a51.88 51.88 0 0 0-36.77 15.303A52.368 52.368 0 0 0 44 74.246c0 16.596 4.296 28.669 20.811 48.898a163.733 163.733 0 0 1 20.053 28.38C90.852 163.721 90.146 172 96 172c5.854 0 5.148-8.279 11.136-20.476a163.723 163.723 0 0 1 20.053-28.38C143.704 102.915 148 90.841 148 74.246a52.37 52.37 0 0 0-15.23-36.943A51.88 51.88 0 0 0 96 22Z"
                  ></path>
                  <circle
                    cx="96"
                    cy="74"
                    r="20"
                    stroke="#000000"
                    stroke-width="12"
                  ></circle>
                </g>
              </svg>
              <div class="flex flex-col">
                <span>{{ inmuebleDetalle.ubicaciones }}</span>
                <NuxtLink 
                  v-if="inmuebleDetalle.ubicaciones"
                  :to="`/propiedades/zona/${slugifyZona(inmuebleDetalle.ubicaciones)}`"
                  class="text-xs text-blue-600 hover:underline mt-1"
                >
                  Propiedades disponibles en {{ inmuebleDetalle.ubicaciones }}
                </NuxtLink>
              </div>
            </div>
            <div class="flex items-center justify-between">
              <svg
                class="w-6 h-6 me-2"
                fill="#000000"
                height="200px"
                width="200px"
                version="1.1"
                id="Layer_1"
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                viewBox="0 0 512 512"
                xml:space="preserve"
              >
                <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                <g
                  id="SVGRepo_tracerCarrier"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                ></g>
                <g id="SVGRepo_iconCarrier">
                  <g>
                    <g>
                      <path
                        d="M501.333,256h-344.32c2.027-6.933,3.093-14.08,2.987-21.333c0-25.6-32.107-42.667-53.333-42.667h-64 c-7.467,0-14.827,2.133-21.333,5.973v-80.64c0-5.867-4.8-10.667-10.667-10.667C4.8,106.667,0,111.467,0,117.333v277.333 c0,5.867,4.8,10.667,10.667,10.667c5.867,0,10.667-4.8,10.667-10.667v-53.333h469.333V384c0,5.867,4.8,10.667,10.667,10.667 c5.867,0,10.667-4.8,10.667-10.667V266.667C512,260.8,507.2,256,501.333,256z M42.667,213.333h64c12.907,0,32,11.2,32,21.333 C138.667,256,132.693,256,128,256H42.667c-11.733,0-21.333-9.6-21.333-21.333S30.933,213.333,42.667,213.333z M490.667,320H21.333 v-42.667h469.333V320z"
                      ></path>
                    </g>
                  </g>
                </g>
              </svg>
              {{ inmuebleDetalle.habitaciones }}
            </div>
            <div class="flex items-center justify-between">
              <svg
                class="w-5 h-5 me-2"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                <g
                  id="SVGRepo_tracerCarrier"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                ></g>
                <g id="SVGRepo_iconCarrier">
                  <path
                    fill-rule="evenodd"
                    clip-rule="evenodd"
                    d="M5.38517 2.75C4.48209 2.75 3.75 3.48209 3.75 4.38516V11.25H4.03429C4.04516 11.25 4.05599 11.25 4.06675 11.25C4.07208 11.25 4.07739 11.25 4.08268 11.25L19.9332 11.25C19.944 11.25 19.9548 11.25 19.9657 11.25H22C22.4142 11.25 22.75 11.5858 22.75 12C22.75 12.4142 22.4142 12.75 22 12.75H21.7321C21.7386 12.7949 21.7433 12.8405 21.7463 12.8864C21.7501 12.9442 21.75 13.0066 21.75 13.0668L21.75 13.1047C21.75 13.4799 21.75 13.6998 21.7344 13.9452C21.5925 16.1815 20.384 18.2467 18.6326 19.597C18.6463 19.6186 18.6591 19.6412 18.6708 19.6646L19.6708 21.6646C19.8561 22.0351 19.7059 22.4856 19.3354 22.6708C18.9649 22.8561 18.5144 22.7059 18.3292 22.3354L17.3615 20.4C16.5597 20.8059 15.6878 21.073 14.7809 21.1648C14.5364 21.1896 14.3872 21.1952 14.133 21.2047L14.1263 21.205C13.3861 21.2328 12.6615 21.25 12 21.25C11.3385 21.25 10.6139 21.2328 9.87368 21.205L9.86699 21.2047C9.61278 21.1952 9.46358 21.1896 9.2191 21.1648C8.31222 21.073 7.44028 20.8059 6.63851 20.4L5.67082 22.3354C5.48558 22.7059 5.03507 22.8561 4.66459 22.6708C4.29411 22.4856 4.14394 22.0351 4.32918 21.6646L5.32918 19.6646C5.34089 19.6412 5.35366 19.6186 5.3674 19.597C3.61596 18.2467 2.4075 16.1815 2.26556 13.9452C2.24999 13.6998 2.24999 13.4798 2.25 13.1046L2.25 13.0827C2.25 13.0774 2.25 13.0721 2.24999 13.0668C2.24999 13.0483 2.24998 13.0296 2.25008 13.0108C2.25003 13.0072 2.25 13.0036 2.25 13V12.75H2C1.58579 12.75 1.25 12.4142 1.25 12C1.25 11.5858 1.58579 11.25 2 11.25H2.25V4.38516C2.25 2.65366 3.65366 1.25 5.38517 1.25C6.66715 1.25 7.81998 2.0305 8.29609 3.22079L8.40623 3.49613C9.19952 3.29489 10.0603 3.34152 10.8717 3.68813C11.887 4.12189 12.6258 4.94029 13.0041 5.90053C13.1526 6.27744 12.975 6.70417 12.6029 6.86436L6.64215 9.43044C6.45572 9.51069 6.24473 9.51197 6.05735 9.43396C5.86997 9.35596 5.72221 9.20535 5.6478 9.01651C5.26959 8.05665 5.24692 6.94515 5.66723 5.91014C5.96643 5.17335 6.45214 4.56929 7.04665 4.13607L6.90338 3.77788C6.65506 3.15708 6.05379 2.75 5.38517 2.75ZM4.08268 12.75C4.04261 12.75 4.01877 12.75 4.00076 12.7502C3.98765 12.7504 3.98298 12.7506 3.98281 12.7506C3.98215 12.7506 3.98276 12.7506 3.98281 12.7506C3.85775 12.7587 3.75904 12.8581 3.75057 12.9831C3.75052 12.9843 3.75035 12.9893 3.75022 13.0008C3.75001 13.0188 3.75 13.0426 3.75 13.0827C3.75 13.4853 3.75031 13.6573 3.76255 13.8501C3.94798 16.7718 6.45762 19.3775 9.37024 19.6725C9.5652 19.6922 9.67311 19.6964 9.92999 19.7061C10.658 19.7334 11.3629 19.75 12 19.75C12.6371 19.75 13.342 19.7334 14.07 19.7061C14.3269 19.6964 14.4348 19.6922 14.6298 19.6725C17.5424 19.3775 20.052 16.7718 20.2375 13.8501C20.2497 13.6573 20.25 13.4853 20.25 13.0827C20.25 13.0426 20.25 13.0188 20.2498 13.0008C20.2497 12.9906 20.2495 12.9855 20.2495 12.9837C20.2494 12.9825 20.2494 12.9824 20.2495 12.9837C20.2413 12.8584 20.1415 12.7587 20.0162 12.7505C20.0174 12.7506 20.0177 12.7506 20.0162 12.7505C20.0142 12.7505 20.009 12.7503 19.9992 12.7502C19.9812 12.75 19.9574 12.75 19.9173 12.75H4.08268ZM10.2824 5.06753C9.62506 4.78672 8.91452 4.82579 8.30713 5.12147C7.76827 5.3838 7.31118 5.8486 7.05701 6.47451C6.89349 6.87716 6.83436 7.29656 6.86648 7.70078L11.2476 5.81471C10.9982 5.49339 10.6713 5.2337 10.2824 5.06753Z"
                    fill="#000000"
                  ></path>
                </g>
              </svg>
              {{ inmuebleDetalle.banos }}
            </div>
            <div class="flex items-center justify-between">
              <svg
                class="w-6 h-6 me-2"
                fill="#000000"
                viewBox="0 0 50 50"
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                <g
                  id="SVGRepo_tracerCarrier"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                ></g>
                <g id="SVGRepo_iconCarrier">
                  <path
                    d="M7 6 A 1.0001 1.0001 0 0 0 6.2929688 6.2929688L3.2929688 9.2929688 A 1.0001 1.0001 0 0 0 3.0292969 10.242188L4.0292969 14.242188 A 1.0001 1.0001 0 0 0 5 15L10 15L10 17L8.5976562 17C6.7095541 17 4.9786821 18.068954 4.1308594 19.755859L4.1621094 19.697266L0.73828125 25.515625C0.25551192 26.302311 0 27.206624 0 28.128906L0 37.308594C0 38.783636 1.2163641 40 2.6914062 40L4.0917969 40C4.5721157 42.828207 7.039829 45 10 45C12.960171 45 15.427884 42.828207 15.908203 40L34.091797 40C34.572116 42.828207 37.039829 45 40 45C42.960171 45 45.427884 42.828207 45.908203 40L47 40C48.645455 40 50 38.645455 50 37L50 32.388672C50 29.951489 48.225687 27.858433 45.822266 27.457031L37.494141 26.068359L30.533203 18.777344C29.583575 17.650285 28.183972 17 26.710938 17L26 17L26 15L31 15 A 1.0001 1.0001 0 0 0 31.894531 14.447266L33.894531 10.447266 A 1.0001 1.0001 0 0 0 33.263672 9.0351562L22.263672 6.0351562 A 1.0001 1.0001 0 0 0 22 6L7 6 z M 7.4140625 8L21.865234 8L31.560547 10.644531L30.382812 13L5.78125 13L5.1074219 10.306641L7.4140625 8 z M 12 15L24 15L24 17L12 17L12 15 z M 8.5976562 19L17 19L17 26L2.7734375 26L5.9023438 20.683594L5.9179688 20.652344C6.4281453 19.637236 7.4617586 19 8.5976562 19 z M 19 19L26.710938 19C27.595903 19 28.433534 19.389465 29.003906 20.066406L29.023438 20.089844L34.662109 26L19 26L19 19 z M 2.0078125 28L36.916016 28L45.494141 29.429688C46.946721 29.672283 48 30.915854 48 32.388672L48 37C48 37.554545 47.554545 38 47 38L45.908203 38C45.427884 35.171793 42.960171 33 40 33C37.039829 33 34.572116 35.171793 34.091797 38L15.908203 38C15.427884 35.171793 12.960171 33 10 33C7.039829 33 4.5721157 35.171793 4.0917969 38L2.6914062 38C2.2984484 38 2 37.701552 2 37.308594L2 28.128906C2 28.085622 2.00596 28.043127 2.0078125 28 z M 10 35C12.220375 35 14 36.779625 14 39C14 41.220375 12.220375 43 10 43C7.7796254 43 6 41.220375 6 39C6 36.779625 7.7796254 35 10 35 z M 40 35C42.220375 35 44 36.779625 44 39C44 41.220375 42.220375 43 40 43C37.779625 43 36 41.220375 36 39C36 36.779625 37.779625 35 40 35 z"
                  ></path>
                </g>
              </svg>
              {{ inmuebleDetalle.parqueos }}
            </div>
          </div>
          <hr class="w-48 mx-auto text-gray-300" />

          <div class="mb-4 mt-4 long-text-roboto text-center">
            <ul
              class="flex flex-wrap items-center justify-center text-gray-900"
            >
              <li
                v-for="(amenidad, index) in inmuebleDetalle.amenidades"
                :key="index"
                class="bg-gray-100 text-gray-800 text-xs font-medium px-2.5 py-0.5 rounded-sm me-2 mb-4"
              >
                {{ amenidad.nombre }}
              </li>
            </ul>
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
            {{ inmuebleDetalle.titulo || "Cargando..." }}
          </h1>

          <div class="prose max-w-none description-content">
            <div v-html="formattedDescription"></div>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="!notFound && suggestedProperties && suggestedProperties.length > 0"
      class="mt-12 mb-12"
    >
      <h1
        class="title-alta-2 text-xl text-center font-bold leading-none tracking-tight text-gray-900 md:text-2xl lg:text-4xl mt-8 mb-8"
      >
        Propiedades Sugeridas
      </h1>

      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        <SugerenciaCard
          v-for="property in suggestedProperties"
          :key="property.id"
          :item="property"
          type="propiedad"
        />
      </div>
    </div>

    <div v-else-if="!notFound" class="mt-12 mb-12 text-center">
      <p class="text-gray-600 text-lg">
        No hay propiedades sugeridas disponibles.
      </p>
    </div>
  </div>
  <RedesFlotantes />
  <Footer />
</template>

<script setup>
import { ref, computed, onMounted, nextTick, watch } from "vue";
import { useRoute, useAsyncData, createError, useHead, useSeoMeta } from "#imports";
import inmuebleService from "../../services/inmuebleService";
import Header from "../../components/header.vue";
import Footer from "../../components/footer.vue";
import RedesFlotantes from "../../components/redesFlotantes.vue";
import SugerenciaCard from "~/components/SugerenciaCard.vue";
import SlugSkeleton from "~/components/slugSkeleton.vue";
import { Swiper, SwiperSlide } from "swiper/vue";
import "swiper/css";
import "swiper/css/free-mode";
import "swiper/css/mousewheel";
import "swiper/css/navigation";
import { FreeMode, Mousewheel, Navigation } from "swiper/modules";

//  1. CARGA DE DATOS ASÍNCRONA EN EL SERVIDOR (SSR)
const route = useRoute();
const slug = route.params.slug;

const {
  data: inmuebleDetalle,
  pending,
  error,
} = await useAsyncData(
  `inmueble-${slug}`, 
  async () => {
    try {
      const data = await inmuebleService.getInmuebleBySlug(slug);
      
      // ✅ Lanzar error 404 real cuando no hay datos
      if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
        throw createError({
          statusCode: 404,
          statusMessage: "Propiedad no encontrada",
          fatal: true,
        });
      }
      
      // Normalización de datos de la API (si tienen $values)
      if (data.imagenesReferencia && data.imagenesReferencia.$values) {
        data.imagenesReferencia = data.imagenesReferencia.$values;
      }
      if (data.amenidades && data.amenidades.$values) {
        data.amenidades = data.amenidades.$values;
      }
      return data;
    } catch (err) {
      console.error('[SSR ERROR] Error cargando inmueble:', err);
      // Si ya es un error de Nuxt, re-lanzarlo
      if (err.statusCode) {
        throw err;
      }
      // Si es otro error, convertirlo a 404
      throw createError({
        statusCode: 404,
        statusMessage: "Inmueble no encontrado",
        fatal: true,
      });
    }
  },
  {
    // Asegurar que se ejecute en el servidor
    server: true,
    // Lazy false para esperar los datos antes de renderizar
    lazy: false,
    // Clave única por slug
    key: `inmueble-detail-${slug}`,
  }
);

const isLoading = computed(() => pending?.value ?? false);

// 🛑 CORRECCIÓN: Declaración de notFound con ref()
const notFound = ref(false);

// NOTFOUND reactivo
watch(
  [() => inmuebleDetalle.value, () => error.value],
  () => {
    notFound.value = !!error.value || !inmuebleDetalle.value;
  },
  { immediate: true }
);

// Recarga de datos al cambiar slug client-side
const fetchDetalleBySlug = async (newSlug) => {
  if (!newSlug) return;
  try {
    try {
      pending.value = true;
    } catch (e) {}
    const data = await inmuebleService.getInmuebleBySlug(newSlug);

    if (!data || (typeof data === "object" && Object.keys(data).length === 0)) {
      inmuebleDetalle.value = null;
      notFound.value = true;
      return;
    }

    if (data.imagenesReferencia && data.imagenesReferencia.$values)
      data.imagenesReferencia = data.imagenesReferencia.$values;
    if (data.amenidades && data.amenidades.$values)
      data.amenidades = data.amenidades.$values;

    inmuebleDetalle.value = data;
    notFound.value = false;

    await nextTick();
    setTimeout(() => {
      if (tituloRef.value) {
        tituloRef.value.scrollIntoView({ behavior: "smooth", block: "start" });
      }
    }, 100);
  } catch (err) {
    inmuebleDetalle.value = null;
    notFound.value = true;
  } finally {
    try {
      pending.value = false;
    } catch (e) {}
  }
};

watch(
  () => route.params.slug,
  (newSlug, oldSlug) => {
    if (newSlug && newSlug !== oldSlug) {
      fetchDetalleBySlug(newSlug);
    }
  }
);

// Scroll al título cuando cambian datos
const tituloRef = ref(null);
const scrollToTitulo = async (smooth = true) => {
  await nextTick();
  if (tituloRef.value) {
    try {
      tituloRef.value.scrollIntoView({
        behavior: smooth ? "smooth" : "auto",
        block: "start",
      });
    } catch (e) {}
  }
};

watch(
  () => inmuebleDetalle.value,
  (detalle) => {
    if (detalle && !pending.value) {
      setTimeout(() => scrollToTitulo(true), 100);
    }
  },
  { immediate: true }
);

// ** FUNCIÓN para asegurar que el precio se lea como entero (ej: "1,600,000" -> 1600000) **
const parsePriceValue = (v) => {
  if (v == null) return null;
  if (typeof v === "number") return Number(v);
  const s = String(v).trim();
  if (s === "") return null;
  const cleaned = s.replace(/[^\d-]/g, "");
  if (cleaned === "" || cleaned === "-") return null;
  const n = Number(cleaned);
  return Number.isFinite(n) ? n : null;
};

// SEO
const pageTitle = computed(
  () => inmuebleDetalle.value?.titulo || "Propiedad en Venta y Alquiler"
);
const pageDescription = computed(() => {
  if (!inmuebleDetalle.value?.contenido) {
    return "Descubre las mejores propiedades en las zonas más exclusivas de Guatemala.";
  }
  const cleanText = inmuebleDetalle.value.contenido
    .replace(/<[^>]*>?/gm, " ")
    .replace(/\s+/g, " ")
    .trim();
  return cleanText.length > 155
    ? cleanText.substring(0, 155) + "..."
    : cleanText;
});

const pageImage = computed(() => {
  const DOMINIO_IMAGENES = "https://app-pool.vylaris.online/dcmigserver/homes";
  // const DEFAULT_IMAGE = `${DOMINIO_IMAGENES}/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp`;
  
  try {
    const img = inmuebleDetalle.value?.imagenPrincipal;
    if (!img) return DEFAULT_IMAGE;
    
    // Si ya es absoluta, retornar
    if (img.startsWith("http")) return img.replace("http://", "https://");
    
    // Si es relativa, construir
    const cleanImg = img.startsWith('/') ? img.substring(1) : img;
    return `${DOMINIO_IMAGENES}/${cleanImg}`;
  } catch (err) {
    return DEFAULT_IMAGE;
  }
});

const propertyUrl = computed(() => {
  const baseUrl = 'https://homesguatemala.com';
  const fullPath = route.fullPath || route.path || '';
  
  // Asegurar que la URL del inmueble sea específica
  if (!fullPath || fullPath === '/') {
    return `${baseUrl}/inmueble/${slug}`;
  }
  
  return `${baseUrl}${fullPath}`;
});

// Log completo de datos para debugging (solo en desarrollo)
if (process.dev) {
  console.log('🔍 [INMUEBLE SEO] Datos completos:', {
    inmuebleDetalle: inmuebleDetalle.value,
    titulo: inmuebleDetalle.value?.titulo,
    ubicaciones: inmuebleDetalle.value?.ubicaciones,
    codigoPropiedad: inmuebleDetalle.value?.codigoPropiedad,
    imagenPrincipal: inmuebleDetalle.value?.imagenPrincipal,
    slug: slug,
    error: error.value,
    pending: pending.value
  });
}

// Log en servidor para debugging de SSR
if (process.server) {
  console.log('🌐 [SSR] Generando metadatos para:', slug);
  console.log('📝 [SSR] pageTitle:', pageTitle.value);
  console.log('📝 [SSR] pageDescription:', pageDescription.value.substring(0, 100) + '...');
  console.log('🖼️ [SSR] pageImage:', pageImage.value);
  console.log('🔗 [SSR] propertyUrl:', propertyUrl.value);
}

// Configurar metadatos SEO - Fuera del watch para que SSR/Prerender los detecte correctamente
useSeoMeta({
  title: () => pageTitle.value,
  description: () => pageDescription.value,
  ogTitle: () => pageTitle.value,
  ogDescription: () => pageDescription.value,
  ogImage: () => pageImage.value,
  ogImageSecureUrl: () => pageImage.value,
  ogImageWidth: '1200',
  ogImageHeight: '630',
  ogImageAlt: () => pageTitle.value,
  ogUrl: () => propertyUrl.value,
  ogType: 'website',
  ogSiteName: 'Homes Guatemala',
  ogLocale: 'es_GT',
  robots: 'index, follow',
  author: 'Homes Guatemala',
});

useHead({
  title: () => pageTitle.value,
  htmlAttrs: {
    lang: 'es',
    prefix: 'og: http://ogp.me/ns#'
  },
  link: [
    {
      rel: 'canonical',
      href: () => propertyUrl.value
    }
  ],
  meta: [
    // Twitter Card meta tags
    { name: 'twitter:card', content: 'summary_large_image' },
    { name: 'twitter:site', content: '@homesguatemala' },
    { name: 'twitter:title', content: () => pageTitle.value },
    { name: 'twitter:description', content: () => pageDescription.value },
    { name: 'twitter:image', content: () => pageImage.value },
    { name: 'twitter:image:alt', content: () => pageTitle.value },
    // Meta tags básicos
    { name: 'description', content: () => pageDescription.value },
    { name: 'robots', content: 'index, follow, max-image-preview:large' },

    // Meta tags adicionales para WhatsApp y Facebook
    { property: 'og:image', content: () => pageImage.value },
    { property: 'og:image:secure_url', content: () => pageImage.value },
    { property: 'og:image:type', content: 'image/webp' },
    { property: 'og:image:width', content: '1200' },
    { property: 'og:image:height', content: '630' },
    { name: 'thumbnail', content: () => pageImage.value },
    { name: 'twitter:image:src', content: () => pageImage.value },

    // FB App ID
    { property: 'fb:app_id', content: '239174403519612' },
  ],
  script: [
    {
      type: 'application/ld+json',
      children: computed(() => JSON.stringify({
        '@context': 'https://schema.org',
        '@type': 'RealEstateListing',
        name: pageTitle.value,
        description: pageDescription.value,
        url: propertyUrl.value,
        image: {
          '@type': 'ImageObject',
          url: pageImage.value,
          width: 1200,
          height: 630
        },
        offers: inmuebleDetalle.value?.precio ? {
          '@type': 'Offer',
          price: parsePriceValue(inmuebleDetalle.value.precio),
          priceCurrency: 'USD',
          availability: 'https://schema.org/InStock'
        } : undefined,
        address: inmuebleDetalle.value?.ubicaciones ? {
          '@type': 'PostalAddress',
          addressLocality: inmuebleDetalle.value.ubicaciones,
          addressCountry: 'GT'
        } : undefined
      }))
    }
  ]
});

// Vista / media / formato
const isMobile = ref(false);
const mainMedia = ref({ url: "", type: "image" });
const currentMediaIndex = ref(0);
const suggestedProperties = ref([]);

// Carrusel y helpers multimedia
const allMedia = computed(() => {
  const media = [];
  if (inmuebleDetalle.value?.video) {
    media.push({ url: inmuebleDetalle.value.video, type: "video" });
  }
  if (inmuebleDetalle.value?.imagenPrincipal) {
    media.push({ url: pageImage.value, type: "image" });
  }
  if (
    inmuebleDetalle.value?.imagenesReferencia &&
    inmuebleDetalle.value.imagenesReferencia.length > 0
  ) {
    const DOMINIO_IMAGENES =
      "https://app-pool.vylaris.online/dcmigserver/homes";
    media.push(
      ...inmuebleDetalle.value.imagenesReferencia.map((img) => ({
        url: img.url.startsWith("http")
          ? img.url
          : `${DOMINIO_IMAGENES}/${img.url}`,
        type: "image",
      }))
    );
  }
  if (media.length > 0) {
    mainMedia.value = media[0];
  }
  return media;
});

// Para evitar el warning de Swiper, solo habilitar loop cuando hay suficientes slides
const shouldEnableLoop = computed(() => {
  const minSlidesForLoop = isMobile.value ? 4 : 5; // slidesPerView + 1
  return allMedia.value.length >= minSlidesForLoop;
});

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

const isYouTubeVideo = (url) =>
  url?.includes("youtube.com/watch?v=") || url?.includes("youtu.be/");
const getYouTubeEmbedUrl = (url) => {
  const videoId = isYouTubeVideo(url)
    ? url.split("v=")[1] || url.split("youtu.be/")[1]
    : "";
  return videoId
    ? `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`
    : "";
};
const getYouTubeThumbnail = (url) => {
  const videoId = isYouTubeVideo(url)
    ? url.split("v=")[1] || url.split("youtu.be/")[1]
    : "";
  return videoId ? `https://img.youtube.com/vi/${videoId}/hqdefault.jpg` : null;
};

// Función para mezclar array aleatoriamente (Fisher-Yates shuffle)
const shuffleArray = (array) => {
  const shuffled = [...array];
  for (let i = shuffled.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
  }
  return shuffled;
};

// Helper para convertir zona a slug
const slugifyZona = (zona) => {
  if (!zona) return '';
  return zona
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/\s+/g, '-')
    .replace(/[^a-z0-9-]/g, '');
};

// Formateo y descripción
const formattedDescription = computed(() => {
  const c = inmuebleDetalle.value?.contenido ?? "";
  const looksLikeHtml =
    /<\s*(p|br|ul|ol|li|h[1-6]|div|span|blockquote|table)\b/i.test(c);
  if (looksLikeHtml) return c;
  return c
    .split(/\r?\n\r?\n/)
    .map((p) => `<p>${p.replace(/\r?\n/g, "<br>")}</p>`)
    .join("");
});

const whatsappLink = computed(() => {
  const phoneNumber = "50256330961";
  const titulo = inmuebleDetalle.value?.titulo || 'Propiedad';
  const url = propertyUrl.value;
  
  const message = `Estoy interesado en esta propiedad: ${titulo}\n\n${url}`;
  
  return `https://api.whatsapp.com/send/?phone=${phoneNumber}&text=${encodeURIComponent(message)}`;
});

const formattedPrice = computed(() => {
  if (inmuebleDetalle.value?.precioActivo === false) {
    return " ";
  }
  if (!inmuebleDetalle.value?.precio) return "";
  const number = parsePriceValue(inmuebleDetalle.value.precio);
  if (number === null) return "";
  return new Intl.NumberFormat("es-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(number);
});

// ----------------- LÓGICA SUGERENCIAS: PRIORIDAD ZONA / PRECIO -----------------
const PRICE_MARGIN = 0.35; // 35% por defecto (ajustable)
const MAX_SUGGESTIONS = 3;

const loadSuggestedProperties = async () => {
  try {
    // Optimización: Reducido de 200 a 20 para evitar over-fetching
    // Solo necesitamos 3 sugerencias, no tiene sentido cargar 200 propiedades
    const responseData = await inmuebleService.getInmueblesPaginados(1, 20);
    const allRaw = Array.isArray(responseData?.items)
      ? responseData.items
      : Array.isArray(responseData)
      ? responseData
      : [];

    if (!allRaw.length || !inmuebleDetalle.value) {
      suggestedProperties.value = [];
      return;
    }

    const current = inmuebleDetalle.value;
    const currentId = current?.id ?? null;

    const getZone = (p) => p?.zona ?? p?.ubicaciones ?? p?.ubicacion ?? p?.zonaPropiedad ?? null;
    const getRawPrice = (p) => p?.precio ?? p?.valor ?? p?.price ?? null;

    const currentZone = getZone(current);
    const currentPrice = parsePriceValue(getRawPrice(current));

    // Si no hay precio válido, igual seguimos con zonas y relleno aleatorio
    // Construir lista parseada y aplicar heurística de escala si parece necesario
    const SCALE_THRESHOLD = 10000;
    const SCALE_FACTOR = 1000;

    const parsedList = allRaw.map((p) => {
      const copy = { ...p };
      const raw = getRawPrice(copy);
      let parsed = parsePriceValue(raw);
      // heurística: si current tiene precio grande y este tiene un número pequeño, escalar
      if (currentPrice != null && currentPrice > 100000 && parsed != null && parsed < SCALE_THRESHOLD) {
        parsed = parsed * SCALE_FACTOR;
        copy.precio = parsed;
      }
      return { original: copy, parsedPrice: parsed };
    });

    const available = parsedList.filter((x) => x.original?.id !== currentId);

    const isPriceSimilar = (a, b) => {
      if (a == null || b == null) return false;
      const max = Math.max(a, b);
      const min = Math.min(a, b);
      if (min === 0) return false;
      return (max - min) / min <= PRICE_MARGIN;
    };

    // Prioridades solicitadas:
    // 1) misma Zona + Precio similar
    // 2) misma Zona, precio no similar
    // 3) precio similar, diferente zona

    const zoneAndPrice = available
      .filter((x) => {
        const z = getZone(x.original);
        return z && currentZone && z === currentZone && isPriceSimilar(x.parsedPrice, currentPrice);
      })
      .map((x) => x.original);

    const zoneOnly = available
      .filter((x) => {
        const z = getZone(x.original);
        return z && currentZone && z === currentZone && !isPriceSimilar(x.parsedPrice, currentPrice);
      })
      .map((x) => x.original);

    const priceOnly = available
      .filter((x) => {
        const z = getZone(x.original);
        const sameZone = z && currentZone && z === currentZone;
        return !sameZone && isPriceSimilar(x.parsedPrice, currentPrice);
      })
      .map((x) => x.original);

    // Combinar en orden y deduplicar
    const combined = [];
    const seen = new Set();
    const push = (list) => {
      for (const it of list) {
        if (!it || !it.id) continue;
        if (!seen.has(it.id)) {
          combined.push(it);
          seen.add(it.id);
        }
        if (combined.length >= MAX_SUGGESTIONS) return;
      }
    };

    push(zoneAndPrice);
    if (combined.length < MAX_SUGGESTIONS) push(zoneOnly);
    if (combined.length < MAX_SUGGESTIONS) push(priceOnly);

    // Rellenar con aleatorias si hace falta
    if (combined.length < MAX_SUGGESTIONS) {
      const remaining = available.map((x) => x.original).filter((p) => !seen.has(p.id));
      const shuffled = shuffleArray(remaining);
      for (const r of shuffled) {
        if (combined.length >= MAX_SUGGESTIONS) break;
        combined.push(r);
        seen.add(r.id);
      }
    }

    suggestedProperties.value = combined.slice(0, MAX_SUGGESTIONS);
  } catch (err) {
    //console.error("Error al cargar propiedades sugeridas:", err);
    suggestedProperties.value = [];
  }
};

// Ejecutar cuando cambie la propiedad (SSR + navegación client-side)
watch(
  () => inmuebleDetalle.value?.id,
  (id) => {
    if (id) loadSuggestedProperties();
  },
  { immediate: true }
);

// también en mounted por si acaso
onMounted(() => {
  loadSuggestedProperties();
  const checkMobile = () => (isMobile.value = window.innerWidth < 768);
  checkMobile();
  window.addEventListener("resize", checkMobile);

  nextTick(() => {
    if (route?.path && route.path.includes("/inmueble") && tituloRef.value) {
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

.description-content p {
  margin-top: 0;
  margin-bottom: 0;
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

@import "swiper/css";
@import "swiper/css/free-mode";
@import "swiper/css/mousewheel";
@import "swiper/css/navigation";
</style>
