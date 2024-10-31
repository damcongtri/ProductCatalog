using Microsoft.AspNetCore.Http;
using ProductCatalog.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class AddProductVariantDto
    {
        public string VariantSku { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }
        public virtual List<VariantAttributeLinkDto>? ProductVariantAttributeLink { get; set; } // Danh sách các value_id của các thuộc tính (ví dụ: Size M, Color Red)
    }
}
