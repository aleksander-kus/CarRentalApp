using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IRegisterCarRentUseCase
    {
        Task RegisterCarRentAsync(string userId, int providerCarId, string providerId, CarRentRequest carRentRequest);
    }
}