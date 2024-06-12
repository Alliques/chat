
using Chat.Core;
using Chat.Repository.Messages;
using Chat.Services.DTO;
using Microsoft.Extensions.Logging;

namespace Chat.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<MessageDto>> Get(TimeInterval timeInterval)
        {
            _logger.LogInformation("Fetching messages for time interval {Start} to {End}", timeInterval.Start, timeInterval.End);
            var messages = await _messageRepository.GetMessagesAsync(timeInterval);
            _logger.LogInformation("Fetched {Count} messages.", messages.Count());

            return messages.Select(x => new MessageDto { Date = x.Date, Message = x.Message });
        }
    }
}
