using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private readonly string _apiKey;

        public DotnetRulezApiConnector(IHttpClientFactory clientFactory, CarProviderConfig config, IConfiguration configuration)
        {
            _config = config;
            _client = clientFactory.CreateClient();
            _apiKey = configuration[_config.Config["ApiKey"]];
        }

        public async Task<ApiResponse<List<CarDetails>>> GetCarsAsync(CarListFilter filter)
        {
            var response = await SendGetAsync<DNZCarsListResponse>("/api/cars");
            
            if (response.Data != null)
            {
                var mapped = response.Data.Cars.Select(dnz => new CarDetails()
                {
                    Brand = dnz.Brand,
                    Capacity = dnz.Capacity,
                    Category = dnz.Category,
                    HorsePower = dnz.HorsePower,
                    Id = dnz.Id.ToString(),
                    Model = dnz.Model,
                    ProductionYear = dnz.ProductionYear,
                    ProviderCompany = dnz.ProviderCompany
                }).ToList();
                
                return new ApiResponse<List<CarDetails>>() {Data = CarFilteringUtil.FilterCars(mapped, filter)};
            }

            return new ApiResponse<List<CarDetails>>() {Error = response.Error};
        }

        public async Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, CarRentRequest carRentRequest)
        {
            var dnzRequest = new DNZRentCarRequest()
            {
                EndDate = carRentRequest.RentTo,
                PriceId = long.Parse(carRentRequest.PriceId),
                StartDate = carRentRequest.RentFrom
            };
            
            var result = await SendPostAsync<DNZRentCarResponse, DNZRentCarRequest>($"/api/cars/{carId}/rent", dnzRequest);

            if (result.Data != null)
            {
                return new ApiResponse<CarRentResponse>()
                {
                    Data = new CarRentResponse()
                    {
                        RentId = result.Data.RentId.ToString()
                    }
                };
            }

            return new ApiResponse<CarRentResponse>() {Error = result.Error};
        }

        public async Task<ApiResponse<CarPrice>> CheckPrice(string carId, int daysCount, UserDetails userDetails)
        {
            var dnzRequest = new DNZCheckPriceRequest()
            {
                Age = userDetails.Age,
                YearsOfHavingLicense = userDetails.YearsHavingDrivingLicense,
                City = userDetails.City,
                Country = "Poland",
                CurrentlyRentedCount = 1,
                OverallRentedCount = 1,
                DaysCount = daysCount
            };

            var response = await SendPostAsync<DNZCarPrice, DNZCheckPriceRequest>($"/cars/{carId}/price", dnzRequest);

            if (response.Data != null)
            {
                return new ApiResponse<CarPrice>()
                {
                    Data = new CarPrice()
                    {
                        Currency = response.Data.Currency,
                        Id = response.Data.Id.ToString(),
                        Price = response.Data.Price
                    }
                };
            }

            return new ApiResponse<CarPrice>() {Error = response.Error};
        }

        private async Task<ApiResponse<T>> SendGetAsync<T>(string url) where T: class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_config.BaseUrl}{url}");
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStreamAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>() {Data = await JsonSerializer.DeserializeAsync<T>(content)};
            }
            if ((int) response.StatusCode >= 500)
            {
                throw new CarProviderException();
            }

            return await JsonSerializer.DeserializeAsync<ApiResponse<T>>(content);
        }
        
        private async Task<ApiResponse<T>> SendPostAsync<T, TP>(string url, TP payload) where T: class
        {
            var serialized = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}{url}");
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("Accept", "application/json");
            request.Content = new StringContent(serialized , Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<T>() {Data = await JsonSerializer.DeserializeAsync<T>(content)};
            }
            if ((int) response.StatusCode >= 500)
            {
                throw new CarProviderException();
            }

            return await JsonSerializer.DeserializeAsync<ApiResponse<T>>(content);
        }

        private class DNZCarDetails
        {
            [JsonPropertyName(("id"))]
            [Required, Range(0, long.MaxValue)]
            public long Id { get; set; }
            [Required]
            [JsonPropertyName(("brand"))]
            public string Brand { get; set; }
            [Required]
            [JsonPropertyName(("model"))]
            public string Model { get; set; }
            [Required]
            [JsonPropertyName(("productionYear"))]
            [Range(1900, 2100)]
            public int ProductionYear { get; set; }
            [Required]
            [JsonPropertyName(("capacity"))]
            [Range(0, 10)]
            public int Capacity { get; set; }
            [Required]
            [JsonPropertyName(("category"))]
            public string Category { get; set; }
            [Required]
            [JsonPropertyName("horsePower")]
            [Range(0, 1000)]
            public int HorsePower { get; set; }
            [Required]
            [JsonPropertyName("providerCompany")]
            public string ProviderCompany { get; set; }
        }
        
        private class DNZCheckPriceRequest
        {
            [JsonPropertyName("yearsOfHavingLicense")]
            [Required, Range(0, int.MaxValue)]
            public int YearsOfHavingLicense { get; set; }
            [JsonPropertyName("age")]
            [Required, Range(0, int.MaxValue)]
            public int Age { get; set; }
            [JsonPropertyName("country")]
            [Required, MinLength(2)]
            public string Country { get; set; }
            [JsonPropertyName("city")]
            [Required]
            public string City { get; set; }
            [JsonPropertyName("currentlyRentedCount")]
            [Required, Range(0, int.MaxValue)]
            public int CurrentlyRentedCount { get; set; }
            [JsonPropertyName("overallRentedCount")]
            [Required, Range(0, int.MaxValue)]
            public int OverallRentedCount { get; set; }
            [JsonPropertyName("daysCount")]
            [Required, Range(1, int.MaxValue)]
            public int DaysCount { get; set; }
        }

        private class DNZCarPrice
        {
            [Required, Range(0, long.MaxValue)]
            [JsonPropertyName("id")]
            public long Id { get; set; }
            [Required]
            [JsonPropertyName("price")]
            public double Price { get; set; }
            [Required]
            [JsonPropertyName("currency")]
            public string Currency { get; set; }
            [Required]
            [JsonPropertyName("generatedAt")]
            public DateTime GeneratedAt { get; set; }
            [Required]
            [JsonPropertyName("expiredAt")]
            public DateTime ExpiredAt { get; set; }
        }

        private class DNZRentCarResponse
        {
            [Required, Range(0, long.MaxValue)]
            [JsonPropertyName("priceId")]
            public long PriceId { get; set; }
            [Required, Range(0, long.MaxValue)]
            [JsonPropertyName("rentId")]
            public long RentId { get; set; }
            [Required, Range(0, long.MaxValue)]
            [JsonPropertyName("rentAt")]
            public DateTime RentAt { get; set; }
            [Required]
            [JsonPropertyName("startDate")]
            public DateTime StartDate { get; set; }
            [Required]
            [JsonPropertyName("endDate")]
            public DateTime EndDate { get; set; }
        }

        private class DNZRentCarRequest
        {
            [Required, Range(0, int.MaxValue)]
            [JsonPropertyName("priceId")]
            public long PriceId { get; set; }
            [Required]
            [JsonPropertyName("startDate")]
            public DateTime StartDate { get; set; }
            [Required]
            [JsonPropertyName("endDate")]
            public DateTime EndDate { get; set; }
        }

        private class DNZCarsListResponse
        {
            [Required]
            [JsonPropertyName("carCount")]
            public int Count { get; set; }
            [Required]
            [JsonPropertyName("cars")]
            public List<DNZCarDetails> Cars { get; set; }
        }
    }
}