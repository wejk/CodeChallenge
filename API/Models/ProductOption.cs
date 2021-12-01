using System;

namespace KenTan.Api.Models
{
    public class ProductOption
    {
        public Guid ProductOptionId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
