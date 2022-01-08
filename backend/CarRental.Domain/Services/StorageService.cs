using System.Threading.Tasks;
using CarRental.Domain.Ports.Out;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Services
{
    public class StorageService
    {
        private IStorageClient _storageClient;

        public StorageService(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<bool> ExistsFile(string fileName)
        {
            var result = await _storageClient.IsFile(fileName);
            return result;
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName)
        {
            var result = await _storageClient.UploadFile(file, fileName);
            return result;
        }
        
    }
}