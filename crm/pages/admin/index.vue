<template> 

  <div class="grid grid-cols-1 gap-4 mt-5">
    <Task />
    <!-- <MatchPendiente :clienteId="clienteId" /> -->
  </div>
</template>
<script setup>

import Task from '../../components/bandejaTarea/task.vue'
// import MatchPendiente from '../../components/matchPendiente.vue'
import { useAuthStore } from "@/stores/auth";

const lineChartRef = ref(null)

// const clienteId = ref(4327) 

definePageMeta({
  requiresAuth: true,
  layout: "admin",
});
</script>
