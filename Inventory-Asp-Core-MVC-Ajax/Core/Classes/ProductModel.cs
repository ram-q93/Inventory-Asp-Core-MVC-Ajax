using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class ProductModel : AuditableEntity
    {
        public int Id { get; set; }

        #region
        [Remote(action: "IsNameInUse", controller: "Product")]
        [Required(ErrorMessage = "You must provide a name.")]
        [StringLength(100, ErrorMessage = "value cannot exceed 100 characters.")]
        [Display(Name = "Name", Prompt = "Name *")]
        #endregion
        public string Name { get; set; }

        #region
        [Required(ErrorMessage = "You must provide a Code.")]
        [StringLength(20, ErrorMessage = "value cannot exceed 20 characters.")]
        [Display(Name = "Code", Prompt = "Code *")]
        #endregion 
        public string Code { get; set; }

        #region
        [Required(ErrorMessage = "You must provide Quantity.")]
        [Display(Name = "Quantity", Prompt = "Quantity *")]
        #endregion
        public int Quantity { get; set; }

        #region
        [Required(ErrorMessage = "You must provide a price.")]
        [PrecisionAndScale(8, 2, ErrorMessage = "Price must not exceed $999999.99")]
        [Display(Name = "Price($)", Prompt = "Price($) *")]
        #endregion
        public decimal UnitePrice { get; set; }

        #region
        [StringLength(1000, ErrorMessage = "value cannot exceed 1000 characters.")]
        [Display(Name = "Description", Prompt = "Description")]
        #endregion
        public string Description { get; set; }

        #region
        [Display(Name = "Enabled", Prompt = "Enabled")]
        #endregion
        public bool Enabled { get; set; } = true;

        public int? StorageId { get; set; }

        public int? ImageId { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        #region
        [Display(Name = "Storage", Prompt = "Storage")]
        #endregion
        public string StorageName { get; set; }

        #region
        [Display(Name = "Supplier", Prompt = "Supplier")]
        #endregion
        public string SupplierCompanyName { get; set; }

        #region
        [Display(Name = "Category", Prompt = "Category")]
        #endregion
        public string CategoryName { get; set; }

        public ImageModel ImageModel { get; set; }

        //public StorageModel StorageModel { get; set; }

        //public ImageModel ImageModel { get; set; }

        //public SupplierModel SupplierModel { get; set; }

        //public Category Category { get; set; }

        [Display(Name = "Product Picture")]
        public IFormFile ProductPicture { get; set; }

    }
}
