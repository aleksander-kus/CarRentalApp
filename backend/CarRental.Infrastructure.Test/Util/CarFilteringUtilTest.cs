using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Infrastructure.Util;
using Xunit;

namespace CarRental.Infrastructure.Test.Util
{
    public class CarFilteringUtilTest
    {
        private readonly CarDetails[] _cars =
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
        
        [Theory]
        [MemberData(nameof(TestFilters))]
        public async Task ShouldReturnAllCarsMatchingFilterAsync(CarListFilter filter, int[] expectedCarIds)
        {
            var result = CarFilteringUtil.FilterCars(_cars.ToList(), filter);
            var actualCarIds = result.Select(car => car.Id).ToArray();
            Array.Sort(actualCarIds);
            Assert.Equal(expectedCarIds, actualCarIds);
        }

        public static IEnumerable<object[]> TestFilters =>
            new List<object[]>
            {
                new object[] {new CarListFilter(), new[] {0, 1, 2, 3, 4}},
                new object[] {new CarListFilter {Brands = new[] {"Volkswagen"}}, new[] {0, 1, 4}},
                new object[] {new CarListFilter {CapacityMin = 6}, new[] {0, 1, 4}},
                new object[] {new CarListFilter {CapacityMax = 5}, new[] {2, 3}},
                new object[] {new CarListFilter {Models = new[] {"Passat", "Transporter", "Panamera"}, CapacityMax = 5}, new[] {3}},
                new object[]
                {
                    new CarListFilter {Brands = new[] {"Volkswagen", "Ford"}, ProductionYearMin = 2010},
                    new[] {0, 2, 4}
                },
                new object[] {new CarListFilter {ProductionYearMin = 2100}, Array.Empty<int>()},
                new object[] {new CarListFilter {CapacityMin = 11}, Array.Empty<int>()},
                new object[]
                {
                    new CarListFilter {Brands = new string[] {}, Models = new string[] {}},
                    new[] {0, 1, 2, 3, 4}
                },
                new object[]{new CarListFilter {Categories = new []{"Big", "Small"}, HorsePowerMin = 200}, new[] {3}}
            };
    }
}