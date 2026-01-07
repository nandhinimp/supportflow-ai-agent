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
        private readonly EscalationService _escalationService;

        public ChatController(
            Kernel kernel,
            OrderService orderService,
            PolicyService policyService,
            EscalationService escalationService)
        {
            _kernel = kernel;
            _orderService = orderService;
            _policyService = policyService;
            _escalationService = escalationService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var originalMessage = request.Message;
            var message = originalMessage.ToLower();

            /* -------------------------------------------------
             * 1️⃣ ESCALATION CHECK (FIRST – safety comes first)
             * ------------------------------------------------- */

            string[] angryKeywords =
            {
                "angry", "frustrated", "worst", "bad service",
                "complaint", "not happy", "irritated"
            };

            bool isAngry = angryKeywords.Any(k => message.Contains(k));

            if (isAngry)
            {
                var summary = _escalationService.CreateSummary(
                    originalMessage,
                    "Customer appears frustrated or unhappy"
                );

                return Ok(new
                {
                    reply = "I understand your concern. I’m escalating this to a human support agent who will assist you shortly.",
                    escalationSummary = summary
                });
            }

            /* -------------------------------------------------
             * 2️⃣ ORDER TOOL CHECK
             * ------------------------------------------------- */

            var match = Regex.Match(message, @"\b\d{4,}\b");
            string? orderInfo = null;

            if (match.Success)
            {
                int orderId = int.Parse(match.Value);
                orderInfo = _orderService.GetOrderStatus(orderId);

                if (orderInfo == "Order not found")
                {
                    var summary = _escalationService.CreateSummary(
                        originalMessage,
                        $"Order ID {orderId} not found in system"
                    );

                    return Ok(new
                    {
                        reply = "I couldn’t locate your order details. I’m escalating this to a human support agent for further assistance.",
                        escalationSummary = summary
                    });
                }
            }

            /* -------------------------------------------------
             * 3️⃣ POLICY (RAG-style) CHECK
             * ------------------------------------------------- */

            string? policyInfo = null;

            if (message.Contains("return"))
            {
                policyInfo = _policyService.GetReturnPolicy();
            }

            /* -------------------------------------------------
             * 4️⃣ AI RESPONSE SYNTHESIS
             * ------------------------------------------------- */

            var prompt = $"""
            You are a professional e-commerce customer support agent.

            Customer message:
            {originalMessage}

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

    /* -------------------------------------------------
     * Request Model
     * ------------------------------------------------- */
    public class ChatRequest
    {
        public required string Message { get; set; }
    }
}
