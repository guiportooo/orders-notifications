using System;
using System.Collections.Generic;
using System.Linq;
using OrdersNotifications.Library.Entities;

namespace OrdersNotifications.Api.Models
{
    public class OrderCreated
    {
        public int Id { get; }
        public DateTime Date { get; }
        public IEnumerable<OrderItem> Items { get; }
        public IEnumerable<OrderNotification> Notifications { get; set; }

        public OrderCreated(Order order)
        {
            if(order == null)
                throw new ArgumentNullException(nameof(order));
            
            Id = order.Id;
            Date = order.Date;
            Items = order.Items
                .Select(x =>
                    new OrderItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    });
            Notifications = order.Notifications
                .Select(x =>
                    new OrderNotification
                    {
                        To = x.To,
                        Notified = x.Notified
                    });
        }
    }
}