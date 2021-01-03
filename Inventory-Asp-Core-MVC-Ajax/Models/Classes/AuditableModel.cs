using System;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class AuditableModel
    {
        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
