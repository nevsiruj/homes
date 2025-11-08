<template>
    <div class="max-w-[1080px] mx-auto p-4 bg-white mt-24 lg:mt-12 animate-pulse">
      <div class="mb-5 mt-5">
        <div class="h-10 bg-gray-300 rounded-md w-3/4 md:w-1/2"></div>
      </div>
  
      <div
        class="grid grid-cols-1 md:grid-cols-6 md:grid-rows-3 gap-8 h-auto md:h-[600px]"
      >
        <div class="md:col-span-5 md:row-span-3 relative">
          <div class="w-full h-96 md:h-[600px] bg-gray-300 rounded-md"></div>
        </div>
  
        <div class="md:col-start-6 p-2 md:p-0 relative flex flex-wrap md:flex-col gap-2">
          <div
            v-for="n in 6"
            :key="n"
            class="!h-24 !w-24 lg:!h-36 lg:!w-36 bg-gray-300 rounded-md"
          ></div>
        </div>
      </div>
  
      <div class="grid grid-cols-1 md:grid-cols-3 md:grid-rows-1 gap-4">
        <div class="order-first md:order-last md:col-span-1">
          <div class="mb-5 mt-8 md:mt-14 lg:mt-14 space-y-3">
            <div class="h-10 bg-gray-300 rounded-md w-2/3 mx-auto"></div>
            <div class="h-10 bg-gray-300 rounded-md w-full"></div>
            <hr class="w-48 mx-auto text-gray-300" />
            <div
              class="grid grid-cols-2 gap-2 justify-items-center text-sm md:text-base"
            >
              <div
                v-for="n in 4"
                :key="n"
                class="flex items-center justify-between h-6 bg-gray-300 rounded-md w-24"
              ></div>
            </div>
            <hr class="w-48 mx-auto text-gray-300" />
            <div class="mb-4 mt-4 flex flex-wrap justify-center gap-2">
              <div
                v-for="n in 6"
                :key="n"
                class="h-6 w-20 bg-gray-300 rounded-sm"
              ></div>
            </div>
            <hr class="w-48 mx-auto text-gray-300" />
            <div class="h-12 w-32 mx-auto bg-gray-300 rounded text-lg"></div>
          </div>
        </div>
  
        <div class="md:col-span-2 md:order-first">
          <div class="mb-5 mt-1 md:mt-14 lg:mt-14 space-y-3">
            <div class="h-8 bg-gray-300 rounded-md w-full"></div>
            <div class="h-4 bg-gray-300 rounded-md w-full"></div>
            <div class="h-4 bg-gray-300 rounded-md w-full"></div>
            <div class="h-4 bg-gray-300 rounded-md w-5/6"></div>
            <div class="h-4 bg-gray-300 rounded-md w-full"></div>
            <div class="h-4 bg-gray-300 rounded-md w-2/3"></div>
          </div>
        </div>
      </div>
  
      <div class="mt-12 mb-12">
        <div class="h-10 bg-gray-300 rounded-md w-1/2 mx-auto mb-8"></div>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          <div
            v-for="n in 3"
            :key="n"
            class="h-96 bg-gray-300 rounded-md w-full"
          ></div>
        </div>
      </div>
    </div>
  </template>
  
  <style scoped>
  /* No se requiere un bloque de script ni estilos adicionales */
  </style>