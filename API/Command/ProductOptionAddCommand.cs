using System.ComponentModel.DataAnnotations;

namespace KenTan.Api.Command
{
    public class ProductOptionAddCommand
    {
        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}