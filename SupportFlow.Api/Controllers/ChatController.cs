using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

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
            var result = await _kernel.InvokePromptAsync(
                $"You are a helpful support agent. Reply to this message:\n{request.Message}"
            );

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
