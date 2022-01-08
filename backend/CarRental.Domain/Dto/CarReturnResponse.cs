using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarReturnResponse
    {
        [Required]
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}