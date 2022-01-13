using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using Microsoft.AspNetCore.Http;

namespace CarRental.Domain.Ports.Out
{
    public interface IStorageClient
    {
        Task<List<string>> ListFilesAsync();
        Task<bool> GetFileAsync(string fileName, string destinationPath);
        Task<bool> IsFileAsync(string fileName);
        Task<bool> UploadFileAsync(Stream fileStream, string fileName);
        Task<string> GetFileSasAsync(string fileName, DateTimeOffset expirationDate);
    }
}