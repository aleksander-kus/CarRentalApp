using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Domain.Dto;
using CarRental.Infrastructure.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace CarRental
{
    public class ConfigurationManager
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public ConfigurationManager(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }
        
        public string DatabaseConnectionString
        {
            get
            {
                var config = _configuration.GetSection("Database");
                var server = _configuration[config["Server"]];
                var dbName = _configuration[config["DatabaseName"]];
                var userName = _configuration[config["Login"]];
                var password = _configuration[config["Password"]];
                var connectionString =
                    $"Server={server}; Database={dbName}; User Id={userName}; Password={password}; Trusted_Connection=false;";

                return connectionString;
            }
        }

        public bool UseSwagger
        {
            get
            {
                var envName = _env.EnvironmentName;
                var dbConfig = _configuration.GetSection("Swagger");

                if (dbConfig[envName] == null)
                {
                    throw new ArgumentException("Invalid environment name");
                }

                return bool.Parse(dbConfig[envName]);
            }
        }

        public IConfigurationSection AzureAdConfig => GetEnvSection("AzureAdB2C");

        public IConfigurationSection SendgridConfig => _configuration.GetSection("Sendgrid");

        public string EmailsCronConfig => _configuration[_configuration["EmailsCron"]];
        
        public Dictionary<string, CarProviderConfig> CarProvidersConfig
        {
            get
            {
                return _configuration.GetSection("CarProviders").GetChildren()
                    .ToDictionary(x => x.Key,
                        x => new CarProviderConfig() {
                            Id = x["Id"],
                            Name = x["Name"],
                            BaseUrl = x["BaseUrl"],
                            Config = x.GetSection("Config").GetChildren()
                                .ToDictionary(y => y.Key, y => y.Value)
                        });
            }
        }

        private IConfigurationSection GetEnvSection(string key)
        {
            var envName = _env.EnvironmentName;
            var dbConfig = _configuration.GetSection(key);

            if (dbConfig.GetSection(envName) == null)
            {
                throw new ArgumentException("Invalid environment name");
            }

            return dbConfig.GetSection(envName);
        }
    }
}