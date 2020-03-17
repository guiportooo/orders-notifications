using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrdersNotifications.Api.Models;
using OrdersNotifications.Library;
using OrdersNotifications.Library.Entities;
using OrdersNotifications.Library.Queues;

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
                .Include(x => x.Notifications)
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
            order.Notifications = MapNotifications(order.Id, order.UserEmail);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return new OrderCreated(order);
        }

        private List<Notification> MapNotifications(int orderId, string userEmail)
        {
            var clientNotification = new Notification(userEmail,
                "Thank you!",
                "Your order is being processed.",
                DateTime.Now);

            var adminNotification = new Notification(_emailSettings.AdminEmail,
                "Order created!",
                $"New order with id {orderId} created by {userEmail}",
                DateTime.Now.AddMinutes(2));
            
            return new List<Notification>
            {
                clientNotification, adminNotification
            };
        }
    }
}