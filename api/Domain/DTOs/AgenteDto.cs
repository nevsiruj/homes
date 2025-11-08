using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AgenteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
      
        public DateTime FechaRegistro { get; set; }

        // Campos opcionales 
        public string? Experiencia { get; set; }
        public int? PropiedadesVendidas { get; set; }
        public string? ValorVendido { get; set; }
        public List<CertificacionAgenteDTO>? Certificaciones { get; set; }
        public List<ClienteAtendidoDTO>? ClientesAtendidos { get; set; }
        public string? Notas { get; set; }
    }

    public class CertificacionAgenteDTO
    {
        public string Nombre { get; set; } 
                                           
    }

    public class ClienteAtendidoDTO
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; } // "Comprador", "Vendedor"
                                         
    }


}
