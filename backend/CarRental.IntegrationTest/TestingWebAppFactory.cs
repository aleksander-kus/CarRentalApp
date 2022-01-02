using System;
using System.Linq;
using CarRental.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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