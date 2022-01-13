using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarRental.IntegrationTest
{
    public class CarReturnControllerTest: IClassFixture<TestingWebAppFactory>
    {
        private readonly HttpClient _client;
        public CarReturnControllerTest(TestingWebAppFactory factory)
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
    }
}