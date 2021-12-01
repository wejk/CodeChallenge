using FluentValidation;
using RefactorThis.Services;

namespace WebApi.Validators
{
    public class ProductQueryByProductNameValidator : AbstractValidator<ProductQuerytByProductName>
    {
        public ProductQueryByProductNameValidator()
        {
            this.RuleFor(x => x.ProductName).NotEmpty().NotNull();
        }
    }
}
