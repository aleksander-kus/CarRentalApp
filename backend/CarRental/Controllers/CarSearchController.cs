using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;

namespace CarRental.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarSearchController : Controller
    {
        private readonly IGetCarProvidersUseCase _getCarProvidersUseCase;
        private readonly IGetCarsFromProviderUseCase _getCarsFromProviderUseCase;

        public CarSearchController(IGetCarsFromProviderUseCase getCarsFromProviderUseCase, IGetCarProvidersUseCase getCarProvidersUseCase)
        {
            _getCarsFromProviderUseCase = getCarsFromProviderUseCase;
            _getCarProvidersUseCase = getCarProvidersUseCase;
        }

        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        [HttpGet("providers")]
        public async Task<ActionResult<List<CarProvider>>> GetCarProviders()
        {
            var result = await _getCarProvidersUseCase.GetCarProvidersAsync();
            
            return Ok(result);
        }
        
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "*" })]
        [HttpGet("{providerId}")]
        public async Task<ActionResult<ApiResponse<List<CarDetails>>>> GetCars([FromQuery] CarListFilter filter, [FromRoute] string providerId)
        {
            filter.Validate();
            
            try
            {
                var response = await _getCarsFromProviderUseCase.GetCarsAsync(providerId, filter);
                return response.Data != null ? Ok(response) : BadRequest(response);
            }
            catch (UnknownCarProviderException ex)
            {
                return NotFound(new ApiResponse<CarPrice>() {Error = "Unknown car provider"});
            }
        }
    }
}