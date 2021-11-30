using System.Linq;

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