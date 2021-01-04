using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardBiz _dashboardBiz;

        public DashboardController(IDashboardBiz dashboardBiz)
        {
            _dashboardBiz = dashboardBiz;
        }

        public async Task<IActionResult> Statistics()
            => View((await _dashboardBiz.Statistics()).Data);

    }
}
