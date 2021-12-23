using System;
using CarRental.Domain.Entity;
using CarRental.Infrastructure.Database;

namespace CarRental.IntegrationTest
{
    public static class SeedData
    {
        public static void PopulateTestData(CarRentalContext dbContext)
        {
            dbContext.CarHistory.Add(new CarHistoryEntry()
            {
                CarBrand = "Toyota",
                CarModel = "Supra",
                CarProvider = "DNZ",
                EndDate = DateTime.Now.AddDays(10),
                IsRentConfirmed = true,
                PriceId = "xxx",
                RentId = "ddd",
                StartDate = DateTime.Now,
                UserId = "xxx-xxx"
            });
            dbContext.SaveChanges();
        }
    }
}