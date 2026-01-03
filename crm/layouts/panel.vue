<template>
  <nav class="fixed top-0 z-50 w-full bg-white border-b border-gray-200 ">
    <div class="px-3 py-3 lg:px-5 lg:pl-3">
      <div class="flex items-center justify-between">
        <div class="flex items-center justify-start rtl:justify-end">
          <!-- para móviles queda igual -->
          <button data-drawer-target="logo-sidebar" data-drawer-toggle="logo-sidebar" aria-controls="logo-sidebar"
            type="button"
            class="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 ">
            <span class="sr-only">Open sidebar</span>
            <svg class="w-6 h-6" aria-hidden="true" fill="currentColor" viewBox="0 0 20 20"
              xmlns="http://www.w3.org/2000/svg">
              <path clip-rule="evenodd" fill-rule="evenodd"
                d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z">
              </path>
            </svg>
          </button>

          <!-- Boton hamburguesa pantalla grade -->

          <button @click="toggleSidebar" type="button"
            class="hidden sm:inline-flex items-center p-2 mr-5 text-sm text-gray-500 rounded-lg hover:bg-gray-100">
            <!-- Icono del menú hamburguesa -->
            <svg class="w-6 h-6" aria-hidden="true" fill="currentColor" viewBox="0 0 20 20"
              xmlns="http://www.w3.org/2000/svg">
              <path clip-rule="evenodd" fill-rule="evenodd"
                d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z">
              </path>
            </svg>
          </button>

          <a href="#" class="flex ms-2 md:me-24">
            <img src="https://app-pool.vylaris.online/dcmigserver/homes/684980eb-ab75-41e5-8c2a-045abba3b954.webp"
              class="h-14 me-3" alt="Logo Homes Guatemala" />
          </a>

          <!-- Boton hamburguesa -->

          <!-- <button
            @click="toggleSidebar"
            type="button"
            class="hidden sm:inline-flex items-center p-2 bg-gray-800 text-sm text-gray-200 rounded-lg hover:bg-gray-600"
          >
           
            <svg
              class="w-6 h-6"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M4 6h16M4 12h16M4 18h16"
              ></path>
            </svg>
          </button> -->
        </div>

        <!-- Información del usuario -->
        <div class="flex items-center">
          <div class="flex items-center gap-3 bg-gray-50 rounded-lg px-4 py-2 border border-gray-200">
            <!-- Avatar -->
            <div class="flex-shrink-0">
              <div
                class="w-10 h-10 rounded-full bg-gradient-to-br from-blue-500 to-blue-600 flex items-center justify-center text-white font-semibold text-sm">
                {{ userInitials }}
              </div>
            </div>

            <!-- Info del usuario -->
            <div class="hidden md:block text-left">
              <p class="text-sm font-semibold text-gray-900">{{ auth.user?.name || 'Usuario' }}</p>
              <p class="text-xs text-gray-500 mb-1">{{ auth.user?.email || 'usuario@email.com' }}</p>
              <div class="flex items-center gap-2 text-xs text-gray-600">
                <span class="px-2 py-0.5 bg-blue-100 text-blue-700 rounded-full font-medium">
                  {{ userRole }}
                </span>
                <span class="text-gray-400">•</span>
                <span>{{ companyName }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- <div class="flex items-center">
          <div class="flex items-center ms-3">
            <div>
              <svg
                class="w-7 h-7"
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
                    d="M8.35179 20.2418C9.19288 21.311 10.5142 22 12 22C13.4858 22 14.8071 21.311 15.6482 20.2418C13.2264 20.57 10.7736 20.57 8.35179 20.2418Z"
                    fill="#1C274C"
                  ></path>
                  <path
                    d="M18.7491 9V9.7041C18.7491 10.5491 18.9903 11.3752 19.4422 12.0782L20.5496 13.8012C21.5612 15.3749 20.789 17.5139 19.0296 18.0116C14.4273 19.3134 9.57274 19.3134 4.97036 18.0116C3.21105 17.5139 2.43882 15.3749 3.45036 13.8012L4.5578 12.0782C5.00972 11.3752 5.25087 10.5491 5.25087 9.7041V9C5.25087 5.13401 8.27256 2 12 2C15.7274 2 18.7491 5.13401 18.7491 9Z"
                    fill="#1C274C"
                  ></path>
                </g>
              </svg>
            </div>
            <div
              class="z-50 hidden my-4 text-base list-none bg-white divide-y divide-gray-100 rounded-sm shadow-sm   "
              id="dropdown-user"
            >
              <div class="px-4 py-3" role="none">
                <p class="text-sm text-gray-900   " role="none">
                  Neil Sims
                </p>
                <p
                  class="text-sm font-medium text-gray-900 truncate "
                  role="none"
                >
                  neil.sims@flowbite.com
                </p>
              </div>
              <ul class="py-1" role="none">
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Dashboard</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Settings</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Earnings</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Sign out</a
                  >
                </li>
              </ul>
            </div>
          </div>

          <div class="flex items-center ms-3">
            <div>
              <button
                type="button"
                class="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 "
                aria-expanded="false"
                data-dropdown-toggle="dropdown-user"
              >
                <span class="sr-only">Open user menu</span>
                <img
                  class="w-8 h-8 rounded-full"
                  src="https://flowbite.com/docs/images/people/profile-picture-5.jpg"
                  alt="user photo"
                />
              </button>
            </div>
            <div
              class="z-50 hidden my-4 text-base list-none bg-white divide-y divide-gray-100 rounded-sm shadow-sm   "
              id="dropdown-user"
            >
              <div class="px-4 py-3" role="none">
                <p class="text-sm text-gray-900   " role="none">
                  Neil Sims
                </p>
                <p
                  class="text-sm font-medium text-gray-900 truncate "
                  role="none"
                >
                  neil.sims@flowbite.com
                </p>
              </div>
              <ul class="py-1" role="none">
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Dashboard</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Settings</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Earnings</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100     "
                    role="menuitem"
                    >Sign out</a
                  >
                </li>
              </ul>
            </div>
          </div>
        </div> -->
      </div>
    </div>
  </nav>

  <!-- Sidebar -->
  <aside id="logo-sidebar"
    class="fixed top-0 left-0 z-40 h-screen pt-28 transition-all duration-300 bg-white border-r border-gray-200     "
    :class="{
      'w-64': sidebarOpen || isMobile,
      'w-20': !sidebarOpen && !isMobile,
      '-translate-x-full': !sidebarOpen && isMobile,
    }" :aria-hidden="!sidebarOpen && isMobile ? 'true' : 'false'" aria-label="Sidebar">
    <div class="h-full px-3 pb-4 overflow-y-auto bg-white  ">
      <ul class="space-y-2 font-medium">
        <!-- Items del menú-->
        <li>
          <NuxtLink to="/admin/" class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-5 h-5 text-gray-500 transition duration-75   group-hover:text-gray-900   " aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg" fill="currentColor" viewBox="0 0 22 21">
              <path
                d="M16.975 11H10V4.025a1 1 0 0 0-1.066-.998 8.5 8.5 0 1 0 9.039 9.039.999.999 0 0 0-1-1.066h.002Z" />
              <path
                d="M12.5 0c-.157 0-.311.01-.565.027A1 1 0 0 0 11 1.02V10h8.975a1 1 0 0 0 1-.935c.013-.188.028-.374.028-.565A8.51 8.51 0 0 0 12.5 0Z" />
            </svg>
            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Dashboard</span>
          </NuxtLink>
        </li>
        <li>
          <NuxtLink to="/admin/pipeline"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-5 h-5 text-gray-500 transition duration-75   group-hover:text-gray-900   " fill="currentColor"
              viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path d="M4 6h16M4 10h16M4 14h16M4 18h16" stroke="currentColor" stroke-width="2" stroke-linecap="round" />
              <rect x="2" y="5" width="3" height="3" rx="1" />
              <rect x="2" y="9" width="3" height="3" rx="1" />
              <rect x="2" y="13" width="3" height="3" rx="1" />
              <rect x="2" y="17" width="3" height="3" rx="1" />
            </svg>
            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Pipeline</span>
          </NuxtLink>
        </li>
        <li>
          <NuxtLink to="/admin/clientes"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-5 h-5 text-gray-500 transition duration-75   group-hover:text-gray-900   " viewBox="0 0 16 16"
              fill="none" xmlns="http://www.w3.org/2000/svg">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <path
                  d="M8 7C9.65685 7 11 5.65685 11 4C11 2.34315 9.65685 1 8 1C6.34315 1 5 2.34315 5 4C5 5.65685 6.34315 7 8 7Z"
                  fill="currentColor  "></path>
                <path d="M14 12C14 10.3431 12.6569 9 11 9H5C3.34315 9 2 10.3431 2 12V15H14V12Z" fill="currentColor">
                </path>
              </g>
            </svg>
            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Clientes</span>
          </NuxtLink>
        </li>

        <li>
          <NuxtLink to="/admin/propiedades"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75   group-hover:text-gray-900   " viewBox="0 0 24 24"
              fill="currentColor" xmlns="http://www.w3.org/2000/svg">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <path
                  d="M11.3196 3H5.08961C3.09961 3 2.09961 4.01 2.09961 6.02V22H7.49961V18.25C7.49961 17.84 7.83961 17.5 8.24961 17.5C8.65961 17.5 8.99961 17.83 8.99961 18.25V22H14.2996V6.02C14.2996 4.01 13.3096 3 11.3196 3ZM10.7496 12.75H5.79961C5.38961 12.75 5.04961 12.41 5.04961 12C5.04961 11.59 5.38961 11.25 5.79961 11.25H10.7496C11.1596 11.25 11.4996 11.59 11.4996 12C11.4996 12.41 11.1596 12.75 10.7496 12.75ZM10.7496 9H5.79961C5.38961 9 5.04961 8.66 5.04961 8.25C5.04961 7.84 5.38961 7.5 5.79961 7.5H10.7496C11.1596 7.5 11.4996 7.84 11.4996 8.25C11.4996 8.66 11.1596 9 10.7496 9Z"
                  fill="currentColor"></path>
                <path
                  d="M23 21.2511H20.73V18.2511C21.68 17.9411 22.37 17.0511 22.37 16.0011V14.0011C22.37 12.6911 21.3 11.6211 19.99 11.6211C18.68 11.6211 17.61 12.6911 17.61 14.0011V16.0011C17.61 17.0411 18.29 17.9211 19.22 18.2411V21.2511H1C0.59 21.2511 0.25 21.5911 0.25 22.0011C0.25 22.4111 0.59 22.7511 1 22.7511H19.93C19.95 22.7511 19.96 22.7611 19.98 22.7611C20 22.7611 20.01 22.7511 20.03 22.7511H23C23.41 22.7511 23.75 22.4111 23.75 22.0011C23.75 21.5911 23.41 21.2511 23 21.2511Z"
                  fill="currentColor"></path>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Propiedades</span>
          </NuxtLink>
        </li>

        <li>
          <NuxtLink to="/admin/proyectos"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75   group-hover:text-gray-900   " viewBox="0 0 24 24"
              fill="currentColor" xmlns="http://www.w3.org/2000/svg">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <path
                  d="M11.3196 3H5.08961C3.09961 3 2.09961 4.01 2.09961 6.02V22H7.49961V18.25C7.49961 17.84 7.83961 17.5 8.24961 17.5C8.65961 17.5 8.99961 17.83 8.99961 18.25V22H14.2996V6.02C14.2996 4.01 13.3096 3 11.3196 3ZM10.7496 12.75H5.79961C5.38961 12.75 5.04961 12.41 5.04961 12C5.04961 11.59 5.38961 11.25 5.79961 11.25H10.7496C11.1596 11.25 11.4996 11.59 11.4996 12C11.4996 12.41 11.1596 12.75 10.7496 12.75ZM10.7496 9H5.79961C5.38961 9 5.04961 8.66 5.04961 8.25C5.04961 7.84 5.38961 7.5 5.79961 7.5H10.7496C11.1596 7.5 11.4996 7.84 11.4996 8.25C11.4996 8.66 11.1596 9 10.7496 9Z"
                  fill="currentColor"></path>
                <path
                  d="M23 21.2511H20.73V18.2511C21.68 17.9411 22.37 17.0511 22.37 16.0011V14.0011C22.37 12.6911 21.3 11.6211 19.99 11.6211C18.68 11.6211 17.61 12.6911 17.61 14.0011V16.0011C17.61 17.0411 18.29 17.9211 19.22 18.2411V21.2511H1C0.59 21.2511 0.25 21.5911 0.25 22.0011C0.25 22.4111 0.59 22.7511 1 22.7511H19.93C19.95 22.7511 19.96 22.7611 19.98 22.7611C20 22.7611 20.01 22.7511 20.03 22.7511H23C23.41 22.7511 23.75 22.4111 23.75 22.0011C23.75 21.5911 23.41 21.2511 23 21.2511Z"
                  fill="currentColor"></path>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Proyectos</span>
          </NuxtLink>
        </li>

        <li>
          <NuxtLink to="/admin/match"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75   group-hover:text-gray-900   " viewBox="0 0 24 24"
              version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
              fill="currentColor">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <title>paper_fill</title>
                <g id="页面-1" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                  <g id="File" transform="translate(-816.000000, -48.000000)" fill-rule="nonzero">
                    <g id="paper_fill" transform="translate(816.000000, 48.000000)">
                      <path
                        d="M24,0 L24,24 L0,24 L0,0 L24,0 Z M12.5934901,23.257841 L12.5819402,23.2595131 L12.5108777,23.2950439 L12.4918791,23.2987469 L12.4918791,23.2987469 L12.4767152,23.2950439 L12.4056548,23.2595131 C12.3958229,23.2563662 12.3870493,23.2590235 12.3821421,23.2649074 L12.3780323,23.275831 L12.360941,23.7031097 L12.3658947,23.7234994 L12.3769048,23.7357139 L12.4804777,23.8096931 L12.4953491,23.8136134 L12.4953491,23.8136134 L12.5071152,23.8096931 L12.6106902,23.7357139 L12.6232938,23.7196733 L12.6232938,23.7196733 L12.6266527,23.7031097 L12.609561,23.275831 C12.6075724,23.2657013 12.6010112,23.2592993 12.5934901,23.257841 L12.5934901,23.257841 Z M12.8583906,23.1452862 L12.8445485,23.1473072 L12.6598443,23.2396597 L12.6498822,23.2499052 L12.6498822,23.2499052 L12.6471943,23.2611114 L12.6650943,23.6906389 L12.6699349,23.7034178 L12.6699349,23.7034178 L12.678386,23.7104931 L12.8793402,23.8032389 C12.8914285,23.8068999 12.9022333,23.8029875 12.9078286,23.7952264 L12.9118235,23.7811639 L12.8776777,23.1665331 C12.8752882,23.1545897 12.8674102,23.1470016 12.8583906,23.1452862 L12.8583906,23.1452862 Z M12.1430473,23.1473072 C12.1332178,23.1423925 12.1221763,23.1452606 12.1156365,23.1525954 L12.1099173,23.1665331 L12.0757714,23.7811639 C12.0751323,23.7926639 12.0828099,23.8018602 12.0926481,23.8045676 L12.108256,23.8032389 L12.3092106,23.7104931 L12.3186497,23.7024347 L12.3186497,23.7024347 L12.3225043,23.6906389 L12.340401,23.2611114 L12.337245,23.2485176 L12.337245,23.2485176 L12.3277531,23.2396597 L12.1430473,23.1473072 Z"
                        id="MingCute" fill-rule="nonzero"></path>
                      <path
                        d="M16,3 C17.597725,3 18.903664,4.24892392 18.9949075,5.82372764 L19,6 L19,16 L19.75,16 C20.39725,16 20.9295391,16.4918359 20.9935469,17.1221875 L21,17.25 L21,19 C21,20.597725 19.7511226,21.903664 18.1762773,21.9949075 L18,22 L8,22 C6.40232321,22 5.09633941,20.7511226 5.00509271,19.1762773 L5,19 L5,9 L3.25,9 C2.6027875,9 2.07046563,8.50812891 2.00645356,7.87780591 L2,7.75 L2,6 C2,4.40232321 3.24892392,3.09633941 4.82372764,3.00509271 L5,3 L16,3 Z M19,18 L10.0001,18 L10.0001,19 C10.0001,19.3506 9.9399,19.6872 9.82935,20 L18,20 C18.5523,20 19,19.5523 19,19 L19,18 Z M12,12 L10,12 C9.44772,12 9,12.4477 9,13 C9,13.51285 9.38604429,13.9355092 9.88337975,13.9932725 L10,14 L12,14 C12.5523,14 13,13.5523 13,13 C13,12.48715 12.613973,12.0644908 12.1166239,12.0067275 L12,12 Z M14,8 L10,8 C9.44772,8 9,8.44772 9,9 C9,9.55228 9.44772,10 10,10 L14,10 C14.5523,10 15,9.55228 15,9 C15,8.44772 14.5523,8 14,8 Z M5,5 C4.44772,5 4,5.44772 4,6 L4,7 L5,7 L5,5 Z"
                        id="形状" fill="currentColor"></path>
                    </g>
                  </g>
                </g>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Match</span>
          </NuxtLink>
        </li>

        <li>
          <NuxtLink to="/admin/recordatorios"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75   group-hover:text-gray-900   "
              viewBox="-1 0 32 32" version="1.1" xmlns="http://www.w3.org/2000/svg"
              xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:sketch="http://www.bohemiancoding.com/sketch/ns"
              fill="currentColor">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <title>calendar</title>
                <desc>Created with Sketch Beta.</desc>
                <defs></defs>
                <g id="Page-1" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd" sketch:type="MSPage">
                  <g id="Icon-Set-Filled" sketch:type="MSLayerGroup" transform="translate(-311.000000, -361.000000)"
                    fill="currentColor">
                    <path
                      d="M325,379 L327,379 L327,377 L325,377 L325,379 Z M325,387 L327,387 L327,385 L325,385 L325,387 Z M333,379 L335,379 L335,377 L333,377 L333,379 Z M333,387 L335,387 L335,385 L333,385 L333,387 Z M317,387 L319,387 L319,385 L317,385 L317,387 Z M339,369 L313,369 L313,367 C313,365.896 313.896,365 315,365 L319,365 L319,366 C319,366.553 319.447,367 320,367 C320.553,367 321,366.553 321,366 L321,365 L331,365 L331,366 C331,366.553 331.447,367 332,367 C332.553,367 333,366.553 333,366 L333,365 L337,365 C338.104,365 339,365.896 339,367 L339,369 L339,369 Z M337,379 C337,380.104 336.104,381 335,381 L333,381 C331.896,381 331,380.104 331,379 L331,377 C331,375.896 331.896,375 333,375 L335,375 C336.104,375 337,375.896 337,377 L337,379 L337,379 Z M337,387 C337,388.104 336.104,389 335,389 L333,389 C331.896,389 331,388.104 331,387 L331,385 C331,383.896 331.896,383 333,383 L335,383 C336.104,383 337,383.896 337,385 L337,387 L337,387 Z M329,379 C329,380.104 328.104,381 327,381 L325,381 C323.896,381 323,380.104 323,379 L323,377 C323,375.896 323.896,375 325,375 L327,375 C328.104,375 329,375.896 329,377 L329,379 L329,379 Z M329,387 C329,388.104 328.104,389 327,389 L325,389 C323.896,389 323,388.104 323,387 L323,385 C323,383.896 323.896,383 325,383 L327,383 C328.104,383 329,383.896 329,385 L329,387 L329,387 Z M321,379 C321,380.104 320.104,381 319,381 L317,381 C315.896,381 315,380.104 315,379 L315,377 C315,375.896 315.896,375 317,375 L319,375 C320.104,375 321,375.896 321,377 L321,379 L321,379 Z M321,387 C321,388.104 320.104,389 319,389 L317,389 C315.896,389 315,388.104 315,387 L315,385 C315,383.896 315.896,383 317,383 L319,383 C320.104,383 321,383.896 321,385 L321,387 L321,387 Z M337,363 L333,363 L333,362 C333,361.448 332.553,361 332,361 C331.447,361 331,361.448 331,362 L331,363 L321,363 L321,362 C321,361.448 320.553,361 320,361 C319.447,361 319,361.448 319,362 L319,363 L315,363 C312.791,363 311,364.791 311,367 L311,389 C311,391.209 312.791,393 315,393 L337,393 C339.209,393 341,391.209 341,389 L341,367 C341,364.791 339.209,363 337,363 L337,363 Z M317,379 L319,379 L319,377 L317,377 L317,379 Z"
                      id="calendar" sketch:type="MSShapeGroup"></path>
                  </g>
                </g>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Recordatorios</span>
          </NuxtLink>
        </li>

        <li v-if="auth.hasRole('admin')">
          <NuxtLink to="/admin/agentes/"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75   group-hover:text-gray-900   " viewBox="0 0 24 24"
              fill="none" xmlns="http://www.w3.org/2000/svg">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <circle cx="12" cy="6" r="4" stroke="currentcolor" stroke-width="1.5"></circle>
                <path
                  d="M19.9975 18C20 17.8358 20 17.669 20 17.5C20 15.0147 16.4183 13 12 13C7.58172 13 4 15.0147 4 17.5C4 19.9853 4 22 12 22C14.231 22 15.8398 21.8433 17 21.5634"
                  stroke="currentcolor" stroke-width="1.5" stroke-linecap="round"></path>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Agentes</span>
          </NuxtLink>
        </li>

        <li v-if="auth.hasRole('admin')">
          <NuxtLink to="/admin/zonas/"
            class="flex items-center p-2 text-gray-900 rounded-lg    hover:bg-gray-100    group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75 group-hover:text-gray-900" fill="currentColor"  version="1.1" id="Capa_1"
              xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
              viewBox="0 0 321.883 321.883" xml:space="preserve">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <path
                  d="M160.941,0c-69.035,0-125,55.964-125,125.001c0,69.035,85.187,196.882,125,196.882c39.813,0,125-127.847,125-196.882 C285.941,55.964,229.977,0,160.941,0z M160.941,182.294c-36.341,0-65.801-29.46-65.801-65.802c0-36.34,29.46-65.801,65.801-65.801 c36.341,0,65.801,29.461,65.801,65.801C226.742,152.834,197.282,182.294,160.941,182.294z">
                </path>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Zonas</span>
          </NuxtLink>
        </li>

        <li>
          <button type="button" @click="onLogout"
            class="w-full flex items-center p-2 text-gray-900 rounded-lg hover:bg-gray-100 group">
            <svg class="w-6 h-6 text-gray-500 transition duration-75 group-hover:text-gray-900" viewBox="0 0 24 24"
              fill="none" xmlns="http://www.w3.org/2000/svg">
              <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
              <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
              <g id="SVGRepo_iconCarrier">
                <path
                  d="M14.9453 1.25C13.5778 1.24998 12.4754 1.24996 11.6085 1.36652C10.7084 1.48754 9.95048 1.74643 9.34857 2.34835C8.82363 2.87328 8.55839 3.51836 8.41916 4.27635C8.28387 5.01291 8.25799 5.9143 8.25196 6.99583C8.24966 7.41003 8.58357 7.74768 8.99778 7.74999C9.41199 7.7523 9.74964 7.41838 9.75194 7.00418C9.75803 5.91068 9.78643 5.1356 9.89448 4.54735C9.99859 3.98054 10.1658 3.65246 10.4092 3.40901C10.686 3.13225 11.0746 2.9518 11.8083 2.85315C12.5637 2.75159 13.5648 2.75 15.0002 2.75H16.0002C17.4356 2.75 18.4367 2.75159 19.1921 2.85315C19.9259 2.9518 20.3144 3.13225 20.5912 3.40901C20.868 3.68577 21.0484 4.07435 21.1471 4.80812C21.2486 5.56347 21.2502 6.56459 21.2502 8V16C21.2502 17.4354 21.2486 18.4365 21.1471 19.1919C21.0484 19.9257 20.868 20.3142 20.5912 20.591C20.3144 20.8678 19.9259 21.0482 19.1921 21.1469C18.4367 21.2484 17.4356 21.25 16.0002 21.25H15.0002C13.5648 21.25 12.5637 21.2484 11.8083 21.1469C11.0746 21.0482 10.686 20.8678 10.4092 20.591C10.1658 20.3475 9.99859 20.0195 9.89448 19.4527C9.78643 18.8644 9.75803 18.0893 9.75194 16.9958C9.74964 16.5816 9.41199 16.2477 8.99778 16.25C8.58357 16.2523 8.24966 16.59 8.25196 17.0042C8.25799 18.0857 8.28387 18.9871 8.41916 19.7236C8.55839 20.4816 8.82363 21.1267 9.34857 21.6517C9.95048 22.2536 10.7084 22.5125 11.6085 22.6335C12.4754 22.75 13.5778 22.75 14.9453 22.75H16.0551C17.4227 22.75 18.525 22.75 19.392 22.6335C20.2921 22.5125 21.0499 22.2536 21.6519 21.6517C22.2538 21.0497 22.5127 20.2919 22.6337 19.3918C22.7503 18.5248 22.7502 17.4225 22.7502 16.0549V7.94513C22.7502 6.57754 22.7503 5.47522 22.6337 4.60825C22.5127 3.70814 22.2538 2.95027 21.6519 2.34835C21.0499 1.74643 20.2921 1.48754 19.392 1.36652C18.525 1.24996 17.4227 1.24998 16.0551 1.25H14.9453Z"
                  fill="currentcolor"></path>
                <path
                  d="M15 11.25C15.4142 11.25 15.75 11.5858 15.75 12C15.75 12.4142 15.4142 12.75 15 12.75H4.02744L5.98809 14.4306C6.30259 14.7001 6.33901 15.1736 6.06944 15.4881C5.79988 15.8026 5.3264 15.839 5.01191 15.5694L1.51191 12.5694C1.34567 12.427 1.25 12.2189 1.25 12C1.25 11.7811 1.34567 11.573 1.51191 11.4306L5.01191 8.43056C5.3264 8.16099 5.79988 8.19741 6.06944 8.51191C6.33901 8.8264 6.30259 9.29988 5.98809 9.56944L4.02744 11.25H15Z"
                  fill="currentcolor"></path>
              </g>
            </svg>

            <span class="ms-3 whitespace-nowrap" :class="{ 'sr-only': !sidebarOpen && !isMobile }">Salir</span>
          </button>
        </li>
      </ul>
    </div>
  </aside>

  <!-- Contenido principal ajustable -->
  <div class="p-4 transition-all duration-300 mt-14" :class="{
    'sm:ml-64': sidebarOpen || isMobile,
    'sm:ml-20': !sidebarOpen && !isMobile,
  }">
    <div class="p-4   ">
      <!-- Tu contenido aquí -->
      <div>
        <NuxtPage></NuxtPage>
      </div>
    </div>
  </div>
</template>
<script setup>
import { onMounted, ref, onBeforeUnmount, computed } from "vue";
import { initFlowbite } from "flowbite";
import { useAuthStore } from "@/stores/auth";

const sidebarOpen = ref(true);
const isMobile = ref(false);
const auth = useAuthStore();

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
};

const onLogout = async () => {
  try {
    await auth.logout();
  } finally {
    await navigateTo("/");
  }
};

// Computed properties para la info del usuario
const userInitials = computed(() => {
  const name = auth.user?.name || 'U';
  const parts = name.trim().split(' ');
  if (parts.length >= 2) {
    return (parts[0][0] + parts[1][0]).toUpperCase();
  }
  return name.substring(0, 2).toUpperCase();
});

const userRole = computed(() => {
  const roles = auth.user?.roles || [];
  if (roles.includes('admin')) return 'Administrador';
  if (roles.includes('agente')) return 'Agente';
  if (roles.includes('gerente')) return 'Gerente';
  return 'Usuario';
});

const companyName = computed(() => {
  return 'Homes Guatemala';
});

const checkScreenSize = () => {
  isMobile.value = window.innerWidth < 640; // sm breakpoint de Tailwind
  // En móviles, el sidebar siempre empieza cerrado
  if (isMobile.value) {
    sidebarOpen.value = false;
  } else {
    // En desktop, puedes decidir si quieres que empiece abierto o cerrado
    sidebarOpen.value = true;
  }
};

onMounted(() => {
  initFlowbite();
  checkScreenSize();
  window.addEventListener("resize", checkScreenSize);
});

onBeforeUnmount(() => {
  window.removeEventListener("resize", checkScreenSize);
});
</script>
