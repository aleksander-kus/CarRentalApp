using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.Out
{
    public interface IUserRepository
    {
        Task<UserDetails> GetUserDetailsAsync(string userId);

        Task<List<string>> GetAllEmailsAsync();
    }
}