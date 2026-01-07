using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.Services;

namespace SupportFlow.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderStatus(int orderId)
        {
            var status = _orderService.GetOrderStatus(orderId);

            return Ok(new
            {
                orderId,
                status
            });
        }
    }
}
