using Astuc.Domain.DTOs;
using Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CreatePreferenciaDTO
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public string? Tipo { get; set; }
        public string? Operacion { get; set; }
        public string? Ubicacion { get; set; }

        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public int? Habitaciones { get; set; }
        public int? Banos { get; set; }
        public int? MetrosCuadrados { get; set; }
        public string? Comentarios { get; set; }


        public List<CreatePreferenciaAmenidadDTO>? PreferenciaAmenidades { get; set; } 
        //public int ClienteId { get; set; }
       
    }


}
