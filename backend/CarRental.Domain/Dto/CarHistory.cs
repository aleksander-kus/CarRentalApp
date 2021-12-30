using System;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class CarHistory
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("carBrand")]
        public string CarBrand { get; set; }
        [JsonPropertyName("carModel")]
        public string CarModel { get; set; }
        [JsonPropertyName("carProvider")]
        public string CarProvider { get; set; }
        [JsonPropertyName("rentDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("returnDate")]
        public DateTime EndDate { get; set; }
    }
}