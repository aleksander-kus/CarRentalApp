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
                .Include(entry => entry.Car).ToListAsync();
        }

        public async Task<List<CarHistoryEntry>> GetActiveHistoryEntriesOfUserAsync(string userId)
        {
            return await _context.CarHistory
                .Where(entry => entry.UserId.Equals(userId) && entry.StartDate <= DateTime.Now &&
                                entry.EndDate >= DateTime.Now)
                .Include(entry => entry.Car).ToListAsync();
        }

        public async Task<Car> GetCarByProviderDataAsync(int providerCarId, string providerId)
        {
            return await _context.Cars.FirstOrDefaultAsync(car =>
                car.ProviderCarId == providerCarId && car.ProviderId.Equals(providerId));
        }

        public async Task AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task AddHistoryEntryAsync(CarHistoryEntry entry)
        {
            await _context.CarHistory.AddAsync(entry);
            await _context.SaveChangesAsync();
        }
    }
}