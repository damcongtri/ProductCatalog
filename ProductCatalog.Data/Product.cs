using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Database
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public int ShopId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string BranchName { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(2, 2)")]
        public decimal Discount { get; set; } = 0;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(255)]
        public string ImageUrl { get; set; }


        // Navigation properties
        public virtual Category? Category { get; set; }
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }

    }

}
