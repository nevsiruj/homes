const clients = [
    {
      id: 1,
      nombre: "Juan Pérez",
      telefono: "+1234567890",
      email: "juan@example.com",
      preferencias: {
        tipo: "Apartamento",
        operacion: "Venta",
        ubicacion: "Centro",
        precioMin: 1000,
        precioMax: 3000,
        habitaciones: 2,
        banos: 2
      }
    },
    {
      id: 2,
      nombre: "María García",
      telefono: "+9876543210",
      email: "maria@example.com",
      preferencias: {
        tipo: "Casa",
        operacion: "Alquiler",
        ubicacion: "Norte",
        precioMin: 1000,
        precioMax: 1500,
        habitaciones: 3,
        banos: 3
      }
    },
    {
      id: 3,
      nombre: "Carlos López",
      telefono: "+1122334455",
      email: "carlos@example.com",
      preferencias: {
        tipo: "Apartamento",
        operacion: "Venta",
        ubicacion: "Centro",
        precioMin: 2000,
        precioMax: 3000,
        habitaciones: 2,
        banos: 2
      }
    }
  ];
  
  export const getMatchingClients = (inmuebleData) => {
    return new Promise((resolve) => {
      // Simular tiempo de respuesta de API
      setTimeout(() => {
        const matchingClients = clients.filter(client => {
          // Coincidencia en tipo de propiedad
          const tipoMatch = client.preferencias.tipo === inmuebleData.tipos;
          
          // Coincidencia en operación
          const operacionMatch = client.preferencias.operacion === inmuebleData.operaciones;
          
          // Coincidencia aproximada en ubicación (contiene)
          const ubicacionMatch = inmuebleData.ubicaciones.toLowerCase().includes(
            client.preferencias.ubicacion.toLowerCase()
          );
          
          // Coincidencia en rango de precio
          const precioMatch = inmuebleData.precio >= client.preferencias.precioMin && 
                            inmuebleData.precio <= client.preferencias.precioMax;
          
          // Coincidencia en características 
          const habitacionesMatch = !inmuebleData.habitaciones || 
                                  client.preferencias.habitaciones <= inmuebleData.habitaciones;
          
          const banosMatch = !inmuebleData.banos || 
                            client.preferencias.banos <= inmuebleData.banos;
          
          return tipoMatch && operacionMatch && ubicacionMatch && precioMatch && 
                 habitacionesMatch && banosMatch;
        });
        
        resolve(matchingClients);
      }, 500); // medio segundo de delay
    });
  };