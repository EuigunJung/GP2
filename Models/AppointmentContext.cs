using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP2.Models
{
    public class AppointmentContext : DbContext
    {
        //Constructor
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
        {
            //Leave blank for now
        }

        public DbSet<AppointmentResponse> Response { get; set; }

        public DbSet<Time> Slots { get; set; }

        //Seed data as default (we want to see it as a test. The data may be changed afterwards)
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Time>().HasData(
                new Time { 
                    TimeId = 1, Start = new DateTime(2022, 4, 1, 8, 0, 0,  },
                new Time { CategoryId = 2, CategoryName = "School" },
                new Time { CategoryId = 3, CategoryName = "Work" },
                new Time { CategoryId = 4, CategoryName = "Church" }
            );

        }
    }
}
