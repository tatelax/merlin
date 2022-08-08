using Fleck;

namespace app.Controllers
{
    public class WebSocketController
    {
        private readonly RedisController _redisController;
        private readonly SessionController _sessionController;

        private readonly WebSocketServer _wsServer;

        public WebSocketController(RedisController redisController, SessionController sessionController)
        {
            _redisController = redisController;
            _sessionController = sessionController;

            _wsServer = new WebSocketServer("ws://0.0.0.0:2414/");
            _wsServer.RestartAfterListenError = true;
            _wsServer.SupportedSubProtocols = new[] { "HelloWorld", "AnExample" };

            _wsServer.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Open! NegotiatedSubProtocol: {socket.ConnectionInfo.NegotiatedSubProtocol}");
                };
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => socket.Send(message);
            });
        }
    }
}