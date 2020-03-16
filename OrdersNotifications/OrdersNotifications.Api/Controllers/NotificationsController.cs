using Microsoft.AspNetCore.Mvc;
using OrdersNotifications.Api.Models;

namespace OrdersNotifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] SendEmail sendEmail)
        {
            return Accepted();
        }
    }
}