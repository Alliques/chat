using Chat.Application.Servces;
using Chat.Repository.Messages;

namespace ChatServer.Extentions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            services.AddSingleton<IMessageRepository>(provider => new MessageRepository(connectionString));
            services.AddSingleton<IConnectionService, ConnectionService>();
        }
    }
}
