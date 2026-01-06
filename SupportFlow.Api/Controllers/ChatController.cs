using Microsoft.SemanticKernel;
using Microsoft.AspNetCore.Mvc;

namespace SupportFlow.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly Kernel _kernel;

        public ChatController(Kernel kernel)
        {
            _kernel = kernel;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var prompt = $"""
            You are an e-commerce customer support assistant.
            Be polite, clear, and helpful.

            User message:
            {request.Message}
            """;

            var result = await _kernel.InvokePromptAsync(prompt);

            return Ok(new
            {
                reply = result.ToString()
            });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
