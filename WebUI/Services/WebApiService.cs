using KenTan.Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebUI.Models;
using System.Linq;
using WebUI.Pages;
using KenTan.Api.Command;
using System.Diagnostics;

namespace WebUI.Services
{
    public interface IWebApiService
    {
        Task<List<ProductModelProduct>> GetProductsAsync();
        Task<ProductModelProduct> AddProductSync(ProductModelProduct product);
        Task<ProductModelProduct> UpdateProductSync(ProductModelProduct product);
        Task<ProductModelProduct> GetProductAsync(string productCode);

        Task DeleteProductAsync(ProductModelProduct product);

        Task<List<ProductOptionModel>> GetProductOptionsAsync(string productCode);
        Task<ProductOptionModel> GetProductOptionsAsync(string productCode, Guid productOptionId);
        Task<ProductOptionModel> AddProductOptionAsync(ProductOptionModel productModel);
        Task<ProductOptionModel> UpdateProductOptionAsync(ProductOptionModel productModel);
        Task DeleteProductOptionAsync(ProductOptionModel product);
    }

    public class WebApiService : IWebApiService
    {
        public static JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true,
            WriteIndented = false
        };

        private readonly HttpClient httpClient;
        public ILogger<WebApiService> Logger { get; }

        public WebApiService(ILogger<WebApiService> logger, HttpClient httpClient)
        {
            Logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<ProductModelProduct> AddProductSync(ProductModelProduct productModel)
        {
            var product = new ProductUpsertCommand
            {
                ProductName = productModel.Name,
                ProductCode = productModel.ProductCode,
                DeliveryPrice = productModel.DeliveryPrice,
                Price = productModel.Price,
                Description = productModel.Description,
            };
            return await SendRequest<ProductUpsertCommand, ProductModelProduct>("api/products/add-product", product, HttpMethod.Post);
        }

        public async Task<ProductModelProduct> UpdateProductSync(ProductModelProduct productModel)
        {
            var product = new ProductUpsertCommand
            {
                ProductName = productModel.Name,
                ProductCode = productModel.ProductCode,
                DeliveryPrice = productModel.DeliveryPrice,
                Price = productModel.Price,
                Description = productModel.Description,
            };
            return await SendRequest<ProductUpsertCommand, ProductModelProduct>("api/products/update-product", product, HttpMethod.Put);
        }

        public async Task<ProductModelProduct> GetProductAsync(string productCode)
        {
            var product = await Get<Product>($"api/products/search-by-product-code?ProductCode={productCode}");
            return ToProductModel(product);
        }

        public async Task<List<ProductModelProduct>> GetProductsAsync()
        {
            var products = await Get<List<Product>>("api/products/search-all");
            return products.Select(p => ToProductModel(p)).ToList();
        }

        public Task DeleteProductAsync(ProductModelProduct productModel)
        {
            var product = new ProductDeleteCommand { ProductCode = productModel.ProductCode };
            return SendRequest("api/products/delete-product", product, HttpMethod.Delete);
        }
        private async Task<TResponse> Get<TResponse>(string endpoint) where TResponse : new()
        {
            try
            {
                var jsonResult = await httpClient.GetStringAsync(endpoint);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    var response = JsonSerializer.Deserialize<TResponse>(jsonResult, JsonSerializerOptions);
                    return response;
                }
                return new TResponse();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while query WebApi Service - [endpoint]: {endpoint}", ex);
                return new TResponse();
            }
        }

        private async Task<TResponse> SendRequest<TRequest,TResponse>(string endpoint, TRequest request, HttpMethod httpMethod) where TResponse : new()
        {
            try
            {
                var rawJsonBody = JsonSerializer.Serialize(request, JsonSerializerOptions);
                var body =  new StringContent(rawJsonBody, Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(httpMethod, endpoint)
                {
                    Content = body,
                };
                var responseMessage = await httpClient.SendAsync(requestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();

                    var response = JsonSerializer.Deserialize<TResponse>(responseJson, JsonSerializerOptions);
                    return response;
                }
                Logger.LogError("Error response from WebApi Service: [{statusCode}]", responseMessage.StatusCode);
                return new TResponse();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while query WebApi Service - [endpoint]: {endpoint}", ex);
                return new TResponse();
            }
        }
        private async Task SendRequest<TRequest>(string endpoint, TRequest request, HttpMethod httpMethod)
        {
            try
            {
                var rawJsonBody = JsonSerializer.Serialize(request, JsonSerializerOptions);
                var body =  new StringContent(rawJsonBody, Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(httpMethod, endpoint)
                {
                    Content = body,
                };
                var responseMessage = await httpClient.SendAsync(requestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    Logger.LogInformation("Send request completed successfully", responseMessage.StatusCode);
                }
                return ;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while query WebApi Service - [endpoint]: {endpoint}", ex);
                return;
            }
        }


        private ProductModelProduct ToProductModel(Product product)
        {
            if(product == null) return new ProductModelProduct();

            return new ProductModelProduct
            {
                Name = product.Name,
                DeliveryPrice = product.DeliveryPrice,
                Description = product.Description,
                Price = product.Price,
                ProductCode = product.ProductCode,
            };
        } 
        private Product ToApiProduct(ProductModelProduct productModel)
        {
            if(productModel == null) return new Product();

            return new Product
            {
                Name = productModel.Name,
                DeliveryPrice = productModel.DeliveryPrice,
                Description = productModel.Description,
                Price = productModel.Price,
                ProductCode = productModel.ProductCode,
            };
        }
        private ProductOptionModel ToProductOptionModel(ProductOption product)
        {
            if(product == null) return new ProductOptionModel();

            return new ProductOptionModel
            {
                Name = product.Name,
                Description = product.Description,
                ProductCode = product.ProductCode,
                ProductOptionId = product.ProductOptionId
            };
        }

        public async Task<List<ProductOptionModel>> GetProductOptionsAsync(string productCode)
        {
            var products = await Get<List<ProductOption>>($"api/Products/options/search-by-productCode?ProductCode={productCode}");
            return products.Select(p => ToProductOptionModel(p)).ToList();
        }

        public async Task<ProductOptionModel> GetProductOptionsAsync(string productCode, Guid productOptionId)
        {
            var url = $"api/Products/options/search-by-productCode-productOptionId?ProductCode={productCode}&ProductOptionId={productOptionId}";
            var product = await Get<ProductOption>(url);
            return ToProductOptionModel(product);
        }

        public async Task<ProductOptionModel> AddProductOptionAsync(ProductOptionModel productOption)
        {
            var command = new ProductOptionAddCommand
            {
                Name = productOption.Name,
                ProductCode = productOption.ProductCode,
                Description = productOption.Description,
            };
            return await SendRequest<ProductOptionAddCommand, ProductOptionModel>("api/products/options/add-product-option", command, HttpMethod.Post);

        }

        public async Task<ProductOptionModel> UpdateProductOptionAsync(ProductOptionModel productOption)
        {
            var command = new ProductOptionUpdateCommand
            {
                Name = productOption.Name,
                ProductCode = productOption.ProductCode,
                Description = productOption.Description,
                ProductOptionId = productOption.ProductOptionId
            };
            return await SendRequest<ProductOptionUpdateCommand, ProductOptionModel>("api/products/options/update-product-option", command, HttpMethod.Put);

        }

        public Task DeleteProductOptionAsync(ProductOptionModel productOption)
        {
            var command = new ProductOptionDeleteCommand { ProductCode = productOption.ProductCode, ProductOptionId = productOption.ProductOptionId };
            return SendRequest("api/products/options/delete-product-option", command, HttpMethod.Delete);
        }
    }
}
