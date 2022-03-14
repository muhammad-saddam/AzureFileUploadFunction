using FileAPI.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileAPI.Services
{
  public  interface IFileService
    {
        Task<string> UploadFile(FileUploadModel fileUploadModel);
    }
}
