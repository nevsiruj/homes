<template>
  <div v-if="showLoader" class="loader-overlay">
    <Loader />
  </div>
  <Header />
  <Seccion1 />
  <RedesFlotantes />
  <Footer />
</template>

<script>
import Header from "../../components/header.vue";
import Seccion1 from "./seccion1.vue";
import Seccion2 from "./seccion2.vue";
import Seccion3 from "./seccion3.vue";
import Seccion4 from "./seccion4.vue";
import Footer from "../../components/footer.vue";
import RedesFlotantes from "../../components/redesFlotantes.vue";
import Loader from "../../components/Loader.vue";

export default {
  components: {
    Header,
    Seccion1,
    Seccion2,
    Seccion3,
    Seccion4,
    Footer,
    RedesFlotantes,
    Loader,    
  },

  setup() {
    const showLoader = ref(true);
    const MIN_DURATION_MS = 2000;

    const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

    onMounted(async () => {
      // Mantiene el loader por al menos 2 segundos
      await delay(MIN_DURATION_MS);
      showLoader.value = false;
    });

    return { showLoader };
  },
  
};
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

/* Fuente Roboto*/
@import url("https://fonts.googleapis.com/css2?family=Roboto+Condensed:wght@300;400&display=swap");

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
