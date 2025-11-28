using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using OrdersTask.Domain.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrdersTask.Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redisDb;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _redisDb = redis.GetDatabase();
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            try
            {
                var value = await _redisDb.StringGetAsync(cacheKey);
                if (value.IsNullOrEmpty)
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving key {cacheKey} from Redis");
                return default;
            }
        }

        public async Task RemoveAsync(string cacheKey)
        {
            try
            {
                await _redisDb.KeyDeleteAsync(cacheKey);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error removing key {cacheKey} from Redis");

            }
        }

        public async Task SetAsync<T>(string cacheKey, T value, TimeSpan? ttl = null)
        {
            try
            {
                var serialized = JsonSerializer.Serialize(value);
                await _redisDb.StringSetAsync(cacheKey, serialized, (Expiration) ttl!);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error setting key {cacheKey} in Redis");
            }
        }
    }
}
