using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrdersNotifications.Api.Models;
using OrdersNotifications.Api.Services;

namespace OrdersNotifications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _service;

        public OrdersController(IOrdersService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetById(id);

            if (order == null)
                return BadRequest("Requested order was not found.");

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrder createOrder)
        {
            var order = await _service.Create(createOrder);
            return CreatedAtAction("GetById", new {id = order.Id}, order);
        }
    }
}