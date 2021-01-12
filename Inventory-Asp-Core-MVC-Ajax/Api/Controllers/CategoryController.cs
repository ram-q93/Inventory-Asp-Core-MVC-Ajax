using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryBiz _categoryBiz;

        public CategoryController(ICategoryBiz categoryBiz)
        {
            _categoryBiz = categoryBiz;
        }


        [HttpGet, ActionName("list-name")]
        public JsonResult ListByNameAndId()
        {
            var result = _categoryBiz.ListName();
            return Json(result.Data);
        }
    }
}
