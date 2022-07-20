namespace RedisTest.Redis
{
    public interface IRedisCacheCollection
    {
        bool SetCollection<T>(string key, IEnumerable<T> cacheObject, TimeSpan expiryTime) where T : IRedisHashEntry;

        IEnumerable<T> GetCollection<T>(string cacheKey) where T : IRedisHashEntry;

        IEnumerable<T> GetOrSetCollection<T>(string cacheKey, TimeSpan cachePeriod, Func<IEnumerable<T>> cacheNotAvailableFunc) where T : IRedisHashEntry;

        IEnumerable<T> GetItemsFromCollection<T>(string collectionCacheKey, params int[] itemIds) where T : IRedisHashEntry;
    }
}
