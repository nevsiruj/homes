using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CertificacionAgente :BaseEntity
    {
        public string? Nombre { get; set; }
        public int AgenteId { get; set; } 
        public ApplicationUser? Agente { get; set; }
    }
}
