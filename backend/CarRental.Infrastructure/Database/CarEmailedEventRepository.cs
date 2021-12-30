using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Database
{
    public class CarEmailedEventRepository: ICarEmailedEventRepository
    {
        private readonly CarRentalContext _context;

        public CarEmailedEventRepository(CarRentalContext context)
        {
            _context = context;
        }

        public async Task AddEmailedAsync(CarEmailedEvent emailedEvent)
        {
            await _context.AddAsync(emailedEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CarEmailedEvent>> GetAllAsync()
        {
            return await _context.CarEmailedEvents.ToListAsync();
        }
    }
}