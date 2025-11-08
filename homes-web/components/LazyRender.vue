<template>
    <div ref="target" :style="{ minHeight: initialHeight }">
        <slot v-if="shouldRender" />
    </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const props = defineProps({
    initialHeight: {
        type: String,
        default: '480px' 
    }
});

const target = ref(null);
const shouldRender = ref(false);
let observer = null;

onMounted(() => {
    observer = new IntersectionObserver(
        ([entry]) => {
            if (entry.isIntersecting) {
                shouldRender.value = true;
                observer.disconnect();
            }
        },
        {
            rootMargin: '200px'
        }
    );

    if (target.value) {
        observer.observe(target.value);
    }
});

onUnmounted(() => {
    if (observer) {
        observer.disconnect();
    }
});
</script>