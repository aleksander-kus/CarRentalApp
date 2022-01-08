using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Ports.Out
{
    public interface IStorageClient
    {
        Task<List<string>> ListFiles();
        Task<bool> GetFile(string fileName, string destinationPath);
        Task<bool> IsFile(string fileName);
        Task<bool> UploadFile(IFormFile file, string fileName);
    }
}