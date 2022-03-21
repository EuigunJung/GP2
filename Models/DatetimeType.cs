using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Models
{
    public class DatetimeType
    {
        [Key]
        [Required]
        public int DatetimeTypeId { get; set; }
        public DateTime DatetimeTypeName { get; set; }
    }
}
