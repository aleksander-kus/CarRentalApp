using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class FileSasResponse
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}