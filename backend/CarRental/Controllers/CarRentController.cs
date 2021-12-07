using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
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
        public async Task<ActionResult<ApiResponse<CarRentResponse>>> BookCar([FromBody] CarRentRequest carRentRequest,
            string providerId, string carId)
        {
            try
            {
                var response = await _bookCarUseCase.TryBookCar(carId, providerId, carRentRequest);
                return response.Data != null ? Ok(response) : BadRequest(response);
            }
            catch (UnknownCarProviderException ex)
            {
                return NotFound(new ApiResponse<CarPrice>() {Error = "Unknown car provider"});
            }
        }
    }
}