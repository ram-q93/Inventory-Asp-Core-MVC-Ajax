using PagedList.Core;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class StorageFilterModel
    {
        public IPagedList<StorageModel> StorageModels { get; set; }
        public QueryModel QueryModel { get; set; } = new QueryModel();
    }
}
