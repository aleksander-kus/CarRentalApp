using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.Out
{
    public interface ICarProvider
    {
        Task<List<CarDetails>> GetCarsAsync(CarListFilter filter);

        Task<bool> TryBookCar(CarRentRequestDto carRentRequest);
    }
}