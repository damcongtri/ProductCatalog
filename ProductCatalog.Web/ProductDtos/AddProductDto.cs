using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace ProductCatalog.Model.Dto.ProductDtos
{

    public class AddProductDto
    {
        [Required(ErrorMessage = "Mã cửa hàng là bắt buộc.")]
        public int ShopId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "Giảm phải trong khoảng 0 - 100%.")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Mã danh mục là bắt buộc.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Hãng này là bắt buộc.")]
        public string BranchName { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public virtual List<AddProductVariantDto>? Variants { get; set; } = [];
    }

}
