using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Models
{
    public partial class Appointment
    {
        [Key]
        [Required]
        public long BookId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        [Required]
        [MaxLength(15, ErrorMessage = "Do not enter more than 15")]
        public int Size { get; set; }

        [Required]
        public DateTime DateTime { get; set; } 
    }
}
