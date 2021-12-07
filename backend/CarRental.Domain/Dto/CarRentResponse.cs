using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarRentResponse
    {
        [Required]
        [JsonPropertyName("rentId")]
        public string RentId { get; set; }
    }
}