using System;
using System.Linq;
using KenTan.Api.Command;
using KenTan.Api.Query;
using KenTan.DataLayer;
using RefactorThis.Services;

namespace WebApi.Services
{
    public interface IProductService
    {
        (bool found, KenTan.Api.Models.Product) TryGetProductByCode(ProductQueryByProductCode query);
        KenTan.Api.Models.Product[] GetProducts(ProductQuery query);
        (bool found,KenTan.Api.Models.Product[]) TryGetProductsByName(ProductQuerytByProductName query);
        (bool success,KenTan.Api.Models.Product) TryAddProduct(ProductUpsertCommand command);
        (bool success, KenTan.Api.Models.Product) TryUpdateProduct(ProductUpsertCommand command);
        bool DeleteProduct(ProductDeleteCommand command);

        KenTan.Api.Models.ProductOption[] GetProductOptions(ProductOptionQueryByProductCode query);
        (bool found,KenTan.Api.Models.ProductOption) TryGetProductOptionQueryByProductCodeProductOptionId(ProductOptionQueryByProductCodeProductOptionId query);
        (bool success,KenTan.Api.Models.ProductOption) TryAddProductOption(ProductOptionAddCommand command);
        (bool success, KenTan.Api.Models.ProductOption) TryUpdateProductOption(ProductOptionUpdateCommand command);
        void DeleteProductOption(ProductOptionDeleteCommand command);
    }

    public class ProductService : IProductService
    {
        private const int DefaultLimit = 10;
        public IProductRepository Repository { get; }
        public ProductService(IProductRepository repository)
        {
            Repository = repository;
        }

        public KenTan.Api.Models.Product[] GetProducts(ProductQuery query)
        {
            var products = Repository.GetProducts(query.Limit == 0 ? DefaultLimit : query.Limit);
            return products.Select(p => ToApiProduct(p)).ToArray();
        }

        public (bool found, KenTan.Api.Models.Product) TryGetProductByCode(ProductQueryByProductCode query)
        {
            var domainProduct = Repository.GetProductByProductCode(query.ProductCode);

            if (domainProduct == null)
            {
                return (false, null);
            }
            return (true,ToApiProduct(domainProduct));
        }

        public (bool found, KenTan.Api.Models.Product[]) TryGetProductsByName(ProductQuerytByProductName query)
        {
            var foundProducts = Repository.GetProductsByProductName(query.ProductName);
            if(!foundProducts.Any()) 
            {
                return (false,null);
            }

            return (true, foundProducts.Select(p => ToApiProduct(p)).ToArray());
        }

        private static KenTan.Api.Models.Product ToApiProduct(KenTan.DataLayer.Models.Product product)
        {
            return new KenTan.Api.Models.Product
            {
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                DeliveryPrice = product.DeliveryPrice,
                Price = product.Price
            };
        }
        private static KenTan.Api.Models.ProductOption ToApiProductOption(KenTan.DataLayer.Models.ProductOption productOption)
        {
            return new KenTan.Api.Models.ProductOption
            {
                ProductCode = productOption.ProductCode,
                Name = productOption.Name,
                Description = productOption.Description,
                ProductOptionId = productOption.ProductOptionId
            };
        }
        private static KenTan.DataLayer.Models.Product ToDomainProduct(KenTan.Api.Models.Product product)
        {
            return new KenTan.DataLayer.Models.Product
            {
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                DeliveryPrice = product.DeliveryPrice,
                Price = product.Price
            };
        }

        public (bool,KenTan.Api.Models.Product) TryAddProduct(ProductUpsertCommand command)
        {
            var added = Repository.UpsertProduct(new KenTan.DataLayer.Models.Product
            {
                ProductCode = command.ProductCode,
                Name = command.ProductName,
                Description = command.Description,
                Price = command.Price,
                DeliveryPrice= command.DeliveryPrice,
            });
            return added == null ? (false, null) : (true, ToApiProduct(added));
        }

        public (bool success, KenTan.Api.Models.Product) TryUpdateProduct(ProductUpsertCommand command)
        {
            var updated = Repository.UpsertProduct(new KenTan.DataLayer.Models.Product
            {
                ProductCode = command.ProductCode,
                Name = command.ProductName,
                Description = command.Description,
                Price = command.Price,
                DeliveryPrice = command.DeliveryPrice,
            });
            return updated == null ? (false, null) : (true, ToApiProduct(updated));
        }

        public bool DeleteProduct(ProductDeleteCommand command)
        {
            return Repository.DeleteProductByProductCode(command.ProductCode) > 0;
        }

        public KenTan.Api.Models.ProductOption[] GetProductOptions(ProductOptionQueryByProductCode query)
        {
            return Repository.GetProductOptionsByProductCode(query.ProductCode)
                    .Select(p => ToApiProductOption(p)).ToArray();
        }

        public (bool found, KenTan.Api.Models.ProductOption) TryGetProductOptionQueryByProductCodeProductOptionId(ProductOptionQueryByProductCodeProductOptionId query)
        {
            var productionOption = Repository.GetProductOptionsByProductCodeProductOptionId(query.ProductCode, query.ProductOptionId);
            if (productionOption != null)
            {
                return (true, ToApiProductOption(productionOption));
            }
            return (false, null);
        }

        public (bool success, KenTan.Api.Models.ProductOption) TryAddProductOption(ProductOptionAddCommand command)
        {
            var added = Repository.UpsertProductOption(new KenTan.DataLayer.Models.ProductOption
            { 
                Description = command.Description,
                Name = command.Name,
                ProductCode = command.ProductCode,
                ProductOptionId = Guid.NewGuid(),
            });
            return added == null ? (false, null) : (true, ToApiProductOption(added));
        }

        public (bool success, KenTan.Api.Models.ProductOption) TryUpdateProductOption(ProductOptionUpdateCommand command)
        {
            var updated = Repository.UpsertProductOption(new KenTan.DataLayer.Models.ProductOption
            {
                Description = command.Description,
                Name = command.Name,
                ProductCode = command.ProductCode,
                ProductOptionId = command.ProductOptionId
            });
            return updated == null ? (false, null) : (true, ToApiProductOption(updated));
        }

        public void DeleteProductOption(ProductOptionDeleteCommand command)
        {
            Repository.DeleteProductOption(command.ProductCode, command.ProductOptionId);
        }
    }
}
