<template>
  <div
    class="absolute left-1/2 -translate-x-1/2 -translate-y-1/3 w-full px-4 z-50"
  >
    <form class="max-w-5xl mx-auto" @submit.prevent="handleSubmit">
      <div class="bg-white rounded-lg shadow-md overflow-hidden">
        <div class="mb-4 border-b border-gray-200">
          <ul
            class="flex flex-wrap -mb-px text-sm font-medium text-center"
            id="default-tab"
            data-tabs-toggle="#default-tab-content"
            role="tablist"
          >
            <li class="me-2" role="presentation">
              <button
                :class="{
                  'inline-block p-4 border-b-2 rounded-t-lg font-roboto': true,
                  'text-black border-black': activeTab === 'busqueda',
                  'text-gray-500 hover:text-black hover:border-gray-300 ':
                    activeTab !== 'busqueda',
                }"
                id="profile-tab"
                data-tabs-target="#profile"
                type="button"
                role="tab"
                aria-controls="profile"
                :aria-selected="activeTab === 'busqueda'"
                @click="activeTab = 'busqueda'"
              >
                BÚSQUEDA
              </button>
            </li>
            <li class="me-2" role="presentation">
              <button
                :class="{
                  'inline-block p-4 border-b-2 rounded-t-lg font-roboto': true,
                  'text-black border-black': activeTab === 'nombre',
                  'text-gray-500 hover:text-black hover:border-gray-300 ':
                    activeTab !== 'nombre',
                }"
                id="dashboard-tab"
                data-tabs-target="#dashboard"
                type="button"
                role="tab"
                aria-controls="dashboard"
                :aria-selected="activeTab === 'nombre'"
                @click="activeTab = 'nombre'"
              >
                PALABRAS CLAVE
              </button>
            </li>
            <li class="me-2" role="presentation">
              <button
                :class="{
                  'inline-block p-4 border-b-2 rounded-t-lg font-roboto': true,
                  'text-black border-black': activeTab === 'codigo',
                  'text-gray-500 hover:text-black hover:border-gray-300 ':
                    activeTab !== 'codigo',
                }"
                id="settings-tab"
                data-tabs-target="#settings"
                type="button"
                role="tab"
                aria-controls="settings"
                :aria-selected="activeTab === 'codigo'"
                @click="activeTab = 'codigo'"
              >
                CÓDIGO
              </button>
            </li>
          </ul>
        </div>
        <div id="default-tab-content">
          <div
            :class="{
              'p-4 rounded-lg ': true,
              hidden: activeTab !== 'busqueda',
            }"
            id="profile"
            role="tabpanel"
            aria-labelledby="profile-tab"
          >
            <div class="grid grid-cols-2 md:flex">
              <button
                id="saleRentDropdownButton"
                data-dropdown-toggle="saleRentDropdownMenu"
                class="border border-gray-300 text-gray-900 text-xs p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto flex-shrink-0"
                type="button"
              >
                {{ filters.tipoTransaccion || "Venta/Renta"
                }}<svg
                  class="w-2.5 h-2.5 ms-3"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 10 6"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="m1 1 4 4 4-4"
                  />
                </svg>
              </button>

              <div
                id="saleRentDropdownMenu"
                class="z-10 hidden w-42 bg-white divide-y divide-gray-100 shadow-sm trans"
              >
                <ul
                  class="p-3 space-y-3 text-xs text-gray-800"
                  aria-labelledby="saleRentDropdownButton"
                >
                  <li>
                    <div class="flex items-center">
                      <input
                        id="radio-Venta/Renta"
                        type="radio"
                        name="sale-rent-group"
                        @change="onClickVentaRenta"
                        :checked="isVentaRentaSelected"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-full focus:ring-black accent-black"
                      />
                      <label
                        for="radio-Venta/Renta"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Venta/Renta ({{
                          propertyCounts.tipoTransaccion.venta +
                            propertyCounts.tipoTransaccion.renta || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="radio-Venta"
                        type="radio"
                        value="Venta"
                        name="sale-rent-group"
                        v-model="filters.tipoTransaccion"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-full focus:ring-black accent-black"
                      />
                      <label
                        for="radio-Venta"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Venta ({{ propertyCounts.tipoTransaccion.venta || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="radio-renta"
                        type="radio"
                        value="Renta"
                        name="sale-rent-group"
                        v-model="filters.tipoTransaccion"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-full focus:ring-black accent-black"
                      />
                      <label
                        for="radio-renta"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Renta ({{ propertyCounts.tipoTransaccion.renta || 0 }})
                      </label>
                    </div>
                  </li>
                </ul>
              </div>
              <button
                id="dropdownCheckboxButton1"
                data-dropdown-toggle="dropdownDefaultCheckbox1"
                class="border border-gray-300 text-gray-900 text-xs w-full p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto"
                type="button"
              >
                Propiedades<svg
                  class="w-2.5 h-2.5 ms-3"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 10 6"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="m1 1 4 4 4-4"
                  />
                </svg>
              </button>
              <div
                id="dropdownDefaultCheckbox1"
                class="z-10 hidden w-42 bg-white divide-y divide-gray-100 shadow-sm trans"
              >
                <ul
                  class="p-3 space-y-3 text-xs text-gray-800"
                  aria-labelledby="dropdownCheckboxButton1"
                >
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-casas"
                        type="checkbox"
                        value="Casa"
                        v-model="filters.tipoPropiedad"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-casas"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Casas ({{ propertyCounts.tipoPropiedad.Casa || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-edificio"
                        type="checkbox"
                        value="Edificio"
                        v-model="filters.tipoPropiedad"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-edificio"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Edificio ({{
                          propertyCounts.tipoPropiedad.Edificio || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-apartamentos"
                        type="checkbox"
                        value="Apartamento"
                        v-model="filters.tipoPropiedad"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-apartamentos"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Apartamentos ({{
                          propertyCounts.tipoPropiedad.Apartamento || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-terreno"
                        type="checkbox"
                        value="Terreno"
                        v-model="filters.tipoPropiedad"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-terreno"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Terreno ({{
                          propertyCounts.tipoPropiedad.Terreno || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-local"
                        type="checkbox"
                        value="Local"
                        v-model="filters.tipoPropiedad"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-local"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Local ({{ propertyCounts.tipoPropiedad.Local || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-oficina"
                        type="checkbox"
                        value="Oficina"
                        v-model="filters.tipoPropiedad"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-oficina"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Oficina ({{
                          propertyCounts.tipoPropiedad.Oficina || 0
                        }})
                      </label>
                    </div>
                  </li>
                </ul>
              </div>
              <button
                id="dropdownCheckboxButton2"
                data-dropdown-toggle="dropdownDefaultCheckbox2"
                class="border border-gray-300 text-gray-900 text-xs w-full p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto"
                type="button"
              >
                Ubicación<svg
                  class="w-2.5 h-2.5 ms-3"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 10 6"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="m1 1 4 4 4-4"
                  />
                </svg>
              </button>
              <div
                id="dropdownDefaultCheckbox2"
                class="z-10 hidden w-48 bg-white divide-y divide-gray-100 shadow-sm trans"
              >
                <ul
                  class="p-3 space-y-3 text-xs text-gray-800"
                  aria-labelledby="dropdownCheckboxButton1"
                >
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-sanjosepinula"
                        type="checkbox"
                        value="San José Pinula"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-sanjosepinula"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        San José Pinula ({{
                          propertyCounts.ubicaciones["San José Pinula"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-caesarriba"
                        type="checkbox"
                        value="CAES Arriba KM 14"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-caesarriba"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        CAES Arriba KM 14 ({{
                          propertyCounts.ubicaciones["CAES Arriba KM 14"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-caesabajo"
                        type="checkbox"
                        value="CAES Abajo KM 14"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-caesabajo"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        CAES Abajo KM 14 ({{
                          propertyCounts.ubicaciones["CAES Abajo KM 14"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Antigua"
                        type="checkbox"
                        value="Antigua"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Antigua"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Antigua ({{ propertyCounts.ubicaciones.Antigua || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-muxbal"
                        type="checkbox"
                        value="Muxbal"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-muxbal"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Muxbal ({{ propertyCounts.ubicaciones.Muxbal || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-playa"
                        type="checkbox"
                        value="Playa"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Playa"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Playa ({{ propertyCounts.ubicaciones.Playa || 0 }})
                      </label>
                    </div>
                  </li>

                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Santa Catarina Pinula"
                        type="checkbox"
                        value="Santa Catarina Pinula"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Santa Catarina Pinula"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Santa Catarina Pinula ({{
                          propertyCounts.ubicaciones["Santa Catarina Pinula"] ||
                          0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-San Cristóbal"
                        type="checkbox"
                        value="San Cristóbal"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-San Cristóbal"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        San Cristóbal ({{
                          propertyCounts.ubicaciones["San Cristóbal"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 4"
                        type="checkbox"
                        value="Zona 4"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 4"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 4 ({{ propertyCounts.ubicaciones["Zona 4"] || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 9"
                        type="checkbox"
                        value="<Zona 9>"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 9"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 9 ({{ propertyCounts.ubicaciones["Zona 9"] || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 10"
                        type="checkbox"
                        value="Zona 10"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 10"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 10 ({{
                          propertyCounts.ubicaciones["Zona 10"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 13"
                        type="checkbox"
                        value=">Zona 13"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 13"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 13 ({{
                          propertyCounts.ubicaciones["Zona 13"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 14"
                        type="checkbox"
                        value="Zona 14"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 14"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 14 ({{
                          propertyCounts.ubicaciones["Zona 14"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 15"
                        type="checkbox"
                        value="Zona 15"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 15"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 15 ({{
                          propertyCounts.ubicaciones["Zona 15"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-Zona 16"
                        type="checkbox"
                        value="Zona 16"
                        v-model="filters.ubicaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-Zona 16"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        Zona 16 ({{
                          propertyCounts.ubicaciones["Zona 16"] || 0
                        }})
                      </label>
                    </div>
                  </li>
                </ul>
              </div>
              <button
                id="dropdownCheckboxButton3"
                data-dropdown-toggle="dropdownDefaultCheckbox3"
                class="border border-gray-300 text-gray-900 text-xs w-full p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto"
                type="button"
              >
                Habitaciones<svg
                  class="w-2.5 h-2.5 ms-3"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 10 6"
                >
                  <path
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="m1 1 4 4 4-4"
                  />
                </svg>
              </button>
              <div
                id="dropdownDefaultCheckbox3"
                class="z-10 hidden w-32 bg-white divide-y divide-gray-100 shadow-sm trans"
              >
                <ul
                  class="p-3 space-y-3 text-xs text-gray-800"
                  aria-labelledby="dropdownCheckboxButton2"
                >
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-1habitacion"
                        type="checkbox"
                        :value="1"
                        v-model="filters.habitaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-1habitacion"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        1 ({{ propertyCounts.habitaciones["1"] || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-2habitaciones"
                        type="checkbox"
                        :value="2"
                        v-model="filters.habitaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-2habitaciones"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        2 ({{ propertyCounts.habitaciones["2"] || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-3habitaciones"
                        type="checkbox"
                        :value="3"
                        v-model="filters.habitaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-3habitaciones"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        3 ({{ propertyCounts.habitaciones["3"] || 0 }})
                      </label>
                    </div>
                  </li>
                  <li>
                    <div class="flex items-center">
                      <input
                        id="checkbox-4habitaciones"
                        type="checkbox"
                        :value="4"
                        v-model="filters.habitaciones"
                        class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                      />
                      <label
                        for="checkbox-4habitaciones"
                        class="ms-2 text-xs text-gray-900 font-roboto"
                      >
                        4 ({{ propertyCounts.habitaciones["4"] || 0 }})
                      </label>
                    </div>
                  </li>
                </ul>
              </div>
              <input
                type="number"
                v-model.number="filters.precioMinimo"
                id="min_price_input"
                class="border border-gray-300 text-gray-900 text-xs block w-full p-2.5 font-roboto focus:ring-black focus:border-black"
                placeholder="$ Mínimo"
              />

              <input
                type="number"
                v-model.number="filters.precioMaximo"
                id="max_price_input"
                class="border border-gray-300 text-gray-900 text-xs block w-full p-2.5"
                placeholder="$ Máximo"
              />

              <div class="col-span-2 flex justify-center mt-2 lg:mt-0">
                <button
                  type="submit"
                  class="text-white bg-black hover:bg-gray-800 font-medium rounded-lg ml-2 text-sm w-56 sm:w-auto px-5 py-2.5 text-center font-roboto"
                >
                  Buscar
                </button>
              </div>
            </div>
          </div>
          <div
            :class="{
              'p-4 rounded-lg  ': true,
              hidden: activeTab !== 'nombre',
            }"
            id="dashboard"
            role="tabpanel"
            aria-labelledby="dashboard-tab"
            class="flex flex-col lg:flex-row md:flex-row items-center"
          >
            <input
              type="text"
              v-model="filters.nombre"
              name="property_name"
              id="property_name"
              class="block w-full p-2.5 text-sm lg:mr-2 md:mr-2 text-gray-900 border border-gray-300 rounded-lg focus:ring-black focus:border-black"
              placeholder="Ingrese Palabra clave"
            />
            <button
              type="submit"
              class="text-white bg-black hover:bg-gray-800 font-medium rounded-lg text-sm px-5 py-2.5 w-56 lg:w-42 mt-2 lg:mt-0 md:mt-0 font-roboto"
              :disabled="!filters.nombre.trim()"
            >
              Buscar por Palabra
            </button>
          </div>

          <div
            :class="{
              'p-4 rounded-lg  ': true,
              hidden: activeTab !== 'codigo',
            }"
            id="settings"
            role="tabpanel"
            aria-labelledby="settings-tab"
            class="flex flex-col lg:flex-row md:flex-row items-center"
          >
            <input
              type="text"
              v-model="filters.codigo"
              name="property_code"
              id="property_code"
              class="block w-full p-2.5 text-sm lg:mr-2 md:mr-2 text-gray-900 border border-gray-300 rounded-lg focus:ring-black focus:border-black"
              placeholder="Ingrese código de la propiedad"
            />

            <button
              type="submit"
              
              class="text-white bg-black hover:bg-gray-800 font-medium rounded-lg text-sm px-5 py-2.5 w-56 lg:w-42 mt-2 lg:mt-0 md:mt-0 font-roboto"
            >
              Buscar por Código
            </button>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, computed } from "vue";
import { useRouter, useRoute } from "vue-router";
import { initFlowbite } from "flowbite";
import inmuebleService from "@/services/inmuebleService";

const router = useRouter();
const route = useRoute();

const activeTab = ref("busqueda");

const filters = ref({
  tipoTransaccion: "",
  tipoPropiedad: [],
  ubicaciones: [],
  habitaciones: [],
  caracteristicasPropiedad: [],
  precioMinimo: null,
  precioMaximo: null,
  nombre: "",
  codigo: "",
});

const propertyCounts = ref({
  tipoPropiedad: {},
  ubicaciones: {},
  habitaciones: {},
  tipoTransaccion: {},
  caracteristicasPropiedad: {},
});

const allInmueblesForCounts = ref([]);

const availableCaracteristicas = ref([
  "Airbnb",
  "Amueblado",
  "Área de Juegos",
  "Área verde",
  "Balcón",
  "Bodega",
  "Business Center",
  "Canchas deportivas",
  "Cuarto de Servicio",
  "Estudio",
  "Gimnasio",
  "Jardín",
  "Lavandería",
  "No mascotas",
  "Parqueo de Visitas",
  "Pet friendly",
  "Pet Garden",
  "Piscina",
  "Sala familiar",
  "Salón Social",
]);

const kebabCase = (str) => {
  return str
    .replace(/[^a-zA-Z0-9 ]/g, "")
    .replace(/([a-z0-9]|(?=[A-Z]))([A-Z])/g, "$1-$2")
    .toLowerCase()
    .replace(/\s+/g, "-");
};

const calculatePropertyCounts = () => {
  const counts = {
    tipoPropiedad: {},
    ubicaciones: {},
    habitaciones: {},
    tipoTransaccion: {},
    caracteristicasPropiedad: {},
  };

  allInmueblesForCounts.value.forEach((inmueble) => {
    const operacionValue = inmueble.operaciones;
    if (operacionValue) {
      const operacionKey = String(operacionValue).toLowerCase();
      if (operacionKey === "venta" || operacionKey === "renta") {
        counts.tipoTransaccion[operacionKey] =
          (counts.tipoTransaccion[operacionKey] || 0) + 1;
      }
    }

    const tipoValue = inmueble.tipos;
    if (tipoValue) {
      const tipoKey = String(tipoValue);
      counts.tipoPropiedad[tipoKey] = (counts.tipoPropiedad[tipoKey] || 0) + 1;
    }

    const ubicacionValue = inmueble.ubicaciones;
    if (ubicacionValue) {
      const ubicacionKey = String(ubicacionValue);
      counts.ubicaciones[ubicacionKey] =
        (counts.ubicaciones[ubicacionKey] || 0) + 1;
    }

    const habitacionesValue = inmueble.habitaciones;
    if (habitacionesValue !== null && habitacionesValue !== undefined) {
      const habitacionesKey = String(habitacionesValue);
      counts.habitaciones[habitacionesKey] =
        (counts.habitaciones[habitacionesKey] || 0) + 1;
    }

    const amenidadesArray =
      inmueble.amenidades?.$values || inmueble.amenidades || [];
    if (Array.isArray(amenidadesArray)) {
      amenidadesArray.forEach((amenidad) => {
        const amenidadKey = String(amenidad);
        counts.caracteristicasPropiedad[amenidadKey] =
          (counts.caracteristicasPropiedad[amenidadKey] || 0) + 1;
      });
    }
  });

  propertyCounts.value = counts;
};

const loadAllInmueblesForCounts = async () => {
  try {
    const result = await inmuebleService.getInmueblesPaginados(1, 1000);
    if (result && result.items) {
      allInmueblesForCounts.value = result.items;
      calculatePropertyCounts();
    }
  } catch (error) {
    //console.error("Error al cargar todos los inmuebles para conteos:", error);
  }
};

const sortedHabitaciones = computed(() => {
  return Object.keys(propertyCounts.value.habitaciones)
    .sort((a, b) => parseInt(a) - parseInt(b))
    .reduce((obj, key) => {
      obj[key] = propertyCounts.value.habitaciones[key];
      return obj;
    }, {});
});

/**
 * Construye el objeto de filtros para enviar al backend, mapeando los nombres.
 */
const buildBackendFilters = () => {
  const backendFilters = {};
  const min = filters.value.precioMinimo;
  const max = filters.value.precioMaximo;

  if (activeTab.value === "busqueda") {
    if (filters.value.tipoTransaccion) {
      backendFilters.Operaciones = filters.value.tipoTransaccion;
    }
    if (filters.value.tipoPropiedad.length > 0) {
      backendFilters.Tipos = filters.value.tipoPropiedad;
    }
    if (filters.value.ubicaciones.length > 0) {
      backendFilters.Ubicaciones = filters.value.ubicaciones;
    }
    if (filters.value.habitaciones.length > 0) {
      const minHabitaciones = Math.min(...filters.value.habitaciones);

      backendFilters.HabitacionesMin = [minHabitaciones];
      backendFilters.Orden = "habitaciones_asc";
    }
    if (filters.value.caracteristicasPropiedad.length > 0) {
      backendFilters.Amenidades = filters.value.caracteristicasPropiedad;
    }

    const minPriceInput = filters.value.precioMinimo;
    const maxPriceInput = filters.value.precioMaximo;
    const hasMinPrice = minPriceInput !== null && minPriceInput !== "";
    const hasMaxPrice = maxPriceInput !== null && maxPriceInput !== "";

    if (hasMinPrice && hasMaxPrice) {
      backendFilters.PrecioMinimo = Number(minPriceInput);
      backendFilters.PrecioMaximo = Number(maxPriceInput);
      backendFilters.Orden = "asc"; // Ordena de menor a mayor si ambos filtros están presentes.
    } else if (hasMinPrice) {
      const basePrice = Number(minPriceInput);
      const range = basePrice * 100.0;
      backendFilters.PrecioMinimo = basePrice;
      backendFilters.PrecioMaximo = basePrice + range;
      backendFilters.RangoAutomatico = true; // Indica que el rango es automático
      backendFilters.Orden = "asc"; // Ordena de menor a mayor si se filtra por precio mínimo.
    } else if (hasMaxPrice) {
      const basePrice = Number(maxPriceInput);
      const range = basePrice * 100.0;
      backendFilters.PrecioMinimo = Math.max(0, basePrice - range);
      backendFilters.PrecioMaximo = basePrice;
      backendFilters.RangoAutomatico = true; // Indica que el rango es automático
      // Esta línea es la corrección clave. Ahora el frontend indica "desc" cuando se filtra por precio máximo.
      backendFilters.Orden = "desc";
    }
  } else if (activeTab.value === "nombre") {
    const searchTerm = (filters.value.nombre || "").trim();

    if (!searchTerm) {
      return null; // No enviar si el campo está vacío
    }

    backendFilters.SearchTerm = searchTerm; // Enviar como SearchTerm
  } else if (activeTab.value === "codigo") {
    if (filters.value.codigo.trim()) {
      backendFilters.CodigoPropiedad = filters.value.codigo.trim();
    } else {
      // console.warn(
      //   'El campo "Ingrese código de la propiedad" es requerido para la búsqueda por código.'
      // );
      return null; 
    }
  }

  //backendFilters.Luxury = true;

  return backendFilters;
};

const initializeFiltersFromUrl = () => {
  const query = route.query;

  if (query.Operaciones) {
    filters.value.tipoTransaccion = query.Operaciones;
  }

  if (query.Titulo) {
    activeTab.value = "nombre";
    filters.value.nombre = query.Titulo;
  } else if (query.CodigoPropiedad) {
    activeTab.value = "codigo";
    filters.value.codigo = query.CodigoPropiedad;
  } else if (Object.keys(query).length > 0) {
    activeTab.value = "busqueda";
    if (query.Tipos) {
      filters.value.tipoPropiedad = Array.isArray(query.Tipos)
        ? query.Tipos
        : [query.Tipos];
    }
    if (query.Ubicaciones) {
      filters.value.ubicaciones = Array.isArray(query.Ubicaciones)
        ? query.Ubicaciones
        : [query.Ubicaciones];
    }
    if (query.HabitacionesMin) {
      const minHabValue = Array.isArray(query.HabitacionesMin)
        ? query.HabitacionesMin[0]
        : query.HabitacionesMin;

      filters.value.habitaciones = [Number(minHabValue)];
    }
    if (query.Amenidades) {
      filters.value.caracteristicasPropiedad = Array.isArray(query.Amenidades)
        ? query.Amenidades
        : [query.Amenidades];
    }

    // Lógica de inicialización para precios con rango automático
    if (query.RangoAutomatico === "true") {
      // Si el rango es automático, solo actualiza el input que se usó para el cálculo.
      if (query.PrecioMinimo && !query.PrecioMaximo) {
        filters.value.precioMinimo = Number(query.PrecioMinimo);
        filters.value.precioMaximo = null; // Se mantiene vacío
      } else if (query.PrecioMaximo && !query.PrecioMinimo) {
        filters.value.precioMaximo = Number(query.PrecioMaximo);
        filters.value.precioMinimo = null; // Se mantiene vacío
      }
    } else {
      // Si no es un rango automático, actualiza ambos inputs normalmente.
      if (query.PrecioMinimo) {
        filters.value.precioMinimo = Number(query.PrecioMinimo);
      } else {
        filters.value.precioMinimo = null;
      }
      if (query.PrecioMaximo) {
        filters.value.precioMaximo = Number(query.PrecioMaximo);
      } else {
        filters.value.precioMaximo = null;
      }
    }
  }
};

const handleSubmit = () => {
  const filtersToSend = buildBackendFilters();
  if (filtersToSend === null) {
    return;
  }

  router.push({ path: "/propiedades", query: filtersToSend });
};

const handleSearchByKeyword = () => {
  const rawSearchTerm = filters.value.nombre.trim();

  if (!rawSearchTerm) {
    return; // Do nothing if the search term is empty
  }

  // Process the search term to match the backend logic
  const words = rawSearchTerm
    .split(/[^a-zA-Z0-9]+/) // Split by non-alphanumeric characters
    .filter((word) => word.length >= 2) // Ignore words shorter than 2 characters
    .map((word) => word.toLowerCase()) // Convert to lowercase
    .join(" "); // Join the processed words back into a single string

  console.log("Processed SearchTerm:", words); // Log the processed search term

  // Update the query parameters to trigger the watcher in `index.vue`
  router.push({
    path: "/propiedades",
    query: { ...route.query, SearchTerm: words },
  });
};

watch(activeTab, (newTab, oldTab) => {
  if (newTab !== oldTab) {
    filters.value = {
      tipoTransaccion: "",
      tipoPropiedad: [],
      ubicaciones: [],
      habitaciones: [],
      caracteristicasPropiedad: [],
      precioMinimo: null,
      precioMaximo: null,
      nombre: "",
      codigo: "",
    };
  }
});

watch(
  () => route.query,
  () => {
    initializeFiltersFromUrl();
  },
  { deep: true, immediate: true }
);

onMounted(() => {
  initFlowbite();
  loadAllInmueblesForCounts();
});

const resetAllFilters = () => {
  filters.value = {
    tipoTransaccion: "",
    tipoPropiedad: [],
    ubicaciones: [],
    habitaciones: [],
    caracteristicasPropiedad: [],
    precioMinimo: null,
    precioMaximo: null,
    nombre: "",
    codigo: "",
  };
};

const onClickVentaRenta = () => {
  resetAllFilters();
  router.push({ path: "/propiedades" });
};
const isVentaRentaSelected = computed(() => {
  return !filters.value.tipoTransaccion;
});
</script>
