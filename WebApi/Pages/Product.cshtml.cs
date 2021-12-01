using KenTan.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RefactorThis.Services;
using System.Collections.Generic;

namespace WebApi.Pages
{
    public class ProductModel : PageModel
    {
        public IProductService ProductService { get; }

        public IList<Product> Products { get; set; }
        public ProductModel(IProductService productService)
        {
            ProductService = productService;
        }


        public void OnGet()
        {
            Products = ProductService.GetProducts(new KenTan.Api.Query.ProductQuery { Limit = 10});
        }
    }
}
