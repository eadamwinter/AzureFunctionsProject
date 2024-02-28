using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FunctionProject.myservice;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionProject
{
    public class Function2
    {
        public Function2(IServiceProvider serviceProvider, IProba proba)
        {
            ServiceProvider = serviceProvider;
            Proba = proba;
        }

        public IServiceProvider ServiceProvider { get; }
        public IProba Proba { get; }

        [FunctionName("Function2")]
        public async Task Run([QueueTrigger("functionproject-queue1", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log,
            [Blob("images", FileAccess.Read, Connection = "AzureWebJobsStorage")] CloudBlobContainer blobContainer)
        {

            var result = Proba.GetCos();
            log.LogWarning($"no i teraz  tak : dependency bitches : '{result}'");
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");


            
            // walidacja jsona
            var file = JsonConvert.SerializeObject(myQueueItem);


            string blobName = GetNewBlobName();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
 
            // sprawdz czy jest to potrzebne
            blob.Properties.ContentType = "application/json";

            //use Upload method for stream

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(myQueueItem)))
            {
                await blob.UploadFromStreamAsync(ms);
            }
        }

        private string GetNewBlobName()
        {
            var currentDate = DateTime.Now;
            var newGuid = Guid.NewGuid();
            var name = $"{currentDate.Year}/{currentDate.Month}/{currentDate.Day}/{currentDate.Hour}/{currentDate.Minute}/{newGuid}.json";
            return name;
        }
    }
}
