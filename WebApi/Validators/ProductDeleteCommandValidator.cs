using FluentValidation;
using KenTan.Api.Command;

namespace WebApi.Validators
{
    public class ProductDeleteCommandValidator : AbstractValidator<ProductDeleteCommand>
    {
        public ProductDeleteCommandValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull();
        }
    }
}
