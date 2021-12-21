using System;
using System.Text.Json.Serialization;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Entity
{
    public class CarHistoryEntry
    {
        [JsonIgnore]
        public int ID { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("carId")]
        public int CarID { get; set; }  // car id in our database
        [JsonPropertyName("car")]
        public Car Car { get; set; }
        [JsonPropertyName("rentDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("returnDate")]
        public DateTime EndDate { get; set; }

        public CarHistoryEntry()
        {
            
        }

        public CarHistoryEntry(string userId, Car car, CarRentRequest carRentRequest)
        {
            UserId = userId;
            CarID = car.Id;
            StartDate = carRentRequest.RentFrom;
            EndDate = carRentRequest.RentTo;
        }
    }
}