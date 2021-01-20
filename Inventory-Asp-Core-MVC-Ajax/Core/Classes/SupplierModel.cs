using AspNetCore.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class SupplierModel : AuditableEntity
    {
        public SupplierModel()
        {
            ProductModels = new HashSet<ProductModel>();
        }

        public int Id { get; set; }

        #region
        [Required(ErrorMessage = "یک نام باید انتخاب کنید")]
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "شرکت", Prompt = "*شرکت")]
        #endregion
        public string CompanyName { get; set; }

        #region
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "مخاطب", Prompt = "مخاطب")]
        #endregion
        public string ContactName { get; set; }

        #region
        [StringLength(14, ErrorMessage = "نباید بیشتر از 14 کاراکتر باشد")]
        [Display(Name = "موبایل", Prompt = "مثال:0912-23432332")]
        #endregion
        public string EmergencyMobile { get; set; }

        #region
        [Required(ErrorMessage = "شماره تلفن ضروری است")]
        [StringLength(16, ErrorMessage = "نباید بیشتر از 16 کاراکتر باشد")]
        [Display(Name = "تلفن", Prompt = "مثال:021-6667723")]
        #endregion
        public string Phone { get; set; }

        #region
        [StringLength(150, ErrorMessage = "نباید بیشتر از 150 کاراکتر باشد")]
        [Display(Name = "آدرس", Prompt = "آدرس")]
        #endregion
        public string Address { get; set; }

        #region
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "شهر", Prompt = "شهر")]
        #endregion
        public string City { get; set; }

        #region
        [StringLength(14, ErrorMessage = "نباید بیشتر از 14 کاراکتر باشد")]
        [Display(Name = "کد پستی", Prompt = "کد پستی")]
        #endregion
        public string PostalCode { get; set; }

        #region
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "وبسایت", Prompt = "وبسایت")]
        #endregion
        public string HomePage { get; set; }

        public bool Enabled { get; set; } = true;

        public ICollection<ProductModel> ProductModels { get; set; }

        public override string ToString() => $" ({CompanyName} - {Phone} - {EmergencyMobile})";

    }
}
