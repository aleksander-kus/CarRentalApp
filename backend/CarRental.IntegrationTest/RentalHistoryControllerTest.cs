using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarRental.IntegrationTest
{
    public class RentalHistoryControllerTest: IClassFixture<TestingWebAppFactory>
    {
        private readonly HttpClient _client;
        
        public RentalHistoryControllerTest(TestingWebAppFactory factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("IntegrationTest")
                        .AddScheme<AuthenticationSchemeOptions, ClientAuthHandler>(
                            "IntegrationTest",
                            options => { }
                        );
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();
        }
        
        [Fact]
        public async Task ShouldReturnListOfRentalHistoryByUser()
        {
            var response = await _client.GetAsync("/api/cars/rentalHistoryByUser");

            response.EnsureSuccessStatusCode();

            var history = await JsonSerializer.DeserializeAsync<List<CarHistory>>(await response.Content.ReadAsStreamAsync());

            Assert.NotEmpty(history);
        }
        
        [Fact]
        public async Task ShouldReturnListOfRentalHistory()
        {
            var response = await _client.GetAsync("/api/cars/rentalHistory");

            response.EnsureSuccessStatusCode();

            var history = await JsonSerializer.DeserializeAsync<List<CarHistory>>(await response.Content.ReadAsStreamAsync());

            Assert.NotEmpty(history);
        }
    }
}