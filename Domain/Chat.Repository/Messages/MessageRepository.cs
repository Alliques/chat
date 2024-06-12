using Chat.Core;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Chat.Repository.Messages
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(string connectionString, ILogger<MessageRepository> logger) 
            : base(connectionString)
        {
            _logger = logger;
        }

        public async Task SaveMessageAsync(string message)
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            string query = "INSERT INTO Messages (message, date) VALUES (@message, @date)";
            _logger.LogInformation("Executing query: {Query} with message: {Message}", query, message);

            await using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("message", message);
            cmd.Parameters.AddWithValue("date", DateTime.UtcNow);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                _logger.LogInformation("Message saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving message. Message: {message}", message);
                throw;
            }
        }

        public async Task<IEnumerable<MessageEntity>> GetMessagesAsync(TimeInterval timeInterval)
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            string query = @"
            SELECT message, date
            FROM messages
            WHERE date >= @start AND date <= @end";

            await using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("start", timeInterval.Start);
            cmd.Parameters.AddWithValue("end", timeInterval.End);

            var messages = new List<MessageEntity>();

            try
            {
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    messages.Add(new MessageEntity
                    {
                        Message = reader.GetString(0),
                        Date = reader.GetDateTime(1)
                    });
                }

                _logger.LogInformation("Retrieved {Count} messages.", messages.Count);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving messages.");
                throw;
            }

            return messages;
        }
    }
}
