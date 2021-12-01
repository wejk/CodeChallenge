using System.ComponentModel.DataAnnotations;

namespace KenTan.Api.Command
{
    public class ProductUpsertCommand
    {
        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }
    }
}
