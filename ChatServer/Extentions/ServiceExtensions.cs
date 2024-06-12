using Chat.Application.Servces;
using Chat.Repository.Messages;
using Chat.Services.Message;

namespace ChatServer.Extentions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            
            services.AddLogging(config =>
            {
                config.ClearProviders();
                config.AddConsole();
                config.AddDebug();
            });


            services.AddSingleton<IMessageRepository>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<MessageRepository>>();
                return new MessageRepository(connectionString, logger);
            });

            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddTransient<IMessageService, MessageService>();
        }
    }
}
