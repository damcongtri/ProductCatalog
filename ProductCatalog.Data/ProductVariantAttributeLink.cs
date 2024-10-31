using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Database
{
    public class ProductVariantAttributeLink
    {
        [ForeignKey("ProductVariant")]
        public int VariantId { get; set; }

        [ForeignKey("VariantAttributeValue")]
        public int ValueId { get; set; }

        // Navigation properties
        public virtual ProductVariant? ProductVariant { get; set; }
        public virtual VariantAttributeValue? VariantAttributeValue { get; set; }
    }

}
