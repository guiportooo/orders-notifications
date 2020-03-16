using System;
using System.Collections.Generic;

namespace OrdersNotifications.Library.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime Date { get; set; }
        public List<Item> Items { get; set; }

        public Order()
        {
            Items = new List<Item>();
        }
    }
}