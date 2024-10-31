using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.CategoryDtos
{
    public class CreateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ID Danh mục cha không hợp lệ")]
        public int? ParentCategoryId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

}
