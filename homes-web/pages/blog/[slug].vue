<template>
    <div class="font-raleway">
        <Header />
        <div class="max-w-3xl mx-auto pt-8 pb-16 px-4 lg:px-0">
            <h1 class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900 font-light text-center">
                {{ blog.title }}
            </h1>
            <hr class="w-48 h-1 mx-auto my-2 mb-6 bg-gray-100 border-0 rounded-sm" />
            <div v-if="blog.mainImage" class="overflow-hidden rounded-lg shadow-lg mb-6">
                <img :src="blog.mainImage" :alt="blog.title"
                    class="w-full h-auto max-h-[400px] object-cover object-center rounded-lg" />
            </div>
            <div class="prose max-w-none text-lg mb-8" v-html="blog.intro"></div>
            <template v-for="(section, idx) in blog.sections" :key="idx">
                <h2 class="text-xl font-semibold mb-2">{{ section.heading }}</h2>
                <div class="prose max-w-none mb-8" v-html="section.content"></div>
                <div v-if="section.image" class="overflow-hidden rounded-lg shadow mb-10">
                    <img :src="section.image" :alt="section.heading"
                        class="w-full h-auto max-h-[400px] object-cover object-center rounded-lg" />
                </div>
            </template>
        </div>
        <Footer />
    </div>
</template>

<script setup>
import Header from "~/components/headerb.vue";
import Footer from "~/components/footer.vue";
import { useRoute } from "vue-router";
import { blogs } from "~/data/blogs.js";
import { computed } from 'vue';

const route = useRoute();
const blog = blogs.find((b) => b.slug === route.params.slug);

if (!blog) {
    throw createError({ statusCode: 404, statusMessage: 'Página No Encontrada', fatal: true });
}

const DOMINIO_BASE = 'https://webhomes.vylaris.online'; // <-- REEMPLAZAR ESTO CON DOMINIO FINAL
const FALLBACK_IMAGE_URL = `${DOMINIO_BASE}/default-social-image.jpg`;

const metaDescription = computed(() => {
    if (!blog?.intro) {
        return 'Un artículo interesante de nuestro blog.';
    }
    const cleanText = blog.intro.replace(/<[^>]*>/g, '').trim();
    return cleanText.substring(0, 155) + (cleanText.length > 155 ? '...' : '');
});

const metaImage = computed(() => {
    const imageUrl = blog?.mainImage;
    if (!imageUrl) {
        return FALLBACK_IMAGE_URL;
    }
    if (imageUrl.startsWith('http')) {
        return imageUrl;
    }
    return `${DOMINIO_BASE}${imageUrl}`;
});

useHead({
    title: () => blog?.title || 'Nuestro Blog',
    meta: [
        // Metas estándar
        { name: 'description', content: () => metaDescription.value },

        // Open Graph (para Facebook, LinkedIn, etc.)
        { property: 'og:title', content: () => blog?.title },
        { property: 'og:description', content: () => metaDescription.value },
        { property: 'og:image', content: () => metaImage.value },           
        { property: 'og:url', content: () => `${DOMINIO_BASE}${route.path}` },
        { property: 'og:type', content: 'article' },

        // Twitter Cards (para Twitter)
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: () => blog?.title },
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
img {
    max-width: 100%;
    height: auto;
}

.rounded-lg {
    border-radius: 0.5rem;
}

.shadow-lg {
    box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}

.prose {
    font-size: 1.13rem;
    font-weight: 400;
    color: #333;
    line-height: 1.7;
}

h1 {
    font-family: 'Raleway', sans-serif;
    margin-bottom: 1.2rem;
}

h2 {
    font-family: 'Raleway', sans-serif;
    font-size: 1.3rem;
    font-weight: 600;
    margin-top: 2rem;
    margin-bottom: 1rem;
}
</style>