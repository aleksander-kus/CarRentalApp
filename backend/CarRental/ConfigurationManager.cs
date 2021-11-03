using System;
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
                var config = GetEnvSection("Database");
                var server = config["server"];
                var dbName = config["DatabaseName"];
                var userName = _configuration[config["SecretLogin"]];
                var password = _configuration[config["SecretPassword"]];
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