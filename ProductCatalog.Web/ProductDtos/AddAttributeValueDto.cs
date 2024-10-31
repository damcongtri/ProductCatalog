using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Dto.ProductDtos
{
    public class AddAttributeValueDto
    {
        [Required(ErrorMessage = "AttributeId bắt buộc phải nhập")]
        public int AttributeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
