using FileAPI.Model;
using FileAPI.Services.BlobService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileAPI.Services
{
    public class FileService : IFileService
    {
        private readonly IBlobService _blobService;
        public FileService(IBlobService blobService)
        {
            _blobService = blobService;

        }
        public async Task<string> UploadFile(FileUploadModel fileUploadModel)
        {
          return await _blobService.UploadBlob(fileUploadModel);
        }
    }
}
