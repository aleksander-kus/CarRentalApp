using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CarRental.Domain.Ports.Out;
using CarRental.Infrastructure.Adapters;
using CarRental.Infrastructure.Database;
using CarRental.Infrastructure.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.IntegrationTest
{
    public class TestingWebAppFactory: WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<CarRentalContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<CarRentalContext>(options =>
                {
                    options.UseInMemoryDatabase("CarRentalApp");
                });
                services.AddSingleton<ICarProviderFactory, CarProviderFactory>(conf => 
                    new CarProviderFactory(new Dictionary<string, CarProviderConfig>()
                        {
                            {"DNR", new CarProviderConfig()
                            {
                                Id = "DNR",
                                Name = "DotnetRulez",
                                BaseUrl = "https://dotnetrulez-car-rental-api.azurewebsites.net",
                                Config = new Dictionary<string, string>()
                                {
                                    {"ApiKey", "DNZ_API_KEY"}
                                }
                            }}
                        },
                        (IConfiguration) conf.GetService(typeof(IConfiguration)), (IHttpClientFactory) conf.GetService(typeof(IHttpClientFactory))));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                using var appContext = scope.ServiceProvider.GetRequiredService<CarRentalContext>();
                try
                {
                    appContext.Database.EnsureCreated();
                    SeedData.PopulateTestData(appContext);
                }
                catch (Exception ex)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            });
        }
    }
}