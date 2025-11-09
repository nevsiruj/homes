<template>
  <div class="p-4">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold text-gray-800">
        Bienvenido, {{ nombreAgente }}
      </h1>
    </div>

    <div
      class="flex flex-wrap md:flex-nowrap items-center justify-between gap-4 mt-6 mb-6 w-full"
    >
      <div class="flex flex-wrap items-center gap-2 w-full lg:w-auto">
        <select
          v-if="isAdmin"
          v-model="selectedAgent"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2 w-full sm:w-auto flex-shrink-0"
        >
          <option value="all">Todos los agentes</option>
          <option
            v-for="agente in agentesArray"
            :key="agente.id"
            :value="agente.id"
          >
            {{ agente.nombre }} {{ agente.apellido }}
          </option>
        </select>

        <div class="relative w-full sm:flex-1 lg:max-w-xs">
          <input
            v-model="searchTerm"
            type="text"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2"
            :placeholder="searchPlaceholder"
          />
        </div>

        <input
          v-model="dateFilter.startDate"
          type="date"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 px-2 py-1.5 w-[calc(50%-4px)] sm:w-auto flex-shrink-0"
          placeholder="Fecha inicio"
        />
        <input
          v-model="dateFilter.endDate"
          type="date"
          class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 px-2 py-1.5 w-[calc(50%-4px)] sm:w-auto flex-shrink-0"
          placeholder="Fecha fin"
        />
      </div>

      <div class="flex justify-center w-full md:w-auto md:justify-end">
        <button
          @click="openAddModal"
          class="px-4 py-1.5 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition-colors text-sm font-medium w-full sm:w-auto"
        >
          Agregar Cliente
        </button>
      </div>
    </div>

    <Loader v-if="isLoading" class="my-8" />

    <div v-else class="flex overflow-x-auto gap-4 pb-4">
      <div
        v-for="column in columns"
        :key="column.estado"
        class="min-w-[300px] bg-gray-50 rounded-lg p-3 border-t-4"
        :class="column.border"
      >
        <h3 class="font-semibold">{{ column.title }}</h3>
        <draggable
          :list="getFilteredTasksByStatus(column.estado)"
          group="tasks"
          @change="(evt) => handleTaskMove(evt, column.estado)"
          item-key="id"
          class="min-h-[100%] space-y-2 mt-2"
        >
          <template #item="{ element }">
            <TaskCard :task="element" :agentes-map="agentesMap" @open-contact="openContactModal" />
          </template>
        </draggable>
      </div>
    </div>
  </div>

  <!-- Modal de Contacto - Movido fuera del contenedor principal -->
  <div
    id="contactModal"
    tabindex="-1"
    aria-hidden="true"
    class="fixed inset-0 bg-[#0000002b] bg-opacity-50 items-center justify-center p-4 z-50"
    :class="selectedTask ? 'flex' : 'hidden'"
  >
    <div
      class="relative w-full max-w-6xl bg-white rounded-lg shadow max-h-[90vh] overflow-hidden"
    >
      <!-- Header -->
      <div
        class="flex items-center justify-between p-4 md:p-5 border-b sticky top-0 bg-white z-10"
      >
        <h3 class="text-xl font-semibold">
          Contacto con {{ selectedTask?.Nombre || selectedTask?.nombre }}
        </h3>
        <button
          @click="closeContactModal"
          type="button"
          class="text-gray-700 border border-gray-300 py-1 px-2 hover:text-gray-900 rounded-md"
        >
          X
        </button>
      </div>

      <!-- Contenido principal con scroll -->
      <div class="p-4 space-y-4 max-h-[calc(90vh-120px)] overflow-y-auto">
        <!-- Loader mientras se cargan los datos -->
        <div v-if="isLoadingDetails" class="relative min-h-[200px]">
          <div class="absolute inset-0">
            <Loader class="!absolute !w-full !h-full" />
          </div>
        </div>

        <div v-else-if="clientDetails" class="space-y-6">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <!-- Información Personal -->
            <div class="bg-gray-50 p-4 rounded-lg">
              <h4 class="font-bold text-lg mb-4">Información Personal</h4>
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Nombre Completo:</span
                  >
                  <p class="">
                    {{ clientDetails.nombre }} {{ clientDetails.apellido }}
                  </p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Teléfono:</span
                  >
                  <p class="">{{ clientDetails.telefono || "N/A" }}</p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Email:</span
                  >
                  <p class="">{{ clientDetails.email || "N/A" }}</p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Fecha de Registro:</span
                  >
                  <p class="">
                    {{ formatDate(clientDetails.fechaRegistro) || "N/A" }}
                  </p>
                </div>
              </div>
            </div>

            <!-- Preferencias -->
            <div class="bg-gray-50 p-4 rounded-lg">
              <h4 class="font-bold text-lg mb-4">Preferencias</h4>
              <div
                v-if="preferencias"
                class="grid grid-cols-2 md:grid-cols-3 gap-4"
              >
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Tipo Inmueble:</span
                  >
                  <p class="">{{ preferencias.tipo || "N/A" }}</p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Operación:</span
                  >
                  <p class="">{{ preferencias.operacion || "N/A" }}</p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Precio Mínimo:</span
                  >
                  <p class="">
                    {{
                      preferencias.precioMin != null
                        ? `$${Number(
                            preferencias.precioMin
                          ).toLocaleString()}`
                        : "N/A"
                    }}
                  </p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Precio Máximo:</span
                  >
                  <p class="">
                    {{
                      preferencias.precioMax != null
                        ? `$${Number(
                            preferencias.precioMax
                          ).toLocaleString()}`
                        : "N/A"
                    }}
                  </p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Habitaciones:</span
                  >
                  <p class="">{{ preferencias.habitaciones ?? "N/A" }}</p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Baños:</span
                  >
                  <p class="">{{ preferencias.banos ?? "N/A" }}</p>
                </div>
                <div>
                  <span class="text-sm font-medium text-gray-500"
                    >Ubicaciones:</span
                  >
                  <p class="">{{ preferencias.ubicacion ?? "N/A" }}</p>
                </div>
              </div>
              <p v-else class="text-gray-500">
                No hay preferencias cargadas.
              </p>
            </div>
          </div>

          <MatchPendiente 
            v-if="selectedTask?.clienteId" 
            :clienteId="selectedTask.clienteId" 
            :clientDetails="clientDetails"
            @open-inmueble="handleOpenInmueble"
          />

          <div class="bg-gray-50 p-4 rounded-lg">
            <div class="flex items-center justify-between mb-4">
              <h4 class="font-bold text-lg">
                Historial de seguimiento
                <!-- ({{
      selectedTask?.Interacciones?.length || 0
    }}) -->
              </h4>

              <button
                @click="loadClientDetails"
                class="text-xs px-2 py-1 rounded border border-gray-300 bg-white hover:bg-gray-100"
              >
                Refrescar
              </button>
            </div>

            <div
              v-if="!selectedTask?.Interacciones?.length"
              class="text-sm text-gray-500"
            >
              No hay interacciones registradas.
            </div>

            <div v-else class="relative overflow-x-auto">
              <table class="w-full text-sm text-left text-gray-500">
                <thead class="bg-gray-100">
                  <tr>
                    <th class="px-4 py-2">Fecha</th>
                    <th class="px-4 py-2">Tipo</th>
                    <th class="px-4 py-2">Descripción</th>
                    <th class="px-4 py-2">Estado</th>
                    <th class="px-4 py-2">Próximo Contacto</th>
                  </tr>
                </thead>
                <tbody class="divide-y divide-gray-200">
                  <tr
                    v-for="(
                      interaccion, index
                    ) in selectedTask.Interacciones.slice().reverse()"
                    :key="index"
                    class="odd:bg-white even:bg-gray-50 border-b border-gray-200"
                  >
                    <td class="px-4 py-2">
                      {{
                        formatDateTime(interaccion.Fecha || interaccion.fecha)
                      }}
                    </td>
                    <td class="px-4 py-2">
                      {{
                        interaccion.Tipo || interaccion.tipo || "Interacción"
                      }}
                    </td>
                    <td class="px-4 py-3 text-sm text-gray-900 max-w-xs">
                      <div class="line-clamp-2">
                        {{
                          interaccion.Descripcion ||
                          interaccion.descripcion ||
                          "Sin descripción"
                        }}
                      </div>
                    </td>
                    <td class="px-4 py-3 text-sm text-gray-900">
                      {{
                        formatStatusForDisplay(
                          interaccion.StatusInteraccion ||
                            interaccion.statusInteraccion
                        )
                      }}
                    </td>
                    <td class="px-4 py-2">
                      <span
                        v-if="
                          interaccion.FechaVencimiento ||
                          interaccion.fechaVencimiento
                        "
                      >
                        {{
                          formatDateTime(
                            interaccion.FechaVencimiento ||
                              interaccion.fechaVencimiento
                          )
                        }}
                      </span>
                      <span v-else class="text-gray-400">-</span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>



          <!-- Formulario de Interacción -->
          <div class="bg-gray-50 p-4 rounded-lg">
            <h4 class="font-bold text-lg mb-4">Registrar Interacción</h4>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Tipo de Contacto</label
                >
                <select
                  v-model="contactData.tipo"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                >
                  <option value="llamada">Llamada</option>
                  <option value="mensaje">Mensaje</option>
                  <option value="email">Email</option>
                  <option value="visita">Visita</option>
                </select>
              </div>

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Estado</label
                >
                <select
                  v-model="currentRecordatorio.estadoInteraccionRaw"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                >
                  <option
                    v-for="opt in statusOptions"
                    :key="opt.key"
                    :value="opt.key"
                  >
                    {{ opt.title }}
                  </option>
                </select>
              </div>

              <div class="md:col-span-2">
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Notas</label
                >
                <textarea
                  v-model="contactData.nota"
                  rows="3"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                  placeholder="Agregue notas de la interacción..."
                ></textarea>
              </div>

              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Fecha próximo contacto</label
                >
                <input
                  v-model="contactData.fechaVencimiento"
                  type="datetime-local"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div
        class="flex items-center justify-end p-4 border-t sticky bottom-0 bg-white z-10"
      >
        <button
          @click="closeContactModal"
          type="button"
          class="text-gray-500 bg-white hover:bg-gray-100 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 mr-2"
        >
          Cancelar
        </button>
        <button
          @click="saveContact"
          type="button"
          class="text-white bg-gray-600 hover:bg-gray-700 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5"
        >
          Guardar
        </button>
      </div>
    </div>
  </div>

  <!-- <AgregarClienteModal
    :isOpen="showAddModal"
    @close="showAddModal = false"
    @client-added="handleClientAdded"
  /> -->
  <AgregarClienteModal
    :isOpen="showAddModal"
    @close="showAddModal = false"
    @client-added="onClientSaved"
    @client-updated="onClientSaved"
  />

  <!-- Modal de detalle inmueble - Movido fuera para evitar conflictos -->
  <DetalleInmuebleModal 
    v-if="showInmuebleModal" 
    :isOpen="showInmuebleModal"
    :inmueble="selectedInmuebleData"
    @close="closeInmuebleModal"
  />

  <!-- Modal de Sesión Expirada -->
  <div v-if="isSessionExpired" class="fixed inset-0 z-50">
    <div class="absolute inset-0 bg-gray-800/50"></div>
    <!-- <div class="relative flex items-center justify-center min-h-full p-4">
      <div class="bg-white rounded-lg shadow p-6 max-w-sm w-full">
        <h2 class="text-xl font-bold text-gray-800 mb-2">Sesión expirada</h2>
        <p class="text-gray-600">
          Tu sesión ha caducado o no existe un token activo. Inicia sesión
          nuevamente para continuar.
        </p>
        <p class="text-xs text-gray-500 mt-3">
          Serás redirigido en {{ redirectCountdown }}s o pulsa "Iniciar sesión".
        </p>
        <div class="flex justify-end mt-6">
          <button
            @click="redirectToLogin"
            class="bg-gray-700 text-white px-4 py-2 rounded-lg hover:bg-gray-800"
          >
            Iniciar sesión
          </button>
        </div>
      </div>
    </div> -->
  </div>
</template>

<script>
import draggable from "vuedraggable";
import TaskCard from "../bandejaTarea/taskcard.vue";
import AgregarClienteModal from "../agregarClienteModal.vue";
import DetalleInmuebleModal from "../modalDetalleInmueble.vue";
import MatchPendiente from "../matchPendiente.vue";
import { Modal } from "flowbite";
import Swal from "sweetalert2";
import inmuebleService from "@/services/inmuebleService";

// Servicios
import clienteService from "@/services/clienteService";
import agenteService from "@/services/agenteService";
import interaccionService from "@/services/interaccionService";

// Constantes
import { statusMap } from "../../api/constants";

// (opcional) store para roles si lo tienes
import { useAuthStore } from "@/stores/auth";

export default {
  components: {
    draggable,
    TaskCard,
    AgregarClienteModal,
    DetalleInmuebleModal,
    MatchPendiente,
  },
  data() {
    return {
      showInmuebleModal: false,
      selectedInmuebleData: null,
      searchTerm: "", // Término de búsqueda
      searchFilter: "all", // Filtro de búsqueda: all, person, agent
      showAddModal: false,
      contactModal: null,
      selectedTask: null,
      isLoading: false, // Loader principal para carga de tareas
      isLoadingDetails: false, // Nuevo: para el loader de detalles
      clientDetails: null, // Nuevo: para datos generales del cliente
      preferencias: null, // Nuevo: para Preferencias del cliente
      pagination: {
        currentPage: 1,
        perPage: 7,
        total: 0,
      },
      contactData: {
        tipo: "llamada",
        nota: "",
        fechaVencimiento: "",
      },
      currentRecordatorio: { estadoInteraccionRaw: null }, // <- agregado
      nombreAgente: "Cargando...",
      agenteId: null,
      isAdmin: false, // <- NUEVO
      originalStatus: null, // <- nuevo: guarda el status previo para poder revertir
      columns: [
        {
          title: "Contacto Inicial Pendiente",
          estado: "contacto_inicial_pendiente",
          border: "border-gray-500",
        },
        {
          title: "Seguimiento 24h",
          estado: "seguimiento_24h",
          border: "border-yellow-500",
        },
        {
          title: "Agendó Cita",
          estado: "agendo_cita",
          border: "border-green-500",
        },
        {
          title: "Negociación",
          estado: "negociacion",
          border: "border-green-500",
        },
        {
          title: "Cierre en proceso",
          estado: "cierre_proceso",
          border: "border-green-500",
        },
        {
          title: "Cierre exitoso",
          estado: "cierre_exitoso",
          border: "border-green-500",
        },
        {
          title: "Seguimiento a Largo Plazo",
          estado: "seguimiento_largo_plazo",
          border: "border-yellow-500",
        },
        {
          title: "Descartado",
          estado: "descartado",
          border: "border-yellow-500",
        },
      ],
      tasks: [],
      agentesArray: [], // <- nuevo arreglo para todos los agentes
      agentesMap: new Map(), // <- Map para acceso rápido a agentes por ID
      selectedAgent: "all", // <- nuevo dato para el agente seleccionado
      dateFilter: {
        startDate: "",
        endDate: "",
      },
      isSessionExpired: false,
      sessionCheckTimer: null,
      redirectCountdown: 10,
      autoRedirectTimer: null,
    };
    
  },
  

  async mounted() {
    try {
      const userData = await agenteService.getCurrentUser();
      const agente = userData?.$values ? userData.$values[0] : userData;

      if (!agente) throw new Error("Datos incompletos del usuario");

      const nombre = agente.Nombre || agente.nombre || "";
      const apellido = agente.Apellido || agente.apellido || "";

      this.nombreAgente = `${nombre} ${apellido}`;
      this.agenteId =
        agente.Id || agente.id || agente.AgenteId || agente.usuarioId || null;

      // Detectar rol admin
      try {
        const auth = useAuthStore?.();
        const storeIsAdmin =
          !!auth &&
          ((typeof auth.hasRole === "function" && auth.hasRole("admin")) ||
            (Array.isArray(auth.roles) &&
              auth.roles
                .map((r) => String(r).toLowerCase())
                .includes("admin")));

        const apiRoles = (agente.Roles || agente.roles || []).map((r) =>
          String(r).toLowerCase()
        );
        const apiSingle = String(agente.Rol || agente.rol || "").toLowerCase();

        this.isAdmin =
          storeIsAdmin || apiRoles.includes("admin") || apiSingle === "admin";
      } catch {
        const apiRoles = (agente.Roles || agente.roles || []).map((r) =>
          String(r).toLowerCase()
        );
        const apiSingle = String(agente.Rol || agente.rol || "").toLowerCase();
        this.isAdmin = apiRoles.includes("admin") || apiSingle === "admin";
      }

      if (!this.agenteId) throw new Error("No se pudo obtener el AgenteId");

      // Cargar datos iniciales
      await this.cargarTodosLosClientes();
    } catch (error) {
      console.error("Error en mounted:", error);
      this.nombreAgente =
        "La sesión ha caducado. Por favor, inicie sesión de nuevo.";
      this.isSessionExpired = true;
      this.startAutoRedirectCountdown();
      return;
    }

    // Inicialización del modal
    const modalElement = document.getElementById("contactModal");
    if (modalElement) {
      this.contactModal = new Modal(modalElement, {
        backdrop: 'static',
        keyboard: false,
        closable: true
      });
      console.log("✅ Modal inicializado correctamente");
    } else {
      console.error("❌ No se encontró el elemento contactModal");
    }

    this.checkNotifications();

    // Verificar sesión y suscribirse a cambios
    this.checkSession();
    this.sessionCheckTimer = setInterval(this.checkSession, 30000);
    window.addEventListener("storage", this.onStorage);
  },
  beforeUnmount() {
    if (this.sessionCheckTimer) clearInterval(this.sessionCheckTimer);
    if (this.autoRedirectTimer) clearInterval(this.autoRedirectTimer);
    window.removeEventListener("storage", this.onStorage);
  },

  methods: {
    async handleOpenInmueble(payload) {
      console.log("🔄 [TASK] Padre recibió open-inmueble:", payload);
      console.log("🔍 [TASK] Tipo de payload.value:", typeof payload.value);
      console.log("🔍 [TASK] Contenido de payload.value:", payload.value);
      console.log("🔍 [TASK] showInmuebleModal ANTES:", this.showInmuebleModal);
      
      // Obtener el ID del inmueble
      const inmuebleId = payload.value?.inmuebleId || 
                        payload.value?.id || 
                        payload.value?.propiedadId ||
                        payload.value;
      
      console.log("🆔 [TASK] ID del inmueble a cargar:", inmuebleId);
      
      if (!inmuebleId) {
        console.error("❌ [TASK] No se pudo obtener el ID del inmueble");
        Swal.fire({
          icon: "error",
          title: "Error",
          text: "No se pudo obtener la información del inmueble",
        });
        return;
      }

      try {
        // Abrir el modal primero con datos básicos
        this.selectedInmuebleData = {
          id: inmuebleId,
          titulo: "Cargando...",
          codigoPropiedad: `PROP-${inmuebleId}`,
          precio: 0,
          habitaciones: 0,
          banos: 0,
          parqueos: 0,
          metrosCuadrados: 0,
          imagenPrincipal: "",
          contenido: "",
          tipos: "",
          operaciones: "",
          ubicaciones: "",
          luxury: false,
          video: "",
          amenidades: [],
          imagenesReferencia: [],
          // Incluir datos del payload si están disponibles
          ...payload.value,
        };
        
        this.showInmuebleModal = true;
        console.log("🎯 [TASK] Modal abierto, cargando datos completos...");
        
        // Cargar datos completos del inmueble desde el backend
        const inmuebleCompleto = await inmuebleService.getInmuebleById(inmuebleId);
        
        if (inmuebleCompleto) {
          console.log("✅ [TASK] Datos completos del inmueble cargados:", inmuebleCompleto);
          
          // Normalizar amenidades
          let amenidades = [];
          if (inmuebleCompleto.amenidades) {
            if (Array.isArray(inmuebleCompleto.amenidades)) {
              amenidades = inmuebleCompleto.amenidades;
            } else if (inmuebleCompleto.amenidades.$values) {
              amenidades = inmuebleCompleto.amenidades.$values;
            }
          }
          
          // Normalizar imágenes de referencia
          let imagenesReferencia = [];
          if (inmuebleCompleto.imagenesReferencia) {
            if (Array.isArray(inmuebleCompleto.imagenesReferencia)) {
              imagenesReferencia = inmuebleCompleto.imagenesReferencia;
            } else if (inmuebleCompleto.imagenesReferencia.$values) {
              imagenesReferencia = inmuebleCompleto.imagenesReferencia.$values;
            }
          }
          
          console.log("🖼️ [TASK] Imágenes de referencia encontradas:", imagenesReferencia);
          console.log("🖼️ [TASK] Cantidad de imágenes:", imagenesReferencia.length);
          if (imagenesReferencia.length > 0) {
            console.log("🖼️ [TASK] Primera imagen:", imagenesReferencia[0]);
          }
          
          // Actualizar con datos completos
          this.selectedInmuebleData = {
            id: inmuebleCompleto.id || inmuebleId,
            codigoPropiedad: inmuebleCompleto.codigoPropiedad || `PROP-${inmuebleId}`,
            titulo: inmuebleCompleto.titulo || inmuebleCompleto.nombre || "Sin título",
            precio: inmuebleCompleto.precio || 0,
            habitaciones: inmuebleCompleto.habitaciones || 0,
            banos: inmuebleCompleto.banos || 0,
            parqueos: inmuebleCompleto.parqueos || 0,
            metrosCuadrados: inmuebleCompleto.metrosCuadrados || 0,
            imagenPrincipal: inmuebleCompleto.imagenPrincipal || "",
            contenido: inmuebleCompleto.contenido || inmuebleCompleto.descripcion || "",
            tipos: inmuebleCompleto.tipos || inmuebleCompleto.tipoPropiedad || "",
            operaciones: inmuebleCompleto.operaciones || inmuebleCompleto.tipoOperacion || "",
            ubicaciones: inmuebleCompleto.ubicaciones || inmuebleCompleto.ubicacion || "",
            luxury: inmuebleCompleto.luxury || false,
            video: inmuebleCompleto.video || "",
            amenidades: amenidades,
            imagenesReferencia: imagenesReferencia,
          };
          
          console.log("🎯 [TASK] Datos del modal actualizados con información completa");
        } else {
          console.log("⚠️ [TASK] No se pudieron cargar los datos completos, usando datos básicos");
        }
        
      } catch (error) {
        console.error("❌ [TASK] Error cargando datos del inmueble:", error);
        // El modal ya está abierto con datos básicos, solo mostrar aviso
        Swal.fire({
          icon: "warning",
          title: "Información limitada",
          text: "Se mostrará información básica del inmueble",
          timer: 2000,
        });
      }
      
      console.log("🔍 [TASK] showInmuebleModal DESPUÉS:", this.showInmuebleModal);
    },
    closeInmuebleModal() {
      console.log("🔄 [TASK] Cerrando modal de inmueble");
      console.log("🔍 [TASK] showInmuebleModal ANTES:", this.showInmuebleModal);
      
      this.showInmuebleModal = false;
      this.selectedInmuebleData = null;
      
      console.log("🔍 [TASK] showInmuebleModal DESPUÉS:", this.showInmuebleModal);
      console.log("🔍 [TASK] selectedInmuebleData DESPUÉS:", this.selectedInmuebleData);
    },

    // Métodos de paginación
    goToPage(page) {
      if (page >= 1 && page <= this.totalPages) {
        this.pagination.currentPage = page;
      }
    },

    nextPage() {
      if (this.pagination.currentPage < this.totalPages) {
        this.pagination.currentPage++;
      }
    },

    prevPage() {
      if (this.pagination.currentPage > 1) {
        this.pagination.currentPage--;
      }
    },

    openAddModal() {
      this.showAddModal = true;
    },
    // Helper para búsqueda acento-insensible y lowercase (igual que en recordatorios)
    normalizeSearch(str) {
      if (!str && str !== 0) return "";
      return String(str)
        .normalize("NFD")
        .replace(/[\u0300-\u036f]/g, "")
        .toLowerCase()
        .trim();
    },
    parseFilterDate(str, end = false) {
      if (!str) return null;
      const [y, m, d] = str.split("-").map(Number);
      return end
        ? new Date(y, m - 1, d, 23, 59, 59, 999)
        : new Date(y, m - 1, d, 0, 0, 0, 0);
    },
    getTasksByStatus(estado) {
      return this.tasks.filter((task) => task.status === estado);
    },
    getFilteredTasksByStatus(estado) {
      return this.filteredTasks
        .filter((task) => task.status === estado)
        .sort((a, b) => new Date(b.fechaRegistro) - new Date(a.fechaRegistro)); // Ordenar por fecha de registro (más reciente primero)
    },
    async handleTaskMove(evt, newEstado) {
      if (evt.added) {
        const task = evt.added.element;
        task.status = newEstado; // Actualizar el estado en el objeto de la tarea
        // Usar openContactModal para mantener la consistencia en la carga de datos
        await this.openContactModal(task);
        // Actualizar datos específicos del movimiento después de cargar los detalles
        this.contactData.tipo = "movimiento";
        this.contactData.nota = `Movido a: ${newEstado}`;
        this.contactData.fechaVencimiento = "";
      }
    },
    updateTaskStatus(taskId, newEstado) {
      const task = this.tasks.find((t) => t.id === taskId);
      if (task) {
        task.status = newEstado;
        this.scheduleNotification(task);
      }
    },
    async openContactModal(task) {
      console.log("🔄 [1] INICIANDO openContactModal para cliente:", task.id);
      console.log("🔄 [1] Task recibido completo:", task);
      console.log("🔄 [1] Estado de la tarea:", task.status);
      console.log("🔄 [1] Columna desde la que proviene:", task.status);

      // Verificar si ya hay un modal abierto
      if (this.selectedTask) {
        console.log("⚠️ [1] Ya hay una tarea seleccionada, cerrando modal anterior");
      }

      // SOLUCIÓN RADICAL: Resetear completamente el estado
      await this.resetModalState();

      this.originalStatus = task.status;
      this.selectedTask = { ...task };
      this.selectedTask.clienteId = task.clienteId || task.id;

      console.log("🔄 [2] SelectedTask configurado:", this.selectedTask.id);
      console.log("🔄 [2] ClienteId asignado:", this.selectedTask.clienteId);

      // Configurar datos básicos del modal
      this.selectedTask.Interacciones = Array.isArray(task.Interacciones)
        ? task.Interacciones
        : [];

      const statusKey = Object.keys(statusMap).find(
        (k) => statusMap[k] === this.selectedTask.status
      );
      this.currentRecordatorio.estadoInteraccionRaw = statusKey
        ? parseInt(statusKey)
        : null;

      if (this.selectedTask.Interacciones.length) {
        const last =
          this.selectedTask.Interacciones[
            this.selectedTask.Interacciones.length - 1
          ];
        this.contactData.tipo = last.Tipo || "llamada";
        this.contactData.nota = last.Descripcion || "";
        this.contactData.fechaVencimiento = last.FechaVencimiento
          ? new Date(last.FechaVencimiento).toISOString().slice(0, 16)
          : "";
      } else {
        this.contactData = {
          tipo: "llamada",
          nota: this.selectedTask.nota || this.selectedTask.Notas || "",
          fechaVencimiento: "",
        };
      }

      console.log("🔄 [3] Abriendo modal...");
      console.log("🔄 [3] Estado del contactModal:", this.contactModal);
      console.log("🔄 [3] Elemento DOM del modal:", document.getElementById("contactModal"));

      // ABRIR EL MODAL
      if (this.contactModal) {
        this.contactModal.show();
        console.log("✅ [3] Modal.show() ejecutado");
      } else {
        console.error("❌ [3] contactModal no está disponible");
      }

      console.log("🔄 [4] Modal abierto, cargando detalles...");

      // CARGAR DETALLES
      await this.loadClientDetails();

      console.log("🔄 [5] openContactModal COMPLETADO");
    },

    // Función simple para cargar detalles del cliente (sin matches)
    async loadClientDetails() {
      if (!this.selectedTask?.clienteId) {
        console.error("❌ No hay clienteId para cargar detalles");
        return;
      }

      this.isLoadingDetails = true;
      try {
        const clienteId = this.selectedTask.clienteId;
        console.log("🔄 Cargando detalles básicos del cliente:", clienteId);

        // Cargar cliente y preferencias
        const clientFullResponse = await clienteService.getClienteById(clienteId);

        // Normalizar la respuesta del cliente
        this.clientDetails = clientFullResponse?.$values
          ? clientFullResponse.$values[0]
          : clientFullResponse;

        console.log("👤 Cliente cargado:", this.clientDetails?.nombre);

        // Extraer preferencias
        const preferenciasArray =
          this.clientDetails?.preferencias?.$values ||
          this.clientDetails?.Preferencias?.$values ||
          [];

        if (preferenciasArray && preferenciasArray.length > 0) {
          this.preferencias = preferenciasArray[0];
          this.preferencias = {
            tipo: this.preferencias.tipo || this.preferencias.tipoPropiedad,
            operacion:
              this.preferencias.operacion ||
              this.preferencias.operacionPreferida,
            precioMin:
              this.preferencias.precioMin || this.preferencias.rangoPrecioMin,
            precioMax:
              this.preferencias.precioMax || this.preferencias.rangoPrecioMax,
            habitaciones: this.preferencias.habitaciones,
            banos: this.preferencias.banos,
            ubicacion: this.preferencias.ubicacion,
            metrosCuadrados: this.preferencias.metrosCuadrados,
            amenidades: this.preferencias.amenidades || [],
          };
          console.log("🎯 Preferencias cargadas:", this.preferencias);
        } else {
          this.preferencias = null;
          console.log("❌ No hay preferencias");
        }

        console.log("✅ Datos del cliente cargados correctamente");
      } catch (error) {
        console.error("❌ Error cargando datos del cliente:", error);
        Swal.fire({
          icon: "error",
          title: "Error",
          text: "No se pudieron cargar los datos del cliente",
        });
      } finally {
        this.isLoadingDetails = false;
      }
    },

    // NUEVO MÉTODO: Reset completo del estado del modal
    async resetModalState() {
      console.log("🔄 Reseteando estado del modal...");
      
      // Verificar si el modal existe
      if (!this.contactModal) {
        console.log("⚠️ contactModal no está inicializado");
        const modalElement = document.getElementById("contactModal");
        if (modalElement) {
          this.contactModal = new Modal(modalElement);
          console.log("✅ Modal reinicializado");
        } else {
          console.error("❌ No se encontró elemento contactModal en el DOM");
        }
      }

      // Cerrar modal si está abierto
      this.contactModal?.hide?.();

      // Limpiar todos los datos reactivos
      this.selectedTask = null;
      this.originalStatus = null;
      this.clientDetails = null;
      this.preferencias = null;
      this.currentRecordatorio.estadoInteraccionRaw = null;
      this.contactData = { tipo: "llamada", nota: "", fechaVencimiento: "" };
      this.isLoadingDetails = false;
      this.pagination.currentPage = 1;

      // Pequeño delay para asegurar que Vue procese los cambios
      await new Promise((resolve) => setTimeout(resolve, 50));

      console.log("🔄 Estado del modal reseteado");
    },

    // MÉTODO AUXILIAR: Comparar propiedad con preferencias
    coincideConPreferencias(propiedad, preferencias) {
      // Lógica básica de matching - ajusta según tus necesidades
      if (preferencias.tipo && propiedad.tipos !== preferencias.tipo)
        return false;
      if (
        preferencias.operacion &&
        propiedad.operaciones !== preferencias.operacion
      )
        return false;

      // Precio
      if (preferencias.precioMin && propiedad.precio < preferencias.precioMin)
        return false;
      if (preferencias.precioMax && propiedad.precio > preferencias.precioMax)
        return false;

      // Habitaciones
      if (
        preferencias.habitaciones &&
        propiedad.habitaciones < preferencias.habitaciones
      )
        return false;

      // Baños
      if (preferencias.banos && propiedad.banos < preferencias.banos)
        return false;

      // Ubicación (búsqueda parcial)
      if (preferencias.ubicacion && propiedad.ubicaciones) {
        const ubicacionProp = propiedad.ubicaciones.toLowerCase();
        const ubicacionPref = preferencias.ubicacion.toLowerCase();
        if (!ubicacionProp.includes(ubicacionPref)) return false;
      }

      return true;
    },
    closeContactModal() {
      // si hubo un drag (o cambio) y no se guardó, revertir status real
      if (this.selectedTask && this.originalStatus != null) {
        const idx = this.tasks.findIndex((t) => t.id === this.selectedTask.id);
        if (idx > -1) {
          this.tasks[idx].status = this.originalStatus;
        }
      }

      // Limpiar todos los datos del modal
      this.selectedTask = null;
      this.originalStatus = null;
      this.clientDetails = null;
      this.preferencias = null;
      this.currentRecordatorio.estadoInteraccionRaw = null;
      this.contactData = { tipo: "llamada", nota: "", fechaVencimiento: "" };
      this.isLoadingDetails = false;

      this.contactModal?.hide?.();
    },
    async saveContact() {
      if (!this.selectedTask) return;
      if (!this.agenteId) {
        Swal.fire({
          icon: "error",
          title: "Sin sesión activa",
          text: "No se puede guardar sin un agente autenticado.",
        });
        return;
      }

      try {
        // 1. Convertir el estado al formato esperado por el backend
        const estadoNumerico = Object.keys(statusMap).find(
          (key) => statusMap[key] === this.selectedTask.status
        );

        if (!estadoNumerico) {
          throw new Error("El estado seleccionado no es válido.");
        }

        const patchPayload = {
          Status: parseInt(estadoNumerico), // Asegurarse de enviar un número
        };

        // 2. Actualizar el estado del cliente
        await clienteService.patchCliente(this.selectedTask.id, patchPayload);

        // 3. Registrar la nueva interacción
        const nuevaInteraccion = {
          fecha: new Date().toISOString(),
          fechaVencimiento: this.contactData.fechaVencimiento
            ? new Date(this.contactData.fechaVencimiento).toISOString()
            : null,
          tipo: this.contactData.tipo,
          descripcion: this.contactData.nota,
          clienteId: this.selectedTask.id,
          agenteId: this.agenteId,
          statusInteraccion: this.selectedTask.status,
        };
        await interaccionService.addInteraccion(nuevaInteraccion);

        if (!this.selectedTask.Interacciones)
          this.selectedTask.Interacciones = [];
        this.selectedTask.Interacciones.push(nuevaInteraccion);

        // Actualizar el estado de la tarjeta en la lista `tasks`
        const index = this.tasks.findIndex(
          (t) => t.id === this.selectedTask.id
        );
        if (index > -1) {
          this.tasks[index].status = this.selectedTask.status; // aplicar el nuevo estado
          this.tasks[index].nota = nuevaInteraccion.descripcion;
          this.tasks[index].Interacciones = [
            ...this.selectedTask.Interacciones,
          ];
        }

        // Mover la tarjeta a la columna correspondiente
        this.tasks = [...this.tasks]; // Forzar reactividad en la lista

        this.originalStatus = null; // limpiar ya que se confirmó

        Swal.fire({
          icon: "success",
          title: "Guardado",
          text: "Estado e interacción actualizados correctamente.",
          timer: 1500,
          showConfirmButton: false,
        });

        if (this.contactData.fechaVencimiento)
          this.scheduleNotification(this.selectedTask);
      } catch (error) {
        // Depurar el error para identificar la causa
        console.error("❌ Error al guardar:", error);

        // Mostrar un mensaje más informativo al usuario
        const errorMessage = error?.message || "Ocurrió un error inesperado.";
        Swal.fire({
          icon: "error",
          title: "Error",
          text: `No se pudo actualizar el cliente o registrar la interacción: ${errorMessage}`,
        });
      }
      this.closeContactModal();
    },
    formatDate(dateString) {
      if (!dateString) return "";
      const options = { day: "2-digit", month: "2-digit", year: "numeric" };
      return new Date(dateString).toLocaleDateString("es-ES", options);
    },
    formatDateTime(dateString) {
      if (!dateString) return "";
      const options = { 
        day: "2-digit", 
        month: "2-digit", 
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        hour12: false
      };
      return new Date(dateString).toLocaleDateString("es-ES", options);
    },
    formatStatusForDisplay(value) {
      return (value || "")
        .replace(/_/g, " ")
        .replace(/\b\w/g, (c) => c.toUpperCase());
    },
    scheduleNotification(task) {
      if (!("Notification" in window)) return;
      const last = task.Interacciones?.length
        ? task.Interacciones[task.Interacciones.length - 1]
        : null;
      if (last?.FechaVencimiento) {
        const timeout = new Date(last.FechaVencimiento) - new Date();
        if (timeout > 0) {
          setTimeout(() => {
            if (Notification.permission === "granted") {
              new Notification(`Recordatorio: ${task.nombre}`, {
                body: `Tienes un seguimiento pendiente.`,
                icon: "/favicon.ico",
              });
            }
          }, timeout);
        }
      }
    },
    checkNotifications() {
      if ("Notification" in window && Notification.permission !== "granted") {
        Notification.requestPermission();
      }
    },

    // ===================== AQUÍ SE FILTRA POR ROL =====================
    async cargarTodosLosClientes() {
      this.isLoading = true;
      try {
        // Solicitar 100 clientes para tener más datos en el dashboard
        const response = await clienteService.getAllClientes(1, 100);
        console.log("📦 Respuesta completa de getAllClientes:", response);
        console.log("📦 Tipo de respuesta:", typeof response);
        console.log("📦 Es array?:", Array.isArray(response));
        console.log("📦 Tiene $values?:", response?.$values);
        console.log("📦 Tiene data?:", response?.data);
        
        // La API ahora devuelve { data: [...], page, pageSize, totalCount, totalPages }
        const clientes = response?.data || response?.$values || response || [];
        console.log("📦 Clientes extraídos:", clientes);
        console.log("📦 Total de clientes:", clientes.length);

        // Cargar agentes para mapear nombres (usar this.agentesMap para compartir con TaskCard)
        this.agentesMap = new Map();
        try {
          const agentesResp = await agenteService.getUsers();
          const agentesRaw = agentesResp?.$values || agentesResp || [];
          agentesRaw.forEach((ag) => {
            const id = ag.Id || ag.id || ag.userId || ag.usuarioId;
            this.agentesMap.set(id, ag);
          });
        } catch (e) {
          this.agentesMap = new Map();
        }

        // Filtrar: admin => todos; no admin => solo clientes del agente
        const filtrados = this.isAdmin
          ? clientes
          : clientes.filter((c) => {
              const cid =
                c.AgenteId ||
                c.agenteId ||
                c.Agente?.Id ||
                c.agente?.Id ||
                c.agente?.id;
              return cid === this.agenteId;
            });

        // Reiniciar tasks antes de llenar
        this.tasks = [];

        filtrados.forEach((rawCliente) => {
          const agenteIdVal =
            rawCliente.AgenteId || rawCliente.agenteId || null;
          const agenteObj = this.agentesMap.get(agenteIdVal) || {};
          const agenteNombre = agenteObj.Nombre || agenteObj.nombre || "";
          const agenteApellido = agenteObj.Apellido || agenteObj.apellido || "";
          const task = {
            id: rawCliente.Id || rawCliente.id,
            agenteId: rawCliente.AgenteId || rawCliente.agenteId || null, // Guardamos para referencia
            agenteNombre: agenteNombre,
            agenteApellido: agenteApellido,
            nombre: rawCliente.Nombre || rawCliente.nombre || "",
            apellido: rawCliente.Apellido || rawCliente.apellido || "",
            dni: rawCliente.Dni || rawCliente.dni || "",
            telefono: rawCliente.Telefono || rawCliente.telefono || "",
            email: rawCliente.Email || rawCliente.email || "",
            origen: rawCliente.Origen || rawCliente.origen || "",
            nota: rawCliente.Notas || rawCliente.nota || "",
            fechaRegistro:
              rawCliente.FechaRegistro ||
              rawCliente.fechaRegistro ||
              new Date().toISOString(),
            // Extraer interacciones desde interacciones.$values
            Interacciones: rawCliente.interacciones?.$values || [],
            Preferencias: rawCliente.Preferencias,
            status:
              statusMap[rawCliente.Status] ||
              statusMap[rawCliente.status] ||
              "contacto_inicial_pendiente",
          };
          this.tasks.push(task);
        });

        // Cargar todos los agentes (solo si es admin)
        if (this.isAdmin) {
          this.agentesArray = Array.from(this.agentesMap.values()).map((ag) => ({
            id: ag.Id || ag.id || ag.userId || ag.usuarioId,
            nombre: ag.Nombre || ag.nombre || "",
            apellido: ag.Apellido || ag.apellido || "",
          }));
        } else {
          // Si no es admin, cargar solo el propio agente
          const propioAgente = this.agentesMap.get(this.agenteId) || {};
          this.agentesArray = [
            {
              id:
                propioAgente.Id ||
                propioAgente.id ||
                propioAgente.userId ||
                propioAgente.usuarioId,
              nombre: propioAgente.Nombre || propioAgente.nombre || "",
              apellido: propioAgente.Apellido || propioAgente.apellido || "",
            },
          ];
        }
        
        console.log("✅ Tasks finales cargadas:", this.tasks.length);
        console.log("✅ Primera tarea:", this.tasks[0]);
        console.log("✅ AgentesArray:", this.agentesArray.length);
      } catch (error) {
        console.error("❌ Error al cargar clientes:", error?.message || error);
        Swal.fire({
          icon: "error",
          title: "Error",
          text: "No se pudieron cargar los clientes desde la API.",
          confirmButtonText: "Aceptar",
        });
      } finally {
        this.isLoading = false;
        console.log("🏁 Carga completada. isLoading:", this.isLoading);
      }
    },
    // ==================================================================

    handleClientAdded(newClient) {
      this.tasks.push({
        id: Date.now(),
        ...newClient,
        status: "contacto_inicial_pendiente",
        Interacciones: [],
        fechaRegistro: new Date().toISOString(),
      });
      // Ordenar las tareas por fecha de registro (más reciente primero)
      this.tasks.sort(
        (a, b) => new Date(b.fechaRegistro) - new Date(a.fechaRegistro)
      );
      this.showAddModal = false;
    },

    async onClientSaved() {
      this.showAddModal = false;
      await this.cargarTodosLosClientes(); // refresca todo desde la API
      // Ordenar las tareas por fecha de registro (más reciente primero)
      this.tasks.sort(
        (a, b) => new Date(b.fechaRegistro) - new Date(a.fechaRegistro)
      );
    },

    // Obtiene un token almacenado (intenta varias claves comunes)
    getStoredToken() {
      const keys = ["jwt-token", "jwtToken", "token", "access_token"];
      for (const k of keys) {
        const v = localStorage.getItem(k);
        if (v) return { key: k, value: v };
      }
      return null;
    },
    // Decodifica base64url de forma segura
    decodeBase64Url(input) {
      let output = input.replace(/-/g, "+").replace(/_/g, "/");
      const pad = output.length % 4;
      if (pad) output += "=".repeat(4 - pad);
      return atob(output);
    },
    // Parsea un JWT sin verificar firma, solo para leer 'exp'
    parseJwt(token) {
      const parts = token.split(".");
      if (parts.length < 2) throw new Error("Token inválido");
      const payload = JSON.parse(this.decodeBase64Url(parts[1]));
      return payload;
    },
    // Valida token por existencia y expiración (claim 'exp' en segundos)
    hasValidToken() {
      const stored = this.getStoredToken();
      if (!stored) return false;
      try {
        const payload = this.parseJwt(stored.value);
        if (!payload || typeof payload.exp !== "number") return false;
        return payload.exp * 1000 > Date.now();
      } catch {
        return false;
      }
    },
    checkSession() {
      const expired = !this.hasValidToken();
      this.isSessionExpired = expired;
      if (expired) this.startAutoRedirectCountdown();
      else this.stopAutoRedirectCountdown();
    },
    onStorage(e) {
      if (["jwt-token", "jwtToken", "token", "access_token"].includes(e.key)) {
        this.checkSession();
      }
    },
    startAutoRedirectCountdown() {
      this.stopAutoRedirectCountdown();
      this.redirectCountdown = 5;
      this.autoRedirectTimer = setInterval(() => {
        this.redirectCountdown -= 1;
        if (this.redirectCountdown <= 0) {
          this.stopAutoRedirectCountdown();
          this.redirectToLogin();
        }
      }, 1000);
    },
    stopAutoRedirectCountdown() {
      if (this.autoRedirectTimer) {
        clearInterval(this.autoRedirectTimer);
        this.autoRedirectTimer = null;
      }
    },
    redirectToLogin() {
      ["jwt-token", "jwtToken", "token", "access_token"].forEach((k) =>
        localStorage.removeItem(k)
      );
      if (this.$router && typeof this.$router.push === "function") {
        this.$router.push("/");
      } else {
        window.location.href = "/";
      }
    },
  },
  computed: {
    searchPlaceholder() {
      switch (this.searchFilter) {
        case "person":
          return "Buscar por nombre o apellido de persona";
        case "agent":
          return "Buscar por nombre o apellido de agente";
        default:
          return "Buscar en todos los campos";
      }
    },
    filteredTasks() {
      let list = this.tasks;

      // Filtrar por agente seleccionado
      if (this.selectedAgent && this.selectedAgent !== "all") {
        const sel = String(this.selectedAgent);
        list = list.filter((t) => String(t.agenteId) === sel);
      }

      // Filtrar por término de búsqueda
      if (this.searchTerm) {
        const raw = this.searchTerm || "";
        const tokens = this.normalizeSearch(raw).split(/\s+/).filter(Boolean);
        if (tokens.length) {
          list = list.filter((task) => {
            let searchFields = [];
            switch (this.searchFilter) {
              case "person":
                searchFields = [task.nombre, task.apellido];
                break;
              case "agent":
                searchFields = [
                  task.agenteNombre || "",
                  task.agenteApellido || "",
                ];
                break;
              default:
                searchFields = [
                  task.nombre,
                  task.apellido,
                  task.telefono,
                  task.email,
                  task.agenteNombre || "",
                  task.agenteApellido || "",
                  task.agenteId || "",
                ];
            }
            const haystack = this.normalizeSearch(searchFields.join(" "));
            return tokens.every((t) => haystack.includes(t));
          });
        }
      }

      // Filtrar por rango de fechas (inclusivo)
      if (this.dateFilter.startDate || this.dateFilter.endDate) {
        let start = this.parseFilterDate(this.dateFilter.startDate, false);
        let end = this.parseFilterDate(this.dateFilter.endDate, true);

        // Si solo hay start -> end = mismo día (fin del día)
        if (start && !end)
          end = this.parseFilterDate(this.dateFilter.startDate, true);
        // Si solo hay end -> start = mismo día (inicio del día)
        if (!start && end)
          start = this.parseFilterDate(this.dateFilter.endDate, false);

        list = list.filter((task) => {
          const rawDate = new Date(task.fechaRegistro);
          // Normalizar a fecha (sin hora) usando zona local
          const taskDateOnly = new Date(
            rawDate.getFullYear(),
            rawDate.getMonth(),
            rawDate.getDate()
          );
          return (
            (!start || taskDateOnly >= start) && (!end || taskDateOnly <= end)
          );
        });
      }

      return list;
    },
    statusOptions() {
      return this.columns
        .map((col) => {
          const key = Object.keys(statusMap).find(
            (k) => statusMap[k] === col.estado
          );
          return key
            ? { key: parseInt(key), title: col.title, estado: col.estado }
            : null;
        })
        .filter(Boolean);
    },
  },
  watch: {
    "currentRecordatorio.estadoInteraccionRaw"(val) {
      if (val != null && this.selectedTask) {
        const mapped = statusMap[val];
        if (mapped) {
          this.selectedTask.status = mapped;
        }
      }
    },
  },
};
//Uno atras
</script>

<style scoped>

</style>
