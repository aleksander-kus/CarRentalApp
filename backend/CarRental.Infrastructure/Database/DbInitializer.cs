using System.Linq;
using CarRental.Domain.Entity;

namespace CarRental.Infrastructure.Database
{
    public class DbInitializer
    {
        public static void Initialize(CarRentalContext context)
        {
            context.Database.EnsureCreated();

            if (context.Cars.Any())
            {
                return; // DB has been seeded
            }

            var cars = new Car[]
            {
                new Car
                {
                    Brand = "Opel",
                    Model = "Insignia",
                    Capacity = 5,
                    Category = "Big",
                    ProductionYear = 2020
                },
                new Car
                {
                    Brand = "Opel",
                    Model = "Astra",
                    Capacity = 4,
                    Category = "Medium",
                    ProductionYear = 2020
                },
                new Car
                {
                    Brand = "Honda",
                    Model = "Civic",
                    Capacity = 3,
                    Category = "Small",
                    ProductionYear = 2017
                },
                new Car
                {
                    Brand = "Honda",
                    Model = "Escapado",
                    Capacity = 7,
                    Category = "XXL",
                    ProductionYear = 2015
                },
                new Car
                {
                    Brand = "Opel",
                    Model = "Insignia",
                    Capacity = 5,
                    Category = "Big",
                    ProductionYear = 2020
                },
                new Car
                {
                    Brand = "Seat",
                    Model = "Ibiza",
                    Capacity = 6,
                    Category = "Big",
                    ProductionYear = 2021
                },
            };
            context.Cars.AddRange(cars);
            context.SaveChanges();
        }
    }
}