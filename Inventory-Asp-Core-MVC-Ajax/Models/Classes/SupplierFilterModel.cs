using PagedList.Core;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class SupplierFilterModel
    {
        public IPagedList<SupplierModel> SupplierPagedList { get; set; }
        public string SearchQuery { get; set; }
    }
}
