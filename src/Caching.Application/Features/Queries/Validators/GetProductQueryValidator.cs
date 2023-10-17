using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using FluentValidation;

namespace Caching.Application.Features.Queries.Validators
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheRepository _cacheRepository;

        public GetProductQueryValidator(IProductRepository productRepository,
                                        ICacheRepository cacheRepository)
        {
            _productRepository = productRepository;
            _cacheRepository = cacheRepository;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage($"{nameof(GetProductQuery.Id)} is not valid.");
            RuleFor(x => x.Id).MustAsync(BeAnExistingProductIdInCache).WithMessage($"{nameof(GetProductQuery.Id)} not exist.");
            RuleFor(x => x.Id).MustAsync(BeAnExistingProductId).WithMessage($"{nameof(GetProductQuery.Id)} not exist.");
        }

        private async Task<bool> BeAnExistingProductIdInCache(int productId, CancellationToken token)
        {
            var products = await _cacheRepository.GetDataAsync<List<Product>>("product");
            if (products is null)
            {
                return true;
            }

            var product = products?.FirstOrDefault(x => x.Id == productId);
            return product is not null;
        }

        private async Task<bool> BeAnExistingProductId(int productId, CancellationToken token)
        {
            var product = await _productRepository.GetFirstOrDefaultAsync(x => x.Id == productId);
            return product is not null;
        }
    }
}
