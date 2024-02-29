# About

This .net based Azure Functions application consists of two Azure Functions. It is designed for developer purposes using azurite (Azure Storage Emulator).


# Functions

1. Queue-Triggered Function **ProcessQueueMessage**  

This function is triggered by every new message in a specified Azure Storage Queue. Each message will be checked and if valid saved as a new file in a specified Azure Blob Storage container. The file path will follow the pattern: *year/month/day/hour/minute/{guid}.json*.

2. Http-Triggered Function **ListBlobs**  

This function will respond to HTTP GET requests by returning a list (*json array containing file names*) of all files currently stored in the specified Azure Blob Storage container.

# Configuration

After cloning the repo, please copy the file *local.settings.template.json* and save it as local.settings.json. If you want you can change some parameters. If working locally with azurite leave *AzureWebJobsStorage* value as  *UseDevelopmentStorage=true*

- Make sure that your local azurite consists of queue with name specified by *QueueName* value
- Make sure that your local azurite consists of blob container specified by *BlobContainerName* value

**The application by default works on 

# Starting the application

1. Open application folder in Visual Studio or any other IDE.
2. Prepare your local.settings.json file.
3. Connect to azurite (for example using Azure Storage Explorer) and make sure about queue and blob container mentioned in Configuration section.
4. Run application.
5. Function names and link to ListBlobs function should be visible on the console.

To use HTTP triggered function go to browser and type http://localhost:{port}/api/ListBlobs.  
To use Queue triggered function go to azure storage explorer and add new sensor message to specified queue.



# Queue Messages

Messages are information provided by sensors. Each consists of designation, location and battery level fields. When added to the queue, message will be consumed by the function and before saving to blob storage, it has 
to pass following validations: 

- Designation is a string and must have the length of 6, 
- Location is an enum that accepts strings such as Kitchen, Hall or LivingRoom,
- Battery level is an integer and has to be of range from 0 to 100 

**Not valid messages will not be save in the blob storage. Error messages will be visible as logs in the output of the application.**

Example valid message:

> {  
>"Location": "Kitchen",  
>"Designation": "sen110",  
>"BatteryLevel": 25  
>}     
