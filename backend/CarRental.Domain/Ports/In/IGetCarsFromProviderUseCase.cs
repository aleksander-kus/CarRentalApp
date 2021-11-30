using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IGetCarsFromProviderUseCase
    {
        Task<List<CarDetails>> GetCarsAsync(string providerId, CarListFilter filters);
    }
}