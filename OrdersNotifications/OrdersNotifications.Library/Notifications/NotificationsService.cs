using System.Threading.Tasks;
using OrdersNotifications.Library.Queues;
using OrdersNotifications.Library.Queues.Handlers;
using OrdersNotifications.Library.Queues.Messages;

namespace OrdersNotifications.Library.Notifications
{
    public interface INotificationsService
    {
        Task SendEmailAsync(string message);
    }

    public class NotificationsService : INotificationsService
    {
        private readonly IQueueCommunicator _queueCommunicator;
        private readonly ISendEmailCommandHandler _handler;

        public NotificationsService(IQueueCommunicator queueCommunicator,
            ISendEmailCommandHandler handler)
        {
            _queueCommunicator = queueCommunicator;
            _handler = handler;
        }

        public async Task SendEmailAsync(string message)
        {
            var command = _queueCommunicator.Read<SendEmailCommand>(message);
            await _handler.Handle(command);
        }
    }
}