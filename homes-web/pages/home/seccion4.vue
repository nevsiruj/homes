<template>
  <Header />
  <section class="bg-white mt-32 md:mt-12 lg:mt-12">
    <div
      class="py-8 px-4 mx-auto max-w-screen-xl text-center lg:py-16 lg:px-12"
    >
      <h4
        class="text-2xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900"
      >
        BLOG INMOBILIARIO
      </h4>

      <!-- Contenedor de carga -->
      <div v-if="loading" class="flex justify-center items-center py-20">
        <div class="text-xl font-semibold text-black">
          Cargando blog...
        </div>
      </div>

      <!-- Contenedor de error -->
      <div v-else-if="error" class="flex justify-center items-center py-20">
        <div
          class="text-red-600 text-xl font-semibold p-4 border border-red-300 rounded-md bg-red-50"
        >
          Error al cargar los blog: {{ error }}
          <p class="text-sm mt-2">
            Verifica la consola para más detalles o si la API no está
            disponible.
          </p>
        </div>
      </div>

      <div v-else>
        <div
          v-if="blog.length === 0"
          class="text-center text-gray-600 text-2xl py-10"
        >
          No hay blog disponibles para mostrar.
        </div>
        <div v-else>
          <div class="flex flex-wrap -m-4 justify-center">
            <div
              v-for="blog in currentblog"
              :key="blog.id"
              class="lg:w-1/3 sm:w-1/2 p-4"
            >
              <div
                class="w-full max-w-sm bg-white border border-gray-200 rounded-lg shadow-sm overflow-hidden"
              >
                <a :href="blog.link || '#'">
                  <img
                    class="p-3 rounded-t-lg"
                    :src="blog.image"
                    :alt="blog.title"
                  />
                </a>
                <div class="flex items-center justify-between px-3 pb-5">
                  <a :href="blog.link || '#'">
                    <p
                      class="text-xl subtitle-optima font-semibold tracking-tight text-gray-900 mb-2"
                    >
                      {{ blog.title }}
                    </p>
                    <p class="mb-3 font-medium text-gray-700">{{ blog.ubicacion }}</p>
                    <p class="mb-3">
                      {{ truncateText(blog.descripcion, 80) }}
                    </p>

                    <NuxtLink
                      :to="blog.link || './seccion4'"
                      class="text-white bg-black hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-roboto rounded-lg text-sm px-5 py-2.5"
                    >
                      Seguir viendo
                    </NuxtLink>
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div v-if="totalPages > 1" class="flex justify-center py-12">
          <nav aria-label="Page navigation" class="flex justify-center">
            <ul
              class="inline-flex items-center -space-x-px text-base h-10 rounded-lg overflow-hidden border border-gray-300 shadow-sm"
            >
              <li>
                <button
                  @click="goToPrevPage"
                  :disabled="currentPage === 1"
                  :class="[
                    'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white border-r border-gray-300 rounded-s-lg transition-colors duration-200',
                    'hover:bg-black hover:text-white ',
                    { 'opacity-50 cursor-not-allowed': currentPage === 1 },
                  ]"
                >
                  Anterior
                </button>
              </li>
              <li v-for="number in pageNumbers" :key="number">
                <button
                  @click="paginate(number)"
                  :class="[
                    'flex items-center justify-center px-4 h-10 leading-tight border-r border-gray-300 transition-colors duration-200',
                    {
                      'text-white bg-gray-800 font-bold border-gray-800 ':
                        currentPage === number,
                    },
                    {
                      'text-black bg-white hover:bg-black hover:text-white  ':
                        currentPage !== number,
                    },
                  ]"
                  :aria-current="currentPage === number ? 'page' : undefined"
                >
                  {{ number }}
                </button>
              </li>
              <li>
                <button
                  @click="goToNextPage"
                  :disabled="currentPage === totalPages"
                  :class="[
                    'flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white rounded-e-lg transition-colors duration-200',
                    'hover:bg-black hover:text-white ',
                    {
                      'opacity-50 cursor-not-allowed':
                        currentPage === totalPages,
                    },
                  ]"
                >
                  Siguiente
                </button>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </div>
  </section>
<RedesFlotantes/>
  <Footer />
</template>

<script setup>
import Header from "../../components/header.vue";
import Footer from "../../components/footer.vue";
import { ref, onMounted, computed } from "vue";
import { Swiper, SwiperSlide } from "swiper/vue";
import { Autoplay, Pagination, Navigation } from "swiper/modules";

// --- Datos de prueba hardcodeados ---
const hardcodedBlog = [
  // {
  //   id: "1",
  //   title: "NAVANI",
  //   image: "https://homesguatemala.com/wp-content/uploads/2024/06/Imagen-1-400x250.jpg",
  //   link: "/blog/blogDetalle"
  // },
  {
    id: "2",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2023/03/WhatsApp-Image-2025-02-05-at-12.40.14-400x250.jpeg",
    ubicacion: "Zona 10",
    descripcion:
      "Giardino es un edificio boutique en el que la calidez de su arquitectura moderna se convierte en una forma de conexión con la naturaleza.",
  },
  {
    id: "3",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2023/03/WhatsApp-Image-2025-02-05-at-12.40.14-400x250.jpeg",
    ubicacion: "Zona 13",
    descripcion:
      "Artis, ubicado en zona 13 pocos metros de la Avenida Las Américas, en uno de los sectores residenciales más privilegiados de la ciudad.",
  },
  {
    id: "4",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2023/03/20230306_101637-400x250.jpg",
    ubicacion: "Muxbal",
    descripcion:
      "Viaggio Muxbal, es un edificio moderno con 60 apartamentos, su excelente ubicación le brinda Increíbles vistas con dos rutas de acceso y a walking distance de centros comerciales, restaurantes, farmacias y supermercados.",
  },
  {
    id: "5",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2023/10/img221-400x250.jpg",
    ubicacion: "Zona 10 ",
    descripcion:
      "Edificio de diseñador con un estilo único y  un alto enfoque en la arquitectura y diseño vanguardista, de la mano de la firma de arquitectos ARQUITECTONICA, reconocida mundialmente por su audaz modernismo y continuo desafío a los límites del diseño, con su uso innovador de materiales, geometría, patrón y color.",
  },
  {
    id: "6",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2024/02/08-Exterior-No3-FORET-400x250.jpg",
    ubicacion: "Zona 16 ",
    descripcion:
      "Un blog que combina elegancia y modernidad, ofreciéndote distintas amenidades para que disfrutes con tu familia.",
  },

  {
    id: "7",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2024/07/1.-Pardela-A-400x250.jpeg",
    ubicacion: "Zona 16 ",
    descripcion:
      "¡La experiencia de vivir rodeado de naturaleza! Sauces de El Pulté es un blog de 14 residencias exclusivas dentro de El Pulté Ecuestre, a 400 metros de la casa club.",
  },
  {
    id: "8",
    title: "Noticias Inmuebles",
    image:
      "https://homesguatemala.com/wp-content/uploads/2024/06/014-400x250.jpg",
    ubicacion: "CAES Arriba KM 14",
    descripcion:
      "Club residencial ubicado en Fraijanes a solo 2 minutos del cruce de Olmeca, muy cercano a colegios y universidades del sector; te ofrece un hogar en un entorno natural que da un valor significativo a la calidad de vida, cuenta con un ambiente que nutre tu bienestar emocional y físico, desde yoga deck hasta el polideportivo, están diseñados para ayudarte a cuidar de ti mismo y mantenerte en movimiento.",
  },
  // {
  //   id: "9",
  //   title: "blog CIMA",
  //   image: "https://homesguatemala.com/wp-content/uploads/2024/06/Imagen-1-400x250.jpg",
  // },
  // {
  //   id: "10",
  //   title: "EDEN RESIDENCIAL",
  //   image: "https://homesguatemala.com/wp-content/uploads/2023/03/WhatsApp-Image-2025-02-05-at-12.40.14-400x250.jpeg",
  // },
  // {
  //   id: "11",
  //   title: "ALTOS DE LA PRADERA",
  //   image: "https://homesguatemala.com/wp-content/uploads/2023/03/WhatsApp-Image-2025-02-05-at-12.40.14-400x250.jpeg",
  // },
  // {
  //   id: "12",
  //   title: "VILLAS DEL LAGO",
  //   image: "https://homesguatemala.com/wp-content/uploads/2023/03/20230306_101637-400x250.jpg",
  // },
  // {
  //   id: "13",
  //   title: "JARDINES DEL VALLE",
  //   image: "https://homesguatemala.com/wp-content/uploads/2023/10/img221-400x250.jpg",
  // },
  // {
  //   id: "14",
  //   title: "CIUDAD DEL SOL",
  //   image: "https://homesguatemala.com/wp-content/uploads/2024/02/08-Exterior-No3-FORET-400x250.jpg",
  // },
];

const blog = ref([]);
const loading = ref(true);
const error = ref(null);
const currentPage = ref(1);
const itemsPerPage = 6;

// Carga de Datos (Simulada para blog)
// `blogervice`
const loadblog = async () => {
  try {
    loading.value = true;
    error.value = null;
    // SIMULACIÓN de carga de API
    //  datos hardcodeados
    await new Promise((resolve) => setTimeout(resolve, 500));

    blog.value = hardcodedBlog;
  } catch (err) {
    //console.error("Error al cargar blog:", err);
    error.value = "Error al cargar blog: " + err.message;
  } finally {
    loading.value = false;
  }
};

onMounted(async () => {
  await loadblog();
});

// blog a mostrar en la página actual
const currentblog = computed(() => {
  const indexOfLastItem = currentPage.value * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  return blog.value.slice(indexOfFirstItem, indexOfLastItem);
});

// Número total de páginas
const totalPages = computed(() => {
  if (blog.value.length === 0) return 0;
  return Math.ceil(blog.value.length / itemsPerPage);
});

// números de página para renderizar la paginación
const pageNumbers = computed(() => {
  const numbers = [];
  for (let i = 1; i <= totalPages.value; i++) {
    numbers.push(i);
  }
  return numbers;
});

const paginate = (pageNumber) => {
  currentPage.value = pageNumber;
};

const goToPrevPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
  }
};

const goToNextPage = () => {
  if (currentPage.value < totalPages.value) {
    currentPage.value++;
  }
};

const modules = [Autoplay, Pagination, Navigation];

const truncateText = (text, limit = 100) => {
  if (!text) return "";
  const trimmedText = text.trim();
  if (trimmedText.length <= limit) return trimmedText;
  return trimmedText.slice(0, limit) + "...";
};
</script>
