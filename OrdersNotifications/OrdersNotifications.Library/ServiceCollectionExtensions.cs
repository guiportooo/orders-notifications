using Microsoft.Extensions.DependencyInjection;
using OrdersNotifications.Library.Queues;

namespace OrdersNotifications.Library
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureQueue(this IServiceCollection services,
            string connectionString)
        {
            services.AddSingleton(new QueueConfig(connectionString));
            services.AddSingleton<ICloudQueueClientFactory, CloudQueueClientFactory>();
            services.AddTransient<IQueueCommunicator, QueueCommunicator>();
            return services;
        }
    }
}