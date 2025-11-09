// https://nuxt.com/docs/api/configuration/nuxt-config
import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: false },
  ssr: false,
  generate: { fallback: true },
  target: 'static',
  
  // Variables de entorno disponibles en runtime
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.VITE_API_BASE_URL || process.env.NUXT_PUBLIC_API_BASE_URL || 'https://localhost:7234'
    }
  },
  
  app: {
    // baseURL: '/homes/crm',
    
    head: {
      link: [
        { rel: 'icon', type: 'image/x-icon', href: 'https://app-pool.vylaris.online/dcmigserver/homes/5369ffc1-5e81-4be1-a01e-617c564b7eed.webp' }
      ],
 
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' }
      ],
      
      script: [
        {
          children: `window.$config = { apiBaseUrl: '${process.env.VITE_API_BASE_URL || process.env.NUXT_PUBLIC_API_BASE_URL || 'https://localhost:7234'}' };`
        }
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
    },
    build: {
      sourcemap: false, // Deshabilitar source maps para mejor rendimiento
      minify: 'esbuild', // Usar esbuild para minificación más rápida
      chunkSizeWarningLimit: 1000
    },
    server: {
      hmr: {
        overlay: false // Deshabilitar overlay de errores para mejor rendimiento
      },
      watch: {
        usePolling: false // Mejorar rendimiento del file watcher
      }
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