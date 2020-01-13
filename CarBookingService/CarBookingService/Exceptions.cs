using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBookingService
{
    public class Exceptions
    {
        public class CarDoesNotExistException : Exception { }
        public class AlreadyBookedException : Exception { }
        public class InvalidDateException : Exception { }
    }
}
