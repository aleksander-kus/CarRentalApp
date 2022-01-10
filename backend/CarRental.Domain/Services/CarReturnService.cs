using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Entity;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class CarReturnService
    {
        private readonly ICarReturnEntryRepository _carReturnRepository;

        public CarReturnService()
        {
            
        }
        
        public CarReturnService(ICarReturnEntryRepository carReturnRepository)
        {
            _carReturnRepository = carReturnRepository;
        }

        public async Task RegisterCarReturnAsync(string carId, int historyEntryId, CarReturnRequest request)
        {
            await _carReturnRepository.AddReturnEntryAsync(new CarReturnEntry()
            {
                PdfFileId = request.PdfFileId,
                PhotoFileId = request.PhotoFileId,
                OdometerValue = request.OdometerValue,
                CarCondition = request.CarCondition,
                CarId = carId,
                RentId = request.RentId,
                RentDate = request.RentDate,
                ReturnDate = request.ReturnDate,
                UserEmail = request.UserEmail,
                HistoryEntryId = historyEntryId
            });
        }

        public async Task<CarReturnEntry> GetReturnEntryAsync(int historyEntryId)
        {
            return await _carReturnRepository.GetReturnEntryAsync(historyEntryId);
        }
    }
}