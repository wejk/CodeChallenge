using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using KenTan.DataLayer;
using KenTan.DataLayer.Models;
using System.Linq;
using Shouldly;
using System;
using WebApi;

namespace Test.Integration
{
    public class WebApiApplicationFactory : WebApplicationFactory<Startup>
    {
    }

    public class ProductRepositoryTest : IClassFixture<WebApiApplicationFactory>
    {
        public WebApiApplicationFactory _apiFactory { get; }
        public ProductRepositoryTest(WebApiApplicationFactory apiFactory)
        {
            _apiFactory = apiFactory;
        }

        [Fact]
        public void Repo_CanAddProduct()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test001";
            var testProduct = CreateTestProduct(productCode, "TestName");

            // Act
            sut.UpsertProduct(testProduct);

            try
            {
                var actual = dbContext.Products.Single(p => p.ProductCode == productCode);
                // Assert
                actual.Name.ShouldBe("TestName");
            }
            finally
            {
               CleanUpTestProduct(testProduct, dbContext);
            }
        }

        [Fact]
        public void Repo_CanUpdateProduct()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test002";
            var testProduct = CreateTestProduct(productCode);
            AddProduct(testProduct, dbContext);
            testProduct.Name = "UpdateForTest";

            // Act
            sut.UpsertProduct(testProduct);

            try
            {
                var actual = dbContext.Products.Single(p => p.ProductCode == productCode);
                // Assert
                actual.Name.ShouldBe("UpdateForTest");
                actual.Description.ShouldBe("TestDescription");
            }
            finally
            {
                CleanUpTestProduct(testProduct,dbContext);
            }
        }

        [Fact]
        public void Repo_CanSearchProductByProductCode()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test003";
            var testProduct = CreateTestProduct(productCode, "SearchByProductCode");
            AddProduct(testProduct, dbContext);
            
            // Act
            var actual = sut.GetProductByProductCode(productCode);
            CleanUpTestProduct(testProduct,dbContext);

            // Assert
            actual.Name.ShouldBe("SearchByProductCode");
            actual.ProductCode.ShouldBe(productCode);
        }

        [Fact]
        public void Repo_CanSearchProductByName_ShouldBeCaseInsensive()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test004";
            var testProduct = CreateTestProduct(productCode, "This is the best product");
            AddProduct(testProduct, dbContext);
            
            // Act
            var actual = sut.GetProductsByProductName("Product").First();
            CleanUpTestProduct(testProduct,dbContext);

            // Assert
            actual.Name.ShouldBe("This is the best product");
            actual.ProductCode.ShouldBe(productCode);
        }

        [Fact]
        public void Repo_CanGetAllProductsByLimit()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode5 = "Test005";
            var productCode6 = "Test006";
            var testProduct5 = CreateTestProduct(productCode5, "product5");
            var testProduct6 = CreateTestProduct(productCode6, "product6");

            // Add two products
            AddProduct(testProduct5, dbContext);
            AddProduct(testProduct6, dbContext);
            
            // Act
            // get limit by 1
            var actual = sut.GetProducts(1).Length;
            CleanUpTestProduct(testProduct5,dbContext);
            CleanUpTestProduct(testProduct6,dbContext);

            // Assert
            actual.ShouldBe(1);
        }
       
        [Fact]
        public void Repo_CanAddProductOption()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test007";
            var productOptionId = Guid.NewGuid();
            var testProduct = CreateTestProductOption(productOptionId, productCode, "TestName");

            // Act
            sut.UpsertProductOption(testProduct);

            try
            {
                var actual = dbContext.ProductOptions.Single(p => p.ProductCode == productCode);
                // Assert
                actual.Name.ShouldBe("TestName");
                actual.ProductOptionId.ShouldBe(productOptionId);
            }
            finally
            {
                CleanUpTestProductOption(testProduct, dbContext);
            }
        }

        [Fact]
        public void Repo_CanUpdateProductOption()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test008";
            var productOptionId = Guid.NewGuid();

            var testProduct = CreateTestProductOption(productOptionId, productCode);
            AddProductOption(testProduct, dbContext);
            testProduct.Name = "UpdateForTest";

            // Act
            sut.UpsertProductOption(testProduct);

            try
            {
                var actual = dbContext.ProductOptions.Single(p => p.ProductCode == productCode);
                // Assert
                actual.Name.ShouldBe("UpdateForTest");
                actual.Description.ShouldBe("TestDescription");
            }
            finally
            {
                CleanUpTestProductOption(testProduct, dbContext);
            }
        }

        [Fact]
        public void Repo_CanSearchProductOptionByProductCode()
        {
            // Arrange
            using var scope = _apiFactory.Services.CreateScope();
            var sut = scope.ServiceProvider.GetService<IProductRepository>();
            var dbContext = scope.ServiceProvider.GetService<ProductDbConext>();
            var productCode = "Test009";
            var productOptionId = Guid.NewGuid();

            var testProduct = CreateTestProductOption(productOptionId, productCode, "SearchByProductCode");
            AddProductOption(testProduct, dbContext);

            // Act
            var actual = sut.GetProductOptionsByProductCode(productCode).First();
            CleanUpTestProductOption(testProduct, dbContext);

            // Assert
            actual.Name.ShouldBe("SearchByProductCode");
            actual.ProductCode.ShouldBe(productCode);
        }

        private Product CreateTestProduct(
             string productCode,
             string name = "TestName",
             string description ="TestDescription",
             decimal price = 1,
             decimal deliveryPrice = 1
        )
        {
            return new Product
            {
                ProductCode = productCode,
                Name = name,
                Description = description,
                Price = price,
                DeliveryPrice = deliveryPrice
            };
        }
        
        private ProductOption CreateTestProductOption(
             Guid productOptionId,
             string productCode,
             string name = "TestName",
             string description ="TestDescription"
        )
        {
            return new ProductOption
            {
                ProductOptionId = productOptionId,
                ProductCode = productCode,
                Name = name,
                Description = description
            };
        }

        private void AddProduct (Product product, ProductDbConext dbContext)
        {
            dbContext.Products.Add(product);
            dbContext.SaveChanges ();
        }

        private void CleanUpTestProduct(Product product, ProductDbConext dbContext)
        {
            dbContext.Remove(product);
            dbContext.SaveChanges();
        }

        private void AddProductOption (ProductOption product, ProductDbConext dbContext)
        {
            dbContext.ProductOptions.Add(product);
            dbContext.SaveChanges ();
        }

        private void CleanUpTestProductOption(ProductOption product, ProductDbConext dbContext)
        {
            dbContext.Remove(product);
            dbContext.SaveChanges();
        }
    }
}
