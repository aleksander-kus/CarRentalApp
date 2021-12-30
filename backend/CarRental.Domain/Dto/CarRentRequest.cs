using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarRentRequest
    {
        [Required]
        [JsonPropertyName("rentFrom")]
        public DateTime RentFrom { get; set; }
        [Required]
        [JsonPropertyName("rentTo")]
        public DateTime RentTo { get; set; }
        [Required]
        [JsonPropertyName("priceId")]
        public string PriceId { get; set; }
    }
}