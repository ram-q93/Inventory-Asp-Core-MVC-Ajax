using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Core;
using Inventory_Asp_Core_MVC_Ajax.Core.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryBiz _categoryBiz;

        public CategoryController(ICategoryBiz categoryBiz)
        {
            _categoryBiz = categoryBiz;
        }

        #region Categories

        [HttpGet]
        public IActionResult Categories() => View();


        [HttpPost]
        public async Task<IActionResult> Categories([FromBody] DataTableParameters dtParameters)
            => Json((await _categoryBiz.List(dtParameters)).Data);

        #endregion

        #region AddOrEditCategory

        [HttpGet, ActionName("AddOrEditCategory")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new CategoryModel());
            }
            else
            {
                var result = await _categoryBiz.GetById(id);
                if (!result.Success)
                    return NotFound();
                return View(result.Data);
            }
        }

        [HttpPost, ActionName("AddOrEditCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind] CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(this.HtmlReponse(view: "AddOrEditCategory", model,
                        Result.Failed(Error.WithCode(ErrorCodes.InvalidModel))));
            }
            if (model.Id == 0)
            {
                var result = await _categoryBiz.Add(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditCategory", model, result));
            }
            else
            {
                var result = await _categoryBiz.Edit(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditCategory", model, result));
            }
            return Json(this.HtmlReponse());
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryBiz.Delete(id);
            if (!result.Success)
                return Json(this.HtmlReponse(result: result));
            return Json(this.HtmlReponse());
        }

        #endregion   

        #region IsNameAvailable

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsNameAvailable(string name) =>
            (await _categoryBiz.IsNameInUse(name)).Data ? Json(false) : Json(true);

        #endregion

        #region ListByNameAndId
        [HttpGet, ActionName("list-name")]
        public JsonResult ListByNameAndId()
        {
            var result = _categoryBiz.ListName();
            return Json(result.Data);
        }
        #endregion

        #region Details
        [HttpGet, ActionName("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _categoryBiz.GetById(id);
            return Json(new { description = result.Data.Description ?? "No description" });
        }
        #endregion
    }
}
