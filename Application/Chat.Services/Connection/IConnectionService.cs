using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;

namespace Chat.Application.Servces
{
    /// <summary>
    /// Сервис управления соединениями
    /// </summary>
    public interface IConnectionService
    {
        /// <summary>
        /// Получить соединение
        /// </summary>
        /// <param name="httpContext">Информация  запросе</param>
        Task ConnectionHandler(WebSocket webSocket);

        Task SendMessageToAllAsync(string message);
    }
}