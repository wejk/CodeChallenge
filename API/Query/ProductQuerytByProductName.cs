using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Services
{
    public class ProductQuerytByProductName
    {
        [Required]
        public string ProductName { get; set; }
    } 
}