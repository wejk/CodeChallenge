using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class DeleteProductModel : PageModel
    {
        public IWebApiService ApiService { get; }

        [BindProperty, Required]
        public string ConfirmDelete { get; set; }
        public string[] Confirmation = new[] { "Yes", "No" };

        [BindProperty]
        public ProductModelProduct Product { get; set; }

        public DeleteProductModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task OnGetAsync(string productCode)
        {
            Product = await ApiService.GetProductAsync(productCode);
        }

        public async Task<IActionResult> OnPostAsync(string confirmDelete)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (confirmDelete == "Yes")
            {
                await ApiService.DeleteProductAsync(Product);
            }

            return RedirectToPage("Product");
        }
    }
}
