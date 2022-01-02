using System;
using System.Linq;
using CarRental.Domain.Entity;

namespace CarRental.Infrastructure.Database
{
    public class DbInitializer
    {
        public static void Initialize(CarRentalContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}