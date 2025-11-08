using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Preferencias : BaseEntity
    {

        // Requerimientos de propiedad
        public string? Tipo { get; set; }  // Casa/Apartamento/Terreno
        public string? Operacion { get; set; }  // Venta/Alquiler
        public string? Ubicacion { get; set; }

        // Rangos
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }

        // Caractersticas
        public int? Habitaciones { get; set; }       
        public int? Banos { get; set; }        
        public int? MetrosCuadrados { get; set; }
        //public List<string> Amenidades { get; set; } = new List<string>();
        public string? Comentarios { get; set; }

        public List<PreferenciaAmenidad> PreferenciaAmenidades { get; set; } = new List<PreferenciaAmenidad>();
        // foranea
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }


    
}
