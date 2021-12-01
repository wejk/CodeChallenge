using Xunit;
using KenTan.Api.Command;
using FluentValidation.TestHelper;
using WebApi.Validators;

/*
  This is a demonstration of unit test for one of the Validator
*/
namespace Test.Unit
{
    public class ProductAddCommandValidatorTests
    {
        private readonly ProductAddCommandValidator sut;
        public ProductAddCommandValidatorTests()
        {
            sut = new ProductAddCommandValidator();
        }

        [Fact]
        public void Should_have_Error_WhenProductCode_Is_null()
        {
            var model = new ProductUpsertCommand{ ProductCode = null};
            var result = sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProductCode);
        }

        [Fact]
        public void Should_have_Error_WhenProductCode_Length_is_greater_than_36_chars()
        {
            var model = new ProductUpsertCommand{ ProductCode = "ProductCodeIsMoreThanThirtySixCharactersInLength"};
            var result = sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ProductCode);
        }

        [Fact]
        public void Should_have_Error_WhenProductPrice_Is_Less_than_zero()
        {
            var model = new ProductUpsertCommand{ Price = -1 };
            var result = sut.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_have_not_Error_When_required_properties_provided()
        {
            var model = new ProductUpsertCommand
            {
                ProductCode = "test",
                ProductName = "test",
                Description = "test",
                Price = 1,
                DeliveryPrice = 1
            };
            var result = sut.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ProductCode);
            result.ShouldNotHaveValidationErrorFor(x => x.ProductName);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
            result.ShouldNotHaveValidationErrorFor(x => x.DeliveryPrice);
        }
    }
}
