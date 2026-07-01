using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Model
{
    public class Inventar
    {

        [Key]
        public int Id { get; set; }

        public string Denumire { get; set; }

        public bool? ArePoza { get; set; }

        public int PretVanzare { get; set; }

        public int PretCumparare { get; set; }

        public bool? EsteEchipament { get; set; }


        public int Stoc { get; set; }

    }
}
