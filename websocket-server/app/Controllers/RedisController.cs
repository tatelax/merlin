using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace app.Controllers;

public class RedisController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;

    public RedisController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        Console.WriteLine("Redis Controller Ready");
    }

    public async Task<IActionResult> WriteStreamData(string appID, int sessionID, byte[] data)
    {
        var db = _redis.GetDatabase();
        var foo = await db.StreamAddAsync($"{appID}:{sessionID}", new NameValueEntry[]
        {
            new("data", data)
        });

        return Ok(foo.ToString());
    }
}