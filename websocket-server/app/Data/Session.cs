using Fleck;

public class Session
{
    public List<IWebSocketConnection> _clients;
    public string _appID;

    public Session(List<IWebSocketConnection> clients, string appID)
    {
        _clients = clients;
        _appID = appID;
    }

    public void AddClient(IWebSocketConnection client)
    {
        _clients.Add(client);
    }
}