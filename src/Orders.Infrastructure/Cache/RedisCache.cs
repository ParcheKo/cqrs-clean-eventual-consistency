using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orders.Core;
using Orders.Core.Shared;
using StackExchange.Redis;

namespace Orders.Infrastructure.Cache;

public class RedisCache : ICache
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisCache> _logger;
    private readonly ConnectionMultiplexer _redis;

    public RedisCache(
        AppConfiguration configuration,
        ILogger<RedisCache> logger
    )
    {
        _redis = ConnectionMultiplexer.Connect(configuration.ConnectionStrings.RedisCache);
        _db = _redis.GetDatabase();
        _logger = logger;
    }

    public async Task<bool> Delete(string key)
    {
        try
        {
            foreach (var ep in _redis.GetEndPoints())
            {
                var server = _redis.GetServer(ep);
                var keys = server.Keys(
                    0,
                    key + "*"
                ).ToArray();
                await _db.KeyDeleteAsync(keys);
            }

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro when deleting redis cache key"
            );

            throw;
        }
    }

    public async Task<T> Get<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue) return default;

        var result = JsonConvert.DeserializeObject<T>(value);

        return await Task.FromResult(result);
    }

    public async Task Store<T>(
        string key,
        T value,
        params string[] @params
    )
    {
        var complexKey = GenerateKeyWithParams(
            key,
            @params
        );
        var cache = JsonConvert.SerializeObject(value);

        await _db.StringSetAsync(
            complexKey,
            cache,
            TimeSpan.FromSeconds(30)
        );
    }

    private string GenerateKeyWithParams(
        string key,
        string[] @params
    )
    {
        if (@params == null) return key;

        var complexKey = key;

        foreach (var param in @params) complexKey += $"&{param}";

        return complexKey;
    }
}