namespace OrdersNotifications.Library.Queues.Messages
{
    public class SendEmailCommand : BaseQueueMessage
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public SendEmailCommand()
            : base(QueueNames.EmailBox)
        {
        }

        public SendEmailCommand(int id, string to, string subject, string body)
            : base(QueueNames.EmailBox)
        {
            Id = id;
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}