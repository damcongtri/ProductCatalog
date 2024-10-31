using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model.Database
{
    public class Cart
    {
        [Key]
        [Column("Id")]
        public int CartId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int? VariantId { get; set; }

        public virtual Product? Product { get; set; }

        public DateTime CreatedAt{ get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

    }
}
