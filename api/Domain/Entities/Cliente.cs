using Astuc.Domain.DTOs;
using Domain.Enums;
using EminenciasApi.Domain.Entities.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cliente : BaseEntity
    {
        // Datos basicos
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Dni { get; set; }
        public string? Email { get; set; }
        public string Telefono { get; set; }

        // Datos adicionales
        public string? Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string? Notas { get; set; }
        public string? Origen { get; set; }
        public EstadoCliente Status { get; set; } = EstadoCliente.contacto_inicial_pendiente;

        // Relacion
        //public Preferencias Preferencias { get; set; }
        public ICollection<Preferencias> Preferencias { get; set; } = new List<Preferencias>();
        public ICollection<Interaccion> Interacciones { get; set; } = new List<Interaccion>();
        public string AgenteId { get; set; }
        public ApplicationUser? Agente { get; set; }

        // Relación con Match (uno a muchos)
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}