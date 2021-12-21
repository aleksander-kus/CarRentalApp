using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CurrentlyRentedCarsByUserController : Controller
    {
        private readonly IGetCurrentlyRentedCarsUseCase _getCurrentlyRentedCarsUse;

        public CurrentlyRentedCarsByUserController(IGetCurrentlyRentedCarsUseCase getCurrentlyRentedCarsUse)
        {
            _getCurrentlyRentedCarsUse = getCurrentlyRentedCarsUse;
        }

        [HttpGet("currentlyRentedByUser")]
        public async Task<ActionResult<List<CarHistoryEntry>>> GetCurrentlyRented()
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _getCurrentlyRentedCarsUse.GetCurrentlyRentedCarsOfUserAsync(userId));
        }
    }
}