using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrdersNotifications.Library.Notifications;

namespace OrdersNotifications.Functions
{
    public static class PendingNotificationsTimerTrigger
    {
        [FunctionName("PendingNotificationsTimerTrigger")]
        public static async Task RunAsync(
            [TimerTrigger("0 */2 * * * *")] TimerInfo timer,
            ILogger log)
        {
            try
            {
                var notificationsService = ServiceProviderBuilder
                    .Instance
                    .GetService<INotificationsService>();
                await notificationsService.QueuePendingNotifications();
                log.LogInformation($"PendingNotificationsTimerTrigger function executed at: {DateTime.UtcNow}");
            }
            catch (Exception e)
            {
                log.LogError(e, $"Error queueing pending notifications at: {DateTime.UtcNow}");
                throw;
            }
        }
    }
}