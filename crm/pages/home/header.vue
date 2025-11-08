<template>
  <div class="relative">
    <!-- Sección Hero con imagen de fondo -->
    <div
      class="h-[550px] bg-cover bg-no-repeat bg-center"
      style="
        background-image: url('https://homesguatemala.com/wp-content/uploads/2021/06/Sin-titulo-7.png');
      "
    >
      <!-- Contenido del hero -->
      <div class="flex items-end pb-18 justify-center h-full bg-[#00000060]">
        <div class="grid grid-rows-2 gap-2 w-3xl">
          <div class="flex items-end">
            <!-- Form busqueda -->
            <form class="w-80 md:w-full max-w-lg mx-auto">
              <label
                for="default-search"
                class="mb-2 text-sm font-medium text-gray-900 sr-only  "
                >Buscar</label
              >
              <div class="relative">
                <div
                  class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none"
                >
                  <svg
                    class="w-4 h-4 text-gray-500 "
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 20 20"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
                    />
                  </svg>
                </div>
                <input
                  type="search"
                  id="default-search"
                  class="block w-full p-4 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50          "
                  placeholder="Buscar..."
                  required
                />
                <button
                  type="submit"
                  class="text-white absolute end-2.5 bottom-2.5 bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2    "
                >
                  Buscar
                </button>
              </div>
            </form>
          </div>
          <div>
            <!-- Form filtros -->
            <form class="w-80 md:w-full max-w-5xl mx-auto">
              <div class="grid grid-cols-2 md:flex ">
                <select
                  id="countries"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-xs md:rounded-l-lg block w-full p-2.5          "
                >
                  <option selected>Venta/renta</option>
                  <option value="US">Venta(422)</option>
                  <option value="CA">Renta(321)</option>
                </select>

                <!-- Proiedad -->

                <button
                  id="dropdownCheckboxButton"
                  data-dropdown-toggle="dropdownDefaultCheckbox"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-xs w-full p-2.5           inline-flex items-center da"
                  type="button"
                >
                  Propiedades<svg
                    class="w-2.5 h-2.5 ms-3"
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
                </button>

                <!-- Dropdown menu -->
                <div
                  id="dropdownDefaultCheckbox"
                  class="z-10 hidden w-42 bg-white divide-y divide-gray-100 shadow-sm    trans"
                >
                  <ul
                    class="p-3 space-y-3 text-xs text-gray-700 "
                    aria-labelledby="dropdownCheckboxButton"
                  >
                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-3"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-3"
                          class="ms-2 text-xs text-gray-900 "
                          >Casas (190)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-4"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-4"
                          class="ms-2 text-xs text-gray-900 "
                          >Apartamentos (90)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-5"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-5"
                          class="ms-2 text-xs text-gray-900 "
                          >Terrenos (60)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-3"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-3"
                          class="ms-2 text-xs text-gray-900 "
                          >Oficinas (30)</label
                        >
                      </div>
                    </li>
                  </ul>
                </div>

                <!-- Zonas -->

                <button
                  id="dropdownCheckboxButton1"
                  data-dropdown-toggle="dropdownDefaultCheckbox1"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-xs w-full p-2.5           inline-flex items-center da"
                  type="button"
                >
                  Ubicacion<svg
                    class="w-2.5 h-2.5 ms-3"
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
                </button>

                <!-- Dropdown menu -->
                <div
                  id="dropdownDefaultCheckbox1"
                  class="z-10 hidden w-48 bg-white divide-y divide-gray-100 shadow-sm    trans"
                >
                  <ul
                    class="p-3 space-y-3 text-xs text-gray-700 "
                    aria-labelledby="dropdownCheckboxButton1"
                  >
                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-3"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-3"
                          class="ms-2 text-xs text-gray-900 "
                          >San José Pinula (2)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-4"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-4"
                          class="ms-2 text-xs text-gray-900 "
                          >CAES Abajo KM 14 (45)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-5"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-5"
                          class="ms-2 text-xs text-gray-900 "
                          >Monteeico (1)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-3"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-3"
                          class="ms-2 text-xs text-gray-900 "
                          >Muxbal (3)</label
                        >
                      </div>
                    </li>
                  </ul>
                </div>

                <!-- Habitaciones -->

                <button
                  id="dropdownCheckboxButton2"
                  data-dropdown-toggle="dropdownDefaultCheckbox2"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-xs w-full p-2.5           inline-flex items-center da"
                  type="button"
                >
                  Habitaciones<svg
                    class="w-2.5 h-2.5 ms-3"
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
                </button>

                <!-- Dropdown menu -->
                <div
                  id="dropdownDefaultCheckbox2"
                  class="z-10 hidden w-32 bg-white divide-y divide-gray-100 shadow-sm    trans"
                >
                  <ul
                    class="p-3 space-y-3 text-xs text-gray-700 "
                    aria-labelledby="dropdownCheckboxButton2"
                  >
                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-3"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-3"
                          class="ms-2 text-xs text-gray-900 "
                          >1 (45)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-4"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-4"
                          class="ms-2 text-xs text-gray-900 "
                          >2 (55)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-5"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-5"
                          class="ms-2 text-xs text-gray-900 "
                          >3 (24)</label
                        >
                      </div>
                    </li>

                    <li>
                      <div class="flex items-center">
                        <input
                          id="checkbox-item-3"
                          type="checkbox"
                          value=""
                          class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm    "
                        />
                        <label
                          for="checkbox-item-3"
                          class="ms-2 text-xs text-gray-900 "
                          >4 (33)</label
                        >
                      </div>
                    </li>
                  </ul>
                </div>

                <input
                  type="number"
                  id="first_name"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-xs block w-full p-2.5"
                  placeholder="$ Minimo"
                />

                <input
                  type="number"
                  id="first_name"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-xs block w-full p-2.5"
                  placeholder="$ Maximo"
                />

                <button
                  type="submit"
                  class="text-white bg-gray-700 hover:bg-gray-800 font-medium md:rounded-r-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center    "
                >
                  Buscar
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>

    <header class="absolute top-0 left-0 w-full mt-8">
      <nav
        class="bg-[#ffffff90] border-gray-200 px-4 lg:px-6 py-8 "
      >
        <div
          class="flex flex-wrap justify-between items-center mx-auto max-w-screen-xl"
        >
          <a href="#" class="flex items-center">
            <img
              src="https://homesguatemala.com/wp-content/themes/homesgtdm/images/logo-homes-guatemala.png"
              class="mr-3 h-16 sm:h-16"
              alt="Homesguatemala Logo"
            />
          </a>
          <div class="flex items-center lg:order-2">
            <button
              data-collapse-toggle="mobile-menu-2"
              type="button"
              class="inline-flex items-center p-2 ml-1 text-sm text-gray-900 rounded-lg lg:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200   "
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
            class="hidden justify-between items-center w-full lg:flex lg:w-auto lg:order-1"
            id="mobile-menu-2"
          >
            <ul
              class="flex flex-col mt-4 text-sm font-medium uppercase lg:flex-row lg:space-x-8 lg:mt-0"
            >
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 bg-primary-700 lg:bg-transparent lg:text-primary-700 lg:p-0   relative group"
                  aria-current="page"
                  >Inicio
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span></a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                  >Lista de propiedades
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span></a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                  >Luxury Homes
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span></a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                >
                  Proyectos
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span>
                </a>
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                  >Nosotros
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span>
                </a>
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                  >Blog
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span></a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                  >Videos
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span></a
                >
              </li>
              <li>
                <a
                  href="#"
                  class="block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 lg:hover:bg-transparent lg:border-0 lg:hover:text-primary-700 lg:p-0  lg:       relative group"
                  >Busqueda Avanzada
                  <span
                    class="absolute bottom-0 left-0 w-0 h-0.5 bg-gray-900 transition-all duration-300 group-hover:w-full"
                  ></span></a
                >
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>
  </div>

  
</template>

<script setup>
import { onMounted } from "vue";
import { initFlowbite } from "flowbite";

onMounted(() => {
  initFlowbite();
});
</script>
