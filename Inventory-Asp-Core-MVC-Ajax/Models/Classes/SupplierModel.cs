using Inventory_Asp_Core_MVC_Ajax.DataAccess.common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class SupplierModel : AuditableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a name.")]
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "The Address value cannot exceed 150 characters.")]
        [Display(Name = "Address", Prompt = "Address")]
        public string Address { get; set; }

        [StringLength(14, ErrorMessage = "The Phone value cannot exceed 16 characters.")]
        [Display(Name = "Phone", Prompt = "Phone (example: 021-1112223344)")]
        public string Phone { get; set; }

        [StringLength(14, ErrorMessage = "The Phone value cannot exceed 14 characters.")]
        [Display(Name = "Emergency Mobile", Prompt = "Mobile (example: 0912-111-2233)")]
        public string EmergencyMobile { get; set; }

        public bool Enabled { get; set; }

        public ICollection<ProductModel> ProductModels { get; set; }

        public override string ToString() =>
            $" ({Name} - {Phone} - {EmergencyMobile} - {Enabled} - {Address}  )";

    }
}
