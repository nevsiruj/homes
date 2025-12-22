<template>
  <Header />
  <section class="text-gray-600 body-font">
    <div class="container px-5 mt-38 md:mt-24 lg:mt-24 mx-auto">
      <div class="flex flex-col text-center w-full">
        <h1
          class="text-2xl md:text-4xl lg:text-5xl font-medium title-alta text-gray-900"
        >
          NOSOTROS
        </h1>
        <hr
          class="w-48 h-1 mx-auto my-2 mb-6 bg-gray-100 border-0 rounded-sm"
        />
      </div>
      <p class="mb-6 leading-relaxed long-text-roboto">
        En Homes Guatemala, combinamos experiencia, visión y un enfoque
        personalizado para ofrecer soluciones inmobiliarias de alto nivel. Nos
        especializamos en la venta y alquiler de casas, apartamentos y terrenos
        en ubicaciones estratégicas como Zona 10, Zona 13, Zona 14, Zona 15,
        Zona 16 y Carretera a El Salvador, así como proyectos vacacionales en
        destinos como Antigua Guatemala y zonas de playa.
      </p>

      <p class="mb-6 leading-relaxed long-text-roboto">
        Además de representar propiedades cuidadosamente seleccionadas,
        desarrollamos nuestros propios proyectos inmobiliarios, diseñados con un
        enfoque en la calidad arquitectónica, la funcionalidad y el potencial de
        inversión. Esto nos permite ofrecer propuestas únicas, tanto para
        quienes buscan un hogar excepcional como para quienes buscan
        oportunidades de inversión en bienes raíces en Guatemala.
      </p>

      <p class="mb-6 leading-relaxed long-text-roboto font-bold">
        Nuestro portafolio incluye:
      </p>

      <ul class="max-w-md space-y-1 long-text-roboto text-gray-500 list-disc list-inside mb-6 ">
    <li>
      Casas modernas y residencias de lujo

    </li>
    <li>
       Apartamentos en zonas exclusivas
    </li>
    <li>
      Terrenos con alto potencial de desarrollo
    </li>
    <li>Propiedades en renta para uso residencial o corporativo</li>
    <li>Proyectos inmobiliarios en preventa y en desarrollo</li>
    <li>Inversiones en destinos turísticos clave</li>
</ul>
<p class="mb-6 leading-relaxed long-text-roboto">
  Ya sea que estés buscando comprar, alquilar o invertir en Guatemala, nuestro equipo de expertos está listo para asesorarte con transparencia, profesionalismo y un profundo conocimiento del mercado local.
      </p>
    </div>
  </section>

  <section class="bg-black text-white py-12 mt-12">
    <div class="container px-5 mx-auto">
      <div class="mx-auto max-w-7xl px-6 lg:px-8">
        <dl
          class="grid grid-cols-1 gap-x-8 gap-y-16 text-center lg:grid-cols-3"
        >
          <div class="mx-auto flex max-w-xs flex-col gap-y-4">
            <dt class="text-base/7 text-white font-bold">Negocios cerrados</dt>
            <dd
              class="order-first text-3xl font-semibold tracking-tight text-white sm:text-5xl"
            >
              1000+
            </dd>
          </div>
          <div class="mx-auto flex max-w-xs flex-col gap-y-4">
            <dt class="text-base/7 text-white font-bold">
              Años de experiencia
            </dt>
            <dd
              class="order-first text-3xl font-bold tracking-tight text-white sm:text-5xl"
            >
              18+
            </dd>
          </div>
          <div class="mx-auto flex max-w-xs flex-col gap-y-4">
            <dt class="text-base/7 text-white font-semibold">
              Enfocados en encontrar tu propiedad ideal
            </dt>
            <dd
              class="order-first text-3xl font-semibold tracking-tight text-white sm:text-5xl"
            >
              100%
            </dd>
          </div>
        </dl>
      </div>
    </div>
  </section>

  <section class="text-gray-600 body-font">
    <div class="container px-5 py-12 mx-auto">
      <!-- Título -->
      <div class="flex flex-col text-center w-full mb-10">
        <!-- <hr
          class="w-48 h-1 mx-auto my-2 bg-gray-100 border-0 rounded-sm md:my-10"
        /> -->
        <h3
          class="text-2xl md:text-4xl lg:text-5xl font-medium title-alta mb-4 text-gray-900"
        >
          Nuestro equipo
        </h3>
      </div>

      <!-- Equipo -->
      <div class="bg-white">
        <div class="mx-auto max-w-7xl px-6 lg:px-8">
          <dl
            class="grid grid-cols-1 gap-x-8 gap-y-16 text-center lg:grid-cols-3"
          >
            <div
              v-for="(miembro, index) in equipo"
              :key="index"
              class="mx-auto flex max-w-xs flex-col gap-y-4"
            >
              <img
                class="mx-auto mb-4 w-72 h-auto"
                :src="miembro.foto"
                alt="Foto del miembro"
              />
              <h3
                class="mb-1 text-2xl font-bold subtitle-optima tracking-tight text-gray-900"
              >
                {{ miembro.nombre }}
              </h3>
              <p class="font-bold subtitle-optima1">{{ miembro.cargo }}</p>
              <p class="font-bold subtitle-optima1">{{ miembro.cargo2 }}</p>

              <!-- Biografía con Ver Más -->
              <p
                :class="{ 'line-clamp-3': !miembro.expanded }"
                class="text-start transition-all long-text-roboto duration-300 ease-in-out"
              >
                {{ miembro.bio }}
              </p>

              <!-- Botón Ver Más -->
              <button
                @click="toggleBio(miembro)"
                v-if="miembro.bio && miembro.bio.length > 100"
                class="mt-2 text-sm text-gray-500 hover:text-black focus:outline-none"
              >
                {{ miembro.expanded ? "Ver menos" : "Ver más" }}
              </button>
            </div>
          </dl>
        </div>
      </div>
    </div>
  </section>
  <RedesFlotantes />
  <Footer />
</template>

<script setup>
import { ref } from "vue";
import RedesFlotantes from "../../components/redesFlotantes.vue";

useSeoMeta({
  title: 'Sobre Homes Guatemala | Expertos Inmobiliarios',
  description: 'Conoce a Homes Guatemala, líderes con 18 años de experiencia en bienes raíces de lujo.',
  ogTitle: 'Sobre Nosotros - Homes Guatemala',
  ogDescription: 'Nuestra historia, misión y equipo de expertos inmobiliarios.',
  ogImage: 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp',
  twitterCard: 'summary_large_image',
})

useHead({
  link: [
    { rel: 'canonical', href: 'https://homesguatemala.com/nosotros' }
  ]
})

// Datos de ejemplo del equipo
const equipo = ref([
  {
    nombre: "Ryan Sisco",
    cargo: "Creative Director",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/5c809c92-b99e-45a7-aa09-7a41222916b5.webp",
    bio: "Creció en Estados Unidos y a los 21 años compro su primer apartamento, desde entonces le ha apasionado el Real Estate. Con estudios en la Universidad Francisco Marroquín en Administración de Proyectos Inmobiliarios, una maestría en relaciones internacionales y otra en educación. Fundó Homes Guatemala en el año 2007 con la idea de brindar un servicio al cliente estilo americano y siempre innovar en tecnología. Actualmente es la gerente y directora creativa de dicha empresa, en los últimos años ha lanzado sus propios proyectos de desarrollo y remodelación de casas. Es una persona creativa, innovadora y aventurera que a viajado a más de 70 países, le encanta el arte, la lectura, la meditación y pasar tiempo en familia.",
    expanded: false,
  },
  {
    nombre: "David Echeverria",
    cargo: "Sales Manager & Agent",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/4cc9c675-ec91-437d-bd11-30af0db8d172.webp",
    bio: "Graduado en Psicología Clínica. Experto en el mercado de inversiones residenciales con más de 15 años de experiencia. Amante de los atardeceres y la belleza de nuestra ciudad con su vista a los volcanes. Apasionado por la fotografía artística y arquitectónica.",
    expanded: false,
  },
  {
    nombre: "Deborah Rímola",
    cargo: "Senior Agent",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/a3bb69b2-24e1-4c24-b589-9d08195e58e1.webp",
    bio: "Graduada en Administración de Empresas, apasionada por la vida, las personas y su bienestar. Le gusta la decoración, el arte y la lectura. Disfruta ver el crecimiento de las familias dando pasos tan importantes como la adquisición de una nueva propiedad.",
    expanded: false,
  },
  {
    nombre: "María Isabel Perdomo",
    cargo: "Real Estate Agent",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/c8a32744-fb90-49eb-a21e-2a116799834d.webp",
    bio: "Graduada en Administración de Empresas con especialización en Mercadeo. Apasionada por la arquitectura, el mundo de los negocios y los fun facts. En su tiempo libre le gusta disfrutar de la naturaleza, practicar yoga y leer.",
    expanded: false,
  },
  {
    nombre: "José Carlos Ralón",
    cargo: "Real Estate Agent",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/6db4a048-2b54-4617-9cf1-40aa5007963d.webp",
    bio: "Graduado de Administración de Empresas. 15 años de experiencia en servicio al cliente interno y externo. Experto en manejo de productos financieros y ejecución de proyectos. La gusta la música clásica y pasar tiempo con sus seres queridos.",
    expanded: false,
  },
  {
    nombre: "María Cifuentes",
    cargo: "Real Estate Agent",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/eeaaaeae-a25b-4c64-99ef-33c1b85cee1a.webp",
    bio: "Graduada de Ciencias Políticas con una maestría en Políticas Públicas y Sociales de la MBA Pontificia Católica de Chile. Le encanta estar en constante aprendizaje, viajar y conocer culturas nuevas. Le gusta la música, la lectura y hacer Barre.",
    expanded: false,
  },
  {
    nombre: "Genesís Marquez",
    cargo: "Real Estate Agent",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/3d4d84fc-099b-4508-9a9b-036fb11d0541.webp",
    bio: "Graduada de Ingeniería Industrial con especialidad en Bienes Raíces. Disfruta de establecer conexiones significativas con los clientes mientras busca la propiedad perfecta para satisfacer sus necesidades. Su enfoque combina habilidades técnicas con empatía y comprensión para encontrar soluciones que se ajusten a los requisitos específicos de cada cliente. Su objetivo es brindar un servicio personalizado y eficiente, guiando a los clientes a través del proceso de búsqueda y adquisición de propiedades de manera satisfactoria y exitosa.",
    expanded: false,
  },
  {
    nombre: "Marí Coma",
    cargo: "Real Estate Agent",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/3e8cd354-efbf-463e-affd-606ceab5d49e.webp",
    bio: "Graduada en Administración de empresas y hotelería. Cuanta con 8 años de experiencia en el área de servicio al cliente y le apasiona crear experiencias para los clientes y apoyarlos en encontrar la propiedad ideal para ellos.",
    expanded: false,
  },
  {
    nombre: "Diana Carvajal",
    cargo: "Coordinadora de Proyectos / Agente",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/7563e477-bec4-4922-9c48-636ddec51889.webp",
    bio: "Con más de 15 años de experiencia en administración de empresas, servicio al cliente y ventas, es una apasionada agente Inmobiliario que combina su amor por las propiedades exclusivas con su misión de encontrar hogares excepcionales para clientes exigentes.",
    expanded: false,
  },
  {
    nombre: "Stefan Kaltschmitt",
    cargo: "Real Estate Agent ",
    cargo2: "Million Dollar Club",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/2e3c8354-1598-444e-a42a-1321ef9df785.webp",
    bio: "Graduado de Ciencias de la Comunicación con un técnico en post producción de imagen y sonido en el CEV en Madrid España. Cuenta con varios años de experiencia en remodelación de espacios para convertirlos en airbnb, apasionado por ofrecer la mejor asesoría y experiencia a los clientes.",
    expanded: false,
  },
  {
    nombre: "Vivian Juárez",
    cargo: "Coordinadora de ventas",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/0e280986-fc3d-4956-90c7-171edf228b3a.webp",
    bio: "Graduada de Perito Contador, con estudios en la carrera de Ciencias Jurídicas y Sociales. Es una persona dedicada y apasionada por el servicio al cliente, comprometida con ofrecer la mejor atención y establecer relaciones confiables con los propietarios y los clientes. Le gusta viajar, leer, escuchar música y compartir tiempo en familia.",
    expanded: false,
  },
  {
    nombre: "Jakelin Tellez",
    cargo: "Asistente de contenido",
    foto: "https://app-pool.vylaris.online/dcmigserver/homes/0e27743d-453c-49e7-8a9d-5c6abc0afda0.webp",
    bio: "Graduada de Perito en Mercadotecnia y publicidad, con estudios en Diseño gráfico y administración de empresas. Es una persona creativa, servicial y dinámica que busca por medio de sus habilidades y conocimiento apoyar con la creación de contenido de calidad que atraiga la atención de los clientes. También busca innovar en las tendencias de branding y redes sociales para que la empresa este alineada con lo que el mercado solicita.",
    expanded: false,
  },
]);

// Función para alternar la expansión de la bio
const toggleBio = (miembro) => {
  miembro.expanded = !miembro.expanded;
};
</script>
