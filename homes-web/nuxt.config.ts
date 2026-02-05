// nuxt.config.ts
import { defineNuxtConfig } from "nuxt/config";

export default defineNuxtConfig({
  compatibilityDate: "2025-06-21",
  devtools: { enabled: false },
  ssr: true,

  app: {
    head: {
      htmlAttrs: { lang: 'es' },
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
        { rel: "preconnect", href: "https://app-pool.vylaris.online" },
        { rel: "dns-prefetch", href: "https://app-pool.vylaris.online" },
        { rel: "preconnect", href: "https://fonts.googleapis.com" },
        { rel: "preconnect", href: "https://fonts.gstatic.com", crossorigin: "" },
        {
          rel: "preload",
          as: "style",
          href: "https://fonts.googleapis.com/css2?family=Raleway:wght@300;400&family=Roboto+Condensed:wght@300;400&display=swap"
        },
        {
          rel: "stylesheet",
          href: "https://fonts.googleapis.com/css2?family=Raleway:wght@300;400&family=Roboto+Condensed:wght@300;400&display=swap",
          media: "print",
          onload: "this.media='all'"
        },
        { rel: "alternate", hreflang: "es-GT", href: "https://homesguatemala.com" },
        { rel: "alternate", hreflang: "x-default", href: "https://homesguatemala.com" },
        { rel: "preload", href: "/assets/css/main.css", as: "style" }
      ],
      script: [
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
        {
          key: "facebook-pixel-script",
          src: "https://connect.facebook.net/en_US/fbevents.js",
          async: true,
          defer: true,
        },
        {
          key: "facebook-pixel-init",
          innerHTML: `
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
    '@nuxtjs/sitemap',
    "@nuxt/content",
    "@pinia/nuxt",
    "@nuxtjs/color-mode"
  ],

  site: {
    url: 'https://homesguatemala.com',
    name: 'Homes Guatemala',
    description: 'Bienes Raíces de Lujo en Guatemala',
    defaultLocale: 'es',
    indexable: true,
  },

  // Configuración de Sitemap corregida para incluir rutas dinámicas
  // sitemap: {
  //   dynamicUrlsApiEndpoint: '/__sitemap__/urls', // Opcional: si creas un server route
  //   exclude: ['/admin/**', '/auth/**'],
  // },

  sitemap: {
  sources: [
    '/api/_sitemap-urls',
    
  ],
  exclude: ['/admin/**', '/auth/**'],
},

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
    server: {
      allowedHosts: ["devserver-imendoza-fixes--homes-test.netlify.app"]
    },
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
      cssCodeSplit: false,
      target: 'esnext',
      minify: 'terser',
      terserOptions: {
        compress: {
          drop_console: true,
          drop_debugger: true,
        }
      },
      rollupOptions: {
        output: {
          manualChunks: {
            'vendor': ['vue', 'vue-router'],
            'swiper': ['swiper'],
          },
          chunkFileNames: '_nuxt/[name]-[hash].js',
          entryFileNames: '_nuxt/[name]-[hash].js',
          assetFileNames: '_nuxt/[name]-[hash].[ext]'
        },
      },
      assetsInlineLimit: 4096,
      cssMinify: true,
      reportCompressedSize: true,
    },
  },

  experimental: {
    payloadExtraction: true,
    renderJsonPayloads: true,
    viewTransition: false,
  },

  image: {
    domains: ['app-pool.vylaris.online', 'homesguatemala.com', 'via.placeholder.com', 'vylaris.ar'],
    provider: 'ipx',
    quality: 80,
    format: ['webp', 'avif', 'jpg'],
    screens: {
      xs: 320, sm: 640, md: 768, lg: 1024, xl: 1280, xxl: 1536,
    },
    presets: {
      card: { modifiers: { format: 'webp', quality: 80, width: 400, height: 256 } },
      thumbnail: { modifiers: { format: 'webp', quality: 75, width: 150, height: 150 } },
      hero: { modifiers: { format: 'webp', quality: 85, width: 1200, height: 600 } }
    }
  },

  hooks: {
    async 'nitro:config'(config) {
      // Solo prerenderizar en producción para no ralentizar el desarrollo
      if (process.env.NODE_ENV === 'production' || process.env.PRERENDER === 'true') {
        try {
          console.log('Fetching dynamic routes for Nitro Prerender...');

          // Inmuebles
          const inmueblesRes = await fetch('https://app-pool.vylaris.online/homes/api/Homes/GetInmueblesPaginados?page=1&pageSize=500');
          const inmueblesData: any = await inmueblesRes.json();
          const inmuebles = inmueblesData.items || inmueblesData || [];

          if (Array.isArray(inmuebles)) {
            inmuebles.forEach((item: any) => {
              const slug = item.slugInmueble || item.slug || item.id;
              if (slug) config.prerender?.routes?.push(`/inmueble/${slug}`);
            });
          }

          // Proyectos
          const proyectosRes = await fetch('https://app-pool.vylaris.online/homes/api/Homes/GetProyectos');
          const proyectos: any = await proyectosRes.json();
          if (Array.isArray(proyectos)) {
            proyectos.forEach((item: any) => {
              const slug = item.slugProyecto || item.slug || item.id;
              if (slug) config.prerender?.routes?.push(`/proyecto/${slug}`);
            });
          }

          console.log(`Prerendering ${config.prerender?.routes?.length} total routes.`);
        } catch (e) {
          console.error('Error fetching dynamic routes for prerender:', e);
        }
      }
    }
  },

  nitro: {
    preset: 'netlify',
    compressPublicAssets: true,
    minify: true,
    prerender: {
      crawlLinks: true,
      failOnError: false,
      // Rutas base que siempre deben existir como HTML
      routes: [
        '/',
        '/propiedades',
        '/inmueble',
        '/proyectos-inmobiliarios',
        '/blog-inmobiliario',
        '/luxury',
        '/busqueda',
        '/custom',
        '/nosotros',
        '/propiedades/renta',
        '/propiedades/venta'
      ],
      ignore: ['/admin/**', '/auth/**']
    },
    routeRules: {
      // Proxy para evitar problemas de CORS y mantener SEO en luxury-homes
      '/luxury-homes/**': { proxy: 'https://old-web.homesguatemala.com/luxury-homes/**' },
      '/luxury-homes': { proxy: 'https://old-web.homesguatemala.com/luxury-homes' },

      // Cache optimizada
      '/_nuxt/**': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } },
      '/images/**': { headers: { 'cache-control': 'public, max-age=31536000, immutable' } },
    }
  }
});