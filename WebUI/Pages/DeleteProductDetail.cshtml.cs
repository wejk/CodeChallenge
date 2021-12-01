using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebUI.Models;
using WebUI.Services;

namespace WebUI.Pages.Shared
{
    public class DeleteProductDetailModel : PageModel
    {
        public IWebApiService ApiService { get; }

        [BindProperty, Required]
        public string ConfirmDelete { get; set; }
        public string[] Confirmation = new[] { "Yes", "No" };

        [BindProperty]
        public ProductOptionModel ProductDetail { get; set; }

        public DeleteProductDetailModel(IWebApiService apiService)
        {
            ApiService = apiService;
        }

        public async Task OnGetAsync(string productCode, Guid productOptionId)
        {
            ProductDetail = await ApiService.GetProductOptionsAsync(productCode, productOptionId);
        }

        public async Task<IActionResult> OnPostAsync(string confirmDelete)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (confirmDelete == "Yes")
            {
                await ApiService.DeleteProductOptionAsync (ProductDetail);
            }

            return RedirectToPage("ProductDetail", new {ProductDetail.ProductCode});
        }
    }
}
