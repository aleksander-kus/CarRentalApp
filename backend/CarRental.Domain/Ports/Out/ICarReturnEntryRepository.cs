using System.Threading.Tasks;
using CarRental.Domain.Entity;

namespace CarRental.Domain.Ports.Out
{
    public interface ICarReturnEntryRepository
    {
        Task AddReturnEntryAsync(CarReturnEntry carReturnEntry);
        Task<CarReturnEntry> GetReturnEntryAsync(int historyEntryId);
    }
}