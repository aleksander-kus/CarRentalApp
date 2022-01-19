using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Exceptions;
using CarRental.Domain.Ports.Out;
using CarRental.Infrastructure.Util;
using Microsoft.Extensions.Configuration;

namespace CarRental.Infrastructure.Adapters.Providers
{
    public class OtherTeamApiConnector: ICarProvider
    {
        private readonly CarProviderConfig _config;
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public OtherTeamApiConnector(IHttpClientFactory clientFactory, CarProviderConfig config, IConfiguration configuration)
        {
            _config = config;
            _client = clientFactory.CreateClient();
            _apiKey = _config != null ? configuration[_config.Config["ApiKey"]] : "";
        }

        public async Task<ApiResponse<List<CarDetails>>> GetCarsAsync(CarListFilter filter)
        {
            var response = await SendGetAsync<List<OTCarDetails>>("/Cars");
            
            if (response.Data != null)
            {
                var mapped = response.Data.Select(obj => new CarDetails()
                {
                    Brand = obj.Brand,
                    Capacity = null,
                    Category = null,
                    HorsePower = obj.HorsePower,
                    Id = obj.Id.ToString(),
                    Description = obj.Description,
                    Model = obj.Model,
                    ProductionYear = obj.ProductionYear,
                    ProviderCompany = _config.Name,
                    ProviderId = _config.Id
                }).ToList();
                
                return new ApiResponse<List<CarDetails>>() {Data = CarFilteringUtil.FilterCars(mapped, filter)};
            }

            return new ApiResponse<List<CarDetails>>() {Error = response.Error};
        }

        public async Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, CarRentRequest carRentRequest)
        {
            var result = await SendPostAsync<OTRentCarResponse>($"/Cars/Rent/{carRentRequest.PriceId}");

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

        public async Task<ApiResponse<CarReturnResponse>> TryReturnCar(string rentId)
        {
            var result = await SendPostAsync<string>($"/Cars/Return/{rentId}", false);

            if (result.Error == null)
            {
                return new ApiResponse<CarReturnResponse>()
                {
                    Data = new CarReturnResponse()
                    {
                        Message = "Car returned"
                    }
                };
            }

            return new ApiResponse<CarReturnResponse>() {Error = result.Error};
        }

        public async Task<ApiResponse<CarPrice>> CheckPrice(string carId, int daysCount, UserDetails userDetails)
        {
            var dnzRequest = new OTCheckPriceRequest()
            {
                Age = userDetails.Age,
                YearsOfHavingLicense = userDetails.YearsHavingDrivingLicense,
                Location = userDetails.City,
                CurrentlyRentedCount = 1,
                OverallRentedCount = 1,
                DaysCount = daysCount
            };

            var response = await SendPostAsync<OTCarPrice, OTCheckPriceRequest>($"/Cars/{carId}/GetPrice", dnzRequest);

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
            request.Headers.Add("ApiKey", _apiKey);
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
            request.Headers.Add("ApiKey", _apiKey);
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
        
        private async Task<ApiResponse<T>> SendPostAsync<T>(string url, bool readResponse = true) where T: class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}{url}");
            request.Headers.Add("ApiKey", _apiKey);
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                if (readResponse)
                {
                    return new ApiResponse<T>() {Data = await JsonSerializer.DeserializeAsync<T>(content)};
                }
                return new ApiResponse<T>();
            }
            if ((int) response.StatusCode >= 500)
            {
                throw new CarProviderException();
            }

            return await JsonSerializer.DeserializeAsync<ApiResponse<T>>(content);
        }

        public class OTCarDetails
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
            public int ProductionYear { get; set; }
            [Required]
            [JsonPropertyName(("capacity"))]
            public int Capacity { get; set; }
            [Required]
            [JsonPropertyName("horsePower")]
            public int HorsePower { get; set; }
            [JsonPropertyName("description")]
            public string Description { get; set; }
        }
        
        public class OTCheckPriceRequest
        {
            [JsonPropertyName("driverLicenceYears")]
            public int YearsOfHavingLicense { get; set; }
            [JsonPropertyName("age")]
            public int Age { get; set; }
            [JsonPropertyName("location")]
            public string Location { get; set; }
            [JsonPropertyName("currentlyRentedCount")]
            public int CurrentlyRentedCount { get; set; }
            [JsonPropertyName("overallRentedCount")]
            public int OverallRentedCount { get; set; }
            [JsonPropertyName("rentDuration")]
            public int DaysCount { get; set; }
        }

        public class OTCarPrice
        {
            [JsonPropertyName("quotaId")]
            public long Id { get; set; }
            [JsonPropertyName("price")]
            public double Price { get; set; }
            [JsonPropertyName("currency")]
            public string Currency { get; set; }
            [JsonPropertyName("expiration")]
            public DateTime ExpiredAt { get; set; }
        }

        public class OTRentCarResponse
        {
            [JsonPropertyName("priceId")]
            public long PriceId { get; set; }
            [JsonPropertyName("rentId")]
            public long RentId { get; set; }
            [JsonPropertyName("rentAt")]
            public DateTime RentAt { get; set; }
            [JsonPropertyName("startDate")]
            public DateTime StartDate { get; set; }
            [JsonPropertyName("endDate")]
            public DateTime EndDate { get; set; }
        }
    }
}