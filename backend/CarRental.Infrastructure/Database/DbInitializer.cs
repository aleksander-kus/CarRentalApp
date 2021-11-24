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
                    ProductionYear = 2020,
                    HorsePower = 190,
                    ProviderCompany = "ExtraCompany"
                },
                new Car
                {
                    Brand = "Opel",
                    Model = "Astra",
                    Capacity = 4,
                    Category = "Medium",
                    ProductionYear = 2020,
                    HorsePower = 90,
                    ProviderCompany = "ExtraCompany"
                },
                new Car
                {
                    Brand = "Honda",
                    Model = "Civic",
                    Capacity = 3,
                    Category = "Small",
                    ProductionYear = 2017,
                    HorsePower = 120,
                    ProviderCompany = "SuperCompany"
                },
                new Car
                {
                    Brand = "Honda",
                    Model = "Escapado",
                    Capacity = 7,
                    Category = "XXL",
                    ProductionYear = 2015,
                    HorsePower = 160,
                    ProviderCompany = "ExtraCompany"
                },
                new Car
                {
                    Brand = "Opel",
                    Model = "Insignia",
                    Capacity = 5,
                    Category = "Big",
                    ProductionYear = 2020,
                    HorsePower = 200,
                    ProviderCompany = "SuperCompany"
                },
                new Car
                {
                    Brand = "Seat",
                    Model = "Ibiza",
                    Capacity = 6,
                    Category = "Big",
                    ProductionYear = 2021,
                    HorsePower = 130,
                    ProviderCompany = "GreatCompany"
                },
            };
            context.Cars.AddRange(cars);
            context.SaveChanges();
        }
    }
}