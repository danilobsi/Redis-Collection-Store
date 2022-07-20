using RedisTest.Redis;

namespace RedisTest.Stub
{
    public class ComplexRedisCacheEntry : IRedisHashEntry
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Field1 { get; private set; }
        public string Field2 { get; private set; }
        public string Field3 { get; private set; }
        public string Field4 { get; private set; }
        public string Field5 { get; private set; }
        public string Field6 { get; private set; }
        public string Field7 { get; private set; }
        public string Field8 { get; private set; }
        public string Field20 { get; private set; }
        public string Field19 { get; private set; }
        public string Field18 { get; private set; }
        public string Field17 { get; private set; }
        public string Field16 { get; private set; }
        public string Field15 { get; private set; }
        public string Field14 { get; private set; }
        public string Field13 { get; private set; }
        public string Field12 { get; private set; }
        public string Field11 { get; private set; }
        public string Field10 { get; private set; }
        public string Field9 { get; private set; }

        public static IEnumerable<ComplexRedisCacheEntry> GenerateRandom(int numberOfEntries)
        {
            for (var i = 0; i < numberOfEntries; i++)
            {
                yield return new ComplexRedisCacheEntry
                {
                    Id = i,
                    Name = $"Name {Utils.RandomString(20)}",
                    Description = $"Description {Utils.RandomString(100)}",
                    Field1 = $"Description {Utils.RandomString(100)}",
                    Field2 = $"Description {Utils.RandomString(100)}",
                    Field3 = $"Description {Utils.RandomString(100)}",
                    Field4 = $"Description {Utils.RandomString(100)}",
                    Field5 = $"Description {Utils.RandomString(100)}",
                    Field6 = $"Description {Utils.RandomString(100)}",
                    Field7 = $"Description {Utils.RandomString(100)}",
                    Field8 = $"Description {Utils.RandomString(100)}",
                    Field9 = $"Description {Utils.RandomString(100)}",
                    Field10 = $"Description {Utils.RandomString(100)}",
                    Field11 = $"Description {Utils.RandomString(100)}",
                    Field12 = $"Description {Utils.RandomString(100)}",
                    Field13 = $"Description {Utils.RandomString(100)}",
                    Field14 = $"Description {Utils.RandomString(100)}",
                    Field15 = $"Description {Utils.RandomString(100)}",
                    Field16 = $"Description {Utils.RandomString(100)}",
                    Field17 = $"Description {Utils.RandomString(100)}",
                    Field18 = $"Description {Utils.RandomString(100)}",
                    Field19 = $"Description {Utils.RandomString(100)}",
                    Field20 = $"Description {Utils.RandomString(100)}",
                };
            }
        }
    }
}
