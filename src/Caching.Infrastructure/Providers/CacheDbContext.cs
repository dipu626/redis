using Caching.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caching.Infrastructure.Providers
{
    public class CacheDbContext : DbContext
    {
        public CacheDbContext(DbContextOptions<CacheDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
