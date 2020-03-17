# Orders Notifications

A POC with Azure Functions simulating notifications being sent after new orders are created.

## Flow

![alt text](https://github.com/guiportooo/orders-notifications/raw/master/Diagram.png "Diagram")

1. A new order is created via API call
2. Two pending notifications related to the new order are saved to the database
2. A timer triggered function executing every 2 minutes calls the API to retrieve pending notifications
3. Pending notifications are queued
4. A queue triggered function calls API for each new queued pending notification
5. Each notification is updated as notified on the database

## Setup

### Dependencies

- [.Net SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [EF Core Tools](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)
- [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)
- Azure Storage Emulator(If not on Windows, one option is [Azurite](https://github.com/Azure/Azurite))

### Migrations

The project uses Entity Framework Core 3.1 with SQLite.

To apply the migrations, execute the following command inside the ***OrdersNotifications.Library*** folder:

```
dotnet ef database update
```

### Running

To run the funcitons locally, create a file ***OrdersNotifications/OrdersNotifications.Functions/local.settings.json*** with the content:

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    },
    "NotificationsApi": {
        "Host": "http://localhost:5000/",
        "Path": "notifications"
    }
}
```

Azure Storage Emulator or Azurite must be running.

The projects ***OrdersNotifications.Api*** and ***OrdersNotifications.Functions*** muts be both running.