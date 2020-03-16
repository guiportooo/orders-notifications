namespace OrdersNotifications.Library.Queues
{
    public class QueueConfig
    {
        public string ConnectionString { get; set; }

        public QueueConfig()
        {
            
        }

        public QueueConfig(string connectionString) => ConnectionString = connectionString;
    }
}