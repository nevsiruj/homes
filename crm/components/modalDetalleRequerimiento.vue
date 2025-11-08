<template>
  <teleport to="body">
    <!-- Overlay y modal teleported al body con z-index alto -->
    <div
      v-if="isOpen"
      class="modal-overlay fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4"
      style="z-index:99999;"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div
        class="bg-white rounded-lg w-full max-w-2xl shadow-lg p-6 overflow-auto"
        role="document"
        style="z-index:100000;"
      >
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-lg font-semibold">Detalle del Requerimiento</h3>
          <button @click="closeModal" class="text-gray-400 hover:bg-gray-200 rounded p-1">
            X
          </button>
        </div>

        <div v-if="isLoading" class="py-8 text-center">Cargando...</div>

        <div v-else-if="!preferencia" class="py-8 text-center text-gray-500">Requerimiento no encontrado.</div>

        <div v-else class="space-y-4">
          <div class="bg-gray-50 p-4 rounded">
            <h4 class="font-bold mb-2">Resumen</h4>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-2">
              <div><span class="text-sm text-gray-500">Tipo:</span> <div>{{ preferencia.tipo || "-" }}</div></div>
              <div><span class="text-sm text-gray-500">Operación:</span> <div>{{ preferencia.operacion || "-" }}</div></div>
              <div><span class="text-sm text-gray-500">Ubicación:</span> <div>{{ preferencia.ubicacion || "-" }}</div></div>
              <div><span class="text-sm text-gray-500">Habitaciones:</span> <div>{{ preferencia.habitaciones ?? "-" }}</div></div>
              <div><span class="text-sm text-gray-500">Baños:</span> <div>{{ preferencia.banos ?? "-" }}</div></div>
              <div><span class="text-sm text-gray-500">M² mínimos:</span> <div>{{ preferencia.metrosCuadrados ?? "-" }}</div></div>
              <div><span class="text-sm text-gray-500">Precio Min:</span> <div>{{ formatMoney(preferencia.precioMin) }}</div></div>
              <div><span class="text-sm text-gray-500">Precio Max:</span> <div>{{ formatMoney(preferencia.precioMax) }}</div></div>
            </div>
          </div>

          <div class="bg-gray-50 p-4 rounded">
            <h4 class="font-bold mb-2">Amenidades</h4>
            <div v-if="amenidadesNames.length === 0" class="text-gray-500">—</div>
            <div v-else class="flex flex-wrap gap-2">
              <span v-for="(a, idx) in amenidadesNames" :key="idx" class="bg-gray-100 px-2 py-1 rounded text-sm">{{ a }}</span>
            </div>
          </div>

          <div class="bg-gray-50 p-4 rounded">
            <h4 class="font-bold mb-2">Comentarios</h4>
            <p class="text-gray-700">{{ preferencia.comentarios || "Sin comentarios." }}</p>
          </div>

          <div class="flex justify-end">
            <button @click="closeModal" class="bg-gray-600 text-white px-4 py-2 rounded">Cerrar</button>
          </div>
        </div>
      </div>
    </div>
  </teleport>
</template>

<script setup>
// filepath: /C:/repo/homes-web/crm/components/modalDetalleRequerimiento.vue
import { ref, watch, defineProps, defineEmits } from "vue";
import clienteService from "@/services/clienteService";

const props = defineProps({
  isOpen: { type: Boolean, required: true },
  clienteId: { type: [Number, String], default: null },
  preferenciaId: { type: [Number, String], default: null },
});
const emit = defineEmits(["close"]);

const preferencia = ref(null);
const isLoading = ref(false);

function formatMoney(v) {
  if (v == null || v === "") return "-";
  const n = Number(v);
  if (Number.isNaN(n)) return "-";
  return `$${n.toLocaleString()}`;
}

const amenidadesNames = ref([]);

// cargar preferencia: busca el cliente y extrae la preferencia por id (compatible con $values y distintas formas)
async function loadPreferencia(clienteId, prefId) {
  preferencia.value = null;
  amenidadesNames.value = [];
  if (!clienteId || !prefId) return;
  isLoading.value = true;
  try {
    const resp = await clienteService.getClienteById(clienteId);
    const clienteData = resp?.$values ? resp.$values[0] : resp;
    const prefsArr =
      clienteData?.preferencias?.$values ??
      (Array.isArray(clienteData?.preferencias) ? clienteData.preferencias : []);
    const found = Array.isArray(prefsArr) ? prefsArr.find((p) => Number(p?.id) === Number(prefId)) : null;
    if (!found) {
      preferencia.value = null;
      return;
    }
    // normalizar campos usados en template
    preferencia.value = {
      id: found.id,
      tipo: found.tipo ?? found.tipoNombre ?? "",
      operacion: found.operacion ?? found.operacionNombre ?? "",
      ubicacion: Array.isArray(found.ubicacion) ? found.ubicacion.join(", ") : found.ubicacion ?? "",
      precioMin: found.precioMin ?? found.precioDesde ?? null,
      precioMax: found.precioMax ?? found.precioHasta ?? null,
      habitaciones: found.habitaciones ?? null,
      banos: found.banos ?? null,
      metrosCuadrados: found.metrosCuadrados ?? null,
      comentarios: found.comentarios ?? found.descripcion ?? "",
      preferenciaAmenidades: Array.isArray(found.preferenciaAmenidades)
        ? found.preferenciaAmenidades
        : Array.isArray(found.preferenciaAmenidades?.$values)
        ? found.preferenciaAmenidades.$values
        : [],
    };

    // extraer nombres de amenidades de forma robusta
    const rawAms = preferencia.value.preferenciaAmenidades || [];
    amenidadesNames.value = (rawAms || []).map((a) => a?.Nombre ?? a?.nombre ?? a?.nombreAmenidad ?? a?.AmenidadNombre ?? a?.amenidad ?? String(a)).filter(Boolean);
  } catch (e) {
    console.error("Error loading preferencia detail:", e);
    preferencia.value = null;
  } finally {
    isLoading.value = false;
  }
}

// reactivar carga cuando cambian props
watch(
  () => [props.isOpen, props.clienteId, props.preferenciaId],
  ([isOpen, clienteId, prefId]) => {
    if (isOpen && clienteId && prefId) {
      loadPreferencia(clienteId, prefId);
    } else {
      preferencia.value = null;
      amenidadesNames.value = [];
    }
  },
  { immediate: true }
);

function closeModal() {
  emit("close");
}
</script>

<style>
.modal-overlay { backdrop-filter: blur(2px); }
</style>
