<template>
    <div class="font-raleway">
        <Header />
        <div class="max-w-3xl mx-auto pt-8 pb-16 px-4 lg:px-0">
            <h1 class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900 font-light text-center">
                {{ blog.Title }}
            </h1>
            <hr class="w-48 h-1 mx-auto my-2 mb-6 bg-gray-100 border-0 rounded-sm" />
            <div v-if="blog['Image URL']" class="overflow-hidden rounded-lg shadow-lg mb-6">
                <img :src="blog['Image URL']" :alt="`${blog.Title} - Homes Guatemala`"
                    class="w-full h-auto max-h-[400px] object-cover object-center rounded-lg" />
            </div>
            <div class="prose max-w-none text-lg mb-8" v-html="blog.Content"></div>
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
const blog = blogs.find((b) => b.Slug === route.params.slug && b['Categorías'].toLowerCase() === route.params.category);

if (!blog) {
    throw createError({ statusCode: 404, statusMessage: 'Página No Encontrada', fatal: true });
}

const DOMINIO_BASE = 'https://webhomes.vylaris.online'; // <-- REEMPLAZAR ESTO CON DOMINIO FINAL
const FALLBACK_IMAGE_URL = `${DOMINIO_BASE}/default-social-image.jpg`;

const metaDescription = computed(() => {
    if (!blog?.Content) {
        return 'Un artículo interesante de nuestro blog.';
    }
    const cleanText = blog.Content.replace(/<[^>]*>/g, '').trim();
    return cleanText.substring(0, 155) + (cleanText.length > 155 ? '...' : '');
});

const metaImage = computed(() => {
    const imageUrl = blog?.['Image URL'];
    if (!imageUrl) {
        return FALLBACK_IMAGE_URL;
    }
    if (imageUrl.startsWith('http')) {
        return imageUrl;
    }
    return `${DOMINIO_BASE}${imageUrl}`;
});

useHead({
    title: () => blog?.Title || 'Nuestro Blog',
    meta: [
        // Metas estándar
        { name: 'description', content: () => metaDescription.value },

        // Open Graph (para Facebook, LinkedIn, etc.)
        { property: 'og:title', content: () => blog?.Title },
        { property: 'og:description', content: () => metaDescription.value },
        { property: 'og:image', content: () => metaImage.value },           
        { property: 'og:url', content: () => `${DOMINIO_BASE}${route.path}` },
        { property: 'og:type', content: 'article' },

        // Twitter Cards (para Twitter)
        { name: 'twitter:card', content: 'summary_large_image' },
        { name: 'twitter:title', content: () => blog?.Title },
        { name: 'twitter:description', content: () => metaDescription.value },
        { name: 'twitter:image', content: () => metaImage.value },       
    ],
    link: [
        { rel: 'canonical', href: () => `${DOMINIO_BASE}${route.path}` }
    ],
    script: [
        {
            type: 'application/ld+json',
            children: JSON.stringify({
                '@context': 'https://schema.org',
                '@type': 'Article',
                headline: blog?.Title || 'Artículo de blog',
                description: metaDescription.value,
                image: metaImage.value,
                author: {
                    '@type': 'Organization',
                    name: 'Homes Guatemala',
                    url: DOMINIO_BASE
                },
                publisher: {
                    '@type': 'Organization',
                    name: 'Homes Guatemala',
                    logo: {
                        '@type': 'ImageObject',
                        url: `${DOMINIO_BASE}/logo.png`
                    }
                },
                mainEntityOfPage: {
                    '@type': 'WebPage',
                    '@id': `${DOMINIO_BASE}${route.path}`
                }
            })
        }
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
