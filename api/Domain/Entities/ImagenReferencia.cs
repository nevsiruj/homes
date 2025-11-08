using Domain.Entities;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ImagenReferencia
    {
        public int Id { get; set; } 
        public string Url { get; set; }
        public int InmuebleId { get; set; }
        public Inmueble? Inmueble { get; set; }
    }
}
