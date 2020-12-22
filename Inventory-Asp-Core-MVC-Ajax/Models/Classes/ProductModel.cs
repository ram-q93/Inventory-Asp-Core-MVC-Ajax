using AspNetCore.Lib.Attributes;
using Inventory_Asp_Core_MVC_Ajax.DataAccess.common;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class ProductModel : AuditableEntity
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

        public int? ImageId { get; set; }

        public int SupplierId { get; set; }

        public StorageModel StorageModel { get; set; }

        public ImageModel ImageModel { get; set; }

        [Display(Name = "Product Picture")]
        public IFormFile ProductPicture { get; set; }
        public SupplierModel SupplierModel { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ProductModel model &&
                   Id == model.Id &&
                   Name == model.Name &&
                   Barcode == model.Barcode &&
                   Type == model.Type &&
                   Quantity == model.Quantity &&
                   Description == model.Description &&
                   IsAvailable == model.IsAvailable &&
                   Price == model.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Barcode, Type, Quantity, Description, IsAvailable, Price);
        }

        public override string ToString() =>
             $"Product: ({Name} - {Barcode} - {Type} - {Quantity} - {IsAvailable} - {Price}" +
            $" - StoragId({StorageId}) - {Description})";



    }
}
