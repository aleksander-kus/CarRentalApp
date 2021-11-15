using System;

namespace CarRental.Domain.Entity
{
    public class Car
    {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        public int ProductionYear { get; set; }
        public int Capacity { get; set; }
    }
}