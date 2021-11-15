using CarRental.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Database
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options)
        {
        }

        public CarRentalContext()
        {
            
        }
        
        public virtual DbSet<Car> Cars { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().ToTable("Car");
        }
    }
}