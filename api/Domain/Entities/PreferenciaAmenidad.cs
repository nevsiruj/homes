using Astuc.Domain.DTOs;
using EminenciasApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities
{
    public class PreferenciaAmenidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int PreferenciasId { get; set; }

        public int AmenidadId { get; set; }
       
    }
}