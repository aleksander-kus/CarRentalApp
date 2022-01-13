using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using CarRental.Domain.Ports.Out;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using CarRental.Domain.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Graph;

namespace CarRental.Infrastructure.Adapters
{
    public class BlobStorageClient : IStorageClient
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorageClient(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }
        
        public async Task<List<string>> ListFilesAsync()
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

        public async Task<bool> GetFileAsync(string fileName, string destinationPath)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            BlobSasBuilder b = new BlobSasBuilder(BlobSasPermissions.Read, DateTimeOffset.Now.AddHours(1))
                {
                    BlobContainerName = blobContainerClient.Name,
                    BlobName = blobClient.Name
                };
            var x = blobClient.GenerateSasUri(b);
            var response = await blobClient.DownloadToAsync(destinationPath);
            return response.Status == StatusCodes.Status200OK;
        }

        public async Task<bool> IsFileAsync(string fileName)
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

        public async Task<bool> UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blob = blobContainerClient.GetBlobClient(fileName);
            var result = await blob.UploadAsync(fileStream);
            fileStream.Close();
            return true;
        }

        public async Task<string> GetFileSasAsync(string fileName, DateTimeOffset expirationDate)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blob = blobContainerClient.GetBlobClient(fileName);
            var builder = new BlobSasBuilder(BlobSasPermissions.Read, expirationDate);
            return blob.GenerateSasUri(builder).AbsoluteUri;
        }
    }
}