using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IReturnCarUseCase
    {
        Task<ApiResponse<CarReturnResponse>> TryReturnCar(string carId, string providerId, CarReturnRequest carReturnRequest);
    }
}