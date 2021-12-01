using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class EditProductDetailModel : PageModel
    {
        [BindProperty]
        public ProductOptionModel ProductDetail { get; set; }

        public IWebApiService ApiService { get; }

        public EditProductDetailModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task OnGetAsync(string productCode, Guid productOptionId)
        {
           ProductDetail = await ApiService.GetProductOptionsAsync(productCode, productOptionId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await ApiService.UpdateProductOptionAsync(ProductDetail);

            return RedirectToPage("ProductDetail", new { ProductDetail.ProductCode });
        }
    }
}
