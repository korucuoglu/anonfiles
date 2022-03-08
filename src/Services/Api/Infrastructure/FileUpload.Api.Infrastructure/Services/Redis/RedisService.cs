using FileUpload.Api.Application.Interfaces.Redis;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace FileUpload.Api.Infrastructure.Services.Redis
{
    public class RedisService : CacheHelper, IRedisService
    {
        public IDatabase Db { get; set; }
        public RedisService(IRedisSettings redisSettings)
        {
            // Db = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase(0);
            Db = ConnectionMultiplexer.Connect($"{redisSettings.Host}:{redisSettings.Port}").GetDatabase(0);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await Db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return Deserialize<T>(value);

        }
        public async Task SetAsync(string key, object data)
        {
            var serializeData = Serialize(data);
            await Db.StringSetAsync(key, serializeData);
        }
        public async Task<bool> IsKeyAsync(string key)
        {
            return await Db.KeyExistsAsync(key);
        }
        public async Task<bool> RemoveAsync(string key)
        {
            return await Db.KeyDeleteAsync(key);
        }



    }
}
