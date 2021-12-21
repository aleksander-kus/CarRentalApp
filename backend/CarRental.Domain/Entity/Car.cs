using System;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Entity
{
    public class Car
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }  // Car id in our database
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("providerCarId")]
        public int ProviderCarId { get; set; }  // Car id given by the provider
        [JsonPropertyName("provider")]
        public string ProviderId { get; set; }
    }
}