using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace app.Controllers
{
    public class SessionController
    {
        private ConcurrentDictionary<WebSocket, int> _sessionConnections; // key = socket; value = session id
        public ConcurrentDictionary<int, Session> _sessions; // key = sessionID; value = session object
        private ConcurrentDictionary<string, int> _activeApps; // key = appID; value = associated sessionID

        private readonly RedisController _redisController;

        public SessionController(RedisController redisController)
        {
            _redisController = redisController;
            _sessionConnections = new ConcurrentDictionary<WebSocket, int>();
            _sessions = new ConcurrentDictionary<int, Session>();
            _activeApps = new ConcurrentDictionary<string, int>();
        }

        public async void AddSession(string userID, string appID, WebSocket socket)
        {
            if (_activeApps.ContainsKey(appID))
            {
                int sessionIDForThisApp = _activeApps[appID];
                _sessions[sessionIDForThisApp].AddClient(socket);
                _sessionConnections.TryAdd(socket, sessionIDForThisApp);

                Console.WriteLine($"Added Client to Session: {sessionIDForThisApp}");
            }
            else
            {
                Session newSession = new Session(new List<WebSocket> { socket }, appID);

                Random random = new Random();
                int newSessionID = random.Next();

                if (!_sessions.ContainsKey(newSessionID))
                    _sessions.TryAdd(newSessionID, newSession);

                _sessionConnections.TryAdd(socket, newSessionID);

                await _redisController.WriteStreamData(appID, newSessionID, new byte[0]);

                _activeApps.TryAdd(appID, newSessionID);
                Console.WriteLine($"Created New Session: {newSessionID}");
            }
        }

        public async Task ReceivedDataAsync(WebSocket webSocket, byte[] data)
        {
            int sessionID = _sessionConnections[webSocket];
            string appID = _sessions[sessionID]._appID;

            await _redisController.WriteStreamData(appID, sessionID, data);

            await UpdateOtherClients(sessionID, data);
        }

        private async Task UpdateOtherClients(int sessionID, byte[] data)
        {


            for (int i = 0; i < _sessions[sessionID]._clients.Count; i++)
            {
                // if(clients[i] == originSocket)
                //     continue;

                await _sessions[sessionID]._clients[i].SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
                Console.WriteLine("Data sent");
            }
        }

        // public void RemoveSession(int id)
        // {
        //     if(!_sessionConnections.ContainsKey(id)) 
        //     {
        //         throw new KeyNotFoundException();
        //     }

        //     _sessionConnections.TryRemove(id, out var sessions);

        //     for (int i = 0; i < sessions?.Count; i++) {
        //         sessions[i].Dispose(); //TODO: Test this
        //     }
        // }
    }
}