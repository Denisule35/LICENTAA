using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Model
{
    public class Tranzactie
    {
        [Key]
        public int Id { get; set; }

        public int Suma { get; set; } 

        public string Descriere { get; set; }

        public DateTime Data { get; set; }

        public bool EVenit { get; set; }
    }
}
