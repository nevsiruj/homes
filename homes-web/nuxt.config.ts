// nuxt.config.ts
import { defineNuxtConfig } from "nuxt/config";

export default defineNuxtConfig({
  // -------------------- Nuxt Core Configuration --------------------
  compatibilityDate: "2025-06-21",
  devtools: { enabled: false },

  // Generación y despliegue del sitio
  ssr: true, // Se recomienda SSR (Server-Side Rendering) para SEO
  generate: {
    fallback: true,
  },

  app: {
    head: {
      meta: [
        // Metadatos de verificación de dominio
        { name: "p:domain_verify", content: "4ff93a868a573e1b24e15558f505f316" },
        { name: "facebook-domain-verification", content: "s7lsdi9b3qj53qjggc8lgumeiythqm" },
        { name: "google-site-verification", content: "0021L2ztWBVMwBLnR8VPXwfs8JspGBFnR3IhyqScj-c" },

        // Metadatos de Open Graph
        { property: "fb:app_id", content: "239174403519612" },
        { property: "og:locale", content: "es_ES" },
        { property: "og:type", content: "website" },
        {
          property: "og:title",
          content: "Homes Guatemala - Apartamentos y Casas en Venta y Alquiler de propiedades",
        },
        {
          property: "og:description",
          content: "En Homes Guatemala contamos con las mejores propiedades inmobiliarias. Casas, Apartamentos, Oficinas, Terrenos, Bodegas en Venta y alquiler. Mansiones y propiedades de lujo en Guatemala.",
        },
        { property: "og:url", content: "https://homesguatemala.com/" },
        {
          property: "og:site_name",
          content: "Homes Guatemala - Apartamentos y Casas en Venta y Alquiler",
        },
        {
          property: "og:image",
          content: "https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp",
        },
        {
          property: "og:image:secure_url",
          content: "https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp",
        },

        // Metadatos para Twitter Card
        { name: "twitter:card", content: "summary_large_image" },
        {
          name: "twitter:title",
          content: "Homes Guatemala - Casas y Apartamentos. Venta y Alquiler de propiedades",
        },
        {
          name: "twitter:description",
          content: "En Homes Guatemala contamos con las mejores propiedades inmobiliarias. Casas, Apartamentos, Oficinas, Terrenos, Bodegas en Venta y alquiler. Mansiones y propiedades de lujo en Guatemala.",
        },
        {
          name: "twitter:image",
          content: "https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp",
        },
      ],
      link: [
        { rel: "icon", type: "image/x-icon", href: "/favicon.ico" },
        // Core Web Vitals optimization
        {
          rel: "preconnect",
          href: "https://app-pool.vylaris.online",
        },
        {
          rel: "dns-prefetch",
          href: "https://app-pool.vylaris.online",
        },
      ],
      script: [
        // Script del Pixel de Facebook
        {
          hid: "facebook-pixel",
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
        },
      ],
      noscript: [
        // NoScript del Pixel de Facebook
        {
          hid: "facebook-pixel-noscript",
          innerHTML: `<img height="1" width="1" style="display:none"
          src="https://www.facebook.com/tr?id=239174403519612&ev=PageView&noscript=1"/>`,
        },
      ],
    },
  },

  // -------------------- Modules & Global Styles --------------------
  modules: [
    "@nuxtjs/seo",
    "@nuxt/content",
    "@pinia/nuxt",
    "@nuxtjs/color-mode",
  ],

  // Configuración de los módulos de SEO
  robots: {},
  sitemap: {
    hostname: 'https://homesguatemala.com',
    routes: async () => {
      try {
        const API_INMUEBLE = 'https://app-pool.vylaris.online/homes/api/Inmueble';
        const API_PROYECTO = 'https://app-pool.vylaris.online/homes/api/Proyecto';

        // Fetch de Inmuebles
        const responseInmueble = await fetch(API_INMUEBLE);
        if (!responseInmueble.ok) {
          throw new Error(`Error Inmueble: ${responseInmueble.statusText}`);
        }
        const dataInmueble = await responseInmueble.json();
        const propiedades = dataInmueble?.items?.$values || [];
        const slugsInmuebles = propiedades.map(
          (propiedad: any) => `/inmueble/${propiedad.slugInmueble}`
        );

        // Fetch de Proyectos
        const responseProyecto = await fetch(API_PROYECTO);
        if (!responseProyecto.ok) {
          throw new Error(`Error Proyecto: ${responseProyecto.statusText}`);
        }
        const dataProyecto = await responseProyecto.json();
        const proyectos = dataProyecto?.items?.$values || [];
        const slugsProyectos = proyectos.map(
          (proyecto: any) => `/proyecto/${proyecto.slugProyecto}`
        );

        // Combinar ambos arrays
        return [...slugsInmuebles, ...slugsProyectos];
      } catch (error) {
        console.error('Error al generar las rutas del sitemap:', error);
        return [];
      }
    },
  },

  colorMode: {
    classSuffix: "",
    preference: "light",
    fallback: "light",
    hid: "nuxt-color-mode-script",
    storageKey: "nuxt-color-mode",
  },

  css: ["~/assets/css/main.css"],

  // -------------------- Build & Vite Configuration --------------------
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
  },

  // -------------------- Nitro & Server Configuration --------------------
  nitro: {
    publicAssets: [
      {
        dir: "public/images",
        baseURL: "/images",
      },
      {
        dir: "assets/fonts",
        baseURL: "/fonts",
      },
    ],
    compressPublicAssets: false,
    routeRules: {
      "/luxury-homes/**": { proxy: "https://homesguatemala.com/**" },
    },
  },
});