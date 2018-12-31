using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ImageManager.API.Helpers;
using ImageManager.API.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageManager.API.Services
{
    public class ImageService : IImageService
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly string _containerName;

        public ImageService(IOptions<AppSettings> appSettings)
        {
            var appSetting = appSettings.Value;
            // getting the container name from settings file (appsettings.json)
            _containerName = appSetting.AzureAccountContainer;

            // getting the connection string from settings file (appsettings.json)
            // and using Azure SDK to open the connection & intialize its API
            _storageAccount = CloudStorageAccount.Parse(appSetting.AzureAccountConnString);
        }

        public async Task<CloudBlockBlob> UploadImageAsync(string imageName, Stream stream)
        {
            // creating a client for the image file
            var imageClient = _storageAccount.CreateCloudBlobClient();

            // getting the container
            var container = imageClient.GetContainerReference(_containerName.ToLower());

            // creating a reference for the image (blob) for saving
            var blockBlob = container.GetBlockBlobReference(imageName);
            try
            {
                // uploading the file stream to azure using Azure SDK
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob;
            }
            catch (Exception e)
            {
                var r = e.Message;
                return null;
            }
        }

        public CloudBlockBlob DownloadImage(string imageName)
        {
            var imageClient = _storageAccount.CreateCloudBlobClient();
            var container = imageClient.GetContainerReference(_containerName);
            var blockBlob = container.GetBlockBlobReference(imageName);
            return blockBlob;
        }

        public async Task<List<IListBlobItem>> ListImagesAsync()
        {

            BlobContinuationToken continuationToken = null;
            var imageClient = _storageAccount.CreateCloudBlobClient();
            var container = imageClient.GetContainerReference(_containerName);
            var results = new List<IListBlobItem>();
            do
            {
                // reading the container's blob list with segmentaion incase there was more than 5000 file
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;

                // adding the result into our result list
                results.AddRange(response.Results);
            }
            while (continuationToken != null); // keep reading as long as there was still remaining items
            return results;
        }

        public async Task<bool> DeleteImageAsync(string imageName)
        {

            var imageClient = _storageAccount.CreateCloudBlobClient();
            var container = imageClient.GetContainerReference(_containerName);
            var blockBlob = container.GetBlockBlobReference(imageName);
            await blockBlob.DeleteAsync();
            return true;
        }

        public async Task<bool> DeleteAllImageAsync()
        {

            BlobContinuationToken continuationToken = null;
            var imageClient = _storageAccount.CreateCloudBlobClient();
            var container = imageClient.GetContainerReference(_containerName);
            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                // Azure SDK does't allow patch delete (as far as i could tell) so we have to do it
                // for each item, and using Parallel to speed up the process utilizing threading
                Parallel.ForEach(response.Results, x => ((CloudBlockBlob)x).DeleteAsync());
            }
            while (continuationToken != null);

            return true;
        }
    }
}