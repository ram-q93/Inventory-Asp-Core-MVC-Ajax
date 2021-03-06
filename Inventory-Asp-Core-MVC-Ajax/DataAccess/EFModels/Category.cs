﻿using AspNetCore.Lib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.EFModels
{
    [Table("Categories")]
    public class Category : AuditableEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public Guid BusinessId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [MaxLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        public ICollection<Product> Products { get; private set; }
    }
}
