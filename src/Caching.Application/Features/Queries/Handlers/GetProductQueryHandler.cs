using AutoMapper;
using Base.Application.Dtos.Responses;
using Base.Application.Extensions;
using Caching.Application.Dtos;
using Caching.Application.Features.Queries.Validators;
using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Caching.Application.Features.Queries.Handlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, CommonResponse<ProductResponse>>
    {
        private readonly ILogger<GetProductQueryHandler> _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly GetProductQueryValidator _getProductQueryValidator;
        private readonly IConfiguration _configuration;

        public GetProductQueryHandler(ILogger<GetProductQueryHandler> logger,
                                      ICacheRepository cacheRepository,
                                      IProductRepository productRepository,
                                      IMapper mapper,
                                      GetProductQueryValidator getProductQueryValidator,
                                      IConfiguration configuration)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _getProductQueryValidator = getProductQueryValidator;
            _configuration = configuration;
        }

        public async Task<CommonResponse<ProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _getProductQueryValidator.ValidateAsync(request);
                validator.EnsureValidationResult();

                var cachedData = await _cacheRepository.GetDataAsync<List<Product>>("product");
                if (cachedData is not null)
                {
                    var filteredData = cachedData.FirstOrDefault(x => x.Id == request.Id);

                    return CommonResponse<ProductResponse>.BuildSuccessResponse(records: new List<ProductResponse> { _mapper.Map<ProductResponse>(filteredData) });
                }
                var expirationTime = DateTimeOffset.Now.AddSeconds(double.Parse(_configuration["Redis:ExpirationTime"]));
                var products = await _productRepository.GetAllAsync();
                await _cacheRepository.SetDataAsync(key: "product", value: products, expirationTime: expirationTime);

                var result = products.FirstOrDefault(x => x.Id == request.Id);

                return CommonResponse<ProductResponse>.BuildSuccessResponse(records: new List<ProductResponse> { _mapper.Map<ProductResponse>(result) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return CommonResponse<ProductResponse>.BuildErrorResponse(errors: ErrorResponse.BuildExternalError(ex.Message));
            }
        }
    }
}
