using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inmueble : BaseEntity
    {
        public string? CodigoPropiedad { get; set; }
        public string? Titulo { get; set; }
        public string? SlugInmueble { get; set; }
        public string? Contenido { get; set; }

        public string? ImagenPrincipal { get; set; }
        public List<ImagenInmueble>? ImagenesReferencia { get; set; }

        public string? Tipos { get; set; }
        public string? Operaciones { get; set; }
        public string? Ubicaciones { get; set; }

        public List<AmenidadInmueble>? Amenidades { get; set; }

        public bool Luxury { get; set; }
        public decimal Precio { get; set; }
        public bool? PrecioActivo { get; set; } = true;
        public int Parqueos { get; set; }
        public int Habitaciones { get; set; }
        public int Banos { get; set; }
        public int Niveles { get; set; }
        public decimal MetrosCuadrados { get; set; }
        public string? Video { get; set; }
       
    }
}



    

