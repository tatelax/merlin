using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace WebSocketsSample.Controllers;

public class RedisController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;
    
    public RedisController(IConnectionMultiplexer redis)
    {
        _redis = redis;
        Console.WriteLine("Redis Controller Ready");
    }

    public async Task<IActionResult> WriteData()
    {
        Console.WriteLine("Hello World!");
        var db = _redis.GetDatabase();
        var foo = await db.StreamAddAsync("MyCoolStream", new NameValueEntry[]
        {
            new("number", 1)
        });
        
        var range = db.StreamRange("MyCoolStream");

        var sum = 0;
        
        for (var i = 0; i < range.Length; i++)
        {
            var values = range[i].Values;

            for (var i1 = 0; i1 < values.Length; i1++)
            {
                values[i1].Value.TryParse(out int val);
                sum += val;
            }
        }
        
        Console.WriteLine(sum);
        return Ok(foo.ToString());
    }
}