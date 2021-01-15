using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class StorageController : Controller
    {
        private readonly IStorageBiz _storageBiz;

        public StorageController(IStorageBiz storageBiz)
        {
            _storageBiz = storageBiz;
        }

        #region Storages

        [HttpGet]
        public IActionResult Storages() => View();

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Storages([FromBody] DataTableParameters dtParameters)
            => Json((await _storageBiz.List(dtParameters)).Data);

        #endregion

        #region AddOrEditStorage

        [HttpGet, ActionName("AddOrEditStorage")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new StorageModel());
            }
            else
            {
                var storageResult = await _storageBiz.GetById(id);
                if (!storageResult.Success)
                    return NotFound();
                return View(storageResult.Data);
            }
        }

        [HttpPost, ActionName("AddOrEditStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind] StorageModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(this.HtmlReponse(view: "AddOrEditStorage", model,
                        Result.Failed(Error.WithCode(ErrorCodes.InvalidModel))));
            }
            if (model.Id == 0)
            {
                var result = await _storageBiz.Add(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditStorage", model, result));
            }
            else
            {
                var result = await _storageBiz.Edit(model);
                if (!result.Success)
                    return Json(this.HtmlReponse(view: "AddOrEditStorage", model, result));
            }
            return Json(this.HtmlReponse());
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _storageBiz.Delete(id);
            if (!result.Success)
                return Json(this.HtmlReponse(result: result));
            return Json(this.HtmlReponse());
        }

        #endregion   

        #region IsNameAvailable

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsNameAvailable(string name) =>
            (await _storageBiz.IsNameInUse(name)).Data ? Json(true) : Json(true);

        #endregion

        #region ListByNameAndId
        [HttpGet, ActionName("list-name")]
        public JsonResult ListByNameAndId()
        {
            var result = _storageBiz.ListName();
            return Json(result.Data);
        }
        #endregion

    }


}
