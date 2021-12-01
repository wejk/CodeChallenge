using FluentValidation;
using RefactorThis.Services;

namespace WebApi.Validators
{
    public class ProductQueryByProductCodeValidator : AbstractValidator<ProductQueryByProductCode>
    {
        public ProductQueryByProductCodeValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull();
        }
    }
}
