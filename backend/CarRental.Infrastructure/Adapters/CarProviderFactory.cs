using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.Out;
using CarRental.Infrastructure.Adapters.Providers;
using CarRental.Infrastructure.Util;
using Microsoft.Extensions.Configuration;

namespace CarRental.Infrastructure.Adapters
{
    public class CarProviderFactory: ICarProviderFactory
    {
        private readonly Dictionary<string, CarProviderConfig> _providerConfigs;

        private readonly Dictionary<string, ICarProvider> _carProviders;

        public CarProviderFactory(Dictionary<string, CarProviderConfig> providerConfigs, IConfiguration configuration, IHttpClientFactory factory)
        {
            _providerConfigs = providerConfigs;
            
            _carProviders = new Dictionary<string, ICarProvider>()
            {
                {"DNR", new DotnetRulezApiConnector(factory, providerConfigs["DNR"], configuration)}
            };
        }

        public Task<List<CarProvider>> GetAvailableProvidersAsync()
        {
            var providers = _providerConfigs.Values
                .Select(c => new CarProvider() {Id = c.Id, Name = c.Name})
                .ToList();

            return Task.FromResult(providers);
        }

        public Task<ICarProvider> GetProviderAsync(string providerId)
        {
            return Task.FromResult(_carProviders[providerId]);
        }
    }
}