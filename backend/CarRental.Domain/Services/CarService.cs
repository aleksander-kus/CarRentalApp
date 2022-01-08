using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class CarService: IGetCarProvidersUseCase, IGetCarsFromProviderUseCase, IBookCarUseCase, IReturnCarUseCase, ICheckPriceUseCase, ISendNewCarsEventUseCase
    {
        private readonly ICarEmailedEventRepository _carEmailedEventRepository;
        private readonly ICarReturnEntryRepository _carReturnEntryRepository;
        private readonly IGetUserDetailsUseCase _getUserDetailsUseCase;
        private readonly ICarProviderFactory _carProviderFactory;
        private readonly EmailService _emailService;
        private readonly CarHistoryService _carHistoryService;
        private readonly StorageService _storageService;

        public CarService(ICarProviderFactory carProviderFactory, IGetUserDetailsUseCase getUserDetailsUseCase,
            CarHistoryService carHistoryService, EmailService emailService, ICarEmailedEventRepository carEmailedEventRepository,
            ICarReturnEntryRepository carReturnEntryRepository, StorageService storageService)
        {
            _carProviderFactory = carProviderFactory;
            _getUserDetailsUseCase = getUserDetailsUseCase;
            _carHistoryService = carHistoryService;
            _emailService = emailService;
            _carEmailedEventRepository = carEmailedEventRepository;
            _carReturnEntryRepository = carReturnEntryRepository;
            _storageService = storageService;
        }

        public Task<List<CarProvider>> GetCarProvidersAsync()
        {
            return _carProviderFactory.GetAvailableProvidersAsync();
        }

        public async Task<ApiResponse<List<CarDetails>>> GetCarsAsync(string providerId, CarListFilter filters)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }
            
            return await provider.GetCarsAsync(filters);
        }
        
        public async Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, string providerId, string userId, CarRentRequest carRentRequest)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }

            var result = await provider.TryBookCar(carId, carRentRequest);
            if (result.Error != null)
                return result;
            
            var carsResult = await GetCarsAsync(providerId, CarListFilter.All);
            var car = carsResult.Data.Find(c => c.Id == carId);
            if (car == null)
                throw new InvalidDataException();

            var userDetails = await _getUserDetailsUseCase.GetUserDetailsAsync(userId);
            await _emailService.NotifyUserAfterCarRent(userDetails, carRentRequest);
            await _carHistoryService.MarkHistoryEntryAsConfirmed(providerId, carRentRequest.PriceId, result.Data.RentId, carRentRequest.RentFrom, carRentRequest.RentTo);

            return result;
        }

        public async Task<ApiResponse<CarReturnResponse>> TryReturnCar(string carId, string providerId, CarReturnRequest carReturnRequest)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);
            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }

            var fileExists = await _storageService.ExistsFile(carReturnRequest.PhotoFileId);
            if (!fileExists)
                return new ApiResponse<CarReturnResponse>() {Error = "Failed to obtain photo"};
                
            fileExists = await _storageService.ExistsFile(carReturnRequest.PdfFileId);
            if (!fileExists)
                return new ApiResponse<CarReturnResponse>() {Error = "Failed to obtain pdf file"};

            var result = await provider.TryReturnCar(carReturnRequest.RentId);
            if (result.Error != null)
                return result;
            
            var historyEntryId = int.Parse(carReturnRequest.HistoryEntryId);
            await _carReturnEntryRepository.AddReturnEntryAsync(new CarReturnEntry()
            {
                PdfFileId = carReturnRequest.PdfFileId,
                PhotoFileId = carReturnRequest.PhotoFileId,
                OdometerValue = carReturnRequest.OdometerValue,
                CarCondition = carReturnRequest.CarCondition,
                CarId = carId,
                RentId = carReturnRequest.RentId,
                RentDate = carReturnRequest.RentDate,
                ReturnDate = carReturnRequest.ReturnDate,
                UserEmail = carReturnRequest.UserEmail,
                HistoryEntryId = historyEntryId
            });

            await _carHistoryService.UpdateCarToReturnedAsync(historyEntryId);
            await _emailService.NotifyUserAfterCarReturn(carReturnRequest.UserEmail);
            
            return result;
        }

        public async Task<ApiResponse<CarPrice>> CheckPrice(CarCheckPrice carCheckPrice, string providerId, string carId, string userId)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);
            var userData = await _getUserDetailsUseCase.GetUserDetailsAsync(userId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }

            var cars = await provider.GetCarsAsync(CarListFilter.All);
            var car = cars.Data.First(c => c.Id == carId);
            
            var response = await provider.CheckPrice(carId, carCheckPrice.DaysCount, userData);

            if (response.Data != null)
            {
                await _carHistoryService.RegisterCarRentProcessStartAsync(userId, car, userData, response.Data.Id);
            }
            
            return response;
        }
        
        public async Task SendNewCars()
        {
            var carsToSend = await GetNotSent();

            if (carsToSend.Any())
            {
                await _emailService.NotifyAboutNewCars(carsToSend);

                foreach (var car in carsToSend)
                {
                    await _carEmailedEventRepository.AddEmailedAsync(new CarEmailedEvent()
                        {CarID = car.Id, ProviderID = car.ProviderId});
                }
            }
        }
        
        private async Task<List<CarDetails>> GetNotSent()
        {
            var newCars = new LinkedList<CarDetails>();
            
            var carProviders = await GetCarProvidersAsync();
            var emailedCars = (await _carEmailedEventRepository.GetAllAsync())
                .GroupBy(c => c.ProviderID)
                .ToDictionary(g => g.Key, cars => cars
                    .ToDictionary(c => c.CarID, c => c));

            foreach (var provider in carProviders)
            {
                var cars = await GetCarsAsync(provider.Id, CarListFilter.All);

                var filteredCars = cars.Data
                    .Where(car => !(emailedCars.ContainsKey(car.ProviderId) && emailedCars[car.ProviderId].ContainsKey(car.Id)));
                
                foreach (var car in filteredCars)
                {
                    newCars.AddLast(car);
                }
            }

            return newCars.ToList();
        }
    }
}