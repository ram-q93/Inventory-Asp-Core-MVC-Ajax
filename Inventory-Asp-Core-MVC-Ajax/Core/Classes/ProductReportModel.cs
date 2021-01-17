using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class ProductReportModel
    {
        #region
        [Display(Name = "Max Quantity", Prompt = "Quantity less than")]
        #endregion
        public int? MaxQuantity { get; set; }

        #region
        [Display(Name = "Min Quantity", Prompt = "Quantity more than")]
        #endregion
        public int? MinQuantity { get; set; }

        
        public bool? Enabled { get; set; }

        #region
        [Display(Name = "Max Price", Prompt = "Price less than")]
        #endregion
        public decimal? MaxPrice { get; set; }

        #region
        [Display(Name = "Min Price", Prompt = "Price more than")]
        #endregion
        public decimal? MinPrice { get; set; }
        public int? StorageId { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }


    }
}
