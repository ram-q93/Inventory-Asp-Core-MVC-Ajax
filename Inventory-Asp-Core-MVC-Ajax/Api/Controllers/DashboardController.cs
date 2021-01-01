﻿using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardBiz _dashboardBiz;

        public DashboardController(IDashboardBiz dashboardBiz)
        {
            _dashboardBiz = dashboardBiz;
        }

        public IActionResult Statistics() => View(_dashboardBiz.Statistics().Data);

    }
}
