using Helper.Library.Extensions;
using Helper.Library.Services;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class StorageController : Controller
    {
        public readonly IStorageBiz storageBiz;
        private readonly ILogger logger;

        public StorageController(IStorageBiz storageBiz, ILogger logger)
        {
            this.storageBiz = storageBiz;
            this.logger = logger;
        }


        #region Storages

        [HttpGet, ActionName("Storages")]
        public async Task<IActionResult> Storages()
        {
            try
            {
                var storageResults = await storageBiz.List();
                if (!storageResults.Success)
                    return View();
                return View(storageResults.Data);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        #endregion

        #region AddOrEditStorage

        [HttpGet, ActionName("AddOrEditStorage")]
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
                var result = await storageBiz.Add(model);
                if (!result.Success)
                    return View(model);
                logger.Info($"New Storage Added  {model}");
            }
            else
            {
                var result = await storageBiz.Edit(model);
                if (!result.Success)
                    return View(model);
                logger.Info($"Storage Edited  {model}");
            }
            return Json(new { isValid = true, html = this.RenderRazorViewToString("_Storages", await storageBiz.List()) });
        }

        #endregion

        #region Edit

        [HttpGet, ActionName("EditStorage")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
                var storageResult = await storageBiz.GetById((int)id);
                if (!storageResult.Success)
                    return NotFound();
                return View(model: storageResult.Data);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        [HttpPost, ActionName("EditStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind] StorageModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var result = await storageBiz.Edit(model);
                if (!result.Success)
                    return View(model);
                logger.Info($"Storage Edited  {model}");
                return RedirectToAction("Storages");
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        #endregion

        #region Delete

        [HttpGet, ActionName("DeleteStorage")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
                var storageResult = await storageBiz.GetById((int)id);
                if (!storageResult.Success || storageResult?.Data == null)
                    return NotFound();
                return View(model: storageResult.Data);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        [HttpPost, ActionName("DeleteStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStorage(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
                var result = await storageBiz.Delete((int)id);
                if (!result.Success)
                    return RedirectToAction("DeleteStorage", new { id });
                return RedirectToAction("Storages");
            }
            catch (Exception e)
            {
                logger.Exception(e);
                return NotFound();
            }
        }

        #endregion
    }
}
