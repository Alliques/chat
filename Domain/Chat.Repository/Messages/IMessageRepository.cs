
using Chat.Core;

namespace Chat.Repository.Messages
{
    public interface IMessageRepository
    {
        Task SaveMessageAsync(string message);
        Task<IEnumerable<MessageEntity>> GetMessagesAsync(TimeInterval timeInterval);
    }
}
