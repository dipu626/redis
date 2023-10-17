using Caching.Domain.Entities;
using EF.Core.Repository.Interface.Repository;

namespace Caching.Domain.Repositories
{
    public interface IProductRepository : ICommonRepository<Product>
    {
    }
}
