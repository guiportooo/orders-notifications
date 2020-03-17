using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrdersNotifications.Api.Models;
using OrdersNotifications.Library;

namespace OrdersNotifications.Api.Services
{
    public interface INotificationsService
    {
        Task<IReadOnlyCollection<PendingNotification>> GetAllPending();
        Task SendNotification(PendingNotification pendingNotification);
    }

    public class NotificationsService : INotificationsService
    {
        private readonly OrdersContext _context;

        public NotificationsService(OrdersContext context)
            => _context = context;

        public async Task<IReadOnlyCollection<PendingNotification>> GetAllPending()
        {
            var notifications = await _context.Notifications
                .Where(x => x.NotifiedAt == null)
                .ToListAsync();

            return notifications
                .Where(x => x.ShouldNotify(DateTime.Now))
                .Select(x => new PendingNotification(x))
                .ToList();
        }

        public async Task SendNotification(PendingNotification pendingNotification)
        {
            var notification = await _context.Notifications.FindAsync(pendingNotification.Id);

            if (notification == null)
                return;

            var now = DateTime.Now;

            if (!notification.ShouldNotify(now))
                return;

            notification.NotifiedAt = now;
            await _context.SaveChangesAsync();
        }
    }
}