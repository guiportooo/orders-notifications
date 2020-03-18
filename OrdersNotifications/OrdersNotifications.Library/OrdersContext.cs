using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using OrdersNotifications.Library.Entities;

namespace OrdersNotifications.Library
{
    public class OrdersContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public OrdersContext()
        {
        }

        public OrdersContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
            => options.UseSqlite("DataSource=orders.db");
    }
}