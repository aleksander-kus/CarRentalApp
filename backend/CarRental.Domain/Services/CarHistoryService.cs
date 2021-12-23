using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class CarHistoryService : IGetCurrentlyRentedCarsUseCase, IGetRentalHistoryUseCase
    {
        private readonly ICarHistoryRepository _carHistoryRepository;

        public CarHistoryService(ICarHistoryRepository carHistoryRepository)
        {
            _carHistoryRepository = carHistoryRepository;
        }

        public async Task<List<CarHistory>> GetRentalHistoryByUserAsync(string userId)
        {
            var history = await _carHistoryRepository.GetHistoryEntriesOfUserAsync(userId);

            return history.Select(h => h.ToDto()).ToList();
        }

        public async Task<List<CarHistory>> GetRentalHistoryAsync()
        {
            var history = await _carHistoryRepository.GetHistoryEntriesAsync();

            return history.Select(h => h.ToDto()).ToList();
        }
        
        public async Task<List<CarHistory>> GetCurrentlyRentedCarsOfUserAsync(string userId)
        {
            var history = await _carHistoryRepository.GetActiveHistoryEntriesOfUserAsync(userId);

            return history.Select(h => h.ToDto()).ToList();
        }

        public async Task<List<CarHistory>> GetCurrentlyRentedCarsAsync()
        {
            var history = await _carHistoryRepository.GetActiveHistoryEntriesAsync();

            return history.Select(h => h.ToDto()).ToList();
        }

        public async Task MarkHistoryEntryAsConfirmed(string providerId, string priceId, string rentId, DateTime rentFrom, DateTime renTo)
        {
            await _carHistoryRepository.MarkHistoryEntryAsConfirmed(priceId, providerId, rentId, rentFrom, renTo);
        }

        public async Task RegisterCarRentProcessStartAsync(string userId, CarDetails carDetails, UserDetails userDetails, string priceId)
        {
            var entry = new CarHistoryEntry()
            {
                CarBrand = carDetails.Brand,
                CarModel = carDetails.Model,
                CarProvider = carDetails.ProviderCompany,
                CarId = carDetails.Id,
                ProviderId = carDetails.ProviderId,
                UserEmail = userDetails.Email,
                PriceId = priceId,
                UserId = userId,
                EndDate = null,
                IsRentConfirmed = false,
                StartDate = null
            };

            await _carHistoryRepository.AddHistoryEntryAsync(entry);
        }
    }
}