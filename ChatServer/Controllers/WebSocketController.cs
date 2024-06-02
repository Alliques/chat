using Chat.Application.Servces;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        public readonly IConnectionService _connectionService;

        public WebSocketController(IConnectionService websocketHandler)
        {
            _connectionService = websocketHandler;
        }

        [Route("/ws")]
        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                await _connectionService.GetConnection(HttpContext);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}