using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<List<CarDetails>>> GetCars([FromQuery] CarListFilter filter, [FromRoute] string providerId)
        {
            filter.Validate();

            var result = await _getCarsFromProviderUseCase.GetCarsAsync(providerId, filter);

            return Ok(result);
        }
    }
}