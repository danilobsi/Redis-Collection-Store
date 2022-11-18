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

//using RedisTest.Redis;

//const string cacheKey = "myRedisHashKey";

////Creates a new redis cache instance 
//var cacheInstance = new RedisHash("127.0.0.1");

////Create the employee instances
//var employeeJohn = new Employee
//{
//    Id = 1,
//    Name = "John White",
//    UserName = "john.white"
//};

//var employeeJack = new Employee
//{
//    Id = 2,
//    Name = "Jack Black",
//    UserName = "jack.black"
//};

//var employeeLucas = new Employee
//{
//    Id = 3,
//    Name = "Lucas Brown",
//    UserName = "lucas.brown"
//};

////Sets the employees in the collection
//cacheInstance.SetCollection(
//    cacheKey, 
//    TimeSpan.FromMinutes(5),
//    new List<(string, Employee)>
//    {
//        (employeeJohn.UserName, employeeJohn),
//        (employeeJack.UserName, employeeJack),
//        (employeeLucas.UserName, employeeLucas)
//    });

////Gets all the employees from Redis
//var allEmployees = cacheInstance.GetCollection<Employee>(cacheKey);

//var jackAndJohn = cacheInstance.GetItemsFromCollection<Employee>(cacheKey, "jack.black", "john.white");

//Console.ReadLine();

//public class Employee
//{
//    public int Id { get; init; }
//    public string Name { get; init; }
//    public string UserName { get; init; }
//}