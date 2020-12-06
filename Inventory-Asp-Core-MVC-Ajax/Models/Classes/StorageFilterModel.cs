using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class StorageFilterModel
    {
        public IPagedList<StorageModel> StorageModels { get; set; }
        public string SearchQuery { get; set; }
    }
}
