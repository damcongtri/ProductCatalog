using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class AttibuteValueDto
    {
        public int ValueId { get; set; }

        public string Attribute { get; set; }
        public string Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
