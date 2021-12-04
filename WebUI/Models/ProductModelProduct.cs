using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebUI.Models
{
    public class ProductModelProduct
    {
        [Required]
        [StringLength(36, MinimumLength = 5)] // Business Rule: Product code must be at least 5 chars, max length is 36
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(4, 2)")] 
        [Range(1,5000)] // Business Rule: Price between 1 and 5000
       // [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(4, 2)")]
        //[DataType(DataType.Currency)]
        [Display(Name = "Delivery Price")]

        [Range(0,5000)] // Business Rule: delivery can be $0 - free delivery
        public decimal DeliveryPrice { get; set; }
    }
}
