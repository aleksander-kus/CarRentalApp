using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarPrice
    {
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [Required]
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [Required]
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}