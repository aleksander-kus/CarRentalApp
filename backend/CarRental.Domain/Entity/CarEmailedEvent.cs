namespace CarRental.Domain.Entity
{
    public class CarEmailedEvent
    {
        public long ID { get; set; }
        public string ProviderID { get; set; }
        public string CarID { get; set; }
    }
}