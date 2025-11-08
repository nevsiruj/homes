<template>
    <Header />
    <Seccion1 />
    <Seccion2 />
    <Seccion3 />
    <Seccion4 />
    
    <Footer />
    

</template>

<script>
import Header from './header.vue'
import Seccion1 from './seccion1.vue'
import Seccion2 from './seccion2.vue'
import Seccion3 from './seccion3.vue'
import Seccion4 from './seccion4.vue'
import Footer from './footer.vue'
//import RedesFlotantes from '../../components/redesFlotantes.vue'

export default {
  components: {
    Header,
    Seccion1,
    Seccion2,
    Seccion3,
    Seccion4,
    Footer,
    //RedesFlotantes
  }
}
</script>