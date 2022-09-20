using CarDealerAPI.DTOS;
using CarDealerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarDealerAPI.Controllers
{
    [Route("api/dealer/{dealerId}/car")]
    [ApiController]
    [Produces("application/json")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            this._carService = carService;
        }

        [HttpPost]
        public ActionResult CreateCar([FromRoute] int dealerId, [FromBody] CarCreateDTO carNew)
        {
            var carId = _carService.CreateNewCar(dealerId, carNew);

            return Created($"api/dealer/{dealerId}/car/{carId}", null);
        }

        [HttpGet("{carId}")]
        public ActionResult<CarReadDTO> GetCarInDealer(int dealerId, int carId)
        {
            CarReadDTO car = _carService.GetCarById(dealerId, carId);

            return Ok(car);
        }

        [HttpGet]
        //[Authorize(Policy = "DealerMinimum")]
        [Authorize(Policy = "ColorEyes")] // how to test this policy??
        public ActionResult<List<CarReadDTO>> GetAllCars(int dealerId)
        {
            var cars = _carService.GetAllCarForDealer(dealerId);

            return Ok(cars);
        }

        [HttpDelete]
        public ActionResult DeleteAllCars(int dealerId)
        {
            _carService.DeleteAll(dealerId);

            return NoContent();
        }

        //delet one car
    }
}