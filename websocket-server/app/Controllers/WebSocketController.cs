using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace app.Controllers
{

    // <snippet>
    public class WebSocketController : ControllerBase
    {
        private readonly RedisController _redisController;
        private readonly SessionController _sessionController;

        public WebSocketController(RedisController redisController, SessionController sessionController)
        {
            _redisController = redisController;
            _sessionController = sessionController;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Adding Session: " + HttpContext.Request.Query["appID"] + ": " + HttpContext.Request.Query["userID"]);
                string _userID = HttpContext.Request.Query["userID"][0];
                string _appID = HttpContext.Request.Query["appID"][0];
                _sessionController.AddSession(_userID, _appID, webSocket);
                await ListenForData(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        // </snippet>

        private async Task ListenForData(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                Console.WriteLine("data received " + System.Text.Encoding.UTF8.GetString(buffer));

                await MessageHandler(webSocket, buffer);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }

        private async Task MessageHandler(WebSocket ws, byte[] data)
        {
            Console.WriteLine("!!!" + System.Text.Encoding.UTF8.GetString(data) + "!!!");

            string msg = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);

            if (msg == "getSessions")
            {
                Console.WriteLine("sednddinging");
                await SendAppList(ws);
            }
            else
            {
                await _sessionController.ReceivedDataAsync(ws, data);
            }
        }

        //TODO: Clean this up and move it somewher else
        private async Task SendAppList(WebSocket ws)
        {
            SessionModel model = new SessionModel();

            List<SessionModel> sessionModels = new List<SessionModel>();

            foreach (KeyValuePair<int, Session> session in _sessionController._sessions)
            {
                SessionModel newModel = new SessionModel();
                newModel.connectedClients = session.Value._clients.Count;
                newModel.sessionID = session.Key;
                sessionModels.Add(newModel);
            }

            string json = JsonConvert.SerializeObject(sessionModels);
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(json)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}