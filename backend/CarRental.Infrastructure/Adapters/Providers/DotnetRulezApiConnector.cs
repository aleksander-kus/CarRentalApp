using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.Out;
using CarRental.Infrastructure.Util;
using Microsoft.Extensions.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CarRental.Infrastructure.Adapters.Providers
{
    public class DotnetRulezApiConnector: ICarProvider
    {
        private readonly CarProviderConfig _config;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public DotnetRulezApiConnector(IHttpClientFactory clientFactory, CarProviderConfig config, IConfiguration configuration)
        {
            _config = config;
            _configuration = configuration;
            _client = clientFactory.CreateClient();
        }

        public async Task<List<CarDetails>> GetCarsAsync(CarListFilter filter)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl}/api/Cars");
            request.Headers.Add("x-api-key", _configuration[_config.Config["SecretApiKey"]]);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStreamAsync();
                var result = (await JsonSerializer.DeserializeAsync<CarsListResponse>(content))?.Cars;

                return CarFilteringUtil.FilterCars(result, filter);
            }
            
            throw new CarProviderException();
        }

        public async Task<bool> TryBookCar(CarRentRequestDto carRentRequest)
        {
            return true;
        }

        private class CarsListResponse
        {
            [JsonPropertyName("carCount")]
            public int Count { get; set; }
            [JsonPropertyName("cars")]
            public List<CarDetails> Cars { get; set; }
        }
    }
}