using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("/api/cars")]
    [ApiController]
    public class CarReturnController: Controller
    {
        private readonly IReturnCarUseCase _returnCarUseCase;

        public CarReturnController(IReturnCarUseCase returnCarUseCase)
        {
            _returnCarUseCase = returnCarUseCase;
        }

        [HttpPost("{providerId}/{carId}/return")]
        //[Authorize(Roles = "Employee")]
        public async Task<ActionResult<ApiResponse<CarReturnResponse>>> ReturnCar([FromBody] CarReturnRequest carReturnRequest, 
            string providerId, string carId)
        {
            try
            {
                var result = await _returnCarUseCase.TryReturnCar(carId, providerId, carReturnRequest);
                if (result.Data == null)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (UnknownCarProviderException)
            {
                return NotFound(new ApiResponse<CarReturnResponse>() {Error = "Unknown car provider"});
            }
        }

    }
}