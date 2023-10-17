using FluentValidation.Results;

namespace Base.Application.Extensions
{
    public static class ValidationResultExtension
    {
        public static bool EnsureValidationResult(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return true;
            }
            throw new Exception(message: string.Join(Environment.NewLine, validationResult.Errors.Select(x => x.ErrorMessage)));
        }
    }
}
