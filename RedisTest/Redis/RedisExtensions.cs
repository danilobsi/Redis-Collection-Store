using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisTest.Redis
{
    public static class RedisExtensions
    {
        static readonly JsonSerializerSettings _serializer = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        public static HashEntry ToHashEntry(this IRedisHashEntry obj)
        {
            return new HashEntry(obj.Id, obj.ToRedisValue());
        }

        public static RedisValue ToRedisValue(this IRedisHashEntry obj)
        {
            return JsonConvert.SerializeObject(obj, _serializer);
        }

        public static IEnumerable<T> ToList<T>(this HashEntry[] obj)
        {
            return obj
                .Where(e => e.Value.HasValue)
                .Select(e => JsonConvert.DeserializeObject<T>(e.Value))
                .ToList();
        }

        public static IEnumerable<T> ToList<T>(this RedisValue[] obj)
        {
            return obj
                .Where(e => e.HasValue)
                .Select(e => JsonConvert.DeserializeObject<T>(e))
                .ToList();
        }

        public static IEnumerable<T> ToArray<T>(this HashEntry[] items)
        {
            var itemsWithValue = items.Where(e => e.Value.HasValue).ToArray();
            var convertedItems = new T[itemsWithValue.Length];

            Parallel.ForEach(itemsWithValue, (item, _, index) =>
                convertedItems[index] = JsonConvert.DeserializeObject<T>(item.Value));

            return convertedItems;
        }

        public static IEnumerable<T> ToArray<T>(this RedisValue[] items)
        {
            var itemsWithValue = items.Where(e => e.HasValue).ToArray();
            var convertedItems = new T[itemsWithValue.Length];

            Parallel.ForEach(itemsWithValue, (item, _, index) =>
                convertedItems[index] = JsonConvert.DeserializeObject<T>(item));

            return convertedItems;
        }
    }
}
