<template>
    <Headerb />
    <div>
        <div class="max-w-6xl mx-auto pt-8 mt-16 pb-16 px-4 lg:px-0">
            <h1 class="text-3xl md:text-4xl lg:text-5xl title-alta mb-4 text-gray-900 text-center">BLOG INMOBILIARIO
            </h1>
            <hr class="w-48 h-1 mx-auto my-2 mb-6 bg-gray-100 border-0 rounded-sm" />
            <div v-if="loading" class="flex flex-wrap -m-4 mt-10 justify-center">
                <Skeleton v-for="n in 9" :key="n" />
            </div>
            <div v-else-if="error" class="flex justify-center items-center py-20">
                <div class="text-red-600 text-xl font-semibold p-4 border border-red-300 rounded-md bg-red-50">
                    Error al cargar los blogs: {{ error }}
                </div>
            </div>
            <div v-else>
                <div v-if="blogs.length === 0" class="text-center text-gray-600 text-2xl py-10">
                    No hay blogs disponibles para mostrar.
                </div>
                <div class="grid gap-10 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3">
                    <NuxtLink v-for="blog in paginatedBlogs" :key="blog.id" :to="`/${blog.category.toLowerCase()}/${blog.slug}`"
                        class="group flex flex-col bg-white border border-gray-200 rounded-lg shadow hover:shadow-lg transition-shadow duration-300 h-full"
                        style="min-height: 280px;">
                        <div v-if="blog.previewImage" class="overflow-hidden rounded-t-lg w-full">
                            <img :src="blog.previewImage" :alt="blog.title"
                                class="w-full h-auto max-h-[400px] object-contain sm:h-60 sm:object-cover object-center transition-transform duration-500 group-hover:scale-105" />
                        </div>
                        <div class="flex flex-col flex-1 px-4 pt-4 pb-6 text-center">
                            <h2 class="font-semibold text-lg mb-1 text-gray-900 tracking-tight">
                                {{ blog.title }}
                            </h2>
                            <div v-if="blog.ubicacion" class="text-base text-gray-500 mb-1">
                                {{ blog.ubicacion }}
                            </div>
                            <div class="text-sm font-light text-gray-700 min-h-[48px]"
                                v-html="truncate(blog.intro, 120)"></div>
                            <div class="mt-auto pt-4">
                                <span
                                    class="bg-black hover:bg-gray-900 text-white text-sm px-5 py-2 rounded transition-colors duration-200 font-light cursor-pointer select-none"
                                    style="outline: none;">
                                    Seguir viendo
                                </span>
                            </div>
                        </div>
                    </NuxtLink>
                </div>
                
                <div v-if="totalPages > 1" class="flex justify-center py-12">
                    <nav aria-label="Page navigation" class="flex justify-center">
                        <ul
                            class="inline-flex items-center -space-x-px text-base h-10 rounded-lg overflow-hidden border border-gray-300 shadow-sm">
                            <li>
                                <button @click="goToPrevPage" :disabled="currentPage === 1"
                                    class="flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white border-r border-gray-300 rounded-s-lg transition-colors duration-200 hover:bg-black hover:text-white"
                                    :class="{ 'opacity-50 cursor-not-allowed': currentPage === 1 }">
                                    Anterior
                                </button>
                            </li>
                            <li v-for="number in pageNumbers" :key="number">
                                <button @click="paginate(number)"
                                    class="flex items-center justify-center px-4 h-10 leading-tight border-r border-gray-300 transition-colors duration-200"
                                    :class="{
                                        'text-white bg-gray-800 font-bold border-gray-800': currentPage === number,
                                        'text-black bg-white hover:bg-black hover:text-white': currentPage !== number,
                                    }" :aria-current="currentPage === number ? 'page' : undefined">
                                    {{ number }}
                                </button>
                            </li>
                            <li>
                                <button @click="goToNextPage" :disabled="currentPage === totalPages"
                                    class="flex items-center justify-center px-4 h-10 leading-tight text-gray-600 bg-white rounded-e-lg transition-colors duration-200 hover:bg-black hover:text-white"
                                    :class="{ 'opacity-50 cursor-not-allowed': currentPage === totalPages }">
                                    Siguiente
                                </button>
                            </li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>
        <Footer />
    </div>
</template>

<script setup>
import Headerb from '~/components/headerb.vue'
import Footer from '~/components/footer.vue'
import { ref, computed, onMounted } from 'vue'
import { blogs as blogsData } from '~/data/blogs.js'
import Skeleton from "~/components/skeleton.vue";

const blogs = ref([])
const loading = ref(true)
const error = ref(null)

const itemsPerPage = 6
const currentPage = ref(1)

// Modificación para simular un tiempo de carga
onMounted(() => {
    setTimeout(() => {
        try {
            blogs.value = blogsData
        } catch (e) {
            error.value = e.message
        } finally {
            loading.value = false
        }
    }, 500) // Retraso de 500 milisegundos
})

// ... El resto de tu lógica computada y funciones permanece igual
const totalPages = computed(() =>
    Math.ceil(blogs.value.length / itemsPerPage)
)

const paginatedBlogs = computed(() => {
    const start = (currentPage.value - 1) * itemsPerPage
    return blogs.value.slice(start, start + itemsPerPage)
})

const pageNumbers = computed(() => {
    const nums = []
    for (let i = 1; i <= totalPages.value; i++) nums.push(i)
    return nums
})

function goToPrevPage() {
    if (currentPage.value > 1) currentPage.value--
}
function goToNextPage() {
    if (currentPage.value < totalPages.value) currentPage.value++
}
function paginate(number) {
    currentPage.value = number
}

function truncate(html, limit = 120) {
    const div = document.createElement('div')
    div.innerHTML = html || ''
    const text = div.innerText || ''
    return text.length > limit ? text.slice(0, limit) + '...' : text
}
</script>

<style scoped>
img {
    width: 100%;
    height: auto;
    object-fit: contain;
}

@media (min-width: 640px) {
    img {
        height: 15rem;
        object-fit: cover;
    }
}


h1 {
    font-family: 'Raleway', Arial, sans-serif;
}
</style>