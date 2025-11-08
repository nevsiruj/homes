<template>
  <div
    v-if="isOpen"
    class="modal-overlay fixed inset-0 bg-black/50 z-50 flex items-center justify-center p-4"
    aria-labelledby="modal-title"
    role="dialog"
    aria-modal="true"
  >
    <div
      class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0"
    >
      <!-- Fondo oscuro -->
      <div
        class="fixed inset-0 bg-gray-500/75 transition-opacity"
        aria-hidden="true"
        @click="closeModal"
      ></div>

      <!-- Contenido del modal -->
      <span
        class="hidden sm:inline-block sm:align-middle sm:h-screen"
        aria-hidden="true"
        >&#8203;</span
      >
      <div
        class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-4xl sm:w-full max-h-screen overflow-y-auto "
      >
        <!-- Header -->
        <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4 ">
          <div class="sm:flex sm:items-start w-full">
            <div class="mt-3 sm:mt-0 sm:flex-grow">
              <div class="flex justify-between items-center mb-4">
                <h3
                  id="modal-title"
                  class="text-lg leading-6 font-medium text-gray-900  "
                >
                  Detalles del Agente Inmobiliario
                </h3>
                <button
                  @click="closeModal"
                  type="button"
                  class="text-gray-400 hover:bg-gray-200 hover:text-gray-900 rounded-lg p-1.5 ml-auto inline-flex    "
                >
                  <svg
                    class="w-5 h-5"
                    fill="currentColor"
                    viewBox="0 0 20 20"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      fill-rule="evenodd"
                      d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 0 010-1.414z"
                      clip-rule="evenodd"
                    ></path>
                  </svg>
                </button>
              </div>

              <!-- Detalles del agente -->
              <div class="mt-4">
                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <!-- Información Personal -->
                  <div class="bg-gray-50 p-4 rounded-lg  ">
                    <h4 class="font-bold text-lg mb-4  ">Información Personal</h4>
                    <div class="space-y-3">
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Nombre completo:</span
                        >
                        <p class="  font-semibold">
                          {{ agente.nombre }} {{ agente.apellido }}
                        </p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >DNI:</span
                        >
                        <p class=" ">{{ agente.dni }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Email:</span
                        >
                        <p class=" ">{{ agente.email }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Teléfono:</span
                        >
                        <p class=" ">{{ agente.telefono }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Fecha de registro:</span
                        >
                        <p class=" ">{{ formatDate(agente.fechaIngreso) }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Rol:</span
                        >
                        <p class="  capitalize">{{ agente.rol }}</p>
                      </div>
                    </div>
                  </div>

                  <!-- Estadísticas Profesionales -->
                  <div class="bg-gray-50 p-4 rounded-lg  ">
                    <h4 class="font-bold text-lg mb-4  ">Estadísticas Profesionales</h4>
                    <div class="space-y-3">
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Especialidad:</span
                        >
                        <p class=" ">{{ agente.especialidad }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Experiencia:</span
                        >
                        <p class=" ">{{ agente.experiencia }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Propiedades vendidas:</span
                        >
                        <p class=" ">{{ agente.propiedadesVendidas || "-" }}</p>
                      </div>
                      <div>
                        <span class="text-sm font-medium text-gray-500 "
                          >Valor total vendido:</span
                        >
                        <p class=" ">{{ agente.valorVendidoTotal || "-" }}</p>
                      </div>
                    </div>
                  </div>

                  <!-- Certificaciones -->
                  <div class="bg-gray-50 p-4 rounded-lg  ">
                    <h4 class="font-bold text-lg mb-4  ">Certificaciones</h4>
                    <ul
                      v-if="agente.certificaciones && agente.certificaciones.length > 0"
                      class="list-disc pl-5 space-y-1  "
                    >
                      <li v-for="(cert, index) in agente.certificaciones" :key="index">
                        {{ cert }}
                      </li>
                    </ul>
                    <p v-else class=" ">No tiene certificaciones registradas.</p>
                  </div>

                  <!-- Clientes Atendidos -->
                  <div class="bg-gray-50 p-4 rounded-lg  ">
                    <h4 class="font-bold text-lg mb-4  ">Clientes Atendidos</h4>
                    <div
                      v-if="agente.clientesAtendidos && agente.clientesAtendidos.length > 0"
                      class="space-y-4"
                    >
                      <div
                        v-for="cliente in agente.clientesAtendidos"
                        :key="cliente.id"
                        class="p-3 border border-gray-200 rounded-md  "
                      >
                        <p class="font-medium  ">
                          {{ cliente.nombre }} ({{ cliente.tipo }})
                        </p>
                        <p class="text-sm text-gray-600 ">
                          Propiedades:
                          {{ cliente.propiedades.join(", ") || "Ninguna" }}
                        </p>
                      </div>
                    </div>
                    <p v-else class=" ">No hay clientes atendidos registrados.</p>
                  </div>
                </div>

                <!-- Notas -->
                <div class="mt-6 bg-gray-50 p-4 rounded-lg  ">
                  <h4 class="font-bold text-lg mb-2  ">Notas</h4>
                  <p class=" ">{{ agente.notas || "Sin notas adicionales." }}</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Botón de cierre -->
          <div
            class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse  "
          >
            <button
              type="button"
              @click="closeModal"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm      "
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps, defineEmits } from "vue";

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true
  },
  agente: {
    type: Object,
    default: () => ({
      nombre: "",
      apellido: "",
      dni: "",
      telefono: "",
      email: "",
      especialidad: "",
      experiencia: "",
      propiedadesVendidas: 0,
      valorVendidoTotal: "",
      fechaIngreso: "",
      rol: "",
      certificaciones: [],
      clientesAtendidos: [],
      notas: ""
    })
  }
});

const emit = defineEmits(["close"]);

function closeModal() {
  emit('close');
}

// === Formatear fecha ===
function formatDate(dateString) {
  if (!dateString) return "Fecha inválida";

  const date = new Date(dateString);
  if (isNaN(date.getTime())) return "Fecha inválida";

  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0"); // Mes empieza en 0
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
}
</script>

<style scoped>
.modal-overlay {
  backdrop-filter: blur(2px);
}
</style>