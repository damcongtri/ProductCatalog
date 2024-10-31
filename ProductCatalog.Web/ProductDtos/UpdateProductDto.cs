using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class UpdateProductDto 
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<UpdateProductVariantDto> Variants { get; set; }
    }
}
