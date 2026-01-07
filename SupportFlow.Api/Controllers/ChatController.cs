using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using SupportFlow.Api.Services;
using System.Text.RegularExpressions;

namespace SupportFlow.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly Kernel _kernel;
        private readonly OrderService _orderService;

        public ChatController(Kernel kernel, OrderService orderService)
        {
            _kernel = kernel;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var userMessage = request.Message;

            // 🔍 STEP 1: Detect order number (4+ digits)
            var match = Regex.Match(userMessage, @"\b\d{4,}\b");

            // 🛠️ STEP 2: If order ID exists → call TOOL
            if (match.Success)
            {
                int orderId = int.Parse(match.Value);
                var orderStatus = _orderService.GetOrderStatus(orderId);

                var toolPrompt = $"""
                You are a customer support agent.

                Order ID: {orderId}
                Order Status: {orderStatus}

                Explain the order status politely to the customer.
                """;

                var result = await _kernel.InvokePromptAsync(toolPrompt);

                return Ok(new
                {
                    reply = result.ToString()
                });
            }

            // 💬 STEP 3: No tool needed → normal AI reply
            var normalPrompt = $"""
            You are a helpful e-commerce support assistant.
            Respond politely and clearly.

            User: {userMessage}
            """;

            var normalResult = await _kernel.InvokePromptAsync(normalPrompt);

            return Ok(new
            {
                reply = normalResult.ToString()
            });
        }
    }

    public class ChatRequest
    {
        public required string Message { get; set; }
    }
}
