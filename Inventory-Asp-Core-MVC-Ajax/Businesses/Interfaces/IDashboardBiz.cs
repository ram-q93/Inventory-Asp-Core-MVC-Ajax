using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IDashboardBiz
    {
        Result<DashboardModel> Statistics();
    }
}
