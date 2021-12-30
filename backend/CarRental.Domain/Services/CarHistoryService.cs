using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class CarHistoryService : IGetCurrentlyRentedCarsUseCase
    {
        private readonly ICarHistoryRepository _carHistoryRepository;
        
        public CarHistoryService(ICarHistoryRepository carHistoryRepository)
        {
            _carHistoryRepository = carHistoryRepository;
        }

        public async Task<List<CarHistoryEntry>> GetCurrentlyRentedCarsOfUserAsync(string userId)
        {
            return await _carHistoryRepository.GetActiveHistoryEntriesOfUserAsync(userId);
        }

        public async Task<List<CarHistoryEntry>> GetCurrentlyRentedCarsAsync()
        {
            return await _carHistoryRepository.GetActiveHistoryEntriesAsync();
        }

        public async Task RegisterCarRentAsync(string userId, int providerCarId, string providerId,
            CarRentRequest carRentRequest)
        {
            // we store basic data about cars in internal backed database when someone rents it
            // this is needed when returning information about rent history
            // NOTE: once a car with provider info is stored, we do not change it
            // If a provider changes their IDs, our database will not be changed
            // An alternative solution: populate our internal database every time we query providers for cars
            var car = await _carHistoryRepository.GetCarByProviderDataAsync(providerCarId, providerId);
            if (car == null)
            {
                car = new Car
                {
                    Brand = carRentRequest.Brand,
                    Model = carRentRequest.Model,
                    ProviderCarId = providerCarId,
                    ProviderId = providerId
                };
                await _carHistoryRepository.AddCarAsync(car);
            }
            await _carHistoryRepository.AddHistoryEntryAsync(new CarHistoryEntry(userId, car, carRentRequest));
        }
    }
}