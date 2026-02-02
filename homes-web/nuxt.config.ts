// nuxt.config.ts
import { defineNuxtConfig } from "nuxt/config";

export default defineNuxtConfig({
  compatibilityDate: "2025-06-21",
  devtools: { enabled: false },
  ssr: true,

  app: {
    head: {
      htmlAttrs: {
        lang: 'es'
      },
      title: 'Venta y Renta de Propiedades | Homes Guatemala',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { name: 'description', content: 'Casas y apartamentos de lujo en venta y renta en Guatemala. Propiedades exclusivas en Zona 10, 14, 15, 16. Asesoría personalizada premium.' },
        { name: "p:domain_verify", content: "4ff93a868a573e1b24e15558f505f316" },
        { name: "facebook-domain-verification", content: "s7lsdi9b3qj53qjggc8lgumeiythqm" },
        { name: "google-site-verification", content: "0021L2ztWBVMwBLnR8VPXwfs8JspGBFnR3IhyqScj-c" },
        { property: "fb:app_id", content: "239174403519612" },
        { property: "og:type", content: "website" },
        { property: "og:locale", content: "es_GT" },
        { name: 'robots', content: 'index, follow' }
      ],
      link: [
        { rel: "icon", type: "image/x-icon", href: "https://app-pool.vylaris.online/dcmigserver/homes/0ecfe259-77d7-450f-afb3-4ec21231dc6f.webp" },
        { rel: "apple-touch-icon", href: "https://app-pool.vylaris.online/dcmigserver/homes/0ecfe259-77d7-450f-afb3-4ec21231dc6f.webp" },
        { rel: "canonical", href: "https://homesguatemala.com" },

        // ===================================================================
        // OPTIMIZACIÓN: Preconnect para recursos externos
        // ===================================================================
        { rel: "preconnect", href: "https://app-pool.vylaris.online" },
        { rel: "dns-prefetch", href: "https://app-pool.vylaris.online" },

        // ===================================================================
        // OPTIMIZACIÓN: Google Fonts - Non-blocking
        // Preconnect acelera la conexión, preload prioriza fuentes críticas
        // ===================================================================
        { rel: "preconnect", href: "https://fonts.googleapis.com" },
        { rel: "preconnect", href: "https://fonts.gstatic.com", crossorigin: "" },

        // Preload de fuentes críticas (Raleway 300 y 400 son las más usadas)
        {
          rel: "preload",
          as: "style",
          href: "https://fonts.googleapis.com/css2?family=Raleway:wght@300;400&family=Roboto+Condensed:wght@300;400&display=swap"
        },

        // Carga asíncrona de fuentes (no bloquea render)
        // Técnica: media="print" + onload para cargar sin bloquear
        {
          rel: "stylesheet",
          href: "https://fonts.googleapis.com/css2?family=Raleway:wght@300;400&family=Roboto+Condensed:wght@300;400&display=swap",
          media: "print",
          onload: "this.media='all'"
        },

        // Hreflang para SEO internacional
        { rel: "alternate", hreflang: "es-GT", href: "https://homesguatemala.com" },
        { rel: "alternate", hreflang: "x-default", href: "https://homesguatemala.com" },

        // Preload CSS crítico para mejorar rendimiento
        { rel: "preload", href: "/assets/css/main.css", as: "style" }
      ],
      script: [
        // ===================================================================
        // Schema.org - SEO Structured Data (no bloquea render)
        // ===================================================================
        {
          key: "schema-org",
          type: "application/ld+json",
          innerHTML: JSON.stringify({
            "@context": "https://schema.org",
            "@type": "RealEstateAgent",
            "name": "Homes Guatemala",
            "image": "https://app-pool.vylaris.online/dcmigserver/homes/5369ffc1-5e81-4be1-a01e-617c564b7eed.webp",
            "@id": "https://homesguatemala.com",
            "url": "https://homesguatemala.com",
            "telephone": "+502-5633-0961",
            "address": {
              "@type": "PostalAddress",
              "streetAddress": "Design Center Local 217, Zona 10",
              "addressLocality": "Guatemala City",
              "postalCode": "01010",
              "addressCountry": "GT"
            },
            "sameAs": [
              "https://www.facebook.com/homesguatemala",
              "https://www.instagram.com/homesguatemala",
              "https://www.youtube.com/@homesguatemala4975",
              "https://www.linkedin.com/company/homes-guatemala",
              "https://twitter.com/homesguatemala"
            ]
          })
        },
        // ===================================================================
        // OPTIMIZACIÓN: Facebook Pixel - Async + Defer
        // Carga después del contenido principal para no bloquear render
        // ===================================================================
        {
          key: "facebook-pixel-script",
          src: "https://connect.facebook.net/en_US/fbevents.js",
          async: true,
          defer: true,
        },
        {
          key: "facebook-pixel-init",
          innerHTML: `
            // Inicializar Facebook Pixel después de que la página cargue
            window.addEventListener('load', function() {
              if (typeof fbq !== 'undefined') {
                fbq('init', '239174403519612');
                fbq('track', 'PageView');
              }
            });
          `,
        }
      ],
    }
  },

  modules: [
    "@nuxt/image-edge",
    // "@nuxtjs/seo",
    '@nuxtjs/sitemap',
    "@nuxt/content",
    "@pinia/nuxt",
    "@nuxtjs/color-mode"
  ],

  // Sitemap configuration
  site: {
    url: 'https://homesguatemala.com',
    name: 'Homes Guatemala',
    description: 'Bienes Raíces de Lujo en Guatemala',
    defaultLocale: 'es',
    indexable: true,
  },

  // Eliminado: configuración de sitemap para evitar conflictos. El sitemap se genera dinámicamente vía endpoint personalizado.
  
  // robots: {
  //   disallow: ['/admin'],
  //   allow: ['/'],
  //   sitemap: ['https://homesguatemala.com/sitemap.xml'],
  // },

  colorMode: {
    classSuffix: "",
    preference: "light",
    fallback: "light",
    storageKey: "nuxt-color-mode",
  },

  css: ["~/assets/css/main.css"],

  build: {
    transpile: ["flowbite", "swiper"],
  },

  vite: {
    plugins: [
      (await import("@tailwindcss/vite")).default(),
    ],
    optimizeDeps: {
      include: ["swiper"],
    },
    css: {
      preprocessorOptions: {
        scss: {
          additionalData: '@import "@/assets/css/fonts.css";',
        },
      },
    },
    build: {
      // Consolidar CSS en menos archivos
      cssCodeSplit: false, // Genera un solo archivo CSS en lugar de múltiples
      target: 'esnext',
      minify: 'terser', // Terser suele comprimir mejor que esbuild para JS complejo
      terserOptions: {
        compress: {
          drop_console: true, // Eliminar console.log en producción
          drop_debugger: true,
          pure_funcs: ['console.log', 'console.info', 'console.debug']
        }
      },
      rollupOptions: {
        output: {
          // Agrupar chunks de manera más eficiente
          manualChunks: {
            'vendor': ['vue', 'vue-router'],
            'swiper': ['swiper'],
          },
          // Optimizar nombres de chunks para mejor caching
          chunkFileNames: '_nuxt/[name]-[hash].js',
          entryFileNames: '_nuxt/[name]-[hash].js',
          assetFileNames: '_nuxt/[name]-[hash].[ext]'
        },
      },
      // Inline CSS pequeños (menos de 4kb)
      assetsInlineLimit: 4096,
      // Minificar CSS
      cssMinify: true,
      // Reportar tamaño de chunks comprimidos
      reportCompressedSize: true,
      // Chunk size warnings
      chunkSizeWarningLimit: 1000,
    },
  },

  experimental: {
    payloadExtraction: true, // Mejora la carga de datos en navegación SPA
    renderJsonPayloads: true,
    viewTransition: false, // Desactivar si no se usa
  },

  image: {
    domains: ['app-pool.vylaris.online', 'homesguatemala.com', 'via.placeholder.com', 'vylaris.ar'],
    provider: 'ipx',
    quality: 80,
    format: ['webp', 'avif', 'jpg'],
    screens: {
      xs: 320,
      sm: 640,
      md: 768,
      lg: 1024,
      xl: 1280,
      xxl: 1536,
    },
    presets: {
      card: {
        modifiers: {
          format: 'webp',
          quality: 80,
          width: 400,
          height: 256
        }
      },
      thumbnail: {
        modifiers: {
          format: 'webp',
          quality: 75,
          width: 150,
          height: 150
        }
      },
      hero: {
        modifiers: {
          format: 'webp',
          quality: 85,
          width: 1200,
          height: 600
        }
      }
    }
  },


  nitro: {
    preset: 'netlify',
    compressPublicAssets: {
      gzip: true,
      brotli: true,
    },
    minify: true,
    experimental: {
      asyncContext: false
    },
    prerender: {
      // Indicar qué rutas pre-renderizar y cuáles manejar como SSR
      crawlLinks: true,
      failOnError: false, // No fallar el build si alguna ruta da error
      routes: [
        '/',
        '/propiedades',
        '/propiedades/venta',
        '/propiedades/renta',
        '/nosotros',
        '/proyectos-inmobiliarios',
        '/blog-inmobiliario',
        '/luxury',
        '/busqueda',
        '/custom',
        '/propiedades/zona/zona-15',
        '/propiedades/zona/cayala'
      ],
      // Ignorar estas rutas que requieren SSR
      ignore: [
        '/inmueble/**',
        '/propiedades/zona/zona-10',
        '/propiedades/zona/zona-14',
        '/propiedades/zona/carretera-a-el-salvador',
        '/blog/**',
        '/proyecto/**',
        '/admin/**',
        '/auth/**'
      ]
    },
    routeRules: {
      // Proxy para luxury homes (mantener)
      '/luxury-homes/**': {
        proxy: 'https://old-web.homesguatemala.com/luxury-homes/**'
      },
      '/luxury-homes': {
        proxy: 'https://old-web.homesguatemala.com/luxury-homes'
      },

      // // Redirecciones para URLs antiguas de propiedades (301 para SEO)
      // '/propiedad/**': { redirect: { to: '/propiedades', statusCode: 301 } },
      // '/property/**': { redirect: { to: '/propiedades', statusCode: 301 } },
      // // '/inmueble/**': { redirect: '/propiedades' }, // REMOVIDO: Esta regla impedía ver detalles de inmuebles
      // '/listing/**': { redirect: { to: '/propiedades', statusCode: 301 } },

      // // Redirecciones de URLs comunes mal escritas (301 para SEO)
      // // '/home': { redirect: { to: '/', statusCode: 301 } },
      // '/inicio': { redirect: { to: '/', statusCode: 301 } },
      // '/index': { redirect: { to: '/', statusCode: 301 } },
      // '/index.html': { redirect: { to: '/', statusCode: 301 } },
      // '/index.php': { redirect: { to: '/', statusCode: 301 } },

      // // Redirecciones de secciones antiguas (301 para SEO)
      // '/contacto': { redirect: { to: '/nosotros', statusCode: 301 } },
      // '/contact': { redirect: { to: '/nosotros', statusCode: 301 } },
      // '/about': { redirect: { to: '/nosotros', statusCode: 301 } },
      // '/acerca-de': { redirect: { to: '/nosotros', statusCode: 301 } },

      // // ===================================================================
      // // REDIRECCIONES LEGACY WORDPRESS - Fix 404 errors Google Search Console
      // // ===================================================================

      // // NOTA: Ya no redirigimos /informativo/**, /tops/**, /consejos/** 
      // // porque ahora usamos esas rutas para el blog con estructura [category]/[slug]

      // // Redirecciones de /propiedades-en-venta/ -> /propiedades?Operaciones=Venta
      // '/propiedades-en-venta/casas-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Casa', statusCode: 301 } },
      // '/propiedades-en-venta/casas-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Casa', statusCode: 301 } },
      // '/propiedades-en-venta/apartamentos-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Apartamento', statusCode: 301 } },
      // '/propiedades-en-venta/apartamentos-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Apartamento', statusCode: 301 } },
      // '/propiedades-en-venta/terrenos-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Terreno', statusCode: 301 } },
      // '/propiedades-en-venta/terrenos-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Terreno', statusCode: 301 } },
      // '/propiedades-en-venta/oficinas-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Oficina', statusCode: 301 } },
      // '/propiedades-en-venta/oficinas-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Oficina', statusCode: 301 } },
      // '/propiedades-en-venta/bodegas-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Bodega', statusCode: 301 } },
      // '/propiedades-en-venta/bodegas-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Bodega', statusCode: 301 } },
      // '/propiedades-en-venta/locales-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Local', statusCode: 301 } },
      // '/propiedades-en-venta/locales-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta&Tipos=Local', statusCode: 301 } },
      // '/propiedades-en-venta/ubicaciones': { redirect: { to: '/propiedades?Operaciones=Venta', statusCode: 301 } },
      // '/propiedades-en-venta/ubicaciones/**': { redirect: { to: '/propiedades?Operaciones=Venta', statusCode: 301 } },
      // '/propiedades-en-venta/**': { redirect: { to: '/propiedades?Operaciones=Venta', statusCode: 301 } },
      // '/propiedades-en-venta': { redirect: { to: '/propiedades?Operaciones=Venta', statusCode: 301 } },

      // // Redirecciones de /propiedades-en-renta/ -> /propiedades?Operaciones=Renta
      // '/propiedades-en-renta/casas-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Casa', statusCode: 301 } },
      // '/propiedades-en-renta/casas-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Casa', statusCode: 301 } },
      // '/propiedades-en-renta/apartamentos-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Apartamento', statusCode: 301 } },
      // '/propiedades-en-renta/apartamentos-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Apartamento', statusCode: 301 } },
      // '/propiedades-en-renta/terrenos-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Terreno', statusCode: 301 } },
      // '/propiedades-en-renta/terrenos-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Terreno', statusCode: 301 } },
      // '/propiedades-en-renta/oficinas-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Oficina', statusCode: 301 } },
      // '/propiedades-en-renta/oficinas-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Oficina', statusCode: 301 } },
      // '/propiedades-en-renta/bodegas-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Bodega', statusCode: 301 } },
      // '/propiedades-en-renta/bodegas-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Bodega', statusCode: 301 } },
      // '/propiedades-en-renta/locales-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Local', statusCode: 301 } },
      // '/propiedades-en-renta/locales-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Local', statusCode: 301 } },
      // '/propiedades-en-renta/ubicaciones': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },
      // '/propiedades-en-renta/ubicaciones/**': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },
      // '/propiedades-en-renta/**': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },
      // '/propiedades-en-renta': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },

      // // Redirecciones de /propiedades-en-alquiler/ -> /propiedades?Operaciones=Renta
      // '/propiedades-en-alquiler/casas-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Casa', statusCode: 301 } },
      // '/propiedades-en-alquiler/casas-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Casa', statusCode: 301 } },
      // '/propiedades-en-alquiler/apartamentos-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Apartamento', statusCode: 301 } },
      // '/propiedades-en-alquiler/apartamentos-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Apartamento', statusCode: 301 } },
      // '/propiedades-en-alquiler/terrenos-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Terreno', statusCode: 301 } },
      // '/propiedades-en-alquiler/terrenos-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Terreno', statusCode: 301 } },
      // '/propiedades-en-alquiler/oficinas-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Oficina', statusCode: 301 } },
      // '/propiedades-en-alquiler/oficinas-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Oficina', statusCode: 301 } },
      // '/propiedades-en-alquiler/bodegas-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Bodega', statusCode: 301 } },
      // '/propiedades-en-alquiler/bodegas-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Bodega', statusCode: 301 } },
      // '/propiedades-en-alquiler/locales-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Local', statusCode: 301 } },
      // '/propiedades-en-alquiler/locales-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta&Tipos=Local', statusCode: 301 } },
      // '/propiedades-en-alquiler/ubicaciones': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },
      // '/propiedades-en-alquiler/ubicaciones/**': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },
      // '/propiedades-en-alquiler/**': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },
      // '/propiedades-en-alquiler': { redirect: { to: '/propiedades?Operaciones=Renta', statusCode: 301 } },

      // // Otras URLs legacy comunes
      // '/category/**': { redirect: { to: '/blog-inmobiliario', statusCode: 301 } },
      // '/tag/**': { redirect: { to: '/blog-inmobiliario', statusCode: 301 } },
      // '/page/**': { redirect: { to: '/', statusCode: 301 } },
      // '/wp-content/**': { redirect: { to: '/', statusCode: 301 } },
      // '/wp-admin/**': { redirect: { to: '/', statusCode: 301 } },
      // '/wp-includes/**': { redirect: { to: '/', statusCode: 301 } },
      // '/feed': { redirect: { to: '/', statusCode: 301 } },
      // '/feed/**': { redirect: { to: '/', statusCode: 301 } },
      // '/author/**': { redirect: { to: '/nosotros', statusCode: 301 } },

      // Cache para assets estáticos (mejorar rendimiento)
      '/_nuxt/**': {
        headers: {
          'Cache-Control': 'public, max-age=31536000, immutable'
        }
      },
      '/images/**': {
        headers: {
          'Cache-Control': 'public, max-age=31536000, immutable'
        }
      },

      // Headers de seguridad y SEO para todas las páginas
      // '/**': {
      //   headers: {
      //     'X-Robots-Tag': 'index, follow',
      //     'X-Content-Type-Options': 'nosniff',
      //     'X-Frame-Options': 'SAMEORIGIN',
      //     'Referrer-Policy': 'strict-origin-when-cross-origin',
      //     'Permissions-Policy': 'geolocation=(), microphone=(), camera=()',
      //     'Strict-Transport-Security': 'max-age=31536000; includeSubDomains'
      //   }
      // }
    }
  }
});
