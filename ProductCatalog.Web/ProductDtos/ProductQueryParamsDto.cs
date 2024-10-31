using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class ProductQueryParamsDto : PaginationDto
    {
        public string? Keyword { get; set; }
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
        public string? BranchName { get; set; }
        
    }
}
