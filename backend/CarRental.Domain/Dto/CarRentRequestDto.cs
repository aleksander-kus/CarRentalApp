using System;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarRentRequestDto
    {
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }
        [JsonPropertyName("carId")]
        public string CarId { get; set; }
        [JsonPropertyName("rentFrom")]
        public DateTime RentFrom { get; set; }
        [JsonPropertyName("rentTo")]
        public DateTime RentTo { get; set; }
        
    }
}