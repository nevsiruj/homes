using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EminenciasApi.Domain.Entities.Common;

namespace Domain.Entities
{
    public class Articulo : BaseEntity
    {
        
        public string Titulo { get; set; }

        public string Contenido { get; set; }

 
        public string Slug { get; set; }

        public string Permalink { get; set; }

        public string ImagenUrl { get; set; }

        public string Categoria { get; set; }

        public string? Etiqueta { get; set; } = null;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        
        public DateTime? FechaActualizacion { get; set; }


        public bool Activo { get; set; } = true;

        public int Orden { get; set; } = 0;
    }
}