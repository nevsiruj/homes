<template>
  <div v-if="isOpenRg" class="modal-overlay overflow-y-auto">
    <div class="relative p-4 w-full max-w-md max-h-full">
      <!-- Modal content -->
      <div class="relative bg-white rounded-lg shadow-sm dark:bg-black">
        <!-- Modal header -->
        <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600 border-gray-200">
          <h3 class="text-lg font-semibold text-gray-900 dark:text-white">Agregar Registro</h3>
          <button
            type="button"
            @click="closeModalRg"
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
        <form class="p-4 md:p-5" @submit.prevent="submitForm">
          <div class="grid gap-4 mb-4 grid-cols-2">
            <!-- Campo de búsqueda por DNI -->
            <div class="col-span-2">
              <label for="dni" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Buscar cliente por DNI</label>
              <input
                type="text"
                id="dni"
                v-model="searchDni"
                @input="searchClientByDni"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Ingrese DNI"
              />
            </div>

            <!-- Campo para mostrar el nombre del cliente -->
            <div class="col-span-2">
              <label for="name" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Nombre del cliente</label>
              <input
                type="text"
                id="name"
                v-model="clienteNombre"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Nombre del cliente"
                readonly
              />
            </div>

            <!-- Resto de los campos del formulario -->
            <div class="col-span-2 sm:col-span-1">
              <label for="periodo" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Periodo</label>
              <select
                id="BalancePeriodo"
                v-model="BalancePeriodo"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
              >
                <option selected>Selecciona</option>
                <option value="Semanal">Semanal</option>
                <option value="Quincenal">Quincenal</option>
                <option value="Mensual">Mensual</option>
              </select>
            </div>

            <div class="col-span-2 sm:col-span-1">
              <label for="fechaInicio" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Inicia el</label>
              <input
                type="date"
                id="fechaInicio"
                v-model="BalanceFechaInicio"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Fecha"
                required
              />
            </div>

            <div class="col-span-2 sm:col-span-1">
              <label for="interes" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">% Interes</label>
              <select
                id="interes"
                v-model="BalanceInteres"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
              >
                <option selected>Selecciona</option>
                <option v-for="i in 100" :key="i" :value="i">{{ i }}</option>
              </select>
            </div>

            <div class="col-span-2 sm:col-span-1">
              <label for="monto" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Monto</label>
              <input
                type="text"
                id="monto"
                v-model="BalanceCapital"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Monto"
                required
              />
            </div>

            <div class="col-span-2 sm:col-span-1">
              <label for="garantia" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Tiene garantia</label>
              <select
                id="garantia"
                v-model="BalanceGarantia"
                @change="handleGarantiaChange"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-500 focus:border-primary-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
              >
                <option selected>Selecciona</option>
                <option value="SI">SI</option>
                <option value="NO">NO</option>
              </select>
            </div>

            <!-- Campos adicionales para garantía -->
            <div v-if="tieneGarantia === 'SI'" class="col-span-2">
              <h3 class="text-lg font-semibold text-gray-900 dark:text-white">Agregar Datos</h3>
            </div>
            <div v-if="tieneGarantia === 'SI'" class="col-span-2">
              <label for="nombreGarantia" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Nombre y apellido</label>
              <input
                type="text"
                id="nombreGarantia"
                v-model="BalanceNombreFiador"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Nombre y apellido"
                required
              />
            </div>
            <div v-if="tieneGarantia === 'SI'" class="col-span-2 sm:col-span-1">
              <label for="dniGarantia" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">DNI</label>
              <input
                type="text"
                id="dniGarantia"
                v-model="BalanceDniFiador"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="DNI"
                required
              />
            </div>
            <div v-if="tieneGarantia === 'SI'" class="col-span-2 sm:col-span-1">
              <label for="telefonoGarantia" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Telefono</label>
              <input
                type="text"
                id="telefonoGarantia"
                v-model="BalanceTelefonoFiador"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500"
                placeholder="Telefono"
                required
              />
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
            Agregar
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import clienteService from '../services/clienteServices.js';
import balanceService from '../services/balanceServices.js'

const props = defineProps({
  isOpenRg: {
    type: Boolean,
    required: true,
  },
});

const tieneGarantia = ref("NO");

function handleGarantiaChange(event) {
  tieneGarantia.value = event.target.value;
}

const emit = defineEmits(["close"]);

//Busque de cliente
const searchDni = ref(''); 
const clientes = ref([]); 
const clienteNombre = ref('');

//Para guardar
const BalanceDni = ref(''); // DNI del cliente encontrado
const BalanceNombre = ref(''); // Nombre del cliente encontrado
const BalancePeriodo = ref('');
const BalanceFechaInicio = ref('');
const BalanceInteres = ref('');
const BalanceCapital = ref('');
const BalanceGarantia = ref('');
const BalanceNombreFiador = ref('');
const BalanceDniFiador = ref('');
const BalanceTelefonoFiador = ref('');

// lista de clientes 
onMounted(async () => {
  try {
    clientes.value = await clienteService.getClientes();
    console.log('Clientes cargados:', clientes.value); 
  } catch (error) {
    console.error('Error al obtener clientes:', error);
  }
});

// Método para buscar cliente por DNI
const searchClientByDni = () => {
  if (searchDni.value.trim().length < 1) {
    clienteNombre.value = ''; 
    BalanceDni.value = ''; // Limpiar DNI
    BalanceNombre.value = ''; // Limpiar nombre
    return; 
  }

  const dniBuscado = searchDni.value.toString().trim();
  const clientesEncontrados = clientes.value.filter(cliente =>
    cliente.clienteDni.toString().includes(dniBuscado)
  );

  if (clientesEncontrados.length > 0) {
    const cliente = clientesEncontrados[0]; // Tomar el primer cliente encontrado
    clienteNombre.value = cliente.clienteNombre; 
    BalanceDni.value = cliente.clienteDni; // Asignar DNI encontrado
    BalanceNombre.value = cliente.clienteNombre; // Asignar nombre encontrado
    console.log('Cliente encontrado:', cliente);
  } else {
    clienteNombre.value = ''; 
    BalanceDni.value = ''; // Limpiar DNI
    BalanceNombre.value = ''; // Limpiar nombre
    alert('Cliente no encontrado'); 
    searchDni.value = '';
  }
};

function closeModalRg() {
  emit("close");
}

const submitForm = async () => {
  const balanceData = {
    clienteDni: BalanceDni.value,
    clienteNombre: BalanceNombre.value,
    BalancePeriodo: BalancePeriodo.value,
    BalanceFechaInicio: BalanceFechaInicio.value,
    BalanceInteres: BalanceInteres.value,
    BalanceCapital: BalanceCapital.value,
    BalanceGarantia: BalanceGarantia.value,
    tieneGarantia: tieneGarantia.value,
    BalanceDni: BalanceDni.value,
    BalanceNombre: BalanceNombre.value,
  };

  if (tieneGarantia.value === "SI") {
    balanceData.BalanceGarantia = BalanceGarantia.value;
    balanceData.BalanceNombreFiador = BalanceNombreFiador.value;
    balanceData.BalanceDniFiador = BalanceDniFiador.value;
    balanceData.BalanceTelefonoFiador = BalanceTelefonoFiador.value;
  } else {
    balanceData.BalanceNombreFiador = "N/A";
    balanceData.BalanceDniFiador = "N/A";
    balanceData.BalanceTelefonoFiador = "N/A";
  }

  try {
    const response = await balanceService.createBalance(balanceData);
    console.log('Balance creado:', response);

    emit('record-added');

    // Cerrar el modal
    closeModalRg();
  } catch (error) {
    console.error('Error al crear el balance:', error);
  }
};
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