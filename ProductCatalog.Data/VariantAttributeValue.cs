using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Database
{
    public class VariantAttributeValue
    {
        [Key]
        public int ValueId { get; set; }

        [ForeignKey("VariantAttribute")]
        public int AttributeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual VariantAttribute? VariantAttribute { get; set; }
        public virtual ICollection<ProductVariantAttributeLink>? ProductVariantAttributeLinks { get; set; }
    }

}
