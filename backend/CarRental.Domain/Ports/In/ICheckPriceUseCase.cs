using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface ICheckPriceUseCase
    {
        Task<ApiResponse<CarPrice>> CheckPrice(CarCheckPrice carCheckPrice, string providerId, string carId, string userId);
    }
}