<template>
  <div
    v-if="isOpen"
    class="modal-overlay fixed inset-0 bg-black bg-opacity-60 z-[1000] flex items-center justify-center p-4 backdrop-blur-sm"
    aria-labelledby="modal-title"
    role="dialog"
    aria-modal="true"
  >
    <div class="flex items-center justify-center w-full h-full">
      <!-- Fondo oscuro -->
      <div
        class="fixed inset-0 bg-gray-900/50 transition-opacity"
        aria-hidden="true"
        @click="closeModal"
      ></div>

      <!-- Contenido del modal -->
      <div
        class="relative bg-white rounded-2xl shadow-2xl transform transition-all overflow-hidden"
        style="
          width: 90%;
          max-width: 1400px;
          height: 90%;
          max-height: 850px;
          overflow-y: auto;
        "
      >
        <!-- Mostrar Skeleton mientras se cargan los datos -->
        <div v-if="isLoading" class="w-full px-4 pt-5 pb-4 sm:px-6 sm:pb-4 md:px-8">
          <div class="animate-pulse">
            <!-- Header Skeleton -->
            <div class="flex justify-between items-center mb-6">
              <div class="h-7 bg-gray-300 rounded w-56"></div>
              <div class="h-8 w-8 bg-gray-300 rounded"></div>
            </div>

            <!-- Información básica y Preferencias Skeleton -->
            <div class="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 mb-6">
              <!-- Información Personal Skeleton -->
              <div class="bg-gray-50 p-4 rounded-lg">
                <div class="h-6 bg-gray-400 rounded w-48 mb-4"></div>
                <div class="grid grid-cols-1 gap-3 md:grid-cols-2 lg:grid-cols-2">
                  <!-- Nombre Completo -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-28 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-32"></div>
                  </div>
                  <!-- Teléfono -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-20 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-28"></div>
                  </div>
                  <!-- Email -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-16 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-24"></div>
                  </div>
                  <!-- Fecha de Registro -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-32 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-24"></div>
                  </div>
                  <!-- Origen -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-16 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-20"></div>
                  </div>
                </div>
              </div>

              <!-- Preferencias Skeleton -->
              <div class="bg-gray-50 p-4 rounded-lg">
                <div class="h-6 bg-gray-400 rounded w-32 mb-4"></div>
                <div class="grid grid-cols-2 gap-3 md:grid-cols-2 lg:grid-cols-3">
                  <!-- Tipo Inmueble -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-24 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-28"></div>
                  </div>
                  <!-- Operación -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-20 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-16"></div>
                  </div>
                  <!-- Precio Mínimo -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-24 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-20"></div>
                  </div>
                  <!-- Precio Máximo -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-24 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-20"></div>
                  </div>
                  <!-- Habitaciones -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-24 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-8"></div>
                  </div>
                  <!-- Baños -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-16 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-8"></div>
                  </div>
                  <!-- M² Construcción -->
                  <div>
                    <div class="h-3 bg-gray-300 rounded w-32 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-12"></div>
                  </div>
                  <!-- Ubicación -->
                  <div class="col-span-2">
                    <div class="h-3 bg-gray-300 rounded w-20 mb-2"></div>
                    <div class="h-4 bg-gray-400 rounded w-32"></div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Amenidades Skeleton (opcional) -->
            <div class="bg-gray-50 p-4 rounded-lg mb-6">
              <div class="flex flex-wrap gap-2">
                <div v-for="i in 4" :key="`amenidad-${i}`" class="h-6 bg-gray-300 rounded w-20"></div>
              </div>
            </div>

            <!-- Match Pendiente Skeleton -->
            <div class="bg-gray-50 p-4 rounded-lg mb-6">
              <div class="h-6 bg-gray-400 rounded w-80 mb-4"></div>
              <div class="space-y-3">
                <div v-for="i in 3" :key="`match-${i}`" class="flex justify-between items-center p-3 bg-white rounded">
                  <div class="h-4 bg-gray-300 rounded flex-grow mr-4"></div>
                  <div class="flex space-x-2">
                    <div class="h-3 w-3 bg-gray-300 rounded"></div>
                    <div class="h-7 w-20 bg-green-300 rounded"></div>
                    <div class="h-7 w-24 bg-blue-300 rounded"></div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Requerimientos Skeleton -->
            <div class="bg-gray-50 p-4 rounded-lg mb-6">
              <div class="flex items-center justify-between mb-4">
                <div class="h-6 bg-gray-400 rounded w-40"></div>
              </div>
              <div class="h-4 bg-gray-300 rounded w-56"></div>
            </div>

            <!-- Historial Skeleton -->
            <div class="bg-gray-50 p-4 rounded-lg mb-6">
              <div class="flex items-center justify-between mb-4">
                <div class="h-6 bg-gray-400 rounded w-56"></div>
                <div class="flex gap-2">
                  <div class="h-7 w-24 bg-gray-300 rounded"></div>
                  <div class="h-7 w-20 bg-gray-300 rounded"></div>
                  <div class="h-7 w-24 bg-gray-300 rounded"></div>
                </div>
              </div>
              <div class="space-y-2">
                <div class="h-8 bg-gray-300 rounded"></div>
                <div v-for="i in 3" :key="`hist-${i}`" class="h-10 bg-gray-200 rounded"></div>
              </div>
            </div>

            <!-- Notas Skeleton -->
            <div class="bg-gray-50 p-4 rounded-lg">
              <div class="h-6 bg-gray-400 rounded w-48 mb-4"></div>
              <div class="space-y-2">
                <div class="h-3 bg-gray-300 rounded w-full"></div>
                <div class="h-3 bg-gray-300 rounded w-5/6"></div>
                <div class="h-3 bg-gray-300 rounded w-4/6"></div>
              </div>
            </div>
          </div>
        </div>

        <div v-else class="flex flex-col h-full">
          <!-- Header mejorado con gradiente -->
          <div class="relative bg-gradient-to-r from-blue-900 to-blue-700 text-white p-6 md:p-8 flex-shrink-0">
            <button
              @click="closeModal"
              type="button"
              class="absolute top-4 right-4 text-white hover:bg-white/20 rounded-full p-2 transition-all duration-200 z-10"
            >
              <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
            
            <div class="pr-12">
              <h3 class="text-2xl md:text-3xl font-bold mb-2">
                Contacto con {{ cliente.nombre }} {{ cliente.apellido }}
              </h3>
              <div class="flex flex-wrap items-center gap-4 text-sm text-blue-100">
                <span class="flex items-center gap-1">
                  <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M2.003 5.884L10 9.882l7.997-3.998A2 2 0 0016 4H4a2 2 0 00-1.997 1.884z"/>
                    <path d="M18 8.118l-8 4-8-4V14a2 2 0 002 2h12a2 2 0 002-2V8.118z"/>
                  </svg>
                  {{ cliente.email || 'Sin email' }}
                </span>
                <span class="flex items-center gap-1">
                  <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M2 3a1 1 0 011-1h2.153a1 1 0 01.986.836l.74 4.435a1 1 0 01-.54 1.06l-1.548.773a11.037 11.037 0 006.105 6.105l.774-1.548a1 1 0 011.059-.54l4.435.74a1 1 0 01.836.986V17a1 1 0 01-1 1h-2C7.82 18 2 12.18 2 5V3z"/>
                  </svg>
                  {{ cliente.telefono || 'Sin teléfono' }}
                </span>
                <span v-if="cliente.fechaRegistro" class="px-3 py-1 bg-white/20 rounded-full text-xs font-medium">
                  Cliente desde {{ formatDate(cliente.fechaRegistro) }}
                </span>
              </div>
            </div>
          </div>

          <!-- Contenido con scroll mejorado -->
          <div class="flex-1 overflow-y-auto p-6 md:p-8 bg-gray-50">
            <div class="space-y-6">
              <!-- Información básica y Preferencias -->
              <div class="grid grid-cols-1 gap-6 lg:grid-cols-2">
                <!-- Información Personal -->
                <div class="bg-white rounded-xl shadow-md p-6">
                  <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                    <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clip-rule="evenodd"/>
                    </svg>
                    Información Personal
                  </h4>

                  <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Nombre Completo</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.nombre }} {{ cliente.apellido }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Teléfono</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.telefono || "N/A" }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Email</span
                        >
                        <p class="text-gray-900 font-semibold text-sm break-all">
                          {{ cliente.email || "N/A" }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Fecha de Registro</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{
                            cliente.fechaRegistro
                              ? formatDate(cliente.fechaRegistro)
                              : "N/A"
                          }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Origen</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.origen || "N/A" }}
                        </p>
                      </div>
                    </div>
                </div>

                <!-- Preferencias -->
                <div class="bg-white rounded-xl shadow-md p-6">
                  <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                    <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9z"/>
                      <path fill-rule="evenodd" d="M4 5a2 2 0 012-2 3 3 0 003 3h2a3 3 0 003-3 2 2 0 012 2v11a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm9.707 5.707a1 1 0 00-1.414-1.414L9 12.586l-1.293-1.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                    </svg>
                    Preferencias
                  </h4>

                  <div class="grid grid-cols-2 gap-4 sm:grid-cols-3">
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Tipo Inmueble</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.preferenciasSummary?.tipo || "N/A" }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Operación</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.preferenciasSummary?.operacion || "N/A" }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Precio Mínimo</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{
                            cliente.preferenciasSummary?.precioMin != null
                              ? `$${Number(
                                  cliente.preferenciasSummary.precioMin
                                ).toLocaleString()}`
                              : "N/A"
                          }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Precio Máximo</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{
                            cliente.preferenciasSummary?.precioMax != null
                              ? `$${Number(
                                  cliente.preferenciasSummary.precioMax
                                ).toLocaleString()}`
                              : "N/A"
                          }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Habitaciones</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{
                            cliente.preferenciasSummary?.habitaciones ?? "N/A"
                          }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Baños</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.preferenciasSummary?.banos ?? "N/A" }}
                        </p>
                      </div>
                      <div class="pb-3 border-b">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >M² Construcción</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{
                            cliente.preferenciasSummary?.metrosCuadrados ??
                            "N/A"
                          }}
                        </p>
                      </div>
                      <div class="col-span-2 pb-3 border-b sm:col-span-3">
                        <span class="text-xs font-medium text-gray-500 uppercase tracking-wide block mb-1"
                          >Ubicación</span
                        >
                        <p class="text-gray-900 font-semibold">
                          {{ cliente.preferenciasSummary?.ubicacion || "N/A" }}
                        </p>
                      </div>
                    </div>
                  </div>
                </div>

              <!-- Amenidades -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-5 flex items-center gap-2">
                  <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9z"/>
                    <path fill-rule="evenodd" d="M4 5a2 2 0 012-2 3 3 0 003 3h2a3 3 0 003-3 2 2 0 012 2v11a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm9.707 5.707a1 1 0 00-1.414-1.414L9 12.586l-1.293-1.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd"/>
                  </svg>
                  Amenidades Preferidas
                </h4>

                <div
                  v-if="
                    cliente?.preferenciasSummary?.amenidades &&
                    cliente.preferenciasSummary.amenidades.length > 0
                  "
                  class="flex flex-wrap gap-2"
                >
                  <span
                    v-for="(nombre, index) in cliente.preferenciasSummary
                      .amenidades"
                    :key="index"
                    class="inline-flex items-center px-3 py-1.5 bg-blue-50 text-blue-700 rounded-lg text-sm font-medium hover:bg-blue-100 transition-colors"
                  >
                    <svg class="w-4 h-4 mr-1.5" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"/>
                    </svg>
                    {{ nombre }}
                  </span>
                </div>

                <p v-else class="text-gray-400 text-sm">No hay amenidades preferidas</p>
              </div>

              <MatchPendiente 
                :clienteId="clienteId" 
                :clientDetails="cliente" 
                @open-inmueble="handleOpenInmueble"
              />

              <!-- Lista de requerimientos -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <div class="flex items-center justify-between mb-5">
                  <h4 class="text-xl font-bold text-gray-900 flex items-center gap-2">
                    <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9z"/>
                      <path fill-rule="evenodd" d="M4 5a2 2 0 012-2 3 3 0 003 3h2a3 3 0 003-3 2 2 0 012 2v11a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm3 4a1 1 0 000 2h6a1 1 0 100-2H7zm0 4a1 1 0 000 2h6a1 1 0 100-2H7z" clip-rule="evenodd"/>
                    </svg>
                    Requerimientos
                  </h4>
                </div>
                <div
                  v-if="preferenciasList.length <= 1"
                  class="text-sm text-gray-400 text-center py-8"
                >
                  No hay requerimientos registrados
                </div>

                  <div
                    v-else
                    class="relative overflow-x-auto shadow-sm rounded"
                  >
                    <table
                      class="w-full text-sm text-left rtl:text-right text-gray-500"
                    >
                      <thead
                        class="text-xs text-gray-700 uppercase bg-gray-100"
                      >
                        <tr>
                          <th class="px-4 py-2">Tipo</th>
                          <th class="px-4 py-2">Operación</th>
                          <th class="px-4 py-2">Ubicación</th>
                          <th class="px-4 py-2">Precio Min</th>
                          <th class="px-4 py-2">Precio Max</th>
                          <th class="px-3 py-2">Hab.</th>
                          <th class="px-3 py-2">Baños</th>
                          <th class="px-4 py-2">M²</th>
                          <th class="px-4 py-2">Amenidades</th>
                          <th class="px-4 py-2">Accion</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr
                          v-for="(pref, index) in preferenciasList.slice(1)"
                          :key="pref.id"
                          class="odd:bg-white odd: even:bg-gray-50 even: border-b border-gray-200"
                        >
                          <td class="px-4 py-3">{{ pref.tipo || "-" }}</td>
                          <td class="px-4 py-3">{{ pref.operacion || "-" }}</td>
                          <td class="px-4 py-3">{{ pref.ubicacion || "-" }}</td>
                          <td class="px-4 py-3">
                            {{
                              pref.precioMin != null
                                ? `$${Number(pref.precioMin).toLocaleString()}`
                                : "-"
                            }}
                          </td>
                          <td class="px-4 py-3">
                            {{
                              pref.precioMax != null
                                ? `$${Number(pref.precioMax).toLocaleString()}`
                                : "-"
                            }}
                          </td>
                          <td class="px-3 py-3 text-center">
                            {{ pref.habitaciones ?? "-" }}
                          </td>
                          <td class="px-3 py-3 text-center">
                            {{ pref.banos ?? "-" }}
                          </td>
                          <td class="px-4 py-3 text-center">
                            {{ pref.metrosCuadrados ?? "-" }}
                          </td>
                          <td class="px-4 py-3">
                            <div class="flex flex-wrap gap-2">
                              <span
                                v-for="(a, idx) in pref.amenidades"
                                :key="idx"
                                class="text-xs bg-gray-100 px-2 py-1 rounded"
                              >
                                {{ a }}
                              </span>
                              <span
                                v-if="
                                  !pref.amenidades ||
                                  pref.amenidades.length === 0
                                "
                                class="text-xs text-gray-500"
                                >—</span
                              >
                            </div>
                          </td>
                          <td
                            class="px-6 py-4 space-x-2 flex content-center justify-center"
                          >
                            <button
                              @click="detallePreferencia(pref.id)"
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
                              @click="editarPreferencia(pref.id)"
                              class="flex font-medium text-gray-600 hover:underline"
                              title="Editar requerimiento"
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
                              @click="eliminarPreferencia(pref.id)"
                              class="flex font-medium text-gray-600 hover:underline"
                              title="Eliminar requerimiento"
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
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
              </div>

              <!--Historial de seguimiento-->
              <div class="bg-white rounded-xl shadow-md p-6">
                <div class="flex items-center justify-between mb-5">
                  <h4 class="text-xl font-bold text-gray-900 flex items-center gap-2">
                    <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M6 2a1 1 0 00-1 1v1H4a2 2 0 00-2 2v10a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2h-1V3a1 1 0 10-2 0v1H7V3a1 1 0 00-1-1zm0 5a1 1 0 000 2h8a1 1 0 100-2H6z" clip-rule="evenodd"/>
                    </svg>
                    Historial de Seguimiento
                  </h4>

                  <div class="flex items-center gap-3">
                    <input
                      type="text"
                      id="filtrarInteracciones"
                      v-model="filtroInteracciones"
                      class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block px-3 py-2"
                      placeholder="Filtrar interacciones..."
                    />

                    <button
                      type="button"
                      @click="openUpdateModal()"
                      class="text-sm px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 transition-colors font-medium"
                    >
                      Agregar
                      </button>

                      <button
                        @click="loadInteracciones"
                        type="button"
                        class="text-xs px-2 py-1 rounded border border-gray-300 bg-white hover:bg-gray-100 disabled:opacity-60"
                        :disabled="isLoadingInteracciones"
                      >
                        {{ isLoadingInteracciones ? "..." : "Refrescar" }}
                      </button>
                    </div>
                  </div>

                  <div
                    v-if="isLoadingInteracciones"
                    class="text-sm text-gray-500"
                  >
                    Cargando...
                  </div>
                  <div
                    v-else-if="!interacciones.length"
                    class="text-sm text-gray-500"
                  >
                    Sin interacciones.
                  </div>
                  <div v-else class="relative overflow-x-auto">
                    <table class="w-full text-sm text-left text-gray-500">
                      <thead
                        class="text-xs text-gray-700 uppercase bg-gray-100"
                      >
                        <tr>
                          <th class="px-4 py-2">Fecha</th>
                          <th class="px-4 py-2">Tipo</th>
                          <th class="px-4 py-2">Status</th>
                          <th class="px-4 py-2">Nota</th>
                          <th class="px-4 py-2">Agente</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr
                          v-for="it in interacciones"
                          :key="it.id"
                          class="odd:bg-white even:bg-gray-50 border-b border-gray-200"
                        >
                          <td class="px-4 py-2 whitespace-nowrap">
                            {{
                              formatDateTime(
                                it.fecha || it.fechaInteraccion || it.createdAt
                              )
                            }}
                          </td>
                          <td class="px-4 py-2">
                            {{ it.tipo || it.tipoInteraccion || "-" }}
                          </td>
                          <td class="px-4 py-2">
                            {{
                              formatStatusForDisplay(
                                it.canal ||
                                  it.statusInteraccion ||
                                  it.status ||
                                  "-"
                              )
                            }}
                          </td>
                          <td
                            class="px-4 py-2 max-w-xs truncate"
                            :title="it.nota || it.comentario || it.descripcion"
                          >
                            {{
                              it.nota || it.comentario || it.descripcion || "-"
                            }}
                          </td>
                          <td class="px-4 py-2">
                            {{
                              it.agente?.nombre
                                ? it.agente.nombre +
                                  " " +
                                  (it.agente.apellido || "")
                                : it.agenteNombre ||
                                  (loggedAgente?.nombre
                                    ? loggedAgente.nombre +
                                      " " +
                                      (loggedAgente.apellido || "")
                                    : "-")
                            }}
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>

              <!-- Notas adicionales -->
              <div class="bg-white rounded-xl shadow-md p-6">
                <h4 class="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                  <svg class="w-6 h-6 text-blue-600" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"/>
                  </svg>
                  Notas Adicionales
                </h4>
                <p class="text-gray-700 leading-relaxed">
                  {{ cliente.notas || "Sin notas adicionales" }}
                </p>
              </div>
            </div>
          </div>

          <!-- Footer mejorado -->
          <div class="bg-white border-t p-6 flex justify-end gap-3 flex-shrink-0">
            <button
              type="button"
              @click="closeModal"
              class="px-6 py-3 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 rounded-xl text-sm font-medium transition-all"
            >
              Cerrar
            </button>
          </div>
        </div>

        <!-- Añadido: modal de detalle de preferencia como fallback local -->
        <ModalDetalleRequerimiento
          :isOpen="showPreferenciaModal"
          :clienteId="preferenciaToShow.clienteId"
          :preferenciaId="preferenciaToShow.preferenciaId"
          @close="handleClosePreferenciaModal"
        />
      </div>
    </div>
  </div>

  <div
    v-if="showUpdateModal"
    id="updateRecordatorioModal"
    tabindex="-1"
    aria-hidden="true"
    role="dialog"
    aria-modal="true"
    class="fixed inset-0 z-[9999]"
    style="z-index: 9999"
  >
    <!-- Overlay -->
    <div
      class="absolute inset-0 bg-gray-500/50 z-0"
      @click="closeUpdateModal"
    ></div>

    <!-- Wrapper centrado -->
    <div class="relative z-10 flex min-h-full items-center justify-center p-4">
      <div class="w-full max-w-2xl">
        <div class="bg-white rounded-lg shadow max-h-[90vh] overflow-y-auto">
          <!-- header -->
          <div
            class="flex items-center justify-between p-4 md:p-5 border-b rounded-t"
          >
            <h3 class="text-xl font-semibold text-gray-900">
              Actualizar Recordatorio
            </h3>
            <button
              type="button"
              @click="closeUpdateModal"
              class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center"
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
              <span class="sr-only">Cerrar modal</span>
            </button>
          </div>
          <!-- body -->
          <div class="p-4 md:p-5 space-y-4">
            <div class="grid grid-cols-1 gap-4 mb-4">
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Cliente</label
                >
                <input
                  v-model="currentRecordatorio.clienteNombre"
                  type="text"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                  disabled
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Tipo de Contacto</label
                >
                <select
                  v-model="currentRecordatorio.tipoInteraccion"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                >
                  <option value="" disabled>Seleccione un tipo</option>
                  <option value="Llamada">Llamada</option>
                  <option value="Mensaje">Mensaje</option>
                  <option value="Email">Email</option>
                  <option value="Visita">Visita</option>
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
                    v-for="(value, key) in statusMap"
                    :key="key"
                    :value="parseInt(key)"
                  >
                    {{ formatStatusForDisplay(value) }}
                  </option>
                </select>
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Fecha Próximo Contacto</label
                >
                <input
                  v-model="currentRecordatorio.fechaProximoContactoRaw"
                  type="date"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-gray-500 focus:border-gray-500 block w-full p-2.5"
                />
              </div>
              <div>
                <label class="block mb-2 text-sm font-medium text-gray-900"
                  >Notas</label
                >
                <textarea
                  v-model="currentRecordatorio.notasInteraccion"
                  rows="4"
                  class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-gray-500 focus:border-gray-500"
                  placeholder="Agregue notas adicionales..."
                ></textarea>
              </div>
            </div>
          </div>
          <!-- footer -->
          <div
            class="flex items-center p-4 md:p-5 border-t border-gray-200 rounded-b"
          >
            <!-- <button
              @click="updateRecordatorio"
              type="button"
              class="text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center    "
            >
              Guardar cambios
            </button> -->
            <button
              @click="addInteraccion"
              type="button"
              class="ms-3 text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
            >
              Añadir Interacción
            </button>
            <button
              @click="closeUpdateModal"
              type="button"
              class="ms-3 text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-gray-300 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10"
            >
              Cancelar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import {
  ref,
  defineProps,
  defineEmits,
  watch,
  onMounted,
  onUnmounted,
  computed,
  reactive,
  nextTick,
} from "vue";
import requerimientoService from "@/services/requerimientoService";
import Swal from "sweetalert2";
import { ref as vueRef } from "vue";
import clienteService from "@/services/clienteService";
import ModalDetalleRequerimiento from "./modalDetalleRequerimiento.vue";
import agenteService from "@/services/agenteService";
import interaccionService from "@/services/interaccionService";
import MatchPendiente from "./matchPendiente.vue";

// Import or define statusMap
import { statusMap } from "@/api/constants"; // Adjust the path if necessary

// If statusMap is not imported, define it locally as a fallback
// const statusMap = {
//   0: "Nuevo",
//   1: "Contactado",
//   2: "En seguimiento",
//   3: "Cerrado",
// };

// === Props ===
const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true,
    default: false,
  },
  isAdmin: {
    type: Boolean,
    default: false,
  },
  cliente: {
    type: Object,
    default: () => ({
      telefono: "",
      email: "",
      dni: "",
      direccion: "",
      fechaNacimiento: null,
      fechaRegistro: null,
      notas: "",
      origen: "",
      status: 0,
      preferencias: {
        tipo: "",
        operacion: "",
        ubicacion: "",
        precioMin: null,
        precioMax: null,
        habitaciones: null,
        banos: null,
        metrosCuadrados: null,
        amenidades: [],
      },
    }),
  },
});

const clienteId = computed(() => cliente.value?.id || props.cliente?.id || null); 

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
  { id: 17, nombre: "Amueblemado" },
  { id: 18, nombre: "No mascotas" },
  { id: 19, nombre: "Sala familiar" },
  { id: 20, nombre: "Canchas deportivas" },
]);

// === Emits ===
// añadimos 'open-preferencia-detail' para mostrar detalle de una preferencia en modalDetalleRequerimiento
const emit = defineEmits([
  "close",
  "pref-deleted",
  "edit-preferencia",
  "open-preferencia-detail",
  "open-inmueble",
]);

// --- helpers ---
function normalizeText(s) {
  if (!s && s !== 0) return "";
  return String(s)
    .normalize("NFKD")
    .replace(/[\u0300-\u036F]/g, "")
    .toLowerCase()
    .trim();
}

// === Datos reactivos ===
const cliente = ref(null);
const isLoading = ref(false);

// Añadidos: estado local para mostrar modal de preferencia (fallback si el padre no lo abre)
const showPreferenciaModal = ref(false);
const preferenciaToShow = ref({ clienteId: null, preferenciaId: null });

function handleClosePreferenciaModal() {
  showPreferenciaModal.value = false;
  preferenciaToShow.value = { clienteId: null, preferenciaId: null };
}

// === Computed: preferenciasList ===
// Mover la definición de preferenciasList al inicio, antes de cualquier watcher o lógica que lo use.
const preferenciasList = computed(() => {
  // permitimos actualizar localmente (cliente.value) y si no existe usamos props.cliente
  const raw =
    cliente.value?.preferencias ?? props.cliente?.preferencias ?? null;
  if (!raw) return [];

  // Obtener array de preferencias (compat con $values)
  let prefsArr = [];
  if (Array.isArray(raw.$values)) prefsArr = raw.$values;
  else if (Array.isArray(raw)) prefsArr = raw;
  else if (
    raw &&
    typeof raw === "object" &&
    (raw.id || raw.tipo || raw.operacion)
  )
    prefsArr = [raw];
  else prefsArr = [];

  return prefsArr.map((p) => {
    // extraer amenidades robustamente
    const rawAms =
      p?.preferenciaAmenidades?.$values ??
      (Array.isArray(p?.preferenciaAmenidades) ? p.preferenciaAmenidades : []);
    const seen = new Set();
    const amNames = (rawAms || [])
      .map((a) => {
        const id = a?.amenidadId ?? a?.AmenidadId ?? a?.id ?? null;
        const nombre = a?.nombre ?? a?.Nombre ?? null;
        return {
          id: id != null ? Number(id) : null,
          nombre: nombre ? String(nombre) : null,
        };
      })
      .filter(Boolean)
      .filter((it) => it.id || it.nombre)
      .filter((it) => {
        const key = it.id
          ? `id:${it.id}`
          : `name:${(it.nombre || "").toLowerCase()}`;
        if (seen.has(key)) return false;
        seen.add(key);
        return true;
      })
      .map((it) => it.nombre || `Amenidad ${it.id}`);

    return {
      id: p.id,
      tipo: p.tipo,
      operacion: p.operacion,
      ubicacion: p.ubicacion,
      precioMin: p.precioMin,
      precioMax: p.precioMax,
      habitaciones: p.habitaciones,
      banos: p.banos,
      metrosCuadrados: p.metrosCuadrados,
      amenidades: amNames,
    };
  });
});

// Computed para calcular dinámicamente preferenciasSummary basado en las preferencias del cliente
const preferenciasSummary = computed(() => {
  const rawPrefs =
    cliente.value?.preferencias?.$values || cliente.value?.preferencias || [];
  const firstPref =
    Array.isArray(rawPrefs) && rawPrefs.length > 0 ? rawPrefs[0] : null;

  if (!firstPref) {
    return {
      tipo: "N/A",
      operacion: "N/A",
      ubicacion: "N/A",
      precioMin: null,
      precioMax: null,
      habitaciones: null,
      banos: null,
      metrosCuadrados: null,
      amenidades: [],
    };
  }

  const summaryAmenidades = Array.isArray(
    firstPref.preferenciaAmenidades?.$values
  )
    ? firstPref.preferenciaAmenidades.$values.map((a) => a.nombre || a.Nombre)
    : Array.isArray(firstPref.amenidades)
    ? firstPref.amenidades
    : [];

  return {
    tipo: firstPref.tipo || "N/A",
    operacion: firstPref.operacion || "N/A",
    ubicacion: firstPref.ubicacion || "N/A",
    precioMin: firstPref.precioMin ?? null,
    precioMax: firstPref.precioMax ?? null,
    habitaciones: firstPref.habitaciones ?? null,
    banos: firstPref.banos ?? null,
    metrosCuadrados: firstPref.metrosCuadrados ?? null,
    amenidades: summaryAmenidades.length ? summaryAmenidades : [],
  };
});

// === Watch - Cargar cliente cuando cambia props.cliente ===
// Añadir una referencia para controlar abortos de solicitudes
const abortController = ref(null);

// Función para cargar el cliente completo con efecto de loading
async function loadClienteCompleto() {
  if (!props.cliente?.id) {
    // Si no hay ID, usar datos de props directamente sin loading
    if (props.cliente) {
      cliente.value = {
        ...props.cliente,
        preferencias: {
          ...props.cliente.preferencias,
          amenidades: props.cliente.preferencias?.amenidades || [],
        },
      };
    }
    isLoading.value = false;
    return;
  }

  try {
    // Establecer loading ANTES de cualquier operación
    isLoading.value = true;
    
    // Limpiar cliente para forzar el skeleton
    cliente.value = null;
    
    // Delay mínimo para que se vea el skeleton
    await new Promise(resolve => setTimeout(resolve, 500));
    
    // Cargar datos completos del cliente
    const clienteData = await clienteService.getClienteById(props.cliente.id);
    const clienteCompleto = clienteData?.$values ? clienteData.$values[0] : clienteData;
    
    if (clienteCompleto) {
      cliente.value = {
        ...clienteCompleto,
        preferencias: {
          ...clienteCompleto.preferencias,
          amenidades: clienteCompleto.preferencias?.amenidades || [],
        },
      };
    }
  } catch (error) {
    console.error("Error al cargar cliente:", error);
    // En caso de error, usar datos de props
    if (props.cliente) {
      cliente.value = {
        ...props.cliente,
        preferencias: {
          ...props.cliente.preferencias,
          amenidades: props.cliente.preferencias?.amenidades || [],
        },
      };
    }
  } finally {
    isLoading.value = false;
  }
}

// Watch de cliente solo para actualizar cuando NO está en modo loading
watch(
  () => props.cliente,
  (newVal, oldVal) => {
    // Si cambió el ID del cliente mientras el modal está abierto
    if (props.isOpen && newVal?.id !== oldVal?.id) {
      // Limpiar cliente anterior y recargar
      cliente.value = null;
      isLoading.value = true;
      loadClienteCompleto();
    } else if (!isLoading.value && newVal) {
      // Solo actualizar si no estamos en estado de carga
      cliente.value = {
        ...newVal,
        preferencias: {
          ...newVal.preferencias,
          amenidades: newVal.preferencias?.amenidades || [],
        },
      };
    }
  },
  { deep: true }
);

// Reemplazo de watcher anterior de isOpen para secuenciar
watch(
  () => props.isOpen,
  async (v, oldV) => {
    if (v && !oldV) {
      // Al abrir el modal, primero limpiar el cliente anterior
      cliente.value = null;
      isLoading.value = true;
      
      // Luego cargar los datos del nuevo cliente
      await loadClienteCompleto();
    } else if (!v && oldV) {
      // Al cerrar, limpiar cualquier operación pendiente
      // Cancelar solicitudes pendientes
      if (abortController.value) {
        abortController.value.abort();
        abortController.value = null;
      }
      // Resetear estado de carga
      isLoading.value = false;
      // Limpiar cliente para que no se vea al reabrir
      cliente.value = null;
    }
  }
);

// === Cerrar modal ===
function closeModal() {
  // Cancelar solicitudes pendientes
  if (abortController.value) {
    abortController.value.abort();
    abortController.value = null;
  }

  // Limpiar temporizadores

  // Usar nextTick para asegurar que la emisión ocurra después de limpiar el estado
  nextTick(() => {
    emit("close");
  });
}

const formatDate = (dateString) => {
  if (!dateString) return "";

  const date = new Date(dateString);
  if (isNaN(date.getTime())) return "Fecha inválida";

  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();

  return `${day}/${month}/${year}`;
};

// --- Status formatting helper (added) ---
function formatStatusForDisplay(raw) {
  if (raw === null || raw === undefined) return "-";
  const v = String(raw).trim();
  if (!v) return "-";
  const lower = v.toLowerCase();

  const map = {
    0: "Nuevo",
    1: "Contactado",
    2: "En seguimiento",
    3: "Cerrado",
    nuevo: "Nuevo",
    contactado: "Contactado",
    seguimiento: "En seguimiento",
    "en seguimiento": "En seguimiento",
    cerrado: "Cerrado",
    pendiente: "Pendiente",
    "pendiente contacto": "Pendiente de contacto",
    "pendiente de contacto": "Pendiente de contacto",
  };

  if (map[lower]) return map[lower];
  return v.charAt(0).toUpperCase() + v.slice(1);
}

// --- Eliminar preferencia ---
async function eliminarPreferencia(prefId) {
  if (!prefId) {
    Swal.fire("Error", "ID de preferencia inválido.", "error");
    return;
  }

  const result = await Swal.fire({
    title: "¿Eliminar preferencia?",
    text: "Esta acción eliminará el requrimiento seleccionado.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d",
    confirmButtonText: "Sí, eliminar",
    cancelButtonText: "Cancelar",
  });

  if (!result.isConfirmed) return;

  try {
    await requerimientoService.deletePreferencia(prefId);
    Swal.fire("Eliminada", "El requerimiento fue eliminado.", "success");

    // Actualizar copia local (cliente.value) para reflejar el cambio inmediatamente
    try {
      if (cliente.value?.preferencias) {
        const p = cliente.value.preferencias;
        if (Array.isArray(p.$values)) {
          cliente.value.preferencias = {
            ...p,
            $values: p.$values.filter((x) => x.id !== prefId),
          };
        } else if (Array.isArray(p)) {
          cliente.value.preferencias = p.filter((x) => x.id !== prefId);
        }
      }
    } catch (e) {
      // console.warn("No se pudo actualizar cliente.value localmente:", e);
    }

    // Emitir para que el padre (si desea) vuelva a recargar datos
    emit("pref-deleted", {
      clienteId: cliente.value?.id ?? props.cliente?.id,
      preferenciaId: prefId,
    });
  } catch (err) {
    // console.error("Error al eliminar preferencia:", err);
    Swal.fire(
      "Error",
      err.message || "No se pudo eliminar el requerimiento.",
      "error"
    );
  }
}

// --- Editar preferencia: emitir evento para que el padre abra modalRequerimiento en modo edición ---
function editarPreferencia(prefId) {
  if (!prefId) {
    console.warn("editarPreferencia llamado sin prefId");
    return;
  }

  const clienteId = cliente.value?.id ?? props.cliente?.id ?? null;

  if (!clienteId) {
    console.error("Cliente ID no definido al intentar editar la preferencia.");
    Swal.fire("Error", "No se pudo editar la preferencia: Cliente ID no definido.", "error");
    return;
  }

  emit("edit-preferencia", {
    preferenciaId: prefId,
    clienteId: clienteId,
    source: "detalleCliente",
  });
}

// Abrir detalle de preferencia (vista, no edición) -> emite y además abre modal localmente
function detallePreferencia(prefId) {
  if (!prefId) return;
  try {
    const clienteId = cliente.value?.id ?? props.cliente?.id ?? null;
    // console.debug("[detalleCliente] emitir open-preferencia-detail ->", { preferenciaId: prefId, clienteId });

    // Emitir al padre (si el padre quiere manejarlo)
    emit("open-preferencia-detail", {
      preferenciaId: prefId,
      clienteId,
      source: "detalleCliente",
    });

    // Fallback local: abrir modalDetalleRequerimiento dentro de este componente
    preferenciaToShow.value = { clienteId, preferenciaId: prefId };
    showPreferenciaModal.value = true;
  } catch (e) {
    // console.warn("detallePreferencia emit failed", e);
  }
}

// timestamp para evitar reprocesar el mismo cambio
const lastProcessedTimestamp = vueRef(0);

// Registrar listener global que actualiza la copia local de preferencias
async function handlePreferenciaChanged(evtOrDetail) {
  try {
    const d =
      evtOrDetail?.detail ??
      (evtOrDetail && evtOrDetail.preferencia ? evtOrDetail : null);
    if (!d || !d.preferencia) return;
    const targetClienteId = d.clienteId ?? null;
    const ts = Number(d.timestamp ?? 0);
    if (ts && lastProcessedTimestamp.value >= ts) return;
    const myClienteId = cliente.value?.id ?? props.cliente?.id ?? null;
    if (!myClienteId || Number(myClienteId) !== Number(targetClienteId)) return;
    if (ts) lastProcessedTimestamp.value = ts;

    await refreshClientePreferencias();

    try {
      const stored = sessionStorage.getItem("preferencia:changed:last");
      if (stored) {
        const parsed = JSON.parse(stored);
        if (Number(parsed?.timestamp ?? 0) === ts) {
          sessionStorage.removeItem("preferencia:changed:last");
        }
      }
    } catch (_) {}
  } catch (_) {
    // console.error("Error en handlePreferenciaChanged:", _);
  }
}

// NUEVO: refetch solo de preferencias/cliente
const isRefreshingPrefs = ref(false);
async function refreshClientePreferencias() {
  const id = cliente.value?.id ?? props.cliente?.id;
  if (!id || isRefreshingPrefs.value) return;
  isRefreshingPrefs.value = true;
  try {
    const resp = await clienteService.getClienteById(id);
    const clienteData = resp?.$values ? resp.$values[0] : resp;
    if (clienteData) {
      cliente.value = {
        ...clienteData,
        preferencias: {
          ...clienteData.preferencias,
          amenidades: clienteData.preferencias?.amenidades || [],
        },
      };
    }
  } catch (e) {
    // console.warn("Fallo al refrescar cliente:", e);
  } finally {
    isRefreshingPrefs.value = false;
  }
}

// NUEVO: watcher para refrescar al reabrir el modal
watch(
  () => props.isOpen,
  (v) => {
    if (v) refreshClientePreferencias();
  }
);

onMounted(() => {
  window.addEventListener("preferencia:changed", handlePreferenciaChanged);
  window.addEventListener("preferencia:updated", handlePreferenciaChanged);
  window.addEventListener("preferencia:saved", handlePreferenciaChanged);

  // Al montar, comprobar si hay un cambio pendiente en sessionStorage (por si el modal despachó antes de que este componente se montara)
  try {
    const raw = sessionStorage.getItem("preferencia:changed:last");
    if (raw) {
      const parsed = JSON.parse(raw);
      const targetClienteId = parsed?.clienteId ?? null;
      const ts = Number(parsed?.timestamp ?? 0);
      const myClienteId = cliente.value?.id ?? props.cliente?.id ?? null;
      if (
        parsed &&
        parsed.preferencia &&
        myClienteId &&
        Number(myClienteId) === Number(targetClienteId) &&
        ts &&
        ts > lastProcessedTimestamp.value
      ) {
        // aplicar de forma sincrónica
        handlePreferenciaChanged(parsed);
      }
    }
  } catch (e) {
    // console.debug("No se pudo leer sessionStorage preferencia:changed:last", e);
  }
});

onUnmounted(() => {
  window.removeEventListener("preferencia:changed", handlePreferenciaChanged);
  window.removeEventListener("preferencia:updated", handlePreferenciaChanged);
  window.removeEventListener("preferencia:saved", handlePreferenciaChanged);
});

const interacciones = ref([]);
const isLoadingInteracciones = ref(false);
const filtroInteracciones = ref('');

function parseDate(d) {
  if (!d) return null;
  const dt = new Date(d);
  return isNaN(dt.getTime()) ? null : dt;
}

async function loadInteracciones() {
  if (!props.cliente?.id) return;
  isLoadingInteracciones.value = true;
  try {
    // Get interactions from client data instead of separate API call
    const clienteData = await clienteService.getClienteById(props.cliente.id);
    const cliente = clienteData?.$values ? clienteData.$values[0] : clienteData;

    // Extract interactions from client data
    let interaccionesData = null;
    if (cliente?.interacciones) {
      interaccionesData = cliente.interacciones;
    } else if (cliente?.historialInteracciones) {
      interaccionesData = cliente.historialInteracciones;
    } else if (cliente?.seguimientos) {
      interaccionesData = cliente.seguimientos;
    }

    let arr = [];
    if (interaccionesData?.$values) {
      arr = interaccionesData.$values;
    } else if (Array.isArray(interaccionesData)) {
      arr = interaccionesData;
    }

    interacciones.value = arr
      .slice()
      .sort(
        (a, b) =>
          (parseDate(b.fecha || b.fechaInteraccion || b.createdAt)?.getTime() ||
            0) -
          (parseDate(a.fecha || a.fechaInteraccion || a.createdAt)?.getTime() ||
            0)
      );
  } catch (e) {
    // console.warn("Error loading interactions from client data:", e);
    interacciones.value = [];
  } finally {
    isLoadingInteracciones.value = false;
  }
}

function formatDateTime(val) {
  const dt = parseDate(val);
  if (!dt) return "-";
  return (
    dt.toLocaleDateString() +
    " " +
    dt.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })
  );
}

const loggedAgente = ref(null); // nuevo

async function loadLoggedAgente() {
  try {
    loggedAgente.value = (await agenteService.getLoggedUser)
      ? await agenteService.getLoggedUser()
      : await agenteService.getCurrentUser();
  } catch (_) {
    loggedAgente.value = null;
  }
}

// Cargar al abrir modal o cambiar de cliente
watch(
  () => props.isOpen,
  (v) => {
    if (v) {
      loadInteracciones();
      loadLoggedAgente();
    }
  }
);
watch(
  () => props.cliente?.id,
  (id) => {
    if (props.isOpen && id) loadInteracciones();
  }
);

// === FUNCIONES DE INTERACCIÓN CON EL USUARIO ===

// Función para manejar el evento open-inmueble del componente MatchPendiente
function handleOpenInmueble(eventData) {
  console.log("🏠 detalleCliente recibió evento open-inmueble:", eventData);
  // Reemitir el evento hacia arriba para que el componente padre (que maneja modalDetalleInmueble) lo capture
  emit("open-inmueble", eventData);
}

// === SECCIÓN DE INTERACCIONES ===

// NUEVO: Modal para actualizar estado
const showUpdateModal = ref(false);
const currentRecordatorio = ref({
  estadoInteraccionRaw: null,
  notasInteraccion: "",
});

const openUpdateModal = (rec = null) => {
  // Construir nombre del cliente de forma segura
  const nombre = (cliente.value?.nombre || props.cliente?.nombre || "")
    .toString()
    .trim();
  const apellido = (cliente.value?.apellido || props.cliente?.apellido || "")
    .toString()
    .trim();
  const clienteNombre =
    [nombre, apellido].filter(Boolean).join(" ") || "Cliente desconocido";

  // Determinar estado actual del cliente (numérico)
  const estadoActual =
    (typeof cliente.value?.status === "number"
      ? cliente.value?.status
      : null) ??
    (typeof props.cliente?.status === "number" ? props.cliente?.status : 0);

  // Aceptar ambos esquemas: {interaccionId} o {id}
  const interId = rec?.interaccionId ?? rec?.id ?? null;

  if (!interId) {
    // Crear nuevo estado (sin ID de interacción)
    currentRecordatorio.value = {
      interaccionId: null,
      clienteNombre,
      tipoInteraccion: "",
      estadoInteraccionRaw: estadoActual || 0,
      fechaProximoContactoRaw: "",
      notasInteraccion: "",
    };
  } else {
    // Usar datos del recordatorio existente con tolerancia de claves
    currentRecordatorio.value = {
      interaccionId: interId,
      clienteNombre: rec?.clienteNombre || clienteNombre,
      tipoInteraccion: rec?.tipoInteraccion || rec?.tipo || "",
      estadoInteraccionRaw:
        (typeof rec?.estadoInteraccionRaw === "number"
          ? rec?.estadoInteraccionRaw
          : null) ??
        estadoActual ??
        0,
      fechaProximoContactoRaw: rec?.fechaProximoContacto
        ? new Date(rec.fechaProximoContacto).toISOString().split("T")[0]
        : "",
      notasInteraccion: rec?.notasInteraccion || rec?.notas || "",
    };
  }

  // Mostrar modal sin bloquear con Loader
  showUpdateModal.value = true;
};

const closeUpdateModal = () => {
  showUpdateModal.value = false;
};

const updateRecordatorio = async () => {
  try {
    isLoading.value = true;

    const payload = {
      id: currentRecordatorio.value.interaccionId,
      tipo: currentRecordatorio.value.tipoInteraccion,
      descripcion: currentRecordatorio.value.notasInteraccion,
      fechaVencimiento: currentRecordatorio.value.fechaProximoContactoRaw
        ? new Date(
            currentRecordatorio.value.fechaProximoContactoRaw
          ).toISOString()
        : null,
      statusInteraccion:
        statusMap[currentRecordatorio.value.estadoInteraccionRaw],
    };

    await interaccionService.updateInteraccion(payload);

    Swal.fire({
      icon: "success",
      title: "¡Actualizado!",
      text: "El estado y la interacción se guardaron correctamente.",
      timer: 1500,
      showConfirmButton: false,
    });

    closeUpdateModal();
    await loadInteracciones(); // Recargar interacciones después de actualizar
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "No se pudo actualizar el estado.",
      confirmButtonText: "Entendido",
    });
  } finally {
    isLoading.value = false;
  }
};

const addInteraccion = async () => {
  try {
    isLoading.value = true;

    // Crear el payload para la nueva interacción
    const nuevaInteraccion = {
      fecha: new Date().toISOString(),
      fechaVencimiento: currentRecordatorio.value.fechaProximoContactoRaw
        ? new Date(
            currentRecordatorio.value.fechaProximoContactoRaw
          ).toISOString()
        : null,
      tipo: currentRecordatorio.value.tipoInteraccion,
      descripcion: currentRecordatorio.value.notasInteraccion,
      statusInteraccion:
        statusMap[currentRecordatorio.value.estadoInteraccionRaw],
      agenteId: loggedAgente.value?.id || null,
      clienteId: cliente.value?.id || null,
    };

    // Enviar la nueva interacción a la API
    await interaccionService.addInteraccion(nuevaInteraccion);

    Swal.fire({
      icon: "success",
      title: "¡Interacción añadida!",
      text: "La nueva interacción se guardó correctamente.",
      timer: 1500,
      showConfirmButton: false,
    });

    // Recargar las interacciones después de agregar la nueva
    await loadInteracciones();
    closeUpdateModal();
  } catch (error) {
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "No se pudo agregar la nueva interacción.",
      confirmButtonText: "Entendido",
    });
  } finally {
    isLoading.value = false;
  }
};

</script>

<style>
.modal-overlay {
  backdrop-filter: blur(2px);
  z-index: 1000; /* Menor que el z-index de modalRequerimiento.vue */
}
</style>
`
