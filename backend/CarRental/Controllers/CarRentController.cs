using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Enum;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("/api/cars")]
    [ApiController]
    public class CarRentController: Controller
    {
        private readonly IBookCarUseCase _bookCarUseCase;

        public CarRentController(IBookCarUseCase bookCarUseCase)
        {
            _bookCarUseCase = bookCarUseCase;
        }

        [HttpPost("{providerId}/{carId}/rent")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<ApiResponse<CarRentResponse>>> BookCar([FromBody] CarRentRequest carRentRequest,
            string providerId, string carId)
        {
            var userId = HttpContext.User.GetUserId();
            try
            {
                var response = await _bookCarUseCase.TryBookCar(carId, providerId, userId, carRentRequest);
                if(response.Data == null) return BadRequest(response);
                return Ok(response);
            }
            catch (UnknownCarProviderException)
            {
                return NotFound(new ApiResponse<CarRentResponse>() {Error = "Unknown car provider"});
            }
        }
    }
}