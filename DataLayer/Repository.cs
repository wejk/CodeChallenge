using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using KenTan.DataLayer.Models;

namespace KenTan.DataLayer
{
    public interface IProductRepository
    {
        Product GetProductByProductCode(string productCode);
        Product[] GetProducts(int limit);
        Product[] GetProductsByProductName(string productName);
        Product UpsertProduct(Product product);
        int DeleteProductByProductCode(string productCode);

        ProductOption[] GetProductOptionsByProductCode(string productCode);
        ProductOption GetProductOptionsByProductCodeProductOptionId(string productCode, Guid productOptionId);
        ProductOption UpsertProductOption(ProductOption productOption);
        ProductOption UpsertProductOption(string productCode, ProductOption productOption);
        void DeleteProductOption(string productCode, Guid productOptionId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbConext _dbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ProductDbConext dbConext, ILogger<ProductRepository> logger)
        {
            _dbContext = dbConext;
            _logger = logger;
        }

        public Product UpsertProduct(Product product)
        {
            try
            {
                var existed = _dbContext.Products.FirstOrDefault(p => p.ProductCode == product.ProductCode);
                if (existed == null)
                {
                    _dbContext.Products.Add(product);
                    _dbContext.SaveChanges();
                    return product;
                }
                else
                {
                    existed.Name = product.Name;
                    existed.Description = product.Description;
                    existed.Price = product.Price;
                    existed.DeliveryPrice = product.DeliveryPrice;
                    _dbContext.Update(existed);
                    _dbContext.SaveChanges();
                    return existed;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upsert product: [{product}]", product);
                return product;
            }
        }

        public Product GetProductByProductCode(string productCode)
        {
            try
            {
                return _dbContext.Products.Single(p => p.ProductCode == productCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieve products from db for productId:[{productCode}]", productCode);
                return null;
            }
        }

        public Product[] GetProducts(int limit)
        {
            try
            {
                return _dbContext.Products.Take(limit).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,  $"Error while retrieve products from db");
                return new Product[0];
            }
        }

        public Product[] GetProductsByProductName(string productName)
        {
            try
            {
                var lowerCaseProductName = productName.ToLower();
                return _dbContext.Products.Where(p => p.Name.ToLower().Contains(lowerCaseProductName)).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while find product by [{productName}]", productName) ;
                return new Product[0];
            }
        }

        public int DeleteProductByProductCode(string productCode)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
                int count = 0;
            try
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.ProductCode == productCode);
                var productOptions = _dbContext.ProductOptions.Where(p => p.ProductCode == productCode);
                if(product != null)
                {
                    _dbContext.Products.Remove(product);
                    _dbContext.SaveChanges();
                }

                if (productOptions.Any())
                {
                    _dbContext.ProductOptions.RemoveRange(productOptions);
                    count = _dbContext.SaveChanges();
                }
                transaction.Commit();
                return count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while find delete product by [{productCode}]", productCode);
                return count;
            }
        }

        public ProductOption[] GetProductOptionsByProductCode(string productCode)
        {
            try
            {
                return _dbContext.ProductOptions.Where(p => p.ProductCode == productCode).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding ProductOption by [{productCode}]", productCode);
                return new ProductOption[0];
            }
        }

        public ProductOption GetProductOptionsByProductCodeProductOptionId(string productCode, Guid productOptionId)
        {
            try
            {
                return _dbContext.ProductOptions.FirstOrDefault(p => p.ProductCode == productCode && p.ProductOptionId == productOptionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while finding ProductOption by [{productCode}] and [{productOptionId}]", productCode, productOptionId);
                return null;
            }
        }

        public ProductOption UpsertProductOption(ProductOption productOption)
        {
            try
            {
                var existed = _dbContext.ProductOptions.FirstOrDefault(p => p.ProductCode == productOption.ProductCode &&
                                                        p.ProductOptionId == productOption.ProductOptionId);
                if (existed == null)
                {
                    _dbContext.ProductOptions.Add(productOption);
                    _dbContext.SaveChanges();
                    return productOption;
                }
                else
                {
                    existed.Name = productOption.Name;
                    existed.Description = productOption.Description;
                    _dbContext.Update(existed);
                    _dbContext.SaveChanges();
                    return existed;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while upsert productOption. [{productOption}]", productOption);
                return null;
            }
        }

        public void DeleteProductOption(string productCode, Guid productOptionId)
        {
            try
            {
                var options = _dbContext.ProductOptions.Where(p => p.ProductCode == productCode && p.ProductOptionId == productOptionId);
                if (options.Any())
                {
                    _dbContext.ProductOptions.RemoveRange(options);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while delete productOption. [{productCode}],[{productOptionId}]", productCode, productOptionId);
            }
        }

        public ProductOption UpsertProductOption(string productCode, ProductOption productOption)
        {
            try
            {
                var option = _dbContext.ProductOptions.FirstOrDefault(p => p.ProductCode == productCode &&
                                                       p.ProductOptionId == productOption.ProductOptionId);
                if(option != null)
                {
                    option.Name = productOption.Name;
                    option.Description = productOption.Description;
                    _dbContext.ProductOptions.Update(option);
                    _dbContext.SaveChanges();
                    return option;
                }
                return productOption;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while update productOption. [{productCode}], [{productOption}]", productCode, productOption);
                return productOption;
            }
        }
    }
}
