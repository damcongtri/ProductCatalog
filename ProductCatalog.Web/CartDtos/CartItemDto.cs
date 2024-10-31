using ProductCatalog.Model.Database;
using ProductCatalog.Model.Dto.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.CartDtos
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int VariantId { get; set; }
        public ProductDto Product { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }
}
