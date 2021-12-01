using Xunit;
using NSubstitute;
using KenTan.DataLayer;
using RefactorThis.Services;
using Shouldly;
using KenTan.DataLayer.Models;
using WebApi.Services;

/* 
 *  Testing the ProductService, we don't need to test with the heavy Controller
 *  This is just a small test example for this code challenge. 
 *  Should have written tests to cover all business logic.
 */

namespace Test.Unit
{
    public class ProductServiceTests
    {
        private readonly ProductService sut;
        private readonly IProductRepository repo;

        // Setup
        public ProductServiceTests()
        {
            repo = Substitute.For<IProductRepository>();
            sut = new ProductService(repo);
        }

        [Fact]
        public void ProductService_ShouldReturnFalse_WhenProductNotFound()
        {
            // Arrange
            repo.GetProductByProductCode(Arg.Any<string>()).Returns(x => null);

            // Act
            var (actual,product) = sut.TryGetProductByCode(new ProductQueryByProductCode { ProductCode = "test123" });

            // Assert
            actual.ShouldBeFalse();
            product.ShouldBeNull();
        } 
        
        [Fact]
        public void ProductService_ShouldReturnProduct_WhenProductFound()
        {
            // Arrange
            repo.GetProductByProductCode(Arg.Any<string>()).Returns(x => new Product { ProductCode = "test123", Name = "Test" });

            // Act
            var (actual,product) = sut.TryGetProductByCode(new ProductQueryByProductCode { ProductCode = "test123" });

            // Assert
            actual.ShouldBeTrue();
            product.ShouldNotBeNull();
        }
    }
}
