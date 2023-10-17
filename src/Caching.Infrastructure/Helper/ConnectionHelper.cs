using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Caching.Infrastructure.Helper
{
    public class ConnectionHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        private static IConfiguration configuration;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        //public ConnectionHelper(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}

        static ConnectionHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configuration["RedisUrl"]);
            });
        }
    }
}
