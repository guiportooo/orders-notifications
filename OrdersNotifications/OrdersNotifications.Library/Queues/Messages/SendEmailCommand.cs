namespace OrdersNotifications.Library.Queues.Messages
{
    public class SendEmailCommand : BaseQueueMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public SendEmailCommand()
            : base(QueueNames.EmailBox)
        {
        }

        public SendEmailCommand(string to, string subject, string body)
            : base(QueueNames.EmailBox)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}