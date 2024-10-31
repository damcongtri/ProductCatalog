using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Database
{
    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [MaxLength(100)]
        public string VariantSku { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual ICollection<ProductVariantAttributeLink>? ProductVariantAttributeLinks { get; set; }
    }

}
