<template>
    <div class="chart-container">
      <Pie :data="chartData" :options="mergedOptions" />
    </div>
  </template>
  
  <script>
  // Configuración por defecto para el gráfico donut
  const defaultChartData = {
    labels: ['Completados', 'En progreso', 'Pendientes'],
    datasets: [{
      data: [60, 25, 15],
      backgroundColor: ['#4CAF50', '#FFC107', '#F44336'],
      borderWidth: 0
    }]
  }
  
  const defaultOptions = {
    responsive: true,
    maintainAspectRatio: false,
    cutout: '70%', 
    plugins: {
      legend: {
        position: 'right',
        labels: {
          boxWidth: 12,
          padding: 16,
          usePointStyle: true,
          pointStyle: 'circle',
          color: '#6B7280'
        }
      },
      tooltip: {
        callbacks: {
          label: function(context) {
            const label = context.label || ''
            const value = context.raw || 0
            const total = context.dataset.data.reduce((a, b) => a + b, 0)
            const percentage = Math.round((value / total) * 100)
            return `${label}: ${percentage}% (${value})`
          }
        }
      }
    }
  }
  </script>
  
  <script setup>
  import { Pie } from 'vue-chartjs'
  import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement, CategoryScale } from 'chart.js'
  
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
    height: 220px;
    width: 100%;
  }
  </style>