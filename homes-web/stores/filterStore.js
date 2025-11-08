import { defineStore } from 'pinia';

export const useFilterStore = defineStore('filter', {
  state: () => ({
    currentFilters: {
      tipoTransaccion: '',
      tipoPropiedad: [],
      ubicaciones: [],
      habitaciones: [],
      precioMinimo: null,
      precioMaximo: null,
      nombre: '',
      codigo: '',
    },
  }),
  actions: {
    setFilters(newFilters) {
      const cleanedFilters = {};
      for (const key in newFilters) {
        const value = newFilters[key];
        if (Array.isArray(value)) {
          if (value.length > 0) {
            cleanedFilters[key] = value;
          }
        } else if (value !== null && value !== undefined && value !== '') {
          cleanedFilters[key] = value;
        }
      }
      this.currentFilters = cleanedFilters;
      //console.log('Filtros actualizados en el store:', this.currentFilters);
    },
    resetFilters() {
      this.currentFilters = {
        tipoTransaccion: '',
        tipoPropiedad: [],
        ubicaciones: [],
        habitaciones: [],
        precioMinimo: null,
        precioMaximo: null,
        nombre: '',
        codigo: '',
      };
    }
  },
  getters: {
    getAppliedFilters: (state) => state.currentFilters,
  },
});