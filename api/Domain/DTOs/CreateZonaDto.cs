using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateZonaDto
    {
        public string Nombre { get; set; }
        public bool? Activo { get; set; }     
        public int? Orden { get; set; }
    }
}
