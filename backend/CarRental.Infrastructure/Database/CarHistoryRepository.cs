using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Database
{
    public class CarHistoryRepository : ICarHistoryRepository
    {
        private readonly CarRentalContext _context;

        public CarHistoryRepository(CarRentalContext context)
        {
            _context = context;
        }

        public async Task<List<CarHistoryEntry>> GetHistoryEntriesAsync()
        {
            return await _context.CarHistory.Where(entry => entry.IsRentConfirmed).ToListAsync();
        }

        public async Task<List<CarHistoryEntry>> GetHistoryEntriesOfUserAsync(string userId)
        {
            return await _context.CarHistory
                .Where(entry => entry.UserId.Equals(userId) && entry.IsRentConfirmed)
                .ToListAsync();
        }
        
        public async Task<List<CarHistoryEntry>> GetActiveHistoryEntriesAsync()
        {
            return await _context.CarHistory
                .Where(entry => entry.Returned == false && entry.IsRentConfirmed)
                .ToListAsync();
        }

        public async Task<List<CarHistoryEntry>> GetActiveHistoryEntriesOfUserAsync(string userId)
        {
            return await _context.CarHistory
                .Where(entry => entry.UserId.Equals(userId) && entry.Returned == false && entry.IsRentConfirmed)
                .ToListAsync();
        }

        public async Task AddHistoryEntryAsync(CarHistoryEntry entry)
        {
            await _context.CarHistory.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task MarkHistoryEntryAsConfirmed(string priceId, string providerId, string rentId, DateTime rentFrom, DateTime renTo)
        {
            var car = await _context.CarHistory
                .Where(c => c.ProviderId == providerId && c.PriceId == priceId)
                .FirstAsync();
            car.IsRentConfirmed = true;
            car.RentId = rentId;
            car.StartDate = rentFrom;
            car.EndDate = renTo;
            await _context.SaveChangesAsync();
        }
    }
}