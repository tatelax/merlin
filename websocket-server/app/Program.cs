using app.Controllers;

namespace app
{
    class Program
    {
        private static void Main(string[] args)
        {
            var _redisController = new RedisController();
            var _sessionController = new SessionController(_redisController);
            var _wsController = new WebSocketController(_redisController, _sessionController);

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            app.UseStaticFiles();
            app.Run();
        }
    }
}