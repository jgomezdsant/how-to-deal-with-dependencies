using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage.Core.Services
{
    public interface IAzureStorageManager
    {
        Task<string> DownloadAzureAsync(string photoGuid);
        string GetStorageName();
        Task<string> UploadAzureAsync(IFormFile file);
    }
}
