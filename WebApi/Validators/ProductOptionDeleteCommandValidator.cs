using FluentValidation;
using KenTan.Api.Command;

namespace WebApi.Validators
{
    public class ProductOptionDeleteCommandValidator : AbstractValidator<ProductOptionDeleteCommand>
    {
        public ProductOptionDeleteCommandValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull();
            this.RuleFor(x => x.ProductOptionId).NotEmpty().NotNull();
        }
    }
}
