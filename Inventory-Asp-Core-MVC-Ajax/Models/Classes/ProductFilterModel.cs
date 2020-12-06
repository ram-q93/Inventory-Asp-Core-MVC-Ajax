using PagedList.Core;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class ProductFilterModel
    {
        public IPagedList<ProductModel> ProductPagedList { get; set; }
        public string SearchQuery { get; set; }
    }
}
