using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Database
{
    public class CarReturnEntryRepository: ICarReturnEntryRepository
    {
        private readonly CarRentalContext _context;

        public CarReturnEntryRepository(CarRentalContext context)
        {
            _context = context;
        }
        
        public async Task AddReturnEntryAsync(CarReturnEntry carReturnEntry)
        {
            await _context.CarReturnEntries.AddAsync(carReturnEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<CarReturnEntry> GetReturnEntryAsync(int historyEntryId)
        {
            return await _context.CarReturnEntries.Where(entry => entry.HistoryEntryId == historyEntryId)
                .FirstOrDefaultAsync();
        }
    }
}