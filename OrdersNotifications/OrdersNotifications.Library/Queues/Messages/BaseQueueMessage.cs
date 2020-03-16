namespace OrdersNotifications.Library.Queues.Messages
{
    public abstract class BaseQueueMessage
    {
        public string QueueName { get; }

        protected BaseQueueMessage(string queueName) => QueueName = queueName;
    }
}