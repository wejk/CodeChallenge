using System;
using System.ComponentModel.DataAnnotations;

namespace KenTan.Api.Query
{
    public class ProductOptionQueryByProductCodeProductOptionId
    {
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public Guid ProductOptionId { get; set; }
    }
}