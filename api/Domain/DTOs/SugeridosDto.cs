using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class Sugeridos
    {
        public decimal Igual { get; set; }
        public decimal Bajo { get; set; }
        public decimal Alto { get; set; }
    }
}


