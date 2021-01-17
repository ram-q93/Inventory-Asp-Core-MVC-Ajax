namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Quantity { get; set; }

        public decimal UnitePrice { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public ImageModel ImageModel { get; set; }

        public StorageModel StorageModel { get; set; }

        public SupplierModel SupplierModel { get; set; }

        public CategoryModel CategoryModel { get; set; }
    }
}
