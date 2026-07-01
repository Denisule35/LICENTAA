using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modern.Model
{
    public class Oameni
    {

        [Key]

        public int Id { get; set; }

        public string Name { get; set; }

        public DateOnly Abonament { get; set; }

        public string? Nivel {  get; set; }

        public string? PuncteTari { get; set; }

        public string? PuncteSlabe {  get; set; }

        public bool? ArePoza {  get; set; }

        public int? PumniNimeritiCap { get; set; }

        public int? PumniNimeritiCorp { get; set; }

        public int? PumniRatati { get; set; }

        public int? PumniIncasatiCap { get; set; }

        public int? PumniIncasatiCorp { get; set; }

        public string? Echipament { get; set; }

        public int? ProcentajPrezenta { get; set; }

        public int? NumarVicotorii { get; set; }

        public int? NumarInfrangeri { get; set; }
    
        public int? NumarRemize { get; set; }

    }
}
