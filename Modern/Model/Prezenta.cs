using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Model
{
    public class Prezenta
    {

        [Key]
        public int Id { get; set; }

        public DateOnly Data { get; set; }

        public string Prezenti { get; set; }
        public string Absenti { get; set; }

    }
}
