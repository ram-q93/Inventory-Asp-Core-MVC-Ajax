namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public byte[] Data { get; set; }

        public string ConvertedData { get; set; }

        public int? ProductId { get; set; }

        public ProductModel ProductModel { get; set; }
    }
}
