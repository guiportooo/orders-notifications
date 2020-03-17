namespace OrdersNotifications.Library.Notifications
{
    public class PendingNotification
    {
        public int Id { get; set; }
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public PendingNotification(int id, string to, string subject, string body)
        {
            Id = id;
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}