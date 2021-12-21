using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;

namespace CarRental.Domain.Ports.In
{
    public interface IGetCurrentlyRentedCarsUseCase
    {
        Task<List<CarHistoryEntry>> GetCurrentlyRentedCarsOfUserAsync(string userId);
        Task<List<CarHistoryEntry>> GetCurrentlyRentedCarsAsync();
    }
}