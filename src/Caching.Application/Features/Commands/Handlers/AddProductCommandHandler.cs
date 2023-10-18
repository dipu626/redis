using AutoMapper;
using Base.Application.Dtos.Responses;
using Base.Application.Extensions;
using Caching.Application.Features.Commands.Validators;
using Caching.Domain.Entities;
using Caching.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Caching.Application.Features.Commands.Handlers
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, CommonResponse<bool>>
    {
        private readonly ILogger<AddProductCommandHandler> _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly AddProductCommandValidator _addProductCommandValidator;

        public AddProductCommandHandler(ILogger<AddProductCommandHandler> logger,
                                        ICacheRepository cacheRepository,
                                        IProductRepository productRepository,
                                        IMapper mapper,
                                        AddProductCommandValidator addProductCommandValidator)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _addProductCommandValidator = addProductCommandValidator;
        }

        public async Task<CommonResponse<bool>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _addProductCommandValidator.ValidateAsync(request);
                validator.EnsureValidationResult();

                var product = _mapper.Map<Product>(request);
                var result = await _productRepository.AddAsync(product);

                await _cacheRepository.DeleteAllAsync();

                return CommonResponse<bool>.BuildSuccessResponse(records: new List<bool> { result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return CommonResponse<bool>.BuildErrorResponse(ErrorResponse.BuildExternalError(errors: ex.Message));
            }
        }
    }
}
