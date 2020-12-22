using Inventory_Asp_Core_MVC_Ajax.DataAccess.common;
using Inventory_Asp_Core_MVC_Ajax.EFModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    [Table("Suppliers")]
    public class Supplier : AuditableEntity
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string CompanyName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ContactName { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string ContactTitle { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(16)")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(14)")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        public string EmergencyMobile { get; set; }

        [Column(TypeName = "varchar(14)")]
        public string Fax { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Country { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Region { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string Address { get; set; }

        [Column(TypeName = "varchar(14)")]
        public string PostalCode { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string HomePage { get; set; }

        public bool Enabled { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
