using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class RentalHistoryController : Controller
    {
        private readonly IGetRentalHistoryUseCase _rentalHistoryUseCase;

        public RentalHistoryController(IGetRentalHistoryUseCase rentalHistoryUseCase)
        {
            _rentalHistoryUseCase = rentalHistoryUseCase;
        }

        [HttpGet("rentalHistory")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<List<CarHistory>>> GetRentalHistory()
        {
            return Ok(await _rentalHistoryUseCase.GetRentalHistoryAsync());
        }
        
        [HttpGet("rentalHistoryByUser")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<List<CarHistory>>> GetRentalHistoryByUser()
        {
            var userId = HttpContext.User.GetUserId();
            return Ok(await _rentalHistoryUseCase.GetRentalHistoryByUserAsync(userId));
        }
    }
}