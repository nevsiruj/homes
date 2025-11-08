<template>
  <div class="chart-container">
    <Pie :data="chartData" :options="mergedOptions" />
  </div>
</template>

<script>
// Parte en script normal para las constantes que necesitan ser hoisted
const defaultChartData = {
  labels: ['Éxito', 'No éxito'],
  datasets: [
    {
      backgroundColor: ['#4CAF50', '#F44336'],
      data: [75, 25]
    }
  ]
}

const defaultOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'bottom',
      labels: {
        color: '#6B7280'
      }
    }
  }
}
</script>

<script setup>
import { Pie } from 'vue-chartjs'
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement, CategoryScale } from 'chart.js'

// Registra los componentes necesarios
ChartJS.register(Title, Tooltip, Legend, ArcElement, CategoryScale)

const props = defineProps({
  chartData: {
    type: Object,
    default: () => ({ ...defaultChartData })
  },
  chartOptions: {
    type: Object,
    default: () => ({ ...defaultOptions })
  }
})

// Mezcla las opciones por defecto con las personalizadas
const mergedOptions = {
  ...defaultOptions,
  ...props.chartOptions,
  plugins: {
    ...defaultOptions.plugins,
    ...(props.chartOptions.plugins || {})
  }
}
</script>

<style scoped>
.chart-container {
  position: relative;
  height: 300px;
  width: 100%;
}
</style>