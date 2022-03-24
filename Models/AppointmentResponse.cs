using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Models
{
    public class AppointmentResponse
    {
        [Key]
        [Required]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = " Please enter a valid group name.")]
        public string Name { get; set; }

        [Required]
        [Range(1, 15, ErrorMessage = "Do not enter more than 15")]
        public int Size { get; set; }

        [Required(ErrorMessage = " Please enter a valid email address.")]
        public string Email { get; set; }
        
      

        public string PhoneNumber { get; set; }
   
        //[Required]
        public string Date { get; set; }
        //[Required]
        public string Time { get; set; }

    }
}
