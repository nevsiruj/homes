using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmenidadInmueble : BaseEntity
    {
        
        public int AmenidadInmuebleId { get; set; }
        public string Nombre { get; set; }        
        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }
    }
}
