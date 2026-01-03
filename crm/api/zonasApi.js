const zonas = [
  { id: 1, nombre: "Amatitlán", activo: true },
  { id: 2, nombre: "Antigua", activo: true },
  { id: 3, nombre: "Atitlan", activo: true },
  { id: 4, nombre: "CAES Abajo KM 14", activo: true },
  { id: 5, nombre: "CAES Arriba KM 14", activo: true },
  { id: 6, nombre: "Carr. Salvador", activo: true },
  { id: 7, nombre: "Fraijanes", activo: true },
  { id: 8, nombre: "Mixco", activo: true },
  { id: 9, nombre: "Monterrico", activo: true },
  { id: 10, nombre: "Muxbal", activo: true },
  { id: 11, nombre: "Playa", activo: true },
  { id: 12, nombre: "Puerto San José", activo: true },
  { id: 13, nombre: "San Cristóbal", activo: true },
  { id: 14, nombre: "San José Pinula", activo: true },
  { id: 15, nombre: "Santa Catarina Pinula", activo: true },
  { id: 16, nombre: "Zona 1", activo: true },
  { id: 17, nombre: "Zona 2", activo: true },
  { id: 18, nombre: "Zona 4", activo: true },
  { id: 19, nombre: "Zona 7", activo: true },
  { id: 20, nombre: "Zona 9", activo: true },
  { id: 21, nombre: "Zona 10", activo: true },
  { id: 22, nombre: "Zona 11", activo: true },
  { id: 23, nombre: "Zona 12", activo: true },
  { id: 24, nombre: "Zona 13", activo: true },
  { id: 25, nombre: "Zona 14", activo: true },
  { id: 26, nombre: "Zona 15", activo: true },
  { id: 27, nombre: "Zona 16", activo: true }
];

export const getAllZonas = () => {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve({
        success: true,
        data: zonas,
        total: zonas.length
      });
    }, 300);
  });
};

export const getZonaById = (id) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const zona = zonas.find(z => z.id === id);
      if (zona) {
        resolve({
          success: true,
          data: zona
        });
      } else {
        reject({
          success: false,
          message: "Zona no encontrada"
        });
      }
    }, 300);
  });
};

export const createZona = (zonaData) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      const newZona = {
        id: zonas.length + 1,
        ...zonaData,
        activo: true
      };
      zonas.push(newZona);
      resolve({
        success: true,
        data: newZona,
        message: "Zona creada exitosamente"
      });
    }, 300);
  });
};

export const updateZona = (id, zonaData) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const index = zonas.findIndex(z => z.id === id);
      if (index !== -1) {
        zonas[index] = { ...zonas[index], ...zonaData };
        resolve({
          success: true,
          data: zonas[index],
          message: "Zona actualizada exitosamente"
        });
      } else {
        reject({
          success: false,
          message: "Zona no encontrada"
        });
      }
    }, 300);
  });
};

export const deleteZona = (id) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const index = zonas.findIndex(z => z.id === id);
      if (index !== -1) {
        zonas.splice(index, 1);
        resolve({
          success: true,
          message: "Zona eliminada exitosamente"
        });
      } else {
        reject({
          success: false,
          message: "Zona no encontrada"
        });
      }
    }, 300);
  });
};
