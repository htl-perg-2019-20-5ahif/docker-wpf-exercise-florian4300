using CarBookingService.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CarBookingService.Exceptions;

namespace CarBookingService
{
    public class DataAccess : IDataAccess
    {
        public CarBookingContext context;

        public DataAccess(CarBookingContext _context)
        {
            context = _context;
        }

        public async Task<Car> BookCar(int carId, DateTime bookingdate)
        {
            Car c = context.Cars.Include(car => car.BookingDates).Where(c => c.CarId == carId).FirstOrDefault();
            if(c == null)
            {
                throw new CarDoesNotExistException();
            }
            if(c.BookingDates == null)
            {
                c.BookingDates = new List<BookingDate>();
            }
            if (ConvertToDateTimeList(c.BookingDates).Contains(bookingdate.Date))
            {
                throw new AlreadyBookedException();
            }
            if(DateTime.Compare(bookingdate.Date,DateTime.Now.Date) < 0)
            {
                throw new InvalidDateException();
            }
            BookingDate bk = new BookingDate();
            bk.BookDate = bookingdate;
            c.BookingDates.Add(bk);
            context.Cars.Update(c); 
            await context.SaveChangesAsync();
            return c;
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }

        public IEnumerable<Car> GetCars()
        {
            return context.Cars.Include(car => car.BookingDates);
        }

        public IEnumerable<Car> GetCars(DateTime date)
        {
            List<Car> cars = context.Cars.Include(car => car.BookingDates).ToList();
            List<Car> filteredCars = new List<Car>();
            foreach(Car c in cars)
            {
                List<DateTime> cardates = ConvertToDateTimeList(c.BookingDates);
                if (!cardates.Contains(date.Date))
                {
                    filteredCars.Add(c);
                }
            }
            return filteredCars;
        }

        private List<DateTime> ConvertToDateTimeList(List<BookingDate> bookingDates)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            
            foreach(BookingDate bd in bookingDates)
            {
                dateTimes.Add(bd.BookDate.Date);
            }
            return dateTimes;
        }
    }
}
