using AspNetCore.Lib.Attributes;
using AspNetCore.Lib.Models;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Common;
using Inventory_Asp_Core_MVC_Ajax.Businesses.Interfaces;
using Inventory_Asp_Core_MVC_Ajax.Models;
using Inventory_Asp_Core_MVC_Ajax.Models.Classes;
using Microsoft.AspNetCore.Mvc;
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
                return Json(this.HtmlReponse("AddOrEditSupplier", model, Result.Failed(Error.WithCode(ErrorCodes.InvalidModel))));
            if (model.Id == 0)
            {
                var result = await supplierBiz.Add(model);
                if (!result.Success)
                    return Json(this.HtmlReponse("AddOrEditSupplier", model, result));
            }
            else
            {
                var result = await supplierBiz.Edit(model);
                if (!result.Success)
                    return Json(this.HtmlReponse("AddOrEditSupplier", model, result));
            }
            return Json(this.HtmlReponse("_Suppliers", await GetSupplierFilterModel()));
        }

        #endregion

        #region Delete

        [HttpPost, ActionName("DeleteSupplier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await supplierBiz.Delete(id);
            if (!result.Success)
                return Json(this.HtmlReponse("_Suppliers", await GetSupplierFilterModel(), result));
            return Json(this.HtmlReponse("_Suppliers", await GetSupplierFilterModel()));
        }

        #endregion

        #region SupplierDetails

        [HttpGet, ActionName("SupplierDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await supplierBiz.Details(id);
            return View(result.Data);
        }

        #endregion

        #region supplier-select-list

        [HttpGet, ActionName("supplier-select-list")]
        public async Task<IActionResult> SupplierSelectList(string criteria)
        {
            var result = await supplierBiz.ListEnableSuppliers();
            return Json(result.Data);
        }

        #endregion

        #region IsNameInUse

        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsNameInUse(string companyName) =>
            (await supplierBiz.IsNameInUse(companyName)).Data ? Json(true) : Json($"Name {companyName} is already in use.");

        #endregion

    }
}
