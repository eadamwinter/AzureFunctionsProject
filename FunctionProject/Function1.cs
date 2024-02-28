using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionProject.myservice;
using Azure.Storage.Blobs;
using System.Linq;

namespace FunctionProject
{

    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");


            var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=True;");
            var images = blobServiceClient.GetBlobContainerClient("images").GetBlobs();

            var imageNames = images.Select(e => e.Name).ToList();
            var result = JsonConvert.SerializeObject(imageNames);

            return new OkObjectResult(result);
        }
    }
}
