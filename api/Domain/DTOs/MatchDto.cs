using Astuc.Domain.DTOs;
using Domain.Entities;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class MatchDto
    {
        public int Id { get; set; } 
        public int ClienteId { get; set; }
        public string CodigoPropiedad { get; set; }
        public int InmuebleId { get; set; }
        public bool EsEnviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
    }
}
