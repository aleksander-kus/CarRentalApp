using System.Threading.Tasks;

namespace CarRental.Domain.Ports.In
{
    public interface ISendNewCarsEventUseCase
    {
        Task SendNewCars();
    }
}