using Npgsql;

namespace Chat.Repository.Messages
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task SaveMessageAsync(string message)
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            string query = "INSERT INTO messages (content, timestamp) VALUES (@content, @timestamp)";
            await using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("content", message);
            cmd.Parameters.AddWithValue("timestamp", DateTime.UtcNow);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
