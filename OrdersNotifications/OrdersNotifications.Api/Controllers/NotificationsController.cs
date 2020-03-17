using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrdersNotifications.Api.Models;
using OrdersNotifications.Api.Services;

namespace OrdersNotifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _service;

        public NotificationsController(INotificationsService service) 
            => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllPending()
        {
            var notifications = await _service.GetAllPending();
            return Ok(notifications);
        }
        
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] PendingNotification notification)
        {
            await _service.SendNotification(notification);
            return Ok();
        }
    }
}