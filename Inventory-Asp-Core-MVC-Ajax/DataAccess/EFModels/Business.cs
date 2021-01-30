using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    public class Business
    {
        public Business()
        {
            Addresses = new HashSet<Address>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(16)")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Column(TypeName = "varchar(14)")]
        public string EmergencyMobile { get; set; }

        [Column(TypeName = "varchar(14)")]
        public string Fax { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
