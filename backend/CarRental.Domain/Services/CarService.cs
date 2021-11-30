using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class CarService: IGetCarProvidersUseCase, IGetCarsFromProviderUseCase, IBookCarUseCase
    {
        private readonly ICarProviderFactory _carProviderFactory;

        public CarService(ICarProviderFactory carProviderFactory)
        {
            _carProviderFactory = carProviderFactory;
        }

        public Task<List<CarProvider>> GetCarProvidersAsync()
        {
            return _carProviderFactory.GetAvailableProvidersAsync();
        }

        public async Task<List<CarDetails>> GetCarsAsync(string providerId, CarListFilter filters)
        {
            var provider = await _carProviderFactory.GetProviderAsync(providerId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }
            
            return await provider.GetCarsAsync(filters);
        }

        public async Task<bool> TryBookCar(CarRentRequestDto carRentRequest)
        {
            var provider = await _carProviderFactory.GetProviderAsync(carRentRequest.ProviderId);

            if (provider == null)
            {
                throw new UnknownCarProviderException();
            }

            return await provider.TryBookCar(carRentRequest);
        }
    }
}