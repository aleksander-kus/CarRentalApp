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

        [JsonPropertyName("carId")] 
        public string CarId { get; set; }
        [JsonPropertyName("carProvider")]
        public string CarProvider { get; set; }
        [JsonPropertyName("rentId")]
        public string RentId { get; set; }
        [JsonPropertyName("rentDate")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("returnDate")]
        public DateTime? EndDate { get; set; }
        [JsonPropertyName("returned")]
        public bool Returned { get; set; }
        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }
        [JsonPropertyName("carCondition")]
        public string CarCondition { get; set; }
        [JsonPropertyName("odometerValue")]
        public double OdometerValue { get; set; }
        [JsonPropertyName("photoFileId")]
        public string PhotoFileId { get; set; }
        [JsonPropertyName("pdfFileId")]
        public string PdfFileId { get; set; }
    }
}