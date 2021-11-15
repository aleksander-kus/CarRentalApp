using System.Linq;
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

        public async Task<Car[]> GetCarsAsync(CarListFilter filter)
        {
            var query = context.Cars.Where(car => !filter.ExcludedBrands.Contains(car.Brand) &&
                                                   !filter.ExcludedModels.Contains(car.Model) &&
                                                   !filter.ExcludedCategories.Contains(car.Category) &&
                                                   car.Capacity >= filter.CapacityMin &&
                                                   car.Capacity <= filter.CapacityMax &&
                                                   car.ProductionYear >= filter.ProductionYearMin &&
                                                   car.ProductionYear <= filter.ProductionYearMax &&
                                                   car.HorsePower >= filter.HorsePowerMin &&
                                                   car.HorsePower <= filter.HorsePowerMax);
            return await query.ToArrayAsync();
        }
    }
}