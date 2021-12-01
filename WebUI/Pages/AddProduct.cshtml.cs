using KenTan.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages
{
    public class AddProductModel : PageModel
    {
        [BindProperty]
        public ProductModelProduct Product { get; set; }

        public IWebApiService ApiService { get; }

        public AddProductModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            await ApiService.AddProductSync(Product);

            return RedirectToPage("Product");
        }
    }
}
