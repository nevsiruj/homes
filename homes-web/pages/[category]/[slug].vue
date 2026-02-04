<template>
    <div class="font-raleway">
        <Header />
        <div v-if="loading" class="flex justify-center items-center py-20">
            <div class="text-gray-600 text-xl">Cargando...</div>
        </div>
        <div v-else-if="error" class="flex justify-center items-center py-20">
            <div class="text-red-600 text-xl font-semibold p-4 border border-red-300 rounded-md bg-red-50">
                Error: {{ error }}
            </div>
        </div>
        <div v-else class="max-w-4xl mx-auto pt-8 pb-16 px-4 lg:px-0">
            <h1 class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900 font-light text-center">
                {{ blog.Title }}
            </h1>
            <hr class="w-48 h-1 mx-auto my-2 mb-6 bg-gray-100 border-0 rounded-sm" />
            
            <!-- Imagen principal del blog -->
            <div v-if="mainImage" class="overflow-hidden rounded-lg shadow-lg mb-8">
                <img :src="mainImage" :alt="blog.Title"
                    class="w-full h-auto max-h-[500px] object-cover object-center rounded-lg" loading="lazy" />
            </div>
            
            <!-- Contenido del blog -->
            <article class="blog-content prose prose-lg max-w-none" v-html="blog.Content"></article>
        </div>
        <Footer />
    </div>
</template>

<script setup>
import Header from "~/components/headerb.vue";
import Footer from "~/components/footer.vue";
import { useRoute } from "vue-router";
import blogService from "~/services/blogService.js";
import { computed, ref, onMounted } from 'vue';
import { useRequestURL } from "#imports";

const route = useRoute();

// Función para normalizar categorías (igual que en index.vue)
function normalizeCategory(category) {
    if (!category) return 'blog'
    return category
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '') // Eliminar acentos
        .replace(/\s+/g, '-') // Reemplazar espacios con guiones
        .trim()
}

// Buscar el blog usando el servicio
const blog = ref(null)
const loading = ref(true)
const error = ref(null)

onMounted(async () => {
    try {
        blog.value = await blogService.getBlogBySlug(route.params.slug)
        // Verificar que la categoría coincida
        const blogCategory = normalizeCategory(blog.value['Categorías'])
        const routeCategory = route.params.category
        if (blogCategory !== routeCategory) {
            throw new Error('Categoría no coincide')
        }
    } catch (e) {
        error.value = e.message
        throw createError({ statusCode: 404, statusMessage: 'Página No Encontrada', fatal: true });
    } finally {
        loading.value = false
    }
})

const dynamicDomain = computed(() => {
    try {
        const url = useRequestURL();
        if (url.origin && !url.origin.includes('localhost')) return url.origin;
    } catch (e) {}
    return 'https://homesguatemala.com';
});
const DOMINIO_BASE = dynamicDomain.value;
const FALLBACK_IMAGE_URL = 'https://app-pool.vylaris.online/dcmigserver/homes/5ba8e587-bc89-4bac-952a-2edf8a1291c4.webp';

// Obtener la imagen principal (primera imagen del array)
const mainImage = computed(() => {
    if (!blog.value?.['Image URL']) {
        return FALLBACK_IMAGE_URL;
    }
    const images = blog.value['Image URL'].split('|');
    return images[0] || FALLBACK_IMAGE_URL;
});

// Generar descripción meta desde el contenido HTML
const metaDescription = computed(() => {
    if (!blog.value?.Content) {
        return 'Descubre los mejores consejos y tips sobre bienes raíces en Guatemala - Homes Guatemala';
    }
    // Limpiar HTML y obtener texto plano
    const cleanText = blog.value.Content
        .replace(/<[^>]*>/g, ' ')
        .replace(/\s+/g, ' ')
        .replace(/&nbsp;/g, ' ')
        .trim();
    return cleanText.substring(0, 155) + (cleanText.length > 155 ? '...' : '');
});

const metaImage = computed(() => {
    return mainImage.value;
});

useHead({
    title: () => blog.value?.Title || 'Blog - Homes Guatemala',
    meta: [
        // Metas estándar
        { name: 'description', content: () => metaDescription.value },

        // Open Graph (para Facebook, LinkedIn, etc.)
        { property: 'og:title', content: () => blog.value?.Title },
        { property: 'og:description', content: () => metaDescription.value },
        { property: 'og:image', content: () => metaImage.value },           
        { property: 'og:url', content: () => `${DOMINIO_BASE}${route.path}` },
        { property: 'og:type', content: 'article' },

        // Twitter Cards (para Twitter)
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: () => blog.value?.Title },
        { name: 'twitter:description', content: () => metaDescription.value },
        { name: 'twitter:image', content: () => metaImage.value },       
    ],
    link: [
        { rel: 'canonical', href: () => `${DOMINIO_BASE}${route.path}` }
    ]
});
</script>

<style>
body {
    font-family: 'Raleway', sans-serif;
}
</style>

<style scoped>
.title-alta {
    font-family: 'Raleway', sans-serif;
    font-weight: 300;
}

.blog-content {
    font-family: 'Raleway', sans-serif;
}

/* Estilos para el contenido del blog */
.blog-content :deep(img) {
    max-width: 100%;
    height: auto;
    border-radius: 0.5rem;
    margin: 1.5rem auto;
    display: block;
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
}

.blog-content :deep(h1),
.blog-content :deep(h2),
.blog-content :deep(h3),
.blog-content :deep(h4) {
    font-family: 'Raleway', sans-serif;
    font-weight: 600;
    color: #1a202c;
    margin-top: 2rem;
    margin-bottom: 1rem;
}

.blog-content :deep(h1) {
    font-size: 2rem;
}

.blog-content :deep(h2) {
    font-size: 1.75rem;
}

.blog-content :deep(h3) {
    font-size: 1.5rem;
}

.blog-content :deep(h4) {
    font-size: 1.25rem;
}

.blog-content :deep(p) {
    margin-bottom: 1rem;
    line-height: 1.8;
    color: #4a5568;
}

.blog-content :deep(ul),
.blog-content :deep(ol) {
    margin-left: 1.5rem;
    margin-bottom: 1rem;
}

.blog-content :deep(li) {
    margin-bottom: 0.5rem;
    line-height: 1.7;
}

.blog-content :deep(a) {
    color: #2563eb;
    text-decoration: underline;
}

.blog-content :deep(a:hover) {
    color: #1d4ed8;
}

.blog-content :deep(blockquote) {
    border-left: 4px solid #e5e7eb;
    padding-left: 1rem;
    margin: 1.5rem 0;
    font-style: italic;
    color: #6b7280;
}

.blog-content :deep(strong),
.blog-content :deep(b) {
    font-weight: 600;
    color: #1a202c;
}

.blog-content :deep(pre) {
    background-color: #f3f4f6;
    padding: 1rem;
    border-radius: 0.5rem;
    overflow-x: auto;
    margin: 1rem 0;
}

.blog-content :deep(code) {
    background-color: #f3f4f6;
    padding: 0.2rem 0.4rem;
    border-radius: 0.25rem;
    font-size: 0.9em;
}

/* FAQ Schema styles */
.blog-content :deep(.rank-math-faq-item) {
    margin-bottom: 1.5rem;
    padding: 1rem;
    background-color: #f9fafb;
    border-radius: 0.5rem;
}

.blog-content :deep(.rank-math-question) {
    font-weight: 600;
    margin-bottom: 0.5rem;
    color: #1a202c;
}

.blog-content :deep(.rank-math-answer) {
    color: #4a5568;
    line-height: 1.7;
}
</style>
