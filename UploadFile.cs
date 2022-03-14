using System;
using System.IO;
using System.Threading.Tasks;
using FileAPI.Model;
using FileAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FileAPI
{
    public  class UploadFile
    {
        private readonly IFileService _fileService;
        public UploadFile(IFileService fileService)
        {
            _fileService = fileService;
        }


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
    }
}

