using Microsoft.AspNetCore.Mvc;

namespace SupportFlow.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        [HttpPost]
        public IActionResult Chat([FromBody] ChatRequest request)
        {
            return Ok(new
            {
                reply = $"[AI placeholder] I received: {request.Message}"

            });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
