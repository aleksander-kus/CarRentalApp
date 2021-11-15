using System.Threading.Tasks;
using CarRental.Domain.CarList;
using CarRental.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Database
{
    public class CarRepository : ICarRepository
    {
        private CarRentalContext context;

        public CarRepository(CarRentalContext carRentalContext)
        {
            context = carRentalContext;
        }

        public async Task<Car[]> GetAllCars()
        {
            return await context.Cars.ToArrayAsync();
        }
    }
}