<template>
  <div class="relative">
    <div class="relative h-[550px] w-full">
      <img
        src="https://app-pool.vylaris.online/dcmigserver/homes/0b304e4c-ed40-4c7e-8e44-2fe821bc5301.webp"
        alt="Imagen de fondo"
        class="absolute inset-0 object-cover w-full h-full"
        loading="lazy"
      />
      <div
        class="flex items-end lg:pb-8 pb-18 justify-center h-full bg-[#00000060]"
      >
        <div class="grid grid-rows-2 gap-2 w-3xl">
          <div class="flex items-end"></div>
          <Filtro v-if="!isBusquedaRoute" />
        </div>
      </div>
    </div>

    <header class="absolute top-0 left-0 w-full mt-8">
      <nav
        class="bg-[#ffffffd8] lg:bg-[#ffffffbd] md:bg-[#ffffffbd] border-gray-200 px-4 lg:px-6 py-3 md:py-8 lg:py-8"
      >
        <div
          class="flex flex-wrap justify-between items-center mx-auto max-w-screen-xl"
        >
          <NuxtLink to="/" class="flex items-center">
            <img
              src="https://app-pool.vylaris.online/dcmigserver/homes/684980eb-ab75-41e5-8c2a-045abba3b954.webp"
              class="mr-3 xl:ml-24 xl:-mb-6 h-16 xl:-mt-6 xl:h-20"
              alt="Homesguatemala Logo"
              loading="lazy"
            />
          </NuxtLink>
          <div class="flex items-center lg:order-2">
            <button
              data-collapse-toggle="mobile-menu-2"
              type="button"
              class="inline-flex items-center p-2 ml-1 text-sm text-gray-900 rounded-lg lg:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200"
              aria-controls="mobile-menu-2"
              aria-expanded="false"
            >
              <span class="sr-only">Open main menu</span>
              <svg
                class="w-10 h-10"
                fill="currentColor"
                viewBox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  fill-rule="evenodd"
                  d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z"
                  clip-rule="evenodd"
                ></path>
              </svg>
              <svg
                class="hidden w-6 h-6"
                fill="currentColor"
                viewBox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  fill-rule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clip-rule="evenodd"
                ></path>
              </svg>
            </button>
          </div>
          <div
            class="hidden justify-between gap-8 items-center w-full lg:flex lg:w-auto lg:order-1"
            id="mobile-menu-2"
          >
            <ul
              class="flex flex-col gap-6 mt-4 text-sm font-medium uppercase lg:flex-row lg:mt-0"
            >
              <li
                class="relative group"
                @mouseenter="openPropertiesDropdown"
                @mouseleave="startClosePropertiesDropdownTimer"
              >
                <div>
                  <NuxtLink
                    to="../propiedades"
                    class="block py-2 pr-4 pl-3 text-black bg-primary-700 lg:bg-transparent lg:text-primary-700 lg:p-0 relative group focus:outline-none"
                    aria-haspopup="true"
                    :aria-expanded="isPropertiesDropdownOpen"
                  >
                    PROPIEDADES
                    <span
                      :class="[
                        'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                        isPropertiesActive ? 'w-full' : 'w-0 group-hover:w-full',
                      ]"
                    ></span>
                    <svg
                      class="w-2.5 h-2.5 ml-2.5 inline-block"
                      aria-hidden="true"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="none"
                      viewBox="0 0 10 6"
                    >
                      <path
                        stroke="currentColor"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        d="m1 1 4 4 4-4"
                      />
                    </svg>
                  </NuxtLink>
                </div>

                <div
                  v-if="isPropertiesDropdownOpen"
                  @mouseenter="cancelClosePropertiesDropdownTimer"
                  @mouseleave="startClosePropertiesDropdownTimer"
                  class="absolute z-10 font-normal bg-white divide-y divide-gray-100 rounded-lg shadow w-44 top-full left-0 mt-2"
                >
                  <ul class="py-2 text-sm text-gray-700">
                    <li>
                      <a
                        href="#"
                        @click.prevent="handlePropertyFilter('Venta')"
                        class="block px-4 py-2 hover:bg-gray-100"
                        >Venta</a
                      >
                    </li>
                    <li>
                      <a
                        href="#"
                        @click.prevent="handlePropertyFilter('Renta')"
                        class="block px-4 py-2 hover:bg-gray-100"
                        >Renta</a
                      >
                    </li>
                  </ul>
                </div>
              </li>

              <li>
                <NuxtLink
                  to="../proyectos-inmobiliarios"
                  class="block py-2 pr-4 pl-3 text-black border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0 relative group"
                >
                  PROYECTOS
                  <span
                    :class="[
                      'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                      isActive('/proyectos-inmobiliarios')
                        ? 'w-full'
                        : 'w-0 group-hover:w-full',
                    ]"
                  ></span>
                </NuxtLink>
              </li>
              <li>
                <NuxtLink
                  to="../custom"
                  class="block py-2 pr-4 pl-3 text-black border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0 relative group"
                >
                  CUSTOM HOMES
                  <span
                    :class="[
                      'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                      isActive('/custom') ? 'w-full' : 'w-0 group-hover:w-full',
                    ]"
                  ></span>
                </NuxtLink>
              </li>
              <li>
                <a
                  href="/luxury-homes"
                  class="block py-2 pr-4 pl-3 text-black border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0 relative group"
                >
                  LUXURY HOMES
                  <span
                    :class="[
                      'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                      isActive('/luxury') ? 'w-full' : 'w-0 group-hover:w-full',
                    ]"
                  ></span>
              </a>
              </li>
              <li>
                <NuxtLink
                  to="../nosotros"
                  class="block py-2 pr-4 pl-3 text-black border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0 relative group"
                >
                  NOSOTROS
                  <span
                    :class="[
                      'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                      isActive('/nosotros')
                        ? 'w-full'
                        : 'w-0 group-hover:w-full',
                    ]"
                  ></span>
                </NuxtLink>
              </li>
              <li>
                <NuxtLink
                  to="../blog"
                  class="block py-2 pr-4 pl-3 text-black border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0 relative group"
                >
                  BLOG
                  <span
                    :class="[
                      'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                      isActive('../blog')
                        ? 'w-full'
                        : 'w-0 group-hover:w-full',
                    ]"
                  ></span>
                </NuxtLink>
              </li>

              <li>
                <NuxtLink
                  to="../busqueda"
                  class="block py-2 pr-4 pl-3 text-black border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0 relative group"
                >
                  BUSQUEDA AVANZADA
                  <span
                    :class="[
                      'absolute bottom-0 left-0 h-0.5 bg-gray-900 transition-all duration-300',
                      isActive('/busqueda')
                        ? 'w-full'
                        : 'w-0 group-hover:w-full',
                    ]"
                  ></span>
                </NuxtLink>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import Filtro from "@/components/filtro.vue";

const route = useRoute();
const router = useRouter();

const isBusquedaRoute = computed(() => {
  return route.path === "/busqueda";
});

const isPropertiesDropdownOpen = ref(false);
let closeDropdownTimer = null; // Variable para almacenar el ID del temporizador

const openPropertiesDropdown = () => {
  // Cuando el ratón entra, asegúrate de que el temporizador de cierre se cancele
  clearTimeout(closeDropdownTimer);
  isPropertiesDropdownOpen.value = true;
};

const startClosePropertiesDropdownTimer = () => {
  // Inicia un temporizador para cerrar el dropdown después de un breve retraso (ej. 200ms)
  closeDropdownTimer = setTimeout(() => {
    isPropertiesDropdownOpen.value = false;
  }, 300); // Puedes ajustar este valor si necesitas más o menos retraso
};

const cancelClosePropertiesDropdownTimer = () => {
  // Cancela el temporizador de cierre si el ratón vuelve a entrar en el dropdown
  clearTimeout(closeDropdownTimer);
};

onMounted(() => {
  // Aquí puedes añadir lógica si necesitas inicializar algo al montar el componente de layout
});

onUnmounted(() => {
  // Limpia el temporizador si el componente se desmonta mientras está activo
  clearTimeout(closeDropdownTimer);
});

/**
 * Maneja el clic en los filtros de 'Venta' o 'Renta' en el menú.
 * Navega a la ruta de búsqueda y establece el parámetro 'Operaciones'.
 * @param {string} operacion - 'Venta' o 'Renta' (debe coincidir con tu backend).
 */
const handlePropertyFilter = (operacion) => {
  // Cierra el dropdown inmediatamente al hacer clic
  isPropertiesDropdownOpen.value = false;
  // Limpia cualquier temporizador de cierre pendiente
  clearTimeout(closeDropdownTimer);

  const queryParams = {
    Operaciones: operacion, // <-- ¡Este es el parámetro que tu backend espera!
  };

  // Si ya estamos en la ruta de búsqueda, reemplaza los query params para mantener el historial limpio.
  // De lo contrario, navega a la ruta de búsqueda con los nuevos parámetros.
  if (route.path === "/propiedades") {
    router.replace({ path: "/propiedades", query: queryParams });
  } else {
    router.push({ path: "/propiedades", query: queryParams });
  }
};

// Propiedad computada para determinar si el enlace "PROPIEDADES" debe estar activo.
// Se considera activo si la ruta actual es /propiedades o /busqueda,
// o si tiene el parámetro 'Operaciones' en la URL.
const isPropertiesActive = computed(() => {
  return (
    route.path === "/propiedades" ||
    route.path === "/busqueda" ||
    route.query.Operaciones // Si hay algún valor para 'Operaciones' en la query
  );
});

// Función para determinar si un enlace de navegación está activo.
const isActive = (path) => {
  // Para enlaces externos (que empiezan con 'http'), compara la URL completa.
  if (path.startsWith("http")) {
    return route.fullPath === path;
  }
  // Para enlaces internos, compara la ruta base.
  return route.path === path;
};
</script>

<style scoped>
/* Opcional: añade una transición suave para la aparición/desaparición del dropdown */
.absolute {
  transition: opacity 0.2s ease-in-out, visibility 0.2s ease-in-out;
  z-index: 20;
}
</style>