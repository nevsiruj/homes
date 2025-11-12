<template>
  <div class="flex items-center gap-2">
    <select
      :value="itemsPerPageLocal"
      @change="onChange($event)"
      class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2"
    >
      <option v-for="opt in perPageOptions" :key="opt" :value="opt">{{ opt }} por página</option>
    </select>
  </div>
</template>

<script setup>
import { ref, watch, toRef } from 'vue';

const props = defineProps({
  itemsPerPage: { type: Number, default: 20 },
  currentPage: { type: Number, default: 1 },
  totalItems: { type: Number, default: 0 },
  perPageOptions: { type: Array, default: () => [20,50,100] },
});

const emit = defineEmits(['update:itemsPerPage','update:currentPage']);

// Local copy to avoid mutating prop directly
const itemsPerPageLocal = ref(props.itemsPerPage ?? 20);

watch(() => props.itemsPerPage, (v) => {
  itemsPerPageLocal.value = v;
});

function onChange(e) {
  const v = Number(e.target.value);
  itemsPerPageLocal.value = v;
  // Al cambiar el tamaño de página, resetear a página 1
  emit('update:itemsPerPage', v);
  emit('update:currentPage', 1);
}
</script>

<style scoped>
</style>
