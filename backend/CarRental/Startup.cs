using System;
using CarRental.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddAuthorization();
            
            services.AddControllers();
            
            services.AddDbContext<CarRentalContext>(options => 
                options.UseSqlServer(_configurationManager.DatabaseConnectionString));

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