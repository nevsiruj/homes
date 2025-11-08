<template>
  <div>
    <div class="mt-5 mb-12">
      <h2 class="text-2xl md:text-4xl lg:text-4xl font-extrabold">
        Búsqueda de Clientes
      </h2>
      <p class="mt-2 text-gray-600">
        Encuentra clientes según sus preferencias usando lenguaje natural o código de propiedad
      </p>
    </div>

    <!-- Formulario de búsqueda -->
    <form @submit.prevent="executeSearch" class="mb-6">
      <div class="mb-6">
        <label for="match" class="block mb-2 text-sm font-medium text-gray-900">
          Buscar por:
        </label>
        
        <!-- Selector de tipo de búsqueda -->
        <div class="mb-4">
          <div class="flex gap-4">
            <label class="flex items-center">
              <input
                v-model="searchType"
                type="radio"
                value="property"
                class="mr-2"
              />
              Código de propiedad
            </label>
            <label class="flex items-center">
              <input
                v-model="searchType"
                type="radio"
                value="preferences"
                class="mr-2"
              />
              Preferencias (texto libre)
            </label>
           
          </div>
        </div>

        <div class="flex gap-2">
          <input
            v-model="searchQuery"
            type="text"
            id="match"
            :placeholder="searchType === 'property' 
              ? 'Ingrese el código de propiedad (ej: ASR0000)' 
              : 'Ejemplo: \'Casa zona 14\', \'Business Center\', \'3 habitaciones hasta $300k\''"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
          />
          <button
            type="submit"
            :disabled="isLoading"
            class="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition-colors disabled:opacity-50"
          >
            {{ isLoading ? 'Buscando...' : 'Buscar' }}
          </button>
        </div>
        
        <p class="mt-2 text-sm text-gray-500">
          <span v-if="searchType === 'property'">
            Ingrese el ID o código de la propiedad para encontrar clientes que coincidan con sus características
          </span>
          <span v-else>
            Ejemplos: "casa en venta", "apartamento alquiler con business center", "3 habitaciones hasta $300k"
          </span>
        </p>
      </div>
    </form>

    <!-- Información de la propiedad encontrada -->
    <div
      v-if="searchType === 'property' && propertyInfo && showResults"
      class="mb-6 p-4 bg-blue-50 border border-blue-200 rounded-lg"
    >
      <h3 class="text-lg font-semibold text-blue-800 mb-2">Propiedad encontrada:</h3>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm">
        <div><strong>Título:</strong> {{ propertyInfo.titulo || 'N/A' }}</div>
        <div><strong>Tipo:</strong> {{ propertyInfo.tipos || 'N/A' }}</div>
        <div><strong>Operación:</strong> {{ propertyInfo.operaciones || 'N/A' }}</div>
        <div><strong>Ubicación:</strong> {{ propertyInfo.ubicaciones || 'N/A' }}</div>
        <div><strong>Precio:</strong> ${{ propertyInfo.precio ? propertyInfo.precio.toLocaleString() : 'N/A' }}</div>
        <div><strong>Habitaciones:</strong> {{ propertyInfo.habitaciones || 'N/A' }}</div>
        <div><strong>Baños:</strong> {{ propertyInfo.banos || 'N/A' }}</div>
        <div><strong>M²:</strong> {{ propertyInfo.metrosCuadrados || 'N/A' }}</div>
        <div v-if="propertyInfo.amenidades && propertyInfo.amenidades.length > 0" class="md:col-span-2">
          <strong>Amenidades:</strong> {{ propertyInfo.amenidades.join(', ') }}
        </div>
      </div>
    </div>

    <!-- Tabla de resultados -->
    <div
      v-if="showResults && filteredClients.length > 0"
      class="relative overflow-x-auto shadow-md sm:rounded-lg mt-6 transition-all duration-300"
    >
      <table
        class="w-full text-sm text-left rtl:text-right text-gray-500"
      >
        <thead
          class="text-xs uppercase bg-gray-50"
        >
          <tr>
            <th scope="col" class="px-6 py-3">Cliente</th>
            <th scope="col" class="px-6 py-3">Contacto</th>
            <th scope="col" class="px-6 py-3">Preferencias</th>
            <th scope="col" class="px-6 py-3">Agente</th>
            <th scope="col" class="px-6 py-3">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="cliente in filteredClients"
            :key="cliente.id"
            class="bg-white border-b hover:bg-gray-50"
          >    
            <td class="px-6 py-4 font-medium text-gray-900">
              {{ cliente.nombre }} {{ cliente.apellido }}
            </td>
            <td class="px-6 py-4">
              <div>Teléfono: {{ cliente.telefono }}</div>
              <div>Email: {{ cliente.email }}</div>
            </td>
            <td class="px-6 py-4">
              <div>
                <span class="font-semibold">Tipo:</span>
                {{ cliente.preferencias.tipo }}
              </div>
              <div>
                <span class="font-semibold">Operación:</span>
                {{ cliente.preferencias.operacion }}
              </div>
              <div>
                <span class="font-semibold">Ubicación:</span>
                {{ cliente.preferencias.ubicacion }}
              </div>
              <div v-if="cliente.preferencias.precioMin || cliente.preferencias.precioMax">
                <span class="font-semibold">Precio:</span> ${{ cliente.preferencias.precioMin?.toLocaleString() || "N/A"
                }} - ${{ cliente.preferencias.precioMax?.toLocaleString() || "N/A" }}
              </div>
              <div v-if="cliente.preferencias.habitaciones !== null">
                <span class="font-semibold">Habitaciones:</span>
                {{ cliente.preferencias.habitaciones }}
              </div>
              <div v-if="cliente.preferencias.banos !== null">
                <span class="font-semibold">Baños:</span>
                {{ cliente.preferencias.banos }}
              </div>
              <div v-if="cliente.preferencias.metrosCuadrados !== null">
                <span class="font-semibold">M²:</span>
                {{ cliente.preferencias.metrosCuadrados }}
              </div>
              <div v-if="cliente.preferencias.amenidades.length > 0">
                <span class="font-semibold">Amenidades:</span>
                <ul class="list-disc pl-5 mt-1 text-sm">
                  <li v-for="amenidad in cliente.preferencias.amenidades" :key="amenidad">
                    {{ amenidad }}
                  </li>
                </ul>
              </div>
              <div v-else>
                <span class="font-semibold">Amenidades:</span> Ninguna
              </div>
            </td>
                 <td class="px-6 py-4 text-gray-700">
        <div v-if="cliente.agenteNombre || cliente.agenteApellido">
          <span class="font-semibold">
            {{ cliente.agenteNombre }} {{ cliente.agenteApellido }}
          </span>
        </div>
        <div v-else class="italic text-gray-400">No asignado</div>
      </td>
        
            <td class="px-6 py-4 flex items-center space-x-3">
              <button
                @click="openClientDetails(cliente)"
                class="text-blue-600 hover:text-blue-800 font-medium"
                title="Ver detalles del cliente"
              >
                Detalles
              </button>
              <button
                @click="contactClient(cliente)"
                class="text-green-600 hover:text-green-800 font-medium"
                title="Contactar por WhatsApp"
              >
                Contactar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Mensaje si no hay resultados -->
    <div
      v-if="showResults && filteredClients.length === 0"
      class="mt-6 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative"
    >
      <span class="block sm:inline">
        {{ searchType === 'property' 
          ? `No se encontraron clientes que coincidan con la propiedad: "${searchQuery}"` 
          : `No se encontraron clientes para: "${searchQuery}"` }}
      </span>
    </div>

    <!-- Mensaje inicial opcional -->
    <div
      v-if="!searchQuery && !showResults"
      class="mt-4 text-gray-500"
    >
      Escribe algo para empezar a buscar clientes por sus preferencias o código de propiedad.
    </div>

    <!-- Modal detalle cliente -->
    <DetalleCliente
      :isOpen="isDetailsOpen"
      :cliente="selectedClient"
      @close="closeClientDetails"
      @open-inmueble="handleOpenInmueble"
      @edit-preferencia="handleEditPreferencia"
    />
  </div>
</template>

<script setup>
import { ref, computed } from "vue";
import clienteService from "@/services/clienteService";
import inmuebleService from "@/services/inmuebleService";
import DetalleCliente from "@/components/detalleCliente.vue";
import { useAuthStore } from "@/stores/auth";
import Swal from "sweetalert2";

const auth = useAuthStore();
const isAdmin = computed(() =>
  typeof auth.hasRole === "function"
    ? auth.hasRole("admin")
    : (auth.roles || []).includes("admin")
);

// === Variables reactivas ===
const searchQuery = ref("");
const searchType = ref("property"); // 'preferences' o 'property' - CAMBIADO A "property" por defecto
const showResults = ref(false);
const isLoading = ref(false);
const clientes = ref([]); // Todos los clientes cargados con sus preferencias
const filteredClients = ref([]);
const propertyInfo = ref(null); // Información de la propiedad encontrada
const uniquePreferences = ref({
  tipos: [],
  operaciones: [],
  ubicaciones: [],
  amenities: []
});

// === Variables para modal de detalle cliente ===
const isDetailsOpen = ref(false);
const selectedClient = ref(null);

definePageMeta({
  layout: "admin",
  requiresAuth: true,
});

// === Función para normalizar texto (sin acentos ni mayúsculas) ===
function normalize(str) {
  if (!str && str !== 0) return "";
  return String(str)
    .normalize("NFKD")
    .replace(/[\u0300-\u036F]/g, "")
    .toLowerCase()
    .trim();
}

// === Buscar clientes por código de propiedad ===
async function searchByCodigoPropiedad(codigoPropiedad) {
  if (!codigoPropiedad) return;

  console.log(`🔍 Iniciando búsqueda por código de propiedad: "${codigoPropiedad}"`);

  try {
    // Intentar cargar la propiedad desde inmuebleService
    // Ahora maneja tanto ID numérico como código alfanumérico
    console.log(`📡 Solicitando propiedad con ID/código: "${codigoPropiedad}"`);
    const propiedad = await inmuebleService.getInmuebleById(codigoPropiedad);

    console.log(`📊 Respuesta de la API:`, propiedad);

    if (!propiedad) {
      console.warn("❌ Propiedad no encontrada.");
      filteredClients.value = [];
      propertyInfo.value = null;
      return;
    }

    // Verificar que el código coincida (más flexible para códigos alfanuméricos)
    const codigoIngresado = codigoPropiedad.toString().toUpperCase().trim();
    const codigoEncontrado = (propiedad.codigoPropiedad || '').toUpperCase().trim();
    const idEncontrado = propiedad.id?.toString() || '';
    
    console.log(`🔍 Verificación de código:`);
    console.log(`   Código ingresado: "${codigoIngresado}"`);
    console.log(`   Código en propiedad: "${codigoEncontrado}"`);
    console.log(`   ID en propiedad: "${idEncontrado}"`);
    
    // Verificación más flexible para códigos alfanuméricos
    const esCoincidenciaExacta = codigoIngresado === codigoEncontrado || codigoIngresado === idEncontrado;
    const esCoincidenciaAproximada = codigoEncontrado.includes(codigoIngresado) || codigoIngresado.includes(codigoEncontrado);
    
    const esCoincidencia = esCoincidenciaExacta || esCoincidenciaAproximada;
    
    console.log(`   ¿Coincidencia exacta? ${esCoincidenciaExacta ? '✅' : '❌'}`);
    console.log(`   ¿Coincidencia aproximada? ${esCoincidenciaAproximada ? '✅' : '❌'}`);
    console.log(`   ¿Es coincidencia válida? ${esCoincidencia ? '✅' : '❌'}`);

    if (!esCoincidencia) {
      console.warn(`❌ El código "${codigoPropiedad}" no coincide suficientemente con la propiedad encontrada`);
      filteredClients.value = [];
      propertyInfo.value = null;
      return;
    }

    // Guardar información de la propiedad para mostrar
    propertyInfo.value = propiedad;
    console.log(`✅ Propiedad validada:`, {
      id: propiedad.id,
      codigoPropiedad: propiedad.codigoPropiedad,
      titulo: propiedad.titulo,
      tipos: propiedad.tipos,
      operaciones: propiedad.operaciones,
      ubicaciones: propiedad.ubicaciones,
      precio: propiedad.precio,
      habitaciones: propiedad.habitaciones,
      banos: propiedad.banos,
      amenidades: propiedad.amenidades
    });

    console.log(`👥 Iniciando comparación con ${clientes.value.length} clientes`);

    // Filtrar clientes basados en las preferencias y la propiedad
filteredClients.value = clientes.value.filter((cliente, index) => {
  const preferencias = cliente.preferencias;
  let matchScore = 0;
  let totalCriteria = 0;

  const tipoPropiedad = normalize(propiedad.tipos || '');
  const tipoCliente = normalize(preferencias.tipo || '');

  const operacionPropiedad = normalize(propiedad.operaciones || '');
  const operacionCliente = normalize(preferencias.operacion || '');

  // --- 1. TIPO (peso alto) ---
  totalCriteria++;
  const tipoMatch =
    tipoCliente === tipoPropiedad ||
    tipoCliente.includes(tipoPropiedad) ||
    tipoPropiedad.includes(tipoCliente) ||
    (tipoCliente.includes('apart') && tipoPropiedad.includes('depto')) || // sinónimos
    (tipoCliente.includes('depto') && tipoPropiedad.includes('apart'));

  if (tipoMatch) matchScore += 1;
  else matchScore += 0; // no eliminar automáticamente

  // --- 2. OPERACIÓN ---
  totalCriteria++;
  const opMatch =
    operacionCliente === operacionPropiedad ||
    operacionCliente.includes(operacionPropiedad) ||
    operacionPropiedad.includes(operacionCliente) ||
    (operacionCliente.includes('renta') && operacionPropiedad.includes('alquiler')) ||
    (operacionCliente.includes('alquiler') && operacionPropiedad.includes('renta')) ||
    (operacionCliente.includes('venta') && operacionPropiedad.includes('comprar')) ||
    (operacionCliente.includes('comprar') && operacionPropiedad.includes('venta'));

  if (opMatch) matchScore += 1;
  else matchScore += 0;

  // --- 3. PRECIO ---
  if (propiedad.precio && (preferencias.precioMin || preferencias.precioMax)) {
    totalCriteria++;
    const precioProp = Number(propiedad.precio);
    const precioMin = preferencias.precioMin || 0;
    const precioMax = preferencias.precioMax || Infinity;
    const margen = 0.25; // 25% de tolerancia

    const dentroRango = precioProp >= precioMin && precioProp <= precioMax;
    const dentroMargen =
      precioProp >= precioMin * (1 - margen) && precioProp <= precioMax * (1 + margen);

    if (dentroRango) matchScore += 1;
    else if (dentroMargen) matchScore += 0.7;
  }

  // --- 4. UBICACIÓN (opcional, pero suma) ---
  if (Array.isArray(preferencias.ubicacionesTokens) && preferencias.ubicacionesTokens.length > 0) {
    totalCriteria++;
    const ubicacionNorm = normalize(propiedad.ubicaciones || propiedad.ubicacion || '');
    const ubicMatch = preferencias.ubicacionesTokens.some(u => ubicacionNorm.includes(u));
    if (ubicMatch) matchScore += 1;
    else if (ubicacionNorm.split(' ').some(word => preferencias.ubicacionesTokens.includes(word)))
      matchScore += 0.5;
  }

  // --- 5. AMENIDADES (bonus) ---
  if (Array.isArray(preferencias.amenidades) && Array.isArray(propiedad.amenidades)) {
    totalCriteria++;
    const amenMatch = preferencias.amenidades.some(a =>
      propiedad.amenidades.map(x => normalize(x)).includes(a)
    );
    if (amenMatch) matchScore += 0.5;
  }

  // --- DECISIÓN FINAL ---
  const porcentaje = matchScore / Math.max(totalCriteria, 1);
  return porcentaje >= 0.5; // bajamos a 50 % para ser más inclusivos
});


    console.log(`\n🎉 Búsqueda completada. ${filteredClients.value.length} clientes encontrados de ${clientes.value.length} total.`);

  } catch (error) {
    console.error("❌ Error al buscar clientes por código de propiedad:", error);
    console.error("📋 Detalles del error:", {
      message: error.message,
      stack: error.stack,
      codigoPropiedad
    });
    
    // Mostrar mensaje más específico al usuario
    if (error.message.includes('400') || error.message.includes('validation')) {
      console.warn(`⚠️ Código "${codigoPropiedad}" no válido o no encontrado`);
    }
    
    filteredClients.value = [];
    propertyInfo.value = null;
  }
}

// === Cargar todos los clientes desde API con preferencias completas ===
const loadClientes = async () => {
  try {
    isLoading.value = true;
    const response = await clienteService.getAllClientes();

    if (response.$values && Array.isArray(response.$values)) {
      const listaClientes = response.$values.map(cliente => {
        let preferenciasRaw = {};

        if (
          cliente.preferencias &&
          Array.isArray(cliente.preferencias.$values) &&
          cliente.preferencias.$values.length > 0
        ) {
          preferenciasRaw = cliente.preferencias.$values[0];
        } else if (cliente.preferencias && typeof cliente.preferencias === "object") {
          preferenciasRaw = cliente.preferencias;
        }

        // Normalización
        const ubicacionRaw = preferenciasRaw.ubicacion ?? "";
        const ubicacionesTokens = String(ubicacionRaw)
          .split(/[,;]+/)
          .map(u => normalize(u))
          .filter(Boolean);

        let precioMin = Math.min(
          preferenciasRaw.precioMin ?? Infinity,
          preferenciasRaw.precioMax ?? Infinity
        );
        let precioMax = Math.max(
          preferenciasRaw.precioMin ?? -Infinity,
          preferenciasRaw.precioMax ?? -Infinity
        );

        return {
          id: cliente.id,
          nombre: cliente.nombre || "N/A",
          apellido: cliente.apellido || "",
          telefono: cliente.telefono || "N/A",
          email: cliente.email || "N/A",
          agenteId: cliente.agenteId, 
          agenteNombre: cliente.agente?.nombre || cliente.agenteNombre || null,
          agenteApellido: cliente.agente?.apellido || cliente.agenteApellido || null,
          preferencias: {
            tipo: (preferenciasRaw.tipo || "N/A").trim(),
            tipoNorm: normalize(preferenciasRaw.tipo || ""),
            operacion: (preferenciasRaw.operacion || "N/A").trim(),
            operacionNorm: normalize(preferenciasRaw.operacion || ""),
            ubicacion: (ubicacionRaw || "").trim(),
            ubicacionesTokens,
            precioMin: precioMin || null,
            precioMax: precioMax || null,
            habitaciones: preferenciasRaw.habitaciones ?? null,
            banos: preferenciasRaw.banos ?? null,
            metrosCuadrados: preferenciasRaw.metrosCuadrados ?? null,
            amenidades: Array.isArray(preferenciasRaw.preferenciaAmenidades?.$values)
              ? preferenciasRaw.preferenciaAmenidades.$values
                  .map(a => normalize(a.nombre))
                  .filter(Boolean)
              : []
          }
        };
      });

      // 👇 Aquí filtramos SOLO los clientes del agente logueado
      const agenteIdActual = auth.user?.id; // o auth.user?.agenteId según tu store
      clientes.value = listaClientes.filter(c => c.agenteId === agenteIdActual);

      extractUniquePreferences();
    } else {
      clientes.value = [];
      filteredClients.value = [];
    }
  } catch (error) {
    console.error("❌ Error al cargar clientes:", error);
  } finally {
    isLoading.value = false;
  }
};


// === Cargar clientes al inicio ===
loadClientes();

// === Extraer valores únicos de preferencias desde DB ===
function extractUniquePreferences() {
  const tipos = new Set();
  const operaciones = new Set();
  const ubicaciones = new Set();
  const amenities = new Set();

  clientes.value.forEach(cliente => {
    const pref = cliente.preferencias;

    if (pref.tipo && pref.tipo !== "N/A")
      tipos.add(normalize(pref.tipo));
    if (pref.operacion && pref.operacion !== "N/A")
      operaciones.add(normalize(pref.operacion));

    // ahora usamos tokens separados
    if (Array.isArray(pref.ubicacionesTokens)) {
      pref.ubicacionesTokens.forEach(u => {
        if (u) ubicaciones.add(u);
      });
    } else if (pref.ubicacion) {
      // fallback: tokenizar si no existe el array
      String(pref.ubicacion).split(/[,;]+/).forEach(u => {
        const n = normalize(u);
        if (n) ubicaciones.add(n);
      });
    }

    if (Array.isArray(pref.amenidades)) {
      pref.amenidades.forEach(am => {
        if (am) amenities.add(normalize(am));
      });
    }
  });

  uniquePreferences.value = {
    tipos: Array.from(tipos),
    operaciones: Array.from(operaciones),
    ubicaciones: Array.from(ubicaciones),
    amenities: Array.from(amenities)
  };
}

// --- Reemplazar executeSearch para manejar ambos tipos de búsqueda ---
async function executeSearch() {
  const raw = (searchQuery.value || "").trim();
  
  if (!raw) {
    filteredClients.value = [...clientes.value];
    showResults.value = true;
    return;
  }

  isLoading.value = true;
  propertyInfo.value = null;

  try {
    if (searchType.value === 'property') {
      // Búsqueda por código de propiedad
      await searchByCodigoPropiedad(raw);
    } else {
      // Búsqueda por preferencias (código existente)
      const f = parseQuery(raw);

      const resultados = clientes.value.filter((cliente) => {
        const pref = cliente.preferencias || {};
        const tipoNorm = normalize(pref.tipo);
        const operNorm = normalize(pref.operacion);
        const ubicTokens = Array.isArray(pref.ubicacionesTokens) ? pref.ubicacionesTokens : String(pref.ubicacion || "").split(/[,;]+/).map(u=>normalize(u)).filter(Boolean);
        const precioMinPref = pref.precioMin ?? null;
        const precioMaxPref = pref.precioMax ?? null;
        const habitacionesPref = pref.habitaciones ?? null;
        const banosPref = pref.banos ?? null;
        const amenPref = Array.isArray(pref.amenidades) ? pref.amenidades.map(a => normalize(a)) : [];

        // Tipo: solo si se detectó en la query; si no fue detectado, no filtrar por tipo
        if (f.tipo && !tipoNorm.includes(f.tipo)) return false;

        // Operacion: si se detectó en la query, debe coincidir
        if (f.operacion && !operNorm.includes(f.operacion)) return false;

        // Ubicación: usar tokens (cliente puede tener múltiples zonas)
        if (f.ubicacion) {
          const matchUb = ubicTokens.some(u => u === f.ubicacion || u.includes(f.ubicacion) || f.ubicacion.includes(u));
          if (!matchUb) return false;
        }

        // Precio: comprobar solapamiento
        if (f.precioMin !== null || f.precioMax !== null) {
          if (precioMinPref === null && precioMaxPref === null) return false;
          const minA = precioMinPref ?? -Infinity;
          const maxA = precioMaxPref ?? Infinity;
          const minB = f.precioMin ?? -Infinity;
          const maxB = f.precioMax ?? Infinity;
          if (maxA < minB || minA > maxB) return false;
        }

        // Habitaciones / baños
        if (f.habitaciones !== null && (habitacionesPref === null || habitacionesPref < f.habitaciones)) return false;
        if (f.banos !== null && (banosPref === null || banosPref < f.banos)) return false;

        // Amenidades: subset
        if (f.amenities.length > 0) {
          const allIncluded = f.amenities.every(a => amenPref.some(x => x.includes(a)));
          if (!allIncluded) return false;
        }

        return true;
      });

      filteredClients.value = resultados;
    }

    showResults.value = true;
  } catch (error) {
    //console.error("Error en la búsqueda:", error);
  } finally {
    isLoading.value = false;
  }
}

// === Funciones parseQuery existentes (se mantienen igual) ===
function parseNumberToken(tok) {
  if (!tok) return null;
  const cleaned = tok.replace(/[^\d]/g, "");
  if (!cleaned) return null;
  return Number(cleaned);
}

function parseQuery(q) {
  const qNorm = normalize(q);
  const tokens = qNorm.split(/\s+/).filter(Boolean);

  const filters = {
    tipo: null,
    operacion: null,
    ubicacion: null,
    precioMin: null,
    precioMax: null,
    habitaciones: null,
    banos: null,
    amenities: [],
  };

  // detect tipo / operacion / ubicacion from uniquePreferences (todos normalizados)
  const detectInList = (list) => {
    for (const item of list) {
      if (!item) continue;
      if (qNorm.includes(item)) return item;
      // token equal
      if (tokens.includes(item.split(/\s+/)[0])) return item;
    }
    return null;
  };

  filters.tipo = detectInList(uniquePreferences.value.tipos);
  filters.operacion = detectInList(uniquePreferences.value.operaciones);
  filters.ubicacion = detectInList(uniquePreferences.value.ubicaciones);

  // --- Preparar una versión de la query SIN los patrones de "zona" para parsear precios ---
  // elimina "zona 10", "zona10", "z10", "z.10" etc.
  const qForPrice = qNorm.replace(/\b(zona|z\.?|z)\s*\d+\b/g, "").replace(/\b(zone)\s*\d+\b/g,"");

  // Numero / precios: buscar "hasta X", "desde X", "X - Y", "entre X y Y", "Xk"
  const kReplacer = (s) => {
    if (!s) return null;
    const kMatch = s.match(/(\d+[.,]?\d*)\s*(k|mil)/);
    if (kMatch) return Number(kMatch[1].replace(/[.,]/g, "")) * 1000;
    const numMatch = s.match(/(\d+[.,]?\d*)/);
    return numMatch ? Number(numMatch[1].replace(/[.,]/g, "")) : null;
  };

  const rangeMatch = qForPrice.match(/(\d[\d\.,]*)\s*(?:-|a|to|hasta)\s*(\d[\d\.,]*)(k|mil)?/);
  if (rangeMatch) {
    filters.precioMin = kReplacer(rangeMatch[1]) ?? null;
    filters.precioMax = kReplacer(rangeMatch[2]) ?? null;
  } else {
    const hastaMatch = qForPrice.match(/(?:hasta|max(?:imo)?|menos de)\s*\$?\s*(\d[\d\.,]*\s*(?:k|mil)?)/);
    if (hastaMatch) {
      filters.precioMax = kReplacer(hastaMatch[1]) ?? null;
    }
    const desdeMatch = qForPrice.match(/(?:desde|mas de|más de|>=|mayor que)\s*\$?\s*(\d[\d\.,]*\s*(?:k|mil)?)/);
    if (desdeMatch) {
      filters.precioMin = kReplacer(desdeMatch[1]) ?? null;
    }

    // standalone numbers: sólo interpretarlas como precio si vienen con $ o sufijo k/mil o son >= 1000
    if (filters.precioMin === null && filters.precioMax === null) {
      const standaloneMatches = [...qForPrice.matchAll(/\$?\s*(\d[\d\.,]*\s*(?:k|mil)?)/g)];
      const detectedPrices = [];
      for (const m of standaloneMatches) {
        const raw = m[0];
        const numToken = m[1];
        if (!numToken) continue;
        const hasDollar = /\$/.test(raw);
        const hasK = /k|mil/.test(raw);
        const value = kReplacer(numToken);
        if (value === null) continue;
        // aceptar como precio si tiene $ ó sufijo ó >= 1000
        if (hasDollar || hasK || (typeof value === "number" && value >= 1000)) {
          detectedPrices.push(value);
        }
      }
      if (detectedPrices.length > 0) {
        detectedPrices.sort((a, b) => a - b);
        filters.precioMin = 0;
        filters.precioMax = detectedPrices[detectedPrices.length - 1];
      }
    }
  }

  // Habitaciones / baños
  const habMatch = qNorm.match(/(\d+)\s*(habitaciones|hab|hab\.)/);
  if (habMatch) filters.habitaciones = parseInt(habMatch[1], 10);
  const banMatch = qNorm.match(/(\d+)\s*(baños|banos|baño|bano|bañ?o)/);
  if (banMatch) filters.banos = parseInt(banMatch[1], 10);

  // Amenities detection
  const foundAmenities = [];
  for (const a of uniquePreferences.value.amenities) {
    if (!a) continue;
    if (qNorm.includes(a)) foundAmenities.push(a);
    else {
      const token = a.split(/\s+/)[0];
      if (tokens.includes(token)) foundAmenities.push(a);
    }
  }
  filters.amenities = Array.from(new Set(foundAmenities));

  return filters;
}

// === Abrir detalles del cliente ===
async function openClientDetails(cliente) {
  try {
    isDetailsOpen.value = true;
    console.log(`📋 Cargando detalles del cliente: ${cliente.nombre} ${cliente.apellido}`);
    
    // Obtener detalles completos del cliente desde la API
    const response = await clienteService.getClienteById(cliente.id);
    const clienteData = response.$values ? response.$values[0] : response;

    if (!clienteData) {
      throw new Error("No se pudieron cargar los detalles del cliente");
    }

    console.log(`📊 Datos del cliente recibidos:`, clienteData);

    // === PROCESAMIENTO EXACTO IGUAL QUE EN CLIENTES.VUE ===
    
    // Mantener la estructura completa de preferencias (puede venir como { $values: [...] })
    const rawPrefsArr = clienteData.preferencias?.$values ?? (Array.isArray(clienteData.preferencias) ? clienteData.preferencias : []);
    const firstPref = rawPrefsArr && rawPrefsArr.length ? rawPrefsArr[0] : null;

    console.log(`📋 Preferencias procesadas:`, { rawPrefsArr, firstPref });

    // extraer amenidades del primer elemento para la vista resumen (compatibilidad)
    const summaryAmenidades = [];
    if (firstPref?.preferenciaAmenidades?.$values) {
      summaryAmenidades.push(...firstPref.preferenciaAmenidades.$values.map((a) => a.nombre || a.Nombre));
    } else if (Array.isArray(firstPref?.amenidades)) {
      summaryAmenidades.push(...firstPref.amenidades);
    }

    console.log(`🏷️ Amenidades resumen:`, summaryAmenidades);

    selectedClient.value = {
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

    console.log(`✅ Cliente procesado para modal:`, selectedClient.value);
    console.log(`✅ Modal de detalles abierto para cliente ID: ${cliente.id}`);

  } catch (error) {
    console.error("❌ Error al cargar detalles del cliente:", error);
    isDetailsOpen.value = false;
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "No se pudieron cargar los detalles del cliente: " + (error.message || "Error desconocido"),
      confirmButtonText: "Aceptar"
    });
  } finally {
    isLoading.value = false;
  }
}

// === Cerrar modal de detalles ===
function closeClientDetails() {
  isDetailsOpen.value = false;
  selectedClient.value = null;
}

// === Contactar cliente por WhatsApp ===
function contactClient(cliente) {
  try {
    const telefono = cliente.telefono || "";
    const nombre = `${cliente.nombre || ""} ${cliente.apellido || ""}`.trim();

    if (!telefono) {
      Swal.fire({
        icon: "warning",
        title: "Sin teléfono",
        text: `El cliente ${nombre} no tiene número de teléfono registrado.`,
        confirmButtonText: "Aceptar"
      });
      return;
    }

    // Limpiar el número de teléfono (remover espacios, guiones, paréntesis)
    const telefonoLimpio = telefono.replace(/[\s\-\(\)\+]/g, "");

    // Construir URL de la propiedad si aplica
    const slug = searchType.value === "property" ? propertyInfo.value?.slugInmueble : null;
    const propertyUrl = slug ? `https://homesguatemala.com/inmueble/${slug}` : "";

    // Construir mensaje personalizado
    let mensaje = `Hola ${nombre || "cliente"}, le puede interesar esta propiedad.`;
    if (propertyUrl) {
      mensaje += `\n\n${propertyUrl}`;
    }
    mensaje += `\n\n\nQuedo atento a sus comentarios.`

    // Construir URL de WhatsApp Web
    const whatsappUrl = `https://wa.me/${telefonoLimpio}?text=${encodeURIComponent(mensaje)}`;

    // Abrir en nueva pestaña
    window.open(whatsappUrl, "_blank", "noopener,noreferrer");
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "No se pudo abrir WhatsApp. Verifique el número de teléfono.",
      confirmButtonText: "Aceptar"
    });
  }
}

// === Handlers para eventos del modal DetalleCliente ===
function handleOpenInmueble(payload) {
  // Manejar apertura de modal de inmueble si es necesario
  console.log("📋 Solicitud de abrir inmueble:", payload);
}

function handleEditPreferencia(payload) {
  // Manejar edición de preferencia si es necesario
  console.log("📝 Solicitud de editar preferencia:", payload);
}
</script>