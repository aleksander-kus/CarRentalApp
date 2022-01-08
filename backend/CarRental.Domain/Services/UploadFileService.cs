using System;
using System.Threading.Tasks;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Services
{
    public class UploadFileService : IUploadFileUseCase
    {
        private readonly StorageService _storageService;

        public UploadFileService(StorageService storageService)
        {
            _storageService = storageService;
        }
        
        public async Task<string> UploadFile(IFormFile file)
        {
            var fileName = Guid.NewGuid() + file.FileName;
            var uploadResult = await _storageService.UploadFile(file, fileName);
            return fileName;
        }
    }
}