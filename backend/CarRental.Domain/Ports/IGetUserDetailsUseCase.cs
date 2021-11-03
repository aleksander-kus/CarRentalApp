using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports
{
    public interface IGetUserDetailsUseCase
    {
        Task<UserDetails> GetUserDetails(string userId);
    }
}