<template>
    <div v-if="isOpen" class="modal-overlay">
      <div class="relative p-4 w-[600px] max-h-full">
        <!-- Modal content -->
        <div
          class="relative bg-white rounded-lg shadow-sm dark:bg-black max-h-[100vh] overflow-y-auto"
        >
          <!-- Modal header -->
          <div
            class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600 border-gray-200"
          >
            <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
              Detalle
            </h3>
            <button
              type="button"
              @click="closeModal"
              class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
              data-modal-toggle="crud-modal"
            >
              <svg
                class="w-3 h-3"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 14 14"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                />
              </svg>
              <span class="sr-only">Close modal</span>
            </button>
          </div>
          <!-- Modal body -->
          <form class="p-4 md:p-5">
            <div class="grid gap-4 mb-4 grid-cols-2">
              <div class="col-span-2 sm:col-span-1">
                <label
                  for="price"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Capital</label
                >
                <input
                  type="text"
                  name="capital"
                  v-model="balanceData.balanceCapital"
                  id="capital"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  readonly
                />
              </div>
              <div class="col-span-2 sm:col-span-1">
                <label
                  for="status"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >% Interes</label
                >
                <input
                  type="text"
                  name="interes"
                  id="interes"
                  v-model="balanceData.balanceInteres"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  readonly
                />
              </div>
  
              <div class="col-span-2 sm:col-span-1">
                <label
                  for="fechaInicio"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Fecha incio
                </label>
                <input
                  ttype="date"
                  name="fechaInicio"
                  id="fechaInicio"
                  :value="
                    balanceData.balanceFechaInicio
                      ? balanceData.balanceFechaInicio
                          .split('T')[0]
                          .split('-')
                          .reverse()
                          .join('/')
                      : ''
                  "
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  readonly
                />
              </div>
  
              <div class="col-span-2 sm:col-span-1">
                <label
                  for="status"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Periodo</label
                >
                <input
                  type="text"
                  name="periodo"
                  id="periodo"
                  v-model="balanceData.balancePeriodo"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                  readonly
                />
              </div>
            </div>
            <div class="grid grid-cols-1 gap-4 mb-4">
              <div class="col-span-1">
                <label
                  for="status"
                  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                  >Cuotas
                </label>
  
                <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                  <table
                    class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400"
                  >
                    <thead
                      class="text-xs text-black uppercase bg-gray-50 dark:bg-black dark:text-gray-400"
                    >
                      <tr>
                        <th scope="col" class="px-6 py-3">Fecha corte</th>
                        <th scope="col" class="px-6 py-3">%</th>
                        <th scope="col" class="px-6 py-3">Monto</th>
                        <th scope="col" class="px-6 py-3">Abono</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr
                        v-for="(pago, index) in nextPayments"
                        :key="index"
                        class="bg-white border-b dark:bg-gray-800 dark:border-black border-gray-200 hover:bg-gray-50 dark:hover:bg-gray-600"
                      >
                        <th
                          scope="row"
                          class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap dark:text-white"
                        >
                          {{ formatDate(pago) }}
                        </th>
                        <td class="px-6 py-4">{{ balanceData.balanceInteres }}</td>
                        <td class="px-6 py-4">{{ calculatePaymentAmount() }}</td>
                        <td class="px-6 py-4">
                          <select
                            id="status"
                            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                          >
                            <option selected="">Selecciona</option>
                            <option value="si">SI</option>
                            <option value="no">NO</option>
                          </select>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
            <button
              type="submit"
              class="text-white inline-flex items-center bg-black hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-gray-600 dark:hover:bg-black dark:focus:ring-gray-800"
            >
              <svg
                class="me-1 -ms-1 w-5 h-5"
                fill="currentColor"
                viewBox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  fill-rule="evenodd"
                  d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z"
                  clip-rule="evenodd"
                ></path>
              </svg>
              Actualizar registro
            </button>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, watch, computed } from "vue";
  
  const props = defineProps({
    isOpen: {
      type: Boolean,
      required: true,
    },
  
    balanceData: {
      type: Object,
      required: true,
    },
  });
  
  const emit = defineEmits(["close"]);
  
  function closeModal() {
    emit("close");
  }
  
  function getIntervalDays(periodo) {
    switch (periodo) {
      case 'semanal':
        return 7;
      case 'quincenal':
        return 15;
      case 'mensual':
        return 30;
      default:
        return 0;
    }
  }
  
  function calculateNextPayments(fechaInicio, intervaloDias, numPagos = 6) {
  const pagos = [];
  let fecha = new Date(fechaInicio);

  // Verificar si la fecha es válida
  if (isNaN(fecha.getTime())) {
    console.error("Fecha de inicio no válida:", fechaInicio);
    return pagos; // Retornar un array vacío si la fecha no es válida
  }

  for (let i = 0; i < numPagos; i++) {
    pagos.push(new Date(fecha));
    fecha.setDate(fecha.getDate() + intervaloDias);
  }

  return pagos;
}

const nextPayments = computed(() => {
  const intervaloDias = getIntervalDays(props.balanceData.balancePeriodo);
  const payments = calculateNextPayments(props.balanceData.balanceFechaInicio, intervaloDias);
  return payments || []; 
});
  
  function formatDate(date) {
    return date.toLocaleDateString('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric' });
  }
  
  function calculatePaymentAmount() {
   // Ejemplo: monto fijo de 100
  }
  </script>
  
  <style>
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 999;
  }
  </style>