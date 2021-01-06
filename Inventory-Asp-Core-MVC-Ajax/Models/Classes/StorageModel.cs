using Inventory_Asp_Core_MVC_Ajax.DataAccess.common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class StorageModel : AuditableModel
    {
        public int Id { get; set; }

        #region
        [Required(ErrorMessage = "You must provide a name.")]
        [MaxLength(50, ErrorMessage = "The name value cannot exceed 50 characters.")]
        [Display(Name = "Name", Prompt = "Name")]
        [Remote(action: "IsNameAvailable", controller: "Storage")]
        #endregion
        public string Name { get; set; }

        #region
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        //   ErrorMessage = "Not a valid phone number.")]
        [StringLength(14, ErrorMessage = "The Phone value cannot exceed 14 characters.")]
        [Display(Name = "Phone", Prompt = "Phone (example: 0000-000-0000)")]
        #endregion
        public string Phone { get; set; }

        #region
        [StringLength(100, ErrorMessage = "The City value cannot exceed 100 characters.")]
        [Display(Name = "City", Prompt = "City")]
        #endregion
        public string City { get; set; }

        #region
        [StringLength(150, ErrorMessage = "The Address value cannot exceed 150 characters.")]
        [Display(Name = "Address", Prompt = "Address")]
        #endregion
        public string Address { get; set; }

        #region
        [Display(Name = "Enabled", Prompt = "Enabled")]
        #endregion
        public bool Enabled { get; set; } = true;

        #region
        [Display(Name = "Products")]
        #endregion
        public ICollection<ProductModel> ProductModels { get; set; }

        public override string ToString() => $"Store: ({Name} - {Phone} - {Address} - {ProductModels} )";

    }
}
