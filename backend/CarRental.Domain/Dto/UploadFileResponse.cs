using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class UploadFileResponse
    {
        [Required]
        [JsonPropertyName("fileId")]
        public string FileId { get; set; }
    }
}