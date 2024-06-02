using Microsoft.AspNetCore.Http;

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
        Task GetConnection(HttpContext httpContext);
    }
}