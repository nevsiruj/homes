using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateClienteDTO
    {
        public int Id { get; set; }
        public string AgenteId { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Dni { get; set; }
        public string? Email { get; set; }
        public string Telefono { get; set; }

        // Datos adicionales
        public string? Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaRegistro { get; set; } = DateTime.Now;
        public string? Notas { get; set; }
        public string? Origen { get; set; }
        public EstadoCliente? Status { get; set; } = EstadoCliente.contacto_inicial_pendiente;


        // Relacion
        //public CreatePreferenciaDTO? Preferencias { get; set; }
        public List<CreatePreferenciaDTO> Preferencias { get; set; }

    }
}
