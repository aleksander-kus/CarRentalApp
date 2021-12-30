using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using Xunit;

namespace CarRental.IntegrationTest
{
    public class CarSearchControllerTest: IClassFixture<TestingWebAppFactory>
    {
        private readonly HttpClient _client;
        public CarSearchControllerTest(TestingWebAppFactory factory) 
            => _client = factory.CreateClient();
        
        [Fact]
        public async Task ShouldReturnListOfProviders()
        {
            var response = await _client.GetAsync("/api/cars/providers");

            response.EnsureSuccessStatusCode();

            var providers = await JsonSerializer.DeserializeAsync<List<CarProvider>>(await response.Content.ReadAsStreamAsync());

            Assert.NotEmpty(providers);
        }
        
        [Fact]
        public async Task ShouldFetchCarsFromProvider()
        {
            var response = await _client.GetAsync("/api/cars/DNR");

            response.EnsureSuccessStatusCode();

            var cars = await JsonSerializer.DeserializeAsync<ApiResponse<List<CarDetails>>>(await response.Content.ReadAsStreamAsync());

            Assert.Null(cars.Error);
            Assert.NotEmpty(cars.Data);
        }
    }
}