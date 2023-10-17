using Base.Application.Dtos.Responses;
using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Caching.Application.Features.Queries.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, CommonResponse<Product>>
    {
        private readonly ILogger<GetProductsQueryHandler> _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public GetProductsQueryHandler(ILogger<GetProductsQueryHandler> logger,
                                       ICacheRepository cacheRepository,
                                       IProductRepository productRepository,
                                       IConfiguration configuration)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
            _productRepository = productRepository;
            _configuration = configuration;
        }

        public async Task<CommonResponse<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheData = await _cacheRepository.GetDataAsync<List<Product>>(key: "product");
                if (cacheData is not null)
                {
                    return CommonResponse<Product>.BuildSuccessResponse(records: cacheData);
                }

                var expirationTime = DateTimeOffset.Now.AddSeconds(double.Parse(_configuration["Redis:ExpirationTime"]));
                cacheData = (List<Product>?)await _productRepository.GetAllAsync();
                await _cacheRepository.SetDataAsync(key: "product", value: cacheData, expirationTime: expirationTime);

                return CommonResponse<Product>.BuildSuccessResponse(records: cacheData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return CommonResponse<Product>.BuildErrorResponse(errors: ErrorResponse.BuildExternalError(ex.Message));
            }
        }
    }
}
