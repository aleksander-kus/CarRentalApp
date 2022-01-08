using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Dto
{
    public class CarReturnRequest
    {
        [Required]
        [JsonPropertyName("rentId")]
        public string RentId { get; set; }
        [Required]
        [JsonPropertyName("historyEntryId")]
        public string HistoryEntryId { get; set; }
        [Required]
        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }        
        [Required]
        [JsonPropertyName("rentDate")]
        public DateTime RentDate { get; set; }
        [JsonPropertyName("returnDate")]
        public DateTime ReturnDate { get; set; }
        [Required]
        [JsonPropertyName("carCondition")]
        public string CarCondition { get; set; }
        [Required]
        [JsonPropertyName("odometerValue")]
        public double OdometerValue { get; set; }
        [Required]
        [JsonPropertyName("photoFileId")]
        public string PhotoFileId { get; set; }
        [Required]
        [JsonPropertyName("pdfFileId")]
        public string PdfFileId { get; set; }
        
    }
}