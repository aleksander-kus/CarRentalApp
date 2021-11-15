using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Domain.CarList;
using CarRental.Domain.Entity;
using CarRental.Infrastructure.Database;
using CarRental.Infrastructure.Test.Async;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarRental.Infrastructure.Test
{
    public class CarRepositoryTest
    {
        private readonly ICarRepository _carRepository;

        private readonly Car[] _cars =
        {
            new()
            {
                Id = 0, Brand = "Volkswagen", Model = "Passat", Capacity = 6, Category = "Big", ProductionYear = 2010, HorsePower = 150
            },
            new()
            {
                Id = 1, Brand = "Volkswagen", Model = "Transporter", Capacity = 10, Category = "XXL",
                ProductionYear = 2005, HorsePower = 200
            },
            new()
            {
                Id = 2, Brand = "Ford", Model = "Mondeo", Capacity = 3, Category = "Small", ProductionYear = 2018, HorsePower = 100
            },
            new()
            {
                Id = 3, Brand = "Porsche", Model = "Panamera", Capacity = 2, Category = "Small", ProductionYear = 2020, HorsePower = 300
            },
            new()
            {
                Id = 4, Brand = "Volkswagen", Model = "Passat", Capacity = 6, Category = "Big", ProductionYear = 2010, HorsePower = 150
            }
        };

        public CarRepositoryTest()
        {
            var data = _cars.AsQueryable();
            var mockSet = new Mock<DbSet<Car>>();
            mockSet.As<IAsyncEnumerable<Car>>().Setup(d => d.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new AsyncEnumerator<Car>(data.GetEnumerator()));
            mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(new AsyncQueryProvider<Car>(data.Provider));
            mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<CarRentalContext>();
            mockContext.Setup(c => c.Cars).Returns(mockSet.Object);
            
            _carRepository = new CarRepository(mockContext.Object);
        }
        
        [Theory]
        [MemberData(nameof(TestFilters))]
        public async Task ShouldReturnAllCarsMatchingFilterAsync(CarListFilter filter, int[] expectedCarIds)
        {
            var result = await _carRepository.GetCarsAsync(filter);
            var actualCarIds = result.Select(car => car.Id).ToArray();
            Array.Sort(actualCarIds);
            Assert.Equal(expectedCarIds, actualCarIds);
        }

        public static IEnumerable<object[]> TestFilters =>
            new List<object[]>
            {
                new object[] {new CarListFilter(), new[] {0, 1, 2, 3, 4}},
                new object[] {new CarListFilter {ExcludedBrands = new[] {"Ford", "Porsche"}}, new[] {0, 1, 4}},
                new object[] {new CarListFilter {CapacityMin = 6}, new[] {0, 1, 4}},
                new object[] {new CarListFilter {CapacityMax = 5}, new[] {2, 3}},
                new object[] {new CarListFilter {ExcludedModels = new[] {"Mondeo"}, CapacityMax = 5}, new[] {3}},
                new object[]
                {
                    new CarListFilter {ExcludedBrands = new[] {"Porsche"}, ProductionYearMin = 2010},
                    new[] {0, 2, 4}
                },
                new object[] {new CarListFilter {ProductionYearMin = 2100}, Array.Empty<int>()},
                new object[] {new CarListFilter {CapacityMin = 11}, Array.Empty<int>()},
                new object[]
                {
                    new CarListFilter {ExcludedBrands = new[] {"NotABrand"}, ExcludedModels = new[] {"NotAModel"}},
                    new[] {0, 1, 2, 3, 4}
                },
                new object[]{new CarListFilter {ExcludedCategories = new []{"XXL"}, HorsePowerMin = 200}, new[] {3}}
            };
    }
}