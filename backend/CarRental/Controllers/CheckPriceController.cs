using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("/api/cars")]
    public class CheckPriceController: Controller
    {
        private readonly ICheckPriceUseCase _checkPriceUseCase;

        public CheckPriceController(ICheckPriceUseCase checkPriceUseCase)
        {
            _checkPriceUseCase = checkPriceUseCase;
        }

        [HttpPost("{providerId}/{carId}/price")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<CarPrice>> CheckPrice([FromBody] CarCheckPrice carCheckPrice, string providerId,
            string carId)
        {
            var userId = HttpContext.User.GetUserId();

            try
            {
                var response = await _checkPriceUseCase.CheckPrice(carCheckPrice, providerId, carId, userId);
                return response.Data != null ? Ok(response) : BadRequest(response);
            }
            catch (UnknownCarProviderException)
            {
                return NotFound(new ApiResponse<CarPrice>() {Error = "Unknown car provider"});
            }
        }
    }
}