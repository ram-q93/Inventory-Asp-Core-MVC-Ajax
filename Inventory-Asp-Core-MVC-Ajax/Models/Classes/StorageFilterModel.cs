using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Models;

namespace Inventory_Asp_Core_MVC_Ajax.Models.Classes
{
    public class StorageFilterModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public PagingModel PagingModel { get; set; }




        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
