using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class EditProductModel : PageModel
    {
        [BindProperty]
        public ProductModelProduct Product { get; set; }

        public IWebApiService ApiService { get; }

        public EditProductModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task OnGetAsync(string productCode)
        {
            Product = await ApiService.GetProductAsync(productCode);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await ApiService.UpdateProductSync(Product);

            return RedirectToPage("Product");
        }
    }
}
