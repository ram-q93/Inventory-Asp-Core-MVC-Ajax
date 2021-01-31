using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class ProductReportModel
    {
        #region
        [Display(Name = "Max Quantity", Prompt = "تعداد کمتر از...")]
        #endregion
        public int? MaxQuantity { get; set; }

        #region
        [Display(Name = "Min Quantity", Prompt = "تعداد بیشتر از...")]
        #endregion
        public int? MinQuantity { get; set; }

        
        public bool? Enabled { get; set; }

        #region
        [Display(Name = "Max Price", Prompt = "قیمت کمتر از...")]
        #endregion
        public decimal? MaxPrice { get; set; }

        #region
        [Display(Name = "Min Price", Prompt = "قیمت بیشتر از...")]
        #endregion
        public decimal? MinPrice { get; set; }
        public int? StorageId { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }


    }
}
