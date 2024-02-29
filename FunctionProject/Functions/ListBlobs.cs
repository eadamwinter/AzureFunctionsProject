using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Linq;
using System.Collections.Generic;

namespace FunctionProject.Functions;

public static class ListBlobs
{
    public const string ConnectionString = "AzureWebJobsStorage";
    public const string ContainerName = "BlobContainerName";

    [FunctionName("ListBlobs")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
    {
        try
        {
            var names = GetBlobNames();
            var result = JsonConvert.SerializeObject(names);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
        }

        return new NotFoundResult();

    }

    private static ICollection<string> GetBlobNames()
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionString);
        var blobContName = Environment.GetEnvironmentVariable(ContainerName);

        var blobServiceClient = new BlobServiceClient(connectionString);
        var blobNames = blobServiceClient.GetBlobContainerClient(blobContName).GetBlobs();

        return blobNames.Select(e => e.Name).ToList();
    }
}
