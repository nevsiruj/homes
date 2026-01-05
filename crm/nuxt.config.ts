// https://nuxt.com/docs/api/configuration/nuxt-config
import tailwindcss from "@tailwindcss/vite";

export default defineNuxtConfig({
  compatibilityDate: '2024-11-01',
  devtools: { enabled: false },
  ssr: false,

  // Deshabilitar app manifest para evitar errores 404 en producción
  app: {
    buildAssetsDir: '/_nuxt/',
  },

  experimental: {
    appManifest: false, // Desactiva los manifests que causan 404 en producción estática
    payloadExtraction: false // Desactiva la extracción de payloads en SPA
  },

  // Configuración específica para SPA
  generate: {
    fallback: '200.html', // Usar 200.html en lugar de 404.html para SPAs
    routes: ['/'] // Solo generar la ruta raíz
  },

  hooks: {
    // Desactivar generación de payloads completamente
    'nitro:build:before': (nitro) => {
      nitro.options.prerender = nitro.options.prerender || {}
      nitro.options.prerender.crawlLinks = false
    }
  },

  // Variables de entorno disponibles en runtime
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.VITE_API_BASE_URL || process.env.NUXT_PUBLIC_API_BASE_URL || 'https://app-pool.vylaris.online/homes/api'
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
        { src: '/config.js' } // Cargar configuración externa
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