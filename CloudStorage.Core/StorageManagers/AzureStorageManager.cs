
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using CloudStorage.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using CloudStorage.Core.Services;

namespace CloudStorage.Core
{
    public class AzureStorageManager : IAzureStorageManager
    {
        private readonly BlobContainerClient _containerClient;

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public string GetStorageName() => "Azure";

        public AzureStorageManager(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
            _containerClient = new BlobServiceClient(_configuration.GetSection("Azure:StorageConnectionString").Value)
                               .GetBlobContainerClient(_configuration.GetSection("Azure:ContainerName").Value);
        }

        public async Task<string> DownloadAzureAsync(string photoGuid)
        {
            string fileName = FileExtensionExtractor.ExtractLastSubstring(photoGuid, "/");
            string pathResult = $"{_environment.WebRootPath}/{fileName}";

            BlobClient blobClient = _containerClient.GetBlobClient(fileName);

            if (!File.Exists(pathResult))
            {
                using FileStream fs = new(pathResult, FileMode.Create);
                await blobClient.DownloadToAsync(fs);
            }
            return $"{EnvironmentManager.GetApplicationHost()}/{fileName}";
        }


        public async Task<string> UploadAzureAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + FileExtensionExtractor.Extract(file.FileName);
            var stream = await SetMemoryStream(file);

            BlobClient _blobClient = _containerClient.GetBlobClient(fileName);
            await _blobClient.UploadAsync(stream, true);

            return _blobClient.Uri.ToString();
        }

        private async Task<MemoryStream> SetMemoryStream(IFormFile file)
        {
            var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            stream.Position = 0;
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}

