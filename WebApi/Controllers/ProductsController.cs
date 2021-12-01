using KenTan.Api.Command; using KenTan.Api.Models; using KenTan.Api.Query; using Microsoft.AspNetCore.Http; using Microsoft.AspNetCore.Mvc; using RefactorThis.Services; using WebApi.Services;  namespace WebApi.Controllers {     [Route("api/[controller]")]     [ApiController]     public class ProductsController : ControllerBase     {         public IProductService _productService { get; }          public ProductsController(IProductService productService)         {             _productService = productService;         }          [HttpGet]         [Route("search-all")]         [ProducesResponseType(typeof(Product[]), 200)]         [ProducesResponseType(StatusCodes.Status404NotFound)]         public ActionResult<Product[]> GetAllProducts([FromQuery]ProductQuery query)         {             return _productService.GetProducts(query);         }          [HttpGet]         [Route("search-by-name")]         [ProducesResponseType(typeof(Product[]), 200)]         [ProducesResponseType(StatusCodes.Status404NotFound)]         public ActionResult<Product[]> GetProductsByName([FromQuery] ProductQuerytByProductName query)         {             var (found, products) = _productService.TryGetProductsByName(query);             if(!found)
            {
                return NotFound();
            }              return products;         }          [HttpGet]         [Route("search-by-product-code")]         [ProducesResponseType(typeof(Product), 200)]         [ProducesResponseType(StatusCodes.Status404NotFound)]         public ActionResult<Product> GetProductById([FromQuery] ProductQueryByProductCode query)         {             var (found, product) = _productService.TryGetProductByCode(query);             if(!found)             {                 return NotFound();             }             return product;         }          [HttpPost]         [Route("add-product")]         [ProducesResponseType(StatusCodes.Status500InternalServerError)]         [ProducesResponseType(typeof(Product), 201)]         public ActionResult<Product> AddProduct([FromBody] ProductUpsertCommand command)         {             var (success, product) = _productService.TryAddProduct(command);             if(!success)
            {
                return new StatusCodeResult(500);
            }              return product;         }          [HttpPut]
        [Route("update-product")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]         [ProducesResponseType(typeof(Product), 201)]         public ActionResult<Product> UpdateProductById([FromBody] ProductUpsertCommand command)         {
            var (success, product) = _productService.TryUpdateProduct(command);             if (!success)
            {
                return new StatusCodeResult(500);
            }              return product;         }

        [HttpDelete]
        [Route("delete-product")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public ActionResult DeleteProductById([FromBody] ProductDeleteCommand command) // //[FromRoute] string productCode
        {
            _productService.DeleteProduct(new ProductDeleteCommand { ProductCode = command.ProductCode});
            return Accepted();
        }

        [HttpGet]
        [Route("options/search-by-productCode")]
        [ProducesResponseType(typeof(ProductOption[]), 200)]         [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductOption[]> GetOptions([FromQuery]ProductOptionQueryByProductCode query)
        {
            return _productService.GetProductOptions(query);
        }

        [HttpGet]
        [Route("options/search-by-productCode-productOptionId")]
        [ProducesResponseType(typeof(ProductOption), 200)]         [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductOption> GetOption([FromQuery]ProductOptionQueryByProductCodeProductOptionId query)
        {
            var (sucess, productOption) = _productService.TryGetProductOptionQueryByProductCodeProductOptionId(query);
            if(!sucess)
            {
                return NotFound();
            }
            return productOption;
        }

        [HttpPost]
        [Route("options/add-product-option")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]         [ProducesResponseType(typeof(Product), 201)]
        public ActionResult<ProductOption> CreateOption([FromBody] ProductOptionAddCommand command)
        {
            var (success, productOption) = _productService.TryAddProductOption(command);             if (!success)
            {
                return new StatusCodeResult(500);
            }              return productOption;
        }

        [HttpPut]
        [Route("options/update-product-option")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]         [ProducesResponseType(typeof(Product), 201)]
        public ActionResult<ProductOption> UpdateOption([FromBody]ProductOptionUpdateCommand command)
        {
            var (success, productOption) = _productService.TryUpdateProductOption(command);             if (!success)
            {
                return new StatusCodeResult(500);
            }              return productOption;
        }

        [HttpDelete]
        [Route("options/delete-product-option")]
        public void DeleteOption([FromBody] ProductOptionDeleteCommand command)
        {
            _productService.DeleteProductOption(command);
        }
    } }