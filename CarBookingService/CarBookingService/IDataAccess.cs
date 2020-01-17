using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarBookingService.Controllers
{
    public interface IDataAccess : IDisposable
    {
        public IEnumerable<Car> GetCars();

        public Task<Car> BookCar(int carId, DateTime day);

        public IEnumerable<Car> GetCars(DateTime day);
    }
}