using Caching.Domain.Repositories;
using FluentValidation;

namespace Caching.Application.Features.Commands.Validators
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage($"{nameof(DeleteProductCommand.Id)} is not valid");
            RuleFor(x => x.Id).MustAsync(BeAnExistingProduct).WithMessage($"{nameof(DeleteProductCommand.Id)} is not exist");
        }

        private async Task<bool> BeAnExistingProduct(int productId, CancellationToken token)
        {
            var product = await _productRepository.GetFirstOrDefaultAsync(x => x.Id == productId);
            return product is not null;
        }
    }
}
