using System;
using IdentityModel.Client;
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
    public class LecturerApiConnector: ICarProvider
    {
        private readonly CarProviderConfig _config;
        private readonly HttpClient _client;
        private readonly string _secret;
        private readonly string _clientId;

        public LecturerApiConnector(IHttpClientFactory clientFactory, CarProviderConfig config, IConfiguration configuration)
        {
            _config = config;
            _client = clientFactory.CreateClient();
            _secret = _config != null ? configuration[_config.Config["ClientSecret"]] : "";
            _clientId = _config != null ? configuration[_config.Config["ClientId"]] : "";
        }

        public async Task<ApiResponse<List<CarDetails>>> GetCarsAsync(CarListFilter filter)
        {
            var response = await SendGetAsync<LECCarsListResponse>("/vehicles");
            
            if (response.Data != null)
            {
                var mapped = response.Data.Cars.Select(dnz => new CarDetails()
                {
                    Brand = dnz.Brand,
                    Capacity = dnz.Capacity,
                    Category = dnz.Capacity > 4 ? "Big" : "Small",
                    HorsePower = dnz.HorsePower,
                    Id = dnz.Id,
                    Model = dnz.Model,
                    ProductionYear = dnz.ProductionYear,
                    ProviderCompany = _config.Name,
                    ProviderId = _config.Id
                }).ToList();
                
                return new ApiResponse<List<CarDetails>>() {Data = CarFilteringUtil.FilterCars(mapped, filter)};
            }

            return new ApiResponse<List<CarDetails>>() {Error = response.Error};
        }

        public async Task<ApiResponse<CarRentResponse>> TryBookCar(string carId, CarRentRequest carRentRequest)
        {
            var dnzRequest = new LECRentCarRequest()
            {
                StartDate = carRentRequest.RentFrom
            };
            
            var result = await SendPostAsync<LECRentCarResponse, LECRentCarRequest>($"/vehicles/Rent/{carRentRequest.PriceId}", dnzRequest);

            if (result.Data != null)
            {
                return new ApiResponse<CarRentResponse>()
                {
                    Data = new CarRentResponse()
                    {
                        RentId = result.Data.RentId
                    }
                };
            }

            return new ApiResponse<CarRentResponse>() {Error = result.Error};
        }

        public async Task<ApiResponse<CarReturnResponse>> TryReturnCar(string rentId)
        {
            var result = await SendPostAsync<string>($"/vehicle/Return/{rentId}", false);

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

            return new ApiResponse<CarReturnResponse>() {Error = "Cannot return car"};
        }

        public async Task<ApiResponse<CarPrice>> CheckPrice(string carId, int daysCount, UserDetails userDetails) 
        {
            var dnzRequest = new LECCheckPriceRequest()
            {
                Age = userDetails.Age,
                YearsOfHavingLicense = userDetails.YearsHavingDrivingLicense,
                Location = userDetails.City,
                CurrentlyRentedCount = 1,
                OverallRentedCount = 1,
                DaysCount = daysCount
            };

            var response = await SendPostAsync<LECCarPrice, LECCheckPriceRequest>($"/vehicle/{carId}/GetPrice", dnzRequest);

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
            var token = await GetToken();

            if (token == null)
            {
                return new ApiResponse<T>() {Error = "Token error"};
            }
            
            var serialized = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}{url}");
            request.SetBearerToken(token);
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
        
        private async Task<ApiResponse<T>> SendPostAsync<T>(string url, bool readResponse) where T: class
        {
            var token = await GetToken();

            if (token == null)
            {
                return new ApiResponse<T>() {Error = "Token error"};
            }
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}{url}");
            request.SetBearerToken(token);
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

        private async Task<string> GetToken()
        {
            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _config.Config["TokenUrl"],
                ClientId = _clientId,
                ClientSecret = _secret,
                Scope = _config.Config["Scope"]
            });
            
            return tokenResponse.IsError ? null : (tokenResponse.AccessToken ?? "");
        }

        public class LECCarDetails
        {
            [JsonPropertyName(("id"))]
            public string Id { get; set; }
            [Required]
            [JsonPropertyName(("brandName"))]
            public string Brand { get; set; }
            [Required]
            [JsonPropertyName(("modelName"))]
            public string Model { get; set; }
            [Required]
            [JsonPropertyName(("year"))]
            public int ProductionYear { get; set; }
            [Required]
            [JsonPropertyName(("capacity"))]
            public int Capacity { get; set; }
            [Required]
            [JsonPropertyName("enginePower")]
            public int HorsePower { get; set; }
            [Required]
            [JsonPropertyName("description")]
            public string Description { get; set; }
        }
        
        public class LECCheckPriceRequest
        {
            [JsonPropertyName("yearsOfHavingLicense")]
            [Required, Range(0, int.MaxValue)]
            public int YearsOfHavingLicense { get; set; }
            [JsonPropertyName("age")]
            [Required, Range(0, int.MaxValue)]
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
        
        public class LECCarPrice
        {
            [Required, Range(0, long.MaxValue)]
            [JsonPropertyName("quotaId")]
            public string Id { get; set; }
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

        public class LECRentCarResponse
        {
            [JsonPropertyName("rentId")]
            public string RentId { get; set; }
            [JsonPropertyName("startDate")]
            public DateTime StartDate { get; set; }
            [JsonPropertyName("endDate")]
            public DateTime EndDate { get; set; }
        }

        public class LECRentCarRequest
        {
            [JsonPropertyName("startDate")] public DateTime StartDate { get; set; }
        }
        
        public class LECCarsListResponse
        {
            [Required]
            [JsonPropertyName("vehiclesCount")]
            public int Count { get; set; }
            [Required]
            [JsonPropertyName("vehicles")]
            public List<LECCarDetails> Cars { get; set; }
        }
    }
}