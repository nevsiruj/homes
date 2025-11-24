<template>
  <div class="mt-6 bg-gray-50 p-4 rounded-lg">
    <h4 class="font-bold text-lg mb-2">
      Propiedades que coinciden con las preferencias
    </h4>
    <!-- Skeleton Loader -->
    <div v-if="isLoading" class="animate-pulse space-y-4">
      <div v-for="i in 3" :key="`skeleton-${i}`" class="property-item mb-4">
        <div class="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-4">
          <!-- Título skeleton -->
          <div class="flex-grow">
            <div class="h-5 bg-gray-300 rounded w-3/4"></div>
          </div>
          <!-- Botones skeleton -->
          <div class="flex flex-wrap items-center space-x-2">
            <div class="w-4 h-4 bg-gray-300 rounded"></div>
            <div class="h-7 w-24 bg-green-300 rounded"></div>
            <div class="h-7 w-24 bg-blue-300 rounded"></div>
          </div>
        </div>
      </div>
      <!-- Paginación skeleton -->
      <div class="mt-4 flex items-center justify-between">
        <div class="h-4 bg-gray-300 rounded w-32"></div>
        <div class="flex space-x-2">
          <div class="h-8 w-20 bg-gray-300 rounded"></div>
          <div class="h-8 w-8 bg-gray-300 rounded"></div>
          <div class="h-8 w-8 bg-gray-300 rounded"></div>
          <div class="h-8 w-20 bg-gray-300 rounded"></div>
        </div>
      </div>
    </div>
    <div v-else-if="isLoadingMatches" class="text-center text-gray-500 py-4">
      Actualizando lista...
    </div>
    <div v-else-if="propiedades.length === 0" class="no-results">
      No se encontraron propiedades pendientes.
    </div>
    <ul v-else class="property-list">
      <li
        v-for="propiedad in paginatedProperties"
        :key="propiedad.id"
        class="property-item mb-4"
      >
        <div
          class="flex flex-col sm:flex-row sm:justify-between sm:items-center flex-wrap"
        >
          <h3 class="flex-grow mb-2 sm:mb-0">
            <a
              :href="buildInmuebleUrl(propiedad)"
              target="_blank"
              rel="noopener noreferrer"
              class="text-blue-600 hover:underline text-md font-regular"
            >
              {{ propiedad.titulo || propiedad.nombre || "Sin título" }}
            </a>
          </h3>
          <div class="flex flex-wrap mx-auto items-center space-x-2">
            <div class="flex items-center">
              <input
                :id="`matchEviado-${propiedad.propiedadId || propiedad.id}`"
                type="checkbox"
                :checked="isMatchEnviado(propiedad.propiedadId || propiedad.id)"
                @change="handleMatchCheckbox(propiedad)"
                class="w-4 h-4 text-gray-600 bg-gray-100 border-gray-300 rounded-sm cursor-pointer"
                :disabled="isLoading || isLoadingMatches || !datasetsReady"
                :title="checkboxTitle(propiedad.propiedadId || propiedad.id)"
              />
            </div>
            <button
              type="button"
              @click="openWhatsApp(propiedad)"
              class="inline-flex items-center px-2 py-1 text-xs font-medium text-white bg-green-500 hover:bg-green-600 rounded"
              title="Enviar por WhatsApp"
            >
              WhatsApp
            </button>
            <button
              type="button"
              @click="openInmuebleModal(propiedad)"
              class="inline-flex items-center px-2 py-1 text-xs font-medium text-white bg-blue-600 hover:bg-blue-700 rounded"
              title="Ver detalle de inmueble"
            >
              Ver detalle
            </button>
          </div>
        </div>
      </li>
    </ul>
    <div
      v-if="propiedades.length > pageSize"
      class="mt-4 flex items-center justify-between"
    >
      <div class="text-sm text-gray-600">
        Mostrando
        {{ (currentPage - 1) * pageSize + 1 }}
        -
        {{ Math.min(currentPage * pageSize, propiedades.length) }}
        de {{ propiedades.length }}
      </div>
      <nav class="inline-flex items-center space-x-1" aria-label="Paginación">
        <button
          @click="goPrev"
          :disabled="currentPage === 1"
          class="px-2 py-1 rounded border bg-black text-white hover:bg-gray-600 disabled:opacity-50"
        >
          Anterior
        </button>
        <button
          v-for="p in visiblePages"
          :key="p"
          @click="setPage(p)"
          :class="[
            'px-2 py-1 rounded border bg-black text-white',
            { 'bg-gray-600': p === currentPage },
          ]"
        >
          {{ p }}
        </button>
        <button
          @click="goNext"
          :disabled="currentPage === totalPages"
          class="px-2 py-1 rounded border bg-black text-white hover:bg-gray-600 disabled:opacity-50"
        >
          Siguiente
        </button>
      </nav>
    </div>
    <!--Enviados-->
    <div class="mt-6 bg-gray-50 p-4 rounded-lg">
      <div class="flex items-center justify-between mb-2">
        <h4 class="font-bold text-lg">Enviados</h4>
        <button
          type="button"
          class="text-xs px-2 py-1 rounded border border-gray-300 bg-white hover:bg-gray-100"
          @click="cargarMatches"
          :disabled="isLoadingMatches"
        >
          {{ isLoadingMatches ? "..." : "Refrescar" }}
        </button>
      </div>
      <!-- Skeleton para enviados -->
      <div v-if="isLoadingMatches" class="animate-pulse space-y-2">
        <div v-for="i in 3" :key="`enviado-skeleton-${i}`" class="flex items-center justify-between pb-1">
          <div class="h-4 bg-gray-300 rounded flex-grow mr-4"></div>
          <div class="flex items-center gap-4">
            <div class="h-3 w-24 bg-gray-300 rounded"></div>
            <div class="h-3 w-16 bg-gray-300 rounded"></div>
          </div>
        </div>
      </div>
      <div v-else>
        <div v-if="matchEnviados.length === 0" class="text-sm text-gray-500">
          Sin enviados.
        </div>
        <ul v-else class="space-y-2">
          <li
            v-for="m in enviadosConPropiedad"
            :key="m.id"
            class="flex items-center justify-between text-sm pb-1 gap-2"
          >
            <div class="flex-grow min-w-0 truncate">
              <a
                v-if="buildInmuebleUrl(m)"
                :href="buildInmuebleUrl(m)"
                target="_blank"
                rel="noopener noreferrer"
                class="text-blue-600 hover:underline"
              >
                {{ getTituloPropiedad(m) }}
              </a>
              <span v-else class="text-gray-700">
                {{ getTituloPropiedad(m) }}
              </span>
            </div>
            <div class="flex items-center gap-4 flex-shrink-0">
              <span class="text-xs text-gray-500 whitespace-nowrap">
                Enviado el
                {{ m.fechaEnvio ? formatDate(m.fechaEnvio) : "-" }}
              </span>
              <button
                @click="deleteMatch(m.id)"
                class="text-red-500 hover:text-red-700 text-xs font-medium whitespace-nowrap"
                title="Eliminar match"
              >
                Eliminar
              </button>
            </div>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref, computed, watch, defineProps, defineEmits, nextTick } from "vue";
import matchService from "~/services/matchService";
import inmuebleService from "~/services/inmuebleService";
import Swal from "sweetalert2";
const props = defineProps({
  clienteId: {
    type: [Number, String],
    required: true,
  },
  clientDetails: {
    type: Object,
    default: null,
  },
});
const emit = defineEmits(["open-inmueble", "refresh-matches"]);
// --- 1. PROPIEDADES Y ESTADO ---
const clienteId = computed(() => {
  const id = parseInt(props.clienteId);
  return isNaN(id) ? null : id;
});
// Estado de carga y datos
const isLoading = ref(true);
const isLoadingMatches = ref(false); // Para acciones específicas (checkbox)
const matchesPendientes = ref([]); // Almacena MatchDto[] (incluye el objeto Inmueble anidado)
const matchEnviados = ref([]); // Almacena matches que han sido marcados como enviados
// La lista de inmuebles que se renderizará
const propiedades = ref([]);
const datasetsReady = computed(() => !isLoading.value); // Asumo que esto indica que los datos están listos
// --- 2. PAGINACIÓN (Manteniendo tu lógica existente) ---
const currentPage = ref(1);
const pageSize = 10;
const totalPages = computed(() => {
  const total = Math.ceil(propiedades.value.length / pageSize);
  return total;
});
const paginatedProperties = computed(() => {
  const start = (currentPage.value - 1) * pageSize;
  const end = start + pageSize;
  return propiedades.value.slice(start, end);
});
// Implementa tus métodos de paginación (goPrev, goNext, setPage, visiblePages) aquí si no están fuera del script setup.
const goPrev = () => {
  if (currentPage.value > 1) currentPage.value--;
};
const goNext = () => {
  if (currentPage.value < totalPages.value) currentPage.value++;
};
const setPage = (p) => {
  currentPage.value = p;
};
// Lógica simple para visiblePages (ej. mostrar todas las páginas si es menos de 5)
const visiblePages = computed(() => {
  return Array.from({ length: totalPages.value }, (_, i) => i + 1);
});
// --- 3. FUNCIONES DE SERVICIO Y LÓGICA DE MATCH ---
/**
 * Llama al backend para obtener los matches sugeridos para el cliente.
 */
const fetchMatchesPendientes = async () => {
  if (!clienteId.value) {
    console.error("ID de cliente inválido.");
    return;
  }
  isLoading.value = true;
  try {
    const data = await matchService.getSugeridos(clienteId.value);
    // Normalizar posibles formas de respuesta
    let items = [];
    if (Array.isArray(data)) items = data;
    else if (Array.isArray(data?.items)) items = data.items;
    else if (Array.isArray(data?.results)) items = data.results;
    else if (Array.isArray(data?.matches)) items = data.matches;
    else if (Array.isArray(data?.$values)) items = data.$values;
    else if (Array.isArray(data?.items?.$values)) items = data.items.$values;
    // Los items ya son objetos completos que contienen tanto la información del match como de la propiedad
    // Solo necesitamos filtrar por esPendiente: true
    const pendientesItems = items.filter(item => item.esPendiente === true);
    // Guardar matches pendientes (objetos completos)
    matchesPendientes.value = pendientesItems.sort((a, b) => (a.matchId || 0) - (b.matchId || 0));
    // Para las propiedades renderizables, usar los mismos objetos ya que contienen toda la info
    propiedades.value = pendientesItems.sort((a, b) => (a.matchId || 0) - (b.matchId || 0));
    // Solo resetear a página 1 si no hay propiedades o si la página actual ya no es válida
    const newTotalPages = Math.ceil(propiedades.value.length / pageSize);
    if (propiedades.value.length === 0) {
      currentPage.value = 1;
    } else if (currentPage.value > newTotalPages) {
      currentPage.value = newTotalPages;
    }
  } catch (error) {
    console.error("Error al obtener matches sugeridos:", error);
    matchesPendientes.value = [];
    propiedades.value = [];
  } finally {
    isLoading.value = false;
  }
};
/**
 * Carga los matches enviados del cliente
 */
const cargarMatches = async () => {
  if (!clienteId.value) return;
  isLoadingMatches.value = true;
  try {
    const data = await matchService.getEnviadosByCliente(clienteId.value);
    let items = [];
    if (Array.isArray(data)) items = data;
    else if (Array.isArray(data?.items)) items = data.items;
    else if (Array.isArray(data?.$values)) items = data.$values;
    else if (Array.isArray(data?.items?.$values)) items = data.items.$values;
    // Enriquecer cada match con datos completos del inmueble
    const matchesEnriquecidos = await Promise.all(
      items.map(async (match) => {
        try {
          if (match.inmuebleId) {
            const inmuebleCompleto = await inmuebleService.getInmuebleById(match.inmuebleId);
            if (inmuebleCompleto) {
              return {
                ...match,
                inmueble: inmuebleCompleto,
                // También copiar algunos campos directamente para mayor compatibilidad
                titulo: inmuebleCompleto.titulo,
                nombre: inmuebleCompleto.nombre,
                slugInmueble: inmuebleCompleto.slugInmueble || inmuebleCompleto.slug
              };
            }
          }
          return match;
        } catch (error) {
          console.error(`❌ Error obteniendo inmueble para match ${match.id}:`, error);
          return match;
        }
      })
    );
    matchEnviados.value = matchesEnriquecidos.sort(
      (a, b) => new Date(b.fechaEnvio || 0) - new Date(a.fechaEnvio || 0)
    );
  } catch (error) {
    console.error("Error al cargar matches enviados:", error);
    matchEnviados.value = [];
  } finally {
    isLoadingMatches.value = false;
  }
};
/**
 * Devuelve el objeto Match completo si la propiedad está actualmente pendiente
 */
const getMatchForPropiedad = (propiedadId) => {
  // Los items son objetos que contienen toda la información del match y la propiedad
  // Buscamos por propiedadId
  const match = matchesPendientes.value.find(
    (item) => item.propiedadId === propiedadId
  );
  return match;
};
// Conjunto de ids de propiedades pendientes (si existen matches)
const pendientePropIds = computed(() => {
  const ids = new Set();
  (matchesPendientes.value || []).forEach((item) => {
    // Cada item tiene propiedadId directamente
    if (item.propiedadId != null) ids.add(item.propiedadId);
  });
  return ids;
});
/**
 * Determina si el checkbox debe estar marcado (es decir, si el Match ha sido enviado/procesado).
 * En la lista "pendientes", NINGUNO está enviado, por lo que el checkbox debe estar DESMARCADO.
 */
const isMatchEnviado = (propiedadId) => {
  // Si no tenemos matches pero sí propiedades, asumir que ninguna ha sido enviada (checkbox desmarcado)
  if (pendientePropIds.value.size === 0 && propiedades.value.length > 0)
    return false;
  // Si el id está en pendientes, no está enviado
  return !pendientePropIds.value.has(propiedadId);
};
/**
 * Maneja el cambio del checkbox: Marcar Match como Enviado.
 */
const handleMatchCheckbox = async (propiedad) => {
  // Obtener el ID correcto de la propiedad
  const propiedadId = propiedad.propiedadId || propiedad.id;
  const match = getMatchForPropiedad(propiedadId);
  if (!match) {
    return;
  }
  // El matchId es lo que necesitamos para el PUT
  const matchIdToUse = match.matchId;
  if (!matchIdToUse) {
    return;
  }
  isLoadingMatches.value = true;
  try {
    const result = await matchService.marcarComoEnviado(matchIdToUse);
    if (result.ok) {
      // Guardar el conteo anterior para comparación
      const countBefore = propiedades.value.length;
      // Refrescar completamente los datos desde el servidor
      await fetchMatchesPendientes(); // Recargar pendientes
      await cargarMatches(); // Recargar enviados
      // Esperar a que Vue actualice el DOM y los computed
      await nextTick();
      const countAfter = propiedades.value.length;
      // Si la diferencia es 0, significa que el servidor devuelve todos los matches
      // y solo cambia el flag esPendiente, por lo que debemos restar manualmente
      if (countBefore === countAfter && countAfter > 0) {
        // Filtrar localmente removiendo el match que acabamos de marcar como enviado
        const propiedadesFiltradas = propiedades.value.filter(prop => {
          const propId = prop.propiedadId || prop.id;
          return propId !== propiedadId;
        });
        propiedades.value = propiedadesFiltradas;
        matchesPendientes.value = propiedadesFiltradas;
      }
      // Verificar si la página actual es válida después de eliminar el item
      const newTotalPages = Math.ceil(propiedades.value.length / pageSize);
      if (currentPage.value > newTotalPages && newTotalPages > 0) {
        currentPage.value = newTotalPages;
      } else if (propiedades.value.length === 0) {
        currentPage.value = 1;
      }
      // Forzar actualización de la reactividad
      await nextTick();
      // Mostrar SweetAlert de éxito
      await Swal.fire({
        icon: "success",
        title: "¡Enviado exitosamente!",
        text: `La propiedad "${
          propiedad.titulo || propiedad.nombre || "Sin título"
        }" ha sido marcada como enviada.`,
        confirmButtonText: "Aceptar",
        timer: 3000,
        timerProgressBar: true,
      });
    } else {
      await Swal.fire({
        icon: "error",
        title: "Error",
        text: "El servicio no pudo procesar la solicitud correctamente.",
        confirmButtonText: "Aceptar",
      });
    }
  } catch (error) {
    console.error("💥 Error al marcar como enviado:", error);
    // Mostrar SweetAlert de error
    await Swal.fire({
      icon: "error",
      title: "Error al enviar",
      text: "Hubo un problema al marcar la propiedad como enviada. Por favor, inténtalo de nuevo.",
      confirmButtonText: "Aceptar",
    });
  } finally {
    isLoadingMatches.value = false;
  }
};
// --- 4. OTRAS FUNCIONES (Necesarias para tu template) ---
// Función helper para extraer el título de una propiedad/match
const getTituloPropiedad = (match) => {
  // Intentar múltiples ubicaciones para el título, priorizando los datos enriquecidos
  const titulo = 
    match.inmueble?.titulo ||
    match.inmueble?.nombre ||
    match.titulo ||
    match.nombre ||
    match.propiedad?.titulo ||
    match.propiedad?.nombre ||
    match.Inmueble?.titulo ||
    match.Inmueble?.nombre ||
    match.Propiedad?.titulo ||
    match.Propiedad?.nombre ||
    match.tituloPropiedad ||
    match.nombrePropiedad ||
    null;
  if (titulo) return titulo;
  // Si no hay título, usar código como fallback
  const codigo = 
    match.codigoPropiedad ||
    match.inmueble?.codigoPropiedad ||
    match.inmueble?.codigo ||
    match.propiedad?.codigo ||
    match.Inmueble?.codigo ||
    match.Propiedad?.codigo ||
    match.codigo ||
    "";
  return codigo ? `Propiedad ${codigo}` : "Sin título";
};
const buildInmuebleUrl = (propiedad) => {
  if (!propiedad) return null;
  // Intentar obtener el slug de múltiples posibles ubicaciones, priorizando datos enriquecidos
  const slug = 
    propiedad.inmueble?.slugInmueble || 
    propiedad.inmueble?.slug ||
    propiedad.slugInmueble || 
    propiedad.slug || 
    propiedad.Slug || 
    propiedad.SlugInmueble;
  if (slug) {
    return `https://homesguatemala.com/inmueble/${slug}`;
  }
  return null;
};
const openWhatsApp = (propiedad) => {
  const cli = props.clientDetails || {};
  const clientName = (cli.nombre || "").toString().trim();
  const shortName = clientName ? clientName.split(" ")[0] : "";
  const propiedadTitulo = propiedad.titulo || propiedad.nombre || "Propiedad";
  const propiedadUrl = buildInmuebleUrl(propiedad);
  const msgBase = `Hola ${
    shortName || clientName || "cliente"
  }, tenemos la siguiente propiedad disponible para usted: ${propiedadTitulo}`;
  const fullMsg = propiedadUrl ? `${msgBase}\n${propiedadUrl}` : msgBase;
  const mensajeCodificado = encodeURIComponent(fullMsg);
  const phoneCandidate =
    cli.telefonoCompleto ||
    cli.telefono ||
    cli.phone ||
    cli.celular ||
    cli.mobile ||
    null;
  const telefonoLimpio = formatPhoneForWhatsApp(phoneCandidate);
  const urlWhatsApp = telefonoLimpio
    ? `https://wa.me/${telefonoLimpio}?text=${mensajeCodificado}`
    : `https://wa.me/?text=${mensajeCodificado}`;
  window.open(urlWhatsApp, "_blank", "noopener,noreferrer");
};
const formatPhoneForWhatsApp = (phone) => {
  if (!phone) return null;
  const cleaned = phone.toString().replace(/\D/g, "");
  if (
    cleaned.startsWith("1") ||
    cleaned.startsWith("52") ||
    cleaned.startsWith("51") ||
    cleaned.startsWith("57") ||
    cleaned.startsWith("58") ||
    cleaned.length >= 11
  ) {
    return cleaned;
  }
  if (cleaned.length === 8) {
    return "502" + cleaned;
  }
  return cleaned;
};
const openInmuebleModal = async (propiedad) => {
  // 🔧 OBTENER DATOS COMPLETOS DEL SERVICIO
  const inmuebleId = propiedad.propiedadId || propiedad.id || propiedad.inmuebleId;
  try {
    // Llamar al servicio para obtener los datos normalizados
    const inmuebleCompleto = await inmuebleService.getInmuebleById(inmuebleId);
    if (inmuebleCompleto) {
      // Emitir con los datos completos normalizados
      emit("open-inmueble", { 
        value: inmuebleCompleto, 
        source: "matchPendiente" 
      });
    } else {
      console.error("❌ [matchPendiente] No se pudo obtener el inmueble completo");
    }
  } catch (error) {
    console.error("❌ [matchPendiente] Error al obtener inmueble:", error);
  }
};
const checkboxTitle = (propiedadId) => {
  // Si el match existe en la lista de pendientes, el título es "Marcar como enviado"
  return getMatchForPropiedad(propiedadId)
    ? "Marcar como enviado"
    : "Match ya procesado (Enviado/Eliminado)";
};
// Computed para mostrar enviados con información de propiedad
const enviadosConPropiedad = computed(() => {
  return matchEnviados.value.map((match) => {
    // Los datos ya vienen enriquecidos desde cargarMatches
    return match;
  });
});
// Función para formatear fechas
const formatDate = (dateString) => {
  if (!dateString) return "-";
  try {
    const date = new Date(dateString);
    return date.toLocaleDateString("es-GT", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    });
  } catch (error) {
    return "-";
  }
};
// Función para eliminar un match
const deleteMatch = async (matchId) => {
  if (!matchId) return;
  try {
    // Mostrar confirmación con SweetAlert
    const result = await Swal.fire({
      title: "¿Estás seguro?",
      text: "¿Quieres eliminar este match? Esta acción hará que la propiedad vuelva a aparecer en los matches sugeridos.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Sí, eliminar",
      cancelButtonText: "Cancelar",
    });
    if (!result.isConfirmed) return;
    isLoadingMatches.value = true;
    // Eliminar el match
    await matchService.deleteMatch(matchId);
    // Refrescar ambas listas: enviados y pendientes
    await Promise.all([
      cargarMatches(),        // Recargar enviados
      fetchMatchesPendientes() // Recargar pendientes (para que vuelva a aparecer)
    ]);
    // Mostrar mensaje de éxito
    await Swal.fire({
      icon: "success",
      title: "¡Eliminado!",
      text: "El match ha sido eliminado y la propiedad vuelve a estar disponible en los matches sugeridos.",
      confirmButtonText: "Aceptar",
      timer: 3000,
      timerProgressBar: true,
    });
  } catch (error) {
    console.error("Error al eliminar match:", error);
    // Mostrar mensaje de error
    await Swal.fire({
      icon: "error",
      title: "Error",
      text: "Error al eliminar el match. Por favor, inténtalo de nuevo.",
      confirmButtonText: "Aceptar",
    });
  } finally {
    isLoadingMatches.value = false;
  }
};
// --- 5. CICLO DE VIDA Y WATCHERS ---
// Observar cambios en clienteId para recargar los matches
watch(
  () => props.clienteId,
  (newId) => {
    if (newId) {
      fetchMatchesPendientes();
      cargarMatches(); // También cargar los enviados
    }
  },
  { immediate: true }
);
// Método público para refrescar desde el componente padre
const refresh = () => {
  fetchMatchesPendientes();
  cargarMatches();
};
// Exponer el método refresh para que el padre pueda llamarlo
defineExpose({
  refresh,
});
</script>
