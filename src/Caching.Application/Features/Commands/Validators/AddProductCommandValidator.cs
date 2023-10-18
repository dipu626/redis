using FluentValidation;

namespace Caching.Application.Features.Commands.Validators
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage($"{nameof(AddProductCommand.ProductName)} is empty");
            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage($"{nameof(AddProductCommand.ProductDescription)} is empty");
        }
    }
}
