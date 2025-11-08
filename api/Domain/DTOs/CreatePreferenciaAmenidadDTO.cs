using Domain.Entities;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreatePreferenciaAmenidadDTO
    {

        public int Id { get; set; }

        public string Nombre { get; set; }
        public int AmenidadId { get; set; }
    }
}
