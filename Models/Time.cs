using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Models
{
    public class Time
    {
        [Key]
        [Required]
        public int TimeId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Available { set; get; }

    }
}
