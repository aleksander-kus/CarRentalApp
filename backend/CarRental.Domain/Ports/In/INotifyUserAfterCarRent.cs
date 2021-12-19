using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface INotifyUserAfterCarRent
    {
        Task NotifyUserAfterCarRent(UserDetails userDetails, CarRentRequest carRentRequest);
    }
}