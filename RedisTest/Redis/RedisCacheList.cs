namespace RedisTest.Redis
{
    internal class RedisCacheList : RedisCacheBase, IRedisCacheCollection
    {
        public RedisCacheList(string redisServer, string password = "", int redisDatabaseId = 0) : base(redisServer, password, redisDatabaseId)
        {
        }

        public bool SetCollection<T>(string key, IEnumerable<T> cacheObject, TimeSpan expiryTime) where T : IRedisHashEntry
        {
            RedisDatabase.ListLeftPush(key, cacheObject.Select(itm => itm.ToRedisValue()).ToArray());
            return RedisDatabase.KeyExpire(key, expiryTime);
        }

        public IEnumerable<T> GetCollection<T>(string cacheKey) where T : IRedisHashEntry
        {
            var result = RedisDatabase.ListRange(cacheKey);
            if (result.Length > 0)
            {
                var items = result.ToList<T>().ToList();
                return items;
            }
            return Array.Empty<T>(); ;
        }

        public IEnumerable<T> GetOrSetCollection<T>(string cacheKey, TimeSpan cachePeriod, Func<IEnumerable<T>> cacheNotAvailableFunc) where T : IRedisHashEntry
        {
            var result = GetCollection<T>(cacheKey);
            if (result.Any())
            {
                return result;
            }

            var items = cacheNotAvailableFunc();
            SetCollection(cacheKey, items, cachePeriod);

            return items;
        }

        public IEnumerable<T> GetItemsFromCollection<T>(string collectionCacheKey, params int[] itemIds) where T : IRedisHashEntry
        {
            return Array.Empty<T>(); ;
        }
    }
}
