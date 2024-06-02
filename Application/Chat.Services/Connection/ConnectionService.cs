using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Chat.Application.Servces
{
    public class ConnectionService : IConnectionService
    {
        private ConcurrentDictionary<Guid, WebSocket> _websocketConnections { get; set; } = new();

        public async Task GetConnection(HttpContext httpContext)
        {
            using var webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
            var id = AddConnection(Guid.NewGuid(), webSocket);
            var connections = GetOther(id);
            await Echo(webSocket, connections);
        }

        private static async Task Echo(WebSocket webSocket, IEnumerable<KeyValuePair<Guid, WebSocket>> connections)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
            var tasks = new List<Task>();

            while (!receiveResult.CloseStatus.HasValue)
            {
                foreach (var item in connections)
                {
                    tasks.Add(item.Value.SendAsync(
                                        new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                                        WebSocketMessageType.Text,
                                        true,
                                        CancellationToken.None));
                }

                await Task.WhenAll(tasks);
                tasks.Clear();
                receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }

        private Guid AddConnection(Guid id, WebSocket webSocket)
        {
            _websocketConnections.TryAdd(id, webSocket);
            return id;
        }

        private IEnumerable<KeyValuePair<Guid, WebSocket>> GetOther(Guid currentId)
        {
            return _websocketConnections.Where(o => o.Key != currentId && o.Value.State == WebSocketState.Open);
        }
    }
}
