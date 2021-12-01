using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ProductOptionModel
    {
        public Guid ProductOptionId { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
