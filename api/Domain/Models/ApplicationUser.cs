using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astuc.Domain.DTOs
{
    public class ApplicationUser : IdentityUser
    {
        public string? Dni { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }

        // Especialización
        public string? Especialidad { get; set; }
        public string Rol { get; set; }

        // Relaciones 
        public List<CertificacionAgente>? Certificaciones { get; set; }
        public string? Experiencia { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string? Notas { get; set; }
        public List<Cliente>? Clientes { get; set; }


    }
}
