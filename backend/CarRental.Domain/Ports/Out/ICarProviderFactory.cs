using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.Out
{
    public interface ICarProviderFactory
    {
        Task<List<CarProvider>> GetAvailableProvidersAsync();

        Task<ICarProvider> GetProviderAsync(string providerId);
    }
}