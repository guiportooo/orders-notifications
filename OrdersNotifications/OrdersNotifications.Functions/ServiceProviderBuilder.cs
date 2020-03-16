using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersNotifications.Library;
using OrdersNotifications.Library.Notifications;

namespace OrdersNotifications.Functions
{
    public sealed class ServiceProviderBuilder
    {
        private static readonly IServiceProvider _instance = Build();
        public static IServiceProvider Instance = _instance;
        
        static ServiceProviderBuilder()
        {
            
        }

        private ServiceProviderBuilder()
        {
            
        }
        
        private static IServiceProvider Build()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var notificationsApi = configuration.GetSection("NotificationsApi");
            
            services.AddAzureQueue(configuration["AzureWebJobsStorage"], notificationsApi["Host"]);
            
            services.Configure<NotificationsApiSettings>(options
                => notificationsApi.Bind(options));

            services.AddTransient<INotificationsService, NotificationsService>();
            return services.BuildServiceProvider();
        }
    }
}