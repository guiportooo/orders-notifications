using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrdersNotifications.Api.Models;
using OrdersNotifications.Library;
using OrdersNotifications.Library.Queues;
using OrdersNotifications.Library.Queues.Messages;

namespace OrdersNotifications.Api.Services
{
    public interface IOrdersService
    {
        Task<IReadOnlyCollection<OrderCreated>> GetAllOrders();
        Task<OrderCreated> GetById(int id);
        Task<OrderCreated> Create(CreateOrder createOrder);
    }

    public class OrdersService : IOrdersService
    {
        private readonly OrdersContext _context;
        private readonly IQueueCommunicator _queueCommunicator;
        private readonly EmailSettings _emailSettings;

        public OrdersService(OrdersContext context,
            IQueueCommunicator queueCommunicator,
            IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _queueCommunicator = queueCommunicator;
            _emailSettings = emailSettings.Value;
        }

        public async Task<IReadOnlyCollection<OrderCreated>> GetAllOrders()
            => await _context
                .Orders
                .Include(x => x.Items)
                .Select(x => new OrderCreated(x))
                .ToListAsync();

        public async Task<OrderCreated> GetById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return new OrderCreated(order);
        }

        public async Task<OrderCreated> Create(CreateOrder createOrder)
        {
            var order = createOrder.MapToEntity();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await SendEmailsAsync(order.Id, order.UserEmail);
            return new OrderCreated(order);
        }

        private async Task SendEmailsAsync(int orderId, string userEmail)
        {
            var clientEmail = new SendEmailCommand(userEmail,
                "Thank you!",
                "Your order is being processed.");

            var adminEmail = new SendEmailCommand(_emailSettings.AdminEmail,
                "Order created!",
                $"New order with id {orderId} created by {userEmail}");

            await _queueCommunicator.SendAsync(clientEmail);
            await _queueCommunicator.SendAsync(adminEmail);
        }
    }
}