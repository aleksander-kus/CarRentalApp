using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CurrentlyRentedCarsController : Controller
    {
        private readonly IGetCurrentlyRentedCarsUseCase _getCurrentlyRentedCarsUse;

        public CurrentlyRentedCarsController(IGetCurrentlyRentedCarsUseCase getCurrentlyRentedCarsUse)
        {
            _getCurrentlyRentedCarsUse = getCurrentlyRentedCarsUse;
        }

        [HttpGet("currentlyRented")]
        public async Task<ActionResult<List<CarHistoryEntry>>> GetCurrentlyRented()
        {
            return Ok(await _getCurrentlyRentedCarsUse.GetCurrentlyRentedCarsAsync());
        }
    }
}