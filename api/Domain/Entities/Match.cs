using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Match: BaseEntity
    {
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public string CodigoPropiedad { get; set; }
        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }

        // si el match ha sido enviado/interactuado
        public bool EsEnviado { get; set; } = false;

        // fecha en que se marcó como enviado
        public DateTime? FechaEnvio { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
