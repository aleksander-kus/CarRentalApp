using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IBookCarUseCase
    {
        Task<bool> TryBookCar(CarRentRequestDto carRentRequest);
    }
}