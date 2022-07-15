using RedisTest;
using RedisTest.Redis;
using RedisTest.Stub;

//Get Mock list
var simpleItems = SimpleRedisCacheEntry.GenerateRandom(numberOfEntries: 80000).ToList();
var complexItems = ComplexRedisCacheEntry.GenerateRandom(numberOfEntries: 20000).ToList();

//Execute tests
GenerateTests(simpleItems);
GenerateTests(complexItems);

static void GenerateTests<T>(List<T> items) where T : IRedisHashEntry
{
    var cacheExpirationPeriod = TimeSpan.FromSeconds(5);

    int arraySize = 1000;
    var randomIdList = new int[arraySize];
    for (int i = 0; i < arraySize; i++)
    {
        randomIdList[i] = Random.Shared.Next(items.Count);
    }

    //Redis Collection
    IRedisCacheCollection[] instances = new[] {
        //(IRedisCacheCollection)new RedisCacheHash("127.0.0.1"),
        (IRedisCacheCollection)new RedisCacheHashParallel("127.0.0.1"),
        //(IRedisCacheCollection)new RedisCacheList("127.0.0.1")
    };

    for (var i = 0; i < instances.Length; i++)
    {
        var redisInstance = instances[i];
        var collectionCacheKey = $"{instances[i].GetType()}";

        Console.WriteLine(collectionCacheKey);
        DSBenchmark.Print(() => redisInstance.SetCollection(collectionCacheKey, items, cacheExpirationPeriod), $"SetCollection({items.Count})");
        DSBenchmark.Print(() => redisInstance.GetCollection<SimpleRedisCacheEntry>(collectionCacheKey), $"GetCollection({items.Count})");
        DSBenchmark.Print(() => redisInstance.GetItemsFromCollection<SimpleRedisCacheEntry>(collectionCacheKey, randomIdList), $"GetItemsFromCollection({arraySize})");
        Console.WriteLine();
    }


    //Redis Set
    var redisSet = new RedisCache("127.0.0.1");
    string setCacheKey = $"{redisSet.GetType()}";

    Console.WriteLine(setCacheKey);
    DSBenchmark.Print(() => redisSet.Set(setCacheKey, items, cacheExpirationPeriod), $"Set({items.Count})");
    DSBenchmark.Print(() => redisSet.Get<IEnumerable<SimpleRedisCacheEntry>>(setCacheKey), $"Get({items.Count})");
    DSBenchmark.Print(() => redisSet.Get<IEnumerable<SimpleRedisCacheEntry>>(setCacheKey).Where(i => randomIdList.Contains(i.Id)), $"GetItems({arraySize})");
    Console.WriteLine();
}