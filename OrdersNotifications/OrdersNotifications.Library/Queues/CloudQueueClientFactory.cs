using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace OrdersNotifications.Library.Queues
{
    public interface ICloudQueueClientFactory
    {
        CloudQueueClient GetClient();
    }
    
    public class CloudQueueClientFactory : ICloudQueueClientFactory
    {
        private readonly QueueConfig _queueConfig;
        private CloudQueueClient _cloudQueueClient;
        
        public CloudQueueClientFactory(QueueConfig queueConfig) 
            => _queueConfig = queueConfig;

        public CloudQueueClient GetClient()
        {
            if (_cloudQueueClient != null)
                return _cloudQueueClient;

            var storageAccount = CloudStorageAccount.Parse(_queueConfig.ConnectionString);
            _cloudQueueClient = storageAccount.CreateCloudQueueClient();
            return _cloudQueueClient;
        }
    }
}