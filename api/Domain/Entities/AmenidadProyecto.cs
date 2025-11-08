using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmenidadProyecto : BaseEntity
    {

        public int AmenidadProyectoId { get; set; }
        public string Nombre { get; set; }
        public int ProyectoId { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}
