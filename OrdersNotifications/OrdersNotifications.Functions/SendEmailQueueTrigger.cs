using System.Threading.Tasks;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrdersNotifications.Library.Notifications;
using OrdersNotifications.Library.Queues;

namespace OrdersNotifications.Functions
{
    public static class SendEmailQueueTrigger
    {
        [FunctionName("SendEmailQueueTrigger")]
        public static async Task RunAsync(
            [QueueTrigger(QueueNames.EmailBox, Connection = "AzureWebJobsStorage")]
            string message, 
            ILogger log)
        {
            try
            {
                var notificationsService = ServiceProviderBuilder
                    .Instance
                    .GetService<INotificationsService>();
                await notificationsService.SendEmailAsync(message);
                log.LogInformation($"SendEmailQueueTrigger function processed: {message}");
            }
            catch (Exception e)
            {
                log.LogError(e, $"Error processing message: {message}");
                throw;
            }
        }
    }
}