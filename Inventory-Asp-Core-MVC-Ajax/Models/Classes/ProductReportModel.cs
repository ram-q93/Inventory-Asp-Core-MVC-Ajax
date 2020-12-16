namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class ProductReportModel
    {
        public int? MaxQuantity { get; set; }
        public int? MinQuantity { get; set; }
        public bool? IsAvailable { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public int? StorageId { get; set; }
        public int? SupplierId { get; set; }
    }
}
