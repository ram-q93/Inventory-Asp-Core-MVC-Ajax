using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class SupplierModel : AuditableEntity
    {
        public SupplierModel()
        {
            ProductModels = new HashSet<ProductModel>();
        }

        public int Id { get; set; }

        #region
        [Remote(action: "IsNameInUse", controller: "Supplier")]
        [Required(ErrorMessage = "You must provide a name.")]
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "Company", Prompt = "Company")]
        #endregion
        public string CompanyName { get; set; }

        #region
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "Contact", Prompt = "Contact")]
        #endregion
        public string ContactName { get; set; }

        #region
        [StringLength(14, ErrorMessage = "The Phone value cannot exceed 14 characters.")]
        [Display(Name = "Emergency Mobile", Prompt = "Mobile (example: 0912-111-2233)")]
        #endregion
        public string EmergencyMobile { get; set; }

        #region
        [StringLength(16, ErrorMessage = "value cannot exceed 16 characters.")]
        [Display(Name = "Phone", Prompt = "Phone (example: 021-1112223344)")]
        #endregion
        public string Phone { get; set; }

        #region
        [StringLength(150, ErrorMessage = "The Address value cannot exceed 150 characters.")]
        [Display(Name = "Address", Prompt = "Address")]
        #endregion
        public string Address { get; set; }

        #region
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "City", Prompt = "City")]
        #endregion
        public string City { get; set; }

        #region
        [StringLength(14, ErrorMessage = "value cannot exceed 14 characters.")]
        [Display(Name = "Postal Code", Prompt = "Postal Code")]
        #endregion
        public string PostalCode { get; set; }

        #region
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "Home Page", Prompt = "Home Page")]
        #endregion
        public string HomePage { get; set; }

        public bool Enabled { get; set; }

        public ICollection<ProductModel> ProductModels { get; set; }

        public override string ToString() => $" ({CompanyName} - {Phone} - {EmergencyMobile})";

    }
}
