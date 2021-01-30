using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class StorageModel : AuditableModel
    {
        public StorageModel()
        {
            ProductModels = new HashSet<ProductModel>();
        }
        public int Id { get; set; }

        #region
        [Required(ErrorMessage = "یک نام باید انتخاب کنید")]
        [StringLength(50, ErrorMessage = "نباید بیشتر از 50 کاراکتر باشد")]
        [Display(Name = "نام انبار", Prompt = "نام انبار*")]
        #endregion
        public string Name { get; set; }

        #region
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        //   ErrorMessage = "Not a valid phone number.")]
        [Required(ErrorMessage = "شماره تلفن ضروری است")]
        [StringLength(14, ErrorMessage = "نباید بیشتر از 14 کاراکتر باشد")]
        [Display(Name = "تلفن", Prompt = "مثال:021-6667723")]
        #endregion
        public string Phone { get; set; }

        #region
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "شهر", Prompt = "شهر")]
        #endregion
        public string City { get; set; }

        #region
        [StringLength(150, ErrorMessage = "نباید بیشتر از 150 کاراکتر باشد")]
        [Display(Name = "آدرس", Prompt = "آدرس")]
        #endregion
        public string Address { get; set; }

        public bool Enabled { get; set; } = true;

        #region
        [Display(Name = "Products")]
        #endregion
        public ICollection<ProductModel> ProductModels { get; set; }

        public override string ToString() => $" ({Name} - {Phone} - {City} - {Address} -{Enabled})";

    }
}
