using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using CarRental.Domain.Ports.Out;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Graph;

namespace CarRental.Infrastructure.Adapters
{
    public class BlobStorageClient : IStorageClient
    {
        private string _connectionString;
        private string _containerName;

        public BlobStorageClient(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }
        
        public async Task<List<string>> ListFiles()
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var resultFiles = new List<string>();
            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                resultFiles.Add(blobItem.Name);
            }
            return resultFiles;
        }

        public async Task<bool> GetFile(string fileName, string destinationPath)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            var response = await blobClient.DownloadToAsync(destinationPath);
            return response.Status == StatusCodes.Status200OK;
        }

        public async Task<bool> IsFile(string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var result = false;
            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                if (blobItem.Name != fileName) continue;
                result = true;
                break;
            }
            return result;
        }

        public async Task<bool> UploadFile(IFormFile file, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blob = blobContainerClient.GetBlobClient(fileName);
            var result = await blob.UploadAsync(file.OpenReadStream());
            return true;

        }

    }
}