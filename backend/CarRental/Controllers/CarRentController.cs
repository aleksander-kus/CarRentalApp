using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CarRentController: Controller
    {
        private readonly IBookCarUseCase _bookCarUseCase;

        public CarRentController(IBookCarUseCase bookCarUseCase)
        {
            _bookCarUseCase = bookCarUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<string>> BookCar([FromBody] CarRentRequestDto carRentRequest)
        {
            if (await _bookCarUseCase.TryBookCar(carRentRequest))
            {
                return Ok("Car booked");
            }
            
            return NotFound("Car not available");
        }
    }
}