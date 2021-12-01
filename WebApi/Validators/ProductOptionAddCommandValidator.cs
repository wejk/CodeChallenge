using FluentValidation;
using KenTan.Api.Command;

namespace WebApi.Validators
{
    public class ProductOptionAddCommandValidator : AbstractValidator<ProductOptionAddCommand>
    {
        public ProductOptionAddCommandValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull().MaximumLength(36);
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(20);
            this.RuleFor(x => x.Description).MaximumLength(100);
        }
    }
}
