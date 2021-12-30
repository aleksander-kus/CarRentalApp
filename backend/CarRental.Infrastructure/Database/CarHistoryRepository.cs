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

        public async Task<List<CarHistoryEntry>> GetActiveHistoryEntriesAsync()
        {
            return await _context.CarHistory
                .Where(entry => entry.StartDate <= DateTime.Now && entry.EndDate >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<CarHistoryEntry>> GetActiveHistoryEntriesOfUserAsync(string userId)
        {
            return await _context.CarHistory
                .Where(entry => entry.UserId.Equals(userId) && entry.EndDate >= DateTime.Now)
                .ToListAsync();
        }

        public async Task AddHistoryEntryAsync(CarHistoryEntry entry)
        {
            await _context.CarHistory.AddAsync(entry);
            await _context.SaveChangesAsync();
        }
    }
}