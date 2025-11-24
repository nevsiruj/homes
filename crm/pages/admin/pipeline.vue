<template> 
  <div class="grid grid-cols-1 gap-4 mt-5">
    <Task />
  </div>
</template>

<script setup>
import Task from '../../components/bandejaTarea/task.vue'

definePageMeta({
  requiresAuth: true,
  layout: "admin",
});
</script>
