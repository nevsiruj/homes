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
    public class MatchSugeridoDto
    {
        public int PropiedadId { get; set; }
        public string CodigoPropiedad { get; set; }
        public string Titulo { get; set; }
        public string Slug { get; set; }
        public string Ubicacion { get; set; }
        public decimal Precio { get; set; }
        public int? Habitaciones { get; set; }
        public int? Banos { get; set; }
        public decimal PuntuacionMatch { get; set; }
        public List<string> Coincidencias { get; set; }
        public List<string> Amenidades { get; set; } 
        public List<string> AmenidadesCoincidentes { get; set; }
        public bool YaEnviado { get; set; }
        public bool EsPendiente { get; set; }
        public int? MatchId { get; set; }
    }
}
