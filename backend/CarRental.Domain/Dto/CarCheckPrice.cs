using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarCheckPrice
    {
        [Required]
        [JsonPropertyName("daysCount")]
        public int DaysCount { get; set; }
    }
}