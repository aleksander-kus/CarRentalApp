using System.IO;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Ports.In
{
    public interface IUploadFileUseCase
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
    }
}