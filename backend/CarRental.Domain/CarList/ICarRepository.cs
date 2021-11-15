using System.Threading.Tasks;
using CarRental.Domain.Entity;

namespace CarRental.Domain.CarList
{
    public interface ICarRepository
    {
        Task<Car[]> GetCarsAsync(CarListFilter filter);
    }
}