using System;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Entity
{
    public class CarHistoryEntry
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string CarId { get; set; }
        public string ProviderId { get; set; }
        public string CarModel { get; set; }
        public string CarBrand { get; set; }
        public string CarProvider { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsRentConfirmed { get; set; }
        public string PriceId { get; set; }
        public string RentId { get; set; }
        public bool Returned { get; set; } = false;
        
        public CarHistory ToDto()
        {
            return new CarHistory()
            {
                Id = ID,
                CarBrand = CarBrand,
                CarModel = CarModel,
                CarProvider = CarProvider,
                StartDate = StartDate,
                EndDate = EndDate,
                UserEmail = UserEmail
            };
        }
    }
}