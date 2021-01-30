using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            ProductModels = new HashSet<ProductModel>();
        }

        public int Id { get; set; }

        #region
        [Required(ErrorMessage = "یک نام باید انتخاب کنید")]
        [StringLength(100, ErrorMessage = "نباید بیشتر از 100 کاراکتر باشد")]
        [Display(Name = "نام", Prompt = "نام*")]
        #endregion
        public string Name { get; set; }

        #region
        [StringLength(1000, ErrorMessage = "نباید بیشتر از 1000 کاراکتر باشد")]
        [Display(Name = "توضیحات", Prompt = "توضیحات")]
        #endregion
        public string Description { get; set; }

        public ICollection<ProductModel> ProductModels { get; private set; }
    }
}