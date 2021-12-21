using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;

namespace CarRental.Domain.Ports.Out
{
    public interface ICarHistoryRepository
    {
        Task<List<CarHistoryEntry>> GetActiveHistoryEntriesAsync();
        Task<List<CarHistoryEntry>> GetActiveHistoryEntriesOfUserAsync(string userId);
        Task AddHistoryEntryAsync(CarHistoryEntry entry);
        Task AddCarAsync(Car car);
        Task<Car> GetCarByProviderDataAsync(int providerCarId, string providerId);
    }
}