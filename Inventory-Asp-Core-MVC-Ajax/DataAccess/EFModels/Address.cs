using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    public class Address
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Country { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string City { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Region { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Street { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string Number { get; set; }

        [Column(TypeName = "varchar(14)")]
        public string PostalCode { get; set; }


        public Guid? BusinessId { get; set; }

        public Business Business { get; set; }
    }
}
