using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
    public class CarPriceTest: IClassFixture<TestingWebAppFactory>
    {
        private readonly HttpClient _client;
        public CarPriceTest(TestingWebAppFactory factory)
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
        public async Task ShouldCheckCarPrice()
        {
            var response = await _client.PostAsync("/api/cars/DNR/1/price",
                new StringContent(JsonSerializer.Serialize(new { DaysCount = 1 }), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var price = await JsonSerializer.DeserializeAsync<CarPrice>(await response.Content.ReadAsStreamAsync());

            Assert.NotNull(price);
        }
    }
}