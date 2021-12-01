using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class AddProductDetailModel : PageModel
    {
        [BindProperty]
        public ProductOptionModel Product { get; set; } = new ProductOptionModel();

        public IWebApiService ApiService { get; }

        public AddProductDetailModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public void OnGet(string productCode)
        {
            Product.ProductCode = productCode;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await ApiService.AddProductOptionAsync(Product);

            return RedirectToPage($"ProductDetail", new { Product.ProductCode});
        }
    }
}
