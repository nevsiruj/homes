<template>
  <!-- Removed forced loader to speed up perceived load time -->
  <Header />
  <Seccion1 />
  <RedesFlotantes />
  <Footer />
</template>

<script setup>
import { defineAsyncComponent } from 'vue';

// Componentes críticos - carga síncrona
import Header from "~/components/header.vue";
import Seccion1 from "./seccion1.vue";

// Componentes no críticos - carga diferida (async)
const Footer = defineAsyncComponent(() => import('~/components/footer.vue'));
const RedesFlotantes = defineAsyncComponent(() => import('~/components/redesFlotantes.vue'));

// Precargar fuentes críticas
useHead({
  link: [
    // Preload fuentes críticas para evitar FOIT (Flash of Invisible Text)
    {
      rel: 'preload',
      href: '/assets/font/ralewey/raleway-v36-latin-300.woff2',
      as: 'font',
      type: 'font/woff2',
      crossorigin: 'anonymous'
    }
  ]
});
</script>

<style>
/* Fuente Alta */
@font-face {
  font-family: "Alta";
  src: url("../../assets/font/alta/Alta_regular.woff2") format("woff2"),
    url("../../assets/font/alta/Alta_regular.woff") format("woff");
  font-weight: 600;
  font-style: normal;
  font-display: swap;
}

/* Fuente Optima */
@font-face {
  font-family: "Optima";
  src: url("../../assets/font/optima/OptimaMedium.woff") format("woff");
  font-weight: 500;
  font-style: normal;
  font-display: swap;
}

/* raleway-200 - latin */
@font-face {
  font-display: swap; /* Check https://developer.mozilla.org/en-US/docs/Web/CSS/@font-face/font-display for other options. */
  font-family: "Raleway";
  font-style: normal;
  font-weight: 200;
  src: url("../../assets/font/ralewey/raleway-v36-latin-200.woff2")
    format("woff2"); /* Chrome 36+, Opera 23+, Firefox 39+, Safari 12+, iOS 10+ */
}
/* raleway-300 - latin */
@font-face {
  font-display: swap; /* Check https://developer.mozilla.org/en-US/docs/Web/CSS/@font-face/font-display for other options. */
  font-family: "Raleway";
  font-style: normal;
  font-weight: 300;
  src: url("../../assets/font/ralewey/raleway-v36-latin-300.woff2")
    format("woff2"); /* Chrome 36+, Opera 23+, Firefox 39+, Safari 12+, iOS 10+ */
}
/* raleway-regular - latin */
@font-face {
  font-display: swap; /* Check https://developer.mozilla.org/en-US/docs/Web/CSS/@font-face/font-display for other options. */
  font-family: "Raleway";
  font-style: normal;
  font-weight: 400;
  src: url("../../assets/font/ralewey/raleway-v36-latin-regular.woff2")
    format("woff2"); /* Chrome 36+, Opera 23+, Firefox 39+, Safari 12+, iOS 10+ */
}

/* Fuente Roboto - Usar font-display swap en lugar de @import bloqueante */
/* Removido: @import url("https://fonts.googleapis.com/css2?family=Roboto+Condensed:wght@300;400&display=swap"); */
/* Las fuentes de Roboto se cargan ahora via preconnect en nuxt.config.ts */

/* Estilos directos */
.title-alta {
  font-family: "Raleway";
  font-weight: 300;
  /* font-size: 60px; */
  line-height: 1.2;
}
.title-alta-2 {
  font-family: "Raleway";
  font-weight: 300;
  font-size: 40px;
  line-height: 1.2;
}

.subtitle-optima {
  font-family: "Raleway";
  font-weight: 400;
  font-size: 1.3rem;
  line-height: 1.5;
}
.subtitle-optima1 {
  font-family: "Raleway";
  font-weight: 300;
  font-size: 1rem;
  line-height: 0.5;
}

.boton-optima {
  font-family: "Optima";
  font-weight: 300;
  font-size: 16px;
  line-height: 1.3;
}

.long-text-roboto {
  font-family: "Raleway";
  font-weight: 300;
  font-size: 14px;
  /* line-height: 1.5; */
}

.font-roboto {
  font-family: "Raleway";
  font-weight: 300;
  font-size: 12px;
}

.font-roboto-1 {
  font-family: "Raleway";
  font-weight: 300;
  font-size: 14px;
}

.font-title {
  font-family: "Raleway";
  font-weight: 300;
  font-size: 30px;
  line-height: 1.2;
}
</style>
