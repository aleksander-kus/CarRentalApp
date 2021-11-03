using System;
using System.Text.Json.Serialization;

namespace CarRental.Domain.Dto
{
    public class UserDetails
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("age")]
        public int Age { get; set; }
        [JsonPropertyName("yearsHavingDrivingLicense")]
        public int YearsHavingDrivingLicense { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }
    }
}