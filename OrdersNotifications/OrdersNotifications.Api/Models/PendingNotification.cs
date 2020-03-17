using OrdersNotifications.Library.Entities;

namespace OrdersNotifications.Api.Models
{
    public class PendingNotification
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public PendingNotification()
        {
            
        }
        
        public PendingNotification(Notification notification)
        {
            Id = notification.Id;
            To = notification.To;
            Subject = notification.Subject;
            Body = notification.Body;
        }
    }
}