using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EminenciasApi.Domain.Entities.Common;

namespace Domain.Entities
{
    public class Zona : BaseEntity
    {
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public int Orden { get; set; } = 0;
    }
}
