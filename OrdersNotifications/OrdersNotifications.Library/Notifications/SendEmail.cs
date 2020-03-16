namespace OrdersNotifications.Library.Notifications
{
    public class SendEmail
    {
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public SendEmail(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}