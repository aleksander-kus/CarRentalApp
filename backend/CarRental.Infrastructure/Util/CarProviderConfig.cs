using System;
using System.Collections.Generic;

namespace CarRental.Infrastructure.Util
{
    public class CarProviderConfig
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public Dictionary<string, string> Config { get; set; }
    }
}