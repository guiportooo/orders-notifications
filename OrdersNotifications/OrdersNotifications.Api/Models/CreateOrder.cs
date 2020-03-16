using System;
using System.Collections.Generic;
using System.Linq;
using OrdersNotifications.Library.Entities;

namespace OrdersNotifications.Api.Models
{
    public class CreateOrder
    {
        public string UserEmail { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }

        public CreateOrder()
        {
            Items = new List<OrderItem>();
        }

        public Order MapToEntity()
            => new Order
            {
                UserEmail = UserEmail,
                Date = DateTime.Now,
                Items = Items
                    .Select(x => new Item
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = new Random().Next(0, 10000)
                    })
                    .ToList()
            };
    }
}