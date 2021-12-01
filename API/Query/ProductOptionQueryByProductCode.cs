using System.ComponentModel.DataAnnotations;

namespace KenTan.Api.Query
{
    public class ProductOptionQueryByProductCode
    {
        [Required]
        public string ProductCode { get; set; }
    }
}
