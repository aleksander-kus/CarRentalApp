using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class CarService: IGetCarProvidersUseCase, IGetCarsFromProviderUseCase, IBookCarUseCase, ICheckPriceUseCase
    {
        private readonly IGetUserDetailsUseCase _getUserDetailsUseCase;
        private readonly ICarProviderFactory _carProviderFactory;

        public CarService(ICarProviderFactory carProviderFactory, IGetUserDetailsUseCase getUserDetailsUseCase)
        {
            _carProviderFactory = carProviderFactory;
            _getUserDetailsUseCase = getUserDetailsUseCase;
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
        
        public async Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, string providerId, CarRentRequest carRentRequest)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }

            return await provider.TryBookCar(carId, carRentRequest);
        }

        public async Task<ApiResponse<CarPrice>> CheckPrice(CarCheckPrice carCheckPrice, string providerId, string carId, string userId)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);
            var userData = await _getUserDetailsUseCase.GetUserDetails(userId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }

            return await provider.CheckPrice(carId, carCheckPrice.DaysCount, userData);
        }
    }
}