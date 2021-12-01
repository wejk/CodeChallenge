using System.ComponentModel.DataAnnotations;

namespace KenTan.Api.Command
{
    public class ProductDeleteCommand
    {
        [Required]
        public string ProductCode { get; set; }
    }
}
