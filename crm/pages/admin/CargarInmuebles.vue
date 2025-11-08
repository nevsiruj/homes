<template>
  <div class="p-6 bg-white rounded-lg shadow-sm">
    <h1 class="text-2xl font-bold mb-6 text-gray-900">Cargar Inmuebles desde Excel</h1>

    <div class="mb-6 p-4 border border-gray-200 rounded-lg bg-gray-50">
      <h2 class="text-lg font-semibold mb-2 text-gray-900">Instrucciones:</h2>
      <ol class="list-decimal pl-5 space-y-1 text-sm text-gray-700">
        <li>Descarga la <strong>plantilla de ejemplo</strong> para asegurar el formato correcto.</li>
        <li>Llena los datos de los inmuebles manteniendo las columnas.</li>
        <li>
          <strong>Prepara tus imágenes:</strong> Coloca todas las imágenes (tanto principales como de referencia) que nombrarás en el Excel, en una <strong>única carpeta en tu computadora local</strong>.
          <br>
          Ejemplo: Si tu Excel dice <code>imagen1.jpg</code> y <code>imagen2.png</code>, ambas deben estar en la carpeta que seleccionarás.
        </li>
        <li>Selecciona el archivo Excel para cargar.</li>
        <li><strong>Selecciona la carpeta de imágenes local</strong> desde donde se tomarán los archivos.</li>
        <li>Revisa la vista previa antes de confirmar.</li>
      </ol>

      <div class="mt-4">
        <button
          @click="downloadTemplate"
          class="text-white bg-gray-600 hover:bg-gray-700 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2 mr-2 focus:outline-none"
        >
          <svg class="w-4 h-4 mr-2 inline" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"></path>
          </svg>
          Descargar Plantilla
        </button>
      </div>
    </div>

    <div class="mb-6">
      <label class="block mb-2 text-sm font-medium text-gray-900">1. Seleccionar archivo Excel (.xlsx)</label>
      <input
        type="file"
        @change="handleExcelUpload"
        accept=".xlsx, .xls"
        class="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 focus:outline-none"
      >
      <p class="mt-1 text-sm text-gray-500">Formatos soportados: .xlsx, .xls</p>
    </div>

    <div class="mb-6">
      <label class="block mb-2 text-sm font-medium text-gray-900">2. Seleccionar carpeta con imágenes</label>
      <input
        type="file"
        @change="handleImageFolderSelection"
        webkitdirectory
        directory
        class="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer bg-gray-50 focus:outline-none"
      >
      <p class="mt-1 text-sm text-gray-500">Selecciona la carpeta que contiene todas tus imágenes.</p>
      <p v-if="selectedImageFiles.size > 0" class="mt-1 text-sm text-gray-600">
        {{ selectedImageFiles.size }} imágenes detectadas en la carpeta.
      </p>
    </div>

    <div v-if="loading" class="text-center py-8">
      <div class="inline-block animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-gray-500"></div>
      <p class="mt-2 text-gray-700">Procesando archivo Excel...</p>
    </div>

    <div v-if="error" class="p-4 mb-4 text-sm text-red-700 bg-red-100 rounded-lg">
      <p class="font-bold">Error:</p>
      <pre class="error-details">{{ error }}</pre>
    </div>

    <div v-if="previewData.length > 0" class="mt-6">
      <h2 class="text-xl font-semibold mb-4 text-gray-900">Vista Previa ({{ previewData.length }} inmuebles)</h2>

      <div class="overflow-x-auto relative shadow-md sm:rounded-lg">
        <table class="w-full text-sm text-left text-gray-500">
          <thead class="text-xs text-gray-700 uppercase bg-gray-50">
            <tr>
              <th scope="col" class="py-3 px-6">Código</th>
              <th scope="col" class="py-3 px-6">Título</th>
              <th scope="col" class="py-3 px-6">Tipo</th>
              <th scope="col" class="py-3 px-6">Operación</th>
              <th scope="col" class="py-3 px-6">Precio</th>
              <th scope="col" class="py-3 px-6">Estado</th>
              <th scope="col" class="py-3 px-6">Imágenes Encontradas</th>
              <!-- NUEVA ÚLTIMA COLUMNA -->
              <th scope="col" class="py-3 px-6">Slug</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in previewData" :key="index" class="bg-white border-b">
              <td class="py-4 px-6">{{ item.codigoPropiedad }}</td>
              <td class="py-4 px-6">{{ item.titulo }}</td>
              <td class="py-4 px-6">{{ item.tipos }}</td>
              <td class="py-4 px-6">{{ item.operaciones }}</td>
              <td class="py-4 px-6">${{ item.precio?.toLocaleString() }}</td>
              <td class="py-4 px-6">
                <span v-if="item.valid" class="bg-green-100 text-green-800 text-xs font-medium px-2.5 py-0.5 rounded">Válido</span>
                <span v-else class="bg-red-100 text-red-800 text-xs font-medium px-2.5 py-0.5 rounded">Error</span>
              </td>
              <td class="py-4 px-6">
                <span v-if="item.allImagesFound" class="bg-green-100 text-green-800 text-xs font-medium px-2.5 py-0.5 rounded">Todas</span>
                <span v-else class="bg-yellow-100 text-yellow-800 text-xs font-medium px-2.5 py-0.5 rounded">Faltantes</span>
              </td>
              <!-- NUEVA ÚLTIMA COLUMNA -->
              <td class="py-4 px-6">{{ item.slugInmueble }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="mt-6 flex justify-end">
        <button
          @click="confirmUpload"
          :disabled="!isValid || uploadingItems || selectedImageFiles.size === 0"
          class="text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 focus:outline-none disabled:opacity-50 disabled:cursor-not-allowed"
        >
          <svg v-if="uploadingItems" class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          {{ uploadingItems ? "Importando..." : "Confirmar Importación" }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import * as XLSX from 'xlsx';
import inmuebleService from '../services/inmuebleService'; // verifica la ruta a tu servicio
import { useRouter } from 'vue-router';
import Swal from 'sweetalert2';

const router = useRouter();
const previewData = ref([]);
const loading = ref(false);
const uploadingItems = ref(false);
const error = ref(null);

// Mapa de archivos de imágenes seleccionadas
const selectedImageFiles = ref(new Map());

// Amenidades disponibles (asegúrate que coincidan con tu backend)
const amenidadesDisponibles = ref([
  { id: 1, nombre: "Airbnb" },
  { id: 2, nombre: "Gimnasio" },
  { id: 3, nombre: "Área verde" },
  { id: 4, nombre: "Balcón" },
  { id: 5, nombre: "Parqueo de Visitas" },
  { id: 6, nombre: "Bodega" },
  { id: 7, nombre: "Estudio" },
  { id: 8, nombre: "Jardin" },
  { id: 9, nombre: "Lavandería" },
  { id: 10, nombre: "Cuarto de Servicio" },
  { id: 11, nombre: "Área de Juegos" },
  { id: 12, nombre: "Business Center" },
  { id: 13, nombre: "Pet friendly" },
  { id: 14, nombre: "Salón Social" },
  { id: 15, nombre: "Piscina" },
  { id: 16, nombre: "Pet Garden" },
  { id: 17, nombre: "Amueblado" },
  { id: 18, nombre: "No mascotas" },
  { id: 19, nombre: "Sala familiar" },
  { id: 20, nombre: "Canchas deportivas" },
]);

// === NUEVO: función para generar slug (si no llega desde el Excel)
const slugify = (title = "", code = "") => {
  const t = (title || "")
    .normalize("NFD").replace(/[\u0300-\u036f]/g, "")
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)/g, "");
  const c = (code || "")
    .toString()
    .trim()
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "");
  return c ? `${t}-${c}` : t;
};

// Validez general para habilitar el botón
const isValid = computed(() => {
  return previewData.value.length > 0 && previewData.value.every(item => item.valid) && selectedImageFiles.value.size > 0;
});

// ===== Descarga de Plantilla (AGREGADO Slug Inmueble como última columna) =====
const downloadTemplate = async () => {
  try {
    const workbook = XLSX.utils.book_new();

    const sampleData = [
      {
        'Código Propiedad': 'PROP-001',
        'Título': 'Hermosa casa en zona residencial',
        'Tipo Propiedad': 'Casa',
        'Operación': 'Venta',
        'Ubicación': 'Sector Norte, Ciudad',
        'Precio': 250000,
        'Habitaciones': 3,
        'Baños': 2,
        'Parqueos': 1,
        'Metros Cuadrados': 180,
        'Luxury': 'Sí',
        'Imagen Principal': 'casa-principal.jpg',
        'Imágenes Referencia': 'casa1.jpg, casa2.jpg, otra-imagen.png',
        'Amenidades': 'Piscina, Área verde, Jardín',
        'Descripción': 'Amplia casa con excelentes acabados y jardín privado.',
        'Video URL': 'https://www.youtube.com/watch?v=ejemplo',
        // ÚLTIMA COLUMNA
        'Slug Inmueble': 'hermosa-casa-en-zona-residencial-prop-001'
      },
      {
        'Código Propiedad': 'APTO-002',
        'Título': 'Apartamento moderno en el centro',
        'Tipo Propiedad': 'Apartamento',
        'Operación': 'Alquiler',
        'Ubicación': 'Zona Central, Ciudad',
        'Precio': 1200,
        'Habitaciones': 2,
        'Baños': 2,
        'Parqueos': 1,
        'Metros Cuadrados': 90,
        'Luxury': 'No',
        'Imagen Principal': 'apartamento-centro.jpeg',
        'Imágenes Referencia': 'cocina-apto.jpg, balcon-apto.jpg',
        'Amenidades': 'Gimnasio, Balcón',
        'Descripción': 'Luminoso apartamento con vista a la ciudad.',
        'Video URL': '',
        // ÚLTIMA COLUMNA
        'Slug Inmueble': 'apartamento-moderno-en-el-centro-apto-002'
      }
    ];

    const worksheet = XLSX.utils.json_to_sheet(sampleData);
    XLSX.utils.book_append_sheet(workbook, worksheet, "Inmuebles");

    XLSX.writeFile(workbook, "Plantilla_Inmuebles.xlsx");
    Swal.fire('¡Éxito!', 'La plantilla se ha descargado correctamente.', 'success');
  } catch (err) {
    console.error("Error al generar plantilla:", err);
    Swal.fire('Error', 'No se pudo generar la plantilla de ejemplo.', 'error');
    error.value = "Error al generar la plantilla de ejemplo";
  }
};

// Selección de carpeta de imágenes
const handleImageFolderSelection = (event) => {
  selectedImageFiles.value.clear();
  const files = event.target.files;
  if (files.length === 0) return;

  for (let i = 0; i < files.length; i++) {
    const file = files[i];
    selectedImageFiles.value.set(file.name.toLowerCase(), file);
  }
  if (previewData.value.length > 0) {
    validatePreviewImages();
  }
};

// Subida de archivo a tu endpoint
async function uploadImageFile(file) {
  const formData = new FormData();
  formData.append("Image", file);

  try {
    const response = await fetch(
      "https://app-pool.vylaris.online/dcmigserver/api/Upload/homes",
      { method: "POST", body: formData }
    );

    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(`HTTP error! status: ${response.status} - ${errorText}`);
    }

    const data = await response.json();
    return {
      url: `https://app-pool.vylaris.online/dcmigserver/homes/${data.path}`,
      path: data.path,
    };
  } catch (error) {
    console.error("Fallo al subir imagen a api/Upload/homes:", error.message);
    return null;
  }
}

// Normalización de strings
const normalizeString = (str) => {
  if (typeof str !== 'string') return '';
  return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase().trim();
};

// Procesar Excel (AGREGADO lectura/generación de slugInmueble)
const handleExcelUpload = (event) => {
  const file = event.target.files[0];
  if (!file) {
    previewData.value = [];
    error.value = null;
    return;
  }

  loading.value = true;
  previewData.value = [];
  error.value = null;

  const reader = new FileReader();
  reader.onload = (e) => {
    try {
      const data = new Uint8Array(e.target.result);
      const workbook = XLSX.read(data, { type: 'array' });
      const firstSheetName = workbook.SheetNames[0];
      const worksheet = workbook.Sheets[firstSheetName];
      const jsonData = XLSX.utils.sheet_to_json(worksheet);

      if (jsonData.length === 0) {
        throw new Error("El archivo Excel no contiene datos o está vacío.");
      }

      previewData.value = jsonData.map(item => {
        // Amenidades
        const amenidadesNames = item['Amenidades']?.toString().split(',').map(a => a.trim()).filter(a => a) || [];
        const amenidadesMapped = amenidadesNames.map(name => {
          const normalizedName = normalizeString(name);
          const amenidad = amenidadesDisponibles.value.find(a => normalizeString(a.nombre) === normalizedName);
          return amenidad ? { amenidadInmuebleId: amenidad.id, nombre: amenidad.nombre } : null;
        }).filter(a => a !== null);

        // Imágenes (nombres)
        const principalImageName = item['Imagen Principal']?.toString() || '';
        const referenceImageNames = item['Imágenes Referencia']?.toString()
          .split(',')
          .map(i => i.trim())
          .filter(i => i) || [];

        const mappedItem = {
          codigoPropiedad: item['Código Propiedad']?.toString() || '',
          titulo: item['Título']?.toString() || '',
          tipos: item['Tipo Propiedad']?.toString() || '',
          operaciones: item['Operación']?.toString() || '',
          ubicaciones: item['Ubicación']?.toString() || '',
          precio: parseFloat(item['Precio']) || 0,
          habitaciones: parseInt(item['Habitaciones']) || 0,
          banos: parseInt(item['Baños']) || 0,
          parqueos: parseInt(item['Parqueos']) || 0,
          metrosCuadrados: parseFloat(item['Metros Cuadrados']) || 0,
          luxury: ['sí', 'si', 'yes', 'true', '1'].includes(item['Luxury']?.toString().toLowerCase()),
          imagenPrincipalName: principalImageName,
          contenido: item['Descripción']?.toString() || '',
          amenidades: amenidadesMapped,
          imagenesReferenciaNames: referenceImageNames,
          video: item['Video URL']?.toString() || '',
          finalImagenPrincipalUrl: null,
          finalImagenesReferenciaUrls: [],
          allImagesFound: false
        };

        // === NUEVO: leer/generar slugInmueble (última columna lógica) ===
        const excelSlug = item['Slug Inmueble']?.toString().trim() || '';
        const autoSlug = slugify(mappedItem.titulo, mappedItem.codigoPropiedad);
        mappedItem.slugInmueble = excelSlug || autoSlug;

        // Validación básica
        mappedItem.valid = [
          mappedItem.codigoPropiedad,
          mappedItem.titulo,
          mappedItem.tipos,
          mappedItem.operaciones,
          mappedItem.ubicaciones,
          mappedItem.imagenPrincipalName,
          mappedItem.contenido
        ].every(field => field && field.toString().trim() !== '');

        if (!mappedItem.valid) {
          console.warn(`Item inválido detectado (Código: ${mappedItem.codigoPropiedad || 'N/A'}):`, item);
        }

        return mappedItem;
      });

      // Validar existencia de imágenes en carpeta
      validatePreviewImages();

      if (previewData.value.every(item => !item.valid)) {
        error.value = "Ninguno de los registros en el archivo Excel parece ser válido o faltan datos esenciales. Por favor, revisa la plantilla y los datos.";
        previewData.value = [];
      }

    } catch (err) {
      console.error("Error al procesar archivo:", err);
      error.value = `Error al procesar el archivo. Verifica el formato y que no esté vacío: ${err.message}`;
      previewData.value = [];
    } finally {
      loading.value = false;
    }
  };
  reader.readAsArrayBuffer(file);
};

// Validar imágenes contra carpeta seleccionada
const validatePreviewImages = () => {
  if (selectedImageFiles.value.size === 0 || previewData.value.length === 0) {
    previewData.value.forEach(item => item.allImagesFound = false);
    return;
  }

  previewData.value.forEach(item => {
    let imagesFoundCount = 0;
    let totalImagesNeeded = 0;

    if (item.imagenPrincipalName) {
      totalImagesNeeded++;
      if (selectedImageFiles.value.has(item.imagenPrincipalName.toLowerCase())) {
        imagesFoundCount++;
      } else {
        console.warn(`Imagen principal no encontrada: ${item.imagenPrincipalName} (${item.codigoPropiedad})`);
      }
    }

    item.imagenesReferenciaNames.forEach(refName => {
      totalImagesNeeded++;
      if (selectedImageFiles.value.has(refName.toLowerCase())) {
        imagesFoundCount++;
      } else {
        console.warn(`Imagen de referencia no encontrada: ${refName} (${item.codigoPropiedad})`);
      }
    });

    item.allImagesFound = (totalImagesNeeded > 0 && imagesFoundCount === totalImagesNeeded) || totalImagesNeeded === 0;
    if (item.valid && !item.allImagesFound && totalImagesNeeded > 0) {
      item.valid = false;
      item.errorMessage = 'Faltan imágenes referenciadas en la carpeta seleccionada.';
    }
  });
};

// Confirmar y subir al backend (AGREGADO slugInmueble en datosEnviar)
const confirmUpload = async () => {
  if (!isValid.value) {
    let message = 'No hay datos válidos para importar o el archivo está vacío.';
    if (previewData.value.length > 0 && !previewData.value.every(item => item.valid)) {
      message = 'Algunos inmuebles tienen errores o faltan datos (revisa "Estado").';
    } else if (selectedImageFiles.value.size === 0) {
      message = 'Selecciona la carpeta que contiene las imágenes antes de importar.';
    }
    Swal.fire('Advertencia', message, 'warning');
    return;
  }

  uploadingItems.value = true;
  error.value = null;

  Swal.fire({
    title: 'Preparando importación...',
    html: 'Subiendo imágenes. No cierres esta ventana.',
    timerProgressBar: true,
    allowOutsideClick: false,
    didOpen: () => Swal.showLoading(),
  });

  const validItems = previewData.value.filter(item => item.valid && item.allImagesFound);
  const results = [];
  let processedCount = 0;

  // 1) Subir imágenes
  for (const item of validItems) {
    processedCount++;
    Swal.update({
      title: `Subiendo imágenes para ${item.codigoPropiedad}... (${processedCount} de ${validItems.length})`,
      html: `Subiendo imagen principal y referencias...`
    });

    try {
      if (item.imagenPrincipalName) {
        const principalFile = selectedImageFiles.value.get(item.imagenPrincipalName.toLowerCase());
        if (principalFile) {
          const uploadedMain = await uploadImageFile(principalFile);
          if (uploadedMain?.url) item.finalImagenPrincipalUrl = uploadedMain.url;
          else throw new Error(`Fallo al subir imagen principal: ${item.imagenPrincipalName}`);
        } else {
          throw new Error(`Imagen principal no encontrada: ${item.imagenPrincipalName}`);
        }
      }

      item.finalImagenesReferenciaUrls = [];
      for (const refName of item.imagenesReferenciaNames) {
        const refFile = selectedImageFiles.value.get(refName.toLowerCase());
        if (refFile) {
          const uploadedRef = await uploadImageFile(refFile);
          if (uploadedRef?.url) item.finalImagenesReferenciaUrls.push({ url: uploadedRef.url });
          else throw new Error(`Fallo al subir imagen de referencia: ${refName}`);
        } else {
          throw new Error(`Imagen de referencia no encontrada: ${refName}`);
        }
      }

    } catch (imgError) {
      console.error(`Error procesando imágenes (${item.codigoPropiedad}):`, imgError);
      results.push({
        success: false,
        itemCode: item.codigoPropiedad,
        error: `Error al procesar imágenes: ${imgError.message}`,
        details: []
      });
      item.valid = false; // evitar intento de alta en el paso 2
    }
  }

  Swal.update({
    title: 'Enviando inmuebles al servidor...',
    html: 'Creando los registros. Esto puede tomar unos momentos.'
  });

  // 2) Enviar inmuebles
  let uploadProcessedCount = 0;
  for (const item of validItems) {
    if (!item.valid) continue; // saltar si falló en imágenes

    uploadProcessedCount++;
    Swal.update({
      title: `Creando inmueble ${item.codigoPropiedad}... (${uploadProcessedCount} de ${validItems.length})`,
      html: `Enviando datos al backend...`
    });

    try {
      const datosEnviar = {
        codigoPropiedad: item.codigoPropiedad,
        titulo: item.titulo,
        precio: item.precio,
        habitaciones: item.habitaciones,
        banos: item.banos,
        parqueos: item.parqueos,
        metrosCuadrados: item.metrosCuadrados,
        contenido: item.contenido,
        tipos: item.tipos,
        operaciones: item.operaciones,
        ubicaciones: item.ubicaciones,
        luxury: item.luxury,
        video: item.video,
        amenidades: item.amenidades,
        imagenPrincipal: item.finalImagenPrincipalUrl,
        imagenesReferencia: item.finalImagenesReferenciaUrls,
        // === NUEVO: enviar slugInmueble ===
        slugInmueble: item.slugInmueble
      };

      const result = await inmuebleService.createInmueble(datosEnviar);
      results.push({ success: true, data: result, itemCode: item.codigoPropiedad });
    } catch (err) {
      console.error(`Error al crear inmueble ${item.codigoPropiedad}:`, err);
      results.push({
        success: false,
        itemCode: item.codigoPropiedad,
        error: err.response?.data?.message || err.message || 'Error desconocido del servidor',
        details: err.response?.data?.errors || []
      });
    }
  }

  Swal.close();
  uploadingItems.value = false;

  const successCount = results.filter(r => r.success).length;
  const errorCount = results.filter(r => !r.success).length;

  if (errorCount > 0) {
    let errorSummary = `Importación completada con errores:<br><br>Correctos: ${successCount}<br>Fallidos: ${errorCount}<br><br>Detalles:`;
    const errorDetailsList = results.filter(r => !r.success)
      .map(r => `<li><span class="font-bold">${r.itemCode || 'Inmueble desconocido'}</span>: ${r.error || 'Error desconocido'}${r.details && Object.keys(r.details).length > 0 ? `<br><span class="text-xs text-gray-600">${JSON.stringify(r.details, null, 2)}</span>` : ''}</li>`)
      .join('');
    error.value = errorSummary + `<ul class="list-disc pl-5 mt-2">${errorDetailsList}</ul>`;

    Swal.fire({
      title: '❌ Importación con Errores',
      html: error.value,
      icon: 'error',
      customClass: { container: 'swal2-container-custom' },
      didRender: () => {
        const swalContainer = document.querySelector('.swal2-container-custom');
        if (swalContainer) swalContainer.style.zIndex = '9999';
      }
    });

  } else {
    Swal.fire('✅ ¡Importación Exitosa!', `Se agregaron ${successCount} inmuebles correctamente.`, 'success');
    previewData.value = [];
    selectedImageFiles.value.clear();
    router.push({ name: 'inmuebles' });
  }
};
</script>

<style scoped>
.error-details {
  white-space: pre-wrap;
  font-family: monospace;
  max-height: 300px;
  overflow-y: auto;
  padding: 10px;
  background-color: #ffebeb;
  border-radius: 5px;
  word-break: break-all;
}

/* Asegurar SweetAlert2 por encima de todo */
.swal2-container {
  z-index: 9999 !important;
}
</style>
