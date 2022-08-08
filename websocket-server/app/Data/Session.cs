using System.Net.WebSockets;

public class Session 
{
    public List<WebSocket> _clients;
    public string _appID;

    public Session(List<WebSocket> clients, string appID)
    {
        _clients = clients;
        _appID = appID;
    }

    public void AddClient(WebSocket client) {
        _clients.Add(client);
    }
}