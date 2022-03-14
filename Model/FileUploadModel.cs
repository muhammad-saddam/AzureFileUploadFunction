using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileAPI.Model
{
   public class FileUploadModel
    {
        public string Id { get; set; }
        public IFormFile File { get; set; }
        public string FileName { get; set; }
    }
}
