using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;

namespace CarRental.Domain.Ports.In
{
    public interface IGetSasUriUseCase
    {
        Task<string> GetFileSasAsync(string fileName, DateTimeOffset expirationDate);
    }
}