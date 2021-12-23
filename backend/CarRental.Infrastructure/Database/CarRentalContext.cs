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

        public virtual DbSet<CarHistoryEntry> CarHistory { get; set; }

        public virtual DbSet<CarEmailedEvent> CarEmailedEvents { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarHistoryEntry>()
                .HasIndex(p => new { p.IsRentConfirmed });
            modelBuilder.Entity<CarHistoryEntry>()
                .HasIndex(p => new { p.UserId, p.IsRentConfirmed });
            modelBuilder.Entity<CarHistoryEntry>()
                .HasIndex(p => new { p.Returned, p.IsRentConfirmed });
        }
    }
}