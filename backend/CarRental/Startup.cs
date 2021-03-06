using System;
using System.Net.Http;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;
using CarRental.Domain.Services;
using CarRental.Infrastructure.Adapters;
using CarRental.Infrastructure.Database;
using CarRental.Infrastructure.Jobs;
using CarRental.Infrastructure.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CarRental
{
    public class Startup
    {
        private readonly ConfigurationManager _configurationManager;
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _configurationManager = new ConfigurationManager(env, configuration);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options => {
                    _configurationManager.AzureAdConfig.Bind(options);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "extension_Role"
                    };

                }, options => 
                    _configurationManager.AzureAdConfig.Bind(options));
            
            services.AddSingleton<IAuthenticationProvider>(o =>
            {
                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(_configurationManager.AzureAdConfig["ClientId"])
                    .WithTenantId(_configurationManager.AzureAdConfig["tenantId"])
                    .WithClientSecret(_configurationManager.AzureAdConfig["Secret"])
                    .Build();

                return new ClientCredentialProvider(confidentialClientApplication);
            });

            services.AddHttpClient();
            services.AddSingleton<ICarProviderFactory, CarProviderFactory>(conf => 
                new CarProviderFactory(_configurationManager.CarProvidersConfig,
                    (IConfiguration) conf.GetService(typeof(IConfiguration)), (IHttpClientFactory) conf.GetService(typeof(IHttpClientFactory))));
            services.AddSingleton<IEmailApi, SendgridApi>(_ =>
                new SendgridApi(_configurationManager.SendgridConfig));
            services.AddSingleton(conf =>
                new GraphServiceClient(conf.GetService<IAuthenticationProvider>()));
            services.AddSingleton<IUserRepository, UserGraphRepository>();
            services.AddScoped<ICarEmailedEventRepository, CarEmailedEventRepository>();
            services.AddSingleton<IGetUserDetailsUseCase, UserService>();
            services.AddScoped<IGetCarProvidersUseCase, CarService>();
            services.AddScoped<IGetCarsFromProviderUseCase, CarService>();
            services.AddScoped<ICheckPriceUseCase, CarService>();
            services.AddScoped<IBookCarUseCase, CarService>();
            services.AddScoped<IReturnCarUseCase, CarService>();
            services.AddScoped<IUploadFileUseCase, StorageService>();
            services.AddScoped<IGetSasUriUseCase, StorageService>();
            services.AddScoped<IGetCurrentlyRentedCarsUseCase, CarHistoryService>();
            services.AddScoped<IGetRentalHistoryUseCase, CarHistoryService>();
            services.AddScoped<ICarHistoryRepository, CarHistoryRepository>();
            services.AddScoped<ICarReturnEntryRepository, CarReturnEntryRepository>();
            services.AddScoped<ISendNewCarsEventUseCase, CarService>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            services.AddScoped<IStorageClient, BlobStorageClient>(conf => 
                new BlobStorageClient(_configurationManager.StorageConnectionString, _configurationManager.StorageContainerName));
            services.AddScoped<CarService>();
            services.AddScoped<CarHistoryService>();
            services.AddScoped<CarReturnService>();
            services.AddScoped<EmailService>();
            services.AddScoped<UserService>();
            services.AddScoped<StorageService>();

            if (_configurationManager.EmailsCronConfig != null)
            {
                services.AddCronJob<EmailBackgroundService>(c =>
                {
                    c.TimeZoneInfo = TimeZoneInfo.Local;
                    c.CronExpression = _configurationManager.EmailsCronConfig;
                });
            }

            services.AddResponseCaching();
            services.AddAuthorization();
            services.AddControllers();
            
            services.AddDbContext<CarRentalContext>(options => 
                options.UseSqlServer(_configurationManager.DatabaseConnectionString, b => b.MigrationsAssembly("CarRental.Infrastructure")));
            
            services.AddCors(o => o.AddPolicy("default", builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            
            if (_configurationManager.UseSwagger)
                services.AddSwaggerGen();

            services.AddApplicationInsightsTelemetry();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            if (_configurationManager.UseSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarRental v1"));
            }

            app.UseCors("default");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}