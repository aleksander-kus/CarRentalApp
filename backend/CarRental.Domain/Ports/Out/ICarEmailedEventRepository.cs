using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Entity;

namespace CarRental.Domain.Ports.Out
{
    public interface ICarEmailedEventRepository
    {
        Task AddEmailedAsync(CarEmailedEvent emailedEvent);

        Task<List<CarEmailedEvent>> GetAllAsync();
    }
}