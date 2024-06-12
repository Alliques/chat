using Chat.Repository.Messages;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Chat.Application.Servces
{
    public class ConnectionService : IConnectionService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageRepository> _logger;
        private ConcurrentDictionary<Guid, WebSocket> _websocketConnections { get; set; } = new();

        public ConnectionService(IMessageRepository messageRepository, ILogger<MessageRepository> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task ConnectionHandler(WebSocket webSocket)
        {
            var id = Guid.NewGuid();
            _websocketConnections.TryAdd(id, webSocket);
            _logger.LogInformation("WebSocket connection added: {ConnectionId}", id);

            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = null;

            try
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation("Received message: {Message}", message);

                    await SendMessageToAllAsync(message);

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
            }
            catch (WebSocketException wsEx)
            {
                _logger.LogWarning(wsEx, "WebSocketException in connection {ConnectionId}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling WebSocket connection {ConnectionId}", id);
            }
            finally
            {
                _websocketConnections.TryRemove(id, out _);
                if (webSocket.State != WebSocketState.Aborted && webSocket.State != WebSocketState.Closed)
                {
                    try
                    {
                        await webSocket.CloseAsync(
                            result?.CloseStatus ?? WebSocketCloseStatus.NormalClosure,
                            result?.CloseStatusDescription ?? "Closing connection",
                            CancellationToken.None
                        );
                        _logger.LogInformation("WebSocket connection closed: {ConnectionId}", id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "WebSocket connection error for connection {ConnectionId}", id);
                    }
                }
                else
                {
                    _logger.LogInformation("Сonnection already closed: {ConnectionId}", id);
                }
            }
        }

        public async Task SendMessageToAllAsync(string message)
        {
            _logger.LogInformation("Broadcasting message to all connections: {Message}", message);

            await _messageRepository.SaveMessageAsync(message);
            var buffer = Encoding.UTF8.GetBytes(message);
            var tasks = _websocketConnections
                .Where(socket => socket.Value.State == WebSocketState.Open)
                .Select(socket => SendMessageAsync(socket.Value, buffer))
                .ToList();

            try
            {
                await Task.WhenAll(tasks);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error broadcasting message to connections.");
            }
        }

        private async Task SendMessageAsync(WebSocket socket, byte[] buffer)
        {
            try
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.LogInformation("Sent message to WebSocket: {Message}", Encoding.UTF8.GetString(buffer));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to WebSocket.");
            }
        }
    }
}
