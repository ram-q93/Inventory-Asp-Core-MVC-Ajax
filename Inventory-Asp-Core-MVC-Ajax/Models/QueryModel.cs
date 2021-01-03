namespace Inventory_Asp_Core_MVC_Ajax.Models
{
    public class QueryModel
    {
        public string Searchby { get; set; }
        public string Sortby { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
