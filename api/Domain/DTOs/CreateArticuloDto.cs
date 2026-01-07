using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateArticuloDto
    {
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public string Slug { get; set; }
        public string ImagenUrl { get; set; }
        public string Categoria { get; set; }
        public bool? Activo { get; set; }
        public int? Orden { get; set; }
    }
}