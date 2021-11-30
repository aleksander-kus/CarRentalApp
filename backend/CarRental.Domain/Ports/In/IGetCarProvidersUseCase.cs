using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IGetCarProvidersUseCase
    {
        Task<List<CarProvider>> GetCarProvidersAsync();
    }
}