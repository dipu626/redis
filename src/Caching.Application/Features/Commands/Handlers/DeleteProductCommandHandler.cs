using Base.Application.Dtos.Responses;
using Base.Application.Extensions;
using Caching.Application.Features.Commands.Validators;
using Caching.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Caching.Application.Features.Commands.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommonResponse<bool>>
    {
        private readonly ILogger<DeleteProductCommandHandler> _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IProductRepository _productRepository;
        private readonly DeleteProductCommandValidator _deleteProductCommandValidator;

        public DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger,
                                           ICacheRepository cacheRepository,
                                           IProductRepository productRepository,
                                           DeleteProductCommandValidator deleteProductCommandValidator)
        {
            _logger = logger;
            _cacheRepository = cacheRepository;
            _productRepository = productRepository;
            _deleteProductCommandValidator = deleteProductCommandValidator;
        }

        public async Task<CommonResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteProductCommandValidator.ValidateAsync(request);
                validator.EnsureValidationResult();

                var product = await _productRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);
                var result = await _productRepository.DeleteAsync(product);

                await _cacheRepository.DeleteAllAsync();

                return CommonResponse<bool>.BuildSuccessResponse(records: new List<bool> { result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return CommonResponse<bool>.BuildErrorResponse(errors: ErrorResponse.BuildExternalError(ex.Message));
            }
        }
    }
}
