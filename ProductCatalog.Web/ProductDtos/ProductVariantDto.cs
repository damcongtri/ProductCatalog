using ProductCatalog.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class ProductVariantDto
    {
        public int VariantId { get; set; }
        public string VariantSku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public List<AttibuteValueDto> AttributesValue { get; set; }
    }
}
