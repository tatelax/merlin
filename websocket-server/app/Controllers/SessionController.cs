using Fleck;
using Newtonsoft.Json;

namespace app.Controllers
{
    public class SessionController
    {
        private Dictionary<IWebSocketConnection, int> _sessionConnections; // key = socket; value = session id
        public Dictionary<int, Session> _sessions; // key = sessionID; value = session object
        private Dictionary<string, List<int>> _activeApps; // key = appID; value = associated sessionIDs

        private readonly RedisController _redisController;
        private int _sessionIncr;

        public SessionController(RedisController redisController)
        {
            _redisController = redisController;
            _sessionConnections = new Dictionary<IWebSocketConnection, int>();
            _sessions = new Dictionary<int, Session>();
            _activeApps = new Dictionary<string, List<int>>();
        }

        public async Task ReceiveStateAsync(IWebSocketConnection socket, string appID, byte[] data)
        {
            if (!_sessionConnections.ContainsKey(socket))
            {
                _sessionIncr++;
                _sessionConnections.Add(socket, _sessionIncr);
                Console.WriteLine($"Added new socket: {socket.ConnectionInfo.Id}");

                if (!_sessions.ContainsKey(_sessionIncr))
                {
                    _sessions.Add(_sessionIncr, new Session(new List<IWebSocketConnection> { socket }, appID));
                    Console.WriteLine($"Added new session {_sessionIncr}");
                }

                if (!_activeApps.ContainsKey(appID))
                {
                    _activeApps.Add(appID, new List<int> { _sessionIncr });
                    Console.WriteLine($"Added active app {appID}");
                }
                else
                {
                    _activeApps[appID].Add(_sessionIncr);
                    Console.WriteLine($"Added connection to app {appID}");
                }
            }
            else
            {
                Console.WriteLine("Socket already registered");
            }

            await _redisController.WriteStateUpdate(appID, _sessionIncr, data);
            SendStateUpdateToClients(_sessionConnections[socket], data);

            Console.WriteLine($"Updated session state for appID {appID}");
        }

        public void RemoveSession(IWebSocketConnection socket)
        {
            if (!_sessionConnections.ContainsKey(socket))
            {
                Console.WriteLine("The session did not exist. Failed to remove it.");
                return;
            }
            int sessionID = _sessionConnections[socket];
            string appID = _sessions[sessionID]._appID;
            _sessionConnections.Remove(socket);

            var clientsForThisSession = _sessions[sessionID]._clients;

            for (int i = 0; i < clientsForThisSession.Count; i++)
            {
                if (clientsForThisSession[i].IsAvailable)
                    clientsForThisSession[i].Close();
            }

            _sessions.Remove(sessionID);
            _activeApps.Remove(appID);

            Console.WriteLine($"Session Removed: {sessionID}");
        }

        public void AddSessionObserver(IWebSocketConnection socket, string appID, int sessionID)
        {
            _sessions[sessionID].AddClient(socket);
        }

        public void SendStateUpdateToClients(int sessionID, byte[] data)
        {
            Session session = _sessions[sessionID];

            for (int i = 0; i < session._clients.Count; i++)
            {
                session._clients[i].Send(data);
                Console.WriteLine(i);
            }

            Console.WriteLine("Send update to all clients");
        }

        // public async void AddSession(string userID, string appID, WebSocket socket)
        // {
        //     if (_activeApps.ContainsKey(appID))
        //     {
        //         int sessionIDForThisApp = _activeApps[appID];
        //         _sessions[sessionIDForThisApp].AddClient(socket);
        //         _sessionConnections.TryAdd(socket, sessionIDForThisApp);

        //         Console.WriteLine($"Added Client to Session: {sessionIDForThisApp}");
        //     }
        //     else
        //     {
        //         Session newSession = new Session(new List<WebSocket> { socket }, appID);

        //         Random random = new Random();
        //         int newSessionID = random.Next();

        //         if (!_sessions.ContainsKey(newSessionID))
        //             _sessions.TryAdd(newSessionID, newSession);

        //         _sessionConnections.TryAdd(socket, newSessionID);

        //         await _redisController.WriteStreamData(appID, newSessionID, new byte[0]);

        //         _activeApps.TryAdd(appID, newSessionID);
        //         Console.WriteLine($"Created New Session: {newSessionID}");
        //     }
        // }

        // public async Task ReceivedDataAsync(WebSocket webSocket, byte[] data)
        // {
        //     int sessionID = _sessionConnections[webSocket];
        //     string appID = _sessions[sessionID]._appID;

        //     await _redisController.WriteStreamData(appID, sessionID, data);

        //     await UpdateOtherClients(sessionID, data);
        // }

        // private async Task UpdateOtherClients(int sessionID, byte[] data)
        // {


        //     for (int i = 0; i < _sessions[sessionID]._clients.Count; i++)
        //     {
        //         // if(clients[i] == originSocket)
        //         //     continue;

        //         await _sessions[sessionID]._clients[i].SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
        //         Console.WriteLine("Data sent");
        //     }
        // }

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

        public SessionListModel[]? GetSessionsForApp(string appID)
        {
            if (!_activeApps.ContainsKey(appID))
            {
                Console.WriteLine($"{appID} was not found.");
                return null;
            }

            SessionListModel[] sessions = new SessionListModel[_activeApps[appID].Count];

            for (int i = 0; i < sessions.Length; i++)
            {
                SessionListModel newModel = new SessionListModel();

                int sessionID = _activeApps[appID][i];

                if (!_sessions.ContainsKey(sessionID))
                {
                    Console.WriteLine($"{sessionID} was not found.");
                    return null;
                }

                int connectedClients = _sessions[sessionID]._clients.Count;

                newModel.sessionID = sessionID;
                newModel.connectedClients = connectedClients;

                sessions[i] = newModel;
            }

            return sessions;
        }
    }
}