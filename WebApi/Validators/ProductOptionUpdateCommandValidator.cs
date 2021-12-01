using FluentValidation;
using KenTan.Api.Command;

namespace WebApi.Validators
{
    public class ProductOptionUpdateCommandValidator : AbstractValidator<ProductOptionUpdateCommand>
    {
        public ProductOptionUpdateCommandValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull().MaximumLength(36);
            this.RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(20);
            this.RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(100);
            this.RuleFor(x => x.ProductOptionId).NotEmpty().NotNull();
        }
    }
}
