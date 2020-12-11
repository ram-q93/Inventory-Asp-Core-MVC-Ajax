using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Extensions;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Threading.Tasks;

namespace Inventory_Asp_Core_MVC_Ajax.Api.Controllers
{
    public class SupplierController : Controller
    {
        public readonly ISupplierBiz supplierBiz;

        public SupplierController(ISupplierBiz supplierBiz)
        {
            this.supplierBiz = supplierBiz;
        }



        #region Suppliers

        [HttpGet, ActionName("Suppliers")]
        public async Task<IActionResult> Suppliers(string query, int? page = null)
            => View(await GetSupplierFilterModel(query, page));

        private async Task<SupplierFilterModel> GetSupplierFilterModel(string query = null, int? page = null)
        {
            var result = await supplierBiz.GetSupplierPagedListFilteredBySearchQuery(page, query);
            return result.Data;
        }

        #endregion

        #region AddOrEditSupplier

        [HttpGet, ActionName("AddOrEditSupplier")]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id)
        {
            if (id == 0)
            {
                return View(new SupplierModel());
            }
            else
            {
                var result = await supplierBiz.GetById(id);
                if (!result.Success)
                    return NotFound();
                return View(result.Data);
            }
        }


        [HttpPost, ActionName("AddOrEditSupplier")]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit([Bind] SupplierModel model)
        {
            if (!ModelState.IsValid)
                return Respo(false, "AddOrEditSupplier", model);
            if (model.Id == 0)
            {
                var result = await supplierBiz.Add(model);
                if (!result.Success)
                    return Respo(false, "AddOrEditSupplier", model, result);
            }
            else
            {
                var result = await supplierBiz.Edit(model);
                if (!result.Success)
                    return Respo(false, "AddOrEditSupplier", model, result);
            }
            return Respo(success: true, view: "_Suppliers", model: await GetSupplierFilterModel());
        }

        #endregion

        #region Delete

        [HttpPost, ActionName("DeleteSupplier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int storageId)
        {
            var result = await supplierBiz.Delete(id);
            if (!result.Success)
                return Respo(false, "_Suppliers", null, result);
            return Respo(true, "_Suppliers", await GetSupplierFilterModel());
        }

        #endregion

        #region ProductDetails

        [HttpGet, ActionName("SupplierDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await supplierBiz.Details(id);
            return View(result.Data);
        }

        #endregion

        private IActionResult Respo(bool success, string view, object model, Result result = null) => Json(new
        {
            success,
            error = success ? "" : $"Error {result?.Error?.Code}",
            html = this.RenderRazorViewToString(view, model)
        });
    }
}
