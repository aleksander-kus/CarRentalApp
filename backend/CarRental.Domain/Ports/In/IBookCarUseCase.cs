using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IBookCarUseCase
    {
        Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, string providerId, CarRentRequest carRentRequest);
    }
}