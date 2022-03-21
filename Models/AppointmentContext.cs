using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Models
{
    public class AppointmentContext : DbContext
    {
        //Constructor
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Leave blank for now
        }

        public DbSet<ApplicationResponse> Response { get; set; }

        public DbSet<DatetimeType> Appointment { get; set; }
    }
}
