namespace Chat.Repository.Messages
{
    public interface IMessageRepository
    {
        Task SaveMessageAsync(string message);
    }
}
