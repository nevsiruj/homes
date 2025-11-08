<template>
    <div class="chart-container">
      <Line :data="chartData" :options="chartOptions" :key="chartKey" />
    </div>
  </template>
  
  <script>
  // Datos iniciales
  const initialData = {
    labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    datasets: [
      {
        label: 'Matches',
        data: [12, 19, 8, 15, 22, 17, 25, 12, 19, 30, 25, 40],
        borderColor: '#3B82F6',
        backgroundColor: 'rgba(59, 130, 246, 0.1)',
        tension: 0.4,
        fill: true
      }
    ]
  }
  
  const initialOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false
      },
      tooltip: {
        mode: 'index',
        intersect: false
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        grid: {
          drawBorder: false
        }
      },
      x: {
        grid: {
          display: false
        }
      }
    },
    animation: {
      duration: 1000
    }
  }
  </script>
  
  <script setup>
  import { Line } from 'vue-chartjs'
  import { Chart as ChartJS, Title, Tooltip, Legend, LineElement, PointElement, LinearScale, CategoryScale } from 'chart.js'
  import { ref, watch } from 'vue'
  
  ChartJS.register(Title, Tooltip, Legend, LineElement, PointElement, LinearScale, CategoryScale)
  
  const props = defineProps({
    chartData: {
      type: Object,
      default: () => ({ ...initialData })
    },
    chartOptions: {
      type: Object,
      default: () => ({ ...initialOptions })
    }
  })
  
  const chartKey = ref(0)
  const localChartData = ref({ ...props.chartData })
  
  // Función para actualizar datos
  const updateData = (newData) => {
    localChartData.value.datasets[0].data = newData
    chartKey.value++ // Force re-render
  }
  
  // Ejemplo: Actualizar cada 5 segundos (simulando datos dinámicos)
  setInterval(() => {
    const newData = localChartData.value.datasets[0].data.map(
      value => Math.max(0, value + Math.floor(Math.random() * 10) - 4)
    )
    updateData(newData)
  }, 5000)
  
  // Exponer función de actualización
  defineExpose({ updateData })
  </script>
  
  <style scoped>
  .chart-container {
    position: relative;
    height: 350px;
    width: 100%;
  }
  </style>