using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Caching.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheRepository _cacheRepository;

        public ProductController(IProductRepository productRepository,
                                 ICacheRepository cacheRepository)
        {
            _productRepository = productRepository;
            _cacheRepository = cacheRepository;
        }

        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var cacheData = await _cacheRepository.GetDataAsync<IEnumerable<Product>>("product");

            if (cacheData is not null)
            {
                return cacheData;
            }

            var expirationTime = DateTimeOffset.Now.AddMinutes(5);
            cacheData = await _productRepository.GetAllAsync();
            await _cacheRepository.SetDataAsync(key: "product", value: cacheData, expirationTime: expirationTime);

            return cacheData;
        }

    }
}
