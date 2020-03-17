using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrdersNotifications.Library.Queues;
using OrdersNotifications.Library.Queues.Handlers;
using OrdersNotifications.Library.Queues.Messages;

namespace OrdersNotifications.Library.Notifications
{
    public interface INotificationsService
    {
        Task QueuePendingNotifications();
        Task SendEmailAsync(string message);
    }

    public class NotificationsService : INotificationsService
    {
        private readonly IQueueCommunicator _queueCommunicator;
        private readonly ISendEmailCommandHandler _handler;
        private readonly HttpClient _httpClient;
        private readonly NotificationsApiSettings _notificationsApiSettings;

        public NotificationsService(IQueueCommunicator queueCommunicator,
            ISendEmailCommandHandler handler,
            HttpClient httpClient,
            IOptions<NotificationsApiSettings> notificationsApiSettings)
        {
            _queueCommunicator = queueCommunicator;
            _handler = handler;
            _httpClient = httpClient;
            _notificationsApiSettings = notificationsApiSettings.Value;
        }

        public async Task QueuePendingNotifications()
        {
            var response = await _httpClient.GetAsync(_notificationsApiSettings.Path);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<IEnumerable<PendingNotification>>(content);

            var commands = notifications
                .Select(x => new SendEmailCommand(x.Id,
                    x.To,
                    x.Subject,
                    x.Body));

            foreach (var command in commands)
                await _queueCommunicator.SendAsync(command);
        }

        public async Task SendEmailAsync(string message)
        {
            var command = _queueCommunicator.Read<SendEmailCommand>(message);
            await _handler.Handle(command);
        }
    }
}