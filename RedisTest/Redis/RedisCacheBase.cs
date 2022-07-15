using StackExchange.Redis;

namespace RedisTest.Redis
{
    public abstract class RedisCacheBase : IDisposable
    {
        protected IDatabase RedisDatabase { get; }
        protected ConnectionMultiplexer Connection { get; }

        //public string Type => this.Type;

        protected RedisCacheBase(string redisServer, string password = "", int redisDatabaseId = 0)
        {
            var config = new ConfigurationOptions
            {
                EndPoints = { redisServer },
                Password = password ?? "",
                DefaultDatabase = redisDatabaseId,
                AllowAdmin = true,
                SyncTimeout = 20000,
            };
            Connection = ConnectionMultiplexer.Connect(config);
            RedisDatabase = Connection.GetDatabase(redisDatabaseId);
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
