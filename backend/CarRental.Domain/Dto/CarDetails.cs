using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarDetails
    {
        [JsonPropertyName(("id"))]
        public string Id { get; set; }
        [JsonPropertyName(("brand"))]
        public string Brand { get; set; }
        [JsonPropertyName(("model"))]
        public string Model { get; set; }
        [JsonPropertyName(("productionYear"))]
        public int ProductionYear { get; set; }
        [JsonPropertyName(("capacity"))]
        public int? Capacity { get; set; }
        [JsonPropertyName(("category"))]
        public string Category { get; set; }
        [JsonPropertyName("horsePower")]
        public int HorsePower { get; set; }
        [JsonPropertyName("providerCompany")]
        public string ProviderCompany { get; set; }
        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}