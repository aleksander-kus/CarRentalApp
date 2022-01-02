using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IGetRentalHistoryUseCase
    {
        Task<List<CarHistory>> GetRentalHistoryByUserAsync(string userId);
        Task<List<CarHistory>> GetRentalHistoryAsync();
    }
}