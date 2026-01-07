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
        private readonly PolicyService _policyService;

        public ChatController(
            Kernel kernel,
            OrderService orderService,
            PolicyService policyService)
        {
            _kernel = kernel;
            _orderService = orderService;
            _policyService = policyService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var message = request.Message;

            // 1️⃣ Detect order ID (4 or more digits)
            var match = Regex.Match(message, @"\b\d{4,}\b");
            string? orderInfo = null;

            if (match.Success)
            {
                int orderId = int.Parse(match.Value);
                orderInfo = _orderService.GetOrderStatus(orderId);
            }

            // 2️⃣ Detect return policy question
            bool wantsPolicy = message.ToLower().Contains("return");
            string? policyInfo = null;

            if (wantsPolicy)
            {
                policyInfo = _policyService.GetReturnPolicy();
            }

            // 3️⃣ Build AI prompt with all available info
           var prompt = $"""
You are a professional e-commerce customer support agent.

Customer message:
{message}

{(policyInfo != null ? $"Return Policy:\n{policyInfo}\n" : "")}
{(orderInfo != null ? $"Order Status:\n{orderInfo}\n" : "")}

You MUST address ALL topics mentioned by the customer.
If both return policy and order status are present,
clearly answer BOTH in your response.
Respond politely and clearly.
""";


            var result = await _kernel.InvokePromptAsync(prompt);

            return Ok(new
            {
                reply = result.ToString()
            });
        }
    }

    // Request model
    public class ChatRequest
    {
        public required string Message { get; set; }
    }
}
