using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class ApiResponse<T> where T: class
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}