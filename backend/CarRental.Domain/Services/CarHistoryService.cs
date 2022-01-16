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
        private readonly CarReturnService _carReturnService;

        public CarHistoryService()
        {
            _carHistoryRepository = null;
        }

        public virtual async Task<CarHistoryEntry> GetByProviderAndPriceId(string providerId, string priceId)
        {
            return await _carHistoryRepository.GetByProviderAndPriceId(providerId, priceId);
        }
        
        public CarHistoryService(ICarHistoryRepository carHistoryRepository, CarReturnService carReturnService)
        {
            _carHistoryRepository = carHistoryRepository;
            _carReturnService = carReturnService;
        }

        private async Task<List<CarHistory>> ProcessToDto(List<CarHistoryEntry> history)
        {
            var ret = new List<CarHistory>();
            foreach (var entry in history)
            {
                var dto = entry.ToDto();
                var returnDetails = await _carReturnService.GetReturnEntryAsync(entry.ID);
                if (returnDetails != null)
                {
                    dto.CarCondition = returnDetails.CarCondition;
                    dto.OdometerValue = returnDetails.OdometerValue;
                    dto.PhotoFileId = returnDetails.PhotoFileId;
                    dto.PdfFileId = returnDetails.PdfFileId;
                }
                ret.Add(dto);
            }
            return ret;
        }
        
        public async Task<List<CarHistory>> GetRentalHistoryByUserAsync(string userId)
        {
            var history = await _carHistoryRepository.GetHistoryEntriesOfUserAsync(userId);

            return await ProcessToDto(history);
        }

        public async Task<List<CarHistory>> GetRentalHistoryAsync()
        {
            var history = await _carHistoryRepository.GetHistoryEntriesAsync();

            return await ProcessToDto(history);
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

        public virtual async Task MarkHistoryEntryAsConfirmed(string providerId, string priceId, string rentId, DateTime rentFrom, DateTime renTo)
        {
            await _carHistoryRepository.MarkHistoryEntryAsConfirmed(priceId, providerId, rentId, rentFrom, renTo);
        }

        public virtual async Task RegisterCarRentProcessStartAsync(string userId, CarDetails carDetails, UserDetails userDetails, string priceId)
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

        public virtual async Task UpdateCarToReturnedAsync(int historyEntryId)
        {
            await _carHistoryRepository.UpdateHistoryEntryToReturnedAsync(historyEntryId);
        }
    }
}