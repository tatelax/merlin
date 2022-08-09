using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace app.Controllers;

public class RedisController
{
    private readonly IConnectionMultiplexer _multiplexer;

    public RedisController()
    {
        _multiplexer = ConnectionMultiplexer.Connect("localhost");

        Console.WriteLine("Redis Controller Ready");
    }

    public async Task<bool> WriteStateUpdate(string appID, int sessionID, byte[] data)
    {
        var db = _multiplexer.GetDatabase();
        var foo = await db.StreamAddAsync($"{appID}:{sessionID}", new NameValueEntry[]
        {
            new("data", data)
        });

        return true;
    }
}