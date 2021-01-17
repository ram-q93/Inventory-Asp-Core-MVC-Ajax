using System.ComponentModel.DataAnnotations;

namespace Inventory_Asp_Core_MVC_Ajax.Core.Classes
{
    public class DashboardModel
    {
        [Display(Name = "Total Storages")]
        public int TotalStorages { get; set; }

        [Display(Name = "Total Suppliers")]
        public int TotalSuppliers { get; set; }
        
        [Display(Name = "Total Products")]
        public int TotalProducts { get; set; }
    }
}
