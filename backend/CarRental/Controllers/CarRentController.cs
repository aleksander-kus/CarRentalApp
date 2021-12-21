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
        private readonly IRegisterCarRentUseCase _registerCarRentUseCase;
        private readonly INotifyUserAfterCarRent _notifyUserAfterCarRent;
        private readonly IGetUserDetailsUseCase _getUserDetailsUseCase;

        public CarRentController(IBookCarUseCase bookCarUseCase, INotifyUserAfterCarRent notifyUserAfterCarRent, IGetUserDetailsUseCase getUserDetailsUseCase, IRegisterCarRentUseCase registerCarRentUseCase)
        {
            _bookCarUseCase = bookCarUseCase;
            _registerCarRentUseCase = registerCarRentUseCase;
            _notifyUserAfterCarRent = notifyUserAfterCarRent;
            _getUserDetailsUseCase = getUserDetailsUseCase;
        }

        [HttpPost("{providerId}/{carId}/rent")]
        public async Task<ActionResult<ApiResponse<CarRentResponse>>> BookCar([FromBody] CarRentRequest carRentRequest,
            string providerId, string carId)
        {
            var userId = HttpContext.User.GetUserId();
            try
            {
                var response = await _bookCarUseCase.TryBookCar(carId, providerId, carRentRequest);
                if(response.Data == null) return BadRequest(response);
                await _notifyUserAfterCarRent.NotifyUserAfterCarRent(
                    await _getUserDetailsUseCase.GetUserDetails(userId), carRentRequest);
                await _registerCarRentUseCase.RegisterCarRentAsync(userId, int.Parse(carId), providerId, carRentRequest);
                return Ok(response);
            }
            catch (UnknownCarProviderException)
            {
                return NotFound(new ApiResponse<CarPrice>() {Error = "Unknown car provider"});
            }
        }
    }
}