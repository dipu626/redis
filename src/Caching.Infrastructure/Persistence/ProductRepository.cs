using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using Caching.Infrastructure.Providers;
using EF.Core.Repository.Repository;

namespace Caching.Infrastructure.Persistence
{
    public class ProductRepository : CommonRepository<Product>, IProductRepository
    {
        public ProductRepository(CacheDbContext dbContext) : base(dbContext)
        {
        }
    }
}
