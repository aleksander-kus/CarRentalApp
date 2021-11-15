using System;
using CarRental.Domain.CarList;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports;
using CarRental.Infrastructure.Database;
using CarRental.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services.AddSingleton<IGetUserDetailsUseCase, UserDetailsService>();
            
            services.AddAuthorization();
            
            services.AddControllers();
            
            services.AddDbContext<CarRentalContext>(options => 
                options.UseSqlServer(_configurationManager.DatabaseConnectionString));
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            
            if (_configurationManager.UseSwagger)
            {
                services.AddSwaggerGen();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}