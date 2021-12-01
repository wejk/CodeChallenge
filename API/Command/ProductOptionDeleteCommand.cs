using System;

namespace KenTan.Api.Command
{
    public class ProductOptionDeleteCommand
    {
        public string ProductCode { get; set; }
        public Guid ProductOptionId { get; set; }

    }
}