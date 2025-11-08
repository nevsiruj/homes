
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string? Dni { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }

        public string? Email { get; set; }
        public string UserName { get; set; }
        public string? Telefono { get; set; }
        public string? Rol { get; set; }

        public string? Especialidad { get; set; }

        public int? PropiedadesVendidas { get; set; }
        public decimal? ValorVendidoTotal { get; set; }
        public string? Experiencia { get; set; }
        public DateTime FechaIngreso { get; set; }

        public string? Notas { get; set; }
    }

}
