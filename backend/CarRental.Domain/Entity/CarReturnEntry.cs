using System;

namespace CarRental.Domain.Entity
{
    public class CarReturnEntry
    {
        public long ID { get; set; }
        public string RentId { get; set; }
        public string CarId { get; set; }
        public int HistoryEntryId { get; set; }
        public string PhotoFileId { get; set; }
        public string PdfFileId { get; set; }
        public double OdometerValue { get; set; }
        public string CarCondition { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string UserEmail { get; set; }
    }
}