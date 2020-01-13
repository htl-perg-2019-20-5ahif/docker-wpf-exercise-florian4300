using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingService
{
    public class Car
    {
        public int CarId { get; set; }
        public string CarName { get; set; }

        public int PS { get; set; }

        public int Range{ get; set; }

        public int MaxSpeed { get; set; }

        public List<BookingDate>BookingDates { get; set; }
    }
    public class BookingDate
    {
        public int BookingDateId { get; set; }

        public DateTime BookDate { get; set; }
    }
}
