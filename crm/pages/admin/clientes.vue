<template>
  <div>
    <div class="mt-5 mb-12">
      <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
        Gestión de Clientes
      </h2>
    </div>
  </div>
  <div class="grid grid-cols-1 gap-4 mb-8 lg:grid-cols-3 xl:grid-cols-4">
    <div class="lg:col-span-2 xl:col-span-3">
      <form
        class="flex flex-col sm:flex-row sm:items-end sm:flex-wrap gap-2"
        @submit.prevent
      >
        <div class="flex flex-col sm:flex-row gap-2 w-full sm:w-auto">
          <select
            v-if="isAdmin"
            v-model="selectedAgent"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2"
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
          <div class="relative w-full">
            <input
              v-model.trim="searchTerm"
              type="text"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full ps-3 p-2"
              placeholder="Buscar cliente..."
            />
          </div>
        </div>

        <div class="flex space-x-2">
          <input
            v-model="dateFrom"
            type="date"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2"
            title="Desde"
          />
          <input
            v-model="dateTo"
            type="date"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 p-2"
            title="Hasta"
          />
        </div>
      </form>
    </div>

    <div class="flex justify-end items-end">
      <button
        type="button"
        @click.prevent="openModal"
        class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 w-full sm:w-auto focus:outline-none"
      >
        Agregar cliente
      </button>
    </div>
  </div>

  <!-- Tabla -->

  <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
    <table class="w-full text-sm text-left rtl:text-right text-gray-500">
      <thead class="text-xs text-gray-700 uppercase bg-gray-50">
        <tr>
          <th scope="col" class="px-6 py-3">Fecha registro</th>
          <th scope="col" class="px-6 py-3">Nombre y apellido</th>
          <!-- <th scope="col" class="px-6 py-3">DNI</th> -->
          <th scope="col" class="px-6 py-3">Telefono</th>
          <!-- <th scope="col" class="px-6 py-3">Direccion</th> -->
          <th scope="col" class="px-6 py-3">Email</th>
          <th scope="col" class="px-6 py-3">Agente Asignado</th>
          <th scope="col" class="px-6 py-3">Accion</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="cliente in filteredClientes"
          :key="cliente.id"
          class="odd:bg-white odd: even:bg-gray-50 even: border-b border-gray-200"
        >
          <th
            scope="row"
            class="px-6 py-4 font-medium text-gray-900 whitespace-nowrap"
          >
            {{ formatDate(cliente.fechaRegistro) }}
          </th>
          <td class="px-6 py-4">{{ cliente.nombre }} {{ cliente.apellido }}</td>
          <!-- <td class="px-6 py-4">{{ cliente.dni }}</td> -->
          <td class="px-6 py-4">{{ cliente.telefono }}</td>
          <!-- <td class="px-6 py-4">{{ cliente.direccion }}</td> -->
          <td class="px-6 py-4">{{ cliente.email || "-" }}</td>
          <td class="px-6 py-4">
            {{
              cliente.agente
                ? `${cliente.agente.nombre} ${cliente.agente.apellido}`
                : "Sin asignar"
            }}
          </td>
          <td class="px-6 py-4 space-x-2 flex content-center justify-center">
            <button
              @click="openDetails(cliente)"
              class="flex items-center font-medium text-gray-600 hover:underline"
              title="Ver detalles"
            >
              <svg
                class="w-5 h-5 mr-1"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M15.0007 12C15.0007 13.6569 13.6576 15 12.0007 15C10.3439 15 9.00073 13.6569 9.00073 12C9.00073 10.3431 10.3439 9 12.0007 9C13.6576 9 15.0007 10.3431 15.0007 12Z"
                  stroke="currentColor"
                  stroke-width="2"
                ></path>
                <path
                  d="M12.0012 5C7.52354 5 3.73326 7.94288 2.45898 12C3.73324 16.0571 7.52354 19 12.0012 19C16.4788 19 20.2691 16.0571 21.5434 12C20.2691 7.94291 16.4788 5 12.0012 5Z"
                  stroke="currentColor"
                  stroke-width="2"
                ></path>
              </svg>
            </button>
            <button
              @click="openEditModal(cliente)"
              class="flex font-medium text-gray-600 hover:underline"
              title="Editar cliente"
            >
              <svg
                class="w-5 h-5 text-gray-800"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="1"
                  d="m14.304 4.844 2.852 2.852M7 7H4a1 1 0 0 0-1 1v10a1 1 0 0 0 1 1h11a1 1 0 0 0 1-1v-4.5m2.409-9.91a2.017 2.017 0 0 1 0 2.853l-6.844 6.844L8 14l.713-3.565 6.844-6.844a2.015 2.015 0 0 1 2.852 0Z"
                />
              </svg>
            </button>

            <button
              @click="eliminarCliente(cliente.id)"
              class="flex font-medium text-gray-600 hover:underline"
              title="Eliminar cliente"
            >
              <svg
                class="w-5 h-5 text-gray-800"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                <g
                  id="SVGRepo_tracerCarrier"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                ></g>
                <g id="SVGRepo_iconCarrier">
                  <path
                    d="M20.5001 6H3.5"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  ></path>
                  <path
                    d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  ></path>
                  <path
                    d="M9.5 11L10 16"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  ></path>
                  <path
                    d="M14.5 11L14 16"
                    stroke="#000000"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  ></path>
                  <path
                    d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                    stroke="#000000"
                    stroke-width="1.5"
                  ></path>
                </g>
              </svg>
            </button>

            <button
              @click="requerimientoCliente(cliente.id)"
              class="flex font-medium text-gray-600 hover:underline"
              title="Agregar Requerimiento"
            >
              <svg
                class="w-5 h-5 text-gray-800"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                fill="none"
                viewBox="0 0 24 24"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M18 9V4a1 1 0 0 0-1-1H8.914a1 1 0 0 0-.707.293L4.293 7.207A1 1 0 0 0 4 7.914V20a1 1 0 0 0 1 1h4M9 3v4a1 1 0 0 1-1 1H4m11 6v4m-2-2h4m3 0a5 5 0 1 1-10 0 5 5 0 0 1 10 0Z"
                />
              </svg>
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <AgregarClienteModal
    :isOpen="isModalOpen"
    :cliente="selectedCliente"
    :isAdmin="isAdmin"
    @close="isModalOpen = false"
    @client-added="handleClientAddedOrUpdated"
    @client-updated="handleClientAddedOrUpdated"
  />

  <DetalleCliente
    :isOpen="isDetailsOpen"
    :cliente="selectedCliente"
    :isAdmin="isAdmin"
    @close="closeDetailsModal"
    @open-inmueble="handleOpenInmueble"
    @edit-preferencia="handleEditPreferencia"
  />

  <!-- Modal detalle de inmueble -->
  <modal-detalle-inmueble
    v-if="showInmuebleModal"
    :isOpen="showInmuebleModal"
    :inmueble="inmuebleSeleccionado"
    @close="closeInmuebleModal"
  />

  <modalRequerimiento
    :isOpen="isRequerimientoModalOpen"
    :clienteId="clienteIdForRequerimiento"
    :preferenciaId="prefIdForRequerimiento"
    @close="closeRequerimientoModal"
  />
</template>

<script setup>
import { ref, onMounted, computed, watch } from "vue";
import AgregarClienteModal from "../../components/agregarClienteModal.vue";
import clienteService from "../../services/clienteService";
import detalleCliente from "~/components/detalleCliente.vue";
import modalDetalleInmueble from "../../components/modalDetalleInmueble.vue";
import inmuebleService from "../../services/inmuebleService";
import Swal from "sweetalert2";
import agenteService from "../../services/agenteService";
import { useAuthStore } from "@/stores/auth";
import modalRequerimiento from "../../components/modalRequerimiento.vue";

// === Auth / Roles ===
const auth = useAuthStore();
const isAdmin = computed(() =>
  typeof auth.hasRole === "function"
    ? auth.hasRole("admin")
    : (auth.roles || []).includes("admin")
);

// === Datos reactivos ===
const clientes = ref([]);
const isLoading = ref(false);
const error = ref(null);

const isModalOpen = ref(false);
const selectedCliente = ref(null);
const isDetailsOpen = ref(false);
const searchTerm = ref("");
const selectedAgent = ref("all");
const showModal = ref(false);
const dateFrom = ref(null);
const dateTo = ref(null);

// Convertir el mapa de agentes a un array para el select
const agentesArray = computed(() => {
  return Object.values(agentesMap.value).map((agente) => ({
    id: agente.id,
    nombre: agente.nombre || agente.Nombre || "",
    apellido: agente.apellido || agente.Apellido || "",
  }));
});

const agentesMap = ref({});

// --- Nuevo: estado para modal detalle de inmueble ---
const showInmuebleModal = ref(false);
const inmuebleSeleccionado = ref(null);
// Marca si el modal de inmueble fue abierto desde el modal DetalleCliente
const inmuebleOpenedFromDetalle = ref(false);

const isRequerimientoModalOpen = ref(false);
const clienteIdForRequerimiento = ref(null);
const prefIdForRequerimiento = ref(null); // <-- nuevo: preferencia a editar (opcional)
const requerimientoOpenedFromDetalle = ref(false); // <-- nuevo flag

const requerimientoCliente = (id, prefId = null) => {
  clienteIdForRequerimiento.value = id;
  prefIdForRequerimiento.value = prefId;
  isRequerimientoModalOpen.value = true;
  requerimientoOpenedFromDetalle.value = false; // apertura manual no viene desde detalle
};

const closeRequerimientoModal = () => {
  isRequerimientoModalOpen.value = false;
  clienteIdForRequerimiento.value = null;
  prefIdForRequerimiento.value = null;

  // Si el modal de requerimiento fue abierto desde detalleCliente, volver a abrirlo
  if (requerimientoOpenedFromDetalle.value) {
    requerimientoOpenedFromDetalle.value = false;
    // reabrir detalleCliente con el cliente actual (si lo teníamos seleccionado)
    if (selectedCliente.value && selectedCliente.value.id) {
      isDetailsOpen.value = true;
    }
  }
};

// Nuevo handler: abrir modal en modo edición desde DetalleCliente
function handleEditPreferencia(payload) {
  clienteIdForRequerimiento.value = payload?.clienteId ?? null;
  prefIdForRequerimiento.value = payload?.preferenciaId ?? null;
  requerimientoOpenedFromDetalle.value = payload?.source === "detalleCliente";
  isRequerimientoModalOpen.value = true;

  if (!clienteIdForRequerimiento.value) {
    console.error(
      "Cliente ID no definido al intentar abrir el modal de requerimientos."
    );
    Swal.fire(
      "Error",
      "No se pudo abrir el modal: Cliente ID no definido.",
      "error"
    );
  }
}

definePageMeta({
  layout: "admin",
  requiresAuth: true,
});

// === Lista base según rol ===
// Si es admin => todos; si es agente => solo los que tengan agenteId === auth.user.id
const baseClientes = computed(() => {
  if (isAdmin.value) return clientes.value;
  const me = auth?.user?.id || auth?.user?.nameid || auth?.user?.sub || null;
  if (!me) return []; // si no hay user aún, mejor lista vacía
  return clientes.value.filter((c) => c.agenteId === me);
});

// Helper acento-insensible para búsqueda
function normalizeSearch(str) {
  return (str || "")
    .toString()
    .toLowerCase()
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "");
}

// Reemplazado: filteredClientes soporta múltiples palabras (todas deben coincidir - AND)
const filteredClientes = computed(() => {
  const list = baseClientes.value
    .slice() // Crear una copia para no mutar el original
    .sort((a, b) => new Date(b.fechaRegistro) - new Date(a.fechaRegistro)); // Ordenar por fecha de registro

  // Primero filtramos por el agente seleccionado
  let filteredList = list;

  if (selectedAgent.value !== "all") {
    filteredList = list.filter(
      (cliente) => cliente.agenteId === selectedAgent.value
    );
  }

  // Filtrar por rango de fechas (inclusive)
  if (dateFrom.value || dateTo.value) {
    const fromTime = dateFrom.value
      ? new Date(dateFrom.value + "T00:00:00").getTime()
      : null;
    const toTime = dateTo.value
      ? new Date(dateTo.value + "T23:59:59").getTime()
      : null;

    filteredList = filteredList.filter((c) => {
      const t = new Date(c.fechaRegistro).getTime();
      if (isNaN(t)) return false; // ignora fechas inválidas
      if (fromTime && t < fromTime) return false;
      if (toTime && t > toTime) return false;
      return true;
    });
  }

  // Luego aplicamos la búsqueda por texto si hay término de búsqueda
  const raw = searchTerm.value || "";
  const tokens = normalizeSearch(raw).split(/\s+/).filter(Boolean);

  if (tokens.length) {
    return filteredList.filter((c) => {
      const searchFields = [
        c.nombre,
        c.apellido,
        `${c.nombre} ${c.apellido}`,
        c.telefono,
        c.email,
      ];

      const haystack = normalizeSearch(searchFields.join(" "));
      return tokens.every((t) => haystack.includes(t));
    });
  }

  return filteredList;
});

// Abre el modal en modo creación
const openModal = () => {
  selectedCliente.value = null;
  isModalOpen.value = true;
};

const openDetails = async (client) => {
  try {
    isDetailsOpen.value = true;
    const response = await clienteService.getClienteById(client.id);

    const clienteData = response.$values ? response.$values[0] : response;

    // Mantener la estructura completa de preferencias (puede venir como { $values: [...] })
    const rawPrefsArr =
      clienteData.preferencias?.$values ??
      (Array.isArray(clienteData.preferencias) ? clienteData.preferencias : []);
    const firstPref = rawPrefsArr && rawPrefsArr.length ? rawPrefsArr[0] : null;

    // extraer amenidades del primer elemento para la vista resumen (compatibilidad)
    const summaryAmenidades = [];
    if (firstPref?.preferenciaAmenidades?.$values) {
      summaryAmenidades.push(
        ...firstPref.preferenciaAmenidades.$values.map(
          (a) => a.nombre || a.Nombre
        )
      );
    } else if (Array.isArray(firstPref?.amenidades)) {
      summaryAmenidades.push(...firstPref.amenidades);
    }

    selectedCliente.value = {
      ...clienteData,
      // pasar la estructura completa para que DetalleCliente pueda iterar todas las preferencias
      preferencias: clienteData.preferencias ?? {},
      // y, para compatibilidad con la UI de resumen, añadimos un resumen con la primera preferencia
      preferenciasSummary: {
        tipo: firstPref?.tipo ?? "N/A",
        operacion: firstPref?.operacion ?? "N/A",
        ubicacion: firstPref?.ubicacion ?? "N/A",
        precioMin: firstPref?.precioMin ?? null,
        precioMax: firstPref?.precioMax ?? null,
        habitaciones: firstPref?.habitaciones ?? null,
        banos: firstPref?.banos ?? null,
        metrosCuadrados: firstPref?.metrosCuadrados ?? null,
        amenidades: summaryAmenidades.length ? summaryAmenidades : [],
      },
    };
  } catch (error) {
    //console.error("Error loading client details:", error);
    isDetailsOpen.value = false;
    Swal.fire("Error", "No se pudieron cargar los detalles", "error");
  }
};

const openEditModal = async (cliente) => {
  try {
    // Obtener los datos completos del cliente desde el backend
    const response = await clienteService.getClienteById(cliente.id);
    const clienteData = response?.$values ? response.$values[0] : response;

    // Pasar el cliente completo al modal, incluyendo FechaRegistro
    selectedCliente.value = {
      ...clienteData,
      fechaRegistro: clienteData.fechaRegistro || clienteData.FechaRegistro || null, // Asegurar que FechaRegistro esté presente
    };

    isModalOpen.value = true;
  } catch (error) {
    Swal.fire("Error", "No se pudieron cargar los datos del cliente.", "error");
  }
};

// Cargar clientes
const loadClientes = async () => {
  isLoading.value = true;
  try {
    // Opción simple (siempre trae todo y filtramos por rol en el front)
    const response = await clienteService.getAllClientes();
    if (!response || !Array.isArray(response.$values)) {
      throw new Error("Datos inválidos desde la API");
    }

    const processedClients = response.$values.map((c) => {
      const preferencias = c.preferencias || {};
      const agenteAsignado = agentesMap.value[c.agenteId] || null;

      return {
        id: c.id,
        fechaRegistro: c.fechaRegistro || "N/A",
        nombre: c.nombre || "N/A",
        apellido: c.apellido || "",
        telefono: c.telefono || "N/A",
        email: c.email || "N/A",
        // importante para filtrar por dueño:
        agenteId: c.agenteId || null,
        agente: agenteAsignado, // objeto agente (si lo tenemos mapeado)
        tipo: preferencias.tipo || "N/A",
        operacion: preferencias.operacion || "N/A",
        ubicacion: preferencias.ubicacion || "N/A",
        precioMin:
          preferencias.precioMin !== null &&
          preferencias.precioMin !== undefined
            ? `$${parseFloat(preferencias.precioMin).toLocaleString()}`
            : "N/A",
        precioMax:
          preferencias.precioMax !== null &&
          preferencias.precioMax !== undefined
            ? `$${parseFloat(preferencias.precioMax).toLocaleString()}`
            : "N/A",
        habitaciones: preferencias.habitaciones ?? "N/A",
        banos: preferencias.banos ?? "N/A",
        metrosCuadrados: preferencias.metrosCuadrados ?? "N/A",
        amenidades: Array.isArray(preferencias.preferenciaAmenidades?.$values)
          ? preferencias.preferenciaAmenidades.$values.map(
              (a) => a.amenidadInmueble?.nombre || "Amenidad"
            )
          : [],
      };
    });

    clientes.value = processedClients;
  } catch (err) {
    //console.error("Error al cargar clientes:", err);
    error.value = "No se pudieron cargar las preferencias";
  } finally {
    isLoading.value = false;
  }
};

// Formatear fechas
const formatDate = (dateString) =>
  dateString
    ? new Date(dateString).toLocaleDateString()
    : "Fecha no disponible";

// Eliminar cliente
const eliminarCliente = async (id) => {
  const result = await Swal.fire({
    title: "¿Estás seguro?",
    text: "Esta acción no se puede deshacer",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d",
    confirmButtonText: "Sí, eliminar cliente",
    cancelButtonText: "Cancelar",
  });

  if (result.isConfirmed) {
    try {
      await clienteService.deleteCliente(id);
      await loadClientes();
      Swal.fire("Eliminado", "El cliente ha sido eliminado.", "success");
    } catch (error) {
      Swal.fire(
        "Error",
        "No se pudo eliminar el cliente: " + error.message,
        "error"
      );
      //console.error("Error al eliminar cliente:", error);
    }
  }
};

// Guardado/actualización
const handleClientAddedOrUpdated = async () => {
  await loadClientes();
};

// --- Nuevo helper: normalizar objeto inmueble para el modal (imagenes como array de URLs, amenidades como objetos) ---
function normalizeInmuebleForModal(raw) {
  if (!raw) return null;

  const obj = { ...raw };

  // imagenesReferencia -> array de strings (urls)
  let imgs = [];
  if (Array.isArray(raw.imagenesReferencia)) {
    imgs = raw.imagenesReferencia
      .map((it) => (typeof it === "string" ? it : it?.url || ""))
      .filter(Boolean);
  } else if (Array.isArray(raw.imagenesReferencia?.$values)) {
    imgs = raw.imagenesReferencia.$values
      .map((it) => (typeof it === "string" ? it : it?.url || ""))
      .filter(Boolean);
  } else if (Array.isArray(raw.imagenesReferencia?.items)) {
    imgs = raw.imagenesReferencia.items
      .map((it) => (typeof it === "string" ? it : it?.url || ""))
      .filter(Boolean);
  }
  obj.imagenesReferencia = imgs;

  // imagenPrincipal fallback
  obj.imagenPrincipal =
    raw.imagenPrincipal ||
    raw.imagen ||
    imgs[0] ||
    null ||
    "https://via.placeholder.com/150";

  // amenidades -> array de objetos { nombre: string }
  let ams = [];
  if (Array.isArray(raw.amenidades)) {
    ams = raw.amenidades
      .map((a) =>
        typeof a === "string"
          ? { nombre: a }
          : a?.nombre
          ? { nombre: a.nombre }
          : null
      )
      .filter(Boolean);
  } else if (Array.isArray(raw.amenidades?.$values)) {
    ams = raw.amenidades.$values
      .map((a) =>
        typeof a === "string"
          ? { nombre: a }
          : a?.nombre
          ? { nombre: a.nombre }
          : null
      )
      .filter(Boolean);
  } else if (Array.isArray(raw.amenidades?.items)) {
    ams = raw.amenidades.items
      .map((a) =>
        typeof a === "string"
          ? { nombre: a }
          : a?.nombre
          ? { nombre: a.nombre }
          : null
      )
      .filter(Boolean);
  }
  obj.amenidades = ams;

  return obj;
}

// --- Nuevo: manejar evento desde detalleCliente -> abrir modal de inmueble ---
async function handleOpenInmueble(payload) {
  try {
    // Evitar que queden ambos modales visibles al mismo tiempo
    // guardamos si antes estaba abierto el detalleCliente para poder reabrirlo al cerrar
    const wasDetailsOpen = isDetailsOpen.value === true;
    isDetailsOpen.value = false;
    inmuebleOpenedFromDetalle.value = false;

    // Backwards-compatible parsing:
    // - New structured form: { value: ..., source: 'detalleCliente' }
    // - Old primitives: number | string | object
    let core = payload;
    if (payload && typeof payload === "object" && "value" in payload) {
      // marca origen si viene del detalleCliente
      if (payload.source === "detalleCliente") {
        inmuebleOpenedFromDetalle.value = true;
      }
      core = payload.value;
    } else {
      // si el evento viene del DetalleCliente antiguo (sin estructura), no asumimos reopening
      inmuebleOpenedFromDetalle.value =
        inmuebleOpenedFromDetalle.value || false;
    }

    // Determinar id/slug/obj desde 'core'
    let id = null;
    if (typeof core === "number") id = core;
    else if (typeof core === "string" && /^\d+$/.test(core)) id = Number(core);
    else if (core && typeof core === "object" && (core.id || core.__raw?.id)) {
      id = core.id ?? core.__raw?.id;
    }

    let fetched = null;
    if (id !== null) {
      const detalle = await inmuebleService.getInmuebleById(id);
      fetched = detalle && detalle.$values ? detalle.$values[0] : detalle;
    } else if (
      core &&
      typeof core === "object" &&
      (core.slug || core.slugInmueble)
    ) {
      // Si solo tenemos slug, intentamos buscar por slug en el servicio si existe (no obligatorio)
      // Fallback: usamos el objeto recibido
      fetched = core;
    } else {
      fetched = core || null;
    }

    // Normalizar antes de asignar al modal para asegurar imagenes y amenidades correctas
    inmuebleSeleccionado.value = normalizeInmuebleForModal(fetched);

    // Si antes estaba abierto detalleCliente y el evento vino del detalle, mantener esa información para restore
    // (isDetailsOpen fue puesto en false arriba). showInmuebleModal abre ahora.
    showInmuebleModal.value = true;
  } catch (err) {
    //console.error("Error al abrir detalle de inmueble:", err);
    Swal.fire("Error", "No se pudo cargar el detalle del inmueble.", "error");
  }
}

function closeInmuebleModal() {
  showInmuebleModal.value = false;
  // Si fue abierto desde detalleCliente, volver a abrir el modal detalleCliente
  if (inmuebleOpenedFromDetalle.value) {
    inmuebleOpenedFromDetalle.value = false;
    isDetailsOpen.value = true;
  }
  inmuebleSeleccionado.value = null;
}

// Modificar el evento de cierre del modal detalleCliente
const closeDetailsModal = () => {
  isDetailsOpen.value = false;
  selectedCliente.value = null; // Limpiar el cliente seleccionado
};

// Watch para refrescar datos al abrir o cerrar cualquier modal
watch(
  [isModalOpen, isDetailsOpen, isRequerimientoModalOpen],
  ([modalOpen, detailsOpen, requerimientoOpen]) => {
    if (!modalOpen && !detailsOpen && !requerimientoOpen) {
      loadClientes();
    }
  }
);

// Montaje
onMounted(async () => {
  try {
    const data = await agenteService.getUsers();
    if (data.$values && Array.isArray(data.$values)) {
      data.$values.forEach((agente) => {
        agentesMap.value[agente.id] = agente;
      });
    }
  } catch (error) {
    //console.error("Error al cargar agentes:", error);
  }
  loadClientes();
});
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
