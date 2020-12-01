using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Enums;
using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using InventoryProject.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
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
        public async Task<IActionResult> Storages(string query, int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;
            var storageResults = await storageBiz.List(new PagingModel()
            { PageNumber = pageNumber-1, PageSize = pageSize, Sort = "UpdatedDate", SortDirection = SortDirection.DESC });
            if (!storageResults.Success)
            {
                return View();
            }
            return View(new SearchStorage()
            {
                StorageModels = new StaticPagedList<StorageModel>(storageResults.Items,
                storageResults.PageNumber+1, storageResults.PageSize, (int)storageResults.TotalCount),
                SearchQuery = ""
            });
        }

        #endregion

        #region Search

        [HttpGet, ActionName("Search")]
        public async Task<IActionResult> Search(string PageNumber, [Bind] StorageFilterModel filterModel)
        {
            var storageResults = await storageBiz.Search(filterModel);
            if (!storageResults.Success)
            {
                return View();
            }
            ViewBag.PageSize = storageResults.PageSize;
            ViewBag.CurrentPage = storageResults.PageNumber;
            ViewBag.TotalItemCount = storageResults.TotalCount;
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
        public async Task<IActionResult> AddOrEdit(int id, [Bind] StorageModel model, PagingModel pagingModel)
        {
            if (!ModelState.IsValid)
                return response(false, "AddOrEditStorage", model);
            if (id == 0)
            {
                var result = await storageBiz.Add(model);
                if (!result.Success)
                    return response(false, "AddOrEditStorage", model, result);
            }
            else
            {
                var result = await storageBiz.Edit(model);
                if (!result.Success)
                    return response(false, "AddOrEditStorage", model, result);
            }
            return response(success: true, view: "_Storages", model: (await storageBiz.List(pagingModel)).Data);
        }

        #endregion

        #region Delete

        [HttpPost, ActionName("DeleteStorage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PagingModel pagingModel)
        {
            var result = await storageBiz.Delete(id);
            if (!result.Success)
                return response(false, "_Storages", null, result);
            return response(true, "_Storages", (await storageBiz.List(pagingModel)).Data);
        }

        #endregion

        private IActionResult response(bool success, string view, object model = null, Result result = null) => Json(new
        {
            success,
            error = success ? "" : $"Error {result?.Error?.Code}",
            html = this.RenderRazorViewToString(view, model)
        });

    }
}
