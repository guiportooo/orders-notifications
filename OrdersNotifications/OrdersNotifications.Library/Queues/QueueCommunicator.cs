using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using OrdersNotifications.Library.Queues.Messages;

namespace OrdersNotifications.Library.Queues
{
    public interface IQueueCommunicator
    {
        T Read<T>(string message);
        Task SendAsync<T>(T obj) where T : BaseQueueMessage;
    }

    public class QueueCommunicator : IQueueCommunicator
    {
        private readonly ICloudQueueClientFactory _cloudQueueClientFactory;

        public QueueCommunicator(ICloudQueueClientFactory cloudQueueClientFactory)
            => _cloudQueueClientFactory = cloudQueueClientFactory;

        public T Read<T>(string message)
            => JsonConvert.DeserializeObject<T>(message);

        public async Task SendAsync<T>(T obj) where T : BaseQueueMessage
        {
            try
            {
                var queueReference = _cloudQueueClientFactory
                    .GetClient()
                    .GetQueueReference(obj.QueueName);

                await queueReference.CreateIfNotExistsAsync();

                var message = JsonConvert.SerializeObject(obj);
                var queueMessage = new CloudQueueMessage(message);
                await queueReference.AddMessageAsync(queueMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}