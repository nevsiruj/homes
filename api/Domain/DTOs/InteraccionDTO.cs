using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InteraccionDTO 
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public DateTime? FechaVencimiento { get; set; }
        public string Tipo { get; set; }  // Llamada/Email/Visita
        public string Descripcion { get; set; }
        public string? StatusInteraccion { get; set; }

        public string AgenteId { get; set; }
        //public ApplicationUser Agente { get; set; }

        // Relación con Cliente
        public int ClienteId { get; set; }
        //public Cliente Cliente { get; set; }



    }
}
