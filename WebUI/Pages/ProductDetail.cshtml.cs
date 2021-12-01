using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class ProductDetailModel : PageModel
    {
        [BindProperty]
        public List<ProductOptionModel> ProductDetails { get; set; }

        public string ProductCode { get; set; }
        public IWebApiService ApiService { get; }

        public ProductDetailModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task<IActionResult> OnGetAsync(string productCode)
        {
            ProductCode = productCode;
            ProductDetails = await ApiService.GetProductOptionsAsync(productCode);
            if (!ProductDetails.Any())
            {
                return RedirectToPage("AddProductDetail",new { productCode });
            }
            return Page();
        }
    }
}
