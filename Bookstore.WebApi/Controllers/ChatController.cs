using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Bookstore.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : Controller
    { 
        private readonly IHubContext<ChatHub> hubContext;
        public ChatController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        [Route("send")]                                           //path looks like this: https://localhost:44379/api/chat/send
        [HttpPost]
        [Authorize]
        public IActionResult SendRequest([FromBody] Message msg)
        {
            hubContext.Clients.All.SendAsync("ReceiveOne", msg.user, msg.msgText);
            return Ok();
        }
    }
}
