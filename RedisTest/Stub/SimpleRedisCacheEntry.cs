using RedisTest.Redis;

namespace RedisTest.Stub
{
    public class SimpleRedisCacheEntry : IRedisHashEntry
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public static IEnumerable<SimpleRedisCacheEntry> GenerateRandom(int numberOfEntries)
        {
            for (var i = 0; i < numberOfEntries; i++)
            {
                yield return new SimpleRedisCacheEntry
                {
                    Id = i,
                    Name = $"Name {Utils.RandomString(20)}",
                    Description = $"Description {Utils.RandomString(100)}",
                };
            }
        }
    }
}
