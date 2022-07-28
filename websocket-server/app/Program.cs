using StackExchange.Redis;
using WebSocketsSample.Controllers;

namespace WebSocketsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            SetupRedis(builder.Services);

            var app = builder.Build();

            SetupWebSocket(app);
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }

        private static void SetupRedis(IServiceCollection services)
        {
            var multiplexer = ConnectionMultiplexer.Connect("localhost");
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddSingleton<RedisController>();
        }

        private static void SetupWebSocket(WebApplication app)
        {
// <snippet_UseWebSockets>
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2),
            };

            app.UseWebSockets(webSocketOptions);
// </snippet_UseWebSockets>
        }
    }
}