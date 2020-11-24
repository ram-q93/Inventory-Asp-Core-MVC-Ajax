using Helper.Library.Attributes;
using Helper.Library.Extensions;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class StorageController : Controller
    {
        public readonly IStorageBiz storageBiz;

        public StorageController(IStorageBiz storageBiz)
        {
            this.storageBiz = storageBiz;
        }


        #region Storages

        [HttpGet, ActionName("Storages")]
        public async Task<IActionResult> Storages()
        {
            var storageResults = await storageBiz.List();
            if (!storageResults.Success)
                return View();
            return View(storageResults.Data);
        }

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
                var storageResult = await storageBiz.GetById(id);
                if (!storageResult.Success)
                    return NotFound();
                return View(storageResult.Data);
            }
        }


        [HttpPost, ActionName("AddOrEditStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind] StorageModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = this.RenderRazorViewToString("AddOrEditStorage", model) });
            if (id == 0)
            {
                if (!(await storageBiz.Add(model)).Success)
                    return View(model);
            }
            else
            {
                if (!(await storageBiz.Edit(model)).Success)
                    return View(model);
            }
            return Json(new { isValid = true, html = this.RenderRazorViewToString("_Storages", (await storageBiz.List()).Data) });
        }

        #endregion

        #region Delete

        [HttpPost, ActionName("DeleteStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await storageBiz.Delete(id);
            return Json(new { html = this.RenderRazorViewToString("_Storages", (await storageBiz.List()).Data) });
        }

        #endregion
    }
}
