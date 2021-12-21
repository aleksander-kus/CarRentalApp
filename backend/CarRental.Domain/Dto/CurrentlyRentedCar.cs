using System;
using System.Text.Json.Serialization;
using CarRental.Domain.Entity;

namespace CarRental.Domain.Dto
{
    public class CurrentlyRentedCar
    {
        [JsonPropertyName("car")]
        public Car Car { get; set; }
        [JsonPropertyName("rentDate")]
        public DateTime StartDate { get; set; }
    }
}