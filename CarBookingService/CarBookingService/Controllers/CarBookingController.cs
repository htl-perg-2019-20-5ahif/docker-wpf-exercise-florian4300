using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static CarBookingService.Exceptions;

namespace CarBookingService.Controllers
{
    [ApiController]
    [Route("")]
    public class CarBookingController : ControllerBase
    {
        private readonly IDataAccess dal;

        private readonly ILogger<CarBookingController> _logger;

        public CarBookingController(IDataAccess _dal, ILogger<CarBookingController> logger)
        {
            _logger = logger;
            this.dal = _dal;
        }

        [HttpGet]
        [Route("cars")]
        public ActionResult<IEnumerable<Car>> GetCars()
        {
            return Ok(dal.GetCars());
        }

        [HttpPost("book/{carId}")]
        public async Task<ActionResult<Car>> BookCar(int carId, [FromBody] DateTime date)
        {
            try
            {
                return await dal.BookCar(carId, date);
            }catch(AlreadyBookedException ex)
            {
                return BadRequest();
            } catch(CarDoesNotExistException ex)
            {
                return NotFound();
            } catch(InvalidDateException ex)
            {
                return BadRequest();
            }

        }
        [HttpGet]
        [Route("cars/{date}")]
        public ActionResult<IEnumerable<Car>> GetCars(DateTime date)
        {
            return Ok(dal.GetCars(date));
        }
    }
}
