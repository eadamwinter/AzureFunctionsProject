using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FunctionProject.Services;
using MediatR;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionProject.Functions;

public class ProcessQueueMessage
{
    public const string ConnectionString = "AzureWebJobsStorage";
    public const string BlobContainerName = "%BlobContainerName%";
    public const string QueueName = "%QueueName%";

    public ProcessQueueMessage(IMessageValidator messageValidator, IMediator mediator)
    {
        MessageValidator = messageValidator;
        Mediator = mediator;
    }

    public IMessageValidator MessageValidator { get; }
    public IMediator Mediator { get; }

    [FunctionName("ProcessQueueMessage")]
    public async Task Run([QueueTrigger(QueueName, Connection = ConnectionString)] string myQueueItem, ILogger log,
        [Blob(BlobContainerName, FileAccess.Read, Connection = ConnectionString)] CloudBlobContainer blobContainer)
    {
        log.LogInformation($"Processing message: {myQueueItem}");

        var command = new ValidateMessageCommand(myQueueItem);
        var result = await Mediator.Send(command);

        if (result.IsValid)
        {
            await AddNewBlob(blobContainer, myQueueItem);
            log.LogInformation("New blob has been added.");
        }
        else
        {
            log.LogInformation($"Error: {result.ErrorMessage}");
            log.LogInformation("Cannot add new blob. Message invalid.");
        }
    }

    private async Task AddNewBlob(CloudBlobContainer blobContainer, string message)
    {
        var blobName = GetNewBlobName();
        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);

        blob.Properties.ContentType = "application/json";

        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(message)))
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
