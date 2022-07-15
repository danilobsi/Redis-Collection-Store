using StackExchange.Redis;

namespace RedisTest.Redis
{
    internal class RedisCacheHashParallel : RedisCacheBase, IRedisCacheCollection
    {
        public RedisCacheHashParallel(string redisServer, string password = "", int redisDatabaseId = 0) : base(redisServer, password, redisDatabaseId) { }

        public bool SetCollection<T>(string key, IEnumerable<T> cacheObject, TimeSpan expiryTime) where T : IRedisHashEntry
        {
            var items = new HashEntry[cacheObject.Count()];
            Parallel.ForEach(cacheObject, (itm, _, index) => items[index] = itm.ToHashEntry());

            RedisDatabase.HashSet(key, items);
            return RedisDatabase.KeyExpire(key, expiryTime);
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

        public IEnumerable<T> GetCollection<T>(string cacheKey) where T : IRedisHashEntry
        {
            var result = RedisDatabase.HashGetAll(cacheKey);
            if (result.Length > 0)
            {
                var items = result.ToArray<T>();
                return items;
            }
            return Array.Empty<T>(); ;
        }

        public IEnumerable<T> GetItemsFromCollection<T>(string collectionCacheKey, params int[] itemIds) where T : IRedisHashEntry
        {
            var redisKeys = itemIds.Select(key => (RedisValue)key).ToArray();
            var hashResult = RedisDatabase.HashGet(collectionCacheKey, redisKeys);

            var items = hashResult.ToArray<T>();

            return items;
        }

        public bool SetItemInCollection<T>(string collectionCacheKey, T item) where T : IRedisHashEntry
        {
            return RedisDatabase.HashSet(collectionCacheKey, (RedisValue)item.Id, item.ToRedisValue());
        }

        public bool DeleteItemInCollection(string collectionCacheKey, int itemId)
        {
            return RedisDatabase.HashDelete(collectionCacheKey, itemId);
        }
    }
}
