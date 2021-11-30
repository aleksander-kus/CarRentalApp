using System;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarProvider
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}