using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserCreateDTO
    {

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DireccionDomicilio { get; set; }
       

        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
  
        public string Whatsapp { get; set; }
        public int Identificador { get; set; }


        public DateTime FechaIngreso { get; set; }
        public string Rol { get; set; }
    }
}
