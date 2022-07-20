using System;
using System.Linq;
using Newtonsoft.Json;
using RedisTest;
using RedisTest.Redis;
using RedisTest.Stub;
using Xunit;

namespace Redis.Tests
{
    public class IRedisCacheCollectionTests
    {
        [Theory]
        [InlineData(typeof(RedisCacheHashParallel))]
        [InlineData(typeof(RedisCacheHash))]
        [InlineData(typeof(RedisCacheList))]
        public void GetOrSetCollection_Success(Type type)
        {
            // Arrange
            var redisKey = Utils.RandomString(100);
            var simpleItems = SimpleRedisCacheEntry.GenerateRandom(numberOfEntries: 100).ToList();
            var sut = Setup(type);

            // Act
            var result = sut.GetOrSetCollection(redisKey, TimeSpan.FromSeconds(5), () => simpleItems);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(simpleItems), JsonConvert.SerializeObject(result));
        }

        [Theory]
        [InlineData(typeof(RedisCacheHashParallel))]
        [InlineData(typeof(RedisCacheHash))]
        [InlineData(typeof(RedisCacheList))]
        public void GetItemsFromCollection_Success(Type type)
        {
            // Arrange
            var redisKey = Utils.RandomString(100);
            var simpleItems = SimpleRedisCacheEntry.GenerateRandom(numberOfEntries: 100).ToList();
            var sut = Setup(type);
            sut.SetCollection(redisKey, simpleItems, TimeSpan.FromSeconds(5));
            var expectedItems = simpleItems.Take(10);

            // Act
            var result = sut.GetItemsFromCollection<SimpleRedisCacheEntry>(redisKey, expectedItems.Select(i => i.Id).ToArray());

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedItems), JsonConvert.SerializeObject(result));
        }

        private IRedisCacheCollection Setup(Type type)
        {
            if (type == typeof(RedisCacheHashParallel))
            {
                return new RedisCacheHashParallel("127.0.0.1");
            }
            if (type == typeof(RedisCacheList))
            {
                return new RedisCacheList("127.0.0.1");
            }
            return new RedisCacheHash("127.0.0.1");
        }
    }
}