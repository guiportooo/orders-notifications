using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersNotifications.Api.Models;
using OrdersNotifications.Library;

namespace OrdersNotifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersContext _context;

        public OrdersController(OrdersContext context)
            => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _context
                .Orders
                .Include(x => x.Items)
                .ToListAsync();
            var ordersCreated = orders.Select(x => new OrderCreated(x));
            return Ok(ordersCreated);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return BadRequest("Requested order was not found.");

            var orderCreated = new OrderCreated(order);
            return Ok(orderCreated);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrder createOrder)
        {
            var order = createOrder.MapToEntity();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var orderCreated = new OrderCreated(order);
            return CreatedAtAction("Get", new {id = order.Id}, orderCreated);
        }
    }
}