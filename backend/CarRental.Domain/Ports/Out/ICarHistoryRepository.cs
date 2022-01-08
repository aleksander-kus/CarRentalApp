using System;
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
        Task MarkHistoryEntryAsConfirmed(string priceId, string providerId, string rentId, DateTime rentFrom, DateTime renTo);
        Task UpdateHistoryEntryToReturnedAsync(int historyEntryId);
        Task<List<CarHistoryEntry>> GetHistoryEntriesAsync();
        Task<List<CarHistoryEntry>> GetHistoryEntriesOfUserAsync(string userId);
    }
}