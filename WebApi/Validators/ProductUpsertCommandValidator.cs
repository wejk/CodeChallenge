using FluentValidation;
using KenTan.Api.Command;

namespace WebApi.Validators
{
    public class ProductUpsertCommandValidator : AbstractValidator<ProductUpsertCommand>
    {
        public ProductUpsertCommandValidator()
        {
            this.RuleFor(x => x.ProductCode).NotEmpty().NotNull().MaximumLength(36);
            this.RuleFor(x => x.ProductName).NotEmpty().NotNull().MaximumLength(20);
            this.RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(100);
            this.RuleFor(x => x.Price).Must(x => x > 0);
            this.RuleFor(x => x.DeliveryPrice).Must(x => x >= 0);
        }
    }
}
