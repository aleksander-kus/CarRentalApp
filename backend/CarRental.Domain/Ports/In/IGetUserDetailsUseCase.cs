using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IGetUserDetailsUseCase
    {
        Task<UserDetails> GetUserDetailsAsync(string userId);
    }
}