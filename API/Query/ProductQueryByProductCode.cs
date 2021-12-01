using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Services
{
    public class ProductQueryByProductCode
    {
        [Required]
        public string ProductCode { get; set; }
    } 
}