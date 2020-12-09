using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class ProductModel : Updateable
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a name.")]
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "You must provide a Barcode.")]
        [StringLength(20, ErrorMessage = "value cannot exceed 20 characters.")]
        [Display(Name = "Barcode", Prompt = "Barcode")]
        public string Barcode { get; set; }

        [StringLength(50, ErrorMessage = "value cannot exceed 50 characters.")]
        [Display(Name = "Type", Prompt = "Type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "You must provide a Quantity.")]
        [Display(Name = "Quantity", Prompt = "Quantity")]
        public int Quantity { get; set; }

        [StringLength(1000, ErrorMessage = "value cannot exceed 1000 characters.")]
        [Display(Name = "Description", Prompt = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must provide a Availability.")]
        [Display(Name = "Is Available", Prompt = "Is Available")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "You must provide a price.")]
        [PrecisionAndScale(8, 2, ErrorMessage = "Price must not exceed $999999.99")]
        [Display(Name = "Price($)", Prompt = "Price($)")]
        public decimal Price { get; set; }

        public int StorageId { get; set; }

        public StorageModel StorageModel { get; set; }

        public ICollection<ImageModel> ImageModels { get; set; }

        public override string ToString() =>
             $"Product: ({Name} - {Barcode} - {Type} - {Quantity} - {IsAvailable} - {Price}" +
            $" - StorId({StorageId}) - {Description})";

    }
}
