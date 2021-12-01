using System;

namespace KenTan.DataLayer.Models
{
    public class Product
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DeliveryPrice { get; set; }

    }

    public class ProductOption
    {
        public Guid ProductOptionId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
