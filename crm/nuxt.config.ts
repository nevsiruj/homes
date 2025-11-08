// https://nuxt.com/docs/api/configuration/nuxt-config
import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: false },
  ssr: false,
  generate: { fallback: true },
  target: 'static',  
  app: {
    // baseURL: '/homes/crm',
    
    head: {
      link: [
        { rel: 'icon', type: 'image/x-icon', href: 'https://app-pool.vylaris.online/dcmigserver/homes/5369ffc1-5e81-4be1-a01e-617c564b7eed.webp' }
      ],
 
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' }
      ]
    }    
  }, 

  modules: [

    // Módulo para gestionar el modo de color
    '@nuxtjs/color-mode',
    '@pinia/nuxt'
  ],

  colorMode: {
    classSuffix: '',
    preference: 'light',
    fallback: 'light'
  } as any,

  css: ['~/assets/css/main.css'],

  

  vite: {
    plugins: [tailwindcss()],
    optimizeDeps: {
      include: ['flowbite']
    }
  },

  build: {
    transpile: ['flowbite']
  },

  nitro: {
    publicAssets: [
      {
        dir: 'public/images'
      }
    ]
  }
})