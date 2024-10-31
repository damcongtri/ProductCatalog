using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class UpdateProductVariantDto : AddProductVariantDto
    {
        public int VariantId { get; set; }
    }
}
