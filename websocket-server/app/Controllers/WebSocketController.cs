using System.Text;
using System.Web;
using Fleck;
using Newtonsoft.Json;

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
            _wsServer.SupportedSubProtocols = new[] { "GetSessionsList", "StateUpdate" };

            _wsServer.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine($"Open! NegotiatedSubProtocol: {socket.ConnectionInfo.NegotiatedSubProtocol}");
                };
                socket.OnClose = () =>
                {
                    HandleClose(socket);
                    Console.WriteLine("Close!");
                };
                socket.OnBinary = async bin =>
                {
                    await HandleMessageAsync(socket, bin);
                };
                socket.OnError = err => Console.WriteLine(err);
                socket.OnMessage = async msg =>
                {
                    await HandleMessageAsync(socket, msg);
                };
                socket.OnPing = ping => socket.Send(ping);
                socket.OnPong = pong => Console.WriteLine("Pong");
            });
        }

        private async Task HandleMessageAsync(IWebSocketConnection socket, string data) => await HandleMessageAsync(socket, Encoding.UTF8.GetBytes(data));

        private async Task HandleMessageAsync(IWebSocketConnection socket, byte[] data)
        {
            switch (socket.ConnectionInfo.NegotiatedSubProtocol)
            {
                case "GetSessionsList":
                    SendSessionsList(socket);
                    break;
                case "StateUpdate":
                    await StateUpdateAsync(socket, data);
                    break;
                default:
                    Console.WriteLine("Received unknown sub protocol...");
                    break;
            }
        }

        private void HandleClose(IWebSocketConnection socket)
        {
            if (socket.ConnectionInfo.SubProtocol == "StateUpdate")
                _sessionController.RemoveSession(socket);
        }

        private void SendSessionsList(IWebSocketConnection socket)
        {
            string appID = GetParamFromURL(socket, "appID");

            if (string.IsNullOrEmpty(appID))
                return;

            SessionListModel[]? sessions;

            if (!String.IsNullOrEmpty(appID))
            {
                sessions = _sessionController.GetSessionsForApp(appID);

                if (socket.IsAvailable)
                {
                    socket.Send(JsonConvert.SerializeObject(sessions));
                    Console.WriteLine($"Sent list of sessions for appID {appID}");
                }
                else
                {
                    Console.WriteLine("Tried to send a list of sessions but the socket was not available");
                }
            }
            else
            {
                Console.WriteLine("appID param not found in URL");
            }
        }

        private async Task StateUpdateAsync(IWebSocketConnection socket, byte[] data)
        {
            string appID = GetParamFromURL(socket, "appID");

            if (!string.IsNullOrEmpty(appID))
                await _sessionController.ReceiveStateAsync(socket, appID, data);
        }

        private string GetParamFromURL(IWebSocketConnection socket, string paramID)
        {
            var uri = new Uri("http://localhost" + socket.ConnectionInfo.Path);
            var query = HttpUtility.ParseQueryString(uri.Query);

            string? possibleOutput = query.Get(paramID);
            string output = string.Empty;

            if (query is not null)
            {
                if (possibleOutput is not null)
                {
                    output = query.Get(paramID) ?? "NULL";
                }
                else
                {
                    Console.WriteLine($"{paramID} is null");
                }
            }
            else
            {
                Console.WriteLine("Query is null");
            }

            return output;
        }
    }
}