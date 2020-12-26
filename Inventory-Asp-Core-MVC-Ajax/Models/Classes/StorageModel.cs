using Inventory_Asp_Core_MVC_Ajax.DataAccess.common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class StorageModel : AuditableEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a name.")]
        [MaxLength(50, ErrorMessage = "The name value cannot exceed 50 characters.")]
        [Display(Name = "Name", Prompt = "Name")]
        [Remote(action: "IsNameInUse", controller: "Storage")]
        public string Name { get; set; }

        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        //   ErrorMessage = "Not a valid phone number.")]
        [StringLength(14, ErrorMessage = "The Phone value cannot exceed 14 characters.")]
        [Display(Name = "Phone", Prompt = "Phone (example: 0000-000-0000)")]
        public string Phone { get; set; }

        [StringLength(150, ErrorMessage = "The Address value cannot exceed 150 characters.")]
        [Display(Name = "Address", Prompt = "Address")]
        public string Address { get; set; }

        [Display(Name = "Products")]
        public ICollection<ProductModel> ProductModels { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StorageModel model &&
                   Id == model.Id &&
                   Name == model.Name &&
                   Phone == model.Phone &&
                   Address == model.Address;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Phone, Address);
        }

        public override string ToString() =>
             $"Store: ({Name} - {Phone} - {Address} - {ProductModels} )";



    }
}
