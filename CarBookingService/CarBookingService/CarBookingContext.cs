using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingService
{
    public class CarBookingContext : DbContext
    {
        public CarBookingContext(DbContextOptions<CarBookingContext> options)
            : base(options)
        { }
        public DbSet<Car> Cars { get; set; }
    }
}
