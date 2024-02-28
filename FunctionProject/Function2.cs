using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FunctionProject.myservice;
using MediatR;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionProject
{
    public class Function2
    {
        public const string ConnectionString = "AzureWebJobsStorage";
        public const string BlobContainerName = "%BlobContainerName%";
        public const string QueueName = "%QueueName%";

        public Function2(IMessageValidator messageValidator, IMediator mediator)
        {
            MessageValidator = messageValidator;
            Mediator = mediator;
        }

        public IMessageValidator MessageValidator { get; }
        public IMediator Mediator { get; }

        [FunctionName("Function2")]
        public async Task Run([QueueTrigger(QueueName, Connection = ConnectionString)] string myQueueItem, ILogger log,
            [Blob(BlobContainerName, FileAccess.Read, Connection = ConnectionString)] CloudBlobContainer blobContainer)
        {

            var result = MessageValidator.Validate(myQueueItem);
            log.LogWarning($"pytanie: czy mozna przesylac dalej message? : '{result}'");
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");



            // proba mediatr
            var command = new ValidateMessageCommand(myQueueItem);
            var isValid = await Mediator.Send(command);
            log.LogCritical($"czy to jest to : {isValid}");


            var blobName = GetNewBlobName();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
 
            // sprawdz czy jest to potrzebne
            blob.Properties.ContentType = "application/json";

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
