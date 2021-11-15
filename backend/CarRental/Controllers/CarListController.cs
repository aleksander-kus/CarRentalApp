using System.Threading.Tasks;
using CarRental.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CarRental.Domain.CarList;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarListController : Controller
    {
        private ICarRepository _carRepository;
        
        public CarListController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetCarList()
        {
            return Ok(JsonSerializer.Serialize(await _carRepository.GetAllCars()));
        }
    }
}