using MassTransit;
using Messaging.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var order = new OrderCreated
            {
                CorrelationId = NewId.NextGuid(), //NewId.NextGuid() MassTransit provide karta hai.
                OrderId = 1001,
                CustomerId = 101,
                Amount = 1500
            };

            await _publishEndpoint.Publish(order);

            return Ok(order);
        }
    }
}
