using System;
using Microsoft.Extensions.DependencyInjection;
using OrdersNotifications.Library.Notifications;
using OrdersNotifications.Library.Queues;
using OrdersNotifications.Library.Queues.Handlers;

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

        public static IServiceCollection AddAzureQueue(this IServiceCollection services,
            string connectionString,
            string notificationsApiHost)
        {
            services.AddAzureQueue(connectionString);
            services.AddHttpClient<ISendEmailCommandHandler, SendEmailCommandHandler>(client
                => client.BaseAddress = new Uri(notificationsApiHost));
            services.AddHttpClient<INotificationsService, NotificationsService>(client
                => client.BaseAddress = new Uri(notificationsApiHost));
            return services;
        }
    }
}