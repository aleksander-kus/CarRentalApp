using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class CurrentlyRentedControllerTest: IClassFixture<TestingWebAppFactory>
    {
        private readonly TestingWebAppFactory _factory;

        public CurrentlyRentedControllerTest(TestingWebAppFactory factory)
        {
            _factory = factory;
        }
        
        [Fact]  
        public async Task ShouldReturnListOfCurrentlyRentedByUser()
        {
            var client = _factory.WithWebHostBuilder(builder =>
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
            
            var response = await client.GetAsync("/api/cars/currentlyRentedByUser");

            response.EnsureSuccessStatusCode();

            var history = await JsonSerializer.DeserializeAsync<List<CarHistory>>(await response.Content.ReadAsStreamAsync());

            Assert.NotEmpty(history);
        }
        
        [Fact]
        public async Task ShouldReturnListOfCurrentlyRented()
        {
            var client = _factory.WithWebHostBuilder(builder =>
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
            
            var response = await client.GetAsync("/api/cars/currentlyRented");

            response.EnsureSuccessStatusCode();

            var history = await JsonSerializer.DeserializeAsync<List<CarHistory>>(await response.Content.ReadAsStreamAsync());

            Assert.NotEmpty(history);
        }
    }
}