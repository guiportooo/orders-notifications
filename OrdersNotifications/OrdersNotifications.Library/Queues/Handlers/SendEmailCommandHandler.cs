using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrdersNotifications.Library.Notifications;
using OrdersNotifications.Library.Queues.Messages;

namespace OrdersNotifications.Library.Queues.Handlers
{
    public interface ISendEmailCommandHandler
    {
        Task Handle(SendEmailCommand command);
    }
    
    public class SendEmailCommandHandler : ISendEmailCommandHandler
    {
        private readonly HttpClient _httpClient;
        private readonly NotificationsApiSettings _notificationsApiSettings;

        public SendEmailCommandHandler(HttpClient httpClient, 
            IOptions<NotificationsApiSettings> notificationsApiSettings)
        {
            _httpClient = httpClient;
            _notificationsApiSettings = notificationsApiSettings.Value;
        }

        public async Task Handle(SendEmailCommand command)
        {
            var notification = new PendingNotification(command.Id,
                command.To,
                command.Subject,
                command.Body);

            var content = JsonConvert.SerializeObject(notification);

            await _httpClient.PostAsync(_notificationsApiSettings.Path, 
                new StringContent(content, Encoding.UTF8, "application/json"));
        }
    }
}