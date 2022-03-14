# AzureFileUploadFunction
This is HTTP triggered Azure Function is used to upload any types of files in azure blob storage under id folder provided in Post request.

**Limitation:**
Upload file should be less than 200 MB with any type of extension.
add azure storage account connection string in front of storage_account_connectionstring in appsettings.json
```
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "storage_account_connectionstring": ""
  }
}

```
**UploadFile Function** 
```
 [FunctionName("UploadFile")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function UploadFile processed a request.");
            try
            {
                DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
                var file = req.Form.Files["file"];
                if (file == null)
                {
                    throw new NullReferenceException("Please choose a file.");
                }
                var test = (file.Length / (1024 * 1024));
                if ((file.Length/ (1024 * 1024)) > 100)
                {
                    throw new NullReferenceException("File size should be less than 100 MB.");
                }
                FileUploadModel model = new FileUploadModel();
                model.File = file;
                model.Id = req.Form["Id"];
                model.FileName = Path.GetFileNameWithoutExtension(file.FileName) + "-" + now.ToString("yyyyMMddHHmmssfff")+ Path.GetExtension(file.FileName);
                var result = await _fileService.UploadFile(model);
                log.LogInformation("UploadFile HTTP triggered function executed successfully.");
                return new OkObjectResult(result);
            }
            catch (System.Exception ex)
            {
                log.LogInformation("UploadFileFuntion HTTP triggered function execution failed.");
                return new BadRequestObjectResult(ex);
            }
        }
```
**Upload Blob Method:**
```
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
