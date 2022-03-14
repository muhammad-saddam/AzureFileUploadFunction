using FileAPI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileAPI.Services.BlobService
{
   public interface IBlobService
    {
        Task<string> UploadBlob(FileUploadModel model);
    }
}
