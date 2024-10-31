using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Database
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }  // Assuming the code is unique, the uniqueness will be enforced in the DbContext.

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DiscountAmount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int ShopId { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? min_order_value { get; set; }
        public int? MaxUse { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
