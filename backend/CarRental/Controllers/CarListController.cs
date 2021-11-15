using System.Threading.Tasks;
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
        //public async Task<ActionResult> GetCarList([FromBody] CarListFiler filter)
        public async Task<ActionResult> GetCarList([FromQuery] CarListFilter filter)
        {
            filter.Validate();
            return Ok(await _carRepository.GetCarsAsync(filter));
        }
    }
}