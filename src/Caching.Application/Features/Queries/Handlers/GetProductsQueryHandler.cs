using AutoMapper;
using Base.Application.Dtos.Responses;
using Caching.Application.Dtos;
using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Caching.Application.Features.Queries.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, CommonResponse<ProductResponse>>
    {
        private readonly ILogger<GetProductsQueryHandler> _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(ILogger<GetProductsQueryHandler> logger,
                                       ICacheRepository cacheRepository,
                                       IProductRepository productRepository,
                                       IConfiguration configuration,
                                       IMapper mapper)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
            _productRepository = productRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CommonResponse<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheData = await _cacheRepository.GetDataAsync<List<Product>>(key: "product");
                if (cacheData is not null)
                {
                    return CommonResponse<ProductResponse>.BuildSuccessResponse(records: _mapper.Map<List<ProductResponse>>(cacheData));
                }

                var expirationTime = DateTimeOffset.Now.AddSeconds(double.Parse(_configuration["Redis:ExpirationTime"]));
                cacheData = (List<Product>?)await _productRepository.GetAllAsync();
                await _cacheRepository.SetDataAsync(key: "product", value: cacheData, expirationTime: expirationTime);

                return CommonResponse<ProductResponse>.BuildSuccessResponse(records: _mapper.Map<List<ProductResponse>>(cacheData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return CommonResponse<ProductResponse>.BuildErrorResponse(errors: ErrorResponse.BuildExternalError(ex.Message));
            }
        }
    }
}
