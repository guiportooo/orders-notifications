using System;

namespace OrdersNotifications.Library.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime NotifyAt { get; set; }
        public DateTime? NotifiedAt { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public bool Notified => NotifiedAt != null;
        public bool ShouldNotify(DateTime now) => !Notified && NotifyAt <= now;

        public Notification(string to, string subject, string body, DateTime notifyAt)
        {
            To = to;
            Subject = subject;
            Body = body;
            NotifyAt = notifyAt;
        }
    }
}