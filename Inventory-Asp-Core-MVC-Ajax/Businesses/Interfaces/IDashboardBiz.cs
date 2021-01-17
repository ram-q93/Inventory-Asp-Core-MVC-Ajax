using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces
{
    public interface IDashboardBiz
    {
        Task<Result<DashboardModel>> Statistics();
    }
}
