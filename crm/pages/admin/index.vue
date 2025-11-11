<template> 
  <div class="mt-5">
    <h1 class="text-3xl font-bold text-gray-900 mb-6">Dashboard de Métricas</h1>
    
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
      <!-- Tarjeta de métrica: Total Clientes -->
      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-gray-500 text-sm">Total Clientes</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ totalClientes }}</p>
          </div>
          <div class="bg-blue-100 p-3 rounded-full">
            <svg class="w-8 h-8 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
              <path d="M9 6a3 3 0 11-6 0 3 3 0 016 0zM17 6a3 3 0 11-6 0 3 3 0 016 0zM12.93 17c.046-.327.07-.66.07-1a6.97 6.97 0 00-1.5-4.33A5 5 0 0119 16v1h-6.07zM6 11a5 5 0 015 5v1H1v-1a5 5 0 015-5z"/>
            </svg>
          </div>
        </div>
      </div>

      <!-- Tarjeta de métrica: Propiedades -->
      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-gray-500 text-sm">Propiedades</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ totalPropiedades }}</p>
          </div>
          <div class="bg-green-100 p-3 rounded-full">
            <svg class="w-8 h-8 text-green-600" fill="currentColor" viewBox="0 0 20 20">
              <path d="M10.707 2.293a1 1 0 00-1.414 0l-7 7a1 1 0 001.414 1.414L4 10.414V17a1 1 0 001 1h2a1 1 0 001-1v-2a1 1 0 011-1h2a1 1 0 011 1v2a1 1 0 001 1h2a1 1 0 001-1v-6.586l.293.293a1 1 0 001.414-1.414l-7-7z"/>
            </svg>
          </div>
        </div>
      </div>

      <!-- Tarjeta de métrica: Matches Activos -->
      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-gray-500 text-sm">Matches Activos</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ matchesActivos }}</p>
          </div>
          <div class="bg-yellow-100 p-3 rounded-full">
            <svg class="w-8 h-8 text-yellow-600" fill="currentColor" viewBox="0 0 20 20">
              <path fill-rule="evenodd" d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z" clip-rule="evenodd"/>
            </svg>
          </div>
        </div>
      </div>

      <!-- Tarjeta de métrica: Tareas Pendientes -->
      <div class="bg-white rounded-lg shadow p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-gray-500 text-sm">Tareas Pendientes</p>
            <p class="text-3xl font-bold text-gray-900 mt-2">{{ tareasPendientes }}</p>
          </div>
          <div class="bg-red-100 p-3 rounded-full">
            <svg class="w-8 h-8 text-red-600" fill="currentColor" viewBox="0 0 20 20">
              <path fill-rule="evenodd" d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z" clip-rule="evenodd"/>
            </svg>
          </div>
        </div>
      </div>
    </div>

    <!-- Gráficos -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Gráfico de Líneas -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold text-gray-900 mb-4">Clientes por Mes</h2>
        <LineChart ref="lineChartRef" />
      </div>

      <!-- Gráfico de Dona -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold text-gray-900 mb-4">Distribución de Estados</h2>
        <DonutChart />
      </div>

      <!-- Gráfico de Pastel -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold text-gray-900 mb-4">Propiedades por Tipo</h2>
        <PieChart />
      </div>

      <!-- Tabla de Actividad Reciente -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-semibold text-gray-900 mb-4">Actividad Reciente</h2>
        <div class="space-y-3">
          <div v-for="(actividad, index) in actividadesRecientes" :key="index" class="flex items-center justify-between border-b pb-2">
            <div class="flex items-center">
              <div class="w-2 h-2 rounded-full mr-3" :class="actividad.color"></div>
              <div>
                <p class="text-sm font-medium text-gray-900">{{ actividad.titulo }}</p>
                <p class="text-xs text-gray-500">{{ actividad.tiempo }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import Swal from 'sweetalert2'
import LineChart from '../../components/LineChart.vue'
import DonutChart from '../../components/DonutChart.vue'
import PieChart from '../../components/pieChart.vue'

const lineChartRef = ref(null)

// Datos de ejemplo para las métricas
const totalClientes = ref(0)
const totalPropiedades = ref(0)
const matchesActivos = ref(0)
const tareasPendientes = ref(0)

const actividadesRecientes = ref([
  { titulo: 'Nuevo cliente registrado', tiempo: 'Hace 5 minutos', color: 'bg-blue-500' },
  { titulo: 'Match realizado', tiempo: 'Hace 15 minutos', color: 'bg-green-500' },
  { titulo: 'Propiedad añadida', tiempo: 'Hace 1 hora', color: 'bg-yellow-500' },
  { titulo: 'Cita agendada', tiempo: 'Hace 2 horas', color: 'bg-purple-500' },
  { titulo: 'Tarea completada', tiempo: 'Hace 3 horas', color: 'bg-red-500' },
])

onMounted(async () => {
  // Mostrar modal de advertencia
  await Swal.fire({
    icon: 'warning',
    title: 'Módulo en Desarrollo',
    html: `
      <p class="text-gray-600">
        Este módulo de <strong>Dashboard</strong> está actualmente maquetado con datos de ejemplo.
      </p>
      <p class="text-gray-600 mt-2">
        Las métricas y gráficos mostrados <strong>no son funcionales</strong> y no reflejan datos reales.
      </p>
    `,
    confirmButtonText: 'Entendido',
    confirmButtonColor: '#3085d6',
    allowOutsideClick: false,
    allowEscapeKey: false,
  })

  // Aquí puedes cargar los datos reales desde la API
  // Por ahora usamos datos de ejemplo
  totalClientes.value = 150
  totalPropiedades.value = 85
  matchesActivos.value = 23
  tareasPendientes.value = 12
})

definePageMeta({
  requiresAuth: true,
  layout: "admin",
});
</script>
