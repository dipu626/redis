using Caching.Domain.Repositories;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Caching.Infrastructure.Persistence
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDatabase _db;

        public CacheRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public async Task<bool> SetAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isInserted = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
            return isInserted;
        }

        public async Task<object> DeleteAsync(string key)
        {
            bool isKeyExist = await _db.KeyExistsAsync(key);

            if (isKeyExist)
            {
                return _db.KeyDeleteAsync(key);
            }

            return false;
        }

        public async Task DeleteAllAsync()
        {
            await _db.ExecuteAsync("FlushDB");
        }
    }
}
