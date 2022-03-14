using FileAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static FileAPI.Constants.Configuration;

namespace FileAPI.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly string _containerName = "patient-files";
        private readonly string _blobStorageConnectionString = "";
        public BlobService(IConfiguration configuration)
        {
            this._blobStorageConnectionString = configuration[ConfigurationParameters.storage_account_connectionstring.ToString()];
        }
        public async Task<string> UploadBlob(FileUploadModel fileUploadModel)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_blobStorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_containerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"{fileUploadModel.Id}/{fileUploadModel.FileName}");

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (fileUploadModel.File.FileName != null)
            {
                await cloudBlockBlob.UploadFromStreamAsync(fileUploadModel.File.OpenReadStream());
            }
            return cloudBlockBlob.Uri.ToString();
        }
    }
}
