using Newtonsoft.Json;

namespace RedisTest.Redis
{
    public sealed class RedisCache : RedisCacheBase
    {
        public RedisCache(string redisServer, string password = "", int redisDatabaseId = 0) : base(redisServer, password, redisDatabaseId)
        {
        }

        public bool Set(string key, object cacheObject, TimeSpan expiryTime)
        {
            return RedisDatabase.StringSet(key, JsonConvert.SerializeObject(cacheObject), expiryTime);
        }

        public T Get<T>(string key)
        {
            var value = RedisDatabase.StringGet(key);
            if (value.IsNull)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public T GetOrSet<T>(string cacheKey, TimeSpan cachePeriod, Func<T> cacheNotAvailableFunc) where T : class
        {
            var result = Get<T>(cacheKey);
            if (result != null)
            {
                return result;
            }

            result = cacheNotAvailableFunc();
            Set(cacheKey, result, cachePeriod);

            return result;
        }

        public List<string> GetAllKeys()
        {
            var endPoints = Connection.GetEndPoints();
            var server = Connection.GetServer(endPoints.FirstOrDefault());
            var keys = new List<string>();
            // show all keys in database 0 that include "foo" in their name
            foreach (var key in server.Keys())
            {
                keys.Add(key);
            }

            return keys;
        }
    }
}
