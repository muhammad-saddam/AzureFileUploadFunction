using FileAPI.Services.BlobService;
using FileAPI.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
[assembly: FunctionsStartup(typeof(FileAPI.Startup))]
namespace FileAPI
{
   public class Startup : FunctionsStartup
    {
        public Startup()
        {

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IBlobService, BlobService>();

        }
    }
}
