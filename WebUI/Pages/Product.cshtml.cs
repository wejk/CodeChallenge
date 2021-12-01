using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class ProductModel : PageModel
    {
        public List<ProductModelProduct> Products { get; set; }
        public IWebApiService ApiService { get; }

        public ProductModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task OnGetAsync()
        {
            Products = await ApiService.GetProductsAsync();
        }
    }
}
