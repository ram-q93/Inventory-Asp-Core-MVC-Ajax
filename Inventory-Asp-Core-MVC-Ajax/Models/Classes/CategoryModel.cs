using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            ProductModels = new HashSet<ProductModel>();
        }

        public int Id { get; set; }

        #region
        [Required(ErrorMessage = "You must provide a name.")]
        [MaxLength(100, ErrorMessage = "The name value cannot exceed 100 characters.")]
        [Display(Name = "Name", Prompt = "Name")]
        #endregion
        public string Name { get; set; }

        #region
        [MaxLength(1000, ErrorMessage = "Value cannot exceed 1000 characters.")]
        [Display(Name = "Name", Prompt = "Name")]
        #endregion
        public string Description { get; set; }

        public ICollection<ProductModel> ProductModels { get; private set; }
    }
}