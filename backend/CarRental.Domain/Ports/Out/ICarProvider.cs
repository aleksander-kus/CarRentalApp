using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.Out
{
    public interface ICarProvider
    {
        Task<ApiResponse<List<CarDetails>>> GetCarsAsync(CarListFilter filter);

        Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, CarRentRequest carRentRequest);

        Task<ApiResponse<CarPrice>> CheckPrice(string carId, int daysCount, UserDetails userDetails);
    }
}