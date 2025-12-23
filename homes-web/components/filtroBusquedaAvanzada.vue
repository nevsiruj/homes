<template>
  <form class="w-80 md:w-full max-w-5xl mx-auto" @submit.prevent="handleSubmit">
    <div
      class="max-w-4xl mx-auto bg-white rounded-lg shadow-md overflow-visible"
    >
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
                'text-black border-black ': activeTab === 'busqueda',
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
                'text-black border-black ': activeTab === 'nombre',
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
                'text-black border-black ': activeTab === 'codigo',
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
            'p-4 rounded-lg bg-gray-50 ': true,
            hidden: activeTab !== 'busqueda',
          }"
          id="profile"
          role="tabpanel"
          aria-labelledby="profile-tab"
        >
          <div class="grid grid-cols-2 md:flex">
            <div class="relative inline-block">
              <button
                id="saleRentDropdownButton"
                @click="toggleDropdown('saleRent')"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-xs p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto flex-shrink-0"
                type="button"
              >
                {{ filters.tipoTransaccion || "Venta/Renta" }}
                <svg
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
                v-show="openDropdown === 'saleRent'"
                @click.stop
                id="saleRentDropdownMenu"
                class="absolute z-[9999] w-56 md:w-48 bg-white divide-y divide-gray-100 shadow-xl border border-gray-200 rounded-lg mt-1 top-full left-0"
              >
              <ul
                class="p-3 space-y-3 text-xs text-gray-800"
                aria-labelledby="saleRentDropdownButton"
              >
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
                      Ventas ({{ displayCount(propertyCounts.tipoTransaccion.venta) }})
                    </label>
                  </div>
                </li>
                <li>
                  <div class="flex items-center">
                    <input
                      id="radio-Renta"
                      type="radio"
                      value="Renta"
                      name="sale-rent-group"
                      v-model="filters.tipoTransaccion"
                      class="w-4 h-4 bg-gray-100 border-gray-300 rounded-full focus:ring-black accent-black"
                    />
                    <label
                      for="radio-Renta"
                      class="ms-2 text-xs text-gray-900 font-roboto"
                    >
                      Renta ({{ displayCount(propertyCounts.tipoTransaccion.renta) }})
                    </label>
                  </div>
                </li>
              </ul>
            </div>
            </div>

            <div class="relative inline-block w-full">
              <button
                id="dropdownCheckboxButton1"
                @click="toggleDropdown('propiedades')"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-xs w-full p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto"
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
              v-show="openDropdown === 'propiedades'"
              @click.stop
              id="dropdownDefaultCheckbox1"
              class="absolute z-[9999] w-64 md:w-48 bg-white divide-y divide-gray-100 shadow-xl border border-gray-200 max-h-[60vh] overflow-y-auto rounded-lg mt-1 top-full left-0"
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
                        Casas ({{ displayCount(propertyCounts.tipoPropiedad.Casa) }})
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
                        Edificio ({{ displayCount(propertyCounts.tipoPropiedad.Edificio) }})
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
                        Apartamentos ({{ displayCount(propertyCounts.tipoPropiedad.Apartamento) }})
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
                        Terreno ({{ displayCount(propertyCounts.tipoPropiedad.Terreno) }})
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
                        Local ({{ displayCount(propertyCounts.tipoPropiedad.Local) }})
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
                        Oficina ({{ displayCount(propertyCounts.tipoPropiedad.Oficina) }})
                      </label>
                    </div>
                  </li>
                </ul>
            </div>
            </div>

            <div class="relative inline-block w-full">
              <button
                id="dropdownCheckboxButton2"
                @click="toggleDropdown('ubicaciones')"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-xs w-full p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto"
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
              v-show="openDropdown === 'ubicaciones'"
              @click.stop
              id="dropdownDefaultCheckbox2"
              class="absolute z-[9999] w-64 md:w-48 bg-white divide-y divide-gray-100 shadow-xl border border-gray-200 max-h-[60vh] overflow-y-auto rounded-lg mt-1 top-full left-0"
            >
            <ul
                  class="p-3 space-y-3 text-xs text-gray-800"
                  aria-labelledby="dropdownCheckboxButton2"
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
                        San José Pinula ({{ displayCount(propertyCounts.ubicaciones["San José Pinula"]) }})
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
                        CAES Arriba KM 14 ({{ displayCount(propertyCounts.ubicaciones["CAES Arriba KM 14"]) }})
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
                        CAES Abajo KM 14 ({{ displayCount(propertyCounts.ubicaciones["CAES Abajo KM 14"]) }})
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
                        Antigua ({{ displayCount(propertyCounts.ubicaciones.Antigua) }})
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
                        Muxbal ({{ displayCount(propertyCounts.ubicaciones.Muxbal) }})
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
                        Playa ({{ displayCount(propertyCounts.ubicaciones.Playa) }})
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
                        Santa Catarina Pinula ({{ displayCount(propertyCounts.ubicaciones["Santa Catarina Pinula"]) }})
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
                        San Cristóbal ({{ displayCount(propertyCounts.ubicaciones["San Cristóbal"]) }})
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
                        Zona 4 ({{ displayCount(propertyCounts.ubicaciones["Zona 4"]) }})
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
                        Zona 9 ({{ displayCount(propertyCounts.ubicaciones["Zona 9"]) }})
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
                        Zona 10 ({{ displayCount(propertyCounts.ubicaciones["Zona 10"]) }})
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
                        Zona 13 ({{ displayCount(propertyCounts.ubicaciones["Zona 13"]) }})
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
                        Zona 14 ({{ displayCount(propertyCounts.ubicaciones["Zona 14"]) }})
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
                        Zona 15 ({{ displayCount(propertyCounts.ubicaciones["Zona 15"]) }})
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
                        Zona 16 ({{ displayCount(propertyCounts.ubicaciones["Zona 16"]) }})
                      </label>
                    </div>
                  </li>
                </ul>
            </div>
            </div>

            <div class="relative inline-block w-full">
              <button
                id="dropdownCheckboxButton3"
                @click="toggleDropdown('habitaciones')"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-xs w-full p-2.5 focus:ring-black focus:border-black inline-flex items-center font-roboto"
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
              v-show="openDropdown === 'habitaciones'"
              @click.stop
              id="dropdownDefaultCheckbox3"
              class="absolute z-[9999] w-48 md:w-40 bg-white divide-y divide-gray-100 shadow-xl border border-gray-200 max-h-[60vh] overflow-y-auto rounded-lg mt-1 top-full left-0"
            >
              <ul
                class="p-3 space-y-3 text-xs text-gray-800"
                aria-labelledby="dropdownCheckboxButton3"
              >
                <li v-for="(count, habs) in sortedHabitaciones" :key="habs">
                  <div class="flex items-center">
                    <input
                      :id="`checkbox-${habs}habitaciones`"
                      type="checkbox"
                      :value="parseInt(habs)"
                      v-model="filters.habitaciones"
                      class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                    />
                    <label
                      :for="`checkbox-${habs}habitaciones`"
                      class="ms-2 text-xs text-gray-900 font-roboto"
                    >
                      {{ habs }} ({{ count }})
                    </label>
                  </div>
                </li>
              </ul>
            </div>
            </div>

            <input
              type="number"
              v-model.number="filters.precioMinimo"
              id="min_price_input"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-xs block w-full p-2.5 font-roboto"
              placeholder="$ Mínimo"
            />

            <input
              type="number"
              v-model.number="filters.precioMaximo"
              id="max_price_input"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-xs block w-full p-2.5"
              placeholder="$ Máximo"
            />

            <div class="col-span-2 flex justify-center mt-2 lg:mt-0">
              <button
                type="submit"
                class="text-white bg-black hover:bg-gray-800 font-medium md:rounded-lg text-sm w-56 rounded-md lg:w-full md:w-full ml-2 px-5 py-2.5 text-center font-roboto"
              >
                Buscar
              </button>
            </div>
          </div>

          <div class="mt-8 pt-4 border-t border-gray-200">
            <h3 class="text-gray-900 text-base font-title mb-4">
              Características de la propiedad
            </h3>
            <div class="grid grid-cols-2 md:grid-cols-4 gap-x-4 gap-y-2">
              <div
                v-for="caracteristica in availableCaracteristicas"
                :key="caracteristica"
                class="flex items-center"
              >
                <input
                  type="checkbox"
                  :id="`caracteristica_${kebabCase(caracteristica)}`"
                  name="caracteristicas_propiedad"
                  :value="caracteristica"
                  v-model="filters.caracteristicasPropiedad"
                  class="w-4 h-4 bg-gray-100 border-gray-300 rounded-sm focus:ring-black accent-black"
                />
                <label
                  :for="`caracteristica_${kebabCase(caracteristica)}`"
                  class="ms-2 text-sm text-gray-900 font-roboto"
                >
                  {{ caracteristica }}
                </label>
              </div>
            </div>
          </div>
        </div>
        <div
          :class="{
            'p-4 rounded-lg bg-gray-50 ': true,
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
            class="block w-full p-2.5 text-sm lg:mr-2 md:mr-2 text-gray-900 bg-gray-50 border border-gray-300 rounded-lg focus:ring-black focus:border-black"
            placeholder="Ingrese Palabra clave"
          />
          <button
            type="submit"
            class="text-white bg-black hover:bg-gray-800 font-medium rounded-lg text-sm px-5 py-2.5 w-56 lg:w-42 mt-2 lg:mt-0 md:mt-0 font-roboto"
            :disabled="!filters.nombre.trim()"
            @click="handleSearchByKeyword"
          >
            Buscar por Palabra
          </button>
        </div>

        <div
          :class="{
            'p-4 rounded-lg bg-gray-50 ': true,
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
            class="block w-full p-2.5 text-sm lg:mr-2 md:mr-2 text-gray-900 bg-gray-50 border border-gray-300 rounded-lg focus:ring-black focus:border-black"
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
</template>

<script setup>
import { ref, onMounted, watch, computed, onBeforeUnmount } from "vue";
import { useRouter, useRoute } from "vue-router";
import inmuebleService from "@/services/inmuebleService";

const router = useRouter();
const route = useRoute();

const activeTab = ref("busqueda");
const openDropdown = ref(null);

const filters = ref({
  tipoTransaccion: "",
  tipoPropiedad: [],
  ubicaciones: [],
  habitaciones: [],
  caracteristicasPropiedad: [], // Aquí se recolectan las amenidades del formulario
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

const countsLoading = ref(true);

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

    // Asegúrate de que el nombre de la propiedad en tu JSON de respuesta sea 'amenidades' o 'Amenidades'
    // Si viene como un array anidado dentro de '$values', ajústalo:
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
    countsLoading.value = true;
    // OPTIMIZACIÓN: Reducido de 3x200 a 2x100 para mejorar rendimiento inicial
    const pagesToFetch = 2; // Reducido de 3 a 2
    const pageSize = 100; // Reducido de 200 a 100 (total: 200 en lugar de 600)
    const fetches = [];
    for (let i = 1; i <= pagesToFetch; i++) {
      fetches.push(inmuebleService.getInmueblesPaginados(i, pageSize));
    }
    const results = await Promise.all(fetches);
    const combined = results.flatMap(r => (r && r.items) ? r.items : []);
    if (combined && combined.length > 0) {
      allInmueblesForCounts.value = combined;
      calculatePropertyCounts();
    }
    countsLoading.value = false;
  } catch (error) {
    //console.error("Error al cargar todos los inmuebles para conteos:", error);
    countsLoading.value = false;
  }
};

const displayCount = (count) => {
  if (countsLoading.value && (count === null || count === undefined)) {
    return "—";
  }
  return count || 0;
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
      backendFilters.Habitaciones = filters.value.habitaciones;
    }
    // ****** CAMBIO CRUCIAL AQUÍ: Mapear a 'Amenidades' ******
    if (filters.value.caracteristicasPropiedad.length > 0) {
      backendFilters.Amenidades = filters.value.caracteristicasPropiedad;
    }
    // if (filters.value.precioMinimo !== null && filters.value.precioMinimo !== "") {
    //   backendFilters.PrecioMinimo = filters.value.precioMinimo;
    // }
    // if (filters.value.precioMaximo !== null && filters.value.precioMaximo !== "") {
    //   backendFilters.PrecioMaximo = filters.value.precioMaximo;
    // }

    if (min !== null && min !== "" && (max === null || max === "")) {
      const base = Number(min);
      const twentyPercent = base * 0.2;
      backendFilters.PrecioMinimo = Math.floor(base - twentyPercent);
      backendFilters.PrecioMaximo = Math.ceil(base + twentyPercent);
    } else {
      if (min !== null && min !== "") {
        backendFilters.PrecioMinimo = Number(min);
      }
      if (max !== null && max !== "") {
        backendFilters.PrecioMaximo = Number(max);
      }
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
    if (query.Habitaciones) {
      filters.value.habitaciones = Array.isArray(query.Habitaciones)
        ? query.Habitaciones.map(Number)
        : [Number(query.Habitaciones)];
    }
    // ****** CAMBIO CRUCIAL AQUÍ: Leer de 'Amenidades' de la URL ******
    if (query.Amenidades) {
      filters.value.caracteristicasPropiedad = Array.isArray(query.Amenidades)
        ? query.Amenidades
        : [query.Amenidades];
    }
    if (query.PrecioMinimo) {
      filters.value.precioMinimo = Number(query.PrecioMinimo);
    }
    if (query.PrecioMaximo) {
      filters.value.precioMaximo = Number(query.PrecioMaximo);
    }
  }
};

const handleSubmit = () => {
  const filtersToSend = buildBackendFilters();

  if (filtersToSend === null) {
    return;
  }

  router.push({ path: "/busqueda", query: filtersToSend });
};

const handleSearchByKeyword = () => {
  const rawSearchTerm = filters.value.nombre.trim();

  if (!rawSearchTerm) {
    return; // Do nothing if the search term is empty
  }

  // Process the search term to match the backend logic
  const words = rawSearchTerm
    .split(/\s+/) // Split by whitespace
    .filter((word) => word.length >= 2) // Ignore words shorter than 2 characters
    .map((word) => word.toLowerCase()) // Convert to lowercase
    .join(" "); // Join the processed words back into a single string

  console.log("Processed SearchTerm:", words); // Log the processed search term

  // Update the query parameters to trigger the watcher in `index.vue`
  router.push({
    path: "/busqueda",
    query: { SearchTerm: words }, // Ensure the query parameter is named `SearchTerm`
  });
};

// Función para toggle dropdowns manualmente
const toggleDropdown = (dropdownName) => {
  if (openDropdown.value === dropdownName) {
    openDropdown.value = null;
  } else {
    openDropdown.value = dropdownName;
  }
};

// Cerrar dropdown al hacer click fuera
const handleClickOutside = (event) => {
  const dropdowns = ['saleRent', 'propiedades', 'ubicaciones', 'habitaciones'];
  const clickedInsideDropdown = dropdowns.some(name => {
    const element = document.getElementById(
      name === 'saleRent' ? 'saleRentDropdownMenu' : 
      name === 'propiedades' ? 'dropdownDefaultCheckbox1' :
      name === 'ubicaciones' ? 'dropdownDefaultCheckbox2' :
      'dropdownDefaultCheckbox3'
    );
    return element && element.contains(event.target);
  });
  
  const clickedButton = event.target.closest('[id*="dropdownCheckbox"], [id*="saleRentDropdown"]');
  
  if (!clickedInsideDropdown && !clickedButton) {
    openDropdown.value = null;
  }
};

// Cerrar dropdowns al hacer scroll
const handleScroll = () => {
  if (openDropdown.value) {
    openDropdown.value = null;
  }
};

watch(activeTab, (newTab, oldTab) => {
  if (newTab !== oldTab) {
    // Cerrar cualquier dropdown abierto al cambiar de tab
    openDropdown.value = null;
    
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
  { deep: true, immediate: true } // Removed the extra closing brace here
);

onMounted(() => {
  loadAllInmueblesForCounts();
  // Agregar listener para cerrar dropdowns al hacer click fuera
  document.addEventListener('click', handleClickOutside);
  // Agregar listener para cerrar dropdowns al hacer scroll
  window.addEventListener('scroll', handleScroll, true);
});

onBeforeUnmount(() => {
  // Limpiar listeners al desmontar el componente
  document.removeEventListener('click', handleClickOutside);
  window.removeEventListener('scroll', handleScroll, true);
});
</script>

<style scoped>
.font-roboto {
  font-family: "Roboto", sans-serif;
}

/* Los dropdowns ahora se manejan con CSS puro sin Flowbite */
.relative {
  position: relative;
}

/* Asegurar que los dropdowns aparezcan encima de todo */
[id*="dropdown"] {
  z-index: 9999;
}
</style>
