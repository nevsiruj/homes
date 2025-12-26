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
      title: 'Homes Guatemala - Bienes Raíces de Lujo',
      meta: [
        { charset: 'utf-8' },
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { name: 'description', content: 'Líderes en bienes raíces en Guatemala. Encuentra casas, apartamentos y propiedades de lujo en venta y alquiler.' },
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
        { rel: "canonical", href: "https://homesguatemala.com" },
        { rel: "preconnect", href: "https://app-pool.vylaris.online" },
        { rel: "dns-prefetch", href: "https://app-pool.vylaris.online" },
        { rel: "alternate", hreflang: "es-GT", href: "https://homesguatemala.com" },
        { rel: "alternate", hreflang: "x-default", href: "https://homesguatemala.com" }
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
          key: "facebook-pixel",
          innerHTML: `!function(f,b,e,v,n,t,s)
          {if(f.fbq)return;n=f.fbq=function(){n.callMethod?
          n.callMethod.apply(n,arguments):n.queue.push(arguments)};
          if(!f._fbq)f._fbq=n;n.push=n;n.loaded=!0;n.version='2.0';
          n.queue=[];t=b.createElement(e);t.async=!0;
          t.src=v;s=b.getElementsByTagName(e)[0];
          s.parentNode.insertBefore(t,s)}(window, document,'script',
          'https://connect.facebook.net/en_US/fbevents.js');
          fbq('init', '239174403519612');
          fbq('track', 'PageView');`,
        }
      ],
    }
  },

  modules: [
    "@nuxt/image-edge",
    "@nuxtjs/seo",
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

  robots: {
    disallow: ['/admin', '/_nuxt'],
    allow: ['/'],
    sitemap: ['https://homesguatemala.com/sitemap.xml'],
  },

  // sitemap: {
  //   sources: [
  //     '/api/sitemap-urls'
  //   ]
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
      rollupOptions: {
        output: {
          // Agrupar chunks de manera más eficiente
          manualChunks: {
            'vendor': ['vue', 'vue-router'],
            'swiper': ['swiper'],
          },
        },
      },
      // Inline CSS pequeños (menos de 4kb)
      assetsInlineLimit: 4096,
      // Minificar CSS
      cssMinify: true,
    },
  },

  nitro: {
    compressPublicAssets: {
      gzip: true,
      brotli: true,
    },
    minify: true,
    routeRules: {
      // Proxy para luxury homes (mantener)
      '/luxury-homes/**': {
        proxy: 'https://old-web.homesguatemala.com/luxury-homes/**'
      },
      '/luxury-homes': {
        proxy: 'https://old-web.homesguatemala.com/luxury-homes'
      },

      // Redirecciones para URLs antiguas de propiedades
      '/propiedad/**': { redirect: '/propiedades' },
      '/property/**': { redirect: '/propiedades' },
      // '/inmueble/**': { redirect: '/propiedades' }, // REMOVIDO: Esta regla impedía ver detalles de inmuebles
      '/listing/**': { redirect: '/propiedades' },

      // Redirecciones de URLs comunes mal escritas
      '/home': { redirect: '/' },
      '/inicio': { redirect: '/' },
      '/index': { redirect: '/' },
      '/index.html': { redirect: '/' },
      '/index.php': { redirect: '/' },

      // Redirecciones de secciones antiguas
      '/contacto': { redirect: '/nosotros' },
      '/contact': { redirect: '/nosotros' },
      '/about': { redirect: '/nosotros' },
      '/acerca-de': { redirect: '/nosotros' },

      // Headers de seguridad y SEO
      '/**': {
        headers: {
          'X-Robots-Tag': 'index, follow',
          'X-Content-Type-Options': 'nosniff',
          'X-Frame-Options': 'SAMEORIGIN',
          'Referrer-Policy': 'strict-origin-when-cross-origin'
        }
      }
    }
  },

  image: {
    domains: ['app-pool.vylaris.online', 'homesguatemala.com', 'via.placeholder.com', 'vylaris.ar'],
    format: ['webp', 'avif'],
    quality: 80,
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
  }
});
