using Microsoft.Extensions.Configuration;

namespace Caching.Infrastructure.Helper
{
    public class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; }

        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
