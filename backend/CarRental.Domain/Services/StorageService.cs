using System;
using System.IO;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Services
{
    public class StorageService : IGetSasUriUseCase, IUploadFileUseCase
    {
        private readonly IStorageClient _storageClient;

        public StorageService()
        {
            
        }
        
        public StorageService(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<bool> ExistsFile(string fileName)
        {
            var result = await _storageClient.IsFileAsync(fileName);
            return result;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            fileName = Guid.NewGuid() + fileName;
            await _storageClient.UploadFileAsync(fileStream, fileName);
            return fileName;
        }

        public async Task<string> GetFileSasAsync(string fileName, DateTimeOffset expirationDate)
        {
            return await _storageClient.GetFileSasAsync(fileName, expirationDate);
        }
    }
}