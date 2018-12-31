using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageManager.API.Interfaces
{
    public interface IImageService
    {
        CloudBlockBlob DownloadImage(string imageName);
        Task<CloudBlockBlob> UploadImageAsync(string imageName, Stream stream);
        Task<List<IListBlobItem>> ListImagesAsync();
        Task<bool> DeleteImageAsync(string imageName);
        Task<bool> DeleteAllImageAsync();
    }
}