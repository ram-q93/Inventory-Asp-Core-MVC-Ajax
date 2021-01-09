using Inventory_Asp_Core_MVC_Ajax.DataAccess.common;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.EFModels
{
    [Table("Products")]
    public class Product : AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string Code { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal UnitePrice { get; set; }

        [MaxLength(1000)]
        [Column(TypeName = "varchar(1000)")]
        public string Description { get; set; }

        [Required]
        public bool Enabled { get; set; }

        public int? StorageId { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        public int? ImageId { get; set; }

        public Storage Storage { get; set; }

        public Supplier Supplier { get; set; }

        public Category Category { get; set; }

        public Image Image { get; set; }
    }
}
