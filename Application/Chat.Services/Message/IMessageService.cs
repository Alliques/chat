using Chat.Core;
using Chat.Services.DTO;

namespace Chat.Services.Message
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> Get(TimeInterval timeInterval);
    }
}