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
        [Required(ErrorMessage = "یک نام باید انتخاب کنید")]
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "نام", Prompt = "*نام")]
        #endregion
        public string Name { get; set; }

        #region
        [Required(ErrorMessage = "یک کد باید انتخاب کنید")]
        [StringLength(20, ErrorMessage = "نباید بیشتر از 20 کاراکتر باشد")]
        [Display(Name = "کد", Prompt = "*کد")]
        #endregion 
        public string Code { get; set; }

        #region
        [Required(ErrorMessage = "تعداد باید انتخاب کنید")]
        [Display(Name = "تعداد", Prompt = "*تعداد")]
        #endregion
        public int Quantity { get; set; }

        #region
        [Required(ErrorMessage = "یک قیمت باید انتخاب کنید")]
        [PrecisionAndScale(8, 2, ErrorMessage = "نباید بیشتر از 999999.99 ربال باشد")]
        [Display(Name = "(قیمت(ریال", Prompt = "*(قیمت(ریال")]
        #endregion
        public decimal UnitePrice { get; set; }

        #region
        [StringLength(1000, ErrorMessage = "نباید بیشتر از 1000 کاراکتر باشد")]
        [Display(Name = "توضیحات", Prompt = "توضیحات")]
        #endregion
        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public int? StorageId { get; set; }

        public int? ImageId { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        #region
        [Display(Name = "انبار", Prompt = "انبار")]
        #endregion
        public string StorageName { get; set; }

        #region
        [Display(Name = "تامیین کننده", Prompt = "تامیین کننده")]
        #endregion
        public string SupplierCompanyName { get; set; }

        #region
        [Display(Name = "دسته بندی", Prompt = "دسته بندی")]
        #endregion
        public string CategoryName { get; set; }

        public ImageModel ImageModel { get; set; }

        //public StorageModel StorageModel { get; set; }

        //public ImageModel ImageModel { get; set; }

        //public SupplierModel SupplierModel { get; set; }

        //public Category Category { get; set; }

        [Display(Name = "عکس محصول")]
        public IFormFile ProductPicture { get; set; }

    }
}
