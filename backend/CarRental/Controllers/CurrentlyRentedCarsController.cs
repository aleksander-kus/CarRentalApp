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
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<List<CarHistory>>> GetCurrentlyRented()
        {
            return Ok(await _getCurrentlyRentedCarsUse.GetCurrentlyRentedCarsAsync());
        }
        
        [HttpGet("currentlyRentedByUser")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<List<CarHistory>>> GetCurrentlyRentedByUser()
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _getCurrentlyRentedCarsUse.GetCurrentlyRentedCarsOfUserAsync(userId));
        }
    }
}