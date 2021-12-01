using FluentValidation;
using KenTan.Api.Query;

namespace WebApi.Validators
{
    public class ProductOptionQueryByProductCodeProductOptionIdValidator : AbstractValidator<ProductOptionQueryByProductCodeProductOptionId>
    {
        public ProductOptionQueryByProductCodeProductOptionIdValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull();
            this.RuleFor(x => x.ProductOptionId).NotEmpty().NotNull();
        }
    }
}
