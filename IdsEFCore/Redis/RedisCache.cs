using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdsEFCore.Redis
{
    public class RedisCache
    {
        private readonly IConfiguration configuration;
        private IDatabase db;
        public RedisCache(
            IConfiguration configuration
            )
        {
            this.configuration = configuration;
            var conn = this.configuration.GetSection("Redis:Connection").Value;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(conn);
            db = redis.GetDatabase();
        }

        public async Task SetString(string key, string value, int expireSecond)
        {
            await db.StringSetAsync(key, value, TimeSpan.FromSeconds(expireSecond));
        }

        public async Task<string> GetString(string key)
        {
            return await db.StringGetAsync(key);
        }

        public async Task Remove(string key)
        {
            await db.KeyDeleteAsync(key);
        }
    }
}
