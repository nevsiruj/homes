using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Proyecto : BaseEntity
    {
        public string? CodigoProyecto { get; set; }
        public string? Titulo { get; set; }
        public string? SlugProyecto { get; set; }
        public string? Contenido { get; set; }
        public string? ImagenPrincipal { get; set; }
        public List<ImagenProyecto>? ImagenesReferenciaProyecto { get; set; }
        public string? Tipos { get; set; }
        //public string? Operaciones { get; set; }
        public string? Ubicaciones { get; set; }
        public List<AmenidadProyecto>? Amenidades { get; set; }        
        public decimal Precio { get; set; }       
        public string? Video { get; set; }

        //public int Parqueos { get; set; }
        //public int Habitaciones { get; set; }
        //public int Banos { get; set; }
        //public int Niveles { get; set; }
        public decimal MetrosCuadrados { get; set; }

    }
}





