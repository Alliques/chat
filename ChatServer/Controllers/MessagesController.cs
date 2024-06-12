using Chat.Application.Servces;
using Chat.Core;
using Chat.Services.DTO;
using Chat.Services.Message;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IConnectionService connectionService, 
            IMessageService messageService,
            ILogger<MessagesController> logger)
        {
            _connectionService = connectionService;
            _messageService = messageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<MessageDto>> Get([FromQuery] TimeInterval timeInterval)
        {
            if (timeInterval.Start > timeInterval.End)
            {
                _logger.LogError("Invalid TimeInterval: Start time {Start} is greater than end time {End}", timeInterval.Start, timeInterval.End);
                new ArgumentException("Start time must be earlier than end time.");
            }

            _logger.LogInformation("Fetching messages from {Start} to {End}", timeInterval.Start, timeInterval.End);
            return await _messageService.Get(timeInterval);
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MessageCreationDto request)
        {
            _logger.LogInformation("Sending message to all connected clients");
            await _connectionService.SendMessageToAllAsync(request.Message);
            return Ok(new { Message = "Message sent to all connected clients" });
        }
    }
}
